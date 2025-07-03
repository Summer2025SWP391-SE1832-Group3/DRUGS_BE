using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Survey
{
    public class SurveyQuestionResultDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public List<SurveyAnswerResultDto> Answers { get; set; } = new List<SurveyAnswerResultDto>();
        public string UserAnswer { get; set; }
    }
}
