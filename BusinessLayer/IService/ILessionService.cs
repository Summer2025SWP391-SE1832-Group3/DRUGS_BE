using DataAccessLayer.Dto.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IService
{
    public interface ILessionService
    {
        Task<LessonDto> CreateLessonAsync(LessonCreateDto lessonCreateDto);
        Task<IEnumerable<LessonDto>> GetLessonsByCourseIdAsync(int courseId);
        Task<LessonDto> GetLessonByIdAsync(int lessonId);

        Task UpdateLessonAsync(int lessonId, LessonUpdateDto lessonUpdateDto);
        Task DeleteLessonAsync(int lessonId);

    }
}
