    using DataAccessLayer.Dto.Survey;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IService
{
    public interface ISurveyService
    {
        //Task<Survey> CreateSurveyAsync(SurveyCreateDto dto);
        Task<SurveyViewDto?> GetSurveyByIdAsync(int surveyId);
        Task<List<SurveyViewDto>> GetAllSurveyAsync();
        Task<bool> UpdateSurveyAsync(SurveyUpdateDto dto);
        Task<bool> DeleteSurveyAsync(int surveyId);

        //Task<SurveyAnswer> CreateAnswerAsync(SurveyAnswerCreateDto dto);
        Task<List<SurveyAnswer>> GetAllAnswersByQuestionIdAsync(int questionId);
        Task<bool> UpdateAnswerAsync(SurveyAnswerUpdateDto dto);
        Task<bool> DeleteAnswerAsync(int answerId);

        //Task<SurveyQuestion> CreateQuestionAsync(SurveyQuestionCreateDto dto);
        Task<List<SurveyQuestion>> GetAllQuestionsBySurveyIdAsync(int surveyId);
        Task<bool> UpdateQuestionAsync(SurveyQuestionUpdateDto dto);
        Task<bool> DeleteQuestionAsync(int questionId);

        Task<SurveyResult> CreateSurveyResultAsync(SurveyResultDto dto);
        Task<List<SurveyAnswerResultDto>> GetSurveyAnswerResultAsync(int surveyResultId);
        Task<Survey> CreateSurveyWithQuestionAndAnswerAsync(SurveyCreateWithQuesAndAnsDto dto);
    }
}
