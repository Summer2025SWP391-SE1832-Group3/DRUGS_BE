using DataAccessLayer.Data;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataAccessLayer.Repository
{
    public class SurveyRepository : ISurveyRepository
    {
        private readonly ApplicationDBContext _context;

        public SurveyRepository(ApplicationDBContext context) {
            _context = context;
        }
        public async Task<SurveyAnswer> CreateAnswerAsync(SurveyAnswer answer)
        {
            await _context.SurveyAnswers.AddAsync(answer);
            await _context.SaveChangesAsync();
            return answer;
        }

        public async Task<Survey> CreateAsync(Survey survey)
        {
            await _context.Surveys.AddAsync(survey);
            await _context.SaveChangesAsync();
            return survey;
        }

        public async Task<SurveyQuestion> CreateQuestionAsync(SurveyQuestion question)
        {
            await _context.SurveyQuestions.AddAsync(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<SurveyResult> CreateSurveyResultAsync(SurveyResult result)
        {
            await _context.SurveyResults.AddAsync(result);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<SurveyAnswerResult> CreateSurveyAnswerResultAsync(SurveyAnswerResult result)
        {
            await _context.SurveyAnswerResults.AddAsync(result);
             await _context.SaveChangesAsync();
            return result;
        }

        public async Task<bool> DeleteAnswerAsync(int answerId)
        {
            var answer = await _context.SurveyAnswers.FindAsync(answerId);
            if (answer == null) return false;
            _context.SurveyAnswers.Remove(answer);
            return await _context.SaveChangesAsync()>0;
        }


        public async Task<bool> DeleteQuestionAsync(int questionId)
        {
            var question=await _context.SurveyQuestions.FindAsync(questionId);
            if(question == null) return false;
            _context.SurveyQuestions.Remove(question);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<SurveyAnswer>> GetAllAnswersAsync(int questionId)
        {
            return await _context.SurveyAnswers
                .Where(a => a.QuestionId == questionId)
                .ToListAsync();
        }
        public async Task<SurveyAnswer?> GetAnswerByIdAsync(int answerId)
        {
            return await _context.SurveyAnswers
                .FirstOrDefaultAsync(a => a.AnswerId == answerId);  
        }

        public async Task<List<Survey>> GetAllAsync(string userRole)
        {
            var query = _context.Surveys
                   .Include(s=>s.SurveyQuestions)
                   .ThenInclude(q=>q.SurveyAnswers)
                   .AsQueryable();
            if (userRole != "Manager")
            {
                query = query.Where(s => s.IsActive);
            }

            return await query.ToListAsync();

        }
        public async Task<List<Survey>> GetAllByTypeAsync(SurveyType? surveyType, string userRole)
        {
            var query = _context.Surveys
                                .Include(s => s.SurveyQuestions)
                                    .ThenInclude(q => q.SurveyAnswers)
                                .AsQueryable();
            if (userRole != "Manager")
            {
                query = query.Where(s => s.IsActive);
            }

            if (surveyType.HasValue)
            {
                query = query.Where(s => s.SurveyType == surveyType.Value);
            }

            return await query.ToListAsync();
        }
        public async Task<List<SurveyQuestion>> GetAllQuestionsAsync(int surveyId)
        {
            return await _context.SurveyQuestions
                .Where(q => q.SurveyId == surveyId)
                .ToListAsync();
        }

        //fillter just survey active
        public async Task<Survey?> GetByIdAsync(int surveyId)
        {
            return await _context.Surveys
                .Include(s=>s.SurveyResults)
                .Include(s => s.SurveyQuestions)
                .ThenInclude(s => s.SurveyAnswers)
                .FirstOrDefaultAsync(s => s.SurveyId == surveyId && s.IsActive==true);
        }

        public async Task<Survey?> GetByIdAnyAsync(int surveyId)
        {
            return await _context.Surveys
                .Include(s => s.SurveyResults)
                .Include(s => s.SurveyQuestions)
                    .ThenInclude(q => q.SurveyAnswers)
                .FirstOrDefaultAsync(s => s.SurveyId == surveyId);
        }

        public async Task<SurveyQuestion?> GetQuestionByIdAsync(int questionId)
        {
            return await _context.SurveyQuestions.FirstOrDefaultAsync(q=>q.QuestionId == questionId);
        }

        public async Task<List<SurveyAnswerResult>> GetSurveyAnswerResultAsync(int surveyResultId)
        {
            return await _context.SurveyAnswerResults
                .Where(r => r.SurveyResultId == surveyResultId)
                .ToListAsync();
        }

        public async Task<bool> UpdateAnswerAsync(SurveyAnswer answer)
        {
            _context.SurveyAnswers.Update(answer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Survey survey)
        {
            _context.Surveys.Update(survey);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateQuestionAsync(SurveyQuestion question)
        {
            _context.SurveyQuestions.Update(question);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<SurveyResult>?> GetSurveyResultAsync(int surveyId, string userId)
        {
            return await _context.SurveyResults
                                 .Where(s=>s.SurveyId==surveyId && s.UserId==userId)
                                 .Include(s => s.SurveyAnswerResults)
                                 .Include(s => s.Survey)
                                     .ThenInclude(s => s.SurveyQuestions)
                                         .ThenInclude(q => q.SurveyAnswers)
                                 .Include(s => s.User)
                                 .OrderByDescending(s => s.TakeAt)
                                 .ToListAsync();
        }

        public async Task<List<SurveyResult>?> GetAddictionSurveyResultsAsync(string userId)
        {
            return await _context.SurveyResults
                                 .Where(s => s.Survey.SurveyType == SurveyType.AddictionSurvey && s.UserId == userId)
                                 .Include(s => s.SurveyAnswerResults)
                                 .Include(s => s.Survey)
                                     .ThenInclude(s => s.SurveyQuestions)
                                         .ThenInclude(q => q.SurveyAnswers)
                                 .Include(s => s.User)
                                 .OrderByDescending(s => s.TakeAt)
                                 .ToListAsync();
        }
        public async Task<Survey?> GetSurveyByCourseIdAsync(int courseId)
        {
            return await _context.Surveys
                .Where(s => s.CourseId == courseId && s.SurveyType == SurveyType.CourseTest && s.IsActive)
                .FirstOrDefaultAsync();
        }
    }
}
