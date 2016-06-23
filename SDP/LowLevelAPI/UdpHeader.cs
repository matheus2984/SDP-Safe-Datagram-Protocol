using System;
using System.Net;

namespace SDP.LowLevelAPI
{
    public class UdpHeader : ProtocolHeader
    {
        public static int UdpHeaderLength = 8;
        private ushort destPort;
        public Ipv4Header Ipv4PacketHeader;

        public Ipv6Header Ipv6PacketHeader;
        private ushort srcPort;
        private ushort udpChecksum;
        private ushort udpLength;

        public ushort SourcePort
        {
            get { return (ushort) IPAddress.NetworkToHostOrder((short) srcPort); }
            set { srcPort = (ushort) IPAddress.HostToNetworkOrder((short) value); }
        }

        public ushort DestinationPort
        {
            get { return (ushort) IPAddress.NetworkToHostOrder((short) destPort); }
            set { destPort = (ushort) IPAddress.HostToNetworkOrder((short) value); }
        }

        public ushort Length
        {
            get { return (ushort) IPAddress.NetworkToHostOrder((short) udpLength); }
            set { udpLength = (ushort) IPAddress.HostToNetworkOrder((short) value); }
        }

        public ushort Checksum
        {
            get { return (ushort) IPAddress.NetworkToHostOrder((short) udpChecksum); }
            set { udpChecksum = (ushort) IPAddress.HostToNetworkOrder((short) value); }
        }

        public UdpHeader()
        {
            srcPort = 0;
            destPort = 0;
            udpLength = 0;
            udpChecksum = 0;

            Ipv6PacketHeader = null;
            Ipv4PacketHeader = null;
        }

        public static UdpHeader Create(byte[] udpData, ref int bytesCopied)
        {
            var udpPacketHeader = new UdpHeader();

            udpPacketHeader.srcPort = BitConverter.ToUInt16(udpData, 0);
            udpPacketHeader.destPort = BitConverter.ToUInt16(udpData, 2);
            udpPacketHeader.udpLength = BitConverter.ToUInt16(udpData, 4);
            udpPacketHeader.udpChecksum = BitConverter.ToUInt16(udpData, 6);

            return udpPacketHeader;
        }

        public override byte[] GetProtocolPacketBytes(byte[] payLoad)
        {
            byte[] udpPacket = new byte[UdpHeaderLength + payLoad.Length], pseudoHeader = null, byteValue = null;
            var offset = 0;

            byteValue = BitConverter.GetBytes(srcPort);
            Array.Copy(byteValue, 0, udpPacket, offset, byteValue.Length);
            offset += byteValue.Length;

            byteValue = BitConverter.GetBytes(destPort);
            Array.Copy(byteValue, 0, udpPacket, offset, byteValue.Length);
            offset += byteValue.Length;

            byteValue = BitConverter.GetBytes(udpLength);
            Array.Copy(byteValue, 0, udpPacket, offset, byteValue.Length);
            offset += byteValue.Length;

            udpPacket[offset++] = 0;
            udpPacket[offset++] = 0;

            Array.Copy(payLoad, 0, udpPacket, offset, payLoad.Length);

            if (Ipv4PacketHeader != null)
            {
                pseudoHeader = new byte[UdpHeaderLength + 12 + payLoad.Length];

                offset = 0;

                byteValue = Ipv4PacketHeader.SourceAddress.GetAddressBytes();
                Array.Copy(byteValue, 0, pseudoHeader, offset, byteValue.Length);
                offset += byteValue.Length;

                byteValue = Ipv4PacketHeader.DestinationAddress.GetAddressBytes();
                Array.Copy(byteValue, 0, pseudoHeader, offset, byteValue.Length);
                offset += byteValue.Length;

                pseudoHeader[offset++] = 0;
                pseudoHeader[offset++] = Ipv4PacketHeader.Protocol;

                byteValue = BitConverter.GetBytes(udpLength);
                Array.Copy(byteValue, 0, pseudoHeader, offset, byteValue.Length);
                offset += byteValue.Length;

                Array.Copy(udpPacket, 0, pseudoHeader, offset, udpPacket.Length);
            }
            else if (Ipv6PacketHeader != null)
            {
                pseudoHeader = new byte[UdpHeaderLength + 40 + payLoad.Length];

                offset = 0;

                byteValue = Ipv6PacketHeader.SourceAddress.GetAddressBytes();
                Array.Copy(byteValue, 0, pseudoHeader, offset, byteValue.Length);
                offset += byteValue.Length;

                byteValue = Ipv6PacketHeader.DestinationAddress.GetAddressBytes();
                Array.Copy(byteValue, 0, pseudoHeader, offset, byteValue.Length);
                offset += byteValue.Length;

                var ipv6PayloadLength = (uint) IPAddress.HostToNetworkOrder(payLoad.Length + UdpHeaderLength);

                byteValue = BitConverter.GetBytes(ipv6PayloadLength);
                Array.Copy(byteValue, 0, pseudoHeader, offset, byteValue.Length);
                offset += byteValue.Length;

                pseudoHeader[offset++] = 0;
                pseudoHeader[offset++] = 0;
                pseudoHeader[offset++] = 0;
                pseudoHeader[offset++] = Ipv6PacketHeader.NextHeader;

                Array.Copy(udpPacket, 0, pseudoHeader, offset, udpPacket.Length);
            }

            if (pseudoHeader != null)
            {
                Checksum = ComputeChecksum(pseudoHeader);
            }

            byteValue = BitConverter.GetBytes(udpChecksum);
            Array.Copy(byteValue, 0, udpPacket, 6, byteValue.Length);

            return udpPacket;
        }

