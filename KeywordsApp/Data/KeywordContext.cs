using KeywordsApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KeywordsApp.Data
{
    public class KeywordContext : IdentityDbContext<UserEntity>
    {
        public KeywordContext(DbContextOptions<KeywordContext> options) : base(options)
        {
        }

        public DbSet<Keyword> Keywords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>();
            modelBuilder.Entity<UserEntity>().HasIndex(i => i.FirstName, "FirstnameIndex");
            modelBuilder.Entity<UserEntity>().HasIndex(i => i.LastName, "LastnameIndex");
            modelBuilder.Entity<UserEntity>().ToTable("Users");

            modelBuilder.Entity<Keyword>().HasIndex(i => i.Name, "NameIndex");
        }
    }
}