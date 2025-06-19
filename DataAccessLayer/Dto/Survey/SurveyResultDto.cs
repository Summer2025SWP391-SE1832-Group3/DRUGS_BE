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
        public int ResultId { get; set; }
        [Required]
        public string UserId { get; set; }
        public int SurveyId { get; set; }
        public List<SurveyAnswerResultDto> SurveyAnswerResults { get; set; } =new List<SurveyAnswerResultDto>();
    }
}
