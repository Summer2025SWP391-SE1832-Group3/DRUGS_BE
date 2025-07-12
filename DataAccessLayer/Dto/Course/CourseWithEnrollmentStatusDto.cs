using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Course
{
    public enum CourseEnrollmentStatus
    {
        NotEnrolled,   
        InProgress,  
        Completed,
        Suspended
    }
    public class CourseWithEnrollmentStatusDto
    {
        public CourseListDto Course { get; set; }
        public CourseEnrollmentStatus Status { get; set; }
    }
}
