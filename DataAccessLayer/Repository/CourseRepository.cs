using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Data;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDBContext _context;
        public CourseRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Course> AddAsync(Course course)
        {
             await _context.Courses.AddAsync(course) ;
             await _context.SaveChangesAsync();
            return course;
        }

        public async Task DeleteAsync(int id)
        {
            var course=await GetByIdAsync(id);
            if (course != null)
            {
                 _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Course>> GetAllAsync()
        {
            return await _context.Courses
                            .Include(c=>c.Lessions)
                            .Include(c=>c.FinalExamSurvey)
                            .ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses
                            .Include(c => c.CourseEnrollments)
                            .Include(c => c.FinalExamSurvey)
                                .ThenInclude(s=>s.SurveyQuestions)
                                    .ThenInclude(sq=>sq.SurveyAnswers)
                            .Include(c=>c.Lessions)
                            .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
             await _context.SaveChangesAsync();
        }

        public async Task<List<Course>> GetByTopicAsync(CourseTopic topic)
        {
            return await _context.Courses
                            .Include(c => c.Lessions)
                            .Include(c => c.FinalExamSurvey)
                            .Where(c=>c.Topic == topic)
                            .ToListAsync();
        }
    }
} 