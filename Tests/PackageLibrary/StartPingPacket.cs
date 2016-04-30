using PackageLibrary.Serialization;

namespace PackageLibrary
{
    public class StartPingPacket:PacketStructure<StartPingPacket>
    {
        public PacketType Type { get; set; }
        public uint Count { get; set; }

        public StartPingPacket(uint count)
            : base(1 + 2)
        {
            Type=PacketType.StartPingPacket;
            Count = count;
        }

        public StartPingPacket(byte[] packet) : base(packet) { }

        public override byte[] Serialize()
        {
            Write((byte) Type);
            Write(Count);
            return GetData();
        }

        public override StartPingPacket Deserialize()
        {
            Type = (PacketType) ReadByte();
            Count = ReadUInt();
            return this;
        }
    }
}
