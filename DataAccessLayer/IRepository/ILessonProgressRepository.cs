using DataAccessLayer.Dto.Lesson;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface ILessonProgressRepository
    {
        Task<LessonProgress> UpdateLessonProgressAsync(string userId, int lessonId, bool isCompleted);
        Task<IEnumerable<LessonProgress>> GetLessonProgressByUserAndCourseAsync(string userId, int courseId);
    }

}
