using System;
using SDP.Events;

namespace SDP.Interfaces
{
    /// <summary>
    /// Expõe os elementos essenciais para os Sockets desta biblioteca
    /// </summary>
    public interface IAsyncSocket
    {
        /// <summary>
        /// Ocorre quando uma conexão é bem sucedida
        /// </summary>
        event EventHandler<ConnectionEventArgs> Connect;
        
        /// <summary>
        /// Ocorre quando há uma desconexão
        /// </summary>
        event EventHandler<ConnectionEventArgs> Disconnect;

        /// <summary>
        /// Ocorre quando dados são recebidos
        /// </summary>
        event EventHandler<ReceiveEventArgs> Receive;

        /// <summary>
        /// Chama o evento de conexão
        /// </summary>
        /// <param name="e"></param>
        void OnConnect(ConnectionEventArgs e);

        /// <summary>
        /// Chama o evento de desconexão
        /// </summary>
        /// <param name="e"></param>
        void OnDisconnect(ConnectionEventArgs e);

        /// <summary>
        /// Chama o evento de recebimento
        /// </summary>
        /// <param name="e"></param>
        void OnReceive(ReceiveEventArgs e);
    }
}