using BusinessLayer.IService;
using DataAccessLayer.Dto.Course;
using DataAccessLayer.Dto.Survey;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class CourseTestSurveyService : ICourseTestSurveyService
    {
        private readonly ICourseTestSurveyRepository _courseTestSurveyRepo;
        private readonly ISurveyRepository _surveyRepo;
        private readonly IUserRepository _userRepo;
        public CourseTestSurveyService(ICourseTestSurveyRepository courseTestSurveyRepo, ISurveyRepository surveyRepo, IUserRepository userRepo)
        {
            _courseTestSurveyRepo = courseTestSurveyRepo;
            _surveyRepo = surveyRepo;
            _userRepo = userRepo;
        }

        public async Task<CourseTestSurveyViewDto> AssignTestToCourseAsync(CourseTestSurveyCreateDto dto)
        {
            var entity = new CourseTestSurvey
            {
                CourseId = dto.CourseId,
                SurveyId = dto.SurveyId,
                CreatedAt = DateTime.Now
            };
            var result = await _courseTestSurveyRepo.AddAsync(entity);
            return new CourseTestSurveyViewDto
            {
                Id = result.Id,
                CourseId = result.CourseId,
                SurveyId = result.SurveyId,
                SurveyName = result.Survey?.SurveyName ?? "",
                CreatedAt = result.CreatedAt
            };
        }

        public async Task<CourseTestSurveyViewDto?> GetTestByCourseIdAsync(int courseId)
        {
            var entity = await _courseTestSurveyRepo.GetByCourseIdAsync(courseId);
            if (entity == null) return null;
            return new CourseTestSurveyViewDto
            {
                Id = entity.Id,
                CourseId = entity.CourseId,
                SurveyId = entity.SurveyId,
                SurveyName = entity.Survey?.SurveyName ?? "",
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<CourseTestResultDto> SubmitCourseTestAsync(int courseId, string userId, CourseTestSubmitDto dto)
        {
            // Lưu kết quả test vào bảng SurveyResult/SurveyAnswerResult
            var survey = await _surveyRepo.GetByIdAsync(dto.SurveyId);
            if (survey == null) throw new Exception("Survey not found");
            int totalScore = 0;
            foreach (var answer in dto.Answers)
            {
                var question = survey.SurveyQuestions.FirstOrDefault(q => q.QuestionId == answer.QuestionId);
                var selectedAnswer = question?.SurveyAnswers.FirstOrDefault(a => a.AnswerId == answer.AnswerId);
                if (selectedAnswer != null && selectedAnswer.IsCorrect == true)
                {
                    totalScore++;
                }
            }
            var surveyResult = new SurveyResult
            {
                UserId = userId,
                SurveyId = dto.SurveyId,
                TotalScore = totalScore,
                TakeAt = DateTime.Now,
                Recommendation = totalScore >= (survey.SurveyQuestions.Count * 0.7) ? "Pass" : "Fail",
                ResultStatus = totalScore >= (survey.SurveyQuestions.Count * 0.7) ? "Pass" : "Fail"
            };
            var createdResult = await _surveyRepo.CreateSurveyResultAsync(surveyResult);
            foreach (var answer in dto.Answers)
            {
                var answerResult = new SurveyAnswerResult
                {
                    SurveyResultId = createdResult.ResultId,
                    AnswerId = answer.AnswerId,
                    QuestionId = answer.QuestionId
                };
                await _surveyRepo.CreateSurveyAnswerResultAsync(answerResult);
            }
            var user = await _userRepo.GetByIdAsync(userId);
            return new CourseTestResultDto
            {
                SurveyResultId = createdResult.ResultId,
                SurveyId = createdResult.SurveyId,
                SurveyName = survey.SurveyName,
                UserId = userId,
                UserName = user?.UserName ?? "",
                SubmittedAt = createdResult.TakeAt,
                TotalScore = createdResult.TotalScore,
                Recommendation = createdResult.Recommendation,
                ResultStatus = createdResult.ResultStatus
            };
        }

        public async Task<List<CourseTestResultDto>> GetCourseTestResultsAsync(int courseId, string userId)
        {
            var courseTest = await _courseTestSurveyRepo.GetByCourseIdAsync(courseId);
            if (courseTest == null) return new List<CourseTestResultDto>();
            var surveyResults = await _surveyRepo.GetSurveyResultAsync(courseTest.SurveyId, userId);
            var user = await _userRepo.GetByIdAsync(userId);
            return surveyResults?.Select(r => new CourseTestResultDto
            {
                SurveyResultId = r.ResultId,
                SurveyId = r.SurveyId,
                SurveyName = r.Survey.SurveyName,
                UserId = userId,
                UserName = user?.UserName ?? "",
                SubmittedAt = r.TakeAt,
                TotalScore = r.TotalScore,
                Recommendation = r.Recommendation,
                ResultStatus = r.ResultStatus
            }).ToList() ?? new List<CourseTestResultDto>();
        }
    }
} 