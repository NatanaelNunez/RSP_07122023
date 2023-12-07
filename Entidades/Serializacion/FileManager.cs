using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Entidades.Serializacion
{
    /// <summary>
    /// Clase de utilidad para el manejo de archivos, incluyendo la serialización de objetos a formato JSON.
    /// </summary>
    public static class FileManager
    {
        // La ruta del directorio se define como estática para todas las instancias de la clase.
        private static string path;

        /// <summary>
        /// Constructor estático que inicializa la ruta del directorio en el escritorio del usuario.
        /// </summary>
        static FileManager()
        {
            // La ruta se compone con la carpeta del escritorio y un directorio específico.
            FileManager.path = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\20231207_Nuñez_Ignacio_Natanael\\";
            // Se verifica y crea el directorio si no existe.
            FileManager.ValidaExistenciaDeDirectorio();
        }

        /// <summary>
        /// Verifica la existencia del directorio y lo crea si no existe.
        /// </summary>
        private static void ValidaExistenciaDeDirectorio()
        {
            try
            {
                // Si no existe el directorio se crea.
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                // Captura la excepción y relanza una nueva excepción específica.
                throw new Exceptions.FileManagerException("Error al crear el directorio:", ex);
            }
        }

        /// <summary>
        /// Guarda datos en un archivo, permitiendo la opción de agregar al archivo existente.
        /// </summary>
        /// <param name="data">Datos a guardar en el archivo.</param>
        /// <param name="nombreArchivo">Nombre del archivo.</param>
        /// <param name="append">Indica si se debe agregar al archivo existente.</param>
        public static void Guardar(string data, string nombreArchivo, bool append)
        {
            try
            {
                string filePath = Path.Combine(path, nombreArchivo);

                // Formatear el contenido con fecha y hora - Extra visual
                string formattedData = $"{DateTime.Now} - {data}{Environment.NewLine}{new string('-', 50)}{Environment.NewLine}";

                if (append && File.Exists(filePath))
                {
                    // Si se especifica "append" y el archivo existe, se agregan los datos.
                    File.AppendAllText(filePath, formattedData);
                }
                else
                {
                    // Si no se especifica "append" o el archivo no existe, se sobrescribe el archivo.
                    File.WriteAllText(filePath, formattedData);
                }
            }
            catch (Exception ex)
            {
                // En caso de error, guardar en un archivo de logs - Punto 6
                FileManager.Guardar(ex.Message, "logs.txt", true);
                // Captura la excepción y relanza una nueva excepción específica.
                throw new Exceptions.FileManagerException("Error al guardar:", ex);
            }
        }

        /// <summary>
        /// Serializa un objeto a formato JSON y lo guarda en un archivo.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto a serializar.</typeparam>
        /// <param name="elementos">Objeto a serializar.</param>
        /// <param name="nombreArchivo">Nombre del archivo.</param>
        /// <returns>True si la serialización fue exitosa.</returns>
        public static bool Serializar<T>(T elementos, string nombreArchivo) where T : class
        {
            try
            {
                string filePath = Path.Combine(path, nombreArchivo);
                Console.WriteLine($"Ruta del archivo de salida: {filePath}");

                // Serializar el objeto a formato JSON.
                string jsonString = JsonConvert.SerializeObject(elementos);
                // Escribir el JSON en el archivo.
                File.WriteAllText(filePath, jsonString);

                // Retornar true al terminar la serialización.
                return true;
            }
            catch (Exception ex)
            {
                // En caso de error, guardar en un archivo de logs - Punto 6
                FileManager.Guardar(ex.Message, "logs.txt", true);
                // Captura la excepción y relanza una nueva excepción específica.
                throw new Entidades.Exceptions.FileManagerException("Error al serializar el objeto:", ex);
            }
        }
    }
}
