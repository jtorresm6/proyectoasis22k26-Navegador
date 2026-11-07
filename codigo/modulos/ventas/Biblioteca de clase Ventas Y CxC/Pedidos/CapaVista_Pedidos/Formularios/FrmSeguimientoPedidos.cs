using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_Pedidos;

namespace CapaVista_Pedidos
{
    // Natali Sofía Montenegro Portillo | 0901-23-10017 | Lista pedidos y abre consulta, anulación y reporte.
    public partial class FrmSeguimientoPedidos : Form
    {
        private readonly ClsPedidoControlador _Controlador = new ClsPedidoControlador();
        private DataTable _Pedidos = new DataTable();

        public FrmSeguimientoPedidos()
        {
            InitializeComponent();
            Load += (Origen, Evento) => VentasMetCargar();
        }
        private void VentasMetReporteClick(object Origen, EventArgs Evento) { VentasMetReporte(); }
        private void VentasMetAyudaClick(object Origen, EventArgs Evento) { VentasMetAyuda(); }
        protected override bool ProcessCmdKey(ref Message Mensaje, Keys Tecla)
        {
            if (Tecla == Keys.F1)
            {
                VentasMetAyuda();
                return true;
            }
            return base.ProcessCmdKey(ref Mensaje, Tecla);
        }
        private void VentasMetReporte()
        {
            try
            {
                using (var Formulario = new FrmReportePedidos(_Controlador)) Formulario.ShowDialog(this);
            }
            catch (Exception Error) { MessageBox.Show(this, Error.Message, "Reporte de pedidos", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void VentasMetAyuda()
        {
            try
            {
                Help.ShowHelp(this, _Controlador.VentasFuncRutaAyuda(typeof(FrmSeguimientoPedidos).Assembly),
                    HelpNavigator.Topic, "html/Seguimiento_Formulario.html");
            }
            catch (Exception Error)
            {
                MessageBox.Show(this, "No fue posible abrir la ayuda. " + Error.Message,
                    "Ayuda de pedidos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void VentasMetCargar()
        {
            try { _Pedidos = _Controlador.VentasFuncListar(); VentasMetMostrar(); }
            catch (Exception Error) { MessageBox.Show(this, Error.Message, "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void VentasMetMostrar()
        {
            VentasDgvPedidos.Rows.Clear();
            string Estado = Convert.ToString(VentasCboEstado.SelectedItem), Buscar = VentasTxtBuscar.Text.Trim();
            foreach (DataRow Pedido in _Pedidos.Rows)
            {
                string Numero = Convert.ToString(Pedido["Numero"]), Cliente = Convert.ToString(Pedido["Cliente"]);
                if (Estado != "Todos los estados" && Estado != Convert.ToString(Pedido["Estado"])) continue;
                if (Buscar.Length > 0 && Numero.IndexOf(Buscar, StringComparison.OrdinalIgnoreCase) < 0 &&
                    Cliente.IndexOf(Buscar, StringComparison.OrdinalIgnoreCase) < 0) continue;
                int Fila = VentasDgvPedidos.Rows.Add(Numero, Cliente,
                    Convert.ToDateTime(Pedido["Fecha"]).ToString("dd/MM/yyyy"), Pedido["Estado"]);
                VentasDgvPedidos.Rows[Fila].Tag = Pedido;
            }
            VentasMetAccion(VentasDgvPedidos, EventArgs.Empty);
        }

        private void VentasMetAccion(object Origen, EventArgs Evento)
        {
            var Pedido = VentasDgvPedidos.CurrentRow?.Tag as DataRow;
            try
            {
                if (Origen == VentasBtnSalir) { Close(); return; }
                if (Origen == VentasBtnActualizar) { VentasMetCargar(); return; }
                if (Origen == VentasCboEstado || Origen == VentasTxtBuscar) { VentasMetMostrar(); return; }
                if (Origen == VentasDgvPedidos && Evento is DataGridViewCellEventArgs Celda && Celda.RowIndex >= 0)
                    Origen = VentasBtnConsultar;
                if (Origen == VentasDgvPedidos)
                {
                    bool Editable = Pedido != null && (Convert.ToString(Pedido["Estado"]) == "PENDIENTE" || Convert.ToString(Pedido["Estado"]) == "CONFIRMADO");
                    VentasBtnConsultar.Enabled = Pedido != null;
                    VentasBtnModificar.Enabled = Editable;
                    VentasBtnEliminar.Enabled = Editable;
                    return;
                }
                if (Origen == VentasBtnNuevo || Origen == VentasBtnConsultar || Origen == VentasBtnModificar)
                {
                    if (Origen != VentasBtnNuevo && Pedido == null) return;
                    using (var Formulario = new FrmPedidoVentas(Origen == VentasBtnNuevo ? 0 : Convert.ToInt32(Pedido["Id"]),
                        Origen == VentasBtnConsultar)) Formulario.ShowDialog(this);
                    VentasMetCargar(); return;
                }
                if (Origen != VentasBtnEliminar || Pedido == null) return;
                if (MessageBox.Show(this, "¿Cambiar el pedido " + Pedido["Numero"] + " a ANULADO?",
                    "Anular pedido", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                _Controlador.VentasMetEstado(Convert.ToInt32(Pedido["Id"]), "ANULADO");
                MessageBox.Show(this, "Pedido anulado correctamente.", "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                VentasMetCargar();
            }
            catch (Exception Error) { MessageBox.Show(this, Error.Message, "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
