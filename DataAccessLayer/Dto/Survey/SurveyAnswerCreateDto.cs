using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Survey
{
    public class SurveyAnswerCreateDto
    {
        [Required, MaxLength(255)]
        public string AnswerText { get; set; }
        public int? Score { get; set; }
        public bool? IsCorrect { get; set; }
    }
}
