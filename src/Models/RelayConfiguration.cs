using Microsoft.Extensions.Configuration;
using Relay.Providers;

namespace Relay
{
    public sealed class RelayConfiguration
    {
        public string Url { get; set; } = "http://127.0.0.1:5004";

        public uint TunerCount { get; set; } = 2;

        public LineupProvider Provider { get; set; } = LineupProvider.Tvheadend; 
    }
}