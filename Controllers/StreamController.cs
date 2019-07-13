using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Relay.Models;
using SQLitePCL;

namespace Relay.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class StreamController
    {
        private readonly LineupContext _lineupContext;

        public StreamController(LineupContext lineupContext)
        {
            _lineupContext = lineupContext;
        }
        
        [HttpGet]
        [Route("/auto/v{channel}")]
        [Route("/stream/v{channel}")]
        public ActionResult FetchGuideUrl(uint channel)
        {
            try
            {
                using (var ctx = _lineupContext)
                {
                    var entry = ctx.Entries.First(e => e.Number == channel);
                    return new RedirectResult(entry.Url, true);
                }
            }
            catch (InvalidOperationException e)
            {
                return new NotFoundResult();
            }

        }
    }
}