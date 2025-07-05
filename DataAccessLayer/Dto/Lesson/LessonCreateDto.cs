using DataAccessLayer.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Lesson
{
    public class LessonCreateDto
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public string? Content { get; set; }
        public IFormFile? Video { get; set; }
        [Required]
        public int CourseId { get; set; }
    }
}
