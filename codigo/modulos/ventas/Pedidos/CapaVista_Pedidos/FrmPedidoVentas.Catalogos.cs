using System;
using System.Linq;
using System.Windows.Forms;
using CapaControlador_Pedidos;

namespace CapaVista_Pedidos
{
    public partial class FrmPedidoVentas
    {
        private void VentasMetCargarCatalogos()
        {
            try
            {
                _Cargando = true;
                VentasMetOpciones(VentasCboCliente, _Controlador.VentasFuncClientes(), -1);
                VentasMetOpciones(VentasCboVendedor, _Controlador.VentasFuncVendedores(), -1);
                var Cotizaciones = _Controlador.VentasFuncCotizaciones();
                Cotizaciones.Insert(0, new ClsCotizacionPedido { Id = 0, Nombre = "Seleccionar cotización" });
                VentasMetOpciones(VentasCboCotizacionOrigen, Cotizaciones, 0);
                VentasMetCargarProductos();
                _MonedaId = _MonedaPublica = _Controlador.VentasFuncMonedaPublica();
                _CatalogosDisponibles = true;
            }
            catch (Exception Error)
            {
                _CatalogosDisponibles = false;
                MessageBox.Show(this, "No se pudieron cargar los datos de pedidos: " + Error.Message,
                    "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally { _Cargando = false; }
            VentasMetActualizarAcciones();
            if (_CatalogosDisponibles && !string.IsNullOrWhiteSpace(_NumeroCotizacionPendiente))
            {
                string Numero = _NumeroCotizacionPendiente;
                _NumeroCotizacionPendiente = null;
                try { VentasMetCargarCotizacion(Numero); }
                catch (Exception Error) { MessageBox.Show(this, Error.Message, "Cotización", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
        }

        private static void VentasMetOpciones(ComboBox Combo, object Datos, int Indice)
        {
            Combo.DisplayMember = "Nombre";
            Combo.DataSource = Datos;
            Combo.SelectedIndex = Indice;
        }

        private void VentasMetCargarProductos()
        {
            try { VentasCboProductoPedido.DataSource = _Controlador.VentasFuncProductos(VentasDtpPedido.Value.Date); }
            catch (Exception Error)
            {
                VentasCboProductoPedido.DataSource = null;
                MessageBox.Show(this, "No se pudieron cargar los productos: " + Error.Message,
                    "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            VentasMetActualizarAcciones();
        }

        public void VentasMetCargarCotizacion(string NumeroCotizacion)
        {
            if (string.IsNullOrWhiteSpace(NumeroCotizacion))
                throw new InvalidOperationException("Indique el número de cotización que desea cargar.");
            if (!_CatalogosDisponibles)
            {
                _NumeroCotizacionPendiente = NumeroCotizacion.Trim();
                return;
            }
            var Opcion = VentasCboCotizacionOrigen.Items.Cast<ClsCotizacionPedido>()
                .FirstOrDefault(Item => Item.Numero == NumeroCotizacion.Trim());
            if (Opcion == null) throw new InvalidOperationException("La cotización no está disponible en pedidos.");
            VentasMetCambiarOrigen(true);
            VentasCboCotizacionOrigen.SelectedItem = Opcion;
        }
    }
}
