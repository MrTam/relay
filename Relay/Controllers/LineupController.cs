using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Relay.Models;

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
        public async Task<ActionResult<LineupEntry>> PostLineup(
            [FromQuery(Name = "favorite")] string favourite)
        {
            if (string.IsNullOrEmpty(favourite) ||
                !(favourite[0] == '+' || favourite[0] == '-') ||
                !int.TryParse(favourite.Substring(1), out var channel))
            {
                return new BadRequestResult();
            }
            
            using (var ctx = _lineupContext)
            {
                var entry = await ctx.Entries.FirstAsync(e => e.Number == channel);
                entry.Favorite = favourite[0] == '+' ? 1 : 0;
                
                await ctx.SaveChangesAsync();
                return entry;
            }
        }
    }
}