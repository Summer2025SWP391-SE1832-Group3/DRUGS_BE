using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Account
{
    public class AccountViewDto
    {
        public string UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; } 
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
    }
}
