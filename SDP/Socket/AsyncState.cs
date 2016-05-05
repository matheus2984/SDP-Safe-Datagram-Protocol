using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SDP.Events;
using SDP.Interfaces;
using SDP.Modules.TCP;
using SDP.Modules.UDP;
using ProtocolType = SDP.Enums.ProtocolType;
using SocketType = SDP.Enums.SocketType;

namespace SDP.Socket
{
    /// <summary>
    /// Objeto que representa uma conexão realizada
    /// </summary>
    internal class AsyncState : IAsyncState, IReceive, ISend
    {
        /// <summary>
        /// Socket da conexão
        /// </summary>
        internal System.Net.Sockets.Socket Socket { get; private set; }

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
        private IReceive receiveModule;

        /// <summary>
        /// Gerencia a forma como os dados devem ser enviados
        /// </summary>
        private ISend sendModule;

        /// <summary>
        /// Configurações do host
        /// </summary>
        public SocketCfg Cfg { get; private set; }

        internal EndPoint endPoint;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="asyncServerSocket"></param>
        /// <param name="cfg"></param>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        internal AsyncState(IAsyncSocket asyncServerSocket, SocketCfg cfg, System.Net.Sockets.Socket socket, byte[] buffer)
        {
            AsyncSocket = asyncServerSocket;
            Socket = socket;
            Buffer = buffer;
            Cfg = cfg;

            switch (cfg.ProtocolType)
            {
                case ProtocolType.TCP:
                    ConfigureTcp(cfg);
                    break;
                case ProtocolType.UDP:
                    ConfigureUdp(cfg);
                    break;
                case ProtocolType.SDP:
                    ConfigureSdp(cfg);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ConfigureTcp(SocketCfg cfg)
        {
            switch (cfg.SocketType)
            {
                case SocketType.Datagram:
                    receiveModule = new TcpDatagramReceiveModule(this);
                    sendModule = new TcpDatagramSendModule(this);
                    break;
                case SocketType.Stream:
                    receiveModule = new TcpStreamReceiveModule(this);
                    sendModule = new TcpStreamSendModule(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ConfigureUdp(SocketCfg cfg)
        {
            switch (cfg.SocketType)
            {
                case SocketType.Datagram:
                    receiveModule = new UdpDatagramReceiveModule(this);
                    sendModule = new UdpDatagramSendModule(this);
                    break;
                case SocketType.Stream:
                    throw new Exception("Este protocolo não permite envio/recebimento do tipo stream");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ConfigureSdp(SocketCfg cfg)
        {
            throw new Exception("Protocolo não implementado");
        }

        /// <summary>
        /// Inicia recebimento de dados de forma assíncrona
        /// </summary>
        public void BeginReceive()
        {
            receiveModule.BeginReceive();
        }

        /// <summary>
        /// Inicia desconexão de forma assíncrona
        /// </summary>
        public void BeginDisconnect()
        {
            if (Cfg.ProtocolType == ProtocolType.UDP) return;
            Socket.Shutdown(SocketShutdown.Both);
            Socket.BeginDisconnect(false, AsyncDisconnect, this);
        }

        /// <summary>
        /// Envia dados
        /// </summary>
        /// <param name="packet"></param>
        public void Send(byte[] packet)
        {
            sendModule.Send(packet);
        }

        /// <summary>
        /// Envia dados de forma assíncrona
        /// </summary>
        /// <param name="packet"></param>
        public void AsyncSend(byte[] packet)
        {
            sendModule.AsyncSend(packet);
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