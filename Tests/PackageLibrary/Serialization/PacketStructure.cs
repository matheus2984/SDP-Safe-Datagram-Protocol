using System;
using System.Text;
using PackageLibrary.Util;

namespace PackageLibrary.Serialization
{
    public abstract unsafe class PacketStructure<T> : IPacket<T> where T : PacketStructure<T>
    {
        private byte[] Data { get; set; }
        private int offset;

        public PacketStructure(int length)
        {
            Data = new byte[length];
        }

        public PacketStructure(byte[] packet)
        {
            Data = packet;
        }

        public int ReadInt()
        {
            int value;
            fixed (byte* packet = Data)
                value = *(int*)(packet + offset);

            offset += sizeof(int);
            return value;
        }
        public uint ReadUInt()
        {
            uint value;
            fixed (byte* packet = Data)
                value = *(uint*)(packet + offset);

            offset += sizeof (uint);
            return value;
        }

        public short ReadShort()
        {
            short value;
            fixed (byte* packet = Data)
                value = *(short*)(packet + offset);

            offset += sizeof (short);
            return value;
        }
        public ushort ReadUShort()
        {
            ushort value;
            fixed (byte* packet = Data)
                value= *(ushort*)(packet + offset);

            offset += sizeof (ushort);
            return value;
        }

        public long ReadLong()
        {
            long value;
            fixed (byte* packet = Data)
                value= *(long*)(packet + offset);

            offset += sizeof (long);
            return value;
        }
        public ulong ReadULong()
        {
            ulong value;
            fixed (byte* packet = Data)
                value= *(ulong*)(packet + offset);

            offset += sizeof (ulong);
            return value;
        }

        public byte ReadByte()
        {
            var data = Data[offset];
            offset += sizeof (byte);
            return data;
        }
        public bool ReadBoolean()
        {
            return ReadByte() == 1;
        }

        public string ReadString(int length)
        {
            var data= Encoding.UTF8.GetString(Data, offset, length);
            offset += data.Length;
            return data;
        }

        public void Write(int value)
        {
            try
            {
                fixed (byte* packet = Data)
                    *(int*) (packet + offset) = value;

                offset += sizeof (int);
            }
            catch (Exception ex) { Console.WriteLine(ex);}
        }
        public void Write(uint value)
        {
            fixed (byte* packet = Data)
                *(uint*)(packet + offset) = value;

            offset += sizeof (uint);
        }

        public void Write(short value)
        {
            fixed (byte* packet = Data)
                *(short*)(packet + offset) = value;

            offset += sizeof (short);
        }

        public void Write(ushort value)
        {
            fixed (byte* packet = Data)
                *(ushort*)(packet + offset) = value;

            offset += sizeof (ushort);
        }

        public void Write(long value)
        {
            fixed (byte* packet = Data)
                *(long*)(packet + offset) = value;

            offset += sizeof (long);
        }

        public void Write(ulong value)
        {
            fixed (byte* packet = Data)
                *(ulong*)(packet + offset) = value;

            offset += sizeof (ulong);
        }

        public void Write(byte value)
        {
            Data[offset] = value;
            offset += sizeof (byte);
        }

        public void Write(bool value)
        {
            Write((byte)(value ? 1 : 0));
        }

        public void Write(string value)
        {
            fixed (byte* packet = Data)
                NativeMethods.memcpy(packet + offset, value, value.Length);

            offset += value.Length;
        }

        public byte[] GetData()
        {
            return Data;
        }

        public abstract byte[] Serialize();
        public abstract T Deserialize();
    }
}