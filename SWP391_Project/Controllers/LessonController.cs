using BusinessLayer.IService;
using BusinessLayer.Service;
using DataAccessLayer.Dto.Lesson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SWP391_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpPost]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> CreateLesson([FromForm] LessonCreateDto lessonCreateDto)
        {
            var lesson = await _lessonService.CreateLessonAsync(lessonCreateDto);
            if (lesson == null)
            {
                return BadRequest("Failed to create lesson.");
            }
            return Ok(new { Message = "Lesson created successfully!", LessonId = lesson.Id });
        }

        [HttpPut("{lessonId}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> UpdateLesson(int lessonId, [FromForm] LessonUpdateDto lessonUpdateDto)
        {
            await _lessonService.UpdateLessonAsync(lessonId, lessonUpdateDto);
            return Ok(new { Message = "Lesson updated successfully!" });
        }

        [HttpDelete("{lessonId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteLesson(int lessonId)
        {
            var result = await _lessonService.DeleteLessonAsync(lessonId);
            if (!result)
            {
                return NotFound(new { Message = "Lesson already deleted or not found!" });
            }
            return Ok(new { Message = "Lesson deleted successfully!" });
        }

        [HttpGet("{lessonId}")]
        public async Task<IActionResult> GetLessonById(int lessonId)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(lessonId);
            return Ok(lesson);
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetLessonsByCourseId(int courseId)
        {
            var lessons = await _lessonService.GetLessonsByCourseIdAsync(courseId);
            return Ok(lessons);
        }


    }
}
