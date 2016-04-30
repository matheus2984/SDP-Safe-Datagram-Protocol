﻿using PackageLibrary.Serialization;

namespace PackageLibrary
{
    public class MessagePacket:PacketStructure<MessagePacket>
    {
        private ushort PacketLength { get; set; }
        private PacketType PacketType { get; set; }
        private ushort MessageLength { get; set; }
        public string Message { get; private set; }

        public MessagePacket(string message):base(2+1+2+message.Length)
        {
            PacketLength = (ushort)(2 +1+ 2 + message.Length);
            PacketType = PacketType.MessagePacket;
            MessageLength = (ushort)message.Length;
            Message = message;
        }

        public MessagePacket(byte[] packet) : base(packet)
        {
            
        }

        public override byte[] Serialize()
        {
            Write(PacketLength);
            Write((byte) PacketType);
            Write((ushort)Message.Length);
            Write(Message);
            return GetData();
        }

        public override MessagePacket Deserialize()
        {
            PacketLength = ReadUShort();
            PacketType = (PacketType)ReadByte();
            var length = ReadUShort();
            Message = ReadString(length);
            return this;
        }
    }
}
