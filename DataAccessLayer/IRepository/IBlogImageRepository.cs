using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IBlogImageRepository
    {
        Task AddAsync(BlogImage blogImage);
        Task<List<BlogImage>> GetByBlogIdAsync(int blogId);
        Task DeleteAsync(int blogImageId);

    }
}
