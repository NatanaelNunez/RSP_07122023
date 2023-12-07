namespace Entidades.Interfaces
{
    /// <summary>
    /// Interfaz que define las propiedades y métodos comunes para los objetos comestibles.
    /// </summary>
    public interface IComestible
    {
        /// <summary>
        /// Obtiene un valor que indica el estado del objeto comestible.
        /// </summary>
        bool Estado { get; }

        /// <summary>
        /// Obtiene la URL de la imagen asociada al objeto comestible.
        /// </summary>
        string Imagen { get; }

        /// <summary>
        /// Obtiene un ticket asociado al objeto comestible.
        /// </summary>
        string Ticket { get; }

        /// <summary>
        /// Finaliza la preparación del objeto comestible, asignando información sobre el cocinero.
        /// </summary>
        /// <param name="cocinero">Nombre del cocinero que finaliza la preparación.</param>
        void FinalizarPreparacion(string cocinero);

        /// <summary>
        /// Inicia la preparación del objeto comestible.
        /// </summary>
        void IniciarPreparacion();
    }
}
