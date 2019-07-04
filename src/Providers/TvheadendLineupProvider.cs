using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Relay.Models;

namespace Relay.Providers
{
    class TvheadendLineupEntry
    {
        public string Uuid { get; set; }
        public uint Number { get; set; }
        public string Name { get; set; }
    }

    class TvheadendLineupEntries
    {
        public IList<TvheadendLineupEntry> Entries { get; set; }
    }
    
    public class TvheadendLineupProvider : ILineupProvider
    {
        private static readonly HttpClient Client = new HttpClient();

        public TvheadendLineupProvider(IConfiguration config)
        {
            Client.BaseAddress = new Uri("http://192.168.1.254:9981/api/");
        }
        
        public Task<IList<LineupEntry>> Entries => FetchChannelInfo();

        private async Task<IList<LineupEntry>> FetchChannelInfo()
        {
            var res = await Client.GetAsync(
                "channel/grid?start=0&limit=999999");

            if (!res.IsSuccessStatusCode) return null;
            
            res.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var entries = await res.Content.ReadAsAsync<TvheadendLineupEntries>();
                
            return entries.Entries
                .Select(e => new LineupEntry
                {
                    Name = e.Name,
                    Number = e.Number,
                    Url = $"{Client.BaseAddress.AbsoluteUri}stream/channel/{e.Uuid}" 
                })
                .ToList();
        }
    }
}