using System.Xml.Serialization;
using Relay.Utils;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

namespace Relay.Models
{
    public class SpecVersion
    {
        [XmlElement("major")]
        public int Major { get; set; } = 1;
        
        [XmlElement("minor")]
        public int Minor { get; set; } = 0;
    }

    public class Device
    {
        [XmlElement("deviceType")]
        public string Type { get; set; } = Constants.UpnpDeviceType;
        
        [XmlElement("friendlyName")]
        public string Identifier { get; set; }
        
        [XmlElement("manufacturer")]
        public string Manufacturer { get; set; }
        
        [XmlElement("modelName")]
        public string ModelName { get; set; }

        [XmlElement("modelNumber")]
        public string ModelNumber { get; set; }
        
        [XmlElement("serialNumber")]
        public string Serial { get; set; }
        
        [XmlElement("UDN")]
        public string DeviceId { get; set; }
    }
    
    [XmlRoot("root", Namespace = Constants.UpnpXmlNamespace)]
    public class UpnpDevice
    {
        [XmlElement("specVersion")]
        public SpecVersion SpecVersion { get; set; } = new SpecVersion();
        
        [XmlElement("URLBase")]
        public string UrlBase { get; set; }
        
        [XmlElement("device")]
        public Device Device { get; set; } = new Device();
        
    }
}