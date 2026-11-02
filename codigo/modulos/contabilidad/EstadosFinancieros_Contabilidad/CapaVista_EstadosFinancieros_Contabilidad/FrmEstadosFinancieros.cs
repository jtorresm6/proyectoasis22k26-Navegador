/* Área: Contabilidad. Autor: Gabriel André Guillén Pocón. Carné: 0901-23-1998.
 * Fecha: 14/10/2026. Propósito: informe de ejemplo de solo lectura sin conexión a base. */
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using Diseno = CapaVista_Contabilidad.Componentes.ClsDisenoContabilidad;
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using CapaControlador_Contabilidad;
// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using CapaVista_Contabilidad.Componentes;
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaVista_Contabilidad
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public partial class FrmEstadosFinancieros : Form
    {
        private readonly ClsControladorEstadosFinancierosContabilidad _Controlador = new ClsControladorEstadosFinancierosContabilidad();
        private DataTable _Resultado;
        private readonly ClsPaginacionContabilidad _Paginacion;
        private readonly ClsValidacionContabilidad _Validacion = new ClsValidacionContabilidad();

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public FrmEstadosFinancieros() : this(ClsModeloPermisosPantallaContabilidad.Total)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public FrmEstadosFinancieros(ClsModeloPermisosPantallaContabilidad Permisos)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            InitializeComponent();
            ContabilidadMetAplicarPermisos(Permisos);
            ClsPresentacionContabilidad.ContabilidadMetPrepararVentana(this, "Estados financieros", new Size(1180, 688));
            _Paginacion = new ClsPaginacionContabilidad(ContabilidadDgvResultado);
            _Validacion.ContabilidadMetObligatorio(ContabilidadDtpDesde);
            _Validacion.ContabilidadMetObligatorio(ContabilidadDtpHasta);
            Disposed += (Remitente, Argumentos) => { _Paginacion.Dispose(); _Validacion.Dispose(); };
            ContabilidadDtpDesde.Value = new DateTime(2026, 10, 1);
            ContabilidadDtpHasta.Value = new DateTime(2026, 10, 31);
            ContabilidadBtnConsultar.Click += (Remitente, Argumentos) => ContabilidadMetConsultar();
            ContabilidadDtpDesde.ValueChanged += (Remitente, Argumentos) => ContabilidadMetInvalidar();
            ContabilidadDtpHasta.ValueChanged += (Remitente, Argumentos) => ContabilidadMetInvalidar();
            ContabilidadMetConsultar();
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        private void ContabilidadMetAplicarPermisos(ClsModeloPermisosPantallaContabilidad Permisos)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
        }

        private void ContabilidadMetInvalidar()
        {
            _Paginacion.ContabilidadMetLimpiar();
            if (_Resultado != null) { _Resultado.Dispose(); _Resultado = null; }
            ContabilidadLblPeriodo.Text = "Filtros modificados · Pulse Ver resultados para actualizar el informe.";
            ContabilidadLblEstado.Text = string.Empty;
            ContabilidadLblResumen.Text = string.Empty;
        }

        private void ContabilidadMetConsultar()
        {
            string ErrorFechas = _Controlador.ContabilidadFuncErrorFechas(ContabilidadDtpDesde.Value, ContabilidadDtpHasta.Value);
            if (!_Validacion.ContabilidadFuncValidar(ContabilidadDtpHasta, ErrorFechas))
            {
                ContabilidadMetInvalidar();
                ContabilidadLblEstado.Text = ErrorFechas;
                return;
            }
            DataTable Anterior = _Resultado;
            try
            {
                _Resultado = _Controlador.ContabilidadFuncResultados(ContabilidadDtpDesde.Value, ContabilidadDtpHasta.Value);
            }
            catch (Exception Excepcion)
            {
                _Resultado = null;
                ContabilidadMetInvalidar();
                ClsDialogosContabilidad.ContabilidadMetError(this, "No se pudo consultar el estado de resultados: " + Excepcion.Message);
                return;
            }
            _Paginacion.ContabilidadMetCargar(_Resultado);
            if (Anterior != null) Anterior.Dispose();
            ContabilidadLblPeriodo.Text = "Estado de resultados del " + ContabilidadDtpDesde.Value.ToString("dd/MM/yyyy")
                + " al " + ContabilidadDtpHasta.Value.ToString("dd/MM/yyyy");
            decimal Ingresos = _Controlador.ContabilidadFuncTotalRubro(_Resultado, "INGRESOS");
            decimal Costos = _Controlador.ContabilidadFuncTotalRubro(_Resultado, "COSTOS");
            decimal Gastos = _Controlador.ContabilidadFuncTotalRubro(_Resultado, "GASTOS");
            ContabilidadLblEstado.Text = _Resultado.Rows.Count == 0 ? "Sin movimientos en el rango seleccionado."
                : "Ingresos: Haber − Debe. Costos y gastos: Debe − Haber.\r\nSe excluyen borradores, anuladas y cuentas de balance.";
            ContabilidadLblResumen.Text = "Ingresos: " + Diseno.ContabilidadFuncMoneda(Ingresos) + "     Costos: " + Diseno.ContabilidadFuncMoneda(Costos)
                + "     Gastos: " + Diseno.ContabilidadFuncMoneda(Gastos) + "     Resultado: " + Diseno.ContabilidadFuncMoneda(_Controlador.ContabilidadFuncResultadoNeto(_Resultado));
        }
    }
}
