using System.Collections.Generic;
using System.Net.Sockets;
using SDP.Util;
using SocketType = SDP.Enums.SocketType;
using ProtocolType = SDP.Enums.ProtocolType;

namespace SDP
{
    /// <summary>
    /// Classe de configuração para a criação de sockets referentes a SdpSocket
    /// </summary>
    public class SocketCfg
    {
        /// <summary>
        /// Endereço
        /// </summary>
        public string IP { get; private set; }

        /// <summary>
        /// Porta
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Algoritmo de controle de fluxo
        /// </summary>
        public bool NagleAlgorithm { get; set; }

        /// <summary>
        /// Tipo de rede
        /// </summary>
        internal AddressFamily AddressFamily { get; set; }

        /// <summary>
        /// Tipo de transmissão
        /// </summary>
        internal SocketType SocketType { get; set; }

        /// <summary>
        /// Tipo de protocolo
        /// </summary>
        internal ProtocolType ProtocolType { get; set; }

        /// <summary>
        /// Lista de opções setadas do socket
        /// </summary>
        private readonly List<Node<SocketOptionLevel, SocketOptionName>> socketOptionList;

        /// <summary>
        /// Construtor
        /// </summary>
        public SocketCfg(string ip, int port, ProtocolType protocolType)
        {
            socketOptionList=new List<Node<SocketOptionLevel, SocketOptionName>>();
            IP = ip;
            Port = port;
        }

        /// <summary>
        /// Coloca uma opção na lista de opções do socket
        /// </summary>
        /// <param name="optionLevel"></param>
        /// <param name="optionName"></param>
        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName)
        {
            // as opções são setadas atraves de uma relação entre o nivel (Socket, IP, TCP, UDP) e o tipo da opção desejada
            var node = new Node<SocketOptionLevel, SocketOptionName>
            {
                Value1 = optionLevel,
                Value2 = optionName
            };

            socketOptionList.Add(node);
        }
    }
}
