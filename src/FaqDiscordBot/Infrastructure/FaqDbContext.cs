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
            modelBuilder.Entity<Question>(b =>
            {
                b.OwnsMany(x => x.Phrasings, x =>
                {
                    x.WithOwner();
                    x.ToTable("QuestionPhrasings");
                });

                b.OwnsOne(x => x.Answer, x =>
                {
                    x.WithOwner();
                    x.ToTable("Answers");
                });

                b.OwnsOne(x => x.Meta, x => x.WithOwner());
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}