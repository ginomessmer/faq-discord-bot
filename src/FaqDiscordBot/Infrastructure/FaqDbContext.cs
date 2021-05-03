using FaqDiscordBot.Models;
using Microsoft.EntityFrameworkCore;

namespace FaqDiscordBot.Infrastructure
{
    public sealed class FaqDbContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }

        public FaqDbContext(DbContextOptions<FaqDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .OwnsMany(x => x.Phrasings, x =>
                {
                    x.WithOwner();
                    x.ToTable("QuestionPhrasings");
                });

            modelBuilder.Entity<Question>()
                .OwnsOne(x => x.Answer, x =>
                {
                    x.WithOwner();
                    x.ToTable("Answers");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}