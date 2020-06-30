using Microsoft.EntityFrameworkCore;
using Xortd.Models;

namespace Xortd.Data
{
    public interface IUrlDbContext
    {
        DbSet<ShortUrl> ShortUrls { get; set; }
    }
}