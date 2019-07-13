using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Relay.Models;
using Relay.Utils;

namespace Relay.Controllers
{
    [ApiController]
    [Route("/device.xml")]
    [Produces("application/xml")]
    public class DeviceController
    {
        private readonly RelayConfiguration _config;

        public DeviceController(IOptionsSnapshot<RelayConfiguration> config) => _config = config.Value;

        [HttpGet]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public XmlResult<UpnpDevice> Get()
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
    }
}