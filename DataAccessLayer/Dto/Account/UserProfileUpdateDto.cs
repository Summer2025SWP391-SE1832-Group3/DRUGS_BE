using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Dto.Account
{
    public class UserProfileUpdateDto
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name must be less than 100 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(6, MinimumLength = 4, ErrorMessage = "Gender must be between 4 and 6 characters")]
        public string Gender { get; set; }
    }
} 