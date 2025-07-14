    using AutoMapper;
using BusinessLayer.IService;
using CloudinaryDotNet.Actions;
using DataAccessLayer.Dto.Course;
using DataAccessLayer.Dto.Lesson;
using DataAccessLayer.Dto.Survey;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly ICourseEnrollmentRepository _courseEnrollmentRepository;
        private readonly ILessonProgressRepository _lessonProgressRepository;
        private readonly ICourseReportRepository _courseReportRepository;
        private readonly ISurveyService _surveyService;
        private readonly IFeedbackService _feedbackService;

        public CourseService(ICourseRepository courseRepository,IMapper mapper,ICourseEnrollmentRepository courseEnrollmentRepository,
            ILessonProgressRepository lessonProgressRepository,ICourseReportRepository courseReportRepository,ISurveyService surveyService,IFeedbackService feedbackService) {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _courseEnrollmentRepository= courseEnrollmentRepository;
            _lessonProgressRepository = lessonProgressRepository;
            _courseReportRepository = courseReportRepository;
            _surveyService = surveyService;
            _feedbackService = feedbackService;
        }

        public async Task<CourseDto> CreateCourseAsync(CourseCreateDto courseCreateDto)
        {
            var course=_mapper.Map<Course>(courseCreateDto);
            course.Status = CourseStatus.Draft; 
            course.CreatedAt = DateTime.Now;
            var createdCourse=await _courseRepository.AddAsync(course);
            return _mapper.Map<CourseDto>(createdCourse);
        }

        public async Task<bool> DeactivateCourseAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
            {
                return false;
            }

            var enrollments = await _courseEnrollmentRepository.GetEnrollmentsByCourseIdAsync(courseId);
            if (enrollments.Any(e => e.Status == EnrollmentStatus.InProgress))
            {
                return false; 
            }
            course.Status = CourseStatus.Inactive;
            course.UpdatedAt = DateTime.Now;  
            await _courseRepository.UpdateAsync(course);  
            return true;
        }


        public async Task<IEnumerable<CourseWithEnrollmentStatusDto>> GetAllCoursesAsync(string userId)
        {
            var courses=await _courseRepository.GetAllAsync();
            courses = courses.Where(c => c.Status==CourseStatus.Active).ToList();
            var result = new List<CourseWithEnrollmentStatusDto>();

            foreach (var course in courses)
            {
                var status = await _courseEnrollmentRepository.GetEnrollmentStatusAsync(userId, course.Id);
                result.Add(new CourseWithEnrollmentStatusDto
                {
                    Course = _mapper.Map<CourseListDto>(course),
                    Status = status,
                });
            }

            return result;
        }
        public async Task<IEnumerable<CourseListDto>> GetAllCoursesForManagerAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            return _mapper.Map<List<CourseListDto>>(courses);
        }
        public async Task<IEnumerable<CourseListDto>> GetCoursesByStatusAsync(CourseStatus? status = null)
        {
            var courses = await _courseRepository.GetAllAsync();
            if (status.HasValue)
            {
                courses = courses.Where(c => c.Status == status.Value).ToList();
            }
            return _mapper.Map<IEnumerable<CourseListDto>>(courses);
        }


        public async Task<bool> CanApproveCourseAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null) return false;
            if (course.Lessions == null || !course.Lessions.Any()) return false;
            if (course.FinalExamSurveyId == null || !await _surveyService.IsSurveyActiveAsync(course.FinalExamSurveyId.Value)) return false;
            return true;
        }
        public async Task UpdateCourseStatusAsync(int courseId, CourseStatus status)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course != null)
            {
                course.Status = status;
                course.UpdatedAt = DateTime.Now;
                await _courseRepository.UpdateAsync(course);
            }
        }

        public async Task<CourseListDto> GetCourseByCourseId(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null) return null;
            else return _mapper.Map<CourseListDto>(course);
        }


        public async Task<CourseDto> GetCourseByIdAsync(int courseId, string userId, string userRole)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null) return null;
            if (userRole == "Member" && course.Status==CourseStatus.Inactive)
                return null;
            var enrollment = await _courseEnrollmentRepository.GetEnrollmentByUserIdAndCourseIdAsync(userId, courseId);
            var isCourseCompleted = enrollment != null && enrollment.IsCompleted;

            if (userRole == "Member")
            {
                if (enrollment == null)
                {
                    return new CourseDto
                    {
                        Id = course.Id,
                        Title = course.Title,
                        Description = course.Description,
                        Topic = course.Topic,
                        IsCompleted = false,
                        Message = "You are not enrolled in this course. Please enroll to view details."
                    };
                }
                var progress = await _lessonProgressRepository.GetLessonProgressByUserAndCourseAsync(userId, courseId);
                var lessonsDto = course.Lessions.Select(lesson =>
                {
                    var lessonProgress = progress.FirstOrDefault(p => p.LessonId == lesson.Id);
                    return new LessonDto
                    {
                        Id = lesson.Id,
                        Title = lesson.Title,
                        Content = lesson.Content,
                        VideoUrl = lesson.VideoUrl,
                        IsCompleted = lessonProgress != null && lessonProgress.IsCompleted
                    };
                }).ToList();

                var finalExamSurvey = course.FinalExamSurvey;

                if (isCourseCompleted)
                {
                    return new CourseDto
                    {
                        Id = course.Id,
                        Title = course.Title,
                        Description = course.Description,
                        Topic = course.Topic,
                        Lessions = lessonsDto,
                        FinalExamSurvey = _mapper.Map<SurveyViewDto>(finalExamSurvey),
                        IsCompleted = true,
                        Message = "You have passed this course.",
                    };
                }

                return new CourseDto
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    Topic = course.Topic,
                    Lessions = lessonsDto,
                    FinalExamSurvey = _mapper.Map<SurveyViewDto>(finalExamSurvey),
                    IsCompleted = false,
                    Message = "You are enrolled in this course and can continue learning."
                };
            }

            else if (userRole == "Manager" || userRole == "Staff")
            {
                var lessonsDto = course.Lessions.Select(lesson =>
                {
                    return new LessonDto
                    {
                        Id = lesson.Id,
                        Title = lesson.Title,
                        Content = lesson.Content,
                        VideoUrl = lesson.VideoUrl,
                        IsCompleted = true,
                    };
                }).ToList();
                var finalExamSurvey = course.FinalExamSurvey;
                return new CourseDto
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    Topic = course.Topic,
                    Lessions = lessonsDto,
                    FinalExamSurvey = _mapper.Map<SurveyViewDto>(finalExamSurvey),
                    IsCompleted = isCourseCompleted,
                    Message = "Viewing course details"
                };
            }
            return null;
        }

        public async Task<IEnumerable<CourseListDto>> GetCoursesInProgressAsync(string userId)
        {
            var enrollments = await _courseEnrollmentRepository.GetEnrollmentsByUserIdAsync(userId);

            var inProgressCourses = enrollments.Where(e => !e.IsCompleted)
                                               .Select(e => e.Course)
                                               .ToList();

            var courseDtos= _mapper.Map<IEnumerable<CourseListDto>>(inProgressCourses);
            foreach (var course in courseDtos)
            {
                if (course.Status==CourseStatus.Inactive)
                {
                    course.StatusMessage = "This course is no longer active. You can only view the content.";
                }
                else
                {
                    course.StatusMessage = "You can continue your course.";
                }
            }

            return courseDtos;
        }

        public async Task<IEnumerable<CourseWithEnrollmentStatusDto>> GetCoursesByTopicAsync(CourseTopic topic, string userId)
        {
            var courses=await _courseRepository.GetByTopicAsync(topic);
            courses = courses.Where(c => c.Status == CourseStatus.Active).ToList();
            var result = new List<CourseWithEnrollmentStatusDto>();

            foreach (var course in courses)
            {
                var status = await _courseEnrollmentRepository.GetEnrollmentStatusAsync(userId, course.Id);
                result.Add(new CourseWithEnrollmentStatusDto
                {
                    Course = _mapper.Map<CourseListDto>(course),
                    Status = status
                });
            }

            return result;
        }
        public async Task<IEnumerable<CourseListDto>> GetCoursesByTopicForManagerAsync(CourseTopic topic, CourseStatus? status = null)
        {
            var courses = await _courseRepository.GetByTopicAsync(topic);
            if (status.HasValue)
                courses = courses.Where(c => c.Status == status.Value).ToList();
            return _mapper.Map<IEnumerable<CourseListDto>>(courses);
        }

        public async Task UpdateCourseAsync(int courseId, CourseUpdateDto courseUpdateDto)
        {
            var existCourse=await _courseRepository.GetByIdAsync(courseId);
            if (existCourse != null)
            {
                _mapper.Map(courseUpdateDto, existCourse);
                existCourse.UpdatedAt = DateTime.Now;
                await _courseRepository.UpdateAsync(existCourse);
            }
        }
        public async Task<IEnumerable<CourseWithEnrollmentStatusDto>> SearchCourseAsync(string searchTerm,string userId)
        {
            var courses = await _courseRepository.SearchCoursesAsync(searchTerm);
            courses = courses.Where(c => c.Status == CourseStatus.Active).ToList();
            var result = new List<CourseWithEnrollmentStatusDto>();

            foreach (var course in courses)
            {
                var status = await _courseEnrollmentRepository.GetEnrollmentStatusAsync(userId, course.Id);
                result.Add(new CourseWithEnrollmentStatusDto
                {
                    Course = _mapper.Map<CourseListDto>(course),
                    Status = status
                });
            }

            return result;
        }
        public async Task<IEnumerable<CourseListDto>> SearchCourseForManagerAsync(string searchTerm, CourseStatus? status = null)
        {
            var courses = await _courseRepository.SearchCoursesAsync(searchTerm);
            if (status.HasValue)
                courses = courses.Where(c => c.Status == status.Value).ToList();
            return _mapper.Map<IEnumerable<CourseListDto>>(courses);
        }

        //course-enrollment
        public async Task<CourseEnrollment> EnrollInCourseAsync(string userId, int courseId)
        {
            return await _courseEnrollmentRepository.EnrollInCourseAsync(userId, courseId);
        }
        public async Task<IEnumerable<CourseEnrollmentDto>> GetAllEnrollmentsForCourseAsync(int courseId)
        {
            var courseEnrollments = await _courseEnrollmentRepository.GetEnrollmentsByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<CourseEnrollmentDto>>(courseEnrollments);
        }
        public async Task<bool> IsUserEnrolledInCourseAsync(string userId, int courseId)
        {
            var enrollment = await _courseEnrollmentRepository.GetEnrollmentByUserIdAndCourseIdAsync(userId, courseId);
            return enrollment != null;
        }

        //lessionprogess
        public async Task<LessonProgressDto> UpdateLessonProgressAsync(string userId, int lessonId, bool isCompleted)
        {
            var progress = await _lessonProgressRepository.UpdateLessonProgressAsync(userId, lessonId, isCompleted);
            return _mapper.Map<LessonProgressDto>(progress);
        }

        public async Task<IEnumerable<LessonProgressDto>> GetLessonProgressForUserAsync(string userId, int courseId)
        {
            var progress = await _lessonProgressRepository.GetLessonProgressByUserAndCourseAsync(userId, courseId);
            return _mapper.Map<IEnumerable<LessonProgressDto>>(progress);
        }

        //statis
        public async Task<CourseReportDto> GetCourseReportAsync(int courseId)
        {
            var enrollments = await _courseReportRepository.GetEnrollmentsByCourseIdAsync(courseId);

            var completedCount = enrollments.Count(e => e.IsCompleted);
            var rating = await _feedbackService.GetAverageRatingAsync(courseId);
            var feedbacks = await _feedbackService.GetFeedbacksByCourseIdAsync(courseId);
            var totalFeedbacks = feedbacks.Count();
            var feedbackDistribution = new Dictionary<int, int>();
            for (int i = 1; i <= 5; i++)
            {
                feedbackDistribution[i] = feedbacks.Count(f => f.Rating == i);
            }

            var report = new CourseReportDto
            {
                CourseId = courseId,
                TotalEnrollments = enrollments.Count(),
                CompletedCount = completedCount,
                PendingCount = enrollments.Count() - completedCount,
                AverageRating = rating.AverageRating,
                TotalFeedbacks = totalFeedbacks,
                FeedbackDistribution = feedbackDistribution
            };

            return report;
        }

        public async Task<LessonProgressReportDto> GetLessonProgressReportAsync(int courseId)
        {
            var report = await _courseReportRepository.GetLessonProgressReportAsync(courseId);
            return report;
        }

        public async Task<bool> IsCourseCompletedAsync(string userId, int courseId)
        {
            var enrollment = await _courseEnrollmentRepository.GetEnrollmentByUserIdAndCourseIdAsync(userId, courseId);
            return enrollment != null && enrollment.IsCompleted;
        }


        public async Task<IEnumerable<CourseListDto>> GetCompletedCoursesAsync(string userId)
        {
            var enrollments = await _courseEnrollmentRepository.GetEnrollmentsByUserIdAsync(userId);

            var completedCourses = enrollments.Where(e => e.IsCompleted)
                                              .Select(e => e.Course)
                                              .ToList();

            var courseDtos= _mapper.Map<IEnumerable<CourseListDto>>(completedCourses);
            foreach (var course in courseDtos)
            {
                if (course.Status==CourseStatus.Inactive)
                {
                    course.StatusMessage = "This course is no longer active.";
                }
                else
                {
                    course.StatusMessage = "You have completed this course.";
                }
            }

            return courseDtos;
        }

        public async Task<CompletedCourseDetailDto> GetCompletedCourseDetailAsync(int courseId, string userId)
        {


            var enrollment = await _courseEnrollmentRepository.GetEnrollmentByUserIdAndCourseIdAsync(userId, courseId);

            if (enrollment == null || !enrollment.IsCompleted)
            {
                return null;
            }

            var completedLessonIds = await _lessonProgressRepository
                                          .GetCompletedLessonIdsByUserAndCourseAsync(userId, courseId);
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null) return null;
            //var lessonsDto = course.Lessions.Where(lesson => lesson.IsActive || (completedLessonIds.Contains(lesson.Id) && !lesson.IsActive))
            //                                .Select(lesson => new LessonDto
            //{
            var lessonsDto = course.Lessions.Select(lesson => new LessonDto
                                           {
                Id = lesson.Id,
                Title = lesson.Title,
                Content = lesson.Content,
                VideoUrl = lesson.VideoUrl,
                IsCompleted = completedLessonIds.Contains(lesson.Id),
            }).ToList();

            var lastSurveyResult = await _surveyService.GetUserSurveyResultNewestAsync(course.FinalExamSurveyId.Value, userId);
            var totalQuestions = lastSurveyResult.Questions.Count;
            var finalScore= (lastSurveyResult.TotalScore / (double)totalQuestions) * 100;
            return new CompletedCourseDetailDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Topic = course.Topic,
                CompletedDate = enrollment.CompletedAt,
                FinalScore = finalScore,
                Lessons = lessonsDto,
                FinalSurveyResult = lastSurveyResult
            };
        }
        public async Task<IEnumerable<CourseListDto>> GetCoursesWithoutSurveyOrInactiveSurveyAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            var coursesWithoutSurveyOrInactiveSurvey = courses.Where(c =>c.FinalExamSurvey == null || (c.FinalExamSurvey != null && !c.FinalExamSurvey.IsActive)
            ).ToList();

            return _mapper.Map<IEnumerable<CourseListDto>>(coursesWithoutSurveyOrInactiveSurvey);
        }

    }
}
