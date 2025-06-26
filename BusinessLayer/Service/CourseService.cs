using System.Collections.Generic;
using System.Linq;
using BusinessLayer.IService;
using DataAccessLayer.Dto.Course;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;

namespace BusinessLayer.Service
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public IEnumerable<CourseViewDto> GetRecommendedCourses(string userId)
        {
            var courses = _courseRepository.GetAllCourses();
            return courses.Select(c => new CourseViewDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Level = c.Level
            });
        }
    }
} 