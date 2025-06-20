using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Survey
{
    public class SurveyAnswerDto
    {
        public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();   
    }
    public class AnswerDto
    {
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
    }
}
