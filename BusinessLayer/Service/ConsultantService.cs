using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.IService;
using DataAccessLayer.Dto.Account;
using DataAccessLayer.Model;
using DataAccessLayer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Service
{
    public class ConsultantService : IConsultantService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDBContext _context;
        public ConsultantService(UserManager<ApplicationUser> userManager, ApplicationDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IEnumerable<ConsultantViewDto>> GetAllConsultantsAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync("Consultant");
            return users.Select(user => new ConsultantViewDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender
            });
        }

        public async Task<ConsultantDetailDto> GetConsultantDetailAsync(string consultantId)
        {
            var user = await _userManager.Users
                .Include(u => u.WorkingHours)
                .Include(u => u.Certificates)
                .FirstOrDefaultAsync(u => u.Id == consultantId);
            if (user == null) return null;
            return new ConsultantDetailDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                Description = user.Description,
                WorkingHours = user.WorkingHours,
                Certificates = user.Certificates
            };
        }

        public async Task<IEnumerable<ConsultantWorkingHour>> GetWorkingHoursAsync(string consultantId)
        {
            return await _context.ConsultantWorkingHours.Where(cw => cw.ConsultantId == consultantId).ToListAsync();
        }

        public async Task<IEnumerable<Certificate>> GetCertificatesAsync(string consultantId)
        {
            return await _context.Certificates.Where(c => c.ApplicationUserId == consultantId).ToListAsync();
        }

        public async Task<bool> UpdateConsultantProfileAsync(string consultantId, ConsultantUpdateDto updateDto)
        {
            var user = await _userManager.FindByIdAsync(consultantId);
            if (user == null) return false;
            user.Description = updateDto.Description;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> AddOrUpdateCertificateAsync(string consultantId, CertificateDto certificateDto, int? certificateId = null)
        {
            Certificate cert;
            if (certificateId.HasValue)
            {
                cert = await _context.Certificates.FirstOrDefaultAsync(c => c.Id == certificateId && c.ApplicationUserId == consultantId);
                if (cert == null) return false;
                cert.Name = certificateDto.Name;
                cert.IssuingOrganization = certificateDto.IssuingOrganization;
                cert.DateIssued = certificateDto.DateIssued;
            }
            else
            {
                cert = new Certificate
                {
                    Name = certificateDto.Name,
                    IssuingOrganization = certificateDto.IssuingOrganization,
                    DateIssued = certificateDto.DateIssued,
                    ApplicationUserId = consultantId
                };
                _context.Certificates.Add(cert);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCertificateAsync(string consultantId, int certificateId)
        {
            var cert = await _context.Certificates.FirstOrDefaultAsync(c => c.Id == certificateId && c.ApplicationUserId == consultantId);
            if (cert == null) return false;
            _context.Certificates.Remove(cert);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ConsultantWorkingHour> AddWorkingHourAsync(string consultantId, ConsultantWorkingHour workingHour)
        {
            if (await IsWorkingHourOverlappingAsync(consultantId, workingHour.DayOfWeek, workingHour.StartTime, workingHour.EndTime))
                throw new InvalidOperationException("Working hour overlaps with existing schedule.");
            workingHour.ConsultantId = consultantId;
            _context.ConsultantWorkingHours.Add(workingHour);
            await _context.SaveChangesAsync();
            return workingHour;
        }

        public async Task<ConsultantWorkingHour> UpdateWorkingHourAsync(string consultantId, int workingHourId, ConsultantWorkingHour workingHour)
        {
            var existing = await _context.ConsultantWorkingHours.FirstOrDefaultAsync(w => w.Id == workingHourId && w.ConsultantId == consultantId);
            if (existing == null) throw new KeyNotFoundException("Working hour not found.");
            if (await IsWorkingHourOverlappingAsync(consultantId, workingHour.DayOfWeek, workingHour.StartTime, workingHour.EndTime, workingHourId))
                throw new InvalidOperationException("Working hour overlaps with existing schedule.");
            existing.DayOfWeek = workingHour.DayOfWeek;
            existing.StartTime = workingHour.StartTime;
            existing.EndTime = workingHour.EndTime;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteWorkingHourAsync(string consultantId, int workingHourId)
        {
            var existing = await _context.ConsultantWorkingHours.FirstOrDefaultAsync(w => w.Id == workingHourId && w.ConsultantId == consultantId);
            if (existing == null) return false;
            _context.ConsultantWorkingHours.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsWorkingHourOverlappingAsync(string consultantId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime, int? excludeId = null)
        {
            var hours = _context.ConsultantWorkingHours.Where(w => w.ConsultantId == consultantId && w.DayOfWeek == dayOfWeek);
            if (excludeId.HasValue)
                hours = hours.Where(w => w.Id != excludeId.Value);
            foreach (var wh in hours)
            {
                if (startTime < wh.EndTime && endTime > wh.StartTime)
                    return true;
            }
            return false;
        }
    }
} 