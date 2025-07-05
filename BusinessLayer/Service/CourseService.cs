using AutoMapper;
using BusinessLayer.IService;
using BusinessLayer.Dto.Common;
using DataAccessLayer.Dto.Course;
using DataAccessLayer.Dto.Lesson;
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
     
        public async Task<CourseDto> GetCourseByIdAsync(int courseId)
        {
            var course=await _courseRepository.GetByIdAsync(courseId);
            if (course == null) return null;
            return _mapper.Map<CourseDto>(course);
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

        // Pagination methods
        public async Task<PaginatedResult<CourseListDto>> GetPaginatedCoursesAsync(string userRole, int page, int pageSize, string? searchTerm = null, CourseTopic? topic = null)
        {
            var courses = await _courseRepository.GetAllAsync();
            var query = courses.AsQueryable();

            // Apply role filter
            if (userRole != "Manager")
            {
                query = query.Where(c => c.IsActive);
            }

            // Apply topic filter
            if (topic.HasValue)
            {
                query = query.Where(c => c.Topic == topic.Value);
            }

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => 
                    c.Title.Contains(searchTerm) || 
                    c.Description.Contains(searchTerm));
            }

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            page = Math.Max(1, Math.Min(page, totalPages));

            var paginatedCourses = query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = _mapper.Map<List<CourseListDto>>(paginatedCourses);

            return new PaginatedResult<CourseListDto>
            {
                Items = result,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        public async Task<PaginatedResult<CourseEnrollmentDto>> GetPaginatedEnrollmentsForCourseAsync(int courseId, int page, int pageSize)
        {
            var enrollments = await _courseEnrollmentRepository.GetEnrollmentsByCourseIdAsync(courseId);
            var query = enrollments.AsQueryable();

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            page = Math.Max(1, Math.Min(page, totalPages));

            var paginatedEnrollments = query
                .OrderByDescending(e => e.EnrolledAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = _mapper.Map<List<CourseEnrollmentDto>>(paginatedEnrollments);

            return new PaginatedResult<CourseEnrollmentDto>
            {
                Items = result,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            };
        }
    }
}
