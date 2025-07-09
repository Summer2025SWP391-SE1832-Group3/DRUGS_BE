using DataAccessLayer.Dto.Course;
using DataAccessLayer.Dto.Lesson;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface ICourseReportRepository
    {
        Task<IEnumerable<CourseEnrollment>> GetEnrollmentsByCourseIdAsync(int courseId);
        Task<LessonProgressReportDto> GetLessonProgressReportAsync(int courseId);
    }

}
