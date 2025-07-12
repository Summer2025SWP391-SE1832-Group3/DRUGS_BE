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
        Task<CourseDto> GetCourseByIdAsync(int courseId,string userId,string userRole);
        Task<CourseListDto> GetCourseByCourseId(int courseId);
        Task<IEnumerable<CourseWithEnrollmentStatusDto>> GetAllCoursesAsync(string userId);
        Task<IEnumerable<CourseListDto>> GetCoursesByStatusAsync(CourseStatus? status = null);
        Task<IEnumerable<CourseListDto>> GetCoursesInProgressAsync(string userId);
        Task<IEnumerable<CourseListDto>> GetCompletedCoursesAsync(string userId);
        Task<bool> IsUserEnrolledInCourseAsync(string userId, int courseId);

        Task<IEnumerable<CourseWithEnrollmentStatusDto>> GetCoursesByTopicAsync(CourseTopic topic,   string userId);

        Task<IEnumerable<CourseWithEnrollmentStatusDto>> SearchCourseAsync(string searchTerm,string userId);

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

        //Manager
        Task<CourseDto> CreateCourseAsync(CourseCreateDto courseCreateDto);
        Task<IEnumerable<CourseListDto>> GetAllCoursesForManagerAsync();
        Task<bool> CanApproveCourseAsync(int courseId);
        Task UpdateCourseStatusAsync(int courseId, CourseStatus status);
        Task UpdateCourseAsync(int courseId, CourseUpdateDto courseUpdateDto);
        Task<bool> DeactivateCourseAsync(int courseId);
        Task<IEnumerable<CourseListDto>> SearchCourseForManagerAsync(string searchTerm, CourseStatus? status = null);
        Task<IEnumerable<CourseListDto>> GetCoursesByTopicForManagerAsync(CourseTopic topic, CourseStatus? status = null);



    }
}
