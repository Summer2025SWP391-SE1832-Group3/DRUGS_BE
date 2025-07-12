using DataAccessLayer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
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
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons {  get; set; }
        public DbSet<CourseEnrollment> CourseEnrollments { get; set; }
        public DbSet<LessonProgress> LessonProgresses { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        // New consultation DbSets
        public DbSet<ConsultationRequest> ConsultationRequests { get; set; }
        public DbSet<ConsultationSession> ConsultationSessions { get; set; }
        public DbSet<ConsultationReview> ConsultationReviews { get; set; }
        public DbSet<ConsultantWorkingHour> ConsultantWorkingHours { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<CourseTestSurvey> CourseTestSurveys { get; set; }

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

                entity.HasOne(s => s.Course)
                      .WithOne(c => c.FinalExamSurvey)
                      .HasForeignKey<Survey>(s => s.CourseId) 
                      .OnDelete(DeleteBehavior.SetNull);


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
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ar => ar.SurveyResult)
                    .WithMany(sr => sr.SurveyAnswerResults)
                    .HasForeignKey(ar => ar.SurveyResultId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ar => ar.SurveyQuestion)
                    .WithMany(sq => sq.SurveyAnswerResults)
                    .HasForeignKey(ar => ar.QuestionId)
                    .OnDelete(DeleteBehavior.Restrict);
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

                entity.HasOne(s => s.Course)
                      .WithOne(c => c.FinalExamSurvey)
                      .HasForeignKey<Survey>(s => s.CourseId) 
                      .OnDelete(DeleteBehavior.SetNull);
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
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ar => ar.SurveyResult)
                    .WithMany(sr => sr.SurveyAnswerResults)
                    .HasForeignKey(ar => ar.SurveyResultId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ar => ar.SurveyQuestion)
                    .WithMany(sq => sq.SurveyAnswerResults)
                    .HasForeignKey(ar => ar.QuestionId)
                    .OnDelete(DeleteBehavior.Restrict);
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
             
             builder.Entity<Course>(entity =>
             {
                 entity.HasKey(cs => cs.Id);
                 entity.HasMany(c => c.Lessions)
                     .WithOne(l => l.Course)
                     .HasForeignKey(l => l.CourseId)
                     .OnDelete(DeleteBehavior.NoAction);

                 entity.HasMany(c => c.CourseEnrollments)
                    .WithOne(ce => ce.Course)
                    .HasForeignKey(ce => ce.CourseId)
                    .OnDelete(DeleteBehavior.NoAction);

                 entity.HasOne(c => c.FinalExamSurvey)
                   .WithOne(f => f.Course)
                   .HasForeignKey<Survey>(s => s.CourseId)
                   .OnDelete(DeleteBehavior.SetNull);
             });

             builder.Entity<Lesson>(entity =>
             {
                 entity.HasKey(l => l.Id);
                 entity.HasMany(l => l.LessonProgresses)
                       .WithOne(lq => lq.Lesson)
                       .HasForeignKey(lq => lq.LessonId)
                       .OnDelete(DeleteBehavior.NoAction);
             });

             builder.Entity<CourseEnrollment>(entity =>
             {
                 entity.HasKey(ce => ce.Id);
                 entity.HasMany(ce => ce.LessonProgresses)
                   .WithOne(lq => lq.CourseEnrollment)
                   .HasForeignKey(lq => lq.CourseEnrollmentId)
                   .OnDelete(DeleteBehavior.Cascade);

                 entity.HasOne(c => c.User)
                  .WithMany(u => u.CourseEnrollments)
                  .HasForeignKey(c => c.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
             });
            builder.Entity<Feedback>(entity =>
            {
                entity.HasKey(f => f.FeedbackId);

                entity.HasOne(f => f.User)
                    .WithMany(u => u.Feedbacks)
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.Course)
                    .WithMany(c => c.Feedbacks)
                    .HasForeignKey(f => f.CourseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }

    public class ApplicationDBContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
    {
        public ApplicationDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDBContext>();
            var connectionString = configuration.GetConnectionString("MyDB");

            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("SWP391_Project"));

            return new ApplicationDBContext(builder.Options);
        }
    }
}
