using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Relay.Models;
using Relay.Providers;

namespace Relay.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class LineupController
    {
        private readonly ILineupProvider _lineupProvider;
        
        public LineupController(ILineupProvider provider)
        {
            _lineupProvider = provider;
        }
        
        [HttpGet]
        [Route("/lineup_status.json")]
        public ActionResult<LineupStatus> GetLineupStatus()
        {
            return new LineupStatus();
        }
 
        [HttpGet]
        [Route("/lineup.json")]
        public async Task<ActionResult<IEnumerable<LineupEntry>>> GetLineup()
        {
            var entries = await _lineupProvider.Entries;
            return new ActionResult<IEnumerable<LineupEntry>>(entries);
        }

        [HttpPost]
        [Route("/lineup.post")]
        public void PostLineup()
        {
        }
    }
}