using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Excepciones
{
    /// <summary>
    /// Excepción lanzada cuando se detecta que la comida o el tipo de comida no es válida.
    /// </summary>
    public class ComidaInvalidaException : Exception
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase ComidaInvalidaException con un mensaje específico.
        /// </summary>
        /// <param name="message">Mensaje que describe la excepción.</param>
        public ComidaInvalidaException(string? message) : base(message)
        {
        }
    }
}