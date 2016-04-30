using System;
using System.Linq;

namespace SDP.Util
{
    public static class BufferOperation
    {
        public static byte[] Combine(params byte[][] dataArgs)
        {
            int currentOffset = 0;

            int totalLength = dataArgs.Sum(t => t.Length);

            byte[] outDate = new byte[totalLength];

            for (int i = 0; i < dataArgs.Length; i++)
            {
                Buffer.BlockCopy(dataArgs[i],0,outDate,currentOffset,dataArgs[i].Length);
                currentOffset += dataArgs[i].Length;
            }

            return outDate;
        }
    }
}
