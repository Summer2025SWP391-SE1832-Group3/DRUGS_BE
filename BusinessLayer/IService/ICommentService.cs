using DataAccessLayer.Dto.BlogPost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IService
{
    public interface ICommentService
    {
        Task<CommentViewDto> CreateAsync(CommentCreateDto commentCreateDto, string userId);
        Task<bool> UpdateAsync(CommentUpdateDto commentUpdateDto,string userId);
        Task<bool> DeleteAsync(int commentId,string userId, string role);
        Task<List<CommentViewDto>> GetCommentByBlogIdAsync(int blogId);
    }
}
