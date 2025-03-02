using ByteBridge.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ByteBridge.Data
{
    public class AppDBContext(DbContextOptions<AppDBContext> options) : IdentityDbContext(options)
    {
        public DbSet<Files> Files { get; set; }
    }
}
