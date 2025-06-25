using System.Collections.Generic;
using DataAccessLayer.Dto.Course;

namespace BusinessLayer.IService
{
    public interface ICourseService
    {
        IEnumerable<CourseViewDto> GetRecommendedCourses(string userId);
    }
} 