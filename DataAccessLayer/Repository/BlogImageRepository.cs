using DataAccessLayer.Data;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
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
    }
}
