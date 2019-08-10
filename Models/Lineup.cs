using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
        
        [JsonIgnore]
        public uint Number { get; set; }

        [NotMapped]
        [JsonProperty("GuideNumber")]
        public string GuideNumber => Number.ToString();
        
        [JsonProperty("GuideName")]
        public string Name { get; set; }
        
        [JsonProperty("URL")]
        public string Url { get; set; }

        [JsonProperty("HD")]
        public int HD { get; set; } = 0;

        [JsonProperty("Favorite")]
        public int Favorite { get; set; } = 0;

        [JsonIgnore]
        public DateTime CreatedTime { get; set; }

        [JsonIgnore, NotMapped]
        public bool IsNew => DateTime.UtcNow <= CreatedTime.AddDays(7);
    }
}