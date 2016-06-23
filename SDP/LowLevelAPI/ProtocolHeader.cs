using System.Collections;

namespace SDP.LowLevelAPI
{
    public abstract class ProtocolHeader
    {
        public abstract byte[] GetProtocolPacketBytes(byte[] payLoad);
        public virtual byte[] GetProtocolPacketBytes(byte[] payLoad, ushort crc)
        {
            return null;
        }

        public byte[] BuildPacket(ArrayList headerList, byte[] payLoad)
        {
            for (int i = headerList.Count - 1; i >= 0; i--)
            {
                var protocolHeader = (ProtocolHeader) headerList[i];
                byte[] newPayload = protocolHeader.GetProtocolPacketBytes(payLoad);

                payLoad = newPayload;
            }

            return payLoad;
        }

        public byte[] BuildPacket(ArrayList headerList, byte[] payLoad, ushort crc)
        {
            for (int i = headerList.Count - 1; i >= 0; i--)
            {
                var protocolHeader = (ProtocolHeader)headerList[i];
                byte[] newPayload = protocolHeader.GetProtocolPacketBytes(payLoad,crc);

                payLoad = newPayload;
            }

            return payLoad;
        }

        public static ushort ComputeChecksum(byte[] payLoad)
        {
            uint xsum = 0;
            ushort shortval;

            for (var i = 0; i < payLoad.Length/2; i++)
            {
                var hiword = (ushort) (payLoad[i*2] << 8);
                ushort loword = payLoad[i*2 + 1];
                shortval = (ushort) (hiword | loword);
                xsum = xsum + shortval;
            }

            if (payLoad.Length%2 != 0)
            {
                xsum += payLoad[payLoad.Length - 1];
            }

            xsum = (xsum >> 16) + (xsum & 0xFFFF);
            xsum = xsum + (xsum >> 16);
            shortval = (ushort) ~xsum;

            return shortval;
        }
    }
}