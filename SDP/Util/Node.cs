namespace SDP.Util
{
    /// <summary>
    /// Classe nó, relaciona dois objetos quaisquer (T1, T2)
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    internal class Node<T1, T2> // T1 e T2 são tipos genericos, ou seja, podem representar quaisquer tipo de dados predefinidos na sua criação
    {
        public T1 Value1 { get; set; } 
        public T2 Value2 { get; set; }

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public Node()
        {
            
        }

        /// <summary>
        /// Construtor, seta os valores de Value1 e Value2 diretamente
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        public Node(T1 value1, T2 value2)
        {
            Value1 = value1;
            Value2 = value2;
        }
    }
}