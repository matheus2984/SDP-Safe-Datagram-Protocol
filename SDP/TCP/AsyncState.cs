using System;
using System.Net.Sockets;
using SDP.Events;
using SDP.Interfaces;

namespace SDP.TCP
{
    /// <summary>
    /// Objeto que representa uma conexão realizada
    /// </summary>
    public class AsyncState : IAsyncState
    {
        /// <summary>
        /// Socket da conexão
        /// </summary>
        public Socket Socket { get; set; }

        /// <summary>
        /// Buffer de recebimento de dados
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// Ultimo pacote de dados recebido
        /// </summary>
        public byte[] ReceivedBuffer { get; set; }

        /// <summary>
        /// Socket que gerou a conexão
        /// </summary>
        private readonly IAsyncSocket asyncSocket;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="asyncServerSocket"></param>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        internal AsyncState(IAsyncSocket asyncServerSocket, Socket socket, byte[] buffer)
        {
            asyncSocket = asyncServerSocket;
            Socket = socket;
            Buffer = buffer;
        }

        /// <summary>
        /// Inicia recebimento de dados de forma assíncrona
        /// </summary>
        internal void BeginReceive()
        {
            Socket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, AsyncReceive, this);
        }

        /// <summary>
        /// Inicia desconexão de forma assíncrona
        /// </summary>
        private void BeginDisconnect()
        {
            Socket.Shutdown(SocketShutdown.Both);
            Socket.BeginDisconnect(false, AsyncDisconnect, this);
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

                if (!Socket.Connected)
                    return;

                // representa a quantidade de bytes que foi recebido, caso ocorra erro ele sera armazenado em 'socketError'
                var size = Socket.EndReceive(result, out socketError);

                if (socketError == SocketError.Success && size != 0)
                {
                    ReceivedBuffer = new byte[size];
                    System.Buffer.BlockCopy(Buffer, 0, ReceivedBuffer, 0, size);

                    asyncSocket.OnReceive(new ReceiveEventArgs(this));

                    BeginReceive();
                }
                else
                {
                    // se ocorreu erro desconectar
                    BeginDisconnect();
                }
            }
            catch (SocketException ex)
            {

            }
        }

        /// <summary>
        /// Realiza desconexão de forma assíncrona
        /// </summary>
        /// <param name="result"></param>
        private void AsyncDisconnect(IAsyncResult result)
        {
            Socket.EndDisconnect(result);
            Socket.Close();

            asyncSocket.OnDisconnect(new ConnectionEventArgs(this));
        }
    }
}