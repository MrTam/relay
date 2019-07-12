using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Relay.Models;

namespace Relay.Providers
{
    public class TvheadendLineupProviderConfig
    {
        public string Url { get; set; }
    }

    internal class TvheadendLineupEntry
    {
        public string Uuid { get; set; }
        public uint Number { get; set; }
        public string Name { get; set; }
    }

    internal class TvheadendLineupEntries
    {
        public IList<TvheadendLineupEntry> Entries { get; set; }
    }
    
    public class TvheadendLineupProvider : ILineupProvider
    {
        private readonly ILogger _log;
        private readonly LineupContext _lineupContext;
        private static readonly HttpClient Client = new HttpClient();

        public TvheadendLineupProvider(
            ILogger<TvheadendLineupProvider> log,
            IConfiguration config,
            LineupContext lineupContext)
        {
            _log = log;
            _lineupContext = lineupContext;

            var cfg = new TvheadendLineupProviderConfig();
            config.Bind("tvheadend", cfg);

            Client.BaseAddress = new Uri($"{cfg.Url}/api/");
            
            _log.LogInformation("Created TVHeadend provider (url: {0})", Client.BaseAddress.AbsoluteUri);
        }

        public LineupProvider ProviderType => LineupProvider.Tvheadend;

        public async Task<IList<LineupEntry>> UpdateLineup()
        {
            var res = await Client.GetAsync("channel/grid?start=0&limit=999999");
            if (!res.IsSuccessStatusCode) return new List<LineupEntry>();

            res.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var entries = await res.Content.ReadAsAsync<TvheadendLineupEntries>();

            _log.LogInformation("Retrieved {0} channel lineup entries", entries.Entries.Count);
           
            return entries.Entries
                .Select(e => new LineupEntry
                {
                    Name = e.Name,
                    Number = e.Number,
                    Url = $"{Client.BaseAddress.AbsoluteUri}stream/channel/{e.Uuid}",
                    HD = e.Name.EndsWith("HD") ? 1 : 0
                })
                .ToList();
        }
    }
}