using DataAccessLayer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Data
{
    public class ApplicationDBContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
        {

        }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyResult> SurveyResults { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
        public DbSet<SurveyAnswerResult> SurveyAnswerResults { get; set; }  

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var roles = new List<IdentityRole>{
                new IdentityRole{Name="Admin",NormalizedName="ADMIN"},
                new IdentityRole{Name="Staff", NormalizedName="STAFF"},
                new IdentityRole{Name="Manager", NormalizedName="MANAGER"},
                new IdentityRole{Name="Consultant", NormalizedName="CONSULTANT"},
                new IdentityRole{Name="Member", NormalizedName="MEMBER"},
            };
            builder.Entity<IdentityRole>().HasData(roles);

            builder.Entity<Blog>(entity =>
            {
                entity.HasKey(b => b.BlogId);

                entity.Property(b => b.PostedAt)
                     .HasDefaultValueSql("GETDATE()");

                entity.HasOne(b => b.PostedBy)
                    .WithMany(b => b.BlogsPosted)
                    .HasForeignKey(b => b.PostedById)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.ApprovedBy)
                    .WithMany(b => b.BlogsApproved)
                    .HasForeignKey(b => b.ApprovedById)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(b => b.Comments)
                    .WithOne(b => b.Blog)
                    .HasForeignKey(b => b.BlogId);

                entity.HasMany(b => b.BlogImages)
                    .WithOne(b => b.Blog)
                    .HasForeignKey(b => b.BlogId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.CommentId);

                entity.Property(c => c.CommentAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasOne(c => c.Blog)
                    .WithMany(c => c.Comments)
                    .HasForeignKey(c => c.BlogId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.User)
                    .WithMany(c => c.Comments)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<BlogImage>(entity =>
            {
                entity.HasKey(b => b.BlogImageId);
                entity.HasOne(b => b.Blog)
                    .WithMany(b => b.BlogImages)
                    .HasForeignKey(b => b.BlogId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Survey>(entity => { 
                entity.HasKey(s => s.SurveyId);

                entity.Property(s => s.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasMany(s=>s.SurveyResults)
                    .WithOne(sr => sr.Survey)
                    .HasForeignKey(sr => sr.SurveyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(s=>s.SurveyQuestions)
                    .WithOne(sq => sq.Survey)
                    .HasForeignKey(sq => sq.SurveyId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<SurveyResult>(entity =>
            {
                entity.HasKey(s => s.ResultId);

                entity.HasOne(s => s.User)
                    .WithMany(u=>u.SurveyResults)
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sr=>sr.Survey)
                    .WithMany(s => s.SurveyResults)
                    .HasForeignKey(sr => sr.SurveyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<SurveyQuestion>(entity =>
            {
                entity.HasKey(e => e.QuestionId);

                entity.HasMany(e => e.SurveyAnswers)
                    .WithOne(a => a.SurveyQuestion)
                    .HasForeignKey(a => a.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<SurveyAnswer>(entity =>
            {
                entity.HasKey(e => e.AnswerId);
                entity.Property(e => e.IsCorrect).HasDefaultValue(false);
            });

            builder.Entity<SurveyAnswerResult>(entity =>
            {
                entity.HasKey(e => new { e.SurveyResultId, e.AnswerId });

                entity.HasOne(ar => ar.SurveyAnswer)
                    .WithMany(sa => sa.SurveyAnswerResults)
                    .HasForeignKey(ar => ar.AnswerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ar => ar.SurveyResult)
                    .WithMany(sr => sr.SurveyAnswerResults)
                    .HasForeignKey(ar => ar.SurveyResultId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ar => ar.SurveyQuestion)
                    .WithMany(sq => sq.SurveyAnswerResults)
                    .HasForeignKey(ar => ar.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
