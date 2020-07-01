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
    }
}