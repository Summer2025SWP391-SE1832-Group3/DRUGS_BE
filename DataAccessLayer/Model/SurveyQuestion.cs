using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class SurveyQuestion
    {
       public int QuestionId { get; set; }
        [Required]
        public int SurveyId { get; set; }
        public Survey Survey { get; set; }
        [Required, MaxLength(1000)]
        public string QuestionText { get; set; }
        //[Required, MaxLength(100)]  
        //public string AnswerType { get; set; }
        public ICollection<SurveyAnswer> SurveyAnswers { get; set; }
        public ICollection<SurveyAnswerResult> SurveyAnswerResults { get; set; }
    }
}
