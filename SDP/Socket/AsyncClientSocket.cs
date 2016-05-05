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

        private  IConnect connectModule;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="addressFamily"></param>
        /// <param name="socketType"></param>
        /// <param name="protocolType"></param>
        public AsyncClientSocket(SocketCfg cfg, AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
            : base(addressFamily, socketType, protocolType)
        {
            // objeto de configuração do socket
            Cfg = cfg;
            
            // Representa o ponto de conexão (IP e Porta) do servidor
            IpEndPoint = new IPEndPoint(IPAddress.Parse(cfg.IP), cfg.Port);

            switch (cfg.ProtocolType)
            {
                case Enums.ProtocolType.TCP:
                 ConfigureTcp();
                    break;
                case Enums.ProtocolType.UDP:
                    ConfigureUdp();
                    break;
                case Enums.ProtocolType.SDP:
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
            // inicia modulo de conexão
            connectModule = new TcpConnectModule(this);
        }

        /// <summary>
        /// Configura o server para o modo udp
        /// </summary>
        private void ConfigureUdp()
        {
            // udp não é baseado em conexão

            AsyncState = new AsyncState(this, Cfg, this, new byte[1024]);
            ((AsyncState)(AsyncState)).endPoint = new IPEndPoint(IPAddress.Parse(Cfg.IP), Cfg.Port);
            ((AsyncState)(AsyncState)).BeginReceive();
        }

        /// <summary>
        /// Inicia tentativa de conexão com o servidor
        /// </summary>
        public void BeginConnect()
        {
            switch (Cfg.ProtocolType)
            {
                case Enums.ProtocolType.TCP:
                    connectModule.BeginConnect();
                    break;
                case Enums.ProtocolType.UDP:
                // udp não é baseado em conexão
                //    throw new Exception("Este protocolo não pode estabelecer uma conexão");
                case Enums.ProtocolType.SDP:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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