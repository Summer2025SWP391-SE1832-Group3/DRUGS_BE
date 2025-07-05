using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Survey
{
    public class SurveyQuestionCreateDto
    {
        [Required]
        public string QuestionText { get; set; } = null!;
        [Required]
        public List<SurveyAnswerCreateDto> AnswersDto { get;set; } = new List<SurveyAnswerCreateDto>();

    }
}
