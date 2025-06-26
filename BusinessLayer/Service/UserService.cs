using BusinessLayer.IService;
using DataAccessLayer.Dto.Account;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            IEmailService emailService,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<string?> LoginAsync(LoginDto loginDto)
        {
            _logger.LogInformation("Attempting login for user: {UserName}", loginDto.UserName);
            var user = await _userRepository.GetUserByUserName(loginDto.UserName);
            if (user == null)
            {
                _logger.LogWarning("Login failed: User not found - {UserName}", loginDto.UserName);
                return null;
            }
            var validPassword = await _userRepository.CheckPassword(user, loginDto.Password);
            if (!validPassword)
            {
                _logger.LogWarning("Login failed: Invalid password for user - {UserName}", loginDto.UserName);
                return null;
            }
            _logger.LogInformation("Login successful for user: {UserName}", loginDto.UserName);
            return await _tokenService.CreateToken(user);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto, string? role, string defaultRole)
        {
            _logger.LogInformation("Attempting to register new user: {Email}", registerDto.Email);
            
            var existingEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingEmail != null)
            {
                _logger.LogWarning("Registration failed: Email already exists - {Email}", registerDto.Email);
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "EmailAlreadyExists",
                    Description = "Email is already taken"
                });
            }

            var existingUserName = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existingUserName != null)
            {
                _logger.LogWarning("Registration failed: Username already exists - {UserName}", registerDto.UserName);
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNameAlreadyExists",
                    Description = "UserName is already taken"
                });
            }

            var result = await _userRepository.CreateUserAsyn(registerDto, role, defaultRole);
            if (result.Succeeded)
            {
                _logger.LogInformation("User registered successfully: {Email}", registerDto.Email);
            }
            else
            {
                _logger.LogError("Registration failed for user {Email}: {Errors}", 
                    registerDto.Email, 
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            return result;
        }

        public async Task<IdentityResult> AdminDeleteUserAsync(string userId)
        {
            _logger.LogInformation("Admin attempting to delete user: {UserId}", userId);
            
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Delete failed: User not found - {UserId}", userId);
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation("User deleted successfully: {UserId}", userId);
            }
            else
            {
                _logger.LogError("Delete failed for user {UserId}: {Errors}", 
                    userId, 
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            return result;
        }

        public async Task<bool> IsAdmin(string userId)
        {
            _logger.LogInformation("Checking if user is admin: {UserId}", userId);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return false;
            }
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            _logger.LogInformation("User {UserId} is admin: {IsAdmin}", userId, isAdmin);
            return isAdmin;
        }

        public async Task<IEnumerable<ApplicationUser>> AdminSearchUsersAsync(string? email, string? username, string? role)
        {
            _logger.LogInformation("Admin searching users with filters - Username: {Username}, Email: {Email}, Role: {Role}", 
                username, email, role);

            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(email))
                users = users.Where(u => u.Email.Contains(email));

            if (!string.IsNullOrEmpty(username))
                users = users.Where(u => u.UserName.Contains(username));

            var userList = users.ToList();

            if (!string.IsNullOrEmpty(role))
            {
                var usersInRole = new List<ApplicationUser>();
                foreach (var user in userList)
                {
                    if (await _userManager.IsInRoleAsync(user, role))
                        usersInRole.Add(user);
                }
                _logger.LogInformation("Found {Count} users matching search criteria", usersInRole.Count);
                return usersInRole;
            }

            _logger.LogInformation("Found {Count} users matching search criteria", userList.Count);
            return userList;
        }

        public async Task<IdentityResult> UpdateUserProfileAsync(string userId, UserProfileUpdateDto dto)
        {
            _logger.LogInformation("Updating profile for user: {UserId}", userId);
            
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Update failed: User not found - {UserId}", userId);
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            if (!string.IsNullOrWhiteSpace(dto.UserName) && dto.UserName != user.UserName)
            {
                var existingUserName = await _userManager.FindByNameAsync(dto.UserName);
                if (existingUserName != null)
                {
                    _logger.LogWarning("Update failed: Username already exists - {UserName}", dto.UserName);
                    return IdentityResult.Failed(new IdentityError { Description = "UserName is already taken" });
                }
                user.UserName = dto.UserName;
                user.NormalizedUserName = _userManager.NormalizeName(dto.UserName);
            }

            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;
            user.DateOfBirth = dto.DateOfBirth;
            user.Gender = dto.Gender;

            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                _logger.LogInformation("Profile updated successfully for user: {UserId}", userId);
            }
            else
            {
                _logger.LogError("Profile update failed for user {UserId}: {Errors}", 
                    userId, 
                    string.Join(", ", updateResult.Errors.Select(e => e.Description)));
            }
            return updateResult;
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDto dto)
        {
            _logger.LogInformation("Changing password for user: {UserId}", userId);
            
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Change password failed: User not found - {UserId}", userId);
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("Password changed successfully for user: {UserId}", userId);
            }
            else
            {
                _logger.LogError("Password change failed for user {UserId}: {Errors}", 
                    userId, 
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            return result;
        }

        public async Task<IdentityResult> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            _logger.LogInformation("Processing forgot password request for email: {Email}", dto.Email);
            
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                _logger.LogWarning("Forgot password failed: User not found with email - {Email}", dto.Email);
                // Return success to prevent email enumeration attacks
                return IdentityResult.Success;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            // Generate reset URL (in production, this should be configured)
            var resetUrl = $"https://your-frontend-app.com/reset-password?token={token}&email={Uri.EscapeDataString(dto.Email)}";
            
            var emailSent = await _emailService.SendPasswordResetEmailAsync(dto.Email, token, resetUrl);
            if (!emailSent)
            {
                _logger.LogError("Failed to send password reset email to {Email}", dto.Email);
                return IdentityResult.Failed(new IdentityError { Description = "Failed to send reset email" });
            }
            
            _logger.LogInformation("Password reset email sent successfully to {Email}", dto.Email);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto dto)
        {
            _logger.LogInformation("Processing password reset for email: {Email}", dto.Email);
            
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                _logger.LogWarning("Reset password failed: User not found with email - {Email}", dto.Email);
                return IdentityResult.Failed(new IdentityError { Description = "Invalid email or token" });
            }

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("Password reset successfully for user: {UserId}", user.Id);
            }
            else
            {
                _logger.LogError("Password reset failed for user {UserId}: {Errors}", 
                    user.Id, 
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            return result;
        }
    }
} 