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
        private IAccept acceptModule;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="addressFamily"></param>
        /// <param name="socketType"></param>
        /// <param name="protocolType"></param>
        public AsyncServerSocket(SocketCfg cfg, AddressFamily addressFamily, SocketType socketType,
            ProtocolType protocolType)
            : base(addressFamily, socketType, protocolType)
        {
            Cfg = cfg;

            switch (cfg.ProtocolType)
            {
                case Enums.ProtocolType.TCP:
                    ConfigureTcp();
                    break;
                case Enums.ProtocolType.UDP:
                    ConfigureUdp();
                    break;
                case Enums.ProtocolType.SDP:
                    ConfigureSdp();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Configura o server para o modo tcp
        /// </summary>
        private void ConfigureTcp()
        {
            // Fecha o soquete normalmente sem remanescentes
            SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            // Verifica a conexão para checar se a outra se mantem ativa
            SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            // define o IP e a Porta em que o servidor ira trabalhar
            Bind(new IPEndPoint(IPAddress.Parse(Cfg.IP), Cfg.Port));

            // limite de conexões que podem existir ao mesmo tempo
            Listen(500);

            acceptModule = new TcpAcceptModule(this);
        }

        /// <summary>
        /// Configura o server para o modo udp
        /// </summary>
        private void ConfigureUdp()
        {
            // udp não é baseado em conexão

            // define o IP e a Porta em que o servidor ira trabalhar
            Bind(new IPEndPoint(IPAddress.Parse(Cfg.IP), Cfg.Port));

            var asyncState = new AsyncState(this, Cfg, this, new byte[1024*2]);
            asyncState.EndPoint = new IPEndPoint(IPAddress.Parse(Cfg.IP), Cfg.Port);
            asyncState.BeginReceive(); 
        }

        /// <summary>
        /// Configura o server para o modo sdp
        /// </summary>
        private void ConfigureSdp()
        {
            // sdp não é baseado em conexão

            // define o IP e a Porta em que o servidor ira trabalhar
            Bind(new IPEndPoint(IPAddress.Parse(Cfg.IP), 0)); // Raw socket não trabalha com portas por isso é 0
            SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, 1); // inclui cabeçalhos udp/ip

            var asyncState = new AsyncState(this, Cfg, this, new byte[1024]);
            asyncState.EndPoint = new IPEndPoint(IPAddress.Parse(Cfg.IP), Cfg.Port);
            asyncState.BeginReceive();
        }

        /// <summary>
        /// Inicia escuta de conexões
        /// </summary>
        public void BeginAccept()
        {
            if (acceptModule == null) return;
            // chama modulo de escuta de conexões
            acceptModule.BeginAccept();
        }
    }
}