using System.Collections.Generic;

namespace Relay.Services.Discovery
{
    internal interface IPacket
    {
        PacketType Type { get; }
        
        int Length { get; }
        
        IEnumerable<Tag> Tags { get; }

        byte[] ToByteArray();
    }
}