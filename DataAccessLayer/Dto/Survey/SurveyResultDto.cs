using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Survey
{
    public class SurveyResultDto
    {
        public int SurveyResultId { get; set; }
        public int  SurveyId { get; set; }
        public string SurveyName { get; set; }
        public string ExcutedBy { get; set; }
        public DateTime SubmittedAt { get; set; }
        public int TotalScore { get; set; }
        public string Recommendation { get; set; } 
        public List<SurveyQuestionResultDto> Questions { get; set; } =new List<SurveyQuestionResultDto>();
    }
}
