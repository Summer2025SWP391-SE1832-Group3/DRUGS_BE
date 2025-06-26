using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Model
{
    public class ConsultantWorkingHour
    {
        public int Id { get; set; }
        [Required]
        public string ConsultantId { get; set; }
        public ApplicationUser Consultant { get; set; }
        [Required]
        public DayOfWeek DayOfWeek { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
    }
} 