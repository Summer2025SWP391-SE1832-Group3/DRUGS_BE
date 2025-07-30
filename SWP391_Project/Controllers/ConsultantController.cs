using Microsoft.AspNetCore.Mvc;
using BusinessLayer.IService;
using DataAccessLayer.Dto.Account;
using DataAccessLayer.Dto.Consultation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;
using System.Linq;

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

        // GET: api/consultant/workinghours/by-date
        [HttpGet("workinghours/by-date")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> GetWorkingHoursByDate([FromQuery] DateTime date)
        {
            var consultantId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (consultantId == null) return Unauthorized();
            
            var hours = await _consultantService.GetWorkingHoursByDateAsync(consultantId, date);
            return Ok(hours);
        }

        // POST: api/consultant/workinghours/range
        [HttpPost("workinghours/range")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> AddWorkingHoursRange([FromBody] WorkingHourRangeDto dto)
        {
            var consultantId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (consultantId == null) return Unauthorized();
            
            try
            {
                // Kiểm tra trùng lịch cho tất cả các ngày trước khi thêm
                var conflictDates = await _consultantService.CheckScheduleConflictForDateRangeAsync(
                    consultantId, dto.FromDate, dto.ToDate, dto.StartTime.Value, dto.EndTime.Value);
                
                // Nếu có trùng lịch, trả về danh sách ngày bị trùng
                if (conflictDates.Any())
                {
                    return BadRequest(new { 
                        success = false, 
                        message = "Schedule conflicts found on the following dates", 
                        conflictDates = conflictDates.Select(d => d.ToString("dd/MM/yyyy")).ToList()
                    });
                }
                
                // Nếu không có trùng lịch, thực hiện thêm lịch
                var days = (dto.ToDate - dto.FromDate).Days;
                for (int i = 0; i <= days; i++)
                {
                    var date = dto.FromDate.AddDays(i);
                    await _consultantService.AddWorkingHourByDateAsync(consultantId, date, dto.StartTime, dto.EndTime);
                }
                return Ok(new { success = true, message = "Working hours created successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while creating working hours" });
            }
        }

        // PUT: api/consultant/workinghours/{date}
        [HttpPut("workinghours/{date}")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> UpdateWorkingHourByDate(DateTime date, [FromBody] WorkingHourUpdateDto dto)
        {
            var consultantId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (consultantId == null) return Unauthorized();
            
            try
            {
                // Kiểm tra trùng lịch trước khi cập nhật
                var hasConflict = await _consultantService.CheckScheduleConflictAsync(consultantId, date, dto.StartTime.Value, dto.EndTime.Value);
                if (hasConflict)
                {
                    return BadRequest(new { 
                        success = false, 
                        message = $"Date {date.ToString("dd/MM/yyyy")} has existing consultations or conflicts with current schedule" 
                    });
                }
                
                var result = await _consultantService.UpdateWorkingHourByDateAsync(consultantId, date, dto.StartTime, dto.EndTime);
                return Ok(new { success = true, message = "Working hours updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while updating working hours" });
            }
        }

        // GET: api/consultant/workinghours/check-conflict
        // [HttpGet("workinghours/check-conflict")]
        // [Authorize(Roles = "Consultant")]
        // public async Task<IActionResult> CheckScheduleConflict([FromQuery] DateTime date, [FromQuery] TimeSpan startTime, [FromQuery] TimeSpan endTime)
        // {
        //     var consultantId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        //     if (consultantId == null) return Unauthorized();
        //     
        //     try
        //     {
        //         var hasConflict = await _consultantService.CheckScheduleConflictAsync(consultantId, date, startTime, endTime);
        //         return Ok(new { 
        //             hasConflict = hasConflict, 
        //             message = hasConflict ? "This time slot has existing consultations or conflicts with current schedule" : "This time slot is available"
        //         });
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, new { success = false, message = "An error occurred while checking schedule conflicts" });
        //     }
        // }
    }

}