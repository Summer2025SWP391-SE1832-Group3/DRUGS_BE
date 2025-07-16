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
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly ApplicationDBContext _context;

        public  FeedbackRepository(ApplicationDBContext context) 
        {
            _context = context;
        }

        public async Task<Feedback> CreateAsync(Feedback feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task<bool> DeleteAsync(int feedbackId)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedback == null) return false;

            _context.Feedbacks.Remove(feedback);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Feedback> GetFeedbackByCourseAndUserAsync(int courseId, string userId)
        {
            return await _context.Feedbacks
           .Where(f => f.CourseId == courseId && f.UserId == userId)
           .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByCourseIdAsync(int courseId)
        {
            return await _context.Feedbacks
                            .Include(f=>f.User)
                            .Include(f=>f.Course)
                            .Where(f => f.CourseId == courseId && f.IsActive)   
                            .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<Feedback> GetFeedbackByIdAsync(int feedbackId)
        {
            return await _context.Feedbacks
                .Where(f => f.FeedbackId == feedbackId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByConsultantIdAsync(string consultantId)
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Consultant)
                .Where(f => f.ConsultantId == consultantId && f.IsActive)
                .ToListAsync();
        }

        public async Task<Feedback> GetFeedbackByConsultantAndUserAsync(string consultantId, string userId)
        {
            return await _context.Feedbacks
                .Where(f => f.ConsultantId == consultantId && f.UserId == userId)
                .FirstOrDefaultAsync();
        }
    }
}
