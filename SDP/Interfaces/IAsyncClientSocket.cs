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
    }
}