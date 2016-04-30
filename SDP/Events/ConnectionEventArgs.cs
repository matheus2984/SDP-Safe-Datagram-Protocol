using System;
using SDP.Interfaces;

namespace SDP.Events
{
    /// <summary>
    /// Classe responsavel por encapsular os valores a serem enviados para eventos do tipo conexão/desconexão
    /// </summary>
    public class ConnectionEventArgs:EventArgs
    {
        /// <summary>
        /// Usuario da conexão
        /// </summary>
        public IAsyncState State { get; private set; }

        public ConnectionEventArgs(IAsyncState state)
        {
            State = state;
        }
    }
}
