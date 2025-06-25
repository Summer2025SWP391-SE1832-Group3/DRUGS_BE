using BusinessLayer.IService;
using BusinessLayer.Service;
using DataAccessLayer.Dto.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (!new[] { "Manager", "Staff", "Consultant" }.Contains(role))
                {
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
                    return StatusCode(500, new { Errors = errorMessages });
                }

                return StatusCode(500,result.Errors);
            }
            catch (Exception ex)
            {
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
                var token = await _userService.LoginAsync(loginDto);
                if (token == null)
                {
                    return Unauthorized("Invalid username or password");
                }
                return Ok(
                    new NewUserDto
                    {
                        UserName=loginDto.UserName,
                        Token=token,
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
        public async Task<IActionResult> GetAllNonAdminAccounts()
        {
            try
            {
                var accounts = await _userService.GetAllNonAdminAccountsAsync();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching non-admin accounts");
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
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender
            };

            
            var result = await _userService.UpdateUserProfileAsync(userId, profileDto);
            if (!result.Succeeded) return StatusCode(500, result.Errors);

            if (!string.IsNullOrEmpty(newRole))
            {

            }

            return Ok(new { message = "User updated successfully" });
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
    }
}
