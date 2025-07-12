﻿using DataAccessLayer.Dto.Survey;
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
        Task<Survey?> GetByIdAnyAsync(int surveyId);
        Task<List<Survey>> GetAllAsync(string userRole);
        Task<List<Survey>> GetAllByTypeAsync(SurveyType? surveyType, string userRole);
        Task<bool> UpdateAsync(Survey survey);
        Task<Survey> GetSurveyByCourseIdAsync(int courseId);
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
        Task<SurveyAnswerResult> CreateSurveyAnswerResultAsync(SurveyAnswerResult result);
        Task<List<SurveyAnswerResult>> GetSurveyAnswerResultAsync(int surveyResultId);

        Task<List<SurveyResult>?> GetSurveyResultAsync(int surveyId, string userId);
        Task<List<SurveyResult>> GetAddictionSurveyResultsAsync(string userId);

    }
}
