using System.Collections.Generic;
using DataAccessLayer.Dto.Survey;

namespace DataAccessLayer.Dto.Course
{
    public class CourseTestSubmitDto
    {
        public int SurveyId { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }
} 