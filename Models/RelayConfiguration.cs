using System.Diagnostics.CodeAnalysis;
using Relay.Providers;

namespace Relay.Models
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    public sealed class RelayConfiguration
    {
        public LineupProvider Provider { get; set; } = LineupProvider.Tvheadend; 
        
        public string Url { get; set; } = "http://127.0.0.1:5004";

        public int TunerCount { get; set; } = 2;

        public int TunerDeviceId { get; set; } = 1337;

        public uint UpdateIntervalSeconds { get; set; } = 3600;
    }
}