using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Course
{
    public class CourseReportDto
    {
        public int CourseId { get; set; }
        public int TotalEnrollments { get; set; }
        public int CompletedCount { get; set; }
        public int PendingCount { get; set; }
    }
}