        public override byte[] GetProtocolPacketBytes(byte[] payLoad, ushort crc)
        {
            byte[] udpPacket = new byte[UdpHeaderLength + payLoad.Length], pseudoHeader = null, byteValue = null;
            var offset = 0;

            byteValue = BitConverter.GetBytes(srcPort);
            Array.Copy(byteValue, 0, udpPacket, offset, byteValue.Length);
            offset += byteValue.Length;

            byteValue = BitConverter.GetBytes(destPort);
            Array.Copy(byteValue, 0, udpPacket, offset, byteValue.Length);
            offset += byteValue.Length;

            byteValue = BitConverter.GetBytes(udpLength);
            Array.Copy(byteValue, 0, udpPacket, offset, byteValue.Length);
            offset += byteValue.Length;

            udpPacket[offset++] = 0; 
            udpPacket[offset++] = 0;

            Array.Copy(payLoad, 0, udpPacket, offset, payLoad.Length);

            if (Ipv4PacketHeader != null)
            {
                pseudoHeader = new byte[UdpHeaderLength + 12 + payLoad.Length];

                offset = 0;

                byteValue = Ipv4PacketHeader.SourceAddress.GetAddressBytes();
                Array.Copy(byteValue, 0, pseudoHeader, offset, byteValue.Length);
                offset += byteValue.Length;

                byteValue = Ipv4PacketHeader.DestinationAddress.GetAddressBytes();
                Array.Copy(byteValue, 0, pseudoHeader, offset, byteValue.Length);
                offset += byteValue.Length;

                pseudoHeader[offset++] = 0;
                pseudoHeader[offset++] = Ipv4PacketHeader.Protocol;

                byteValue = BitConverter.GetBytes(udpLength);
                Array.Copy(byteValue, 0, pseudoHeader, offset, byteValue.Length);
                offset += byteValue.Length;

                Array.Copy(udpPacket, 0, pseudoHeader, offset, udpPacket.Length);
            }
            else if (Ipv6PacketHeader != null)
            {
                pseudoHeader = new byte[UdpHeaderLength + 40 + payLoad.Length];

                offset = 0;

                byteValue = Ipv6PacketHeader.SourceAddress.GetAddressBytes();
                Array.Copy(byteValue, 0, pseudoHeader, offset, byteValue.Length);
                offset += byteValue.Length;

                byteValue = Ipv6PacketHeader.DestinationAddress.GetAddressBytes();
                Array.Copy(byteValue, 0, pseudoHeader, offset, byteValue.Length);
                offset += byteValue.Length;

                var ipv6PayloadLength = (uint)IPAddress.HostToNetworkOrder(payLoad.Length + UdpHeaderLength);

                byteValue = BitConverter.GetBytes(ipv6PayloadLength);
                Array.Copy(byteValue, 0, pseudoHeader, offset, byteValue.Length);
                offset += byteValue.Length;

                pseudoHeader[offset++] = 0;
                pseudoHeader[offset++] = 0;
                pseudoHeader[offset++] = 0;
                pseudoHeader[offset++] = Ipv6PacketHeader.NextHeader;

                Array.Copy(udpPacket, 0, pseudoHeader, offset, udpPacket.Length);
            }

            if (pseudoHeader != null)
            {
                    Checksum = crc;
            }

            byteValue = BitConverter.GetBytes(udpChecksum);
            Array.Copy(byteValue, 0, udpPacket, 6, byteValue.Length);

            return udpPacket;
        }
    }
}