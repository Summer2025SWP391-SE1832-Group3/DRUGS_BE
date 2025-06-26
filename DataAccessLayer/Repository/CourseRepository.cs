using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Data;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;

namespace DataAccessLayer.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDBContext _context;
        public CourseRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return _context.Courses.ToList();
        }
    }
} 