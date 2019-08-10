using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Relay.Models;
using Relay.Services;

namespace Relay.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class LineupController : Controller
    {
        private readonly RelayDbContext _lineupContext;
        private readonly LineupUpdater _lineupUpdater;
        
        public LineupController(
            RelayDbContext context,
            LineupUpdater updater)
        {
            _lineupContext = context;
            _lineupUpdater = updater;
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
                    var entry = ctx.LineupEntries.First(e => e.Number == channel);
                    return new RedirectResult(entry.Url, true);
                }
            }
            catch (InvalidOperationException)
            {
                return new NotFoundResult();
            }
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
            return new ActionResult<IEnumerable<LineupEntry>>(
                _lineupContext.LineupEntries.OrderBy(e => e.Number));
        }

        [HttpPost]
        [Route("/lineup.update")]
        public ActionResult UpdateLineup()
        {
            _lineupUpdater.Start();
            return Ok();
        }

        [HttpPost]
        [Route("/lineup.post")]
        public async Task<ActionResult<LineupEntry>> PostLineup(
            [FromQuery(Name = "favorite")] string favourite)
        {
            // Check for space and plus here - as encoding breaks the plus character
            
            if (string.IsNullOrEmpty(favourite) ||
                !(favourite[0] == ' ' || favourite[0] == '+' || favourite[0] == '-') ||
                !int.TryParse(favourite.Substring(1), out var channel))
            {
                return new BadRequestResult();
            }
            
            using (var ctx = _lineupContext)
            {
                var entry = await ctx.LineupEntries.FirstAsync(e => e.Number == channel);
                entry.Favorite = favourite[0] == '-' ? 0 : 1;
                
                await ctx.SaveChangesAsync();
                return entry;
            }
        }
    }
}