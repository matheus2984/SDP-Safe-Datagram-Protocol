namespace SDP.Interfaces
{ 
    /// <summary>
    /// Expõe os elementos essenciais para classes que contem metodos de Envio de dados
    /// </summary>
    public interface ISend
    {
        void Send(byte[] packet);
        void AsyncSend(byte[] packet);
    }
}