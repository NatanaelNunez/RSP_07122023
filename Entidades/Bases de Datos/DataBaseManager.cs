using System;
using System.Collections.Generic;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Interfaces;
using System.Data.SqlClient;
using Entidades.Serializacion;

/// <summary>
/// Clase estática que gestiona operaciones de base de datos.
/// </summary>
public static class DataBaseManager
{
    private static string connectionString;

    /// <summary>
    /// Inicializa la cadena de conexión estática.
    /// </summary>
    static DataBaseManager()
    {
        connectionString = "Data Source=PCNTN\\SS2;Initial Catalog=20230622SP;User ID=sa;Password=cinettorcel;";
    }

    /// <summary>
    /// Obtiene la URL de la imagen asociada a un tipo de comida desde la base de datos.
    /// </summary>
    /// <param name="tipoComida">Tipo de comida.</param>
    /// <returns>URL de la imagen de la comida.</returns>
    public static string GetImagenComida(string tipoComida)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT imagen FROM Comidas WHERE Tipo_comida = @Tipo";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Tipo", tipoComida);

                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    return (string)result;
                }
                else
                {
                    throw new ComidaInvalidaException("Comida o tipo no válida");
                }
            }
        }
        catch (Exception ex)
        {
            // En caso de error, guardar en un archivo de logs - Punto 6
            FileManager.Guardar(ex.Message, "logs.txt", true);
            throw new DataBaseManagerException("Error al leer", ex);
        }
    }

    /// <summary>
    /// Guarda un ticket en la base de datos.
    /// </summary>
    /// <typeparam name="T">Tipo de comida.</typeparam>
    /// <param name="nombreEmpleado">Nombre del empleado.</param>
    /// <param name="comida">Comida a la que se asocia el ticket.</param>
    public static void GuardarTicket<T>(string nombreEmpleado, T comida)
        where T : IComestible, new()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Tickets 
                                VALUES (@empleado, @tk)";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@empleado", nombreEmpleado);
                command.Parameters.AddWithValue("@tk", comida.Ticket);

                connection.Open();

                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            // En caso de error, guardar en un archivo de logs - Punto 6
            FileManager.Guardar(ex.Message, "logs.txt", true);
            throw new DataBaseManagerException("Error al escribir", ex);
        }
    }
}
