using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Entidades.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando se produce un error durante la gestión de archivos.
    /// </summary>
    public class FileManagerException : Exception
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase FileManagerException con un mensaje específico.
        /// </summary>
        /// <param name="message">Mensaje que describe la excepción.</param>
        public FileManagerException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase FileManagerException con un mensaje específico y una excepción interna.
        /// </summary>
        /// <param name="message">Mensaje que describe la excepción.</param>
        /// <param name="innerException">Excepción interna que causó la excepción.</param>
        public FileManagerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}