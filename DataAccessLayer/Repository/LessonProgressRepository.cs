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
    public class LessonProgressRepository : ILessonProgressRepository
    {
        private readonly ApplicationDBContext _context;

        public LessonProgressRepository(ApplicationDBContext context) {
            _context = context;
        }
        public async Task<IEnumerable<LessonProgress>> GetLessonProgressByUserAndCourseAsync(string userId, int courseId)
        {
            return await _context.LessonProgresses
                .Where(c => c.CourseEnrollment.UserId == userId && c.Lesson.CourseId==courseId)
                .Include(c => c.CourseEnrollment)
                .Include(c => c.Lesson)
                .ToListAsync();
        }

        public async Task<LessonProgress> UpdateLessonProgressAsync(string userId, int lessonId, bool isCompleted)
        {
            var lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.Id == lessonId);
            if (lesson == null)
            {
                throw new Exception("Lesson not found");
            }
            var courseEnrollment = await _context.CourseEnrollments
                .FirstOrDefaultAsync(ce => ce.UserId == userId && ce.CourseId == lesson.CourseId);
            if (courseEnrollment == null)
            {
                throw new Exception("User is not enrolled in this course");
            }
            var lessonProgress = await _context.LessonProgresses
                                               .FirstOrDefaultAsync(p => p.CourseEnrollmentId == courseEnrollment.Id && p.LessonId == lessonId);

            if (lessonProgress == null)
            {
                lessonProgress = new LessonProgress
                {
                    CourseEnrollmentId = courseEnrollment.Id, 
                    LessonId = lessonId,
                    IsCompleted = isCompleted,
                    CompletedAt = isCompleted ? DateTime.UtcNow : null
                };

                await _context.LessonProgresses.AddAsync(lessonProgress);
            }
            else
            {
                lessonProgress.IsCompleted = isCompleted;
                lessonProgress.CompletedAt = isCompleted ? DateTime.UtcNow : null;
            }

            await _context.SaveChangesAsync();
            return lessonProgress;
        }
    }
}
