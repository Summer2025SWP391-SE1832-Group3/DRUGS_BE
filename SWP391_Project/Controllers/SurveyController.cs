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
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> CreateSurvey([FromBody] SurveyCreateWithQuesAndAnsDto dto,int? courseId)
        {
          
            SurveyType surveyTypeEnum;
            if (!Enum.TryParse(dto.SurveyType.ToString(), out surveyTypeEnum))
            {
                return BadRequest("Invalid SurveyType.");
            }
            if (courseId.HasValue)
            {
                var course=await _courseService.GetCourseByIdAsync(courseId.Value);
                if (course == null)
                {
                    return BadRequest("Invalid CourseId, course not found.");
                }
            }
            var createdSurvey = await _surveyService.CreateSurveyWithQuestionAndAnswerAsync(dto,courseId);
            if (createdSurvey == null)
            {
                return BadRequest("Failed to create survey.");
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
            var surveys = await _surveyService.GetAllSurveyAsync();
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

        [HttpDelete("{surveyId:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteSurvey(int surveyId)
        {
            var result = await _surveyService.DeleteSurveyAsync(surveyId);
            if (result)
            {
                return Ok("Survey deleted successfully.");
            }
            return NotFound("Survey not found");
        }

        [HttpPut("{surveyId:int}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateSurvey(int surveyId, [FromBody] SurveyUpdateWithQuesAndAnsDto surveyUpdateDto)
        {
            var result = await _surveyService.UpdateSurveyAsync(surveyUpdateDto, surveyId);
            if (result)
            {
                return Ok("Survey updated successfully.");
            }
            return NotFound("Survey not found or could not be updated.");
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

        // Pagination endpoints
        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedSurveys(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] SurveyType? surveyType = null)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var result = await _surveyService.GetPaginatedSurveysAsync(page, pageSize, searchTerm, surveyType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving surveys");
            }
        }

        [HttpGet("{surveyId:int}/surveyResult/paginated")]
        public async Task<IActionResult> GetPaginatedUserSurveyResults(
            int surveyId,
            string userId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var result = await _surveyService.GetPaginatedUserSurveyResultsAsync(surveyId, userId, page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving survey results");
            }
        }
    }
}
