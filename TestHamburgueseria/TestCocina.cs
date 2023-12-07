using Entidades.Exceptions;
using Entidades.Serializacion;
using Entidades.Modelos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MisTest
{
    [TestClass]
    public class TestCocina
    {
        [TestMethod]
        [ExpectedException(typeof(FileManagerException), "Nombre de archivo inválido")]
        public void AlGuardarUnArchivo_ConNombreInvalido_TengoUnaExcepcion()
        {
            // Arrange
            string contenido = "Contenido de prueba";
            string nombreArchivoInvalido = "archivo*.txt";

            // Act
            FileManager.Guardar(contenido, nombreArchivoInvalido, false);

            // Assert
            // La excepción se espera como atributo del atributo ExpectedException del TestMethod

        }

        [TestMethod]
        public void AlInstanciarUnCocinero_SeEspera_PedidosCero()
        {
            // Arrange
            Cocinero<Hamburguesa> cocinero = new Cocinero<Hamburguesa>("Pepe");

            // Act
            int pedidos = cocinero.CantPedidosFinalizados;

            // Assert
            Assert.AreEqual(0, pedidos);
        }
    }
}