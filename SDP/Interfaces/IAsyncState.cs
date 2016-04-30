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
        // Socket Socket { get; set; } 

        /// <summary>
        /// Buffer de recebimento de dados
        /// </summary>
        byte[] Buffer { get; set; }

        /// <summary>
        /// Ultimo pacote de dados recebido
        /// </summary>
        byte[] ReceivedBuffer { get; set; }

        /// <summary>
        /// Envia dados
        /// </summary>
        /// <param name="packet"></param>
        void Send(byte[] packet);

        /// <summary>
        /// Envia dados de forma assíncrona
        /// </summary>
        /// <param name="packet"></param>
        void AsyncSend(byte[] packet);
    }
}