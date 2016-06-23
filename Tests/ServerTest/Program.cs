using System;
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
            var serverCfg = new SocketCfg("25.175.152.176", 9959, ProtocolType.SDP);
            Console.Title = serverCfg.ProtocolType + " - Async Server Socket Test - SDP LIBRARY";

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
            Console.WriteLine("Recebi " + e.ReceivedData.Length + " bytes de "+e.State.EndPoint);
          //  e.State.Send(e.ReceivedData);
        //    var packet = new MessagePacket(e.ReceivedData).Deserialize();
        //    Console.WriteLine(packet.Message);
        }

        static void server_Disconnect(object sender, ConnectionEventArgs e)
        {
            Console.WriteLine("Desconectaram");
        }
    }
}