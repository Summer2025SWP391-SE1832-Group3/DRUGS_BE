using DataAccessLayer.Data;
using DataAccessLayer.Dto.BlogPost;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDBContext _context;

        public BlogRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Blog> CreateAsync(Blog blog)
        {
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return blog;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var blog = await _context.Blogs
                 .Include(b => b.BlogImages)
                 .FirstOrDefaultAsync(b => b.BlogId == id);
            if (blog == null)
            {
                return false;
            }
            foreach (var img in blog.BlogImages)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", img.ImageUrl.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Blog>> GetAllAsync()
        {
            return await _context.Blogs
               .Include(b => b.PostedBy)
               .Include(b => b.Comments)
                    .ThenInclude(c => c.User)
               .Include(b=>b.BlogImages)
               .OrderByDescending(b => b.PostedAt)
               .ToListAsync();
        }

        public async Task<List<Blog>> GetAllApprovedAsync()
        {
            return await _context.Blogs
               .Where(b=>b.Status==BlogStatus.Approved)
               .Include(b => b.PostedBy)
               .Include(b => b.Comments)
                   .ThenInclude(c=>c.User)
               .Include(b => b.BlogImages)
               .OrderByDescending(b => b.PostedAt)
               .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Blog blog)
        {
             _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ApproveAsync(int blogId, string managerId)
        {
            var blog = await _context.Blogs.FindAsync(blogId);
            if(blog==null || blog.Status == BlogStatus.Approved)
            {
                return false;
            }
            blog.Status = BlogStatus.Approved;
            blog.ApprovedAt= DateTime.Now;
            blog.ApprovedById = managerId;
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> RejectAsync(int blogId, string managerId)
        {
            var blog = await _context.Blogs.FindAsync(blogId);
            if (blog == null || blog.Status == BlogStatus.Rejected)
            {
                return false;
            }
            blog.Status = BlogStatus.Rejected;
            blog.ApprovedAt = DateTime.Now;
            blog.ApprovedById = managerId;
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<Blog?> GetByIdAsync(int id)
        {
            return await _context.Blogs
               .Include(b => b.PostedBy)
               .Include(b => b.Comments)
                    .ThenInclude(c=>c.User)
               .Include(b => b.BlogImages)
               .FirstOrDefaultAsync(b => b.BlogId == id);
        }

        public async Task<List<Blog>> GetByUserIdAsync(string userId)
        {
            return await _context.Blogs
                 .Where(b => b.PostedById.Equals(userId))
                 .Include(b=>b.PostedBy)
                 .Include(b => b.BlogImages)
                 .OrderByDescending(b => b.PostedAt)
                 .ToListAsync();
        }

        public async Task<List<Blog>> GetBlogByStatus(BlogStatus blogStatus)
        {
            return await _context.Blogs
                .Where(b=>b.Status==blogStatus)
                .Include(b => b.PostedBy)
                .Include(b => b.BlogImages)
                .OrderByDescending(b => b.PostedAt)
                .ToListAsync(); ;
        }
    }
}
