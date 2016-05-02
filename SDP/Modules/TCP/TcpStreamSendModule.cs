using System;
using System.Diagnostics;
using System.Net.Sockets;
using SDP.Interfaces;
using SDP.Socket;

namespace SDP.Modules.TCP
{
    /// <summary>
    /// Modulo de envio de dados do tipo stream
    /// </summary>
    internal class TcpStreamSendModule : ISend
    {
        /// <summary>
        ///     Objeto de conexão
        /// </summary>
        private readonly AsyncState asyncState;

        public TcpStreamSendModule(AsyncState state)
        {
            asyncState = state;
        }

        /// <summary>
        ///     Finaliza o envio de dados de forma assíncrona
        /// </summary>
        /// <param name="result"></param>
        private void AsyncSend(IAsyncResult result)
        {
            try
            {
                asyncState.Socket.EndSend(result);
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
        ///     Envia dados
        /// </summary>
        /// <param name="packet"></param>
        public void Send(byte[] packet)
        {
            try
            {
                asyncState.Socket.Send(packet);
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
                asyncState.Socket.BeginSend(packet, 0, packet.Length, SocketFlags.None, AsyncSend, this);
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