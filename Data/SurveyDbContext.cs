using Microsoft.EntityFrameworkCore;
using SurveyApi.Models;

namespace SurveyApi.Data
{
    public class SurveyDbContext : DbContext
    {
        public SurveyDbContext(DbContextOptions<SurveyDbContext> options) : base(options)
        {
        }

        public DbSet<PersonalDetails> PersonalDetails { get; set; }
        public DbSet<Options> Options { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite primary key for Options table
            modelBuilder.Entity<Options>()
                .HasKey(o => new { o.SurveyID, o.OptionValue });

            // Configure relationship
            modelBuilder.Entity<Options>()
                .HasOne(o => o.PersonalDetails)
                .WithMany(p => p.Options)
                .HasForeignKey(o => o.SurveyID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure table names to match database
            modelBuilder.Entity<PersonalDetails>()
                .ToTable("personaldetails");

            modelBuilder.Entity<Options>()
                .ToTable("options");
        }
    }
}
