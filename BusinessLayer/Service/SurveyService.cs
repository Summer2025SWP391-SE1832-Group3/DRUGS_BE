using AutoMapper;
using BusinessLayer.IService;
using DataAccessLayer.Dto.Survey;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class SurveyService:ISurveyService
    {
        private readonly IMapper _mapper;
        private readonly ISurveyRepository _repository;

        public SurveyService(IMapper mapper,ISurveyRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<bool> DeleteAnswerAsync(int answerId)
        {
            return await _repository.DeleteAnswerAsync(answerId);
        }

        public async Task<bool> DeleteSurveyAsync(int surveyId)
        {
            var survey=await _repository.GetByIdAsync(surveyId);
            if(survey==null) return false;
            survey.IsActive = false;
            survey.UpdatedAt= DateTime.Now;
            return await _repository.DeleteAsync(survey);
        }

        public async Task<bool> DeleteQuestionAsync(int questionId)
        {
            return await _repository.DeleteQuestionAsync(questionId);
        }

        public async Task<List<SurveyAnswer>> GetAllAnswersByQuestionIdAsync(int questionId)
        {
            return await _repository.GetAllAnswersAsync(questionId);
        }

        public async Task<List<SurveyQuestion>> GetAllQuestionsBySurveyIdAsync(int surveyId)
        {
            return await _repository.GetAllQuestionsAsync(surveyId);
        }

        public async Task<List<SurveyViewDto>> GetAllSurveyAsync()
        {
           var surveys=await _repository.GetAllAsync();
            return _mapper.Map<List<SurveyViewDto>>(surveys);
        }

        public async Task<List<SurveyAnswerResultDto>> GetSurveyAnswerResultAsync(int surveyResultId)
        {
            var results=await _repository.GetSurveyAnswerResultAsync(surveyResultId);
            return _mapper.Map<List<SurveyAnswerResultDto>>(results);
        }

        public async Task<SurveyViewDto?> GetSurveyByIdAsync(int surveyId)
        { 
            var survey=await _repository.GetByIdAsync(surveyId);
            if(survey == null) return null;
            return _mapper.Map<SurveyViewDto>(survey);
        }

        public async Task<Survey> CreateSurveyWithQuestionAndAnswerAsync(SurveyCreateWithQuesAndAnsDto dto)
        {
            //tạo survey
            var survey = new Survey
            {
                SurveyName = dto.SurveyName,
                Description = dto.Description,
                SurveyType = dto.SurveyType,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.Now

            };
            var createdSurvey =await _repository.CreateAsync(survey);

            //duyệt và tạo câu hỏi
            foreach(var questionDto in dto.QuestionsDto)
            {
                var question = new SurveyQuestion
                {
                    SurveyId = createdSurvey.SurveyId,
                    QuestionText = questionDto.QuestionText
                };
                var createdQuestion = await _repository.CreateQuestionAsync(question);

                //duyệt qua và tạo câu trả lời
                foreach (var answerDto in questionDto.AnswersDto)
                {
                    var answer = new SurveyAnswer
                    {
                        AnswerText = answerDto.AnswerText,
                        IsCorrect = answerDto.IsCorrect ?? false,
                        Score = answerDto.Score,
                        QuestionId = createdQuestion.QuestionId,
                    };
                    await _repository.CreateAnswerAsync(answer);
                }

            }
            return createdSurvey;
        }


        public async Task<int> CalculatorScore(SurveyAnswerDto surveyAnswerDto, int surveyId)
        {
            var survey = await _repository.GetByIdAsync(surveyId);
            int totalScore = 0;
            int totalCorrect = 0;
            if(survey== null) return 0;
            foreach (var answer in surveyAnswerDto.Answers)
            {
                var question = survey.SurveyQuestions.FirstOrDefault(q => q.QuestionId == answer.QuestionId);
                var selectedAnswer = question?.SurveyAnswers.FirstOrDefault(a => a.AnswerId == answer.AnswerId);
                if (selectedAnswer != null)
                {
                    if (survey.SurveyType == SurveyType.AddictionSurvey)
                    {
                        totalScore += (int)selectedAnswer.Score;
                    }
                    else if (survey.SurveyType == SurveyType.CourseTest)
                    {
                        if (selectedAnswer.IsCorrect == true)
                        {
                            totalScore++;
                        }
                    }
                }
            }

            return totalScore;

        }

        //tạo surveyresult
        public async Task<SurveyResult?> CreateSurveyResultAsync(int surveyId, SurveyAnswerDto surveyAnswerDto, string userId, int totalScore)
        {
            var survey= await _repository.GetByIdAsync(surveyId);
            if (survey == null) return null;
            var surveyResult = new SurveyResult
            {
                UserId = userId,
                SurveyId = surveyId,
                TotalScore = totalScore,
                TakeAt = DateTime.Now,
            };
            if (survey.SurveyType == SurveyType.AddictionSurvey)
            {
                if (totalScore < 10)
                {
                    surveyResult.Recommendation = "No risk of addiction";
                }
                else if (totalScore < 20)
                {
                    surveyResult.Recommendation = "At risk, should attend prevention course";
                }
                else
                {
                    surveyResult.Recommendation = "High risk, should seek professional counseling";
                }
            }
            else if(survey.SurveyType == SurveyType.CourseTest)
            {
                if (totalScore >= 7)
                {
                    surveyResult.Recommendation = "Pass";
                }
                else
                {
                    surveyResult.Recommendation = "Fail";
                }
            }
            var createdSurveyResult = await _repository.CreateSurveyResultAsync(surveyResult);
            if (createdSurveyResult == null) return null;
            foreach(var answerDto in surveyAnswerDto.Answers)
            {
                var surveyAnswerResult = new SurveyAnswerResult
                {
                    AnswerId = answerDto.AnswerId,
                    SurveyResultId = createdSurveyResult.ResultId,
                    QuestionId = answerDto.QuestionId
                };
                var CreatedAnswerResult = await _repository.CreateSurveyAnswerResultAsync(surveyAnswerResult);
            }
            return createdSurveyResult;
        }

        public async Task<bool> UpdateSurveyAsync(SurveyUpdateWithQuesAndAnsDto surveyUpdateDto, int surveyId)
        {
            var survey=await _repository.GetByIdAsync(surveyId);
            if(survey==null) return false;
            bool hasResult =survey.SurveyResults!=null && survey.SurveyResults.Any();
            if (hasResult)
            {
                survey.Description = surveyUpdateDto.Description;
                survey.SurveyName = surveyUpdateDto.SurveyName;
                survey.UpdatedAt= DateTime.Now; 
                foreach (var questionDto in surveyUpdateDto.Questions)
                {
                    var question = survey.SurveyQuestions.FirstOrDefault(q=>q.QuestionId == questionDto.QuestionId);
                    if (question != null)
                    {
                        question.QuestionText = questionDto.QuestionText;
                        foreach (var answerDto in questionDto.AnswersDto)
                        {
                            var answer = question.SurveyAnswers.FirstOrDefault(a => a.AnswerId == answerDto.AnswerId);
                            if (answer != null)
                            {
                                answer.AnswerText = answerDto.AnswerText;
                                answer.IsCorrect = answerDto.IsCorrect;
                                answer.Score = answerDto.Score;
                            }

                        }
                    }
                }
            }
            return await _repository.UpdateAsync(survey);
        }

        public async Task<SurveyStatisticDto> GetSurveyStatisticAsync(int surveyId)
        {
            var survey=await _repository.GetByIdAsync(surveyId);
            if (survey == null) return null;
            var surveyResults = survey.SurveyResults;
            var scores = surveyResults.Select(s => s.TotalScore).ToList();
            var surveyStatistic = new SurveyStatisticDto
            {
                TotalSubmissions = surveyResults.Count,
                TotalUsers=surveyResults.Select(s=>s.UserId).Distinct().Count(),
                AverageScore = scores.Any()? scores.Average() : 0,
                HighestScore = scores.Any()? scores.Max() : 0,
                LowestScore = scores.Any() ? scores.Min() : 0,
            };
            if (survey.SurveyType == SurveyType.AddictionSurvey)
            {
                surveyStatistic.RiskLevel = new RiskLevelStatistic
                {
                    NoRisk = surveyResults.Count(s => s.TotalScore < 10),
                    MildRisk = surveyResults.Count(s => s.TotalScore >= 10 && s.TotalScore < 20),
                    HighRisk = surveyResults.Count(s => s.TotalScore >= 20)
                };
            }
            else if(survey.SurveyType == SurveyType.CourseTest)
            {
                surveyStatistic.Pass = surveyResults.Count(s => s.Recommendation == "Pass");
                surveyStatistic.Fail = surveyResults.Count(s => s.Recommendation == "Fail");
            }
            return surveyStatistic;

        }

        public async Task<List<SurveyResultDto>> GetUserSurveyResultAsync(int surveyId, string userId)
        {
            var surveyResults =await _repository.GetSurveyResultAsync(surveyId, userId);
            if (surveyResults == null || !surveyResults.Any()) return new List<SurveyResultDto>();
            var surveyResultDtos = surveyResults.Select(surveyResult => new SurveyResultDto
            {
                SurveyResultId = surveyResult.ResultId,
                SurveyId = surveyResult.SurveyId,
                SurveyName = surveyResult.Survey.SurveyName,
                ExcutedBy = surveyResult.User.UserName,
                SubmittedAt = surveyResult.TakeAt,
                TotalScore = surveyResult.TotalScore,
                Recommendation = surveyResult.Recommendation,
                Questions = surveyResult.Survey.SurveyQuestions.Select(q => new SurveyQuestionResultDto
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.QuestionText,
                    Answers = q.SurveyAnswers.Select(a => new SurveyAnswerResultDto
                    {
                        AnswerId = a.AnswerId,
                        AnswerText = a.AnswerText,
                        IsCorrect = a.IsCorrect,
                        Score = a.Score
                    }).ToList()

                }).ToList()
            }).ToList();
            return surveyResultDtos;
        }
    }
}
