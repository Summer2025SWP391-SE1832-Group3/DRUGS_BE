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
        //[Required, MaxLength(100)]  
        //public string AnswerType { get; set; }
        [Required, MaxLength(1000)]
        public string QuestionText { get; set; } = null!;
    }
}
