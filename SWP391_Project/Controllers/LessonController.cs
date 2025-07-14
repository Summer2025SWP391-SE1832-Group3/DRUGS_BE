using BusinessLayer.IService;
using BusinessLayer.Service;
using DataAccessLayer.Dto.Lesson;
using DataAccessLayer.Model;
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
        private readonly ICourseService _courseService;

        public LessonController(ILessonService lessonService,ICourseService courseService)
        {
            _lessonService = lessonService;
            _courseService = courseService;
        }

        [HttpPost]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> CreateLesson([FromForm] LessonCreateDto lessonCreateDto)
        {
            var course = await _courseService.GetCourseByCourseId(lessonCreateDto.CourseId);
            if (course == null || course.Status == CourseStatus.Active)
            {
                return BadRequest("Cannot add lessons to an active course.");
            }

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
            var lesson = await _lessonService.GetLessonByIdAsync(lessonId);
            if (lesson == null)
            {
                return NotFound("Lesson not found.");
            }

            var course = await _courseService.GetCourseByCourseId(lesson.CourseId);
            if (course == null || course.Status == CourseStatus.Active)
            {
                return BadRequest("Cannot update lessons in an active course.");
            }

            await _lessonService.UpdateLessonAsync(lessonId, lessonUpdateDto);
            return Ok(new { Message = "Lesson updated successfully!" });
        }

        [HttpDelete("{lessonId}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> DeleteLesson(int lessonId)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(lessonId);
            if (lesson == null)
            {
                return NotFound("Lesson not found.");
            }

            var course = await _courseService.GetCourseByCourseId(lesson.CourseId);
            if (course == null || course.Status == CourseStatus.Active)
            {
                return BadRequest("Cannot delete lessons from an active course.");
            }

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
