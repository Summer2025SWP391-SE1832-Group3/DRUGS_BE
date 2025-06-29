using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Dto.Consultation
{
    public class ConsultationRequestCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        
        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Category { get; set; }
        
        [Required]
        public DateTime RequestedDate { get; set; }
        
        [Range(30, 180)]
        public int DurationMinutes { get; set; } = 60;
        
        [MaxLength(500)]
        public string? Notes { get; set; }
        
        [Required]
        public string ConsultantId { get; set; }
    }
} 