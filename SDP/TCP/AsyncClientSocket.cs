using System;
using System.Diagnostics;
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
        /// Configurações do socket
        /// </summary>
        private readonly SocketCfg cfg;

        /// <summary>
        /// Objeto de conexão
        /// </summary>
        private IAsyncState asyncState;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="cfg"></param>
        public AsyncClientSocket(SocketCfg cfg)
            : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
            // objeto de configuração do socket
            this.cfg = cfg;

            // Fecha o soquete normalmente sem remanescentes
            SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            // Verifica a conexão para checar se a outra se mantem ativa
            SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            // Representa o ponto de conexão (IP e Porta) do servidor
            ipEndPoint = new IPEndPoint(IPAddress.Parse(cfg.IP), cfg.Port);
        }

        /// <summary>
        /// Inicia tentativa de conexão com o servidor
        /// </summary>
        public void BeginConnect()
        {
            // cria objeto de conexão
            asyncState = new AsyncState(this, cfg, this, new byte[1024]);
            // chama AsyncConnect
            BeginConnect(ipEndPoint, AsyncConnect, asyncState);
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
                    Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// Envia dados
        /// </summary>
        /// <param name="packet"></param>
        public new void Send(byte[] packet)
        {
            asyncState.Send(packet);
        }
    }
}