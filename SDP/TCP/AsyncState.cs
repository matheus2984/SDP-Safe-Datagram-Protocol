using System;
using System.Net.Sockets;
using SDP.Events;
using SDP.Interfaces;
using SDP.TCP.Managers;
using SocketType = SDP.Enums.SocketType;

namespace SDP.TCP
{
    /// <summary>
    /// Objeto que representa uma conexão realizada
    /// </summary>
    public class AsyncState : IAsyncState, IReceive, ISend
    {
        /// <summary>
        /// Socket da conexão
        /// </summary>
        internal Socket Socket { get; private set; }

        /// <summary>
        /// Buffer de recebimento de dados
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// Ultimo pacote de dados recebido
        /// </summary>
        public byte[] ReceivedBuffer { get; set; }

        /// <summary>
        /// Cabeçalho do ultimo pacote de dados recebido
        /// </summary>
        internal byte[] ReceiveBufferHeader { get; set; }

        /// <summary>
        /// Socket que gerou a conexão
        /// </summary>
        internal readonly IAsyncSocket AsyncSocket;

        /// <summary>
        /// Gerencia a forma como os dados devem ser recebidos
        /// </summary>
        private readonly IReceive receiveManager;

        /// <summary>
        /// Gerencia a forma como os dados devem ser enviados
        /// </summary>
        private readonly ISend sendManager;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="asyncServerSocket"></param>
        /// <param name="cfg"></param>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        internal AsyncState(IAsyncSocket asyncServerSocket, SocketCfg cfg, Socket socket, byte[] buffer)
        {
            AsyncSocket = asyncServerSocket;
            Socket = socket;
            Buffer = buffer;

            switch (cfg.SocketType)
            {
                case SocketType.Datagram:
                    receiveManager = new TcpDatagramReceiveManager(this);
                    sendManager = new TcpDatagramSendManager(this);
                    break;
                case SocketType.Stream:
                    receiveManager = new TcpStreamReceiveManager(this);
                    sendManager = new TcpStreamSendManager(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Inicia recebimento de dados de forma assíncrona
        /// </summary>
        public void BeginReceive()
        {
            receiveManager.BeginReceive();
        }

        /// <summary>
        /// Inicia desconexão de forma assíncrona
        /// </summary>
        public void BeginDisconnect()
        {
            Socket.Shutdown(SocketShutdown.Both);
            Socket.BeginDisconnect(false, AsyncDisconnect, this);
        }

        /// <summary>
        /// Envia dados
        /// </summary>
        /// <param name="packet"></param>
        public void Send(byte[] packet)
        {
            sendManager.Send(packet);
        }

        /// <summary>
        /// Envia dados de forma assíncrona
        /// </summary>
        /// <param name="packet"></param>
        public void AsyncSend(byte[] packet)
        {
            sendManager.AsyncSend(packet);
        }

        /// <summary>
        /// Realiza desconexão de forma assíncrona
        /// </summary>
        /// <param name="result"></param>
        private void AsyncDisconnect(IAsyncResult result)
        {
            Socket.EndDisconnect(result);
            Socket.Close();

            AsyncSocket.OnDisconnect(new ConnectionEventArgs(this));
        }
    }
}