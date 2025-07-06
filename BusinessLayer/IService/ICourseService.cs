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
        Task<CourseListDto> GetCourseByCourseId(int courseId);
        Task<IEnumerable<CourseWithEnrollmentStatusDto>> GetAllCoursesAsync(string userRole, string userId);
        Task<IEnumerable<CourseListDto>> GetAllCoursesForManagerAsync();
        Task<IEnumerable<CourseListDto>> GetActiveCoursesAsync();
        Task<IEnumerable<CourseListDto>> GetDraftCoursesAsync();
        Task<IEnumerable<CourseListDto>> GetInactiveCoursesAsync();
        Task<bool> CanApproveCourseAsync(int courseId);
        Task UpdateCourseStatusAsync(int courseId, CourseStatus status);
        Task<IEnumerable<CourseListDto>> GetCoursesInProgressAsync(string userId);
        Task<IEnumerable<CourseListDto>> GetCompletedCoursesAsync(string userId);
        Task<bool> IsUserEnrolledInCourseAsync(string userId, int courseId);
        Task UpdateCourseAsync(int courseId, CourseUpdateDto courseUpdateDto);
        Task DeleteCourseAsync(int courseId);
        Task<IEnumerable<CourseWithEnrollmentStatusDto>> GetCoursesByTopicAsync(CourseTopic topic, string userRole, string userId);

        Task<IEnumerable<CourseWithEnrollmentStatusDto>> SearchCourseAsync(string searchTerm, string userRole, string userId);

        Task<CourseEnrollment> EnrollInCourseAsync(string userId, int courseId);
        Task<IEnumerable<CourseEnrollmentDto>> GetAllEnrollmentsForCourseAsync(int courseId);

        //updateProgressAsync when complete lesson
        Task<LessonProgressDto> UpdateLessonProgressAsync(string userId, int lessonId, bool isCompleted);
        Task<IEnumerable<LessonProgressDto>> GetLessonProgressForUserAsync(string userId, int courseId);

        //Statis/survey
        Task<CourseReportDto> GetCourseReportAsync(int courseId);
        Task<LessonProgressReportDto> GetLessonProgressReportAsync(int courseId);

        //getdetailCompleteCourse
        Task<CompletedCourseDetailDto> GetCompletedCourseDetailAsync(int courseId, string userId);
        Task<IEnumerable<CourseListDto>> GetCoursesWithoutSurveyOrInactiveSurveyAsync();
    }
}
