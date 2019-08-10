using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Relay.Models;
using Relay.Providers;
using Relay.Utils;

namespace Relay.Controllers
{
    [ApiController]
    public class DeviceController
    {
        private readonly RelayConfiguration _config;
        private readonly ILineupProvider _provider;
        public DeviceController(
            IOptionsSnapshot<RelayConfiguration> config,
            ILineupProvider provider)
        {
            _config = config.Value;
            _provider = provider;
        }

        [HttpGet]
        [Route("/device.xml")]
        [Produces("application/xml")]
        public XmlResult<UpnpDevice> GetUpnpDevice()
        {
            return new UpnpDevice
            {
                UrlBase = _config.Url,
                Device =
                {
                    DeviceId = _config.TunerDeviceId.ToString(),
                    Identifier = "HDHomerun (Relay)",
                    Manufacturer = "Relay",
                    ModelName = "HDHomerun (Relay)",
                    ModelNumber = "1337" 
                }
            };
        }

        [HttpGet]
        [Route("/discover.json")]
        [Produces("application/json")]
        public ActionResult<Discover> GetDiscoverData()
        {
            return new Discover
            {
                FriendlyName = "HDHomerun (Relay)",
                BaseUrl = _config.Url,
                DeviceId = _config.TunerDeviceId.ToString(),
                TunerCount = _config.TunerCount,
                ModelNumber = "1337"
                
            };
        }

        [HttpGet]
        [Route("/status.json")]
        [Produces("application/json")]
        public JsonResult GetStatus()
        {
            return new JsonResult(new
            {
                tuners = _config.TunerCount,
                provider = _config.Provider.ToString(),
                providerUrl = _provider.InfoUri,
                refreshInterval = TimeSpan.FromSeconds(_config.UpdateIntervalSeconds)
            });
        }
    }
}