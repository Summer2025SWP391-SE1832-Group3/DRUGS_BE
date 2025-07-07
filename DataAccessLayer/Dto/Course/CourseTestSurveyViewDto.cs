using System;

namespace DataAccessLayer.Dto.Course
{
    public class CourseTestSurveyViewDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int SurveyId { get; set; }
        public string SurveyName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 