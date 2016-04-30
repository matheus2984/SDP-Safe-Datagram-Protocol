using System;
using PackageLibrary;
using SDP;
using SDP.Enums;
using SDP.Events;
using SDP.Interfaces;

namespace ServerTest
{
    internal static class Program
    {
        private static IAsyncServerSocket server;

        private static void Main()
        {
            Console.Title = "TCP - Async Server Socket Test - SDP LIBRARY";

            var serverCfg = new SocketCfg("127.0.0.1", 9959, ProtocolType.TCP);

            server = SdpSocket.ServerFactory(serverCfg);
            server.Connect += server_Connect;
            server.Receive += server_Receive;
            server.Disconnect += server_Disconnect;

            server.BeginAccept();

            Console.ReadKey();
        }

        static void server_Connect(object sender, ConnectionEventArgs e)
        {
            Console.WriteLine("Conectaram");
        }

        static void server_Receive(object sender, ReceiveEventArgs e)
        {
            Console.WriteLine("Recebi " + e.ReceivedData.Length + " bytes");
            var packet = new MessagePacket(e.ReceivedData).Deserialize();
            Console.WriteLine(packet.Message);
        }

        static void server_Disconnect(object sender, ConnectionEventArgs e)
        {
            Console.WriteLine("Desconectaram");
        }
    }
}