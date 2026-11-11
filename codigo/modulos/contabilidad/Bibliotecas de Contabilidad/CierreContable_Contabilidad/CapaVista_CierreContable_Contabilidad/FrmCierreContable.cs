/* Área: Contabilidad. Autor: Donald Osorio. Carné: 0901-23-17982.
 * Fecha: 14/10/2026. Propósito: revisión de cierre sin cambiar períodos. Responsable: Donald. */
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
    public partial class FrmCierreContable : Form
    {
        private readonly ClsControladorCierreContabilidad _Controlador = new ClsControladorCierreContabilidad();
        private DataTable _Polizas;
        private readonly ClsPaginacionContabilidad _Paginacion;

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public FrmCierreContable() : this(ClsModeloPermisosPantallaContabilidad.Total)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public FrmCierreContable(ClsModeloPermisosPantallaContabilidad Permisos)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            InitializeComponent();
            ContabilidadMetAplicarPermisos(Permisos);
            ClsPresentacionContabilidad.ContabilidadMetPrepararVentana(this, "Cierre contable", new Size(1180, 688));
            ContabilidadDgvPolizas.RowTemplate.Height = 28;
            ContabilidadDgvPolizas.ColumnHeadersHeight = 34;
            _Paginacion = new ClsPaginacionContabilidad(ContabilidadDgvPolizas);
            Disposed += (Remitente, Argumentos) => _Paginacion.Dispose();
            ContabilidadDtpPeriodo.Value = new DateTime(2026, 10, 1);
            ContabilidadDtpPeriodo.ValueChanged += (Remitente, Argumentos) => ContabilidadMetRevisar();
            ContabilidadBtnRevisar.Click += (Remitente, Argumentos) => ContabilidadMetRevisar();
            ContabilidadBtnSimular.Click += (Remitente, Argumentos) =>
            {
                if (ClsDialogosContabilidad.ContabilidadFuncConfirmar(this, "¿Desea continuar con el cierre del período seleccionado?", "Sí, continuar"))
                    Diseno.ContabilidadMetAviso(this, _Controlador.ContabilidadFuncValidarCierre(_Polizas), "Período validado para cierre.");
            };
            ContabilidadMetRevisar();
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        private void ContabilidadMetAplicarPermisos(ClsModeloPermisosPantallaContabilidad Permisos)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnSimular, Permisos.PuedeEditar);
        }

        private void ContabilidadMetRevisar()
        {
            DataTable Anterior = _Polizas;
            try
            {
                _Polizas = _Controlador.ContabilidadFuncPolizas(ContabilidadDtpPeriodo.Value);
            }
            catch (Exception Excepcion)
            {
                _Polizas = new DataTable();
                ClsDialogosContabilidad.ContabilidadMetError(this, "No se pudieron consultar las pólizas: " + Excepcion.Message);
            }
            _Paginacion.ContabilidadMetCargar(_Polizas);
            if (Anterior != null) Anterior.Dispose();
            string Error = _Controlador.ContabilidadFuncValidarCierre(_Polizas);
            ContabilidadLblValidacion.Text = string.IsNullOrEmpty(Error)
                ? "Revisión completada. El período está listo para el cierre."
                : "Cierre pendiente: " + Error;
            ContabilidadLblValidacion.ForeColor = string.IsNullOrEmpty(Error) ? Color.FromArgb(70, 104, 125) : Color.FromArgb(197, 143, 62);
            ContabilidadBtnSimular.Enabled = string.IsNullOrEmpty(Error);
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            ClsModeloResumenCierreContabilidad Resumen = _Controlador.ContabilidadFuncResumenCierre(_Polizas);
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            ContabilidadLblResumen.Text = "Aplicadas: " + Resumen.Aplicadas + "     Borradores: " + Resumen.Borradores
                + "     Debe aplicado: " + Diseno.ContabilidadFuncMoneda(Resumen.Debe) + "     Haber aplicado: " + Diseno.ContabilidadFuncMoneda(Resumen.Haber);
        }
    }
}
