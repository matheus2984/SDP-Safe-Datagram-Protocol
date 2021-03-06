﻿using System;
using System.Net.Sockets;
using SDP.Interfaces;
using SDP.Socket;
using ProtocolType = SDP.Enums.ProtocolType;

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
                    return new AsyncServerSocket(cfg, AddressFamily.InterNetwork, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                case ProtocolType.UDP:
                    return new AsyncServerSocket(cfg, AddressFamily.InterNetwork, SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp);
                case ProtocolType.SDP:
                    return new AsyncServerSocket(cfg, AddressFamily.InterNetwork,SocketType.Raw, System.Net.Sockets.ProtocolType.Udp);
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
                    return new AsyncClientSocket(cfg, AddressFamily.InterNetwork, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                case ProtocolType.UDP:
                    return new AsyncClientSocket(cfg, AddressFamily.InterNetwork, SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp);
                case ProtocolType.SDP:
                    return new AsyncClientSocket(cfg, AddressFamily.InterNetwork, SocketType.Raw, System.Net.Sockets.ProtocolType.Udp);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}