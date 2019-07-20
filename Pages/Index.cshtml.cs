using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Relay.Models;

namespace Relay.Pages
{
    public class Index : PageModel
    {
        private readonly LineupContext _context;
        
        public RelayConfiguration Config { get; }

        public IEnumerable<LineupEntry> Channels =>
            _context.Entries.OrderBy(e => e.Number);

        public int HdCount =>
            _context.Entries.Count(e => e.HD == 1);
        
        public int FavouritesCount =>
            _context.Entries.Count(e => e.Favorite == 1);

        public int ChannelCount => _context.Entries.Count();

        public Index(
            IOptionsSnapshot<RelayConfiguration> config,
            LineupContext context)
        {
            Config = config.Value;
            _context = context;
        }
    }
}
