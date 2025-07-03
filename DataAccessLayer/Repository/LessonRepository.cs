using DataAccessLayer.Data;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class LessonRepository : ILessonRepository
    {

        private readonly ApplicationDBContext _context;
        public LessonRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Lesson> AddAsync(Lesson lesson)
        {
            await _context.Lessons.AddAsync(lesson);
            await _context.SaveChangesAsync();
            return lesson;
        }

        public async Task DeleteAsync(int id)
        {
            var lesson=await GetByIdAsync(id);
            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Lesson>> GetByCourseIdAsync(int courseId)
        {
            return await _context.Lessons
                            .Where(l => l.CourseId == courseId)
                            .ToListAsync();
        }

        public async Task<Lesson?> GetByIdAsync(int id)
        {   
            return await _context.Lessons
                            .FirstOrDefaultAsync(l=>l.Id==id);
        }

        public async Task UpdateAsync(Lesson lesson)
        {
            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync();
        }
    }
}
