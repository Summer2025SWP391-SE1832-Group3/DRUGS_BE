using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string? FullName { get; set; }
        [PersonalData]
        public DateTime? DateOfBirth { get; set; }
        [PersonalData]
        public string? Gender { get; set; }
        [PersonalData]
        public DateTime CreatedAt { get; set; } 
        [PersonalData]
        public string? Description { get; set; }
        public ICollection<Blog> BlogsPosted { get; set; }
        public ICollection<Blog> BlogsApproved { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<SurveyResult> SurveyResults { get; set; }
        public ICollection<ConsultationRequest> ConsultationRequests { get; set; }
        public ICollection<ConsultationRequest> ConsultationRequestsAsConsultant { get; set; }
        public ICollection<ConsultantWorkingHour> WorkingHours { get; set; }
        public ICollection<Certificate> Certificates { get; set; }
    }
}