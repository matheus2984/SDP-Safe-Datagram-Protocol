using System;
using System.Diagnostics;
using System.Net.Sockets;
using SDP.Events;
using SDP.Interfaces;
using SDP.Socket;

namespace SDP.Modules.TCP
{
    /// <summary>
    /// Modulo de escuta de novas conexões
    /// </summary>
    internal class TcpAcceptModule:IAccept
    {
        /// <summary>
        /// Servidor host
        /// </summary>
        private readonly AsyncServerSocket server;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="server"></param>
        internal TcpAcceptModule(AsyncServerSocket server)
        {
            this.server = server;
        }

        /// <summary>
        /// Inicia escuta de conexões
        /// </summary>
        public void BeginAccept()
        {
            // chama AsyncAccept
            server.BeginAccept(AsyncAccept, null);
        }

        /// <summary>
        /// Aceita uma conexão de forma assincrona
        /// </summary>
        /// <param name="result"></param>
        private void AsyncAccept(IAsyncResult result)
        {
            try
            {
                // recebe o socket referentee ao usuario que realizou a conexão
                var clientSocket = server.EndAccept(result);

                if (clientSocket != null)
                {
                    // cria objeto de conexão
                    var state = new AsyncState(server, server.Cfg, clientSocket, new byte[1024]);

                    // invoca evento de conexão realizada
                    server.OnConnect(new ConnectionEventArgs(state));

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
