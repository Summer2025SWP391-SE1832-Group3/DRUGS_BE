using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Survey
{
    public class SurveyQuestionUpdateDto
    {
        [Required]
        public int QuestionId { get; set; }
        [Required, MaxLength(250)]
        public string QuestionText { get; set; } = null!;
        [Required]
        public List<SurveyAnswerUpdateDto> AnswersDto { get; set; } = new List<SurveyAnswerUpdateDto>();
    }
}
