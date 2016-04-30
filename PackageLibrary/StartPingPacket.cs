using PackageLibrary.Serialization;

namespace PackageLibrary
{
    public class StartPingPacket:PacketStructure<StartPingPacket>
    {
        private ushort PacketLength { get; set; }
        public PacketType Type { get; set; }
        public uint Count { get; set; }

        public StartPingPacket(uint count)
            : base(2 + 1 + 2)
        {
            PacketLength = (ushort)(2+1+2);
            Type=PacketType.StartPingPacket;
            Count = count;
        }

        public StartPingPacket(byte[] packet) : base(packet) { }

        public override byte[] Serialize()
        {
            Write(PacketLength);
            Write((byte) Type);
            Write(Count);
            return GetData();
        }

        public override StartPingPacket Deserialize()
        {
            PacketLength = ReadUShort();
            Type = (PacketType) ReadByte();
            Count = ReadUInt();
            return this;
        }
    }
}
