using BusinessLayer.IService;
using DataAccessLayer.Dto.Consultation;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Data;

namespace BusinessLayer.Service
{
    public class ConsultationService : IConsultationService
    {
        private readonly IConsultationRepository _consultationRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ConsultationService> _logger;
        private readonly ApplicationDBContext _context;

        public ConsultationService(
            IConsultationRepository consultationRepository,
            UserManager<ApplicationUser> userManager,
            ILogger<ConsultationService> logger,
            ApplicationDBContext context)
        {
            _consultationRepository = consultationRepository;
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        // ConsultationRequest methods
        public async Task<ConsultationRequestViewDto> CreateConsultationRequestAsync(ConsultationRequestCreateDto dto, string userId)
        {
            _logger.LogInformation("Creating consultation request for user: {UserId}", userId);

            // Validate consultant exists and is actually a consultant
            var consultant = await _userManager.FindByIdAsync(dto.ConsultantId);
            if (consultant == null)
            {
                throw new InvalidOperationException("Consultant not found");
            }

            if (!await _userManager.IsInRoleAsync(consultant, "Consultant"))
            {
                throw new InvalidOperationException("Selected user is not a consultant");
            }

            // Kiểm tra trùng lịch tư vấn
            if (await IsConsultationRequestOverlappingAsync(dto.ConsultantId, dto.RequestedDate, dto.DurationMinutes))
            {
                throw new InvalidOperationException("The requested time overlaps with consultant's schedule or another consultation.");
            }

            var request = new ConsultationRequest
            {
                Title = dto.Title,
                Description = dto.Description,
                Category = dto.Category,
                RequestedDate = dto.RequestedDate,
                DurationMinutes = dto.DurationMinutes,
                Notes = dto.Notes,
                UserId = userId,
                ConsultantId = dto.ConsultantId,
                Status = ConsultationStatus.Pending
            };

            var createdRequest = await _consultationRepository.CreateConsultationRequestAsync(request);
            _logger.LogInformation("Created request with ID: {RequestId}", createdRequest.Id);

            // Reload with navigation properties to ensure we have User and Consultant data
            var requestWithDetails = await _consultationRepository.GetConsultationRequestByIdAsync(createdRequest.Id);
            if (requestWithDetails == null)
            {
                _logger.LogError("Failed to reload request with details. RequestId: {RequestId}", createdRequest.Id);
                throw new InvalidOperationException("Failed to create consultation request");
            }

            return await MapToConsultationRequestViewDtoAsync(requestWithDetails);
        }

        public async Task<ConsultationRequestViewDto?> GetConsultationRequestByIdAsync(int id, string currentUserId)
        {
            var request = await _consultationRepository.GetConsultationRequestByIdAsync(id);
            if (request == null) return null;

            if (!await CanAccessConsultationRequestAsync(id, currentUserId))
            {
                throw new UnauthorizedAccessException("You don't have permission to access this consultation request");
            }

            return await MapToConsultationRequestViewDtoAsync(request);
        }

        public async Task<IEnumerable<ConsultationRequestViewDto>> GetMyConsultationRequestsAsync(string userId)
        {
            var requests = await _consultationRepository.GetConsultationRequestsByUserIdAsync(userId);
            var result = new List<ConsultationRequestViewDto>();
            
            foreach (var request in requests)
            {
                result.Add(await MapToConsultationRequestViewDtoAsync(request));
            }
            
            return result;
        }

        public async Task<IEnumerable<ConsultationRequestViewDto>> GetConsultationRequestsForConsultantAsync(string consultantId)
        {
            if (!await IsConsultantAsync(consultantId))
            {
                throw new InvalidOperationException("User is not a consultant");
            }

            var requests = await _consultationRepository.GetConsultationRequestsByConsultantIdAsync(consultantId);
            var result = new List<ConsultationRequestViewDto>();
            
            foreach (var request in requests)
            {
                result.Add(await MapToConsultationRequestViewDtoAsync(request));
            }
            
            return result;
        }

        public async Task<IEnumerable<ConsultationRequestViewDto>> GetAllConsultationRequestsAsync(string currentUserId)
        {
            // Only Admin can see all requests
            var user = await _userManager.FindByIdAsync(currentUserId);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                throw new UnauthorizedAccessException("Only Admin can view all consultation requests");
            }

            var requests = await _consultationRepository.GetAllConsultationRequestsAsync();
            var result = new List<ConsultationRequestViewDto>();
            
            foreach (var request in requests)
            {
                result.Add(await MapToConsultationRequestViewDtoAsync(request));
            }
            
            return result;
        }

