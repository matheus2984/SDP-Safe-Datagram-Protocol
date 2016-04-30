using System.Collections.Generic;
using System.Net.Sockets;
using SDP.Util;

namespace SDP.Cfg
{
    /// <summary>
    /// Classe de configuração para a criação de sockets referentes a SdpSocket
    /// </summary>
    public class TcpCfg
    {
        /// <summary>
        /// Endereço
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// Porta
        /// </summary>
        public int Port { get; set; }

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
        public TcpCfg()
        {
            socketOptionList=new List<Node<SocketOptionLevel, SocketOptionName>>();
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
