using DataAccessLayer.Dto.Survey;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface ISurveyRepository
    {
        Task<Survey> CreateAsync(Survey survey);
        Task<Survey?> GetByIdAsync(int surveyId);
        Task<List<Survey>> GetAllAsync();
        Task<bool> UpdateAsync(Survey survey);
        Task<bool> DeleteAsync(int surveyId);

        Task<SurveyAnswer> CreateAnswerAsync(SurveyAnswer answer);
        Task<List<SurveyAnswer>> GetAllAnswersAsync(int questionId);
        Task<bool> UpdateAnswerAsync(SurveyAnswer answer);
        Task<bool> DeleteAnswerAsync(int answerId);
        Task<SurveyAnswer> GetAnswerByIdAsync(int answerId);


        Task<SurveyQuestion> CreateQuestionAsync(SurveyQuestion question);
        Task<SurveyQuestion?> GetQuestionByIdAsync(int questionId);
        Task<List<SurveyQuestion>> GetAllQuestionsAsync(int surveyId);
        Task<bool> UpdateQuestionAsync(SurveyQuestion question);
        Task<bool> DeleteQuestionAsync(int questionId);

        Task<SurveyResult> CreateSurveyResultAsync(SurveyResult result);
        Task<List<SurveyAnswerResult>> GetSurveyAnswerResultAsync(int surveyResultId);

    }
}
