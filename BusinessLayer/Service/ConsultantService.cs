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

        // New methods for slot management
        public async Task<IEnumerable<ConsultantWorkingHour>> GetAvailableSlotsAsync(string consultantId, DateTime fromDate, DateTime toDate)
        {
            // Get slots that are available within the date range
            return await _context.ConsultantWorkingHours
                .Where(slot => slot.ConsultantId == consultantId &&
                              slot.Status == WorkingHourStatus.Available &&
                              slot.SlotDate >= fromDate.Date &&
                              slot.SlotDate <= toDate.Date)
                .OrderBy(slot => slot.SlotDate)
                .ThenBy(slot => slot.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<ConsultantWorkingHour>> GenerateSlotsForDateRangeAsync(string consultantId, DateTime fromDate, DateTime toDate, int slotDurationMinutes = 60)
        {
            var slots = new List<ConsultantWorkingHour>();
            var workingHours = await _context.ConsultantWorkingHours
                .Where(wh => wh.ConsultantId == consultantId && wh.SlotDate == null) // Base working hours
                .ToListAsync();

            var currentDate = fromDate.Date;
            while (currentDate <= toDate.Date)
            {
                var dayOfWeek = currentDate.DayOfWeek;
                var dayWorkingHours = workingHours.Where(wh => wh.DayOfWeek == dayOfWeek);

                foreach (var workingHour in dayWorkingHours)
                {
                    // Tạo các slot nhỏ trong khoảng thời gian làm việc
                    var currentSlotStart = workingHour.StartTime;
                    var slotDuration = TimeSpan.FromMinutes(slotDurationMinutes);

                    while (currentSlotStart + slotDuration <= workingHour.EndTime)
                    {
                        var currentSlotEnd = currentSlotStart + slotDuration;

                        // Check if slot already exists for this date and time
                        var existingSlot = await _context.ConsultantWorkingHours
                            .FirstOrDefaultAsync(slot => slot.ConsultantId == consultantId &&
                                                       slot.SlotDate == currentDate &&
                                                       slot.StartTime == currentSlotStart &&
                                                       slot.EndTime == currentSlotEnd);

                        if (existingSlot == null)
                        {
                            // Create new slot for this specific date and time
                            var newSlot = new ConsultantWorkingHour
                            {
                                ConsultantId = consultantId,
                                DayOfWeek = dayOfWeek,
                                StartTime = currentSlotStart,
                                EndTime = currentSlotEnd,
                                Status = WorkingHourStatus.Available,
                                SlotDate = currentDate,
                                CreatedAt = DateTime.Now
                            };

                            _context.ConsultantWorkingHours.Add(newSlot);
                            slots.Add(newSlot);
                        }

                        // Move to next slot (có thể thêm break time giữa các slot)
                        currentSlotStart = currentSlotEnd;
                    }
                }

                currentDate = currentDate.AddDays(1);
            }

            await _context.SaveChangesAsync();
            return slots;
        }

        public async Task<bool> BookSlotAsync(int slotId, int consultationRequestId)
        {
            var slot = await _context.ConsultantWorkingHours.FindAsync(slotId);
            if (slot == null || slot.Status != WorkingHourStatus.Available)
                return false;

            slot.Status = WorkingHourStatus.Booked;
            slot.ConsultationRequestId = consultationRequestId;
            slot.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelSlotAsync(int slotId)
        {
            var slot = await _context.ConsultantWorkingHours.FindAsync(slotId);
            if (slot == null || slot.Status != WorkingHourStatus.Booked)
                return false;

            slot.Status = WorkingHourStatus.Available;
            slot.ConsultationRequestId = null;
            slot.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteSlotAsync(int slotId)
        {
            var slot = await _context.ConsultantWorkingHours.FindAsync(slotId);
            if (slot == null || slot.Status != WorkingHourStatus.Booked)
                return false;

            slot.Status = WorkingHourStatus.Completed;
            slot.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object> GetSlotConfigurationAsync(string consultantId)
        {
            var workingHours = await _context.ConsultantWorkingHours
                .Where(wh => wh.ConsultantId == consultantId && wh.SlotDate == null) // Base working hours
                .ToListAsync();

            var slotStats = await _context.ConsultantWorkingHours
                .Where(slot => slot.ConsultantId == consultantId && slot.SlotDate != null)
                .GroupBy(slot => slot.Status)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                .ToListAsync();

            var totalWorkingHours = workingHours.Sum(wh => (wh.EndTime - wh.StartTime).TotalHours);
            var totalSlots = slotStats.Sum(s => s.Count);

            return new
            {
                WorkingHours = workingHours.Select(wh => new
                {
                    DayOfWeek = wh.DayOfWeek.ToString(),
                    StartTime = wh.StartTime.ToString(@"hh\:mm"),
                    EndTime = wh.EndTime.ToString(@"hh\:mm"),
                    Duration = (wh.EndTime - wh.StartTime).TotalHours
                }),
                TotalWorkingHoursPerWeek = totalWorkingHours,
                SlotStatistics = slotStats,
                TotalSlots = totalSlots,
                RecommendedSlotDurations = new[] { 30, 60, 90, 120 } // 30min, 1h, 1.5h, 2h
            };
        }

        // Auto slot management methods
        public async Task<IEnumerable<ConsultantWorkingHour>> GetAvailableSlotsWithAutoGenerationAsync(string consultantId, DateTime fromDate, DateTime toDate, int slotDurationMinutes = 60)
        {
            // Kiểm tra xem có slot nào trong khoảng thời gian này không
            var existingSlots = await _context.ConsultantWorkingHours
                .Where(slot => slot.ConsultantId == consultantId &&
                              slot.SlotDate >= fromDate.Date &&
                              slot.SlotDate <= toDate.Date)
                .AnyAsync();

            // Nếu chưa có slot, tự động tạo
            if (!existingSlots)
            {
                await GenerateSlotsForDateRangeAsync(consultantId, fromDate, toDate, slotDurationMinutes);
            }

            // Trả về các slot available
            return await GetAvailableSlotsAsync(consultantId, fromDate, toDate);
        }

        public async Task<int> AutoGenerateSlotsForFutureWeeksAsync(string consultantId, int weeksAhead = 4, int slotDurationMinutes = 60)
        {
            var today = DateTime.Today;
            var fromDate = today;
            var toDate = today.AddDays(weeksAhead * 7);

            // Kiểm tra xem đã có slot cho tuần tới chưa
            var existingSlots = await _context.ConsultantWorkingHours
                .Where(slot => slot.ConsultantId == consultantId &&
                              slot.SlotDate >= fromDate &&
                              slot.SlotDate <= toDate)
                .AnyAsync();

            if (existingSlots)
            {
                return 0; // Đã có slot rồi
            }

            // Tạo slot cho tuần tới
            var slots = await GenerateSlotsForDateRangeAsync(consultantId, fromDate, toDate, slotDurationMinutes);
            return slots.Count();
        }

        public async Task<int> CleanupOldSlotsAsync(int daysToKeep = 30)
        {
            var cutoffDate = DateTime.Today.AddDays(-daysToKeep);
            
            var oldSlots = await _context.ConsultantWorkingHours
                .Where(slot => slot.SlotDate != null && 
                              slot.SlotDate < cutoffDate &&
                              slot.Status != WorkingHourStatus.Booked) // Không xóa slot đã book
                .ToListAsync();

            _context.ConsultantWorkingHours.RemoveRange(oldSlots);
            await _context.SaveChangesAsync();
            
            return oldSlots.Count;
        }

        public async Task<int> AutoGenerateSlotsForAllConsultantsAsync(int weeksAhead = 4, int slotDurationMinutes = 60)
        {
            var totalSlotsGenerated = 0;
            
            // Lấy tất cả consultant
            var consultants = await _userManager.GetUsersInRoleAsync("Consultant");
            
            foreach (var consultant in consultants)
            {
                var slotsGenerated = await AutoGenerateSlotsForFutureWeeksAsync(consultant.Id, weeksAhead, slotDurationMinutes);
                totalSlotsGenerated += slotsGenerated;
            }
            
            return totalSlotsGenerated;
        }
    }
} 