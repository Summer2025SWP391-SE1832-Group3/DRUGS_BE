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

        [HttpPost("createAccount")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateAccount([FromBody]RegisterDto dto,[FromQuery]string role)
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
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _userService.RegisterAsync(dto, currentUserId, role);
                if (result.Succeeded)
                {
                    return Ok(
                        new NewUserDto
                        {
                            UserName = dto.UserName,
                            Email = dto.Email,
                        }
                    );
                }
                if (result.Errors.Any())
                {
                    // Duyệt tất cả các lỗi và trả về thông báo phù hợp
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
    }
}
