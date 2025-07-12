using DataAccessLayer.Dto.Course;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.IService
{
    public interface ICourseTestSurveyService
    {
        Task<CourseTestSurveyViewDto> AssignTestToCourseAsync(CourseTestSurveyCreateDto dto);
        Task<CourseTestSurveyViewDto?> GetTestByCourseIdAsync(int courseId);
        Task<CourseTestResultDto> SubmitCourseTestAsync(int courseId, string userId, CourseTestSubmitDto dto);
        Task<List<CourseTestResultDto>> GetCourseTestResultsAsync(int courseId, string userId);
    }
} 