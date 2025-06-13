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
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public ICollection<Blog> BlogsPosted { get; set; }
        public ICollection<Blog> BlogsApproved { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}