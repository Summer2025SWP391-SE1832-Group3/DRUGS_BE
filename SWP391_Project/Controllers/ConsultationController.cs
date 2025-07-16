using Microsoft.AspNetCore.Mvc;
using BusinessLayer.IService;
using DataAccessLayer.Dto.Consultation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace SWP391_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationController : ControllerBase
    {
        private readonly IConsultationService _consultationService;
        private readonly ApplicationDBContext _context;
        public ConsultationController(IConsultationService consultationService, ApplicationDBContext context)
        {
            _consultationService = consultationService;
            _context = context;
        }

        // GET: api/consultant/{id}/available-slots
        [HttpGet("/api/consultant/{id}/available-slots")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableSlots(string id, [FromQuery] DateTime date)
        {
            var slots = await _consultationService.GetAvailableSlotsAsync(id, date);
            return Ok(slots);
        }

        // POST: api/consultation/booking
        [HttpPost("booking")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> BookConsultation([FromBody] ConsultationBookingRequestDto dto)
        {
            var memberId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (memberId == null) return Unauthorized();
            var result = await _consultationService.BookConsultationAsync(memberId, dto.ConsultantId, dto.SlotId);
            return Ok(result);
        }

        // GET: api/consultation/my-bookings
        [HttpGet("my-bookings")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> GetMyBookings()
        {
            var memberId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (memberId == null) return Unauthorized();
            var bookings = await _consultationService.GetMyBookingsAsync(memberId);
            return Ok(bookings);
        }

        // POST: api/consultation/{id}/feedback
        [HttpPost("{id}/feedback")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> FeedbackConsultant(int id, [FromBody] ConsultationFeedbackDto dto)
        {
            var memberId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (memberId == null) return Unauthorized();
            var result = await _consultationService.FeedbackConsultantAsync(memberId, id, dto);
            return Ok(result);
        }

        // PUT: api/consultation/{id}/confirm
        [HttpPut("/api/consultation/{id}/confirm")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> ConfirmConsultation(int id)
        {
            var consultantId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (consultantId == null) return Unauthorized();
            var result = await _consultationService.ConfirmConsultationAsync(consultantId, id);
            if (!result) return BadRequest("Cannot confirm this consultation.");
            return Ok(true);
        }

        // PUT: api/consultation/{id}/reject
        [HttpPut("/api/consultation/{id}/reject")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> RejectConsultation(int id)
        {
            var consultantId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (consultantId == null) return Unauthorized();
            var result = await _consultationService.RejectConsultationAsync(consultantId, id);
            if (!result) return BadRequest("Cannot reject this consultation.");
            return Ok(true);
        }

        // GET: api/consultation/my-requests
        [HttpGet("my-requests")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> GetMyConsultationRequests()
        {
            var consultantId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (consultantId == null) return Unauthorized();

            var requests = await _context.ConsultationRequests
                .Where(r => r.ConsultantId == consultantId)
                .OrderByDescending(r => r.RequestedDate)
                .ToListAsync();

            var result = requests.Select(r => new ConsultationBookingDto
            {
                Id = r.Id,
                ConsultantId = r.ConsultantId,
                MemberId = r.UserId,
                StartTime = r.RequestedDate,
                EndTime = r.RequestedDate.AddMinutes(r.DurationMinutes),
                Status = r.Status.ToString()
            });

            return Ok(result);
        }
    }
} 