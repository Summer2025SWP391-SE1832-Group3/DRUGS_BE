using Microsoft.AspNetCore.Mvc;
using BusinessLayer.IService;
using DataAccessLayer.Dto.Consultation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SWP391_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationController : ControllerBase
    {
        private readonly IConsultationService _consultationService;
        public ConsultationController(IConsultationService consultationService)
        {
            _consultationService = consultationService;
        }

        // GET: api/consultant/{id}/available-slots
        [HttpGet("/api/consultant/{id}/available-slots")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableSlots(string id, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var slots = await _consultationService.GetAvailableSlotsAsync(id, from, to);
            return Ok(slots);
        }

        // POST: api/consultation/booking
        [HttpPost("booking")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> BookConsultation([FromBody] ConsultationBookingDto dto)
        {
            var memberId = User.Identity?.Name;
            if (memberId == null) return Unauthorized();
            var result = await _consultationService.BookConsultationAsync(memberId, dto.ConsultantId, dto);
            return Ok(result);
        }

        // GET: api/consultation/my-bookings
        [HttpGet("my-bookings")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> GetMyBookings()
        {
            var memberId = User.Identity?.Name;
            if (memberId == null) return Unauthorized();
            var bookings = await _consultationService.GetMyBookingsAsync(memberId);
            return Ok(bookings);
        }

        // POST: api/consultation/{id}/feedback
        [HttpPost("{id}/feedback")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> FeedbackConsultant(int id, [FromBody] ConsultationFeedbackDto dto)
        {
            var memberId = User.Identity?.Name;
            if (memberId == null) return Unauthorized();
            var result = await _consultationService.FeedbackConsultantAsync(memberId, id, dto);
            return Ok(result);
        }

        // PUT: api/consultation/{id}/confirm
        [HttpPut("{id}/confirm")]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> ConfirmConsultation(int id)
        {
            var consultantId = User.Identity?.Name;
            if (consultantId == null) return Unauthorized();
            var result = await _consultationService.ConfirmConsultationAsync(consultantId, id);
            return Ok(result);
        }
    }
} 