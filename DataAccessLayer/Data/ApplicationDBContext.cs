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
        }
    }
}
