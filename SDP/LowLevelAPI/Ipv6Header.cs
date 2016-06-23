using System;
using System.Net;

namespace SDP.LowLevelAPI
{
    public class Ipv6Header : ProtocolHeader
    {
        public static int Ipv6HeaderLength = 40;
        private uint ipFlow;
        private ushort ipPayloadLength;

        public byte Version { get; set; }

        public byte TrafficClass { get; set; }

        public uint Flow
        {
            get { return (uint) IPAddress.NetworkToHostOrder((int) ipFlow); }
            set { ipFlow = (uint) IPAddress.HostToNetworkOrder((int) value); }
        }

        public ushort PayloadLength
        {
            get { return (ushort) IPAddress.NetworkToHostOrder((short) ipPayloadLength); }
            set { ipPayloadLength = (ushort) IPAddress.HostToNetworkOrder((short) value); }
        }

        public byte NextHeader { get; set; }

        public byte HopLimit { get; set; }

        public IPAddress SourceAddress { get; set; }

        public IPAddress DestinationAddress { get; set; }

        public Ipv6Header()
        {
            Version = 6;
            TrafficClass = 0;
            ipFlow = 0;
            ipPayloadLength = 0;
            NextHeader = 0;
            HopLimit = 32;
            SourceAddress = IPAddress.IPv6Any;
            DestinationAddress = IPAddress.IPv6Any;
        }

        public static Ipv6Header Create(byte[] ipv6Packet, ref int bytesCopied)
        {
            var ipv6Header = new Ipv6Header();
            var addressBytes = new byte[16];
            uint tempVal = 0, tempVal2 = 0;

            if (ipv6Packet.Length < Ipv6HeaderLength)
                return null;

            tempVal = ipv6Packet[0];
            tempVal = (tempVal >> 4) & 0xF;
            ipv6Header.Version = (byte) tempVal;

            tempVal = ipv6Packet[0];
            tempVal = (tempVal & 0xF) >> 4;
            ipv6Header.TrafficClass = (byte) (tempVal | (uint) ((ipv6Packet[1] >> 4) & 0xF));

            tempVal2 = ipv6Packet[1];
            tempVal2 = (tempVal2 & 0xF) << 16;
            tempVal = ipv6Packet[2];
            tempVal = tempVal << 8;
            ipv6Header.ipFlow = tempVal2 | tempVal | ipv6Packet[3];

            ipv6Header.NextHeader = ipv6Packet[4];
            ipv6Header.HopLimit = ipv6Packet[5];

            Array.Copy(ipv6Packet, 6, addressBytes, 0, 16);
            ipv6Header.SourceAddress = new IPAddress(addressBytes);

            Array.Copy(ipv6Packet, 24, addressBytes, 0, 16);
            ipv6Header.DestinationAddress = new IPAddress(addressBytes);

            bytesCopied = Ipv6HeaderLength;

            return ipv6Header;
        }

        public override byte[] GetProtocolPacketBytes(byte[] payLoad)
        {
            var offset = 0;

            var ipv6Packet = new byte[Ipv6HeaderLength + payLoad.Length];
            ipv6Packet[offset++] = (byte) ((Version << 4) | ((TrafficClass >> 4) & 0xF));

            ipv6Packet[offset++] = (byte) ((uint) ((TrafficClass << 4) & 0xF0) | (Flow >> 16) & 0xF);
            ipv6Packet[offset++] = (byte) ((Flow >> 8) & 0xFF);
            ipv6Packet[offset++] = (byte) (Flow & 0xFF);

            Console.WriteLine("Next header = {0}", NextHeader);

            byte[] byteValue = BitConverter.GetBytes(ipPayloadLength);
            Array.Copy(byteValue, 0, ipv6Packet, offset, byteValue.Length);
            offset += byteValue.Length;

            ipv6Packet[offset++] = NextHeader;
            ipv6Packet[offset++] = HopLimit;

            byteValue = SourceAddress.GetAddressBytes();
            Array.Copy(byteValue, 0, ipv6Packet, offset, byteValue.Length);
            offset += byteValue.Length;

            byteValue = DestinationAddress.GetAddressBytes();
            Array.Copy(byteValue, 0, ipv6Packet, offset, byteValue.Length);
            offset += byteValue.Length;

            Array.Copy(payLoad, 0, ipv6Packet, offset, payLoad.Length);

            return ipv6Packet;
        }
    }
}