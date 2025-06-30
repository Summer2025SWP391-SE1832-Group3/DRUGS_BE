using AutoMapper;
using BusinessLayer.IService;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DataAccessLayer.Dto.Lesson;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using DataAccessLayer.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<LessonService> _logger;

        public LessonService(ILessonRepository lessonRepository,IMapper mapper,Cloudinary cloudinary,ILogger<LessonService> logger) {
            _lessonRepository = lessonRepository;
            _mapper = mapper;
            _logger = logger;
            _cloudinary= cloudinary;
        }
        public async Task<LessonDto> CreateLessonAsync(LessonCreateDto lessonCreateDto)
        {
            string videoUrl = null;
            if (lessonCreateDto.Video != null)
            {
                using (var stream = lessonCreateDto.Video.OpenReadStream())
                {
                    var uploadParams = new VideoUploadParams()
                    {
                        File = new FileDescription(lessonCreateDto.Video.FileName, stream),
                        PublicId = $"lesson_videos/{Guid.NewGuid()}",
                    };

                    try
                    {
                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                        if (uploadResult == null)
                        {
                            throw new Exception("Upload failed. No response from Cloudinary.");
                        }

                        if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            videoUrl = uploadResult.Url.ToString();
                        }
                        else
                        {
                            _logger.LogError("Cloudinary upload failed with status code: {StatusCode}", uploadResult.StatusCode);
                            throw new Exception("Video upload failed.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Exception occurred during upload: {Message}", ex.Message);
                        throw;
                    }
                }
            }

            var lesson = new Lesson
            {
                Title = lessonCreateDto.Title,

                Content = lessonCreateDto.Content,
                VideoUrl = videoUrl,  
                CourseId = lessonCreateDto.CourseId
            };

            var createdLesson = await _lessonRepository.AddAsync(lesson);

            return _mapper.Map<LessonDto>(createdLesson);
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
            var lesson = await _lessonRepository.GetByIdAsync(lessonId);
            if (lesson == null)
            {
                throw new Exception("Lesson not found");
            }

            lesson.Title = lessonUpdateDto.Title ?? lesson.Title;
            lesson.Content = lessonUpdateDto.Content ?? lesson.Content;

            if (lessonUpdateDto.Video != null)
            {
                using (var stream = lessonUpdateDto.Video.OpenReadStream())
                {
                    var uploadParams = new VideoUploadParams()
                    {
                        File = new FileDescription(lessonUpdateDto.Video.FileName, stream),
                        PublicId = $"lesson_videos/{Guid.NewGuid()}",
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        lesson.VideoUrl = uploadResult.Url.ToString();
                    }
                    else
                    {
                        throw new Exception("Video upload failed.");
                    }
                }
            }

            await _lessonRepository.UpdateAsync(lesson);
        }
    }
}
