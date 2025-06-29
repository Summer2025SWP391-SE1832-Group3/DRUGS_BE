using AutoMapper;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService,
            IMapper mapper,
            IEmailService emailService,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _logger = logger;
            _mapper = mapper;
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

        public async Task<IdentityResult> CreateAccountAsync(CreateAccountDto res, string role)
        {
            var existingUserName = await _userManager.FindByNameAsync(res.UserName);
            if (existingUserName != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNameAlreadyExists",
                    Description = "UserName is already taken"
                });
            }

            var result = await _userRepository.CreateAsync(res, role);
            return result; ;
        }
        public async Task<IdentityResult> RegisterAsync(RegisterDto res, string currentUserId, string role = "Member")
        {
            var existingEmail = await _userManager.FindByEmailAsync(res.Email);
            if (existingEmail != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "EmailAlreadyExists",
                    Description = "Email is already taken"
                });
            }

            var existingUserName = await _userManager.FindByNameAsync(res.UserName);
            if (existingUserName != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNameAlreadyExists",
                    Description = "UserName is already taken"
                });
            }

            var result = await _userRepository.RegisterAsync(res, currentUserId, role);
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
            else
            {
                _logger.LogInformation("User found: {UserId}", userId);
            }
            var result = await _userManager.DeleteAsync(user);
            if (result == null)
            {
                _logger.LogError("DeleteAsync returned null for user {UserId}", userId);
            }
            else if (!result.Succeeded)
            {
                _logger.LogError("Delete failed for user {UserId}: {Errors}",
                    userId,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            else
            {
                _logger.LogInformation("User deleted successfully: {UserId}", userId);
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
            user.Email = dto.Email;
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

        public async Task<List<AccountViewDto>> GetAllNonAdminAccountsAsync()
        {
            var users = _userManager.Users.ToList();
            var result = new List<AccountViewDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();
                if (role != "Admin")
                {
                    var dto = _mapper.Map<AccountViewDto>(user);
                    dto.Role = role;
                    result.Add(dto);
                }
            }

            return result.OrderBy(r=>r.Role).ThenBy(r=>r.UserName).ToList();
        }

        public async Task<IdentityResult> UpdateUserPasswordAsync(string userId, string newPassword)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Update failed: User not found - {UserId}", userId);
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (resetResult.Succeeded)
            {
                _logger.LogInformation("Password updated successfully for user: {UserId}", userId);
            }
            else
            {
                _logger.LogError("Password update failed for user {UserId}: {Errors}", userId, string.Join(", ", resetResult.Errors.Select(e => e.Description)));
            }

            return resetResult;
        }

        public async Task<IdentityResult> UpdateUserRoleAsync(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Update failed: User not found - {UserId}", userId);
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }
            var roleExists = await _roleManager.RoleExistsAsync(newRole);
            if (!roleExists)
            {
                _logger.LogWarning("Update failed: Role does not exist - {RoleName}", newRole);
                return IdentityResult.Failed(new IdentityError { Description = "Role does not exist" });
            }
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Contains(newRole))
            {
                return IdentityResult.Success;  
            }
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Failed to remove old roles" });
            }

            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            if (addResult.Succeeded)
            {
                _logger.LogInformation("Role updated successfully for user: {UserId}, new role: {RoleName}", userId, newRole);
            }
            else
            {
                _logger.LogError("Failed to add new role for user: {UserId}, role: {RoleName}", userId, newRole);
            }

            return addResult;
        }
    }
} 