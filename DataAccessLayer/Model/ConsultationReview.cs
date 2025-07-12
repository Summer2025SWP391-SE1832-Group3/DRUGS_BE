using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Model
{
    public class ConsultationReview
    {
        public int Id { get; set; }
        
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Comment { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [Required]
        public int ConsultationSessionId { get; set; }
        public ConsultationSession ConsultationSession { get; set; }
    }
} 