using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface ILessonRepository
    {
        Task<Lesson> GetByIdAsync(int id);
        Task<List<Lesson>> GetByCourseIdAsync(int courseId);
        Task<Lesson> AddAsync(Lesson lesson);
        Task DeleteAsync(int id);
        Task UpdateAsync(Lesson lesson);


    }
}
