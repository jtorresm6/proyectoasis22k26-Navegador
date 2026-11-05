/*
 * INICIO DEL CÓDIGO — Ventas y cuentas por cobrar
 * Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
 * Fecha de actualización: 15/10/2026
 * Propósito: Formulario de diseño para el prototipo no funcional de comisiones.
 */
using System.Windows.Forms;

namespace CapaVista_ComisionVendedores
{
    public partial class FrmComisionesVendedoresVentas : Form
    {
        public FrmComisionesVendedoresVentas()
        {
            InitializeComponent();
            VentasMetCargarFilasDeMuestra();
        }

        private void VentasMetCargarFilasDeMuestra()
        {
            VentasDgvVentas.Rows.Add("03/10/2026", "FAC-004581", "Distribuidora El Roble", "Q 12,500.00", "5.00%", "Q 625.00", "Elegible");
            VentasDgvVentas.Rows.Add("08/10/2026", "FAC-004612", "Abarrotería San Miguel", "Q 18,750.00", "5.00%", "Q 937.50", "Elegible");
            VentasDgvVentas.Rows.Add("14/10/2026", "FAC-004689", "Comercial La Esperanza", "Q 17,500.00", "5.00%", "Q 875.00", "Elegible");
            VentasDgvHistorial.Rows.Add("Octubre 2026", "Ana López", "12", "Q 2,437.50", "COM-2026-0108", "Pendiente");
            VentasDgvHistorial.Rows.Add("Octubre 2026", "Mario Castillo", "9", "Q 1,820.00", "COM-2026-0107", "Aprobada");
            VentasDgvHistorial.Rows.Add("Septiembre 2026", "Ana López", "15", "Q 2,190.00", "COM-2026-0092", "Enviada a nómina");
            VentasDgvHistorial.Rows.Add("Septiembre 2026", "Carlos Méndez", "7", "Q 980.00", "COM-2026-0091", "Rechazada");
            VentasDgvAutorizacion.Rows.Add("Octubre 2026", "Ana López", "12", "Q 2,437.50", "20/10/2026", "Pendiente");
            VentasDgvAutorizacion.Rows.Add("Octubre 2026", "Luis García", "8", "Q 1,480.00", "19/10/2026", "Pendiente");
        }
    }
}

// FIN DEL CÓDIGO — Equipo asignado al formulario | Fecha: 15/10/2026
