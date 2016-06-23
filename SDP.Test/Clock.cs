using System.Diagnostics;

namespace SDP.Test
{
    public static class Clock
    {
        private static Stopwatch sw;
        private static readonly long Frequency = Stopwatch.Frequency / 1000000;

        private const int MS_PER_SECOND = 1000;
        private const int MS_PER_MINUTE = 60000;
        private const int MS_PER_HOUR = 3600000;

        public static void Start()
        {
            sw = Stopwatch.StartNew();
        }

        public static void Stop()
        {
            sw.Stop();
        }

        public static void Reset()
        {
            sw.Reset();
        }

        public static long Ticks
        {
            get { return sw.ElapsedMilliseconds; }
        }

        public static uint SecondsTicks
        {
            get { return (uint)(sw.ElapsedMilliseconds / MS_PER_SECOND); }
        }

        public static uint MinutesTicks
        {
            get { return (uint)(sw.ElapsedMilliseconds / MS_PER_MINUTE); }
        }

        public static uint HoursTicks
        {
            get { return (uint)(sw.ElapsedMilliseconds / MS_PER_HOUR); }
        }

        public static long MicroSeconds
        {
            get { return sw.ElapsedTicks / Frequency; }
        }

        public static long NanoSeconds
        {
            get { return sw.ElapsedTicks / Frequency * 1000; }
        }
    }
}