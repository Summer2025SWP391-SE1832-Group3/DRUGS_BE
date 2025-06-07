using BusinessLayer.IService;
using BusinessLayer.Service;
using DataAccessLayer.Dto.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                var result = await _userService.RegisterAsync(registerDto);
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
