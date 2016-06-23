using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using SDP.LowLevelAPI;

namespace SDP.Modules.SDP.Package
{
    public static class SdpPacketBuild
    {
        public static byte[] AddUdpIpHeader(IPEndPoint srcAdr, IPEndPoint dstAdr, byte[] packet)
        {
            srcAdr = new IPEndPoint(IPAddress.Parse("25.175.152.176"),9959);
            dstAdr = new IPEndPoint(IPAddress.Parse("25.175.152.176"), 9958);

            byte[] builtPacket = new byte[packet.Length];

            ArrayList headerList = new ArrayList();

            UdpHeader udpPacket = new UdpHeader();
            udpPacket.SourcePort = (ushort)srcAdr.Port;
            udpPacket.DestinationPort = (ushort)dstAdr.Port;
            udpPacket.Length = (ushort)(UdpHeader.UdpHeaderLength + packet.Length);
            udpPacket.Checksum = 0;

            if (srcAdr.AddressFamily == AddressFamily.InterNetwork)
            {
                Ipv4Header ipv4Packet = new Ipv4Header();
                ipv4Packet.Version = 4;
                ipv4Packet.Protocol = (byte) ProtocolType.Udp;
                ipv4Packet.Ttl = 2;
                ipv4Packet.Offset = 0;
                ipv4Packet.Length = (byte) Ipv4Header.Ipv4HeaderLength;
                ipv4Packet.TotalLength =
                    Convert.ToUInt16(Ipv4Header.Ipv4HeaderLength + UdpHeader.UdpHeaderLength + packet.Length);
                ipv4Packet.SourceAddress = srcAdr.Address;
                ipv4Packet.DestinationAddress = dstAdr.Address;

                udpPacket.Ipv4PacketHeader = ipv4Packet;

                headerList.Add(ipv4Packet);
            }
            else if (srcAdr.AddressFamily == AddressFamily.InterNetworkV6)
            {
                Ipv6Header ipv6Packet = new Ipv6Header();

                ipv6Packet.Version = 6;
                ipv6Packet.TrafficClass = 1;
                ipv6Packet.Flow = 2;
                ipv6Packet.HopLimit = 2;
                ipv6Packet.NextHeader = (byte)ProtocolType.Udp;
                ipv6Packet.PayloadLength = (ushort)(UdpHeader.UdpHeaderLength + packet.Length);
                ipv6Packet.SourceAddress = srcAdr.Address;
                ipv6Packet.DestinationAddress = dstAdr.Address;

                udpPacket.Ipv6PacketHeader = ipv6Packet;

                headerList.Add(ipv6Packet);
            }

            headerList.Add(udpPacket);

            builtPacket = udpPacket.BuildPacket(headerList, packet);

            return builtPacket;
        }
    }
}