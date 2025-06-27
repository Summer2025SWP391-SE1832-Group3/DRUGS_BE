using DataAccessLayer.Data;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class ConsultationRepository : IConsultationRepository
    {
        private readonly ApplicationDBContext _context;

        public ConsultationRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        // ConsultationRequest methods
        public async Task<ConsultationRequest> CreateConsultationRequestAsync(ConsultationRequest request)
        {
            _context.ConsultationRequests.Add(request);
            await _context.SaveChangesAsync();
            
            // Reload with navigation properties
            return await _context.ConsultationRequests
                .Include(cr => cr.User)
                .Include(cr => cr.Consultant)
                .Include(cr => cr.ConsultationSession)
                .Include(cr => cr.ConsultantWorkingHour)
                .FirstOrDefaultAsync(cr => cr.Id == request.Id);
        }

        public async Task<ConsultationRequest?> GetConsultationRequestByIdAsync(int id)
        {
            return await _context.ConsultationRequests
                .Include(cr => cr.User)
                .Include(cr => cr.Consultant)
                .Include(cr => cr.ConsultationSession)
                .Include(cr => cr.ConsultantWorkingHour)
                .FirstOrDefaultAsync(cr => cr.Id == id);
        }

        public async Task<IEnumerable<ConsultationRequest>> GetConsultationRequestsByUserIdAsync(string userId)
        {
            return await _context.ConsultationRequests
                .Include(cr => cr.Consultant)
                .Include(cr => cr.ConsultationSession)
                .Include(cr => cr.ConsultantWorkingHour)
                .Where(cr => cr.UserId == userId)
                .OrderByDescending(cr => cr.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ConsultationRequest>> GetConsultationRequestsByConsultantIdAsync(string consultantId)
        {
            return await _context.ConsultationRequests
                .Include(cr => cr.User)
                .Include(cr => cr.ConsultationSession)
                .Include(cr => cr.ConsultantWorkingHour)
                .Where(cr => cr.ConsultantId == consultantId)
                .OrderByDescending(cr => cr.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ConsultationRequest>> GetAllConsultationRequestsAsync()
        {
            return await _context.ConsultationRequests
                .Include(cr => cr.User)
                .Include(cr => cr.Consultant)
                .Include(cr => cr.ConsultationSession)
                .Include(cr => cr.ConsultantWorkingHour)
                .OrderByDescending(cr => cr.CreatedAt)
                .ToListAsync();
        }

        public async Task<ConsultationRequest> UpdateConsultationRequestAsync(ConsultationRequest request)
        {
            request.UpdatedAt = DateTime.Now;
            _context.ConsultationRequests.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<bool> DeleteConsultationRequestAsync(int id)
        {
            var request = await _context.ConsultationRequests.FindAsync(id);
            if (request == null) return false;
            
            _context.ConsultationRequests.Remove(request);
            await _context.SaveChangesAsync();
            return true;
        }

        // ConsultationSession methods
        public async Task<ConsultationSession> CreateConsultationSessionAsync(ConsultationSession session)
        {
            _context.ConsultationSessions.Add(session);
            await _context.SaveChangesAsync();
            
            // Reload with navigation properties
            return await _context.ConsultationSessions
                .Include(cs => cs.ConsultationRequest)
                .FirstOrDefaultAsync(cs => cs.Id == session.Id);
        }

        public async Task<ConsultationSession?> GetConsultationSessionByIdAsync(int id)
        {
            return await _context.ConsultationSessions
                .Include(cs => cs.ConsultationRequest)
                .FirstOrDefaultAsync(cs => cs.Id == id);
        }

        public async Task<ConsultationSession?> GetConsultationSessionByRequestIdAsync(int requestId)
        {
            return await _context.ConsultationSessions
                .Include(cs => cs.ConsultationRequest)
                .FirstOrDefaultAsync(cs => cs.ConsultationRequestId == requestId);
        }

        public async Task<ConsultationSession> UpdateConsultationSessionAsync(ConsultationSession session)
        {
            session.UpdatedAt = DateTime.Now;
            _context.ConsultationSessions.Update(session);
            await _context.SaveChangesAsync();
            return session;
        }

        // ConsultationReview methods
        public async Task<ConsultationReview> CreateConsultationReviewAsync(ConsultationReview review)
        {
            _context.ConsultationReviews.Add(review);
            await _context.SaveChangesAsync();
            // Reload with navigation properties
            return await _context.ConsultationReviews
                .Include(cr => cr.ConsultationSession)
                .FirstOrDefaultAsync(cr => cr.Id == review.Id);
        }

        public async Task<ConsultationReview?> GetConsultationReviewBySessionIdAsync(int sessionId)
        {
            return await _context.ConsultationReviews
                .Include(cr => cr.ConsultationSession)
                .FirstOrDefaultAsync(cr => cr.ConsultationSessionId == sessionId);
        }

        public async Task<IEnumerable<ConsultationReview>> GetConsultationReviewsByConsultantIdAsync(string consultantId)
        {
            return await _context.ConsultationReviews
                .Include(cr => cr.ConsultationSession)
                .ThenInclude(cs => cs.ConsultationRequest)
                .Where(cr => cr.ConsultationSession.ConsultationRequest.ConsultantId == consultantId)
                .OrderByDescending(cr => cr.CreatedAt)
                .ToListAsync();
        }

        // Search and filter methods
        public async Task<IEnumerable<ConsultationRequest>> SearchConsultationRequestsAsync(
            string? userId = null, 
            string? consultantId = null, 
            ConsultationStatus? status = null,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var query = _context.ConsultationRequests
                .Include(cr => cr.User)
                .Include(cr => cr.Consultant)
                .Include(cr => cr.ConsultationSession)
                .Include(cr => cr.ConsultantWorkingHour)
                .AsQueryable();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(cr => cr.UserId == userId);

            if (!string.IsNullOrEmpty(consultantId))
                query = query.Where(cr => cr.ConsultantId == consultantId);

            if (status.HasValue)
                query = query.Where(cr => cr.Status == status.Value);

            if (fromDate.HasValue)
                query = query.Where(cr => cr.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(cr => cr.CreatedAt <= toDate.Value);

            return await query.OrderByDescending(cr => cr.CreatedAt).ToListAsync();
        }
    }
}