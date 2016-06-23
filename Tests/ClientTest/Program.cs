using System;
using System.Threading;
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

            var clientCfg = new SocketCfg("25.175.152.176", 9959, ProtocolType.SDP);
   
            Console.Title =  clientCfg.ProtocolType+" - Async Client Socket Test - SDP LIBRARY";

            client = SdpSocket.ClientFactory(clientCfg);
            client.Connect += client_Connect;
            client.Receive += client_Receive;
            client.Disconnect += client_Disconnect;

            client.BeginConnect();

            manualReset.WaitOne(2000);

            var packet = new byte[16];
            client.Send(packet);

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