using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Dto.Consultation
{
    public class ConsultationSessionCreateDto
    {
        [Required]
        public DateTime StartTime { get; set; }
        
        [MaxLength(2000)]
        public string? SessionNotes { get; set; }
        
        [MaxLength(2000)]
        public string? Recommendations { get; set; }
        
        [MaxLength(500)]
        public string? GoogleMeetLink { get; set; }
    }
} 