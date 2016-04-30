using System;
using System.Net.Sockets;
using SDP.Events;

namespace SDP.Interfaces
{
    /// <summary>
    /// Classe base para as classes de Socket dessa biblioteca
    /// </summary>
    internal abstract class AsyncSocketBase : Socket, IAsyncSocket
    {
        /// <summary>
        /// Ocorre quando uma conexão é bem sucedida
        /// </summary>
        public new event EventHandler<ConnectionEventArgs> Connect;

        /// <summary>
        /// Ocorre quando há uma desconexão
        /// </summary>
        public new event EventHandler<ConnectionEventArgs> Disconnect;

        /// <summary>
        /// Ocorre quando dados são recebidos
        /// </summary>
        public new event EventHandler<ReceiveEventArgs> Receive;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="socketInformation"></param>
        protected AsyncSocketBase(SocketInformation socketInformation) : base(socketInformation)
        {

        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="socketType"></param>
        /// <param name="protocolType"></param>
        protected AsyncSocketBase(SocketType socketType, ProtocolType protocolType)
            : base(socketType, protocolType)
        {

        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="addressFamily"></param>
        /// <param name="socketType"></param>
        /// <param name="protocolType"></param>
        protected AsyncSocketBase(AddressFamily addressFamily, SocketType socketType,
            ProtocolType protocolType)
            : base(addressFamily, socketType, protocolType)
        {

        }

        /// <summary>
        /// Chama o evento de conexão
        /// </summary>
        /// <param name="e"></param>
        public void OnConnect(ConnectionEventArgs e)
        {
            var handler = Connect;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Chama o evento de desconexão
        /// </summary>
        /// <param name="e"></param>
        public void OnDisconnect(ConnectionEventArgs e)
        {
            var handler = Disconnect;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Chama o evento de recebimento
        /// </summary>
        /// <param name="e"></param>
        public void OnReceive(ReceiveEventArgs e)
        {
            var handler = Receive;
            if (handler != null) handler(this, e);
        }
    }
}