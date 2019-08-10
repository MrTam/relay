using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Relay.Models;

namespace Relay.Controllers
{
    [ApiController]
    public class LogController : Controller
    {
        private readonly RelayDbContext _dbContext;

        public LogController(RelayDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        [Route("/logs")]
        public ActionResult<IEnumerable<LogEntry>> GetLogs()
        {
            return new ActionResult<IEnumerable<LogEntry>>(
                _dbContext.LogEntries.AsEnumerable());
        }
    }
}