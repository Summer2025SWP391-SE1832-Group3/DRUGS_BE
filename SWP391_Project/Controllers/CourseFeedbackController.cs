using BusinessLayer.IService;
using DataAccessLayer.Dto.Feedback;
using DataAccessLayer.IRepository;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SWP391_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseFeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public CourseFeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost("course/{courseId}/feedback")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> CreateFeedback(int courseId, [FromBody] FeedbackDto feedbackDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isCourseCompleted = await _feedbackService.IsCourseCompletedAsync(userId, courseId);
            if (!isCourseCompleted)
            {
                return BadRequest("You must complete the course before providing feedback.");
            }
            var existingFeedback = await _feedbackService.GetFeedbackByCourseAndUserAsync(courseId, userId);
            if (existingFeedback != null)
            {
                if (!existingFeedback.IsActive)
                {
                    var updateResult = await _feedbackService.RestoreAndUpdateFeedbackAsync(existingFeedback.FeedbackId, feedbackDto);
                    if (updateResult)
                        return Ok("Feedback has been restored and updated.");
                    else
                        return BadRequest("Failed to restore and create feedback.");
                }

                return BadRequest("You have already provided feedback for this course.");
            }

            var feedback = await _feedbackService.CreateFeedbackAsync(feedbackDto, userId, courseId);
            if (feedback == null)
            {
                return BadRequest("You must complete the course before providing feedback.");
            }

            return Ok("Feedback created successfully.");
        }

        [HttpPut("course/{courseId}/feedback")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> UpdateFeedback(int courseId, [FromBody] FeedbackDto feedbackDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var feedback = await _feedbackService.GetFeedbackByCourseAndUserAsync(courseId, userId);
            if (feedback == null)
            {
                return NotFound("Feedback not found.");
            }
            if (!feedback.IsActive)
            {
                return BadRequest("Your feedback has been deactivated by the manager and cannot be updated.");
            }

            await _feedbackService.UpdateFeedbackAsync(feedback.FeedbackId, feedbackDto);
            return Ok("Feedback updated successfully.");
        }

        [HttpDelete("course/feedback/{feedbackId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteFeedback(int feedbackId)
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(feedbackId);
            if (feedback == null || !feedback.IsActive)
            {
                return BadRequest("Feedback is already deactivated or does not exist.");
            }
            var result = await _feedbackService.DeleteFeedbackAsync(feedbackId);
            if (!result)
            {
                return BadRequest("Failed to delete feedback. The feedback might be inactive or does not exist.");
            }

            return Ok("Feedback deleted successfully.");
        }

        [HttpGet("course/{courseId}/memberfeedback")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> GetFeedbackForCourse(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var feedback = await _feedbackService.GetFeedbackByCourseAndUserAsync(courseId, userId);
            if (feedback == null)
            {
                return NotFound("You have not provided feedback for this course.");
            }
            if (!feedback.IsActive)
            {
                return BadRequest("Your feedback has been deactivated by the manager. You can create a new feedback.");
            }
            return Ok(feedback);
        }

        [HttpGet("course/{courseId}/all_feedback")]
        public async Task<IActionResult> GetAllFeedbacksForCourse(int courseId)
        {
            var feedbacks = await _feedbackService.GetFeedbacksByCourseIdAsync(courseId);
            if (feedbacks == null || !feedbacks.Any())
            {
                return NotFound("No feedback found for this course.");
            }

            return Ok(feedbacks);
        }

        [HttpGet("course/{courseId}/average_rating")]
        public async Task<IActionResult> GetAverageRating(int courseId)
        {
            var (averageRating, totalFeedbacks) = await _feedbackService.GetAverageRatingAsync(courseId);
            return Ok(
                new {
                    AverageRating = averageRating, 
                    TotalFeedbacks = totalFeedbacks });
        }

    }
}
