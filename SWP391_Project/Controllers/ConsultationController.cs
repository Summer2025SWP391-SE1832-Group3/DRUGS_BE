using BusinessLayer.IService;
using DataAccessLayer.Dto.Consultation;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SWP391_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationController : ControllerBase
    {
        private readonly IConsultationService _consultationService;
        private readonly ILogger<ConsultationController> _logger;

        public ConsultationController(IConsultationService consultationService, ILogger<ConsultationController> logger)
        {
            _consultationService = consultationService;
            _logger = logger;
        }

        // === CONSULTATION REQUEST ENDPOINTS ===

        [HttpPost("requests")]
        [Authorize]
        public async Task<ActionResult<ConsultationRequestViewDto>> CreateConsultationRequest([FromBody] ConsultationRequestCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Additional validation
                if (string.IsNullOrWhiteSpace(dto.Title))
                {
                    return BadRequest(new { message = "Title is required" });
                }

                if (string.IsNullOrWhiteSpace(dto.Description))
                {
                    return BadRequest(new { message = "Description is required" });
                }

                if (string.IsNullOrWhiteSpace(dto.Category))
                {
                    return BadRequest(new { message = "Category is required" });
                }

                if (string.IsNullOrWhiteSpace(dto.ConsultantId))
                {
                    return BadRequest(new { message = "Consultant ID is required" });
                }

                if (dto.RequestedDate < DateTime.Today)
                {
                    return BadRequest(new { message = "Requested date cannot be in the past. Please select today or a future date." });
                }

                if (dto.DurationMinutes < 30 || dto.DurationMinutes > 180)
                {
                    return BadRequest(new { message = "Duration must be between 30 and 180 minutes" });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var result = await _consultationService.CreateConsultationRequestAsync(dto, userId);
                
                _logger.LogInformation("Consultation request created successfully. RequestId: {RequestId}, UserId: {UserId}", result.Id, userId);
                return CreatedAtAction(nameof(GetConsultationRequest), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Failed to create consultation request: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating consultation request");
                return StatusCode(500, new { message = "An error occurred while creating the consultation request" });
            }
        }

        [HttpGet("requests/{id}")]
        [Authorize]
        public async Task<ActionResult<ConsultationRequestViewDto>> GetConsultationRequest(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _consultationService.GetConsultationRequestByIdAsync(id, userId);
                
                if (result == null)
                    return NotFound(new { message = "Consultation request not found" });
                
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access to consultation request {RequestId}: {Message}", id, ex.Message);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving consultation request {RequestId}", id);
                return StatusCode(500, new { message = "An error occurred while retrieving the consultation request" });
            }
        }

        [HttpGet("requests/my")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ConsultationRequestViewDto>>> GetMyConsultationRequests()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _consultationService.GetMyConsultationRequestsAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving consultation requests for user {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
                return StatusCode(500, new { message = "An error occurred while retrieving consultation requests" });
            }
        }

        [HttpGet("requests/consultant")]
        [Authorize(Roles = "Consultant")]
        public async Task<ActionResult<IEnumerable<ConsultationRequestViewDto>>> GetConsultationRequestsForConsultant()
        {
            try
            {
                var consultantId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _consultationService.GetConsultationRequestsForConsultantAsync(consultantId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Failed to get consultation requests for consultant: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving consultation requests for consultant {ConsultantId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
                return StatusCode(500, new { message = "An error occurred while retrieving consultation requests" });
            }
        }

        [HttpGet("requests")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ConsultationRequestViewDto>>> GetAllConsultationRequests()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _consultationService.GetAllConsultationRequestsAsync(userId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access to all consultation requests: {Message}", ex.Message);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all consultation requests");
                return StatusCode(500, new { message = "An error occurred while retrieving consultation requests" });
            }
        }

        [HttpPut("requests/{id}/status")]
        [Authorize(Roles = "Consultant,Admin")]
        public async Task<ActionResult<ConsultationRequestViewDto>> UpdateConsultationRequestStatus(int id, [FromBody] ConsultationRequestUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _consultationService.UpdateConsultationRequestStatusAsync(id, dto, userId);
                
                _logger.LogInformation("Consultation request status updated. RequestId: {RequestId}, NewStatus: {Status}, UpdatedBy: {UserId}", 
                    id, dto.Status, userId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Failed to update consultation request status: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access to update consultation request {RequestId}: {Message}", id, ex.Message);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating consultation request status {RequestId}", id);
                return StatusCode(500, new { message = "An error occurred while updating the consultation request" });
            }
        }

        [HttpDelete("requests/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteConsultationRequest(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _consultationService.DeleteConsultationRequestAsync(id, userId);
                
                if (!result)
                    return NotFound(new { message = "Consultation request not found" });
                
                _logger.LogInformation("Consultation request deleted. RequestId: {RequestId}, DeletedBy: {UserId}", id, userId);
                return Ok(new { message = "Consultation request deleted successfully" });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access to delete consultation request {RequestId}: {Message}", id, ex.Message);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting consultation request {RequestId}", id);
                return StatusCode(500, new { message = "An error occurred while deleting the consultation request" });
            }
        }

        // === CONSULTATION SESSION ENDPOINTS ===

        [HttpPost("requests/{requestId}/sessions")]
        [Authorize(Roles = "Consultant")]
        public async Task<ActionResult<ConsultationSessionViewDto>> CreateConsultationSession(int requestId, [FromBody] ConsultationSessionCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _consultationService.CreateConsultationSessionAsync(requestId, dto, userId);
                
                _logger.LogInformation("Consultation session created. RequestId: {RequestId}, SessionId: {SessionId}, ConsultantId: {ConsultantId}", 
                    requestId, result.Id, userId);
                return CreatedAtAction(nameof(GetConsultationSession), new { requestId }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Failed to create consultation session: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access to create consultation session: {Message}", ex.Message);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating consultation session for request {RequestId}", requestId);
                return StatusCode(500, new { message = "An error occurred while creating the consultation session" });
            }
        }

        [HttpGet("requests/{requestId}/sessions")]
        [Authorize]
        public async Task<ActionResult<ConsultationSessionViewDto>> GetConsultationSession(int requestId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _consultationService.GetConsultationSessionAsync(requestId, userId);
                
                if (result == null)
                    return NotFound(new { message = "Consultation session not found" });
                
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access to consultation session: {Message}", ex.Message);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving consultation session for request {RequestId}", requestId);
                return StatusCode(500, new { message = "An error occurred while retrieving the consultation session" });
            }
        }

        [HttpPut("sessions/{sessionId}")]
        [Authorize(Roles = "Consultant")]
        public async Task<ActionResult<ConsultationSessionViewDto>> UpdateConsultationSession(int sessionId, [FromBody] ConsultationSessionCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _consultationService.UpdateConsultationSessionAsync(sessionId, dto, userId);
                
                _logger.LogInformation("Consultation session updated. SessionId: {SessionId}, ConsultantId: {ConsultantId}", sessionId, userId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Failed to update consultation session: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access to update consultation session: {Message}", ex.Message);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating consultation session {SessionId}", sessionId);
                return StatusCode(500, new { message = "An error occurred while updating the consultation session" });
            }
        }

        [HttpPost("sessions/{sessionId}/complete")]
        [Authorize(Roles = "Consultant")]
        public async Task<ActionResult<ConsultationSessionViewDto>> CompleteConsultationSession(int sessionId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _consultationService.CompleteConsultationSessionAsync(sessionId, userId);
                
                _logger.LogInformation("Consultation session completed. SessionId: {SessionId}, ConsultantId: {ConsultantId}", sessionId, userId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Failed to complete consultation session: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access to complete consultation session: {Message}", ex.Message);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing consultation session {SessionId}", sessionId);
                return StatusCode(500, new { message = "An error occurred while completing the consultation session" });
            }
        }

        // === CONSULTATION REVIEW ENDPOINTS ===

        [HttpPost("sessions/{sessionId}/reviews")]
        [Authorize]
        public async Task<ActionResult<ConsultationReviewViewDto>> CreateConsultationReview(int sessionId, [FromBody] ConsultationReviewCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _consultationService.CreateConsultationReviewAsync(sessionId, dto, userId);
                
                _logger.LogInformation("Consultation review created. SessionId: {SessionId}, ReviewId: {ReviewId}, UserId: {UserId}", 
                    sessionId, result.Id, userId);
                return CreatedAtAction(nameof(GetConsultationReview), new { sessionId }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Failed to create consultation review: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access to create consultation review: {Message}", ex.Message);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating consultation review for session {SessionId}", sessionId);
                return StatusCode(500, new { message = "An error occurred while creating the consultation review" });
            }
        }

        [HttpGet("sessions/{sessionId}/reviews")]
        [Authorize]
        public async Task<ActionResult<ConsultationReviewViewDto>> GetConsultationReview(int sessionId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _consultationService.GetConsultationReviewAsync(sessionId, userId);
                
                if (result == null)
                    return NotFound(new { message = "Consultation review not found" });
                
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access to consultation review: {Message}", ex.Message);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving consultation review for session {SessionId}", sessionId);
                return StatusCode(500, new { message = "An error occurred while retrieving the consultation review" });
            }
        }

        [HttpGet("consultants/{consultantId}/reviews")]
        [Authorize(Roles = "Consultant")]
        public async Task<ActionResult<IEnumerable<ConsultationReviewViewDto>>> GetConsultantReviews(string consultantId)
        {
            try
            {
                var result = await _consultationService.GetConsultantReviewsAsync(consultantId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Failed to get consultant reviews: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving reviews for consultant {ConsultantId}", consultantId);
                return StatusCode(500, new { message = "An error occurred while retrieving consultant reviews" });
            }
        }

        // === SEARCH ENDPOINTS ===

        [HttpGet("requests/search")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ConsultationRequestViewDto>>> SearchConsultationRequests(
            [FromQuery] string? userId = null,
            [FromQuery] string? consultantId = null,
            [FromQuery] ConsultationStatus? status = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _consultationService.SearchConsultationRequestsAsync(
                    currentUserId, userId, consultantId, status, fromDate, toDate);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access to search consultation requests: {Message}", ex.Message);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching consultation requests");
                return StatusCode(500, new { message = "An error occurred while searching consultation requests" });
            }
        }

        // Pagination endpoints
        [HttpGet("requests/my/paginated")]
        [Authorize]
        public async Task<ActionResult> GetPaginatedMyConsultationRequests(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] ConsultationStatus? status = null)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _consultationService.GetPaginatedMyConsultationRequestsAsync(userId, page, pageSize, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paginated my consultation requests");
                return StatusCode(500, new { message = "An error occurred while retrieving consultation requests" });
            }
        }

        [HttpGet("requests/consultant/paginated")]
        [Authorize(Roles = "Consultant")]
        public async Task<ActionResult> GetPaginatedConsultationRequestsForConsultant(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] ConsultationStatus? status = null)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var consultantId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _consultationService.GetPaginatedConsultationRequestsForConsultantAsync(consultantId, page, pageSize, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paginated consultation requests for consultant");
                return StatusCode(500, new { message = "An error occurred while retrieving consultation requests" });
            }
        }

        [HttpGet("requests/paginated")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetPaginatedAllConsultationRequests(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? userId = null,
            [FromQuery] string? consultantId = null,
            [FromQuery] ConsultationStatus? status = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _consultationService.GetPaginatedAllConsultationRequestsAsync(
                    currentUserId, page, pageSize, userId, consultantId, status, fromDate, toDate);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access to paginated consultation requests: {Message}", ex.Message);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paginated all consultation requests");
                return StatusCode(500, new { message = "An error occurred while retrieving consultation requests" });
            }
        }

        [HttpGet("consultants/{consultantId}/reviews/paginated")]
        [Authorize(Roles = "Consultant")]
        public async Task<ActionResult> GetPaginatedConsultantReviews(
            string consultantId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var result = await _consultationService.GetPaginatedConsultantReviewsAsync(consultantId, page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paginated consultant reviews for consultant {ConsultantId}", consultantId);
                return StatusCode(500, new { message = "An error occurred while retrieving consultant reviews" });
            }
        }
    }
} 