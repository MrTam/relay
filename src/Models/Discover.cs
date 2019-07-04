using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Relay.Models
{
    public class Discover
    {
        public string FriendlyName { get; set; }

        public string Manufacturer { get; set; } = "Silicondust";
        
        public string ModelNumber { get; set; }

        public string FirmwareName { get; } = "hdhomerun_atsc";
        
        public uint TunerCount { get; set; }

        public string FirmwareVersion { get; } = "1337";

        public string DeviceId { get; set; }

        public string DeviceAuth { get; } = "NoAuth";
        
        [JsonProperty("BaseURL")]
        public string BaseUrl { get; set; }

        [JsonProperty("LineupURL")]
        public string LineupUrl => $"{BaseUrl}/lineup.json";
    }
}