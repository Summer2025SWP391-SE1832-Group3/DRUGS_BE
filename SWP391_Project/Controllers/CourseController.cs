using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.IService;
using DataAccessLayer.Dto.Course;
using System.Collections.Generic;
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

        [HttpGet("recommend")]
        [Authorize]
        public ActionResult<IEnumerable<CourseViewDto>> GetRecommendedCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var courses = _courseService.GetRecommendedCourses(userId);
            return Ok(courses);
        }
    }
} 