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
        Task<bool> UpdateConsultantProfileAsync(string consultantId, ConsultantUpdateDto updateDto);
        Task<bool> AddOrUpdateCertificateAsync(string consultantId, CertificateDto certificateDto, int? certificateId = null);
        Task<bool> DeleteCertificateAsync(string consultantId, int certificateId);
        Task<ConsultantWorkingHour> AddWorkingHourAsync(string consultantId, ConsultantWorkingHour workingHour);
        Task<ConsultantWorkingHour> UpdateWorkingHourAsync(string consultantId, int workingHourId, ConsultantWorkingHour workingHour);
        Task<bool> DeleteWorkingHourAsync(string consultantId, int workingHourId);
        Task<bool> IsWorkingHourOverlappingAsync(string consultantId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime, int? excludeId = null);
    }
} 