using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Dto.Account;
using DataAccessLayer.Model;

namespace BusinessLayer.IService
{
    public interface IConsultantService
    {
        Task<IEnumerable<ConsultantViewDto>> GetAllConsultantsAsync();
        Task<ConsultantDetailDto> GetConsultantDetailAsync(string consultantId);
        Task<IEnumerable<ConsultantWorkingHour>> GetWorkingHoursAsync(string consultantId);
        Task<IEnumerable<Certificate>> GetCertificatesAsync(string consultantId);
        Task<bool> AddOrUpdateCertificateAsync(string consultantId, CertificateDto certificateDto, int? certificateId = null);
        Task<bool> DeleteCertificateAsync(string consultantId, int certificateId);
        Task<ConsultantWorkingHour> AddWorkingHourAsync(string consultantId, ConsultantWorkingHour workingHour);
        Task<ConsultantWorkingHour> UpdateWorkingHourAsync(string consultantId, int workingHourId, ConsultantWorkingHour workingHour);
        Task<bool> DeleteWorkingHourAsync(string consultantId, int workingHourId);
        Task<bool> IsWorkingHourOverlappingAsync(string consultantId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime, int? excludeId = null);
        
        // New methods for slot management
        Task<IEnumerable<ConsultantWorkingHour>> GetAvailableSlotsAsync(string consultantId, DateTime fromDate, DateTime toDate);
        Task<IEnumerable<ConsultantWorkingHour>> GenerateSlotsForDateRangeAsync(string consultantId, DateTime fromDate, DateTime toDate, int slotDurationMinutes = 60);
        Task<bool> BookSlotAsync(int slotId, int consultationRequestId);
        Task<bool> CancelSlotAsync(int slotId);
        Task<bool> CompleteSlotAsync(int slotId);
        Task<object> GetSlotConfigurationAsync(string consultantId);
        
        // Auto slot management
        Task<IEnumerable<ConsultantWorkingHour>> GetAvailableSlotsWithAutoGenerationAsync(string consultantId, DateTime fromDate, DateTime toDate, int slotDurationMinutes = 60);
        Task<int> AutoGenerateSlotsForFutureWeeksAsync(string consultantId, int weeksAhead = 4, int slotDurationMinutes = 60);
        Task<int> CleanupOldSlotsAsync(int daysToKeep = 30);
        Task<int> AutoGenerateSlotsForAllConsultantsAsync(int weeksAhead = 4, int slotDurationMinutes = 60);
        
        // --- CONSULTANT ADVANCED METHODS ---
        Task<IEnumerable<ConsultantViewDto>> GetConsultantsByStatusAsync(string status);
        Task<IEnumerable<ConsultantViewDto>> GetTopConsultantsByPerformanceAsync(int topN);
        Task<IEnumerable<ConsultantViewDto>> GetTopConsultantsByRatingAsync(int topN);
        Task<bool> UpdateConsultantStatusAsync(string consultantId, string status);
        Task<bool> UpdateConsultantPerformanceAsync(string consultantId, int totalConsultations, double averageRating, int feedbackCount);
        
        // --- FEEDBACK FOR CONSULTANT ---
        Task<bool> AddConsultantFeedbackAsync(string consultantId, string userId, int rating, string reviewText);
        Task<IEnumerable<DataAccessLayer.Dto.Feedback.FeedbackViewDto>> GetConsultantFeedbacksAsync(string consultantId);

        Task<PaginatedResult<ConsultantViewDto>> GetPagedConsultantsAsync(int page, int pageSize);
    }
} 