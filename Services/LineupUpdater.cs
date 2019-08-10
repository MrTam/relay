using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Relay.Models;
using Relay.Providers;

namespace Relay.Services
{
    public sealed class LineupUpdater
    {
        private readonly ILogger _log;
        private readonly RelayDbContext _lineupContext;
        private readonly ILineupProvider _provider;
        private readonly Timer _updateTimer;
        private bool _running;

        public LineupUpdater(
            ILogger<LineupUpdater> log,
            IOptionsSnapshot<RelayConfiguration> config,
            RelayDbContext dbContext,
            ILineupProvider provider)
        {
            _log = log;
            _lineupContext = dbContext;
            _provider = provider;

            _updateTimer = new Timer(TimeSpan.FromSeconds(
                config.Value.UpdateIntervalSeconds).TotalMilliseconds)
            {
                AutoReset = true
            };
            
            _updateTimer.Elapsed += async (_, __) => await UpdateGuide();
        }

        public async void Start() => await UpdateGuide();

        private async Task UpdateGuide()
        {
            _log.LogInformation("Updating channel lineup with provider: {0}", _provider.ProviderType);
            
            var lineupEntries = await _provider.UpdateLineup();
            var lineupChannels = lineupEntries.Select(e => e.Number).ToList();

            int added = 0, updated = 0, removed = 0;

            // Remove missing
            
            foreach(var e in _lineupContext.LineupEntries
                .Where(e => !lineupChannels.Contains(e.Number)))
            {
                _log.LogInformation("Removed channel {0}: {1}", e.Number, e.Name);
                _lineupContext.Remove(e);
                ++removed;
            }
            
            // Update existing

            foreach(var e in _lineupContext.LineupEntries
                .Where(e => lineupChannels.Contains(e.Number)))
            {
                var other = lineupEntries.First(o => o.Number == e.Number);
                if (e.Name.Equals(other.Name) && e.Url.Equals(other.Url) && e.HD.Equals(other.HD)) continue;
                
                e.Name = other.Name;
                e.Url = other.Url;
                e.HD = other.HD;
                
                _log.LogInformation("Updated channel {0}: {1} => {2}", e.Name, e.Number, e.Url);
                
                ++updated;
            }
            
            // Add new

            var currentChannels = _lineupContext.LineupEntries.Select(e => e.Number).ToList();
            
            foreach (var e in lineupEntries.Where(e => !currentChannels.Contains(e.Number)))
            {
                _log.LogInformation("Added channel {0}: {1} => {2}", e.Number, e.Name, e.Url);
                _lineupContext.Add(e);
                ++added;
            }

            await _lineupContext.SaveChangesAsync();
            
            _log.LogInformation("Lineup update complete: {0} added, {1} updated, {2} removed",
                added, updated, removed);
            
            if(!_running)
            {
                _running = true;
                _updateTimer.Start();
            }
        }
    }
}