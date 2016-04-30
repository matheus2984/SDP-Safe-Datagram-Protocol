using PackageLibrary.Serialization;

namespace PackageLibrary
{
    public class PingEndPacket : PacketStructure<PingEndPacket>
    {
        public PacketType PacketType { get; set; }

        public PingEndPacket(): base(1)
        {
            PacketType = PacketType.PingEndPacket;
        }

        public PingEndPacket(byte[] packet) : base(packet)
        {

        }

        public override byte[] Serialize()
        {
            Write((byte)PacketType);

            return GetData();
        }

        public override PingEndPacket Deserialize()
        {
            PacketType = (PacketType)ReadByte();
            return this;
        }
    }
}
