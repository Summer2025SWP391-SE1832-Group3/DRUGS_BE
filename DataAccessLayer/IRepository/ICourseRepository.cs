using System.Collections.Generic;
using DataAccessLayer.Model;

namespace DataAccessLayer.IRepository
{
    public interface ICourseRepository
    {
        IEnumerable<Course> GetAllCourses();
    }
} 