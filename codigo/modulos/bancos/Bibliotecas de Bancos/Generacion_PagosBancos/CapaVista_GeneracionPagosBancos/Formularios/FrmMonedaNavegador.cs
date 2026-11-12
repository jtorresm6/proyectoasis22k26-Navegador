/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 23/10/2026
 * ================================================================
 * Propósito :
 *  Formulario navegador del módulo Moneda. Inicializa el control
 *  de navegación apuntando a la tabla tblMoneda, dentro del módulo
 *  Bancos (8000), aplicación 8001, y le entrega los datos del
 *  reporte de monedas para el botón Imprimir.
 * ================================================================
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CapaControlador_GeneracionPagos;

namespace CapaVista_GeneracionPagos.Formularios
{
    public partial class FrmMonedaNavegador : Form
    {
        private readonly ClsControladorGeneracionPagos _Controlador = new ClsControladorGeneracionPagos();
        private readonly DataTable _Monedas = new DataTable();

        public FrmMonedaNavegador()
        {
            InitializeComponent();
            BancosMetCargarMonedas();

            Dictionary<string, object> Fuentes = new Dictionary<string, object>();
            Fuentes.Add("DsMoneda", _Monedas);

            BancosNavMoneda.NavegadorAccionSolicitada += BancosNavMoneda_AccionSolicitada;
            BancosNavMoneda.NavegadorMetConfigurar("tblMoneda", 8000, 8001, Fuentes);
        }

        private void BancosNavMoneda_AccionSolicitada(string Accion)
        {
            if (Accion == "IMPRIMIR")
            {
                BancosMetCargarMonedas();
            }
        }

        private void BancosMetCargarMonedas()
        {
            string Error = _Controlador.BancosMetConsultarReporteMonedas(out DataTable Resultado);
            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "No fue posible cargar el reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _Monedas.Clear();
            _Monedas.Merge(Resultado);
        }
    }
}
