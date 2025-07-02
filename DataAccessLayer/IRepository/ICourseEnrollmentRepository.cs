using DataAccessLayer.Dto.Course;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface ICourseEnrollmentRepository
    {
        Task<CourseEnrollment> EnrollInCourseAsync(string userId, int courseId);
        Task<IEnumerable<CourseEnrollment>> GetEnrollmentsByCourseIdAsync(int courseId);
        Task<IEnumerable<CourseEnrollment>> GetEnrollmentsByUserIdAsync(string userId);
        Task<CourseEnrollment> GetEnrollmentByUserIdAndCourseIdAsync(string userId, int courseId);
        Task<bool> UpdateStatus(string userId, int courseId);
        Task<CourseStatus> GetEnrollmentStatusAsync(string userId, int courseId);
    }
}
