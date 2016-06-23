using System;
using System.Net;
using System.Net.Sockets;
using SDP.Events;
using SDP.Interfaces;
using SDP.Socket;

namespace SDP.Modules.UDP
{
    /// <summary>
    /// Modulo de recebimento de dados do tipo datagrama
    /// </summary>
    internal class UdpDatagramReceiveModule : IReceive
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

        private void DoReceiveFrom(IAsyncResult iar)
        {
            EndPoint clientEp = new IPEndPoint(IPAddress.Any, 0);
            int msgLen = asyncState.Socket.EndReceiveFrom(iar, ref clientEp);
            asyncState.EndPoint = clientEp;
            asyncState.ReceivedBuffer = new byte[msgLen];
            Buffer.BlockCopy(asyncState.Buffer, 0, asyncState.ReceivedBuffer, 0, msgLen);

            asyncState.AsyncSocket.OnReceive(new ReceiveEventArgs(asyncState));

            BeginReceive();
        }

        private void ReceiveData(IAsyncResult ar)
        {
            int msgLen = asyncState.Socket.EndReceive(ar);
            asyncState.ReceivedBuffer = new byte[msgLen];
            Buffer.BlockCopy(asyncState.Buffer, 0, asyncState.ReceivedBuffer, 0, msgLen);

            asyncState.AsyncSocket.OnReceive(new ReceiveEventArgs(asyncState));

            BeginReceive();
        }

        /// <summary>
        ///     Inicia recebimento de dados de forma assíncrona
        /// </summary>
        public void BeginReceive()
        {
            EndPoint remoteEnd = new IPEndPoint(IPAddress.Any, 0);
            if (asyncState.AsyncSocket is IAsyncClientSocket)
            {
             //   asyncState.Socket.BeginReceiveFrom(asyncState.Buffer, 0, asyncState.Buffer.Length, SocketFlags.None,
              //  ref remoteEnd, DoReceiveFrom, null);
                
            }
            else
            {
                asyncState.Socket.BeginReceiveFrom(asyncState.Buffer, 0, asyncState.Buffer.Length, SocketFlags.None,
                    ref remoteEnd, DoReceiveFrom, null);
            }
        }
    }
}