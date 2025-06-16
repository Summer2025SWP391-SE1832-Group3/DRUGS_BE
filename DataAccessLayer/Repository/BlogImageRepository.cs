using DataAccessLayer.Data;
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
    public class BlogImageRepository : IBlogImageRepository
    {
        private readonly ApplicationDBContext _context;

        public BlogImageRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task AddAsync(BlogImage blogImage)
        {
           await _context.BlogImages.AddAsync(blogImage);
           await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int blogImageId)
        {
            var blogImage =await _context.BlogImages.FindAsync(blogImageId);
            if (blogImage != null)
            {
                _context.BlogImages.Remove(blogImage);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<BlogImage>> GetByBlogIdAsync(int blogId)
        {
            return await _context.BlogImages.Where(i => i.BlogId == blogId).ToListAsync();
        }
    }
}
