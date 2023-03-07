using DevEncurtaUrl.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevEncurtaUrl.API.Persistence
{
    public class DevEncurtaUrlDbContext : DbContext
    {
        public DbSet<ShortenedCustomLink> Links { get; set; }

        public DevEncurtaUrlDbContext(DbContextOptions<DevEncurtaUrlDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ShortenedCustomLink>(e =>
            {
                e.HasKey(l => l.Id);
            });
        }
    }
}