using System;

namespace DataAccessLayer.Dto.Consultation
{
    public class ConsultationSessionViewDto
    {
        public int Id { get; set; }
        //public DateTime StartTime { get; set; }
        //public DateTime? EndTime { get; set; }
        public string? SessionNotes { get; set; }
        public string? Recommendations { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 