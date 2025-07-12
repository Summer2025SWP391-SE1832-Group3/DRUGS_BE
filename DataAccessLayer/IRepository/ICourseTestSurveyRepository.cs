using DataAccessLayer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface ICourseTestSurveyRepository
    {
        Task<CourseTestSurvey> AddAsync(CourseTestSurvey entity);
        Task<CourseTestSurvey?> GetByCourseIdAsync(int courseId);
        Task<CourseTestSurvey?> GetByIdAsync(int id);
        Task<List<CourseTestSurvey>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
} 