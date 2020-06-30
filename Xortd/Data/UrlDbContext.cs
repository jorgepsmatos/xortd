using Microsoft.EntityFrameworkCore;
using Xortd.Models;

namespace Xortd.Data
{
    public class UrlDbContext : DbContext, IUrlDbContext
    {
        public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options)
        {
        }

        public DbSet<ShortUrl> ShortUrls { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite(@"Data Source=Database\Database.db;");
        }
    }
}