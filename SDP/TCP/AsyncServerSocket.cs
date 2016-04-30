using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using SDP.Events;
using SDP.Interfaces;

namespace SDP.TCP
{
    /// <summary>
    /// Socket de servidor TCP assincrono
    /// </summary>
    internal class AsyncServerSocket : AsyncSocketBase, IAsyncServerSocket
    {
        /// <summary>
        /// configurações do socket
        /// </summary>
        private readonly SocketCfg cfg;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="cfg"></param>
        public AsyncServerSocket(SocketCfg cfg)
            : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
            this.cfg = cfg;

            // Fecha o soquete normalmente sem remanescentes
            SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            // Verifica a conexão para checar se a outra se mantem ativa
            SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            // define o IP e a Porta em que o servidor ira trabalhar
            Bind(new IPEndPoint(IPAddress.Parse(cfg.IP), cfg.Port));
            // limite de conexões que podem existir ao mesmo tempo
            Listen(500);
        }

        /// <summary>
        /// Inicia escuta de conexões
        /// </summary>
        public void BeginAccept()
        {
            // chama AsyncAccept
            BeginAccept(AsyncAccept, null);
        }

        /// <summary>
        /// Aceita uma conexão de forma assincrona
        /// </summary>
        /// <param name="result"></param>
        public void AsyncAccept(IAsyncResult result)
        {
            try
            {
                // recebe o socket referentee ao usuario que realizou a conexão
                var clientSocket = EndAccept(result); 

                if (clientSocket != null)
                {
                    // cria objeto de conexão
                    var state = new AsyncState(this,cfg, clientSocket, new byte[1024]);

                    // invoca evento de conexão realizada
                    OnConnect(new ConnectionEventArgs(state));

                    // inicia recebimento assincrono de dados do usuario conectado
                    state.BeginReceive(); 
                }
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode != SocketError.Disconnecting &&
                    ex.SocketErrorCode != SocketError.NotConnected &&
                    ex.SocketErrorCode != SocketError.ConnectionReset &&
                    ex.SocketErrorCode != SocketError.ConnectionAborted &&
                    ex.SocketErrorCode != SocketError.Shutdown)
                    Debug.WriteLine(ex);
            }
            finally
            {
                // volta a aceitar novas conexões
                BeginAccept();
            }
        }
    }
}