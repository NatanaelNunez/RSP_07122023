using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Entidades.Exceptions;
using Entidades.Interfaces;
using Entidades.Serializacion;

namespace Entidades.Modelos
{
    /// <summary>
    /// Delegado para notificar la demora en la atención.
    /// </summary>
    /// <param name="demora">Tiempo de demora.</param>
    public delegate void DelegadoDemoraAtencion(double demora);

    /// <summary>
    /// Delegado para notificar un nuevo ingreso al menú.
    /// </summary>
    /// <param name="menu">Menú ingresado.</param>
    public delegate void DelegadoNuevoIngreso(IComestible menu);

    /// <summary>
    /// Clase que representa a un cocinero genérico.
    /// </summary>
    /// <typeparam name="T">Tipo de menú que puede preparar el cocinero.</typeparam>
    public class Cocinero<T> where T : IComestible, new()
    {
        public event DelegadoDemoraAtencion OnDemora;
        public event DelegadoNuevoIngreso OnIngreso;

        private CancellationTokenSource cancellation;

        private int cantPedidosFinalizados;
        private double demoraPreparacionTotal;

        private T menu;
        private string nombre;
        private Task tarea;

        /// <summary>
        /// Obtiene el tiempo medio de preparación.
        /// </summary>
        public double TiempoMedioDePreparacion
        {
            get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados;
        }

        /// <summary>
        /// Obtiene el nombre del cocinero.
        /// </summary>
        public string Nombre
        {
            get => nombre;
        }

        /// <summary>
        /// Obtiene la cantidad de pedidos finalizados.
        /// </summary>
        public int CantPedidosFinalizados
        {
            get => cantPedidosFinalizados;
        }

        /// <summary>
        /// Constructor de la clase Cocinero.
        /// </summary>
        /// <param name="nombre">Nombre del cocinero.</param>
        public Cocinero(string nombre)
        {
            this.nombre = nombre;
        }

        /// <summary>
        /// Propiedad que indica si la cocina está habilitada o no.
        /// </summary>
        public bool HabilitarCocina
        {
            get
            {
                return this.tarea is not null && (this.tarea.Status == TaskStatus.Running ||
                                                  this.tarea.Status == TaskStatus.WaitingToRun ||
                                                  this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && !this.HabilitarCocina)
                {
                    this.cancellation = new CancellationTokenSource();
                    this.IniciarIngreso();
                }
                else
                {
                    this.cancellation.Cancel();
                }
            }
        }

        /// <summary>
        /// Método privado para iniciar el ingreso de pedidos en un hilo separado.
        /// </summary>
        private void IniciarIngreso()
        {
            CancellationToken token = this.cancellation.Token;

            this.tarea = Task.Run(() =>
            {
                while (!this.cancellation.IsCancellationRequested)
                {
                    this.NotificarNuevoIngreso();
                    this.EsperarProximoIngreso();
                    this.cantPedidosFinalizados++;

                    try
                    {
                        // Guardar el ticket en la base de datos.
                        DataBaseManager.GuardarTicket(this.Nombre, this.menu);
                    }
                    catch (DataBaseManagerException ex)
                    {
                        // En caso de error, guardar en un archivo de logs - Punto 6
                        FileManager.Guardar(ex.Message, "logs.txt", true);
                        throw new DataBaseManagerException("Error al guardar el ticket", ex.InnerException);
                    }
                }
            }, token);
        }

        /// <summary>
        /// Método privado para notificar un nuevo ingreso.
        /// </summary>
        private void NotificarNuevoIngreso()
        {
            if (this.OnIngreso is not null)
            {
                this.menu = new T();
                this.menu.IniciarPreparacion();
                this.OnIngreso.Invoke(this.menu);
            }
        }

        /// <summary>
        /// Método privado para esperar el próximo ingreso.
        /// </summary>
        private void EsperarProximoIngreso()
        {
            int tiempoEspera = 0;

            while (this.OnDemora is not null && !this.menu.Estado && !this.cancellation.IsCancellationRequested)
            {
                tiempoEspera++;
                this.OnDemora.Invoke(tiempoEspera);
                Thread.Sleep(1000);
            }

            this.demoraPreparacionTotal += tiempoEspera;
        }
    }
}
