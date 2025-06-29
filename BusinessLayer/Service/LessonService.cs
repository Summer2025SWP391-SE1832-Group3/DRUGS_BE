using AutoMapper;
using BusinessLayer.IService;
using DataAccessLayer.Dto.Lesson;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class LessonService : ILessionService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IMapper _mapper;

        public LessonService(ILessonRepository lessonRepository,IMapper mapper) {
            _lessonRepository = lessonRepository;
            _mapper = mapper;
        }
        public async Task<LessonDto> CreateLessonAsync(LessonCreateDto lessonCreateDto)
        {
            var lesson = _mapper.Map<Lesson>(lessonCreateDto);
            await _lessonRepository.AddAsync(lesson);
            return _mapper.Map<LessonDto>(lesson);


        }

        public async Task DeleteLessonAsync(int lessonId)
        {
             await _lessonRepository.DeleteAsync(lessonId);
        }

        public async Task<LessonDto> GetLessonByIdAsync(int lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId);
            return _mapper.Map<LessonDto>(lesson);
        }

        public async Task<IEnumerable<LessonDto>> GetLessonsByCourseIdAsync(int courseId)
        {
            var lessons=await _lessonRepository.GetByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<LessonDto>>(lessons);
        }

        public async Task UpdateLessonAsync(int lessonId, LessonUpdateDto lessonUpdateDto)
        {
            var existLesson=await _lessonRepository.GetByIdAsync(lessonId);
            if (existLesson != null)
            {
               _mapper.Map(lessonUpdateDto, existLesson);
                await _lessonRepository.UpdateAsync(existLesson);
            }
        }
    }
}
