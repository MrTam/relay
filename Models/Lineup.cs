using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Relay.Providers;
using Relay.Utils;

namespace Relay.Models
{
    public class LineupStatus
    {
        [JsonProperty("ScanInProgress")]
        public uint ScanInProgress { get; }

        [JsonProperty("ScanPossible")]
        public uint ScanPossible { get; } = 1;

        [JsonProperty("Source")]
        public string Source { get; } = Constants.LineupSource;

        [JsonProperty("SourceList")]
        public IList<string> SourceList { get; } = new List<string> {Constants.LineupSource};
    };

    public class LineupEntry
    {
        [JsonIgnore]
        public int LineupEntryId { get; set; }

        [JsonIgnore]
        public LineupProvider Provider { get; set; }

        [JsonProperty("GuideNumber")]
        public uint Number { get; set; }
        
        [JsonProperty("GuideName")]
        public string Name { get; set; }
        
        [JsonProperty("URL")]
        public string Url { get; set; }

        [JsonProperty("HD")]
        public int HD { get; set; } = 0;

        [JsonProperty("Favorite")]
        public int Favorite {get; set; } = 0;
    }

    public class LineupContext : DbContext
    {
        private readonly RelayConfiguration _config;
        
        public LineupContext(
            IOptionsSnapshot<RelayConfiguration> config,
            DbContextOptions<LineupContext> options)
            : base(options)
        {
            _config = config.Value;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LineupEntry>().HasQueryFilter(
                e => e.Provider == _config.Provider);
        }

        public DbSet<LineupEntry> Entries { get; set; }
    }
}