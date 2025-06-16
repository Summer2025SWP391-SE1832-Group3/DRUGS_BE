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

        // Các trường đổi mật khẩu (không bắt buộc)
        public string? CurrentPassword { get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "New password must be at least 8 characters")]
        public string? NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
} 