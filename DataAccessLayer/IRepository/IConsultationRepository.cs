using DataAccessLayer.Dto.Consultation;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IConsultationRepository
    {
        // ConsultationRequest methods
        Task<ConsultationRequest> CreateConsultationRequestAsync(ConsultationRequest request);
        Task<ConsultationRequest?> GetConsultationRequestByIdAsync(int id);
        Task<IEnumerable<ConsultationRequest>> GetConsultationRequestsByUserIdAsync(string userId);
        Task<IEnumerable<ConsultationRequest>> GetConsultationRequestsByConsultantIdAsync(string consultantId);
        Task<IEnumerable<ConsultationRequest>> GetAllConsultationRequestsAsync();
        Task<ConsultationRequest> UpdateConsultationRequestAsync(ConsultationRequest request);
        Task<bool> DeleteConsultationRequestAsync(int id);
        
        // ConsultationSession methods
        Task<ConsultationSession> CreateConsultationSessionAsync(ConsultationSession session);
        Task<ConsultationSession?> GetConsultationSessionByIdAsync(int id);
        Task<ConsultationSession?> GetConsultationSessionByRequestIdAsync(int requestId);
        Task<ConsultationSession> UpdateConsultationSessionAsync(ConsultationSession session);
        
        // ConsultationReview methods
        Task<ConsultationReview> CreateConsultationReviewAsync(ConsultationReview review);
        Task<ConsultationReview?> GetConsultationReviewBySessionIdAsync(int sessionId);
        Task<IEnumerable<ConsultationReview>> GetConsultationReviewsByConsultantIdAsync(string consultantId);
        
        // Search and filter methods
        Task<IEnumerable<ConsultationRequest>> SearchConsultationRequestsAsync(
            string? userId = null, 
            string? consultantId = null, 
            ConsultationStatus? status = null,
            DateTime? fromDate = null,
            DateTime? toDate = null);
    }
} 