using System;
using System.Data;
using System.Windows.Forms;

namespace CapaVista_Pedidos
{
    // Natali Sofía Montenegro Portillo | 0901-23-10017 | Selecciona un cliente y muestra su tipo sin depender de los formularios.
    public partial class VentasUsrCliente : UserControl
    {
        internal Label VentasLblCampoCliente { get { return VentasLblCliente; } }

        public ComboBox VentasCboCliente { get { return VentasCboSeleccionCliente; } }

        public VentasUsrCliente()
        {
            InitializeComponent();
        }

        public int TipoClienteId
        {
            get
            {
                var Fila = VentasCboCliente.SelectedItem as DataRowView;
                return Fila == null || Fila["TipoClienteId"] == DBNull.Value
                    ? 0 : Convert.ToInt32(Fila["TipoClienteId"]);
            }
        }

        public void VentasMetCargarClientes(DataTable Clientes)
        {
            VentasCboCliente.DataSource = null;
            if (Clientes == null || !Clientes.Columns.Contains("Id") ||
                !Clientes.Columns.Contains("Nombre") || !Clientes.Columns.Contains("TipoClienteId") ||
                !Clientes.Columns.Contains("TipoCliente"))
                throw new ArgumentException("El catálogo de clientes no tiene los campos requeridos.");
            VentasCboCliente.DisplayMember = "Nombre";
            VentasCboCliente.ValueMember = "Id";
            VentasCboCliente.DataSource = Clientes;
            VentasCboCliente.SelectedIndex = -1;
        }

        private void VentasMetMostrarTipo()
        {
            var Fila = VentasCboCliente.SelectedItem as DataRowView;
            VentasLblTipo.Text = Fila == null ? "Tipo: seleccione un cliente"
                : "Tipo: " + Convert.ToString(Fila["TipoCliente"]);
        }

        private void VentasMetClienteCambiado(object Origen, EventArgs Evento) { VentasMetMostrarTipo(); }
    }
}
