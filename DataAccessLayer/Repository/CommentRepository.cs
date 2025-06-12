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
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;

        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment =await _context.Comments.FindAsync(id);
            if (comment == null) return false;
             _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Comment>> GetByBlogIdAsync(int id)
        {
            return await _context.Comments.Include(c=>c.User).Where(c=>c.BlogId==id).ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(c=>c.User).FirstOrDefaultAsync(c => c.CommentId == id);
        }

        public async Task<bool> UpdateAsync(Comment comment)
        {
            var comment1=await _context.Comments.FindAsync(comment.CommentId);
            if(comment1==null) return false;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
