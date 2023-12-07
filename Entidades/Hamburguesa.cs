using Entidades.Enumerados;
using Entidades.Exceptions;
using Entidades.Serializacion;
using Entidades.Interfaces;
using Entidades.MetodosDeExtension;
using System.Text;

namespace Entidades.Modelos
{
    /// <summary>
    /// Representa una hamburguesa que implementa la interfaz IComestible.
    /// </summary>
    public class Hamburguesa : IComestible
    {
        private static int costoBase;
        private bool esDoble;
        private double costo;
        private bool estado;
        private string imagen;

        List<EIngrediente> ingredientes;
        Random random;

        /// <summary>
        /// Obtiene un ticket que representa la información de la hamburguesa y el total a pagar.
        /// </summary>
        public string Ticket => $"{this}\nTotal a pagar:{this.costo}";

        /// <summary>
        /// Obtiene el estado actual de la hamburguesa.
        /// </summary>
        public bool Estado => this.estado;

        /// <summary>
        /// Obtiene la imagen asociada a la hamburguesa.
        /// </summary>
        public string Imagen => this.imagen;

        /// <summary>
        /// Agrega ingredientes de forma aleatoria a la hamburguesa.
        /// </summary>
        private void AgregarIngredientes()
        {
            this.ingredientes = this.random.IngredientesAleatorios();
        }

        /// <summary>
        /// Finaliza la preparación de la hamburguesa, calcula el costo total y cambia el estado.
        /// </summary>
        /// <param name="cocinero">Nombre del cocinero que finaliza la preparación.</param>
        public void FinalizarPreparacion(string cocinero)
        {
            this.costo = this.ingredientes.CalcularCostoIngredientes(Hamburguesa.costoBase);
            this.estado = !this.Estado;
        }

        /// <summary>
        /// Inicializa el costo base de la hamburguesa.
        /// </summary>
        static Hamburguesa() => Hamburguesa.costoBase = 1500;

        /// <summary>
        /// Inicializa una nueva instancia de la clase Hamburguesa.
        /// </summary>
        public Hamburguesa() : this(false) { }

        /// <summary>
        /// Inicializa una nueva instancia de la clase Hamburguesa con la opción de ser doble.
        /// </summary>
        /// <param name="esDoble">Indica si la hamburguesa es doble.</param>
        public Hamburguesa(bool esDoble)
        {
            this.esDoble = esDoble;
            this.random = new Random();
            this.ingredientes = new List<EIngrediente>();
        }

        /// <summary>
        /// Inicia la preparación de la hamburguesa si no está en estado preparado.
        /// </summary>
        public void IniciarPreparacion()
        {
            if (!this.estado)
            {
                int indice = this.random.Next(1, 9);

                try
                {
                    this.imagen = DataBaseManager.GetImagenComida($"Hamburguesa_{indice}");
                    this.AgregarIngredientes();
                }
                catch (DataBaseManagerException ex)
                {
                    // En caso de error, guardar en un archivo de logs - Punto 6
                    FileManager.Guardar(ex.Message, "logs.txt", true);
                    throw new DataBaseManagerException(ex.Message);
                }
            }
        }

        /// <summary>
        /// Muestra los datos de la hamburguesa, incluyendo su tipo, ingredientes, etc.
        /// </summary>
        /// <returns>Una cadena que representa los datos de la hamburguesa.</returns>
        private string MostrarDatos()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Hamburguesa {(this.esDoble ? "Doble" : "Simple")}");
            stringBuilder.AppendLine("Ingredientes: ");

            this.ingredientes.ForEach(i => stringBuilder.AppendLine(i.ToString()));

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Devuelve una cadena que representa los datos de la hamburguesa.
        /// </summary>
        /// <returns>Una cadena que representa los datos de la hamburguesa.</returns>
        public override string ToString() => this.MostrarDatos();
    }
}
