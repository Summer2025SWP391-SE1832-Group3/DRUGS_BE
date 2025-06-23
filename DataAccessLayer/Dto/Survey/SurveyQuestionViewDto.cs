using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Survey
{
    public class SurveyQuestionViewDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = null!;

        public List<SurveyQuestionResultDto> Questions { get; set; } = new List<SurveyQuestionResultDto>();
    }
}
