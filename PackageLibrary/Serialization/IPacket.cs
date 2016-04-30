namespace PackageLibrary.Serialization
{
    public interface IPacket<out T>
    {
        byte[] Serialize();
        T Deserialize();
    }
}