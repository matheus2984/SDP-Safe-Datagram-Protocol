namespace SDP.Interfaces
{
    /// <summary>
    /// Expõe elementos essenciais para classes de Socket Servidor desta biblioteca
    /// </summary>
    public interface IAsyncServerSocket : IAsyncSocket
    {
        /// <summary>
        /// Inicia escuta de novas conexões no socket
        /// </summary>
        void BeginAccept();
    }
}