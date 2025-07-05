using System.Collections.Generic;
using DataAccessLayer.Model;

namespace DataAccessLayer.IRepository
{
    public interface ICourseRepository
    {
        Task<Course> GetByIdAsync(int id);
        Task<List<Course>> GetAllAsync();
        Task<Course> AddAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(int id);
        Task<List<Course>> GetByTopicAsync(CourseTopic topic);
        Task<List<Course>> SearchCoursesAsync(string searchTerm);
    }
} 