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
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
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

            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;
            user.DateOfBirth = dto.DateOfBirth;
            user.Gender = dto.Gender;

            if (!string.IsNullOrEmpty(dto.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
                if (!result.Succeeded)
                {
                    _logger.LogError("Password update failed for user {UserId}: {Errors}", 
                        userId, 
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                    return result;
                }
            }

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
    }
} 