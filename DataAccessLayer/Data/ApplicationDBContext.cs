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
        public DbSet<Course> Courses { get; set; }
        
        // New consultation DbSets
        public DbSet<ConsultationRequest> ConsultationRequests { get; set; }
        public DbSet<ConsultationSession> ConsultationSessions { get; set; }
        public DbSet<ConsultationReview> ConsultationReviews { get; set; }
        public DbSet<ConsultantWorkingHour> ConsultantWorkingHours { get; set; }
        public DbSet<Certificate> Certificates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            

            builder.Entity<Blog>(entity =>
            {
                entity.HasKey(b => b.BlogId);

                entity.Property(b => b.PostedAt)
                     .HasDefaultValueSql("GETDATE()");

                entity.HasOne(b=>b.PostedBy)
                    .WithMany(b=>b.BlogsPosted)
                    .HasForeignKey(b=>b.PostedById)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.ApprovedBy)
                    .WithMany(b=>b.BlogsApproved)
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
                    .WithMany(c=>c.Comments)
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
            
            // New consultation configurations
            builder.Entity<ConsultationRequest>(entity =>
            {
                entity.HasKey(cr => cr.Id);
                
                entity.Property(cr => cr.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
                
                entity.Property(cr => cr.Status)
                    .HasDefaultValue(ConsultationStatus.Pending);
                
                entity.HasOne(cr => cr.User)
                    .WithMany(u => u.ConsultationRequests)
                    .HasForeignKey(cr => cr.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(cr => cr.Consultant)
                    .WithMany(u => u.ConsultationRequestsAsConsultant)
                    .HasForeignKey(cr => cr.ConsultantId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(cr => cr.ConsultationSession)
                    .WithOne(cs => cs.ConsultationRequest)
                    .HasForeignKey<ConsultationSession>(cs => cs.ConsultationRequestId);
                
                entity.HasOne(cr => cr.ConsultantWorkingHour)
                    .WithMany()
                    .HasForeignKey(cr => cr.ConsultantWorkingHourId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            builder.Entity<ConsultationSession>(entity =>
            {
                entity.HasKey(cs => cs.Id);
                
                entity.Property(cs => cs.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
            });
            
            builder.Entity<ConsultationReview>(entity =>
            {
                entity.HasKey(cr => cr.Id);
                
                entity.Property(cr => cr.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
                
                entity.Property(cr => cr.Rating)
                    .HasDefaultValue(5);
                entity.HasOne(cr => cr.ConsultationSession)
                    .WithOne(cs => cs.ConsultationReview)
                    .HasForeignKey<ConsultationReview>(cr => cr.ConsultationSessionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            builder.Entity<ConsultantWorkingHour>(entity =>
            {
                entity.HasKey(cwh => cwh.Id);
                
                entity.Property(cwh => cwh.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
                    
                entity.Property(cwh => cwh.Status)
                    .HasDefaultValue(WorkingHourStatus.Available);
                
                entity.HasOne(cwh => cwh.Consultant)
                    .WithMany()
                    .HasForeignKey(cwh => cwh.ConsultantId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(cwh => cwh.ConsultationRequest)
                    .WithMany()
                    .HasForeignKey(cwh => cwh.ConsultationRequestId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
