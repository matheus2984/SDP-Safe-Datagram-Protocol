using System.Net.Sockets;

namespace SDP.Interfaces
{
    /// <summary>
    /// Interface que expõe os elementos indispensaveis para classes de AsyncState/Cliente
    /// </summary>
    public interface IAsyncState
    {
        /// <summary>
        /// Socket da conexão
        /// </summary>
        Socket Socket { get; set; } 

        /// <summary>
        /// Buffer de recebimento de dados
        /// </summary>
        byte[] Buffer { get; set; } 

        /// <summary>
        /// Ultimo pacote de dados recebido
        /// </summary>
        byte[] ReceivedBuffer { get; set; }
    }
}