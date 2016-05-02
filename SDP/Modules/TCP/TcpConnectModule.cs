using System;
using System.Diagnostics;
using System.Net.Sockets;
using SDP.Events;
using SDP.Interfaces;
using SDP.Socket;

namespace SDP.Modules.TCP
{
    /// <summary>
    /// Modulo de conexão
    /// </summary>
    internal class TcpConnectModule:IConnect
    {    
        /// <summary>
        /// Objeto de conexão
        /// </summary>
        private readonly AsyncClientSocket clientHost;

        public TcpConnectModule(AsyncClientSocket client)
        {
            clientHost = client;
        }

        /// <summary>
        /// Inicia tentativa de conexão com o servidor
        /// </summary>
        public void BeginConnect()
        {
            // cria objeto de conexão
            clientHost.AsyncState = new AsyncState(clientHost, clientHost.Cfg, clientHost, new byte[1024]);
            // chama AsyncConnect
            clientHost.BeginConnect(clientHost.IpEndPoint, AsyncConnect, clientHost.AsyncState);
        }

        /// <summary>
        /// Realiza conexão assincrona com o servidor
        /// </summary>
        /// <param name="result"></param>
        private void AsyncConnect(IAsyncResult result)
        {
            try
            {
                // objeto de conexão
                var state = (AsyncState)result.AsyncState;

                try
                {
                    // conexão concluida
                    state.Socket.EndConnect(result);
                }
                catch (SocketException exception)
                {
                    Console.WriteLine(exception);
                }

                if (!state.Socket.Connected) return;

                // invoca evento de conexão realizada com sucesso
                clientHost.OnConnect(new ConnectionEventArgs(state));

                // inicia recebimento assíncrono
                state.BeginReceive();
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode != SocketError.Disconnecting &&
                    e.SocketErrorCode != SocketError.NotConnected &&
                    e.SocketErrorCode != SocketError.ConnectionReset &&
                    e.SocketErrorCode != SocketError.ConnectionAborted &&
                    e.SocketErrorCode != SocketError.Shutdown)
                    Debug.WriteLine(e);
            }
        }
    }
}