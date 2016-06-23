using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SDP.Interfaces;
using SDP.Modules.SDP.Package;
using SDP.Socket;

namespace SDP.Modules.SDP
{
    /// <summary>
    /// Modulo de envio de dados do tipo datagrama
    /// </summary>
    internal class SdpDatagramSendModule : ISend
    {
      /// <summary>
        ///     Objeto de conexão
        /// </summary>
        private readonly AsyncState asyncState;

        /// <summary>
        /// Necessario para o controle de chamada de envio assíncrono
        /// </summary>
        private readonly ManualResetEvent manualReset;

        public SdpDatagramSendModule(AsyncState state)
        {
            asyncState = state;
            manualReset = new ManualResetEvent(false);
        }

        /// <summary>
        ///     Finaliza o envio de dados de forma assíncrona
        /// </summary>
        /// <param name="result"></param>
        private void AsyncSend(IAsyncResult result)
        {
            try
            {
                asyncState.Socket.EndSendTo(result);

                // sinaliza que os dados foram enviados
                manualReset.Set();
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode != SocketError.Disconnecting &&
                    ex.SocketErrorCode != SocketError.NotConnected &&
                    ex.SocketErrorCode != SocketError.ConnectionReset &&
                    ex.SocketErrorCode != SocketError.ConnectionAborted &&
                    ex.SocketErrorCode != SocketError.Shutdown)
                    Debug.WriteLine(ex);
            }
        }

        /// <summary>
        ///     Cria cabeçalho para o pacote recebido como parametro
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private byte[] CreateHeader(byte[] packet)
        {
            byte[] header = BitConverter.GetBytes((ushort) packet.Length);
            return header;
        }

        /// <summary>
        ///     Envia dados
        /// </summary>
        /// <param name="packet"></param>
        public void Send(byte[] packet)
        {
            try
            {
                packet = SdpPacketBuild.AddUdpIpHeader
                    (
                        new IPEndPoint(IPAddress.Parse( "25.175.152.176"), 9959),
                        (IPEndPoint) asyncState.EndPoint, packet
                    );

                asyncState.Socket.SendTo(packet, asyncState.EndPoint);
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode != SocketError.Disconnecting &&
                    ex.SocketErrorCode != SocketError.NotConnected &&
                    ex.SocketErrorCode != SocketError.ConnectionReset &&
                    ex.SocketErrorCode != SocketError.ConnectionAborted &&
                    ex.SocketErrorCode != SocketError.Shutdown)
                    Debug.WriteLine(ex);
            }
        }

        /// <summary>
        ///     Envia dados de forma assíncrona
        /// </summary>
        /// <param name="packet"></param>
        public void AsyncSend(byte[] packet)
        {
            try
            {
                asyncState.Socket.BeginSendTo(packet, 0, packet.Length, SocketFlags.None,
                    asyncState.EndPoint, AsyncSend, asyncState);

                // espera a sinalização de que os dados foram transmitidos ou aguarda 1 segundo
                manualReset.WaitOne(1000);
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode != SocketError.Disconnecting &&
                    ex.SocketErrorCode != SocketError.NotConnected &&
                    ex.SocketErrorCode != SocketError.ConnectionReset &&
                    ex.SocketErrorCode != SocketError.ConnectionAborted &&
                    ex.SocketErrorCode != SocketError.Shutdown)
                    Debug.WriteLine(ex);
            }
        }
    }
}