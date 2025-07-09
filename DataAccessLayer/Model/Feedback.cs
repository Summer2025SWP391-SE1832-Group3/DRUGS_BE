using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int CourseId { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; } 
        public string ReviewText { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public Course Course { get; set; }
        public ApplicationUser User { get; set; }


    }
}
