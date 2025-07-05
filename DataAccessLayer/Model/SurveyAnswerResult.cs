using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class SurveyAnswerResult
    {
        public int SurveyResultId { get; set; }
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }

        public SurveyResult SurveyResult { get; set; }
        public SurveyAnswer SurveyAnswer { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }
    }
}
