using Microsoft.AspNetCore.Mvc;
using BusinessLayer.IService;
using DataAccessLayer.Dto.Account;
using DataAccessLayer.Dto.Consultation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

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

        // GET: api/consultant
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllConsultants()
        {
            var consultants = await _consultantService.GetAllConsultantsAsync();
            return Ok(consultants);
        }

        // GET: api/consultant/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetConsultantDetail(string id)
        {
            var consultant = await _consultantService.GetConsultantDetailAsync(id);
            if (consultant == null) return NotFound();
            return Ok(consultant);
        }

        // PUT: api/consultant/profile
        [HttpPut("profile")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> UpdateProfile([FromBody] ConsultantProfileUpdateDto dto)
        {
            var consultantId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (consultantId == null) return Unauthorized();
            var result = await _consultantService.UpdateProfileAsync(consultantId, dto);
            return Ok(result);
        }

        // GET: api/consultant/{id}/certificates
        [HttpGet("{id}/certificates")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCertificates(string id)
        {
            var certs = await _consultantService.GetCertificatesAsync(id);
            return Ok(certs);
        }

        // POST: api/consultant/certificates
        [HttpPost("certificates")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> AddCertificate([FromBody] CertificateDto dto)
        {
            var consultantId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (consultantId == null) return Unauthorized();
            var result = await _consultantService.AddCertificateAsync(consultantId, dto);
            return Ok(result);
        }

        // PUT: api/consultant/certificates/{certificateId}
        [HttpPut("certificates/{certificateId}")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> UpdateCertificate(int certificateId, [FromBody] CertificateDto dto)
        {
            var consultantId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (consultantId == null) return Unauthorized();
            var result = await _consultantService.UpdateCertificateAsync(consultantId, certificateId, dto);
            return Ok(result);
        }

        // DELETE: api/consultant/certificates/{certificateId}
        [HttpDelete("certificates/{certificateId}")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> DeleteCertificate(int certificateId)
        {
            var consultantId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (consultantId == null) return Unauthorized();
            var result = await _consultantService.DeleteCertificateAsync(consultantId, certificateId);
            return Ok(result);
        }

        // GET: api/consultant/{id}/workinghours
        [HttpGet("{id}/workinghours")]
        [AllowAnonymous]
        public async Task<IActionResult> GetWorkingHours(string id)
        {
            var hours = await _consultantService.GetWorkingHoursAsync(id);
            return Ok(hours);
        }

        // POST: api/consultant/workinghours
        [HttpPost("workinghours")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> AddWorkingHour([FromBody] ConsultantWorkingHourDto dto)
        {
            var consultantId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (consultantId == null) return Unauthorized();
            var result = await _consultantService.AddWorkingHourAsync(consultantId, dto);
            return Ok(result);
        }

        // PUT: api/consultant/workinghours/{workingHourId}
        [HttpPut("workinghours/{workingHourId}")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> UpdateWorkingHour(int workingHourId, [FromBody] ConsultantWorkingHourDto dto)
        {
            var consultantId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (consultantId == null) return Unauthorized();
            var result = await _consultantService.UpdateWorkingHourAsync(consultantId, workingHourId, dto);
            return Ok(result);
        }

        // DELETE: api/consultant/workinghours/{workingHourId}
        [HttpDelete("workinghours/{workingHourId}")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> DeleteWorkingHour(int workingHourId)
        {
            var consultantId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (consultantId == null) return Unauthorized();
            var result = await _consultantService.DeleteWorkingHourAsync(consultantId, workingHourId);
            return Ok(result);
        }
    }
}