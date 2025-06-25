using System;

namespace DataAccessLayer.Dto.Consultation
{
    public class ConsultationReviewViewDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 