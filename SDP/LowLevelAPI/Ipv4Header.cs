using System;
using System.Net;

namespace SDP.LowLevelAPI
{
    public class Ipv4Header : ProtocolHeader
    {
        public static int Ipv4HeaderLength = 20;
        private ushort ipChecksum;
        private ushort ipId;
        private byte ipLength; 
        private ushort ipOffset;
        private ushort ipTotalLength;

        public byte Version { get; set; }

        public byte Length
        {
            get { return (byte) (ipLength*4); }
            set { ipLength = (byte) (value/4); }
        }

        public byte TypeOfService { get; set; }

        public ushort TotalLength
        {
            get { return (ushort) IPAddress.NetworkToHostOrder((short) ipTotalLength); }
            set { ipTotalLength = (ushort) IPAddress.HostToNetworkOrder((short) value); }
        }

        public ushort Id
        {
            get { return (ushort) IPAddress.NetworkToHostOrder((short) ipId); }
            set { ipId = (ushort) IPAddress.HostToNetworkOrder((short) value); }
        }
        public ushort Offset
        {
            get { return (ushort) IPAddress.NetworkToHostOrder((short) ipOffset); }
            set { ipOffset = (ushort) IPAddress.HostToNetworkOrder((short) value); }
        }

        public byte Ttl { get; set; }

        public byte Protocol { get; set; }

        public ushort Checksum
        {
            get { return (ushort) IPAddress.NetworkToHostOrder((short) ipChecksum); }
            set { ipChecksum = (ushort) IPAddress.HostToNetworkOrder((short) value); }
        }
        public IPAddress SourceAddress { get; set; }
        public IPAddress DestinationAddress { get; set; }
        public Ipv4Header()
        {
            Version = 4;
            ipLength = (byte) Ipv4HeaderLength; 
            TypeOfService = 0;
            ipId = 0;
            ipOffset = 0;
            Ttl = 1;
            Protocol = 0;
            ipChecksum = 0;
            SourceAddress = IPAddress.Any;
            DestinationAddress = IPAddress.Any;
        }

        public static Ipv4Header Create(byte[] ipv4Packet, ref int bytesCopied)
        {
            var ipv4Header = new Ipv4Header();

            if (ipv4Packet.Length < Ipv4HeaderLength)
                return null;

            ipv4Header.Version = (byte) ((ipv4Packet[0] >> 4) & 0xF);
            ipv4Header.ipLength = (byte) (ipv4Packet[0] & 0xF);
            ipv4Header.TypeOfService = ipv4Packet[1];
            ipv4Header.ipTotalLength = BitConverter.ToUInt16(ipv4Packet, 2);
            ipv4Header.ipId = BitConverter.ToUInt16(ipv4Packet, 4);
            ipv4Header.ipOffset = BitConverter.ToUInt16(ipv4Packet, 6);
            ipv4Header.Ttl = ipv4Packet[8];
            ipv4Header.Protocol = ipv4Packet[9];
            ipv4Header.ipChecksum = BitConverter.ToUInt16(ipv4Packet, 10);

            ipv4Header.SourceAddress = new IPAddress(BitConverter.ToUInt32(ipv4Packet, 12));
            ipv4Header.DestinationAddress = new IPAddress(BitConverter.ToUInt32(ipv4Packet, 16));

            bytesCopied = ipv4Header.Length;

            return ipv4Header;
        }

        public override byte[] GetProtocolPacketBytes(byte[] payLoad)
        {
            var index = 0;

            var ipv4Packet = new byte[Ipv4HeaderLength + payLoad.Length];

            ipv4Packet[index++] = (byte) ((Version << 4) | ipLength);
            ipv4Packet[index++] = TypeOfService;

            byte[] byteValue = BitConverter.GetBytes(ipTotalLength);
            Array.Copy(byteValue, 0, ipv4Packet, index, byteValue.Length);
            index += byteValue.Length;

            byteValue = BitConverter.GetBytes(ipId);
            Array.Copy(byteValue, 0, ipv4Packet, index, byteValue.Length);
            index += byteValue.Length;

            byteValue = BitConverter.GetBytes(ipOffset);
            Array.Copy(byteValue, 0, ipv4Packet, index, byteValue.Length);
            index += byteValue.Length;

            ipv4Packet[index++] = Ttl;
            ipv4Packet[index++] = Protocol;
            ipv4Packet[index++] = 0; 
            ipv4Packet[index++] = 0; 

            byteValue = SourceAddress.GetAddressBytes();
            Array.Copy(byteValue, 0, ipv4Packet, index, byteValue.Length);
            index += byteValue.Length;

            byteValue = DestinationAddress.GetAddressBytes();
            Array.Copy(byteValue, 0, ipv4Packet, index, byteValue.Length);
            index += byteValue.Length;

            Array.Copy(payLoad, 0, ipv4Packet, index, payLoad.Length);
            index += payLoad.Length;

            Checksum = ComputeChecksum(ipv4Packet);

            byteValue = BitConverter.GetBytes(ipChecksum);
            Array.Copy(byteValue, 0, ipv4Packet, 10, byteValue.Length);

            return ipv4Packet;
        }

        public override byte[] GetProtocolPacketBytes(byte[] payLoad, ushort crc)
        {
            var index = 0;

            var ipv4Packet = new byte[Ipv4HeaderLength + payLoad.Length];

            ipv4Packet[index++] = (byte)((Version << 4) | ipLength);
            ipv4Packet[index++] = TypeOfService;

            byte[] byteValue = BitConverter.GetBytes(ipTotalLength);
            Array.Copy(byteValue, 0, ipv4Packet, index, byteValue.Length);
            index += byteValue.Length;

            byteValue = BitConverter.GetBytes(ipId);
            Array.Copy(byteValue, 0, ipv4Packet, index, byteValue.Length);
            index += byteValue.Length;

            byteValue = BitConverter.GetBytes(ipOffset);
            Array.Copy(byteValue, 0, ipv4Packet, index, byteValue.Length);
            index += byteValue.Length;

            ipv4Packet[index++] = Ttl;
            ipv4Packet[index++] = Protocol;
            ipv4Packet[index++] = 0; 
            ipv4Packet[index++] = 0; 

            byteValue = SourceAddress.GetAddressBytes();
            Array.Copy(byteValue, 0, ipv4Packet, index, byteValue.Length);
            index += byteValue.Length;

            byteValue = DestinationAddress.GetAddressBytes();
            Array.Copy(byteValue, 0, ipv4Packet, index, byteValue.Length);
            index += byteValue.Length;

            Array.Copy(payLoad, 0, ipv4Packet, index, payLoad.Length);
            index += payLoad.Length;

            Checksum = crc;

            byteValue = BitConverter.GetBytes(ipChecksum);
            Array.Copy(byteValue, 0, ipv4Packet, 10, byteValue.Length);

            return ipv4Packet;
        }
    }
}