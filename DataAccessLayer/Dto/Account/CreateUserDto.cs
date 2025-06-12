using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dto.Account
{
    public class CreateUserDto
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 4)]
        public string Gender { get; set; } = null!;
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public string Role { get; set; }
    }
}
