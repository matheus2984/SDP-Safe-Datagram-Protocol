using SDP.Interfaces;
using SDP.TCP;

namespace SDP
{
    /// <summary>
    /// Classe Factory responsavel por criar os Sockets e retornar apenas o necessario para sua manipulação
    /// </summary>
    public static class SdpSocket
    {
        /// <summary>
        /// Responsavel por criar o socket de servidor de acordo com a configuração especificada pelo parametro
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static IAsyncServerSocket ServerFactory(SocketCfg cfg)
        {
            return new AsyncServerSocket(cfg); // Retorna um novo socket de servidor TCP. Unico implementado atualmente
        }

        /// <summary>
        /// Responsavel por criar o socket de cliente de acordo com a configuração especificada pelos parametros
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static IAsyncClientSocket ClientFactory(SocketCfg cfg)
        {
            return new AsyncClientSocket(cfg); // Retorna um novo socket de cliente TCP. Unico implementado atualmente
        }
    }
}