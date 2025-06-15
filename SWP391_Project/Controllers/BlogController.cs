using BusinessLayer.IService;
using DataAccessLayer.Dto.BlogPost;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SWP391_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IWebHostEnvironment _environment;

        public BlogController(IBlogService blogService,IWebHostEnvironment environment) {
            _blogService = blogService;
            _environment = environment;
        }

        [HttpPost]
        [Authorize(Roles ="Staff")]
        public async Task<IActionResult> Create([FromForm] BlogCreateDto dto,[FromForm] List<IFormFile> images)
        {
            var staffId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var blog= await _blogService.CreateAsync(dto, staffId);
            if (blog == null) return BadRequest(new { message = "Failed to create blog." });
            if(images!=null && images.Count > 0)
            {
                var blogImages = new List<BlogImage>();
                foreach(var imageFile in images)
                {
                    if (imageFile.Length > 0)
                    {
                        var filePath = Path.Combine(_environment.WebRootPath, "uploads", Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName));
                        var directory = Path.GetDirectoryName(filePath);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        var blogImage = new BlogImage
                        {
                            ImageUrl = "/upload/" + Path.GetFileName(filePath),
                            BlogId = blog.BlogId
                        };
                        blogImages.Add(blogImage);
                    }
                }
                foreach(var blogImage in blogImages)
                {
                    await _blogService.AddBlogImageAsync(blogImage);
                }
            }
            var blogWithImages=await _blogService.GetByIdAsync(blog.BlogId);
            return Ok(blogWithImages);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> Update(int id, BlogUpdateDto dto)
        {
            var staffId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isManager = User.IsInRole("Manager");
            if (id != dto.BlogId)
            {
                return BadRequest(new { message = "ID in URL and data do not match." });
            }
            var result = await _blogService.UpdateAsync(dto, staffId, isManager);
            if (!result)
            {
                return Forbid();
            }
            return Ok(new
            {
                message="Update blog successfull!"
            });
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles ="Staff,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var staffId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isManager = User.IsInRole("Manager");
            var result = await _blogService.DeleteAsync(id, staffId, isManager);
            if(!result) { return Forbid(); }
            return Ok(new
            {
                message="Delete blog successfull!"
            });
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetail(int id)
        {
            var blog =await _blogService.GetByIdAsync(id);
            if (blog == null) return NotFound();
            return Ok(blog);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllApproved()
        {
            var blog = await _blogService.GetAllApprovedAsync();
            return Ok(blog);
        }

        [HttpGet("Manager")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var blog = await _blogService.GetAllAsync();
            return Ok(blog);
        }

        [HttpPost("approve/{id:int}")]
        [Authorize(Roles ="Manager")]
        public async Task<IActionResult> Approve(int id)
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result=await _blogService.ApproveAsync(id, managerId);
            if(!result) { return Forbid(); }
            return Ok(new
            {
                message = "Blog was approved!"
            });

        }

        [HttpPost("reject/{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Reject(int id)
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _blogService.RejectAsync(id, managerId);
            if (!result) { return Forbid(); }
            return Ok(new
            {
                message = "Blog was rejected!"
            });

        }
    }
}
