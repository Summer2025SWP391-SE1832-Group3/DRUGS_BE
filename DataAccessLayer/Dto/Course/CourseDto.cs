using DataAccessLayer.Dto.Lesson;
using DataAccessLayer.Dto.Survey;
using DataAccessLayer.Model;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Dto.Course
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public CourseTopic Topic { get; set; }
        public IEnumerable<LessonDto> Lessons { get; set; } 
        public SurveyViewDto FinalExamSurvey { get; set; }
    }
}