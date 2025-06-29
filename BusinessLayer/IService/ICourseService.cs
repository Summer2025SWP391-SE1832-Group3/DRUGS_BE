using DataAccessLayer.Dto.Course;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IService
{
    public interface ICourseService
    {
        Task<CourseDto> CreateCourseAsync(CourseCreateDto courseCreateDto);
        Task<CourseDto> GetCourseByIdAsync(int courseId);
        Task<IEnumerable<CourseListDto>> GetAllCoursesAsync(string userRole);
        Task UpdateCourseAsync(int courseId, CourseUpdateDto courseUpdateDto);
        Task DeleteCourseAsync(int courseId);
        Task<IEnumerable<CourseListDto>> GetCoursesByTopicAsync(CourseTopic topic, string userRole);

        //Task<IEnumerable<CourseListDto>> SearchCourseAsync(string searchTerm, string userRole);

        Task<CourseEnrollment> EnrollInCourseAsync(string userId, int courseId);
        Task<IEnumerable<CourseEnrollment>> GetAllEnrollmentsForCourseAsync(int courseId);


    }
}
