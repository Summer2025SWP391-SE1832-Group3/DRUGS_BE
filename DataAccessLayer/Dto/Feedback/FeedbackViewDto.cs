using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Feedback
{
    public class FeedbackViewDto
    {
        public int FeedbackId { get; set; }
        public string CreateBy { get; set; }
        public string CourseTitle { get; set; } 
        public int Rating { get; set; } 
        public string ReviewText { get; set; } 
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
