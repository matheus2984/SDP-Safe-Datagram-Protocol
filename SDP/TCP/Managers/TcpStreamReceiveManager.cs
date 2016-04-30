using System;
using System.Diagnostics;
using System.Net.Sockets;
using SDP.Events;
using SDP.Interfaces;

namespace SDP.TCP.Managers
{
    /// <summary>
    /// Classe resposavel por gerenciar o recebimento de dados no formato de stream
    /// </summary>
    internal class TcpStreamReceiveManager : IReceive
    {
        /// <summary>
        /// Objeto de conexão
        /// </summary>
        private readonly AsyncState asyncState;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="state"></param>
        public TcpStreamReceiveManager(AsyncState state)
        {
            asyncState = state;
        }

        /// <summary>
        /// Inicia recebimento de dados de forma assíncrona
        /// </summary>
        public void BeginReceive()
        {
            asyncState.Socket.BeginReceive(asyncState.Buffer, 0, asyncState.Buffer.Length,
                SocketFlags.None, AsyncReceive, asyncState);
        }

        /// <summary>
        /// Recebe dados de forma assíncrona
        /// </summary>
        /// <param name="result"></param>
        private void AsyncReceive(IAsyncResult result)
        {
            try
            {
                SocketError socketError;

                if (!asyncState.Socket.Connected)
                    return;

                // representa a quantidade de bytes que foi recebido, caso ocorra erro ele sera armazenado em 'socketError'
                var size = asyncState.Socket.EndReceive(result, out socketError);

                if (socketError == SocketError.Success && size != 0)
                {
                    // seta o tamanho do buffer de pacote
                    asyncState.ReceivedBuffer = new byte[size];
                    // passa os dados do buffer de recebimento para o buffer de pacote
                    Buffer.BlockCopy(asyncState.Buffer, 0, asyncState.ReceivedBuffer, 0, size);

                    // chama evento de recebimento
                    asyncState.AsyncSocket.OnReceive(new ReceiveEventArgs(asyncState));

                    // Volta a receber dados
                    BeginReceive();
                }
                else
                {
                    // se ocorreu erro desconectar
                    asyncState.BeginDisconnect();
                }
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
