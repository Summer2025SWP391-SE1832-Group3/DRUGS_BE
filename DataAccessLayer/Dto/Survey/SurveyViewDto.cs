using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Survey
{
    public class SurveyViewDto
    {
        public int SurveyId { get; set; }
        public string SurveyName { get; set; } = null!;     
        public string? Description { get; set; }    
        public bool IsActive { get; set; }
        public List<SurveyQuestionViewDto> SurveyQuestions { get; set; } = new List<SurveyQuestionViewDto>(); 
    }
}
