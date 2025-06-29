using DataAccessLayer.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Dto.Consultation
{
    public class ConsultationRequestUpdateDto
    {
        [Required]
        public ConsultationStatus Status { get; set; }
        
        public DateTime? ScheduledDate { get; set; }
        
        [MaxLength(500)]
        public string? GoogleMeetLink { get; set; }
        
        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
} 