using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Relay.Models
{
    public class RelayDbContext : DbContext
    {
        private readonly RelayConfiguration _config;

        public RelayDbContext(
            DbContextOptions<RelayDbContext> options,
            IOptionsSnapshot<RelayConfiguration> config)
            : base(options)
        {
            _config = config.Value;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LineupEntry>()
                .HasQueryFilter(e => e.Provider == _config.Provider);

            modelBuilder.Entity<LineupEntry>()
                .Property(p => p.CreatedTime)
                .HasDefaultValueSql("DATETIME('now')");
        }

        public DbSet<LineupEntry> LineupEntries { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
    }
}