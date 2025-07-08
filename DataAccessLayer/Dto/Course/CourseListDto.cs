using DataAccessLayer.Dto.Lesson;
using DataAccessLayer.Dto.Survey;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Course
{
    public class CourseListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        //public bool IsActive { get; set; }
        public CourseStatus Status { get; set; }
        public CourseTopic Topic { get; set; }
        public string? StatusMessage { get; set; }
    }
}
