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
            var courses = await _courseService.GetAllCoursesAsync(userRole, userId);

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            var courses = await _courseService.GetCoursesByTopicAsync(topic, userRole, userId);

            return Ok(courses);
        }

        [HttpGet("searchTitle")]
        public async Task<IActionResult> SearchCourse([FromQuery] string? searchTerm)
        {
            if (searchTerm == null)
            {
                searchTerm = "";
            }
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var courses = await _courseService.SearchCourseAsync(searchTerm, userRole, userId);
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

        [HttpGet("completed-course/{courseId}")]
        [Authorize(Roles = "Member,Manager")]
        public async Task<IActionResult> GetCompletedCourseById(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var course = await _courseService.GetCompletedCourseDetailAsync(courseId, userId);
            if (course == null)
            {
                return NotFound("You have not complete this course!");
            }

            return Ok(course);
        }
        [HttpGet("allCourses/Manager")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetCourses([FromQuery] string? status = null)
        {
            IEnumerable<CourseListDto> courses;

            if (string.IsNullOrEmpty(status))
            {
                courses = await _courseService.GetAllCoursesForManagerAsync();
            }
            else
            {
                switch (status.ToLower())
                {
                    case "active":
                        courses = await _courseService.GetActiveCoursesAsync();
                        break;
                    case "inactive":
                        courses = await _courseService.GetInactiveCoursesAsync();
                        break;
                    default:
                        return BadRequest("Invalid status filter.");
                }
            }

            return Ok(courses);
        }
        [HttpGet("courses_without_survey")]
        [Authorize(Roles = "Manager,Staff")]
        public async Task<IActionResult> GetCoursesWithoutSurveyOrInactiveSurvey()
        {
            var courses = await _courseService.GetCoursesWithoutSurveyOrInactiveSurveyAsync();
            return Ok(courses);
        }

    }
}
