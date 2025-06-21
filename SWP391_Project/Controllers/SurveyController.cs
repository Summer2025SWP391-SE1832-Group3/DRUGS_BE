using BusinessLayer.IService;
using BusinessLayer.Service;
using DataAccessLayer.Dto.Survey;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        //[HttpPost()]

    }
}
