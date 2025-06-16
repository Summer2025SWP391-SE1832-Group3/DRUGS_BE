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

        public AccountController(IUserService userService)
        {
            _userService = userService;
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
                else return StatusCode(500, result.Errors);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
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
            if (ModelState.IsValid)
            {
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
            return BadRequest("Invalid data");
        }

        // --- ADMIN ACCOUNT MANAGEMENT ---
        [HttpPut("admin/update/{userId}")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = false)] 
        public async Task<IActionResult> AdminUpdate(string userId, [FromBody] RegisterDto dto, [FromQuery] string? newRole)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.AdminUpdateUserAsync(userId, dto, newRole);
            if (result.Succeeded) return Ok(new { message = "User updated successfully" });
            return StatusCode(500, result.Errors);
        }

        [HttpDelete("admin/delete/{userId}")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = false)] 
        public async Task<IActionResult> AdminDelete(string userId)
        {
            var result = await _userService.AdminDeleteUserAsync(userId);
            if (result.Succeeded) return Ok(new { message = "User deleted successfully" });
            return StatusCode(500, result.Errors);
        }

        [HttpGet("admin/search")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = false)]
        public async Task<IActionResult> AdminSearch([FromQuery] string? email, [FromQuery] string? username, [FromQuery] string? role)
        {
            var users = await _userService.AdminSearchUsersAsync(email, username, role);
            return Ok(users.Select(u => new {
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
    }
}
