using System;
using System.Text;
using PackageLibrary;
using SDP;
using SDP.Events;
using SDP.Interfaces;

namespace ClientTest
{
    internal static class Program
    {
        private static IAsyncClientSocket client;
        private static void Main()
        {
            Console.Title = "TCP - Async Client Socket Test - SDP LIBRARY";

            client = SdpSocket.ClientFactory("25.175.152.176", 9959);
            client.Connect += client_Connect;
            client.Receive += client_Receive;
            client.Disconnect += client_Disconnect;

            client.BeginConnect();

            while (true)
            {
                Console.ReadKey();
            }
        }
        static void client_Connect(object sender, ConnectionEventArgs e)
        {
            Console.WriteLine("Conectei");
            var packet = new MessagePacket("eae");
            e.State.Socket.Send(packet.Serialize());
            Console.WriteLine("Enviei");
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