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
    public class CommentService:ICommentService
    {
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository;

        public CommentService(IMapper mapper,ICommentRepository commentRepository)
        {
            _mapper=mapper;
            _commentRepository = commentRepository;
        }

        public async Task<CommentViewDto> CreateAsync(CommentCreateDto commentCreateDto, string userId)
        {
            var comment =new Comment
            {
                Content=commentCreateDto.Content,
                BlogId=commentCreateDto.BlogId,
                CommentAt=DateTime.Now,
                UserId=userId
            };
            var createComment=await _commentRepository.CreateAsync(comment);
            return _mapper.Map<CommentViewDto>(createComment);

        }

        public async Task<bool> DeleteAsync(int commentId, string userId,string role)
        {
            var comment=await _commentRepository.GetByIdAsync(commentId);
            if (comment == null || (comment.UserId != userId && role != "Staff" && role != "Manager")) 
            {
                return false;
            }
            return await _commentRepository.DeleteAsync(commentId);
        }

        public async Task<List<CommentViewDto>> GetCommentByBlogIdAsync(int blogId)
        {
            var comments=await _commentRepository.GetByBlogIdAsync(blogId);
            return _mapper.Map<List<CommentViewDto>>(comments);
        }

        public async Task<bool> UpdateAsync(CommentUpdateDto commentUpdateDto, string userId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentUpdateDto.CommentId);
            if (comment == null || comment.UserId!= userId)
            {
                return false;
            }
            comment.Content = commentUpdateDto.Content;
            comment.CommentAt=DateTime.Now;
            return await _commentRepository.UpdateAsync(comment);
        }
    }
}
