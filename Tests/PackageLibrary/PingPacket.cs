using PackageLibrary.Serialization;

namespace PackageLibrary
{
    public class PingPacket:PacketStructure<PingPacket>
    {
        public PacketType PacketType { get; set; }
        public ushort ArraySize { get; set; }
        public byte[] Array { get; set; }

        public PingPacket(ushort pingSize) : base(1+2+pingSize)
        {
            PacketType = PacketType.PingPacket;
            ArraySize = pingSize;
            Array = new byte[pingSize];
        }

        public PingPacket(byte[] packet) : base(packet)
        {
            
        }

        public override byte[] Serialize()
        {
            Write((byte) PacketType);
            Write(ArraySize);
            for (var i = 0; i < Array.Length; i++)
                Write((byte)0x00);

            return GetData();
        }

        public override PingPacket Deserialize()
        {
            PacketType = (PacketType) ReadByte();
            ArraySize = ReadUShort();

            Array = new byte[ArraySize];

            for (var i = 0; i < ArraySize ; i++)
            {
                Array[i] = ReadByte();
            }

            return this;    
        }
    }
}
