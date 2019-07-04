using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Relay.Models;

namespace Relay.Controllers
{
    [ApiController]
    [Route("/discover.json")]
    [Produces("application/json")]
    public class DiscoverController
    {
        [HttpGet]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public ActionResult<Discover> GetDiscoverData()
        {
            return new Discover
            {
                FriendlyName = "test",
                BaseUrl = "http://192.168.1.35:5004",
                DeviceId = "Foobarwibble",
                TunerCount = 4,
                ModelNumber = "1337"
                
            };
        }
    }
}