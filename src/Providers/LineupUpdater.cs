using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Relay.Models;

namespace Relay.Providers
{
    public sealed class LineupUpdater
    {
        private readonly ILogger _log;
        private readonly LineupContext _lineupContext;
        private readonly ILineupProvider _provider;
        private readonly Timer _updateTimer;
        private bool _running;

        public LineupUpdater(
            ILogger<LineupUpdater> log,
            IOptionsSnapshot<RelayConfiguration> config,
            LineupContext dbContext,
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

            if(!_running)
            {
                _running = true;
                _updateTimer.Start();
            }
        }
    }
}