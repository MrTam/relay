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
        private readonly LineupContext _lineupContext;
        
        public LineupController(LineupContext context)
        {
            _lineupContext = context;
        }
        
        [HttpGet]
        [Route("/lineup_status.json")]
        public ActionResult<LineupStatus> GetLineupStatus()
        {
            return new LineupStatus();
        }
 
        [HttpGet]
        [Route("/lineup.json")]
        public ActionResult<IEnumerable<LineupEntry>> GetLineup()
        {
            return _lineupContext.Entries;
        }

        [HttpPost]
        [Route("/lineup.post")]
        public void PostLineup()
        {
        }
    }
}