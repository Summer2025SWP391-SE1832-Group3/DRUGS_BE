using AutoMapper;
using BusinessLayer.IService;
using BusinessLayer.Dto.Common;
using DataAccessLayer.Dto.BlogPost;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IBlogImageRepository _blogImageRepository;
        private readonly IMapper _mapper;

        public BlogService(IBlogRepository blogRepository,IMapper mapper,IBlogImageRepository blogImageRepository)
        {
            _blogRepository = blogRepository;
            _blogImageRepository = blogImageRepository;
            _mapper = mapper;
        }

        public async Task<Blog> CreateAsync(BlogCreateDto dto, string staffId)
        {
            var blog = new Blog
            {
                Title = dto.Title,
                Content = dto.Content,
                Category = dto.Category,
                PostedAt = DateTime.Now,
                PostedById = staffId,
                Status = BlogStatus.Pending,
            };
            return await _blogRepository.CreateAsync(blog);

        }

        public async Task<bool> DeleteAsync(int blogId, string staffId, bool isManager)
        {
            var blog = await _blogRepository.GetByIdAsync(blogId);
            if (blog == null || (blog.PostedById != staffId && !isManager))
            {
                return false;
            }
            return await _blogRepository.DeleteAsync(blogId);

        }

        public async Task<List<BlogViewDto>> GetAllAsync()
        {
            var blogs= await _blogRepository.GetAllAsync();
            return  _mapper.Map<List<BlogViewDto>>(blogs);
        }

        public async Task<List<BlogViewDto>> GetAllApprovedAsync()
        {
            var blogs= await _blogRepository.GetAllApprovedAsync();
            return _mapper.Map<List<BlogViewDto>>(blogs);
        }

        public async Task<BlogViewDto> GetByIdAsync(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null)
            {
                return null;
            }
            return _mapper.Map<BlogViewDto>(blog);
        }
        public async Task<bool> UpdateAsync(int blogId,BlogUpdateDto dto, string staffId,bool isManager)
        {
            var blog = await _blogRepository.GetByIdAsync(blogId);
            if (blog == null || (blog.PostedById != staffId && !isManager))
            {
                return false;
            }
            blog.Content = dto.Content;
            blog.Category = dto.Category;
            blog.Title = dto.Title;
          
            return await _blogRepository.UpdateAsync(blog);
        }

        public async Task<bool> RejectAsync(int blogId, string managerId)
        {
            return await _blogRepository.RejectAsync(blogId, managerId);
        }
        public async Task<bool> ApproveAsync(int blogId, string managerId)
        {
            return await _blogRepository.ApproveAsync(blogId, managerId);
        }

        public async Task AddBlogImageAsync(BlogImage blogImage)
        {
           await _blogImageRepository.AddAsync(blogImage);
        }

        public async Task<List<BlogImage>> GetImagesByBlogIdAsync(int blogId)
        {
            return await _blogImageRepository.GetByBlogIdAsync(blogId);
        }

        public async Task DeleteBLogImage(int blogImageId)
        {
           await _blogImageRepository.DeleteAsync(blogImageId);
        }

        public async Task<List<BlogViewDto>> GetBlogByUserIdAsync(string userId)
        {

            var blogs = await _blogRepository.GetByUserIdAsync(userId);
            return _mapper.Map<List<BlogViewDto>>(blogs);
        }

        public async Task<List<BlogViewDto>> GetBlogByStatus(string status)
        {
            BlogStatus blogStatus;
            if(!Enum.TryParse(status,true,out blogStatus))
            {
                return null;
            }
            var blogs= await _blogRepository.GetBlogByStatus(blogStatus);
            if(blogs == null || blogs.Count == 0)
            {
                return new List<BlogViewDto>();
            }
            return _mapper.Map<List<BlogViewDto>>(blogs);
        }

        public async Task<List<BlogViewDto>> SearchBlogByTitle(string search, string? userId=null, string? status=null)
        {
            var blogs =await _blogRepository.SearchAsync(search, userId, status);
            if(blogs==null || blogs.Count == 0)
            {
                return new List<BlogViewDto>();

            }
            return  _mapper.Map<List<BlogViewDto>>(blogs);
        }

        public async Task<PaginatedResult<BlogViewDto>> GetPaginatedBlogsAsync(int page, int pageSize, string? status = null, string? searchTerm = null)
        {
            var query = await _blogRepository.GetAllAsync();
            var blogs = query.AsQueryable();

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<BlogStatus>(status, true, out var blogStatus))
                {
                    blogs = blogs.Where(b => b.Status == blogStatus);
                }
            }

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                blogs = blogs.Where(b => 
                    b.Title.Contains(searchTerm) || 
                    b.Content.Contains(searchTerm) || 
                    b.Category.Contains(searchTerm));
            }

            var totalCount = blogs.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            page = Math.Max(1, Math.Min(page, totalPages));

            var paginatedBlogs = blogs
                .OrderByDescending(b => b.PostedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = _mapper.Map<List<BlogViewDto>>(paginatedBlogs);

            return new PaginatedResult<BlogViewDto>
            {
                Items = result,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        public async Task<PaginatedResult<BlogViewDto>> GetPaginatedApprovedBlogsAsync(int page, int pageSize, string? searchTerm = null)
        {
            var approvedBlogs = await _blogRepository.GetAllApprovedAsync();
            var blogs = approvedBlogs.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                blogs = blogs.Where(b => 
                    b.Title.Contains(searchTerm) || 
                    b.Content.Contains(searchTerm) || 
                    b.Category.Contains(searchTerm));
            }

            var totalCount = blogs.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            page = Math.Max(1, Math.Min(page, totalPages));

            var paginatedBlogs = blogs
                .OrderByDescending(b => b.PostedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = _mapper.Map<List<BlogViewDto>>(paginatedBlogs);

            return new PaginatedResult<BlogViewDto>
            {
                Items = result,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        public async Task<PaginatedResult<BlogViewDto>> GetPaginatedBlogsByUserAsync(string userId, int page, int pageSize, string? status = null)
        {
            var userBlogs = await _blogRepository.GetByUserIdAsync(userId);
            var blogs = userBlogs.AsQueryable();

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<BlogStatus>(status, true, out var blogStatus))
                {
                    blogs = blogs.Where(b => b.Status == blogStatus);
                }
            }

            var totalCount = blogs.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            page = Math.Max(1, Math.Min(page, totalPages));

            var paginatedBlogs = blogs
                .OrderByDescending(b => b.PostedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = _mapper.Map<List<BlogViewDto>>(paginatedBlogs);

            return new PaginatedResult<BlogViewDto>
            {
                Items = result,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            };
        }
    }
}
