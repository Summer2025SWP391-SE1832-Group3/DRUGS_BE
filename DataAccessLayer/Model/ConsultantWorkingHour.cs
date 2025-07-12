using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Model
{
    public enum WorkingHourStatus
    {
        Available,
        Booked,
        Completed,
        Cancelled
    }

    public class ConsultantWorkingHour
    {
        public int Id { get; set; }
        [Required]
        public string ConsultantId { get; set; }
        public ApplicationUser Consultant { get; set; }
        [Required]
        public DayOfWeek DayOfWeek { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        
        // Status to track if this slot is available, booked, completed, or cancelled
        public WorkingHourStatus Status { get; set; } = WorkingHourStatus.Available;
        
        // Date for this specific working hour slot (for tracking specific dates)
        public DateTime? SlotDate { get; set; }
        
        // Reference to the consultation request if this slot is booked
        public int? ConsultationRequestId { get; set; }
        public ConsultationRequest? ConsultationRequest { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
} 