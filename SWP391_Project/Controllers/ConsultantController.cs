using Microsoft.AspNetCore.Mvc;
using BusinessLayer.IService;
using DataAccessLayer.Dto.Account;
using DataAccessLayer.Dto.Consultation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;

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
        public async Task<ActionResult> GetAllConsultants([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _consultantService.GetAllConsultantsAsync();
            return Ok(result);
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

        // POST: api/consultant/workinghours/range
        [HttpPost("workinghours/range")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> AddWorkingHoursRange([FromBody] WorkingHourRangeDto dto)
        {
            var consultantId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (consultantId == null) return Unauthorized();
            var days = (dto.ToDate - dto.FromDate).Days;
            for (int i = 0; i <= days; i++)
            {
                var date = dto.FromDate.AddDays(i);
                await _consultantService.AddWorkingHourByDateAsync(consultantId, date, dto.StartTime, dto.EndTime);
            }
            return Ok(true);
        }

        // PUT: api/consultant/workinghours/{date}
        [HttpPut("workinghours/{date}")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> UpdateWorkingHourByDate(DateTime date, [FromBody] WorkingHourUpdateDto dto)
        {
            var consultantId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (consultantId == null) return Unauthorized();
            var result = await _consultantService.UpdateWorkingHourByDateAsync(consultantId, date, dto.StartTime, dto.EndTime);
            return Ok(result);
        }
    }

    public class WorkingHourRangeDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
    public class WorkingHourUpdateDto
    {
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}