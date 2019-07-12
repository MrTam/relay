using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Relay.Models;

namespace Relay.Controllers
{
    [ApiController]
    [Route("/discover.json")]
    [Produces("application/json")]
    public class DiscoverController
    {
        private readonly RelayConfiguration _config;

        public DiscoverController(IOptionsSnapshot<RelayConfiguration> config) => _config = config.Value;

        [HttpGet]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public ActionResult<Discover> GetDiscoverData()
        {
            return new Discover
            {
                FriendlyName = "test",
                BaseUrl = _config.Url,
                DeviceId = "Foobarwibble",
                TunerCount = _config.TunerCount,
                ModelNumber = "1337"
                
            };
        }
    }
}