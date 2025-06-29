using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Lesson
{
    public class LessonProgressReportDto
    {
        public int CourseId { get; set; }
        public List<LessonProgressDetailDto> Reports { get; set; }
    }

    public class LessonProgressDetailDto
    {
        public string UserId { get; set; }
        public int TotalLessons { get; set; }
        public int CompletedLessons { get; set; }
    }
}
