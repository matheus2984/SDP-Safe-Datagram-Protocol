using PackageLibrary.Serialization;

namespace PackageLibrary
{
    public class PingEndPacket : PacketStructure<PingEndPacket>
    {
        private ushort PacketLength { get; set; }
        public PacketType PacketType { get; set; }

        public PingEndPacket(): base(2+1)
        {
            PacketLength = (ushort)(2 + 1);
            PacketType = PacketType.PingEndPacket;
        }

        public PingEndPacket(byte[] packet) : base(packet)
        {

        }

        public override byte[] Serialize()
        {
            Write(PacketLength);
            Write((byte)PacketType);

            return GetData();
        }

        public override PingEndPacket Deserialize()
        {
            PacketLength = ReadUShort();
            PacketType = (PacketType)ReadByte();
            return this;
        }
    }
}
