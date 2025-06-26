using DataAccessLayer.Dto.Consultation;
using DataAccessLayer.Model;

namespace BusinessLayer.IService
{
    public interface IConsultationService
    {
        // ConsultationRequest methods
        Task<ConsultationRequestViewDto> CreateConsultationRequestAsync(ConsultationRequestCreateDto dto, string userId);
        Task<ConsultationRequestViewDto?> GetConsultationRequestByIdAsync(int id, string currentUserId);
        Task<IEnumerable<ConsultationRequestViewDto>> GetMyConsultationRequestsAsync(string userId);
        Task<IEnumerable<ConsultationRequestViewDto>> GetConsultationRequestsForConsultantAsync(string consultantId);
        Task<IEnumerable<ConsultationRequestViewDto>> GetAllConsultationRequestsAsync(string currentUserId);
        Task<ConsultationRequestViewDto> UpdateConsultationRequestStatusAsync(int id, ConsultationRequestUpdateDto dto, string currentUserId);
        Task<bool> DeleteConsultationRequestAsync(int id, string currentUserId);

        // ConsultationSession methods
        Task<ConsultationSessionViewDto> CreateConsultationSessionAsync(int requestId, ConsultationSessionCreateDto dto, string currentUserId);
        Task<ConsultationSessionViewDto?> GetConsultationSessionAsync(int requestId, string currentUserId);
        Task<ConsultationSessionViewDto> UpdateConsultationSessionAsync(int sessionId, ConsultationSessionCreateDto dto, string currentUserId);
        Task<ConsultationSessionViewDto> CompleteConsultationSessionAsync(int sessionId, string currentUserId);

        // ConsultationReview methods
        Task<ConsultationReviewViewDto> CreateConsultationReviewAsync(int requestId, ConsultationReviewCreateDto dto, string currentUserId);
        Task<ConsultationReviewViewDto?> GetConsultationReviewAsync(int requestId, string currentUserId);
        Task<IEnumerable<ConsultationReviewViewDto>> GetConsultantReviewsAsync(string consultantId);

        // Search and filter methods
        Task<IEnumerable<ConsultationRequestViewDto>> SearchConsultationRequestsAsync(
            string currentUserId,
            string? userId = null, 
            string? consultantId = null, 
            ConsultationStatus? status = null,
            DateTime? fromDate = null,
            DateTime? toDate = null);

        // Helper methods
        Task<bool> IsConsultantAsync(string userId);
        Task<bool> CanAccessConsultationRequestAsync(int requestId, string currentUserId);
        Task<bool> IsConsultationRequestOverlappingAsync(string consultantId, DateTime requestedDate, int durationMinutes);
    }
}