using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class SurveyAnswer
    {
        public int AnswerId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }
        [Required, MaxLength(255)]
        public string AnswerText { get; set; }
        public int? Score { get; set; }
        public bool? IsCorrect { get; set; }
        public ICollection<SurveyAnswerResult> SurveyAnswerResults { get; set; }

    }
}
