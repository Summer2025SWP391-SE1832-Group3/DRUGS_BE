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

        public async Task<IEnumerable<ConsultantListItemDto>> GetAllConsultantsAsync()
        {
            
            var users = await _userManager.GetUsersInRoleAsync("Consultant");
            return users.Select(u => new ConsultantListItemDto
            {
                Id = u.Id,
                FullName = u.FullName,       
                Rating = 0       
            });
        }
        public async Task<ConsultantDetailDto?> GetConsultantDetailAsync(string consultantId)
        {
            var user = await _userManager.FindByIdAsync(consultantId);
            if (user == null)
                return null;
            // Lấy certificates từ bảng Certificates
            var certs = await _context.Certificates.Where(c => c.ApplicationUserId == user.Id).ToListAsync();
            return new ConsultantDetailDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                WorkingHours = user.WorkingHours,
                Certificates = certs.Select(c => new CertificateDto
                {
                    Name = c.Name,
                    IssuingOrganization = c.IssuingOrganization,
                    DateIssued = c.DateIssued
                })
            };
        }
        public async Task<bool> UpdateProfileAsync(string consultantId, ConsultantProfileUpdateDto dto)
        {
            var user = await _userManager.FindByIdAsync(consultantId);
            if (user == null)
                return false;
            if (!string.IsNullOrEmpty(dto.FullName)) user.FullName = dto.FullName;
            if (!string.IsNullOrEmpty(dto.PhoneNumber)) user.PhoneNumber = dto.PhoneNumber;
            if (!string.IsNullOrEmpty(dto.Email)) user.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Gender)) user.Gender = dto.Gender;
            
            if (dto.Certifications != null)
            {
                var oldCerts = _context.Certificates.Where(c => c.ApplicationUserId == user.Id);
                _context.Certificates.RemoveRange(oldCerts);
                foreach (var cert in dto.Certifications)
                {
                    _context.Certificates.Add(new Certificate
                    {
                        Name = cert.Name ?? string.Empty,
                        IssuingOrganization = cert.Issuer ?? string.Empty,
                        DateIssued = cert.DateIssued ?? DateTime.Now,
                        ApplicationUserId = user.Id
                    });
                }
            }
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<CertificateDto>> GetCertificatesAsync(string consultantId)
        {
            var certs = await _context.Certificates.Where(c => c.ApplicationUserId == consultantId).ToListAsync();
            return certs.Select(c => new CertificateDto
            {
                Name = c.Name,
                IssuingOrganization = c.IssuingOrganization,
                DateIssued = c.DateIssued
            });
        }

        public async Task<bool> AddCertificateAsync(string consultantId, CertificateDto dto)
        {
            var cert = new Certificate
            {
                Name = dto.Name ?? string.Empty,
                IssuingOrganization = dto.IssuingOrganization ?? string.Empty,
                DateIssued = dto.DateIssued,
                ApplicationUserId = consultantId
            };
            _context.Certificates.Add(cert);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCertificateAsync(string consultantId, int certificateId, CertificateDto dto)
        {
            var cert = await _context.Certificates.FirstOrDefaultAsync(c => c.Id == certificateId && c.ApplicationUserId == consultantId);
            if (cert == null) return false;
            cert.Name = dto.Name ?? cert.Name;
            cert.IssuingOrganization = dto.IssuingOrganization ?? cert.IssuingOrganization;
            cert.DateIssued = dto.DateIssued;
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

        public async Task<IEnumerable<ConsultantWorkingHourDto>> GetWorkingHoursAsync(string consultantId)
        {
            var hours = await _context.ConsultantWorkingHours.Where(w => w.ConsultantId == consultantId).ToListAsync();
            return hours.Select(w => new ConsultantWorkingHourDto
            {
                DayOfWeek = w.DayOfWeek,
                StartTime = w.StartTime,
                EndTime = w.EndTime
            });
        }

        public async Task<bool> AddWorkingHourAsync(string consultantId, ConsultantWorkingHourDto dto)
        {
            var hour = new ConsultantWorkingHour
            {
                ConsultantId = consultantId,
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime ?? TimeSpan.Zero,
                EndTime = dto.EndTime ?? TimeSpan.Zero
            };
            _context.ConsultantWorkingHours.Add(hour);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateWorkingHourAsync(string consultantId, int workingHourId, ConsultantWorkingHourDto dto)
        {
            var hour = await _context.ConsultantWorkingHours.FirstOrDefaultAsync(w => w.Id == workingHourId && w.ConsultantId == consultantId);
            if (hour == null) return false;
            hour.DayOfWeek = dto.DayOfWeek;
            hour.StartTime = dto.StartTime ?? hour.StartTime;
            hour.EndTime = dto.EndTime ?? hour.EndTime;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteWorkingHourAsync(string consultantId, int workingHourId)
        {
            var hour = await _context.ConsultantWorkingHours.FirstOrDefaultAsync(w => w.Id == workingHourId && w.ConsultantId == consultantId);
            if (hour == null) return false;
            _context.ConsultantWorkingHours.Remove(hour);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 