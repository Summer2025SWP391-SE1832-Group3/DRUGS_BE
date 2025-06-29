using DataAccessLayer.Data;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class CourseEnrollmentRepository : ICourseEnrollmentRepository
    {
        private readonly ApplicationDBContext _context;

        public CourseEnrollmentRepository(ApplicationDBContext context) {
            _context = context;
        }
        public async Task<CourseEnrollment> EnrollInCourseAsync(string userId, int courseId)
        {
            var enroll = new CourseEnrollment
            {
                UserId = userId,
                CourseId = courseId,
                EnrolledAt = DateTime.Now
            };
            await _context.CourseEnrollments.AddAsync(enroll);
            await _context.SaveChangesAsync();
            return enroll;

        }

        public async Task<IEnumerable<CourseEnrollment>> GetEnrollmentsByCourseIdAsync(int courseId)
        {
            return await _context.CourseEnrollments
                             .Where(c => c.CourseId == courseId)
                             .Include(c => c.User)
                             .ToListAsync();
        }
    }
}
