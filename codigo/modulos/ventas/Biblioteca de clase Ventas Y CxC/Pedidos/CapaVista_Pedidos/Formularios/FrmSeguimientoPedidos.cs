using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using CapaControlador_Pedidos;

namespace CapaVista_Pedidos
{
    public partial class FrmSeguimientoPedidos : Form
    {
        private readonly ClsPedidoControlador _Controlador = new ClsPedidoControlador();
        private DataTable _Pedidos = new DataTable();

        public FrmSeguimientoPedidos()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                Load += (Origen, Evento) => VentasMetCargar();
        }
        private void VentasMetCargar()
        {
            try { _Pedidos = _Controlador.VentasFuncListar(); VentasMetMostrar(); }
            catch (Exception Error) { MessageBox.Show(this, Error.Message, "Pedidos"); }
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
                    VentasBtnModificar.Enabled = VentasBtnAnular.Enabled = Editable;
                    VentasBtnConfirmar.Enabled = Editable && Convert.ToString(Pedido["Estado"]) == "PENDIENTE";
                    return;
                }
                if (Origen == VentasBtnNuevo || Origen == VentasBtnConsultar || Origen == VentasBtnModificar)
                {
                    if (Origen != VentasBtnNuevo && Pedido == null) return;
                    using (var Formulario = new FrmPedidoVentas(Origen == VentasBtnNuevo ? 0 : Convert.ToInt32(Pedido["Id"]),
                        Origen == VentasBtnConsultar)) Formulario.ShowDialog(this);
                    VentasMetCargar(); return;
                }
                if (Pedido == null) return;
                string EstadoNuevo = Origen == VentasBtnConfirmar ? "CONFIRMADO" : "ANULADO";
                if (MessageBox.Show(this, "¿Cambiar el pedido " + Pedido["Numero"] + " a " + EstadoNuevo + "?",
                    "Pedidos", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                _Controlador.VentasMetEstado(Convert.ToInt32(Pedido["Id"]), EstadoNuevo);
                VentasMetCargar();
            }
            catch (Exception Error) { MessageBox.Show(this, Error.Message, "Pedidos"); }
        }
    }
}
