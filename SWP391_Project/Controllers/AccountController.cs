using BusinessLayer.IService;
using BusinessLayer.Service;
using DataAccessLayer.Dto;
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
        public async Task<IActionResult> Register(RegisterDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _userService.RegisterAsync(model);
                if (result.Succeeded)
                {
                    return Ok("User created");
                }
                else return StatusCode(500, result.Errors);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
    }
}
