using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Relay.Models;
using Relay.Providers;
using Relay.Services;

namespace Relay.Pages
{
    public class IndexModel : PageModel
    {
        private readonly LineupContext _context;
        
        private readonly LineupUpdater _updater;
        
        public RelayConfiguration Config { get; }
        
        public Uri LineupUri { get; }

        public IEnumerable<LineupEntry> Channels =>
            _context.Entries.OrderBy(e => e.Number);

        public int HdCount =>
            _context.Entries.Count(e => e.HD == 1);
        
        public int FavouritesCount =>
            _context.Entries.Count(e => e.Favorite == 1);

        public int ChannelCount => _context.Entries.Count();

        public IndexModel(
            IOptionsSnapshot<RelayConfiguration> config,
            ILineupProvider lineupProvider,
            LineupUpdater lineupUpdater,
            LineupContext context)
        {
            _context = context;
            _updater = lineupUpdater;
            
            Config = config.Value;
            LineupUri = lineupProvider.InfoUri;
        }

        public void OnPostReload()
        {
            _updater.Start();
        }
    }
}
