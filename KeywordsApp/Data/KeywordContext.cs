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
            modelBuilder.Entity<User>();
            modelBuilder.Entity<User>().HasIndex(i => i.FirstName, "FirstnameIndex");
            modelBuilder.Entity<User>().HasIndex(i => i.LastName, "LastnameIndex");
            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<Keyword>().HasIndex(i => i.Name, "NameIndex");
        }
    }
}