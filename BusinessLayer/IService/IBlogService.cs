using DataAccessLayer.Dto.BlogPost;
using DataAccessLayer.Model;
using BusinessLayer.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IService
{
    public interface IBlogService
    {
        Task<Blog> CreateAsync(BlogCreateDto dto,string staffId);
        Task<List<BlogViewDto>> GetAllAsync();
        Task<List<BlogViewDto>> GetAllApprovedAsync();


        Task<BlogViewDto> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int blogId,BlogUpdateDto dto, string staffId,bool isManager);

        Task<bool> DeleteAsync(int blogId,string staffId,bool isManager);

        Task<bool> ApproveAsync(int blogId,string managerId);
        Task<bool> RejectAsync(int blogId, string managerId);

        Task AddBlogImageAsync(BlogImage blogImage);
        Task<List<BlogImage>> GetImagesByBlogIdAsync(int blogId);

        Task DeleteBLogImage(int blogImageId);
        Task<List<BlogViewDto>> GetBlogByUserIdAsync(string userId);
        Task<List<BlogViewDto>> GetBlogByStatus(string status);
        Task<List<BlogViewDto>> SearchBlogByTitle(string search, string userId, string status);

        // Pagination methods
        Task<PaginatedResult<BlogViewDto>> GetPaginatedBlogsAsync(int page, int pageSize, string? status = null, string? searchTerm = null);
        Task<PaginatedResult<BlogViewDto>> GetPaginatedApprovedBlogsAsync(int page, int pageSize, string? searchTerm = null);
        Task<PaginatedResult<BlogViewDto>> GetPaginatedBlogsByUserAsync(string userId, int page, int pageSize, string? status = null);
    }
}
