using System;
using System.Diagnostics;
using System.Net.Sockets;
using SDP.Interfaces;
using SDP.Socket;
using SDP.Util;

namespace SDP.Modules.TCP
{
    /// <summary>
    /// Modulo de envio de dados do tipo datagrama
    /// </summary>
    internal class TcpDatagramSendModule : ISend
    {
        /// <summary>
        ///     Objeto de conexão
        /// </summary>
        private readonly AsyncState asyncState;

        public TcpDatagramSendModule(AsyncState state)
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
                // cria o cabeçalho
                byte[] header = CreateHeader(packet);
                // combina o corpo do pacote com o cabeçalho
                byte[] finalPacket = BufferOperation.Combine(header, packet);

                asyncState.Socket.Send(finalPacket);
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
                // cria o cabeçalho
                byte[] header = CreateHeader(packet);
                // combina o corpo do pacote com o cabeçalho
                byte[] finalPacket = BufferOperation.Combine(header, packet);

                asyncState.Socket.BeginSend(finalPacket, 0, finalPacket.Length, SocketFlags.None, AsyncSend, this);
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