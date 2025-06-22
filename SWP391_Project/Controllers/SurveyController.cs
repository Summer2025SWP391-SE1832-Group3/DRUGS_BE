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

        public SurveyController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        [HttpPost("create-survey")]
        public async Task<IActionResult> CreateSurvey([FromBody] SurveyCreateWithQuesAndAnsDto dto)
        {
            var createdSurvey = await _surveyService.CreateSurveyWithQuestionAndAnswerAsync(dto);
            SurveyType surveyTypeEnum;
            if (!Enum.TryParse(dto.SurveyType.ToString(), out surveyTypeEnum))
            {
                return BadRequest("Invalid SurveyType.");
            }
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
                return NotFound("Survey not found.");
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
                        recommendation = surveyResult.Recommendation
                    });
                }
                else if (surveyResult.Survey.SurveyType == SurveyType.CourseTest)
                {
                    return Ok(new
                    {
                        Message = "Survey submitted successfully!",
                        Status = surveyResult.Recommendation
                    });
                }
            }
            return BadRequest("Failed to submit survey.");
        }

        [HttpDelete("{surveyId:int}")]
        public async Task<IActionResult> DeleteSurvey(int surveyId)
        {
            var result = await _surveyService.DeleteSurveyAsync(surveyId);
            if (result)
            {
                return Ok("Survey deleted successfully.");
            }
            return NotFound("Survey not found or could not be deleted.");
        }

        [HttpPut("{surveyId:int}")]
        public async Task<IActionResult> UpdateSurvey(int surveyId, [FromBody] SurveyUpdateWithQuesAndAnsDto surveyUpdateDto)
        {
            var result = await _surveyService.UpdateSurveyAsync(surveyUpdateDto, surveyId);
            if (result)
            {
                return Ok("Survey updated successfully.");
            }
            return NotFound("Survey not found or could not be updated.");
        }
    }


}
