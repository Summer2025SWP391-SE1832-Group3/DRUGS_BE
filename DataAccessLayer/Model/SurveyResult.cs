using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public enum RiskLevel
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
    public class SurveyResult
    {
        public int ResultId { get; set; }
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public int SurveyId { get; set; }
        public Survey Survey { get; set; }
        [Required]
        public int TotalScore { get; set; }
        public RiskLevel? RiskLevel { get; set; } //Low,Med,Hight
        public string? ResultStatus { get; set; } //pass,fail
        [MaxLength(500)]
        public string? Recommendation { get; set; }
        public DateTime TakeAt { get; set; }
        
        public ICollection<SurveyAnswerResult> SurveyAnswerResults { get; set; }
    }
}
