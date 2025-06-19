using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public enum SurveyType
    {
        AddictionSurvey=1,
        CourseTest=2,
    }
    public class Survey
    {
        public int SurveyId { get; set; }
        [Required]
        [MaxLength(100)]
        public string SurveyName { get; set; } = null!;
        [MaxLength(1000)]
        public string? Description { get; set; }
        [Required]
        //[MaxLength(50)]
        public SurveyType SurveyType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public ICollection<SurveyResult> SurveyResults { get; set; }
        public ICollection<SurveyQuestion> SurveyQuestions { get; set; }

    }
}
