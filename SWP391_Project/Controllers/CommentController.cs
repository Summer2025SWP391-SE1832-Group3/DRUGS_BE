using BusinessLayer.IService;
using BusinessLayer.Service;
using DataAccessLayer.Dto.BlogPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SWP391_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService) 
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CommentCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            var comment= await _commentService.CreateAsync(dto, userId);
            if (comment == null)
            {
                return BadRequest("Create fail");
            }
            return Ok(comment);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment(CommentUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState) ;
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result =await _commentService.UpdateAsync(dto,userId);
            if (!result)
            {
                return Forbid();
            }
            return Ok(new
            {
                message = "Update comment successfull"
            });
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Member,Staff,Manager")]
        public async Task<IActionResult> RemoveComment(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var result = await _commentService.DeleteAsync(id, userId,role);
            if (!result)
            {
                return Forbid();
            }
            return Ok(new
            {
                message = "Remove comment successfull"
            });
        }

        [HttpGet("blog/{blogId:int}")]
        public async Task<IActionResult> GetAll(int blogId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comments = await _commentService.GetCommentByBlogIdAsync(blogId);
            if(comments == null)
            {
                return NotFound();
            }
            return Ok(comments);
        }
    }
}
