using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Dto.Account;
using DataAccessLayer.Model;

namespace BusinessLayer.IService
{
    public interface IConsultantService
    {
        Task<IEnumerable<ConsultantListItemDto>> GetAllConsultantsAsync();
        Task<ConsultantDetailDto?> GetConsultantDetailAsync(string consultantId);
        Task<bool> UpdateProfileAsync(string consultantId, ConsultantProfileUpdateDto dto);
        Task<IEnumerable<CertificateDto>> GetCertificatesAsync(string consultantId);
        Task<bool> AddCertificateAsync(string consultantId, CertificateDto dto);
        Task<bool> UpdateCertificateAsync(string consultantId, int certificateId, CertificateDto dto);
        Task<bool> DeleteCertificateAsync(string consultantId, int certificateId);
        Task<IEnumerable<ConsultantWorkingHourDto>> GetWorkingHoursAsync(string consultantId);
        Task<bool> AddWorkingHourAsync(string consultantId, ConsultantWorkingHourDto dto);
        Task<bool> UpdateWorkingHourAsync(string consultantId, int workingHourId, ConsultantWorkingHourDto dto);
        Task<bool> DeleteWorkingHourAsync(string consultantId, int workingHourId);
    }
} 