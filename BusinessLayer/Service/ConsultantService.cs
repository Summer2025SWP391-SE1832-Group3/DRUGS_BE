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
            var profiles = await _context.ConsultantProfiles.ToListAsync();
            return users.Select(u => {
                var profile = profiles.FirstOrDefault(p => p.ConsultantId == u.Id);
                return new ConsultantListItemDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Rating = profile?.AverageRating ?? 0
                };
            });
        }
        public async Task<ConsultantDetailDto?> GetConsultantDetailAsync(string consultantId)
        {
            var user = await _userManager.FindByIdAsync(consultantId);
            if (user == null)
                return null;
            var profile = await _context.ConsultantProfiles.FindAsync(consultantId);
            var certs = await _context.Certificates.Where(c => c.ApplicationUserId == user.Id).ToListAsync();
            var workingHours = await _context.ConsultantWorkingHours.Where(w => w.ConsultantId == user.Id).ToListAsync();
            return new ConsultantDetailDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                Status = profile?.Status ?? "Active",
                AverageRating = profile?.AverageRating ?? 0,
                FeedbackCount = profile?.FeedbackCount ?? 0,
                TotalConsultations = profile?.TotalConsultations ?? 0,
                WorkingHours = workingHours.Select(w => new ConsultantWorkingHourDto
                {
                    Date = w.SlotDate ?? DateTime.MinValue,
                    StartTime = w.StartTime,
                    EndTime = w.EndTime
                }),
                Certificates = certs.Select(c => new CertificateDto
                {
                    Id = c.Id,
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
        public async Task<IEnumerable<CertificateViewDto>> GetCertificatesAsync(string consultantId)
        {
            var certs = await _context.Certificates.Where(c => c.ApplicationUserId == consultantId).ToListAsync();
            return certs.Select(c => new CertificateViewDto
            {
                Id = c.Id,
                Name = c.Name,
                IssuingOrganization = c.IssuingOrganization,
                DateIssued = c.DateIssued
            });
        }
        public async Task<bool> AddCertificateAsync(string consultantId, CertificateDto dto)
        {
            if (dto.DateIssued > DateTime.Now)
                throw new ArgumentException("Issue date cannot be greater than current date.");
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
            if (dto.DateIssued > DateTime.Now)
                throw new ArgumentException("Issue date cannot be greater than current date.");
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
                Date = w.SlotDate ?? DateTime.MinValue,
                StartTime = w.StartTime,
                EndTime = w.EndTime
            });
        }
        public async Task<bool> AddWorkingHourByDateAsync(string consultantId, DateTime date, TimeSpan? startTime, TimeSpan? endTime)
        {
            if (startTime == null || endTime == null || startTime >= endTime)
                throw new ArgumentException("Start time must be less than end time.");
            
            var oldSlots = _context.ConsultantWorkingHours.Where(w => w.ConsultantId == consultantId && w.SlotDate == date);
            _context.ConsultantWorkingHours.RemoveRange(oldSlots);
           
            // Thay đổi slot thành 2 tiếng thay vì 1 tiếng
            for (var t = startTime.Value; t + TimeSpan.FromHours(2) <= endTime.Value; t += TimeSpan.FromHours(2))
            {
                var slot = new ConsultantWorkingHour
                {
                    ConsultantId = consultantId,
                    SlotDate = date,
                    DayOfWeek = date.DayOfWeek,
                    StartTime = t,
                    EndTime = t + TimeSpan.FromHours(2), // Slot 2 tiếng
                    Status = WorkingHourStatus.Available,
                    CreatedAt = DateTime.Now
                };
                _context.ConsultantWorkingHours.Add(slot);
            }
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateWorkingHourByDateAsync(string consultantId, DateTime date, TimeSpan? startTime, TimeSpan? endTime)
        {
            if (startTime == null || endTime == null || startTime >= endTime)
                throw new ArgumentException("Start time must be less than end time.");
            
            // Xóa toàn bộ slot cũ trong ngày này
            var oldSlots = _context.ConsultantWorkingHours.Where(w => w.ConsultantId == consultantId && w.SlotDate == date);
            _context.ConsultantWorkingHours.RemoveRange(oldSlots);
            // Tạo lại từng slot 2 tiếng, mỗi slot 1 bản ghi
            for (var t = startTime.Value; t + TimeSpan.FromHours(2) <= endTime.Value; t += TimeSpan.FromHours(2))
            {
                var slot = new ConsultantWorkingHour
                {
                    ConsultantId = consultantId,
                    SlotDate = date,
                    DayOfWeek = date.DayOfWeek,
                    StartTime = t,
                    EndTime = t + TimeSpan.FromHours(2), // Slot 2 tiếng
                    Status = WorkingHourStatus.Available,
                    CreatedAt = DateTime.Now
                };
                _context.ConsultantWorkingHours.Add(slot);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Kiểm tra trùng lịch trước khi thêm/cập nhật
        /// </summary>
        private async Task<bool> CheckScheduleConflictInternalAsync(string consultantId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            // Kiểm tra trùng với slots hiện tại trong ngày
            var existingSlots = await _context.ConsultantWorkingHours
                .Where(w => w.ConsultantId == consultantId && 
                           w.SlotDate == date.Date &&
                           w.Status != WorkingHourStatus.Cancelled) // Không tính slots đã hủy
                .ToListAsync();

            foreach (var slot in existingSlots)
            {
                // Kiểm tra xem có overlap không
                if (IsTimeOverlap(startTime, endTime, slot.StartTime, slot.EndTime))
                {
                    return true; // Có trùng lịch
                }
            }

            // Kiểm tra trùng với consultation requests đã được approve
            var existingConsultations = await _context.ConsultationRequests
                .Where(c => c.ConsultantId == consultantId && 
                           c.RequestedDate.Date == date.Date &&
                           c.Status == ConsultationStatus.Approved)
                .ToListAsync();

            foreach (var consultation in existingConsultations)
            {
                var consultationStart = consultation.RequestedDate.TimeOfDay;
                var consultationEnd = consultation.RequestedDate.AddMinutes(consultation.DurationMinutes).TimeOfDay;
                
                if (IsTimeOverlap(startTime, endTime, consultationStart, consultationEnd))
                {
                    return true; // Có trùng lịch
                }
            }

            return false; // Không có trùng lịch
        }

        /// <summary>
        /// Kiểm tra xem hai khoảng thời gian có overlap không
        /// </summary>
        private bool IsTimeOverlap(TimeSpan start1, TimeSpan end1, TimeSpan start2, TimeSpan end2)
        {
            // Hai khoảng thời gian overlap khi:
            // start1 < end2 AND start2 < end1
            return start1 < end2 && start2 < end1;
        }

        /// <summary>
        /// Kiểm tra trùng lịch trước khi tạo (public method)
        /// </summary>
        public async Task<bool> CheckScheduleConflictAsync(string consultantId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            return await CheckScheduleConflictInternalAsync(consultantId, date, startTime, endTime);
        }

        /// <summary>
        /// Lấy thông tin slots hiện tại của consultant trong ngày
        /// </summary>
        public async Task<IEnumerable<ConsultantWorkingHourDto>> GetWorkingHoursByDateAsync(string consultantId, DateTime date)
        {
            var hours = await _context.ConsultantWorkingHours
                .Where(w => w.ConsultantId == consultantId && w.SlotDate == date.Date)
                .OrderBy(w => w.StartTime)
                .ToListAsync();
            
            return hours.Select(w => new ConsultantWorkingHourDto
            {
                Date = w.SlotDate ?? DateTime.MinValue,
                StartTime = w.StartTime,
                EndTime = w.EndTime,
                Status = w.Status.ToString()
            });
        }

        /// <summary>
        /// Kiểm tra trùng lịch cho nhiều ngày
        /// </summary>
        public async Task<List<DateTime>> CheckScheduleConflictForDateRangeAsync(string consultantId, DateTime fromDate, DateTime toDate, TimeSpan startTime, TimeSpan endTime)
        {
            var conflictDates = new List<DateTime>();
            var currentDate = fromDate.Date;
            
            while (currentDate <= toDate.Date)
            {
                var hasConflict = await CheckScheduleConflictAsync(consultantId, currentDate, startTime, endTime);
                if (hasConflict)
                {
                    conflictDates.Add(currentDate);
                }
                currentDate = currentDate.AddDays(1);
            }
            
            return conflictDates;
        }
    }
} 