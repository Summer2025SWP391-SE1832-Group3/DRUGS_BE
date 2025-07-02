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
        [HttpPost]
        [Authorize(Roles = "Manager")] 
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDto courseCreateDto)
        {
            var course = await _courseService.CreateCourseAsync(courseCreateDto);
            if (course == null)
            {
                return BadRequest("Failed to create course.");
            }
            return Ok(new { Message = "Course created successfully!", CourseId = course.Id });
        }

        [HttpGet("Detail/{courseId:int}")]
        public async Task<IActionResult> GetCourseById(int courseId)
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
            if (role == "Member")
            {
                var isEnrolled = await _courseService.IsUserEnrolledInCourseAsync(userId, courseId);
                if (!isEnrolled)
                {
                    return Ok(new
                    {
                        Message = "You are not enrolled in this course.",
                        Course = course,
                        IsEnrolled = false
                    });
                }
                return Ok(new
                {
                    Message = "You are enrolled in this course.",
                    Course = course,
                    IsEnrolled = true
                });
            }
            else if (role == "Manager" || role == "Staff")
            {
                return Ok(new
                {
                    Message = "Viewing course details",
                    Course = course,
                });
            }

            return BadRequest("Role not recognized.");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCourses()
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role); 
            var courses = await _courseService.GetAllCoursesAsync(userRole);
            return Ok(courses);
        }


        [HttpGet("user/courses")]
        [Authorize(Roles = "Member")]
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


        [HttpPut("{courseId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateCourse(int courseId, [FromBody] CourseUpdateDto courseUpdateDto)
        {
            await _courseService.UpdateCourseAsync(courseId, courseUpdateDto);
            return Ok(new { Message = "Course updated successfully!" });
        }

        [HttpDelete("{courseId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            await _courseService.DeleteCourseAsync(courseId);
            return Ok(new { Message = "Course deleted successfully!" });
        }

        [HttpGet("topic/{topic}")]
        public async Task<IActionResult> GetCoursesByTopic(CourseTopic topic)
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role); 
            var courses = await _courseService.GetCoursesByTopicAsync(topic, userRole);
            return Ok(courses);
        }

        [HttpGet("searchTitle")]
        public async Task<IActionResult> SearchCourse([FromQuery] string searchTerm)
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var courses = await _courseService.SearchCourseAsync(searchTerm, userRole);
            var result = new List<CourseWithEnrollmentStatusDto>();

            foreach (var course in courses)
            {
                var isEnrolled = await _courseService.IsUserEnrolledInCourseAsync(userId, course.Id);

                result.Add(new CourseWithEnrollmentStatusDto
                {
                    Course = course,
                    IsEnrolled = isEnrolled
                });
            }

            return Ok(result);
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

        [HttpGet("enrollments/{courseId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllEnrollmentsForCourse(int courseId)
        {
            var enrollments = await _courseService.GetAllEnrollmentsForCourseAsync(courseId);
            return Ok(enrollments);
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

        [HttpGet("report/{courseId}")]
        [Authorize(Roles = "Manager,Admin")] 
        public async Task<IActionResult> GetCourseReport(int courseId)
        {
            var report = await _courseService.GetCourseReportAsync(courseId);
            return Ok(report);
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
