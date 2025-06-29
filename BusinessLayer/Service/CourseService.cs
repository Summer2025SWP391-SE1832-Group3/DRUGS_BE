using AutoMapper;
using BusinessLayer.IService;
using DataAccessLayer.Dto.Course;
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

        public CourseService(ICourseRepository courseRepository,IMapper mapper,ICourseEnrollmentRepository courseEnrollmentRepository) {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _courseEnrollmentRepository= courseEnrollmentRepository;
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

        public async Task<IEnumerable<CourseListDto>> GetAllCoursesAsync(string userRole)
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


        //course-enrollment
        public async Task<CourseEnrollment> EnrollInCourseAsync(string userId, int courseId)
        {
            return await _courseEnrollmentRepository.EnrollInCourseAsync(userId, courseId);
        }
        public async Task<IEnumerable<CourseEnrollment>> GetAllEnrollmentsForCourseAsync(int courseId)
        {
            return await _courseEnrollmentRepository.GetEnrollmentsByCourseIdAsync(courseId);
        }

        //lessionprogess - follow progress

    }
}
