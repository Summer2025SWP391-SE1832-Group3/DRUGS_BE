﻿    using AutoMapper;
using BusinessLayer.IService;
using CloudinaryDotNet.Actions;
using DataAccessLayer.Dto.Course;
using DataAccessLayer.Dto.Lesson;
using DataAccessLayer.Dto.Survey;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using DataAccessLayer.Repository;
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

        public CourseService(ICourseRepository courseRepository,IMapper mapper,ICourseEnrollmentRepository courseEnrollmentRepository,
            ILessonProgressRepository lessonProgressRepository,ICourseReportRepository courseReportRepository) {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _courseEnrollmentRepository= courseEnrollmentRepository;
            _lessonProgressRepository = lessonProgressRepository;
            _courseReportRepository = courseReportRepository;
        }

        public async Task<CourseDto> CreateCourseAsync(CourseCreateDto courseCreateDto)
        {
            var course=_mapper.Map<Course>(courseCreateDto);
            course.IsActive = true;
            course.CreatedAt = DateTime.Now;
            var createdCourse=await _courseRepository.AddAsync(course);
            return _mapper.Map<CourseDto>(createdCourse);
        }

        public async Task DeleteCourseAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course != null)
            {
                course.IsActive = false; 
                await _courseRepository.UpdateAsync(course);  
            }
        }

        public async Task<IEnumerable<CourseListDto>> GetAllCoursesAsync(string? userRole)
        {
            var courses=await _courseRepository.GetAllAsync();
            if (userRole == "Manager")
            {
                 return _mapper.Map<IEnumerable<CourseListDto>>(courses);
            }
            else
            {
                var activeCourse = courses.Where(c => c.IsActive).ToList();
                return _mapper.Map<IEnumerable<CourseListDto>>(activeCourse);

            }
        }

        public async Task<CourseDto> GetCourseByCourseId(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null) return null;
            else return _mapper.Map<CourseDto>(course);
        }


        public async Task<CourseDto> GetCourseByIdAsync(int courseId, string userId, string userRole)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null) return null;
            if (userRole == "Member")
            {
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
                return new CourseDto
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    Topic = course.Topic,
                    Lessions = lessonsDto,
                    FinalExamSurvey = _mapper.Map<SurveyViewDto>(finalExamSurvey)
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
                        IsCompleted = true 
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
                    FinalExamSurvey = _mapper.Map<SurveyViewDto>(finalExamSurvey)
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

            return _mapper.Map<IEnumerable<CourseListDto>>(inProgressCourses);
        }

        public async Task<IEnumerable<CourseListDto>> GetCompletedCoursesAsync(string userId)
        {
            var enrollments = await _courseEnrollmentRepository.GetEnrollmentsByUserIdAsync(userId);

            var completedCourses = enrollments.Where(e => e.IsCompleted)
                                              .Select(e => e.Course)
                                              .ToList();

            return _mapper.Map<IEnumerable<CourseListDto>>(completedCourses);
        }
        public async Task<IEnumerable<CourseListDto>> GetCoursesByTopicAsync(CourseTopic topic, string userRole)
        {
            var courses=await _courseRepository.GetByTopicAsync(topic);
            if (userRole == "Manager")
            {
                return _mapper.Map<IEnumerable<CourseListDto>>(courses);

            }
            var filteredCourses = courses.Where(c => c.IsActive).ToList();
            return _mapper.Map<IEnumerable<CourseListDto>>(filteredCourses);
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
        public async Task<IEnumerable<CourseListDto>> SearchCourseAsync(string searchTerm, string userRole)
        {
            var courses = await _courseRepository.SearchCoursesAsync(searchTerm);

            if (userRole != "Manager")
            {
                courses = courses.Where(c => c.IsActive).ToList();
            }

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
            var report = await _courseReportRepository.GetCourseReportAsync(courseId);
            return report;
        }

        public async Task<LessonProgressReportDto> GetLessonProgressReportAsync(int courseId)
        {
            var report = await _courseReportRepository.GetLessonProgressReportAsync(courseId);
            return report;
        }

    }
}
