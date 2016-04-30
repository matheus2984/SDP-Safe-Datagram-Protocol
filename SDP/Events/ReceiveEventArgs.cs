using System;
using SDP.Interfaces;

namespace SDP.Events
{
    /// <summary>
    /// Classe responsavel por encapsular os valores a serem passados durante eventos do tipo recebimento
    /// </summary>
    public class ReceiveEventArgs : EventArgs
    {
        /// <summary>
        /// Usuario da conexão
        /// </summary>
        public IAsyncState State { get; private set; }

        /// <summary>
        /// Pacote recebido
        /// </summary>
        public byte[] ReceivedData { get { return State.ReceivedBuffer; } }

        public ReceiveEventArgs(IAsyncState state)
        {
            State = state;
        }
    }
}