        public async Task<ConsultationRequestViewDto> UpdateConsultationRequestStatusAsync(int id, ConsultationRequestUpdateDto dto, string currentUserId)
        {
            var request = await _consultationRepository.GetConsultationRequestByIdAsync(id);
            if (request == null)
            {
                throw new InvalidOperationException("Consultation request not found");
            }

            // Only consultant or admin can update status
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            if (request.ConsultantId != currentUserId && (currentUser == null || !await _userManager.IsInRoleAsync(currentUser, "Admin")))
            {
                throw new UnauthorizedAccessException("You don't have permission to update this consultation request");
            }

            request.Status = dto.Status;
            request.ScheduledDate = dto.ScheduledDate;
            request.GoogleMeetLink = dto.GoogleMeetLink;
            request.Notes = dto.Notes;
            request.UpdatedAt = DateTime.Now;

            var updatedRequest = await _consultationRepository.UpdateConsultationRequestAsync(request);
            // Reload with navigation properties
            var requestWithDetails = await _consultationRepository.GetConsultationRequestByIdAsync(updatedRequest.Id);
            return await MapToConsultationRequestViewDtoAsync(requestWithDetails);
        }

        public async Task<bool> DeleteConsultationRequestAsync(int id, string currentUserId)
        {
            var request = await _consultationRepository.GetConsultationRequestByIdAsync(id);
            if (request == null) return false;

            // Only user who created the request, consultant, or admin can delete
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            if (request.UserId != currentUserId && 
                request.ConsultantId != currentUserId && 
                (currentUser == null || !await _userManager.IsInRoleAsync(currentUser, "Admin")))
            {
                throw new UnauthorizedAccessException("You don't have permission to delete this consultation request");
            }

            return await _consultationRepository.DeleteConsultationRequestAsync(id);
        }

        // ConsultationSession methods
        public async Task<ConsultationSessionViewDto> CreateConsultationSessionAsync(int requestId, ConsultationSessionCreateDto dto, string currentUserId)
        {
            var request = await _consultationRepository.GetConsultationRequestByIdAsync(requestId);
            if (request == null)
            {
                throw new InvalidOperationException("Consultation request not found");
            }

            if (request.ConsultantId != currentUserId)
            {
                throw new UnauthorizedAccessException("Only the assigned consultant can create a session");
            }

            if (request.Status != ConsultationStatus.Approved)
            {
                throw new InvalidOperationException("Can only create session for approved requests");
            }

            var session = new ConsultationSession
            {
                StartTime = dto.StartTime,
                SessionNotes = dto.SessionNotes,
                Recommendations = dto.Recommendations,
                ConsultationRequestId = requestId
            };

            var createdSession = await _consultationRepository.CreateConsultationSessionAsync(session);
            return MapToConsultationSessionViewDto(createdSession);
        }

        public async Task<ConsultationSessionViewDto?> GetConsultationSessionAsync(int requestId, string currentUserId)
        {
            if (!await CanAccessConsultationRequestAsync(requestId, currentUserId))
            {
                throw new UnauthorizedAccessException("You don't have permission to access this consultation session");
            }

            var session = await _consultationRepository.GetConsultationSessionByRequestIdAsync(requestId);
            return session != null ? MapToConsultationSessionViewDto(session) : null;
        }

        public async Task<ConsultationSessionViewDto> UpdateConsultationSessionAsync(int sessionId, ConsultationSessionCreateDto dto, string currentUserId)
        {
            var session = await _consultationRepository.GetConsultationSessionByIdAsync(sessionId);
            if (session == null)
            {
                throw new InvalidOperationException("Consultation session not found");
            }

            if (session.ConsultationRequest.ConsultantId != currentUserId)
            {
                throw new UnauthorizedAccessException("Only the assigned consultant can update the session");
            }

            session.StartTime = dto.StartTime;
            session.SessionNotes = dto.SessionNotes;
            session.Recommendations = dto.Recommendations;
            session.UpdatedAt = DateTime.Now;

            var updatedSession = await _consultationRepository.UpdateConsultationSessionAsync(session);
            return MapToConsultationSessionViewDto(updatedSession);
        }

