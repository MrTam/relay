namespace Relay.Services.Discovery
{
    internal static class PacketFactory
    {
        private const int TunerDevice = 0x1;
        
        public static IPacket CreateDiscoverReply(
            int deviceId,
            int tunerCount,
            string url)
        {
            var packet = new Packet(PacketType.DiscoverReply);
            
            packet.SetTag(Tag.DeviceType, TunerDevice);
            packet.SetTag(Tag.DeviceId, deviceId);
            packet.SetTag(Tag.GetSetName, url);
            packet.SetTag(Tag.TunerCount, tunerCount);

            return packet;
        }
    }
}