using System.Collections.Generic;
using Newtonsoft.Json;
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
        [JsonProperty("GuideNumber")]
        public uint Number { get; set; }
        
        [JsonProperty("GuideName")]
        public string Name { get; set; }
        
        [JsonProperty("URL")]
        public string Url { get; set; }

        [JsonProperty("HD")]
        public int HD { get; set; } = 0;
    }
}