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

        //public async Task<SurveyAnswer> CreateAnswerAsync(SurveyAnswerCreateDto dto)
        //{
        //    var answer=_mapper.Map<SurveyAnswer>(dto);
        //    return await _repository.CreateAnswerAsync(answer);
        //}

        //public async Task<SurveyQuestion> CreateQuestionAsync(SurveyQuestionCreateDto dto)
        //{
        //    var question=_mapper.Map<SurveyQuestion>(dto);
        //    return await _repository.CreateQuestionAsync(question); 
        //}

        //public async Task<Survey> CreateSurveyAsync(SurveyCreateDto dto)
        //{
        //     var survey=_mapper.Map<Survey>(dto);
        //    return await _repository.CreateAsync(survey);
        //}

        public async Task<SurveyResult> CreateSurveyResultAsync(SurveyResultDto dto)
        {
            var result=_mapper.Map<SurveyResult>(dto);
            return await _repository.CreateSurveyResultAsync(result);
        }

        public async Task<bool> DeleteAnswerAsync(int answerId)
        {
            return await _repository.DeleteAnswerAsync(answerId);
        }

        public async Task<bool> DeleteSurveyAsync(int surveyId)
        {
            return await _repository.DeleteAsync(surveyId);
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

        public async Task<bool> UpdateAnswerAsync(SurveyAnswerUpdateDto dto)
        {
            var answer = await _repository.GetAnswerByIdAsync(dto.QuestionId) ;
            if (answer==null)
            {
                return false;
            }
            answer.AnswerText = dto.AnswerText;
            answer.IsCorrect = dto.IsCorrect;
            answer.Score = dto.Score;
            return await _repository.UpdateAnswerAsync(answer);
        }

        public async Task<bool> UpdateSurveyAsync(SurveyUpdateDto dto)
        {
            var survey=await _repository.GetByIdAsync(dto.SurveyId);
            if(survey == null) return false;
            _mapper.Map(dto, survey);
            return await _repository.UpdateAsync(survey);
        }

        public async Task<bool> UpdateQuestionAsync(SurveyQuestionUpdateDto dto)
        {
            var question=await _repository.GetQuestionByIdAsync(dto.QuestionId);
            if(question == null) return false;
            _mapper.Map(dto, question);
            return await _repository.UpdateQuestionAsync(question);
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
    }
}
