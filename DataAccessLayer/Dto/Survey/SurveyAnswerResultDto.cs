using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Survey
{
    public class SurveyAnswerResultDto
    {
        public int AnswerId { get; set; }
        public bool? IsCorrect { get; set; }
        public int? Score { get; set; }
    }
}
