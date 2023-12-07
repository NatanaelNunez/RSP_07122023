using Entidades.Exceptions;
using Entidades.Serializacion;
using Entidades.Interfaces;
using Entidades.Modelos;

namespace FrmView
{
    /// <summary>
    /// Formulario principal que representa la interfaz de usuario para la aplicación.
    /// </summary>
    public partial class FrmView : Form
    {
        private Queue<IComestible> comidas;
        private Cocinero<Hamburguesa> hamburguesero;

        /// <summary>
        /// Inicializa una nueva instancia de la clase FrmView.
        /// </summary>
        public FrmView()
        {
            InitializeComponent();
            this.comidas = new Queue<IComestible>();
            this.hamburguesero = new Cocinero<Hamburguesa>("Pepe");

            // Alumno - agregar manejadores al cocinero
            this.hamburguesero.OnDemora += this.MostrarConteo;
            this.hamburguesero.OnIngreso += this.MostrarComida;
        }

        // Alumno: Realizar los cambios necesarios sobre MostrarComida de manera que se refleje
        // en el formulario los datos de la comida
        private void MostrarComida(IComestible comida)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(() => MostrarComida(comida));
            }
            else
            {
                this.comidas.Enqueue(comida);
                this.pcbComida.Load(comida.Imagen);
                this.rchElaborando.Text = comida.ToString();
            }
        }

        // Alumno: Realizar los cambios necesarios sobre MostrarConteo de manera que se refleje
        // en el formulario el tiempo transcurrido
        private void MostrarConteo(double tiempo)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(() => MostrarConteo(tiempo));
            }
            else
            {
                this.lblTiempo.Text = $"{tiempo} segundos";
                this.lblTmp.Text = $"{this.hamburguesero.TiempoMedioDePreparacion:F1} segundos";
                //this.lblTmp.Text = $"{this.hamburguesero.TiempoMedioDePreparacion.ToString("00.0")} segundos";
            }
        }

        /// <summary>
        /// Actualiza el cuadro de texto con los pedidos finalizados.
        /// </summary>
        /// <param name="comida">Comida que se ha finalizado.</param>
        private void ActualizarAtendidos(IComestible comida)
        {
            this.rchFinalizados.Text += "\n" + comida.Ticket;
        }

        /// <summary>
        /// Manejador del evento de hacer clic en el botón "Abrir/Cerrar Cocina".
        /// </summary>
        private void btnAbrir_Click(object sender, EventArgs e)
        {
            if (!this.hamburguesero.HabilitarCocina)
            {
                this.hamburguesero.HabilitarCocina = true;
                this.btnAbrir.Image = Properties.Resources.close_icon;
            }
            else
            {
                this.hamburguesero.HabilitarCocina = false;
                this.btnAbrir.Image = Properties.Resources.open_icon;
            }
        }

        /// <summary>
        /// Manejador del evento de hacer clic en el botón "Siguiente Pedido".
        /// </summary>
        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (!this.hamburguesero.HabilitarCocina)
            {
                MessageBox.Show("La tienda no esta abierta, vuelva mas tarde", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (this.comidas.Count > 0)
            {
                IComestible comida = this.comidas.Dequeue();
                comida.FinalizarPreparacion(this.hamburguesero.Nombre);
                this.ActualizarAtendidos(comida);
            }
            else
            {
                MessageBox.Show("El Cocinero no posee comidas", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Manejador del evento de cerrar el formulario.
        /// </summary>
        private void FrmView_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Alumno: Serializar el cocinero antes de cerrar el formulario
            try
            {
                FileManager.Serializar(this.hamburguesero, "cocinero.json");
            }
            catch (FileManagerException ex)
            {
                FileManager.Guardar(ex.Message, "logs.txtd", true);
            }
        }

        /// <summary>
        /// Manejador del evento de carga del formulario.
        /// </summary>
        private void FrmView_Load(object sender, EventArgs e)
        {
            // Accidente
        }

        /// <summary>
        /// Manejador del evento de hacer clic en el label "lblTmp".
        /// </summary>
        private void lblTmp_Click(object sender, EventArgs e)
        {
            // Accidente
        }
    }
}
