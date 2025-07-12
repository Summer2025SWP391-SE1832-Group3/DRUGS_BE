using DataAccessLayer.Model;
using System;

namespace DataAccessLayer.Dto.Consultation
{
    public class ConsultationRequestViewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public int DurationMinutes { get; set; }
        public string? Notes { get; set; }
        public ConsultationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // User info
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        
        // Consultant info
        public string ConsultantId { get; set; }
        public string ConsultantName { get; set; }
        public string ConsultantFullName { get; set; }
        
        // Session info
        public ConsultationSessionViewDto? Session { get; set; }
        public int? ConsultantWorkingHourId { get; set; }
    }
} 