        public async Task<ConsultationSessionViewDto> CompleteConsultationSessionAsync(int sessionId, string currentUserId)
        {
            var session = await _consultationRepository.GetConsultationSessionByIdAsync(sessionId);
            if (session == null)
            {
                throw new InvalidOperationException("Consultation session not found");
            }

            if (session.ConsultationRequest.ConsultantId != currentUserId)
            {
                throw new UnauthorizedAccessException("Only the assigned consultant can complete the session");
            }

            session.EndTime = DateTime.Now;
            session.IsCompleted = true;
            session.UpdatedAt = DateTime.Now;

            // Update request status to completed
            var request = session.ConsultationRequest;
            request.Status = ConsultationStatus.Completed;
            request.UpdatedAt = DateTime.Now;
            await _consultationRepository.UpdateConsultationRequestAsync(request);

            var updatedSession = await _consultationRepository.UpdateConsultationSessionAsync(session);
            return MapToConsultationSessionViewDto(updatedSession);
        }

        // ConsultationReview methods
        public async Task<ConsultationReviewViewDto> CreateConsultationReviewAsync(int requestId, ConsultationReviewCreateDto dto, string currentUserId)
        {
            var request = await _consultationRepository.GetConsultationRequestByIdAsync(requestId);
            if (request == null)
            {
                throw new InvalidOperationException("Consultation request not found");
            }

            if (request.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("Only the user who created the request can review");
            }

            if (request.Status != ConsultationStatus.Completed)
            {
                throw new InvalidOperationException("Can only review completed consultations");
            }

            // Check if review already exists
            var existingReview = await _consultationRepository.GetConsultationReviewByRequestIdAsync(requestId);
            if (existingReview != null)
            {
                throw new InvalidOperationException("Review already exists for this consultation");
            }

            var review = new ConsultationReview
            {
                Rating = dto.Rating,
                Comment = dto.Comment,
                ConsultationRequestId = requestId
            };

            var createdReview = await _consultationRepository.CreateConsultationReviewAsync(review);
            return MapToConsultationReviewViewDto(createdReview);
        }

        public async Task<ConsultationReviewViewDto?> GetConsultationReviewAsync(int requestId, string currentUserId)
        {
            if (!await CanAccessConsultationRequestAsync(requestId, currentUserId))
            {
                throw new UnauthorizedAccessException("You don't have permission to access this review");
            }

            var review = await _consultationRepository.GetConsultationReviewByRequestIdAsync(requestId);
            return review != null ? MapToConsultationReviewViewDto(review) : null;
        }

        public async Task<IEnumerable<ConsultationReviewViewDto>> GetConsultantReviewsAsync(string consultantId)
        {
            if (!await IsConsultantAsync(consultantId))
            {
                throw new InvalidOperationException("User is not a consultant");
            }

            var reviews = await _consultationRepository.GetConsultationReviewsByConsultantIdAsync(consultantId);
            return reviews.Select(r => MapToConsultationReviewViewDto(r));
        }

        // Search and filter methods
        public async Task<IEnumerable<ConsultationRequestViewDto>> SearchConsultationRequestsAsync(
            string currentUserId,
            string? userId = null, 
            string? consultantId = null, 
            ConsultationStatus? status = null,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            // Only Admin can search with filters
            var user = await _userManager.FindByIdAsync(currentUserId);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                throw new UnauthorizedAccessException("Only Admin can search consultation requests");
            }

            var requests = await _consultationRepository.SearchConsultationRequestsAsync(userId, consultantId, status, fromDate, toDate);
            var result = new List<ConsultationRequestViewDto>();
            
            foreach (var request in requests)
            {
                result.Add(await MapToConsultationRequestViewDtoAsync(request));
            }
            
            return result;
        }

