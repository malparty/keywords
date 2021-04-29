using KeywordsApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KeywordsApp.Data
{
    public class KeywordContext : IdentityDbContext<User>
    {
        public KeywordContext(DbContextOptions<KeywordContext> options) : base(options)
        {
        }

        public DbSet<Keyword> Keywords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Keyword>().ToTable("Keywords");
        }
    }
}