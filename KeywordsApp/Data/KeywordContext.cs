using System.Linq;
using KeywordsApp.Models;
using KeywordsApp.Models.File;
using KeywordsApp.Models.Keyword;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KeywordsApp.Data
{
    public class KeywordContext : IdentityDbContext<UserEntity>
    {
        public DbSet<KeywordEntity> Keywords { get; set; }
        public DbSet<FileEntity> Files { get; set; }

        public KeywordContext(DbContextOptions<KeywordContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>();
            modelBuilder.Entity<UserEntity>().HasIndex(i => i.FirstName, "FirstnameIndex");
            modelBuilder.Entity<UserEntity>().HasIndex(i => i.LastName, "LastnameIndex");
            modelBuilder.Entity<UserEntity>().ToTable("Users");

            modelBuilder.Entity<KeywordEntity>().HasIndex(i => i.Name, "NameIndex");
        }

        public string GetUserId(string username)
        {
            return Users.Where(x => x.Email == username)
                .Select(x => x.Id)
                .FirstOrDefault();
        }
    }
}