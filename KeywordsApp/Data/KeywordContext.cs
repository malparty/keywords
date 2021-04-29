using KeywordsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace KeywordsApp.Data
{
    public class KeywordContext : DbContext
    {
        public KeywordContext(DbContextOptions<KeywordContext> options) : base(options)
        {
        }

        public DbSet<Keyword> Keywords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Keyword>().ToTable("Keywords");
        }
    }
}