using System;
using SDP.Enums;
using SDP.Interfaces;
using SDP.Socket;

namespace SDP
{
    /// <summary>
    /// Classe Factory responsavel por criar os Sockets e retornar apenas o necessario para sua manipulação
    /// </summary>
    public static class SdpSocket
    {   
        /// <summary>
        /// Responsavel por criar o sockets de servidor
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static IAsyncServerSocket ServerFactory(SocketCfg cfg)
        {
            // retorna um novo socket de servidor de acordo com o protocolo definido na configuração
            switch (cfg.ProtocolType)
            {
                case ProtocolType.TCP:
                    return new AsyncServerSocket(cfg);
                case ProtocolType.UDP:
                    throw new NotImplementedException();
                case ProtocolType.SDP:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Responsavel por criar sockets de cliente
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static IAsyncClientSocket ClientFactory(SocketCfg cfg)
        {
            // retorna um novo socket de cliente de acordo com o protocolo definido na configuração
            switch (cfg.ProtocolType)
            {
                case ProtocolType.TCP:
                    return new AsyncClientSocket(cfg);
                case ProtocolType.UDP:
                    throw new NotImplementedException();
                case ProtocolType.SDP:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}