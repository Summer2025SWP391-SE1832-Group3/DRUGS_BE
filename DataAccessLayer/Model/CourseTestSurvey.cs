using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Model
{
    public class CourseTestSurvey
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }
        [Required]
        public int SurveyId { get; set; }
        public Survey Survey { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
} 