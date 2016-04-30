using System;
using System.Threading;
using PackageLibrary;
using SDP;
using SDP.Enums;
using SDP.Events;
using SDP.Interfaces;

namespace ClientTest
{
    internal static class Program
    {
        private static IAsyncClientSocket client;
        private static ManualResetEvent manualReset;

        private static void Main()
        {
            manualReset = new ManualResetEvent(false);

            Console.Title = "TCP - Async Client Socket Test - SDP LIBRARY";

            var clientCfg = new SocketCfg("127.0.0.1", 9959, ProtocolType.TCP);

            client = SdpSocket.ClientFactory(clientCfg);
            client.Connect += client_Connect;
            client.Receive += client_Receive;
            client.Disconnect += client_Disconnect;

            client.BeginConnect();

            manualReset.WaitOne();

            for (int i = 0; i < 50; i++)
            {
                var packet = new MessagePacket(i.ToString());
                client.Send(packet.Serialize());
            }

            while (true)
            {
                Console.ReadKey();
            }
        }

        static void client_Connect(object sender, ConnectionEventArgs e)
        {
            Console.WriteLine("Conectei");
            manualReset.Set();
        }

        static void client_Receive(object sender, ReceiveEventArgs e)
        {
            Console.WriteLine("Recebi " + e.ReceivedData.Length + " bytes");
        }

        static void client_Disconnect(object sender, ConnectionEventArgs e)
        {
            Console.WriteLine("Fui desconectado");
        }
    }
}