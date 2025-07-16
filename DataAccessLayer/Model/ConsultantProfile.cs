using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Model
{
    public class ConsultantProfile
    {
        [Key, ForeignKey("Consultant")]
        public string ConsultantId { get; set; }
        public ApplicationUser Consultant { get; set; }
        public string Status { get; set; } = "Active";
        public double AverageRating { get; set; } = 0.0;
        public int FeedbackCount { get; set; } = 0;
        public int TotalConsultations { get; set; } = 0;
    }
} 