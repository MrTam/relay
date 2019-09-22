using Newtonsoft.Json;

namespace Relay.Models
{
    public class Discover
    {
        [JsonProperty("FriendlyName")]
        public string FriendlyName { get; set; }

        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; set; } = "Silicondust";
        
        [JsonProperty("ModelNumber")]
        public string ModelNumber { get; set; }

        [JsonProperty("FirmwareName")]
        public string FirmwareName { get; } = "hdhomerun_atsc";
        
        [JsonProperty("TunerCount")]
        public int TunerCount { get; set; }

        [JsonProperty("FirmwareVersion")]
        public string FirmwareVersion { get; } = "1337";

        [JsonProperty("DeviceID")]
        public string DeviceId { get; set; }

        [JsonProperty("DeviceAuth")]
        public string DeviceAuth { get; } = "NoAuth";
        
        [JsonProperty("BaseURL")]
        public string BaseUrl { get; set; }

        [JsonProperty("LineupURL")]
        public string LineupUrl => $"{BaseUrl}/lineup.json";
    }
}
