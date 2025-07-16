using DataAccessLayer.Dto.Consultation;
using DataAccessLayer.Model;
using BusinessLayer.Dto.Common;

namespace BusinessLayer.IService
{
    public interface IConsultationService
    {
        Task<IEnumerable<AvailableSlotDto>> GetAvailableSlotsAsync(string consultantId, DateTime date);
        Task<bool> BookConsultationAsync(string memberId, string consultantId, int slotId);
        Task<IEnumerable<ConsultationBookingDto>> GetMyBookingsAsync(string memberId);
        Task<bool> FeedbackConsultantAsync(string memberId, int consultationId, ConsultationFeedbackDto dto);
        Task<bool> ConfirmConsultationAsync(string consultantId, int consultationId);
        Task<bool> RejectConsultationAsync(string consultantId, int consultationId);
        Task<ConsultationSessionViewDto> CompleteConsultationSessionAsync(int sessionId, string currentUserId);
        Task<ConsultationSessionViewDto> CreateConsultationSessionAsync(int requestId, ConsultationSessionCreateDto dto, string currentUserId);
        Task<ConsultationSessionViewDto?> GetConsultationSessionAsync(int requestId, string currentUserId);
        Task<ConsultationSessionViewDto> UpdateConsultationSessionAsync(int sessionId, ConsultationSessionCreateDto dto, string currentUserId);
        Task<bool> DeleteConsultationSessionAsync(int sessionId, string currentUserId);
    }
}