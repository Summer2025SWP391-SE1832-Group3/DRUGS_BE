using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public enum EnrollmentStatus
    {
        InProgress,  
        Completed, 
        Suspended   
    }
    public class CourseEnrollment
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public DateTime EnrolledAt { get; set; }
        public EnrollmentStatus Status { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? SuspendedUntil { get; set; }
        public ICollection<LessonProgress> LessonProgresses { get; set; }
    }
}
