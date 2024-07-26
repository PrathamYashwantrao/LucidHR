
using LucidHR.Models;
using Microsoft.EntityFrameworkCore;
using static LucidHR.Models.Performance;

namespace LucidHR.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //---------------------combined--------------------------
        public DbSet<LeaveRequest> LeaveRequest { get; set; }

        public DbSet<JoiningForm> JoiningForm { get; set; }



        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<f16> f16s { get; set; }

        //--calendar-----------------------
        public DbSet<Event> Events { get; set; }

        public DbSet<userLogin> userLogin { get; set; }
        public DbSet<Career> Careers { get; set; }


        //--------------------Performance -----------------

        public DbSet<Question> Questions { get; set; }
        public DbSet<PerformanceReview> PerformanceReviews { get; set; }



        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    // Additional model configuration if needed
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationships
            modelBuilder.Entity<PerformanceReview>()
                .HasOne(pr => pr.Question)
                .WithMany()
                .HasForeignKey(pr => pr.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
