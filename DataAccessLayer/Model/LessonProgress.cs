using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class LessonProgress
    {
        public int Id { get; set; }
        [Required]
        public int CourseEnrollmentId { get; set; }
        public CourseEnrollment CourseEnrollment { get; set; }
        [Required]
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }

    }
}
