using System;

namespace DataAccessLayer.Dto.Course
{
    public class CourseTestResultDto
    {
        public int SurveyResultId { get; set; }
        public int SurveyId { get; set; }
        public string SurveyName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime SubmittedAt { get; set; }
        public int TotalScore { get; set; }
        public string Recommendation { get; set; }
        public string ResultStatus { get; set; }
    }
} 