        // Helper methods
        public async Task<bool> IsConsultantAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null && await _userManager.IsInRoleAsync(user, "Consultant");
        }

        public async Task<bool> CanAccessConsultationRequestAsync(int requestId, string currentUserId)
        {
            var request = await _consultationRepository.GetConsultationRequestByIdAsync(requestId);
            if (request == null) return false;

            // User can access if they are the requester, consultant, or admin
            if (request.UserId == currentUserId || request.ConsultantId == currentUserId)
                return true;

            var user = await _userManager.FindByIdAsync(currentUserId);
            return user != null && await _userManager.IsInRoleAsync(user, "Admin");
        }

        public async Task<bool> IsConsultationRequestOverlappingAsync(string consultantId, DateTime requestedDate, int durationMinutes)
        {
            // 1. Kiểm tra có nằm trong WorkingHour không
            var workingHours = await _userManager.Users
                .Where(u => u.Id == consultantId)
                .SelectMany(u => u.WorkingHours)
                .ToListAsync();
            var dayOfWeek = requestedDate.DayOfWeek;
            var startTime = requestedDate.TimeOfDay;
            var endTime = startTime.Add(TimeSpan.FromMinutes(durationMinutes));
            var validWorkingHour = workingHours.Any(wh => wh.DayOfWeek == dayOfWeek && startTime >= wh.StartTime && endTime <= wh.EndTime);
            if (!validWorkingHour) return true; // Không nằm trong working hour thì coi là trùng (không hợp lệ)

            // 2. Kiểm tra trùng với các ConsultationRequest đã có (Pending, Approved)
            var requests = await _context.ConsultationRequests
                .Where(r => r.ConsultantId == consultantId && (r.Status == ConsultationStatus.Pending || r.Status == ConsultationStatus.Approved))
                .ToListAsync();
            foreach (var req in requests)
            {
                var reqStart = req.RequestedDate;
                var reqEnd = reqStart.AddMinutes(req.DurationMinutes);
                if (requestedDate < reqEnd && requestedDate.AddMinutes(durationMinutes) > reqStart)
                    return true;
            }
            return false;
        }

        // Private mapping methods
        private async Task<ConsultationRequestViewDto> MapToConsultationRequestViewDtoAsync(ConsultationRequest request)
        {
            _logger.LogInformation("Mapping request {RequestId}. UserId: {UserId}, ConsultantId: {ConsultantId}", 
                request.Id, request.UserId, request.ConsultantId);
            
            // Load User and Consultant if they are null
            var user = request.User;
            if (user == null)
            {
                _logger.LogInformation("User navigation property is null, loading from UserManager. UserId: {UserId}", request.UserId);
                user = await _userManager.FindByIdAsync(request.UserId);
            }
            
            var consultant = request.Consultant;
            if (consultant == null)
            {
                _logger.LogInformation("Consultant navigation property is null, loading from UserManager. ConsultantId: {ConsultantId}", request.ConsultantId);
                consultant = await _userManager.FindByIdAsync(request.ConsultantId);
            }
            
            if (user == null)
            {
                _logger.LogError("Failed to load user. UserId: {UserId}", request.UserId);
                throw new InvalidOperationException($"Failed to load user information for UserId: {request.UserId}");
            }
            
            if (consultant == null)
            {
                _logger.LogError("Failed to load consultant. ConsultantId: {ConsultantId}", request.ConsultantId);
                throw new InvalidOperationException($"Failed to load consultant information for ConsultantId: {request.ConsultantId}");
            }
            
            _logger.LogInformation("Successfully loaded user: {UserName} and consultant: {ConsultantName}", 
                user.UserName, consultant.UserName);
            
            return new ConsultationRequestViewDto
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                Category = request.Category,
                RequestedDate = request.RequestedDate,
                ScheduledDate = request.ScheduledDate,
                DurationMinutes = request.DurationMinutes,
                GoogleMeetLink = request.GoogleMeetLink,
                Notes = request.Notes,
                Status = request.Status,
                CreatedAt = request.CreatedAt,
                
                UserId = request.UserId,
                UserName = user.UserName ?? "Unknown",
                UserFullName = user.FullName ?? "Unknown",
                
                ConsultantId = request.ConsultantId,
                ConsultantName = consultant.UserName ?? "Unknown",
                ConsultantFullName = consultant.FullName ?? "Unknown",
                
                Session = request.ConsultationSession != null ? MapToConsultationSessionViewDto(request.ConsultationSession) : null,
                Review = request.Review != null ? MapToConsultationReviewViewDto(request.Review) : null
            };
        }

        private ConsultationSessionViewDto MapToConsultationSessionViewDto(ConsultationSession session)
        {
            return new ConsultationSessionViewDto
            {
                Id = session.Id,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                SessionNotes = session.SessionNotes,
                Recommendations = session.Recommendations,
                IsCompleted = session.IsCompleted,
                CreatedAt = session.CreatedAt
            };
        }

        private ConsultationReviewViewDto MapToConsultationReviewViewDto(ConsultationReview review)
        {
            return new ConsultationReviewViewDto
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }
    }
}