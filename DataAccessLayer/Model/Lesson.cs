using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class Lesson
    {
        public int Id { get; set; }
        [Required,MaxLength(100)]
        public string Title { get; set; }
        public string? Content { get; set; } 
        public string? VideoUrl { get; set; }
        [Required] 
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public ICollection<LessonProgress> LessonProgresses { get; set; }
    }
}
