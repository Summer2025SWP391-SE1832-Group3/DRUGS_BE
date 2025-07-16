using DataAccessLayer.Dto.Consultation;
using DataAccessLayer.Model;
using BusinessLayer.Dto.Common;

namespace BusinessLayer.IService
{
    public interface IConsultationService
    {
        Task<IEnumerable<AvailableSlotDto>> GetAvailableSlotsAsync(string consultantId, DateTime from, DateTime to);
        Task<bool> BookConsultationAsync(string memberId, string consultantId, ConsultationBookingDto dto);
        Task<IEnumerable<ConsultationBookingDto>> GetMyBookingsAsync(string memberId);
        Task<bool> FeedbackConsultantAsync(string memberId, int consultationId, ConsultationFeedbackDto dto);
        Task<bool> ConfirmConsultationAsync(string consultantId, int consultationId);
    }
}