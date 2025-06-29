using BusinessLayer.IService;
using DataAccessLayer.Dto.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DataAccessLayer.Model;

namespace SWP391_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultantController : ControllerBase
    {
        private readonly IConsultantService _consultantService;
        public ConsultantController(IConsultantService consultantService)
        {
            _consultantService = consultantService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ConsultantViewDto>>> GetAllConsultants()
        {
            var consultants = await _consultantService.GetAllConsultantsAsync();
            return Ok(consultants);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ConsultantDetailDto>> GetConsultantDetail(string id)
        {
            var consultant = await _consultantService.GetConsultantDetailAsync(id);
            if (consultant == null)
                return NotFound();
            return Ok(consultant);
        }

        [HttpGet("{id}/certificates")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCertificates(string id)
        {
            var certs = await _consultantService.GetCertificatesAsync(id);
            return Ok(certs);
        }

        [HttpPost("add_certificates")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> AddCertificate( [FromBody] CertificateDto dto)
        {
            var consultantId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var result = await _consultantService.AddOrUpdateCertificateAsync(consultantId, dto);
            if (result) return Ok(new { Message = "Certificate added." });
            return BadRequest(new { Message = "Failed to add certificate." });
        }

        [HttpPut("{certificateId}/upd_certificates")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> UpdateCertificate(int certificateId, [FromBody] CertificateDto dto)
        {
            var consultantId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var result = await _consultantService.AddOrUpdateCertificateAsync(consultantId, dto, certificateId);
            if (result) return Ok(new { Message = "Certificate updated." });
            return BadRequest(new { Message = "Failed to update certificate." });
        }

        [HttpDelete("{certificateId}/del_certificates")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> DeleteCertificate( int certificateId)
        {
            var consultantId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var result = await _consultantService.DeleteCertificateAsync(consultantId, certificateId);
            if (result) return Ok(new { Message = "Certificate deleted." });
            return BadRequest(new { Message = "Failed to delete certificate." });
        }

        [HttpGet("{id}/workinghours")]
        [AllowAnonymous]
        public async Task<IActionResult> GetWorkingHours(string id)
        {
            var hours = await _consultantService.GetWorkingHoursAsync(id);
            return Ok(hours);
        }

        [HttpPost("add_workinghours")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> AddWorkingHour( [FromBody] ConsultantWorkingHourDto workingHourDto)
        {
            var consultantId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var workingHour = new ConsultantWorkingHour
            {
                DayOfWeek = workingHourDto.DayOfWeek,
                StartTime = workingHourDto.StartTime,
                EndTime = workingHourDto.EndTime
            };
            try
            {
                var result = await _consultantService.AddWorkingHourAsync(consultantId, workingHour);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{workingHourId}/upd_workinghours")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> UpdateWorkingHour( int workingHourId, [FromBody] ConsultantWorkingHourDto workingHourDto)
        {
            var consultantId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var workingHour = new ConsultantWorkingHour
            {
                DayOfWeek = workingHourDto.DayOfWeek,
                StartTime = workingHourDto.StartTime,
                EndTime = workingHourDto.EndTime
            };
            try
            {
                var result = await _consultantService.UpdateWorkingHourAsync(consultantId, workingHourId, workingHour);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{workingHourId}/del_workinghours")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> DeleteWorkingHour( int workingHourId)
        {
            var consultantId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var result = await _consultantService.DeleteWorkingHourAsync(consultantId, workingHourId);
            if (result) return Ok(new { Message = "Working hour deleted." });
            return NotFound(new { Message = "Working hour not found." });
        }
    }
} 