/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Mantenimiento del catálogo de países (tblPais).
 *  Usa el control Navegador: la grilla, los botones y los permisos los maneja el
 *  componente con los IDs de módulo y aplicación registrados en Seguridad.
 * ===================================================================
*/

using System.Drawing;
using System.Windows.Forms;
using CapaControlador_Contabilidad;

namespace CapaVista_Contabilidad
{
    public partial class FrmMantenimientoPais : Form
    {
        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public FrmMantenimientoPais() : this(ClsModeloPermisosPantallaContabilidad.Total)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public FrmMantenimientoPais(ClsModeloPermisosPantallaContabilidad Permisos)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            InitializeComponent();
            ClsPresentacionContabilidad.ContabilidadMetPrepararVentana(this, "Mantenimiento de países", new Size(1180, 688));
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            ContabilidadNavPais.NavegadorMetConfigurar("tblpais", Permisos.IdModulo, Permisos.IdAplicacion);
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 27/10/2026
            ClsPresentacionContabilidad.ContabilidadMetAjustarCamposNavegador(ContabilidadNavPais);
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 27/10/2026
        }
    }
}
