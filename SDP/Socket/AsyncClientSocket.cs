using System;
using System.Net;
using System.Net.Sockets;
using SDP.Interfaces;
using SDP.Modules.TCP;

namespace SDP.Socket
{
    /// <summary>
    /// Socket de cliente assíncrono
    /// </summary>
    internal class AsyncClientSocket : AsyncSocketBase, IAsyncClientSocket
    {
        /// <summary>
        /// Representa o ponto de conexão (IP e Porta) do servidor
        /// </summary>
        internal readonly IPEndPoint IpEndPoint;

        /// <summary>
        /// Configurações do socket
        /// </summary>
        internal readonly SocketCfg Cfg;

        /// <summary>
        /// Objeto de conexão
        /// </summary>
        internal IAsyncState AsyncState;

        private readonly IConnect connectModule;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="cfg"></param>
        public AsyncClientSocket(SocketCfg cfg, AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
            : base(addressFamily, socketType, protocolType)
        {
            // objeto de configuração do socket
            Cfg = cfg;
            
            // Fecha o soquete normalmente sem remanescentes
            SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            // Verifica a conexão para checar se a outra se mantem ativa
            SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            // Representa o ponto de conexão (IP e Porta) do servidor
            IpEndPoint = new IPEndPoint(IPAddress.Parse(cfg.IP), cfg.Port);

            switch (cfg.ProtocolType)
            {
                case Enums.ProtocolType.TCP:
                    connectModule = new TcpConnectModule(this);
                    break;
                case Enums.ProtocolType.UDP:
                    break;
                case Enums.ProtocolType.SDP:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Inicia tentativa de conexão com o servidor
        /// </summary>
        public void BeginConnect()
        {
           connectModule.BeginConnect();
        }

        /// <summary>
        /// Envia dados
        /// </summary>
        /// <param name="packet"></param>
        public new void Send(byte[] packet)
        {
            AsyncState.Send(packet);
        }
    }
}