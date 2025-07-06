using DataAccessLayer.Dto.Lesson;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IService
{
    public interface ILessonService
    {
        Task<LessonViewDto> CreateLessonAsync(LessonCreateDto lessonCreateDto);
        Task<IEnumerable<LessonViewDto>> GetLessonsByCourseIdAsync(int courseId);
        Task<LessonViewDto> GetLessonByIdAsync(int lessonId);

        Task UpdateLessonAsync(int lessonId, LessonUpdateDto lessonUpdateDto);
        Task<bool> DeleteLessonAsync(int lessonId);

    }
}
