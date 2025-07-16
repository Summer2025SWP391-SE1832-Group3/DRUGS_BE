using System;

namespace DataAccessLayer.Model
{
    public class ConsultantFeedback
    {
        public int ConsultantFeedbackId { get; set; }
        public string ConsultantId { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        public ApplicationUser Consultant { get; set; }
        public ApplicationUser User { get; set; }
    }
} 