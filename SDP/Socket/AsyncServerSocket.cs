using System;
using System.Net;
using System.Net.Sockets;
using SDP.Interfaces;
using SDP.Modules.TCP;

namespace SDP.Socket
{
    /// <summary>
    /// Socket de servidor assincrono
    /// </summary>
    internal class AsyncServerSocket : AsyncSocketBase, IAsyncServerSocket
    {
        /// <summary>
        /// configurações do socket
        /// </summary>
        internal readonly SocketCfg Cfg;

        /// <summary>
        /// Gerencia a forma como os usuarios irão se conectar
        /// </summary>
        private readonly IAccept acceptModule;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="addressFamily"></param>
        /// <param name="socketType"></param>
        /// <param name="protocolType"></param>
        public AsyncServerSocket(SocketCfg cfg, AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
            : base(addressFamily,socketType,protocolType)
        {
            Cfg = cfg;

            // Fecha o soquete normalmente sem remanescentes
            SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            // Verifica a conexão para checar se a outra se mantem ativa
            SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            // define o IP e a Porta em que o servidor ira trabalhar
            Bind(new IPEndPoint(IPAddress.Parse(cfg.IP), cfg.Port));
            // limite de conexões que podem existir ao mesmo tempo
            Listen(500);

            switch (cfg.ProtocolType)
            {
                case Enums.ProtocolType.TCP:
                    acceptModule = new TcpAcceptModule(this);
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
        /// Inicia escuta de conexões
        /// </summary>
        public void BeginAccept()
        {
            // chama modulo de escuta de conexões
            acceptModule.BeginAccept();
        }
    }
}