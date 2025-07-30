using DataAccessLayer.Dto.Consultation;
using DataAccessLayer.Model;
using BusinessLayer.Dto.Common;

namespace BusinessLayer.IService
{
    public interface IConsultationService
    {
        // Consultation Request methods
        Task<ConsultationRequestViewDto> CreateConsultationRequestAsync(ConsultationRequestCreateDto dto, string userId);
        Task<ConsultationRequestViewDto?> GetConsultationRequestByIdAsync(int id, string currentUserId);
        Task<IEnumerable<ConsultationRequestViewDto>> GetMyConsultationRequestsAsync(string userId);
        Task<IEnumerable<ConsultationRequestViewDto>> GetConsultationRequestsForConsultantAsync(string consultantId);
        Task<IEnumerable<ConsultationRequestViewDto>> GetAllConsultationRequestsAsync(string currentUserId);
        Task<ConsultationRequestViewDto> UpdateConsultationRequestStatusAsync(int id, ConsultationRequestUpdateDto dto, string currentUserId);
        Task<bool> DeleteConsultationRequestAsync(int id, string currentUserId);
        
        // Consultation Session methods
        Task<ConsultationSessionViewDto> CreateConsultationSessionAsync(int requestId, ConsultationSessionCreateDto dto, string currentUserId);
        Task<ConsultationSessionViewDto?> GetConsultationSessionAsync(int requestId, string currentUserId);
        Task<ConsultationSessionViewDto> UpdateConsultationSessionAsync(int sessionId, ConsultationSessionCreateDto dto, string currentUserId);
        Task<ConsultationSessionViewDto> CompleteConsultationSessionAsync(int sessionId, string currentUserId);
        Task<bool> DeleteConsultationSessionAsync(int sessionId, string currentUserId);
        
        // Consultation Review methods
        Task<ConsultationReviewViewDto> CreateConsultationReviewAsync(int sessionId, ConsultationReviewCreateDto dto, string currentUserId);
        Task<ConsultationReviewViewDto?> GetConsultationReviewAsync(int sessionId, string currentUserId);
        Task<IEnumerable<ConsultationReviewViewDto>> GetConsultantReviewsAsync(string consultantId);
        
        // Booking and Slot methods
        Task<IEnumerable<AvailableSlotDto>> GetAvailableSlotsAsync(string consultantId, DateTime date);
        Task<bool> BookConsultationAsync(string memberId, string consultantId, int slotId);
        Task<IEnumerable<ConsultationBookingDto>> GetMyBookingsAsync(string memberId);
        
        // Feedback methods
        Task<bool> FeedbackConsultantAsync(string memberId, int consultationId, ConsultationFeedbackDto dto);
        
        // Approval methods
        Task<bool> ConfirmConsultationAsync(string consultantId, int consultationId);
        Task<bool> RejectConsultationAsync(string consultantId, int consultationId);
        
        // Utility methods
        Task<IEnumerable<ConsultationRequestViewDto>> SearchConsultationRequestsAsync(
            string currentUserId,
            string? userId = null, 
            string? consultantId = null, 
            ConsultationStatus? status = null,
            DateTime? fromDate = null,
            DateTime? toDate = null);
        Task<int> DeleteOldConsultationRequestsAsync();
        Task<bool> IsConsultantAsync(string userId);
        Task<bool> CanAccessConsultationRequestAsync(int requestId, string currentUserId);
        Task<bool> IsConsultationRequestOverlappingAsync(string consultantId, DateTime requestedDate, int durationMinutes);
        Task<bool> CanCompleteSessionAsync(int sessionId, string consultantId);
    }
}