using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IBlogRepository
    {
        Task<Blog> CreateAsync(Blog blog);
        Task<Blog?> GetByIdAsync(int id);
        Task<List<Blog>> GetAllApprovedAsync();
        Task<List<Blog>> GetAllAsync();
        Task<bool> UpdateAsync(Blog blog);
        Task<bool> DeleteAsync(int id);
        Task<bool> ApproveAsync(int blogId, string managerId);
        Task<bool> RejectAsync(int blogId, string managerId);
    }
}
