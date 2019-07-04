using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Relay.Models;
using Relay.Utils;

namespace Relay.Controllers
{
    [ApiController]
    [Route("/")]
    [Route("/device.xml")]
    [Produces("application/xml")]
    public class DeviceController
    {
        [HttpGet]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public XmlResult<UpnpDevice> Get()
        {
            return new UpnpDevice
            {
                UrlBase = "http://192.168.1.35:5004",
                Device =
                {
                    DeviceId = "123",
                    Identifier = "MrTam Amazing Tuner",
                    Manufacturer = "MrTam",
                    ModelName = "Amazing Tuner",
                    ModelNumber = "1337" 
                }
            };
        }
    }
}