using System;
using System.Net;
using System.Net.Sockets;
using SDP.Events;
using SDP.Interfaces;

namespace SDP.TCP
{
    /// <summary>
    /// Socket de cliente TCP assíncrono
    /// </summary>
    internal class AsyncClientSocket : AsyncSocketBase, IAsyncClientSocket
    {
        /// <summary>
        /// Representa o ponto de conexão (IP e Porta) do servidor
        /// </summary>
        private readonly IPEndPoint ipEndPoint;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="serverIP"></param>
        /// <param name="port"></param>
        public AsyncClientSocket(string serverIP, int port)
            : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        { 
            // Fecha o soquete normalmente sem remanescentes
            SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            // Verifica a conexão para checar se a outra se mantem ativa
            SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            // Representa o ponto de conexão (IP e Porta) do servidor
            ipEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), port);
        }

        /// <summary>
        /// Inicia tentativa de conexão com o servidor
        /// </summary>
        public void BeginConnect()
        {
            BeginConnect(ipEndPoint, AsyncConnect, new AsyncState(this, this, new byte[1024]));
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
                var state = (AsyncState) result.AsyncState;

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
                OnConnect(new ConnectionEventArgs(state));

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
                    Console.WriteLine(e);
            }
        }
    }
}