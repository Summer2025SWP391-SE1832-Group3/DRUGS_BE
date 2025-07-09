using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Model
{
    public enum CourseStatus
    {
        Draft,
        Active,
        Inactive
    }
    public enum CourseTopic
    {
        Awareness,         // Nhận thức 
        Prevention,        // Kỹ năng phòng tránh
        Refusal,           // Từ chối   
    }
    public class Course{
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public CourseStatus Status { get; set; }
        //public bool IsActive { get; set; }
        public CourseTopic Topic { get; set; }
        public int? FinalExamSurveyId { get; set; }
        public Survey? FinalExamSurvey { get; set; }
        public ICollection<Lesson> Lessions { get; set; }
        public ICollection<CourseEnrollment> CourseEnrollments { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }


    }
}