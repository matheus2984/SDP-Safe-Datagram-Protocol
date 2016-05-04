using System;
using System.Diagnostics;
using System.Net.Sockets;
using SDP.Events;
using SDP.Socket;

namespace SDP.Modules.UDP
{
    /// <summary>
    /// Modulo de recebimento de dados do tipo datagrama
    /// </summary>
    internal class UdpDatagramReceiveModule
    {
        /// <summary>
        ///     Objeto de conexão
        /// </summary>
        private readonly AsyncState asyncState;

        /// <summary>
        ///     Construtor
        /// </summary>
        /// <param name="state"></param>
        public UdpDatagramReceiveModule(AsyncState state)
        {
            asyncState = state;
        }

        /// <summary>
        ///     Recebe os 2 primeiros bytes do pacote (cabeçalho) de forma assincrona
        ///     estes bytes representam o tamanho do resto do pacote
        ///     isso é necessario para realizar a quebra da stream em datagramas
        /// </summary>
        /// <param name="result"></param>
        private void AsyncReceiveHeader(IAsyncResult result)
        {
            try
            {
                SocketError socketError;

                if (!asyncState.Socket.Connected)
                    return;

                // representa a quantidade de bytes que foi recebido, caso ocorra erro ele sera armazenado em 'socketError'
                int size = asyncState.Socket.EndReceive(result, out socketError);

                if (socketError == SocketError.Success && size == 2)
                {
                    asyncState.ReceiveBufferHeader = new byte[size];
                    Buffer.BlockCopy(asyncState.Buffer, 0, asyncState.ReceiveBufferHeader, 0, size);

                    // tamanho do pacote
                    ushort packetHeaderLength = BitConverter.ToUInt16(asyncState.ReceiveBufferHeader, 0);
                    // buffer de recebimento setado para o tamanho do pacote
                    asyncState.ReceivedBuffer = new byte[packetHeaderLength];

                    // chama AsyncReceiveBody para receber o pacote
                    asyncState.Socket.BeginReceive(asyncState.ReceivedBuffer, 0, packetHeaderLength,
                        SocketFlags.None, AsyncReceiveBody, asyncState);
                }
                else
                {
                    // se ocorreu erro desconectar
                    asyncState.BeginDisconnect();
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
        }

        /// <summary>
        ///     Recebe o resto do pacote, ou seja o conteudo real do pacote
        /// </summary>
        /// <param name="result"></param>
        private void AsyncReceiveBody(IAsyncResult result)
        {
            try
            {
                SocketError socketError;

                if (!asyncState.Socket.Connected)
                    return;

                // representa a quantidade de bytes que foi recebido, caso ocorra erro ele sera armazenado em 'socketError'
                int size = asyncState.Socket.EndReceive(result, out socketError);

                if (socketError == SocketError.Success && size != 0)
                {
                    // chama evento de recebimento
                    asyncState.AsyncSocket.OnReceive(new ReceiveEventArgs(asyncState));

                    // volta a receber dados
                    BeginReceive();
                }
                else
                {
                    // se ocorreu erro desconectar
                    asyncState.BeginDisconnect();
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
        }

        /// <summary>
        ///     Inicia recebimento de dados de forma assíncrona
        /// </summary>
        public void BeginReceive()
        {
            asyncState.Socket.BeginReceive(asyncState.Buffer, 0, 2, SocketFlags.None, AsyncReceiveHeader, asyncState);
        }
    }
}