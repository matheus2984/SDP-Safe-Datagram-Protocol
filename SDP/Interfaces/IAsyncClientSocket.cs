namespace SDP.Interfaces
{
    /// <summary>
    /// Expõe elementos essenciais para classes de Socket Cliente desta biblioteca
    /// </summary>
    public interface IAsyncClientSocket : IAsyncSocket
    {
        /// <summary>
        /// Inicia tentativa de conexão com o servidor
        /// </summary>
        void BeginConnect();

        /// <summary>
        /// Envia dados
        /// </summary>
        /// <param name="packet"></param>
        void Send(byte[] packet);

        /// <summary>
        /// Envia dados de forma assincrona
        /// </summary>
        /// <param name="packet"></param>
        void SendAsync(byte[] packet);
    }
}