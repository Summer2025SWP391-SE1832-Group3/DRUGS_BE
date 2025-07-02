using DataAccessLayer.Dto.Course;
using DataAccessLayer.Dto.Lesson;
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
        Task<CourseDto> GetCourseByIdAsync(int courseId,string userId,string userRole);
        Task<CourseDto> GetCourseByCourseId(int courseId);
        Task<IEnumerable<CourseListDto>> GetAllCoursesAsync(string userRole);
        Task<IEnumerable<CourseListDto>> GetCoursesInProgressAsync(string userId);
        Task<IEnumerable<CourseListDto>> GetCompletedCoursesAsync(string userId);
        Task<bool> IsUserEnrolledInCourseAsync(string userId, int courseId);
        Task UpdateCourseAsync(int courseId, CourseUpdateDto courseUpdateDto);
        Task DeleteCourseAsync(int courseId);
        Task<IEnumerable<CourseListDto>> GetCoursesByTopicAsync(CourseTopic topic, string userRole);

        Task<IEnumerable<CourseListDto>> SearchCourseAsync(string searchTerm, string userRole);

        Task<CourseEnrollment> EnrollInCourseAsync(string userId, int courseId);
        Task<IEnumerable<CourseEnrollmentDto>> GetAllEnrollmentsForCourseAsync(int courseId);

        //updateProgressAsync when complete lesson
        Task<LessonProgressDto> UpdateLessonProgressAsync(string userId, int lessonId, bool isCompleted);
        Task<IEnumerable<LessonProgressDto>> GetLessonProgressForUserAsync(string userId, int courseId);

        //Statis/survey
        Task<CourseReportDto> GetCourseReportAsync(int courseId);
        Task<LessonProgressReportDto> GetLessonProgressReportAsync(int courseId);
    }
}
