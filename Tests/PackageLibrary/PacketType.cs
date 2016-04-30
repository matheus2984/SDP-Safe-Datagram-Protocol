namespace PackageLibrary
{
    public enum PacketType
    {
        StartPingPacket = 0x00,
        PingPacket = 0x01,
        PingEndPacket = 0x02,
        MessagePacket = 0x03,
    }
}