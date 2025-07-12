using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Model
{
    public enum ConsultationStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Completed = 3,
        Cancelled = 4
    }

    public class ConsultationRequest
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public string Category { get; set; }
        
        public DateTime RequestedDate { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public int DurationMinutes { get; set; } = 60;
        
        public string? Notes { get; set; }
        
        public ConsultationStatus Status { get; set; } = ConsultationStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        
        // Foreign Keys
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        [Required]
        public string ConsultantId { get; set; }
        public ApplicationUser Consultant { get; set; }
        
        public ConsultationSession? ConsultationSession { get; set; }
        
        public int? ConsultantWorkingHourId { get; set; }
        public ConsultantWorkingHour ConsultantWorkingHour { get; set; }
    }
} 