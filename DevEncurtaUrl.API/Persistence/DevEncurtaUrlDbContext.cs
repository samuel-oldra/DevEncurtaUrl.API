using DevEncurtaUrl.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevEncurtaUrl.API.Persistence
{
    public class DevEncurtaUrlDbContext : DbContext
    {
        public DevEncurtaUrlDbContext(DbContextOptions<DevEncurtaUrlDbContext> options)
            : base(options)
        {
        }

        public DbSet<ShortenedCustomLink> Links { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<ShortenedCustomLink>(e => {
                e.HasKey(l => l.Id);
            });
        }
    }
}