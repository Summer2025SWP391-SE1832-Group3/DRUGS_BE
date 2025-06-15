using AutoMapper;
using BusinessLayer.IService;
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
        public async Task<bool> UpdateAsync(BlogUpdateDto dto, string staffId,bool isManager)
        {
            var blog = await _blogRepository.GetByIdAsync(dto.BlogId);
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
    }
}
