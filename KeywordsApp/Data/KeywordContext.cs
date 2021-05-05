using KeywordsApp.Models;
using KeywordsApp.Models.File;
using KeywordsApp.Models.Keyword;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KeywordsApp.Data
{
    public class KeywordContext : IdentityDbContext<UserEntity>
    {
        public KeywordContext(DbContextOptions<KeywordContext> options) : base(options)
        {
        }

        public DbSet<KeywordEntity> Keywords { get; set; }
        public DbSet<FileEntity> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>();
            modelBuilder.Entity<UserEntity>().HasIndex(i => i.FirstName, "FirstnameIndex");
            modelBuilder.Entity<UserEntity>().HasIndex(i => i.LastName, "LastnameIndex");
            modelBuilder.Entity<UserEntity>().ToTable("Users");

            modelBuilder.Entity<KeywordEntity>().HasIndex(i => i.Name, "NameIndex");
        }
    }
}