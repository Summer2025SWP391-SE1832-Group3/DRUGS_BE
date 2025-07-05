using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Survey
{
    public class SurveyStatisticDto
    {
        public int TotalSubmissions { get; set; }
        public int TotalUsers { get; set; }
        public int HighestScore { get; set; }
        public double AverageScore { get; set; }
        public int LowestScore { get; set; }
        public RiskLevelStatistic? RiskLevel { get; set; }
        public int? Pass { get; set; } 
        public int? Fail { get; set; } 

    }
    public class RiskLevelStatistic 
    {
        public int NoRisk { get; set; }
        public int MildRisk { get; set; }
        public int HighRisk { get; set; }
    }
}
