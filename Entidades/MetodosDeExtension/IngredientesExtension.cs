using Entidades.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entidades.MetodosDeExtension
{
    /// <summary>
    /// Clase de métodos de extensión para la manipulación de listas de ingredientes.
    /// </summary>
    public static class IngredientesExtension
    {
        /// <summary>
        /// Calcula el costo total de los ingredientes en base al costo inicial y los porcentajes de aumento asociados a cada ingrediente.
        /// </summary>
        /// <param name="ingredientes">Lista de ingredientes.</param>
        /// <param name="costoInicial">Costo inicial al que se aplicarán los aumentos.</param>
        /// <returns>Costo total de los ingredientes.</returns>
        public static double CalcularCostoIngredientes(this List<EIngrediente> ingredientes, int costoInicial)
        {
            double costoTotal = costoInicial;

            foreach (EIngrediente ingrediente in ingredientes)
            {
                int porcentajeAumento = (int)ingrediente;
                double aumento = costoTotal * porcentajeAumento / 100;

                costoTotal = costoTotal + aumento;
            }

            return costoTotal;
        }

        /// <summary>
        /// Genera una lista de ingredientes de forma aleatoria en base a los valores del enumerado EIngrediente.
        /// </summary>
        /// <param name="random">Instancia de la clase Random utilizada para generar números aleatorios.</param>
        /// <returns>Lista de ingredientes aleatorios.</returns>
        public static List<EIngrediente> IngredientesAleatorios(this Random random)
        {
            List<EIngrediente> ingredientes = Enum.GetValues(typeof(EIngrediente)).Cast<EIngrediente>().ToList();

            int cantidadIngredientes = ingredientes.Count;

            int cantidadRandom = random.Next(1, cantidadIngredientes + 1);

            List<EIngrediente> ingredientesAleatorios = new List<EIngrediente>();

            for (int i = 0; i < cantidadRandom; i++)
            {
                EIngrediente randomIngredient = ingredientes[random.Next(cantidadIngredientes)];
                ingredientesAleatorios.Add(randomIngredient);
            }

            return ingredientesAleatorios;
        }
    }
}
