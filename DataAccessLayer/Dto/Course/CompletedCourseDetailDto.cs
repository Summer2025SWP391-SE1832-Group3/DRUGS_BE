using DataAccessLayer.Dto.Lesson;
using DataAccessLayer.Dto.Survey;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Course
{
    public class CompletedCourseDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public CourseTopic Topic { get; set; }
        public IEnumerable<LessonDto> Lessons { get; set; }
        public SurveyViewDto FinalExamSurvey { get; set; }
        public double Progress { get; set; }         
        public DateTime? CompletedDate { get; set; }  
        public double? FinalScore { get; set; }      
        public string? CertificateUrl { get; set; }  
    }
}