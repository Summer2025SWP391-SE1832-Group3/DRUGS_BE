    using BusinessLayer.IService;
using DataAccessLayer.Dto.Course;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SWP391_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("{courseId:int}")]
        public async Task<IActionResult> GetCourseById(int courseId)
        {

            var course = await _courseService.GetCourseByCourseId(courseId);
            if (course == null)
            {
                return NotFound("Course not found.");
            }

            return Ok(course);
        }


        [HttpGet("Detail/{courseId:int}")]
        [Authorize(Roles = "Member,Manager,Staff")]
        public async Task<IActionResult> GetCourseDetailById(int courseId)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var course = await _courseService.GetCourseByIdAsync(courseId, userId, role);
            if (course == null)
            {
                return NotFound("Course not found.");
            }

            return Ok(course);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var courses = await _courseService.GetAllCoursesAsync(userId);

            return Ok(courses);
        }


        [HttpGet("user/courses")]
        [Authorize(Roles = "Member,Manager")]
        public async Task<IActionResult> GetCoursesByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found.");
            }

            var coursesInProgress = await _courseService.GetCoursesInProgressAsync(userId);
            var completedCourses = await _courseService.GetCompletedCoursesAsync(userId);

            return Ok(new
            {
                InProgress = coursesInProgress,
                Completed = completedCourses
            });
        }

        [HttpGet("topic/{topic}")]
        public async Task<IActionResult> GetCoursesByTopic(CourseTopic topic)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var courses = await _courseService.GetCoursesByTopicAsync(topic,userId);

            return Ok(courses);
        }

        [HttpGet("searchTitle")]
        public async Task<IActionResult> SearchCourse([FromQuery] string? searchTerm)
        {
            if (searchTerm == null)
            {
                searchTerm = "";
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var courses = await _courseService.SearchCourseAsync(searchTerm, userId);
            return Ok(courses);
        }

        [HttpPost("enroll/{courseId}")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> EnrollInCourse(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isEnrolled = await _courseService.IsUserEnrolledInCourseAsync(userId, courseId);
            if (isEnrolled)
            {
                return BadRequest("You are already enrolled in this course.");
            }
            var enrollment = await _courseService.EnrollInCourseAsync(userId, courseId);
            if (enrollment == null)
            {
                return BadRequest("Failed to enroll in the course.");
            }
            return Ok(new { Message = "Enrolled successfully!", CourseId = courseId });
        }

        [HttpPut("progress/{lessonId}")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> UpdateLessonProgress(int lessonId, bool isCompleted)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var progress = await _courseService.UpdateLessonProgressAsync(userId, lessonId, isCompleted);
            return Ok(progress);
        }

        [HttpGet("progress/{courseId}")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> GetLessonProgressForUser(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var progress = await _courseService.GetLessonProgressForUserAsync(userId, courseId);
            return Ok(progress);
        }


        [HttpGet("lesson-progress-report/{courseId}")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> GetLessonProgressReport(int courseId)
        {
            var report = await _courseService.GetLessonProgressReportAsync(courseId);
            return Ok(report);
        }


       

    }
}
