using BusinessLayer.IService;
using DataAccessLayer.Dto.BlogPost;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace SWP391_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IWebHostEnvironment _environment;

        public BlogController(IBlogService blogService, IWebHostEnvironment environment)
        {
            _blogService = blogService;
            _environment = environment;
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Create([FromForm] BlogCreateDto dto, [FromForm] List<IFormFile> images)
        {
            var staffId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var blog = await _blogService.CreateAsync(dto, staffId);
            if (blog == null) return BadRequest(new { message = "Failed to create blog." });
            if (images != null && images.Count > 0)
            {
                var blogImages = new List<BlogImage>();
                foreach (var imageFile in images)
                {
                    if (imageFile.Length > 0)
                    {
                        var filePath = Path.Combine(_environment.WebRootPath, "hotlink-ok", Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName));
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
                            ImageUrl = "/hotlink-ok/" + Path.GetFileName(filePath),
                            BlogId = blog.BlogId
                        };
                        blogImages.Add(blogImage);
                    }
                }
                foreach (var blogImage in blogImages)
                {
                    await _blogService.AddBlogImageAsync(blogImage);
                }
            }
            var blogWithImages = await _blogService.GetByIdAsync(blog.BlogId);
            return Ok(blogWithImages);
        }

        [HttpPut("{blogId:int}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> Update(int blogId, [FromForm] BlogUpdateDto dto, [FromForm] List<IFormFile> images)
        {
            var staffId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isManager = User.IsInRole("Manager");
            var result = await _blogService.UpdateAsync(blogId,dto, staffId, isManager);
            if (!result)
            {
                return Forbid();
            }
            if (images != null && images.Count > 0)
            {
                var uploadImage = new List<BlogImage>();
                foreach (var imageFile in images)
                {
                    if (imageFile.Length > 0)
                    {
                        var filePath = Path.Combine(_environment.WebRootPath, "hotlink-ok", Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName));
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
                            BlogId = blogId,
                            ImageUrl = "/hotlink-ok/" + Path.GetFileName(filePath)
                        };
                        uploadImage.Add(blogImage);
                    }
                }
                foreach (var blogImage in uploadImage)
                {
                    await _blogService.AddBlogImageAsync(blogImage);
                }

                var blogImages = await _blogService.GetImagesByBlogIdAsync(blogId);
                foreach (var oldImage in blogImages)
                {
                    if (!uploadImage.Any(i => i.ImageUrl == oldImage.ImageUrl))
                    {
                        var oldFilePath = Path.Combine(_environment.WebRootPath, oldImage.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                        await _blogService.DeleteBLogImage(oldImage.BlogImageId);
                    }
                }
            }
            var blogWithImages = await _blogService.GetByIdAsync(blogId);
            return Ok(blogWithImages);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var staffId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isManager = User.IsInRole("Manager");
            var result = await _blogService.DeleteAsync(id, staffId, isManager);
            if (!result) { return Forbid(); }
            return Ok(new
            {
                message = "Delete blog successfull!"
            });
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetail(int id)
        {
            var blog = await _blogService.GetByIdAsync(id);
            if (blog == null) return NotFound();
            return Ok(blog);
        }
            
        [HttpGet("approvedBlogs")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllApproved()
        {
            var blog = await _blogService.GetAllApprovedAsync();
            return Ok(blog);
        }

        [HttpGet("allBlogs")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var blog = await _blogService.GetAllAsync();
            return Ok(blog);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var curentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isStaff = User.IsInRole("Staff");
            if (isStaff && curentUserId != userId)
            {
                return Forbid();
            }
            var blogs = await _blogService.GetBlogByUserIdAsync(userId);
            if (blogs == null || blogs.Count == 0)
            {
                return NotFound(new { message = "No blogs found" });
            }
            return Ok(blogs);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var curentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isStaff = User.IsInRole("Staff");
            if (isStaff && curentUserId != userId)
            {
                return Forbid();
            }
            var blogs = await _blogService.GetBlogByUserIdAsync(userId);
            if (blogs == null || blogs.Count == 0)
            {
                return NotFound(new { message = "No blogs found" });
            }
            return Ok(blogs);
        }

        [HttpPost("approve/{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Approve(int id)
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _blogService.ApproveAsync(id, managerId);
            if (!result) { return Forbid(); }
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

        [HttpGet("GetByStatus")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetBlogByStatus(string status)
        {
            var blogs = await _blogService.GetBlogByStatus(status);
            if(blogs == null || blogs.Count == 0)
            {
                return NotFound(new { message = "No blogs found with the specified status." });
            }
            return Ok(blogs);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBlogByTitle(string? search)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role=User.FindFirstValue(ClaimTypes.Role);
            if(string.IsNullOrEmpty(search))
            {
                search = "";
            }
            List<BlogViewDto> blogs=null;

            if (role == "Staff")
            {
                blogs = await _blogService.SearchBlogByTitle(search, userId,null);
            }
            else if (role=="Manager")
            {
                blogs = await _blogService.SearchBlogByTitle(search, null,null);
            }
            else
            {
                blogs = await _blogService.SearchBlogByTitle(search, null,BlogStatus.Approved.ToString());

            }
            if (blogs == null || blogs.Count == 0)
            {
                return NotFound(new { message = "No blogs found matching the search criteria." });
            }
            return Ok(blogs);
        }
    }
}
