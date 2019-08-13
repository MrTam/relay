using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Relay.Models;

namespace Relay.Controllers
{
    [ApiController]
    public class LogController : Controller
    {
        private const int LogBatchSize = 25;
        private readonly ILogger _log;
        private readonly RelayDbContext _dbContext;

        public LogController(
            ILogger<LogController> log,
            RelayDbContext context)
        {
            _log = log;
            _dbContext = context;
        }

        [HttpGet]
        [Route("/status/logs/{page?}")]
        public ActionResult GetLogs(
            int page = 1)
        {
            var logs = _dbContext.LogEntries
                .OrderByDescending(e => e.Id)
                .AsQueryable();

            var count = logs.Count();

            var pages = count / LogBatchSize; 
            pages = (count % LogBatchSize != 0) ? pages+1 : pages;

            if(page == 0 || page > pages)
                return NotFound();

            return new JsonResult(new
            {
                pages = pages,
                count = count,
                logs = logs
                    .Skip((page - 1) * LogBatchSize)
                    .Take(LogBatchSize)
            });
        }

        [HttpDelete]
        [Route("/status/logs")]
        public ActionResult DeleteLogs()
        {
            _dbContext.LogEntries.RemoveRange(_dbContext.LogEntries);
            _dbContext.SaveChanges();
            _log.LogInformation("Cleared all application logs");

            return Ok();
        }
    }
}