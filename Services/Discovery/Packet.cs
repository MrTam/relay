using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Force.Crc32;

namespace Relay.Services.Discovery
{
    internal class PacketException : Exception
    {
    }

    internal class InvalidLengthException : PacketException
    {
    }

    internal class InvalidTypeException : PacketException
    {
    }

    internal class InvalidCrcException : PacketException
    {
    }

    /// <summary>
    /// Packet format (according to libhdhomerun documentation):
    ///
    /// * uint16_t  Packet type (BE)
    /// * uint16_t  Payload length (bytes) (BE)
    /// * uint8_t[] Payload data (0-n bytes).
    /// * uint32_t  CRC (Ethernet style 32-bit CRC) (LE)
    ///
    /// Payload is an array of tags:
    ///
    /// * uint8_t   Tag
    /// * varlen    Length
    /// * uint8_t[] Value (0-n bytes)
    ///
    /// Tag length is variable - if MSB is set, then it is two bytes (BE) else 1 byte.
    /// </summary>
    internal sealed class Packet : IPacket
    {
        private readonly IDictionary<Tag, byte[]> _tags = new Dictionary<Tag, byte[]>();
        
        public PacketType Type { get; }

        public int Length { get; private set; }

        public IEnumerable<Tag> Tags => _tags.Keys; 
    
        public Packet(PacketType type)
        {
            Type = type;
        }

        public void SetTag(Tag tag, int value) =>
            SetTag(tag, BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)));

        public void SetTag(Tag tag, string value) =>
            SetTag(tag, Encoding.ASCII.GetBytes(value));

        public void SetTag(Tag tag, byte[] value)
        {
            if (_tags.ContainsKey(tag))
            {
                var toSubtract = _tags[tag].Length;
                toSubtract += toSubtract > 127 ? 3 : 2;
                Length -= toSubtract;
            }

            _tags[tag] = value;
            Length += (value.Length > 127 ? 3 : 2) + value.Length;
        }
        
        public static IPacket FromBytes(byte[] data)
        {
            if (data.Length < 7)
            {
                throw new InvalidLengthException();
            }

            var rawType = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(data));
            
            if(!Enum.IsDefined(typeof(PacketType), rawType))
            {
                throw new InvalidTypeException();
            }
            
            if (!Crc32Algorithm.IsValidWithCrcAtEnd(data))
            {
                throw new InvalidCrcException();
            }
            
            var type = (PacketType) rawType;
            
            var ret = new Packet(type)
            {
                Length = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(data, 2))
            };
            
            var offset = 4;
            
            while (offset < ret.Length)
            {
                var tag = (Tag) data[offset++];

                int len;
                
                if ((data[offset] & 0x80) == 0)
                {
                    len = data[offset++];
                }
                else
                {
                    len = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(data, offset));
                    offset += 2;
                }

                var res = new byte[len];
                Array.Copy(data, offset, res, 0, len);
                ret._tags.Add(tag, res);
                
                offset += len;
            }

            return ret;
        }

        public byte[] ToByteArray()
        {
            var len = 2 + 2 + Length + 4;
            var buf = new byte[len];

            // Type
            
            Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short) Type)),
                0, buf, 0, 2);
            
            // Length
            
            Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short) Length)),
                0,buf, 2, 2);
            
            // Payload

            var offset = 4;

            foreach (var (tag, value) in _tags)
            {
                buf[offset++] = (byte) tag;

                if (value.Length > 127)
                {
                    var tagLen = IPAddress.HostToNetworkOrder((short) value.Length);
                    tagLen |= 0x80;

                    var lenBytes = BitConverter.GetBytes(tagLen);
                    Array.Copy(lenBytes, 0, buf, offset, 2);
                    offset += 2;
                }
                else
                {
                    buf[offset++] = (byte) value.Length;
                }
                
                Array.Copy(value, 0, buf, offset, value.Length);
                offset += value.Length;
            }

            var crc = BitConverter.GetBytes(Crc32Algorithm.Compute(buf, 0, len - 4));
            Array.Copy(crc, 0, buf, offset, crc.Length);
            
            return buf;
        }
    }
}