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
        [Authorize(Roles = "Staff,Manager")]
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


}
