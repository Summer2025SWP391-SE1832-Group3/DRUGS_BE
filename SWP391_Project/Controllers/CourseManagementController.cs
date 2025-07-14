using BusinessLayer.IService;
using BusinessLayer.Service;
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
    public class CourseManagementController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseManagementController(ICourseService courseService)
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

        [HttpPut("approve/{courseId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ApproveCourse(int courseId)
        {
            var course = await _courseService.GetCourseByCourseId(courseId);
            if (course == null)
                return NotFound("Course not found.");

            if (course.Status != CourseStatus.Draft)
                return BadRequest("Only Draft courses can be activated.");

            if (!await _courseService.CanApproveCourseAsync(courseId))
                return BadRequest("Course must have at least one lesson and one active survey before activation.");

            await _courseService.UpdateCourseStatusAsync(courseId, CourseStatus.Active);
            return Ok(new { Message = "Course is now active and visible to members!" });
        }

        [HttpPut("{courseId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateCourse(int courseId, [FromBody] CourseUpdateDto courseUpdateDto)
        {
            await _courseService.UpdateCourseAsync(courseId, courseUpdateDto);
            return Ok(new { Message = "Course updated successfully!" });
        }


        [HttpDelete("{courseId:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            var result = await _courseService.DeactivateCourseAsync(courseId);
            if (result)
            {
                return Ok("Course has been deactivated successfully.");
            }

            return BadRequest("Course cannot be deactivated because there are active enrollments.");
        }

        [HttpGet("draft")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> GetDraftCourses()
        {
            var draftCourses = await _courseService.GetCoursesByStatusAsync(CourseStatus.Draft);
            return Ok(draftCourses);
        }

        [HttpGet("searchTitle")]
        public async Task<IActionResult> SearchCourse([FromQuery] string searchTerm, [FromQuery] CourseStatus? status = null)
        {
            var courses = await _courseService.SearchCourseForManagerAsync(searchTerm, status);
            return Ok(courses);
        }

        [HttpGet("topic/{topic}")]  
        public async Task<IActionResult> GetCoursesByTopic(CourseTopic topic, [FromQuery] CourseStatus? status = null)
        {
            var courses = await _courseService.GetCoursesByTopicForManagerAsync(topic, status);
            return Ok(courses);
        }

        [HttpGet("enrollments/{courseId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllEnrollmentsForCourse(int courseId)
        {
            var enrollments = await _courseService.GetAllEnrollmentsForCourseAsync(courseId);
            return Ok(enrollments);
        }



        [HttpGet("allCourses/Manager")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetCourses([FromQuery] CourseStatus? status = null)
        {
            var courses = await _courseService.GetCoursesByStatusAsync(status);
            return Ok(courses);
        }
        
        [HttpGet("courses_without_survey")]
        [Authorize(Roles = "Manager,Staff")]
        public async Task<IActionResult> GetCoursesWithoutSurveyOrInactiveSurvey()
        {
            var courses = await _courseService.GetCoursesWithoutSurveyOrInactiveSurveyAsync();
            return Ok(courses);
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
