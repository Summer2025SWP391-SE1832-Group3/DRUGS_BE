using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Model
{
    public class ConsultationSession
    {
        public int Id { get; set; }
        
        [Required]
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        
        public string? SessionNotes { get; set; }
        public string? Recommendations { get; set; }
        public bool IsCompleted { get; set; } = false;
        
        public string? GoogleMeetLink { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        
        // Foreign Key
        [Required]
        public int ConsultationRequestId { get; set; }
        public ConsultationRequest ConsultationRequest { get; set; }
        
        // Navigation property to Review
        public ConsultationReview? ConsultationReview { get; set; }
    }
} 