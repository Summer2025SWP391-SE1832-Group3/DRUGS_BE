using AutoMapper.Execution;
using BusinessLayer.IService;
using BusinessLayer.Service;
using DataAccessLayer.Dto.Survey;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SWP391_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _surveyService;
        private readonly ICourseService _courseService;

        public SurveyController(ISurveyService surveyService,ICourseService courseService)
        {
            _surveyService = surveyService;
            _courseService= courseService;
        }

        [HttpPost("create-survey")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> CreateSurvey([FromBody] SurveyCreateWithQuesAndAnsDto dto,int? courseId)
        {
          
            SurveyType surveyTypeEnum;
            if (!Enum.TryParse(dto.SurveyType.ToString(), out surveyTypeEnum))
            {
                return BadRequest("Invalid SurveyType.");
            }
            if (courseId.HasValue)
            {
                var course=await _courseService.GetCourseByCourseId(courseId.Value);
                if (course == null)
                {
                    return BadRequest("Invalid CourseId, course not found.");
                }
            }
            var createdSurvey = await _surveyService.CreateSurveyWithQuestionAndAnswerAsync(dto,courseId);
            if (createdSurvey == null)
            {
                return BadRequest("Each course can only have one active CourseTest survey. Please delete the existing survey before creating a new one.");
            }
            else return Ok(new
            {
                Message = "Survey is created successfully!",
                SurveyId = createdSurvey.SurveyId
            });
        }

        [HttpGet("{surveyId:int}")]
        public async Task<IActionResult> GetSurveyById(int surveyId)
        {
            var survey = await _surveyService.GetSurveyByIdAsync(surveyId);
            if (survey == null)
            {
                return NotFound("Survey not found or Inactive ");
            }
            return Ok(survey);
        }

        [HttpGet("all_survey")]
        public async Task<IActionResult> GetAllSurveys()
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var surveys = await _surveyService.GetAllSurveyAsync(userRole);
            return Ok(surveys);
        }

        [HttpGet("surveys/surveyType")]
        public async Task<IActionResult> GetAllSurveys([FromQuery] SurveyType? surveyType = null)
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var surveys = await _surveyService.GetAllSurveyByType(surveyType, userRole);
            return Ok(surveys);
        }

        [HttpPost("submit/{surveyId}")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> SubmitSurvey(int surveyId, SurveyAnswerDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var totalScore = await _surveyService.CalculatorScore(dto, surveyId);
            var surveyResult = await _surveyService.CreateSurveyResultAsync(surveyId, dto, userId, totalScore);
            if (surveyResult != null)
            {
                if (surveyResult.Survey.SurveyType == SurveyType.AddictionSurvey)
                {
                    return Ok(new
                    {
                        Message = "Survey submitted successfully!",
                        RiskLevel = surveyResult.RiskLevel,
                        Recommendation = surveyResult.Recommendation
                    });
                }
                else if (surveyResult.Survey.SurveyType == SurveyType.CourseTest)
                {
                    return Ok(new
                    {
                        Message = "Survey submitted successfully!",
                        Status= surveyResult.ResultStatus,
                        Recommendation = surveyResult.Recommendation
                    });
                }
            }
            return BadRequest("Failed to submit survey.");
        }

        [HttpPut("{surveyId:int}/status")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> ChangeSurveyStatus(int surveyId, [FromQuery] bool isActive)
        {
            var survey = await _surveyService.GetSurveyByIdAnyAsync(surveyId);
            if (survey == null)
                return NotFound("Survey not found.");
            if (survey.SurveyType == SurveyType.CourseTest && survey.CourseId.HasValue)
            {
                var course = await _courseService.GetCourseByCourseId(survey.CourseId.Value);
                if (course != null && course.Status == CourseStatus.Active && !isActive)
                    return BadRequest("You cannot deactivate the survey as the course is active");
                if (course != null && course.Status == CourseStatus.Inactive && isActive)
                {
                    return BadRequest("You cannot activate the survey as the course is inactive.");
            }

                if (isActive)
                {
                    var existingSurvey = await _surveyService.GetSurveyByCourseIdAsync(course.Id);
                    if (existingSurvey != null && existingSurvey.IsActive)
                    {
                        return BadRequest("The course already has an active survey. Please deactivate the existing survey before activating a new one.");
        }
                }
            }

            var result = await _surveyService.SetSurveyStatusAsync(surveyId, isActive);
            if (result)
                return Ok(isActive ? "Survey activated." : "Survey deactivated.");

            return BadRequest("Failed to update survey status.");
        }

        
        [HttpPut("{surveyId:int}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> UpdateSurvey(int surveyId, [FromBody] SurveyUpdateWithQuesAndAnsDto surveyUpdateDto)
        {
            var survey = await _surveyService.GetSurveyByIdAnyAsync(surveyId);
            if (survey == null)
            {
                return NotFound(new { Message = "Survey not found." });
            }
            if (survey.SurveyType == SurveyType.CourseTest && survey.CourseId.HasValue)
            {
                var course = await _courseService.GetCourseByCourseId(survey.CourseId.Value);

                if (course == null)
                {
                    return BadRequest(new { Message = "Associated course not found." });
                }
                if (course.Status == CourseStatus.Active)
                {
                    return BadRequest(new { Message = "Cannot update the survey because the associated course is already active." });
                }
            }

            var (success, message) = await _surveyService.UpdateSurveyAsync(surveyUpdateDto, surveyId);
            if (!success)
            {
                return BadRequest(new { Message = message });
            }
            return Ok(new { Message = message });
        }

        [HttpGet("{surveyId:int}/statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSurveyStatistics(int surveyId)
        {
            var surveyStatistic = await _surveyService.GetSurveyStatisticAsync(surveyId);
            if(surveyStatistic == null)
            {
                return NotFound("Survey not found.");
            }
            return Ok(surveyStatistic);

        }

        [HttpGet("{surveyId:int}/surveyResult")]
        public async Task<IActionResult> GetUserSurveyResult(int surveyId, string userId)
        {
            var surveyResult = await _surveyService.GetUserSurveyResultAsync(surveyId, userId);
            if (surveyResult == null)
            {
                return NotFound("Survey result not found or inactive.");
            }
            return Ok(surveyResult);
        }

        [HttpGet("user/{userId}/addiction-surveys")]
        public async Task<IActionResult> GetAllAddictionSurveyResults(string userId)
        {
            var surveyResults = await _surveyService.GetAddictionSurveyResultsAsync(userId);
            if (surveyResults == null || !surveyResults.Any())
            {
                return NotFound("No addiction survey results found for this user.");
            }

            return Ok(surveyResults);
            }
        }

        //[HttpGet("{surveyId:int}/surveyResult/paginated")]
        //public async Task<IActionResult> GetPaginatedUserSurveyResults(
        //    int surveyId,
        //    string userId,
        //    [FromQuery] int page = 1,
        //    [FromQuery] int pageSize = 10)
        //{
        //    try
        //    {
        //        if (page < 1) page = 1;
        //        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        //        var result = await _surveyService.GetPaginatedUserSurveyResultsAsync(surveyId, userId, page, pageSize);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "An error occurred while retrieving survey results");
        //    }
        //}

}
