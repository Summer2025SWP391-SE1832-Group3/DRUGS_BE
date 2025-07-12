using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace DataAccessLayer.Repository
{
    public class CourseTestSurveyRepository : ICourseTestSurveyRepository
    {
        private readonly ApplicationDBContext _context;
        public CourseTestSurveyRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<CourseTestSurvey> AddAsync(CourseTestSurvey entity)
        {
            _context.CourseTestSurveys.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<CourseTestSurvey?> GetByCourseIdAsync(int courseId)
        {
            return await _context.CourseTestSurveys
                .Include(x => x.Survey)
                .FirstOrDefaultAsync(x => x.CourseId == courseId);
        }

        public async Task<CourseTestSurvey?> GetByIdAsync(int id)
        {
            return await _context.CourseTestSurveys
                .Include(x => x.Survey)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<CourseTestSurvey>> GetAllAsync()
        {
            return await _context.CourseTestSurveys.Include(x => x.Survey).ToListAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.CourseTestSurveys.FindAsync(id);
            if (entity == null) return false;
            _context.CourseTestSurveys.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 