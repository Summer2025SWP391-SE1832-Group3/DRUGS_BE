    using DataAccessLayer.Dto.Survey;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IService
{
    public interface ISurveyService
    {
        Task<SurveyViewDto?> GetSurveyByIdAsync(int surveyId);
        Task<List<SurveyViewDto>> GetAllSurveyAsync(string userRole);
        Task<List<SurveyViewDto>> GetAllSurveyByType(SurveyType? surveyType, string userRole);
        Task<bool> SetSurveyStatusAsync(int surveyId, bool isActive);
        Task<Survey?> GetSurveyByIdAnyAsync(int surveyId);
        Task<bool> IsSurveyActiveAsync(int surveyId);
        Task<Survey?> GetSurveyByCourseIdAsync(int courseId);
        Task<List<SurveyAnswer>> GetAllAnswersByQuestionIdAsync(int questionId);
        Task<bool> DeleteAnswerAsync(int answerId);
        
        Task<List<SurveyQuestion>> GetAllQuestionsBySurveyIdAsync(int surveyId);
        Task<bool> DeleteQuestionAsync(int questionId);

        Task<Survey> CreateSurveyWithQuestionAndAnswerAsync(SurveyCreateWithQuesAndAnsDto dto,int? courseId);
        Task<(bool Success, string Message)> UpdateSurveyAsync(SurveyUpdateWithQuesAndAnsDto surveyUpdateDto, int surveyId);
        Task<SurveyResult> CreateSurveyResultAsync(int surveyId,SurveyAnswerDto surveyAnswerDto, string userId, int totalScore);
        Task<int> CalculatorScore(SurveyAnswerDto surveyAnswerDto, int surveyId);
        Task<List<SurveyAnswerResultDto>> GetSurveyAnswerResultAsync(int surveyResultId);
        Task<SurveyStatisticDto> GetSurveyStatisticAsync(int surveyId);
        Task<List<SurveyResultDto>> GetUserSurveyResultAsync(int surveyId, string userId);
        Task<List<SurveyResultDto>> GetAddictionSurveyResultsAsync(string userId);
        Task<SurveyResultDto> GetUserSurveyResultNewestAsync(int surveyId, string userId);
    }
}
