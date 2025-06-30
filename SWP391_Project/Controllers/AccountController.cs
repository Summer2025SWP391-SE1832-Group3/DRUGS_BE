using BusinessLayer.IService;
using BusinessLayer.Service;
using DataAccessLayer.Dto.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;

namespace SWP391_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false)] 
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IUserService userService, ILogger<AccountController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _userService.RegisterAsync(registerDto,null,"Member");
                if (result.Succeeded)
                {
                    return Ok(
                        new NewUserDto
                        {
                            UserName = registerDto.UserName,
                            Email = registerDto.Email,
                        }
                        );
                }
                return StatusCode(500, result.Errors);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return StatusCode(500, "An error occurred during registration");
            }
        }

        [HttpPost("admin/createAccount")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateAccount([FromBody]CreateAccountDto dto,[FromQuery]string role)
        {
            try
            {
                _logger.LogInformation("Admin attempting to create account. Role: {Role}", role);
                _logger.LogInformation("Current user: {UserName}, User ID: {UserId}", 
                    User.Identity?.Name, 
                    User.FindFirstValue(ClaimTypes.NameIdentifier));
                
                // Log user roles
                var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
                _logger.LogInformation("Current user roles: {Roles}", string.Join(", ", userRoles));
                
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Model state is invalid: {Errors}", 
                        string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                    return BadRequest(ModelState);
                }
                
                if (!new[] { "Manager", "Staff", "Consultant" }.Contains(role))
                {
                    _logger.LogWarning("Invalid role requested: {Role}", role);
                    return BadRequest("Invalid role!!");
                }
                var result = await _userService.CreateAccountAsync(dto, role);
                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        UserName = dto.UserName,
                        Role = role
                    });
                }
                
                if (result.Errors.Any())
                {
                    var errorMessages = result.Errors.Select(e => e.Description).ToList();
                    _logger.LogError("Failed to create account: {Errors}", string.Join(", ", errorMessages));
                    return StatusCode(500, new { Errors = errorMessages });
                }

                return StatusCode(500,result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during account creation");
                return StatusCode(500, new { Message = "An unexpected error occurred", ErrorDetails = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _userService.LoginAsync(loginDto);

                if (result == "Invalid username")
                {
                    return Unauthorized(new { Message = "Invalid username" });
                }
                if (result == "Invalid password")
                {
                    return Unauthorized(new { Message = "Invalid password" });
                }
                if (result == "Account is inactive")
                {
                    return Unauthorized(new { Message = "Account is inactive" });
                }

                return Ok(
                    new NewUserDto
                    {
                        UserName = loginDto.UserName,
                        Token = result,
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user login");
                return StatusCode(500, "An error occurred during login");
            }
        }

        [HttpGet("admin/all-account")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllNonAdminAccounts([FromQuery] string status = "")
        {
            try
            {
                var accounts = await _userService.GetAllNonAdminAccountsAsync(status);
                return Ok(accounts);    
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving accounts.");
            }
        }

        // --- ADMIN ACCOUNT MANAGEMENT ---
        [HttpPut("admin/update/{userId}")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = false)] 
        public async Task<IActionResult> AdminUpdate(string userId, [FromBody] RegisterDto dto, [FromQuery] string? newRole)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var profileDto = new UserProfileUpdateDto
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender
            };

            var result = await _userService.UpdateUserProfileAsync(userId, profileDto);
            if (!result.Succeeded) return StatusCode(500, result.Errors);

            var passwordResult = await _userService.UpdateUserPasswordAsync(userId, dto.Password);
            if (!passwordResult.Succeeded) return StatusCode(500, passwordResult.Errors);
            if (!string.IsNullOrEmpty(newRole))
            {
                var roleResult = await _userService.UpdateUserRoleAsync(userId, newRole);
                if (!roleResult.Succeeded) return StatusCode(500, roleResult.Errors);
            }
            return Ok(new { message = "User updated successfully" });
        }
        [HttpPost("deactivate/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeactivateUser(string userId)
        {
            try
            {
                var result = await _userService.DeactivateUserAsync(userId);

                if (result.Succeeded)
                {
                    return Ok(new { Message = "Account deactivated successfully." });
                }

                return StatusCode(500, new { Message = "Failed to deactivate user.", Errors = result.Errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred during user deactivation");
            }
        }

        [HttpPost("activate/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivateUser(string userId)
        {
            try
            {
                var result = await _userService.ActivateUserAsync(userId);
                if (result.Succeeded)
                {
                    return Ok(new { Message = "User account activated successfully." });
                }
                return StatusCode(500, new { Message = "Failed to activate user.", Errors = result.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user activation");
                return StatusCode(500, "An error occurred during user activation");
            }
        }

        [HttpDelete("admin/delete/{userId}")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = false)] 
        public async Task<IActionResult> AdminDelete(string userId)
        {
            try
            {
                
                if (await _userService.IsAdmin(userId))
                {
                    return BadRequest(new { message = "Cannot delete admin account" });
                }

                var result = await _userService.AdminDeleteUserAsync(userId);
                if (result.Succeeded)
                {
                    return Ok(new { message = "User deactivated successfully" });
                }
                return StatusCode(500, result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", userId);
                return StatusCode(500, "An error occurred while deleting user");
            }
        }

        [HttpGet("admin/search")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminSearch(
            [FromQuery] string? email,
            [FromQuery] string? username,
            [FromQuery] string? role)
        {
            try
            {
                var users = await _userService.AdminSearchUsersAsync(email, username, role);
                return Ok(users.Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.FullName,
                    u.Gender,
                    u.DateOfBirth,
                    u.PhoneNumber,
                    u.CreatedAt
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user search");
                return StatusCode(500, "An error occurred while searching users");
            }
        }

        // --- USER PROFILE MANAGEMENT (FOR ALL ROLES) ---
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { 
                        message = "Invalid input data", 
                        errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage) 
                    });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _userService.UpdateUserProfileAsync(userId, dto);
                
                if (result.Succeeded)
                {
                    return Ok(new { message = "Profile updated successfully" });
                }
                return StatusCode(500, result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile for user {UserId}", 
                    User.FindFirstValue(ClaimTypes.NameIdentifier));
                return StatusCode(500, "An error occurred while updating profile");
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { 
                        message = "Invalid input data", 
                        errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage) 
                    });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _userService.ChangePasswordAsync(userId, dto);
                
                if (result.Succeeded)
                {
                    return Ok(new { message = "Password changed successfully" });
                }
                return BadRequest(new { 
                    message = "Failed to change password", 
                    errors = result.Errors.Select(e => e.Description) 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user {UserId}", 
                    User.FindFirstValue(ClaimTypes.NameIdentifier));
                return StatusCode(500, "An error occurred while changing password");
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { 
                        message = "Invalid input data", 
                        errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage) 
                    });
                }

                var result = await _userService.ForgotPasswordAsync(dto);
                
                if (result.Succeeded)
                {
                    return Ok(new { message = "If the email exists, a password reset link has been sent" });
                }
                return BadRequest(new { 
                    message = "Failed to process forgot password request", 
                    errors = result.Errors.Select(e => e.Description) 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing forgot password request for email {Email}", dto.Email);
                return StatusCode(500, "An error occurred while processing forgot password request");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { 
                        message = "Invalid input data", 
                        errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage) 
                    });
                }

                var result = await _userService.ResetPasswordAsync(dto);
                
                if (result.Succeeded)
                {
                    return Ok(new { message = "Password reset successfully" });
                }
                return BadRequest(new { 
                    message = "Failed to reset password", 
                    errors = result.Errors.Select(e => e.Description) 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for email {Email}", dto.Email);
                return StatusCode(500, "An error occurred while resetting password");
            }
        }
    }
}
