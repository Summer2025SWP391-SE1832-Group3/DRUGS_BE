using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Dto.Consultation
{
    public class ConsultationReviewCreateDto
    {
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Comment { get; set; }
    }
} 