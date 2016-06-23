using System.Net;

namespace SDP.LowLevelAPI
{
    public class Ipv6FragmentHeader
    {
        public static int Ipv6FragmentHeaderLength = 8;
        private uint fragmentId;
        private ushort fragmentOffset;

        public byte NextHeader { get; set; }

        public byte Reserved { get; set; }

        public ushort Offset
        {
            get { return (ushort) IPAddress.NetworkToHostOrder((short) fragmentOffset); }
            set { fragmentOffset = (ushort) IPAddress.HostToNetworkOrder((short) value); }
        }

        public uint Id
        {
            get { return (uint) IPAddress.NetworkToHostOrder((int) fragmentId); }
            set { fragmentId = (uint) IPAddress.HostToNetworkOrder((int) value); }
        }

        public Ipv6FragmentHeader()
        {
            NextHeader = 0;
            Reserved = 0;
            fragmentOffset = 0;
            fragmentId = 0;
        }
    }
}