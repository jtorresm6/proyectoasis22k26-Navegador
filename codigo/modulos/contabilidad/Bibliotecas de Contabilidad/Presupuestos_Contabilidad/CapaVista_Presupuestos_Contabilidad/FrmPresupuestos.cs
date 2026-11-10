/* Área: Contabilidad. Autor: Mario Taracena. Carné: 0901-23-9335.
 * Fecha: 14/10/2026. Propósito: presupuesto de ejemplo sin persistencia. Responsable: Mario. */
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
    public partial class FrmPresupuestos : Form
    {
        private readonly ClsControladorPresupuestosContabilidad _Controlador = new ClsControladorPresupuestosContabilidad();
        private DataTable _Detalle;
        private readonly ClsValidacionContabilidad _Validacion = new ClsValidacionContabilidad();
        private readonly ClsPaginacionContabilidad _Paginacion;
        private bool _CargandoDatos;
        private bool _HayCambios;

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public FrmPresupuestos() : this(ClsModeloPermisosPantallaContabilidad.Total)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public FrmPresupuestos(ClsModeloPermisosPantallaContabilidad Permisos)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            InitializeComponent();
            ContabilidadMetAplicarPermisos(Permisos);
            ClsPresentacionContabilidad.ContabilidadMetPrepararVentana(this, "Presupuestos", new Size(1180, 688));
            _Paginacion = new ClsPaginacionContabilidad(ContabilidadDgvDetalle);
            foreach (Control Campo in new Control[] { ContabilidadDtpInicio, ContabilidadTxtNombre, ContabilidadCboModulo, ContabilidadNudCantidad })
                _Validacion.ContabilidadMetObligatorio(Campo);
            Disposed += (Remitente, Argumentos) => { _Paginacion.Dispose(); _Validacion.Dispose(); };
            ContabilidadCboModulo.DataSource = _Controlador.ContabilidadFuncModulos();
            ContabilidadNudGastado.ValueChanged += ContabilidadMetCambiarEncabezado;
            ContabilidadDtpInicio.ValueChanged += ContabilidadMetCambiarEncabezado;
            ContabilidadDtpCierre.ValueChanged += ContabilidadMetCambiarEncabezado;
            ContabilidadBtnAgregar.Click += ContabilidadMetAgregar;
            ContabilidadBtnQuitar.Click += (Remitente, Argumentos) =>
            {
                if (ContabilidadDgvDetalle.CurrentRow == null) return;
                DataRow Fila = _Paginacion.ContabilidadFuncFilaActual();
                if (Fila != null) _Detalle.Rows.Remove(Fila);
                _HayCambios = true;
                ContabilidadMetActualizar();
            };
            ContabilidadBtnNueva.Click += (Remitente, Argumentos) => ContabilidadMetCargar(false);
            ContabilidadBtnEjemplo.Click += (Remitente, Argumentos) => ContabilidadMetCargar(true);
            ContabilidadBtnGuardar.Click += (Remitente, Argumentos) =>
            {
                DateTime? Cierre = ContabilidadDtpCierre.Checked ? (DateTime?)ContabilidadDtpCierre.Value : null;
                string ErrorFecha = ContabilidadDtpCierre.Checked ? _Controlador.ContabilidadFuncErrorFechas(ContabilidadDtpInicio.Value, ContabilidadDtpCierre.Value) : string.Empty;
                if (!_Validacion.ContabilidadFuncValidar(ContabilidadDtpCierre, ErrorFecha)) return;
                string Mensaje;
                bool Grabado = _Controlador.ContabilidadFuncGrabarCambios(EstadoEntidadContabilidad.Agregar, ContabilidadDtpInicio.Value,
                    Cierre, ContabilidadNudGastado.Value, _Detalle, out Mensaje);
                Diseno.ContabilidadMetAviso(this, Grabado ? string.Empty : Mensaje, Mensaje);
            };
            ContabilidadMetCargar(true);
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        private void ContabilidadMetAplicarPermisos(ClsModeloPermisosPantallaContabilidad Permisos)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnNueva, Permisos.PuedeInsertar);
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnEjemplo, Permisos.PuedeInsertar);
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnAgregar, Permisos.PuedeInsertar);
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnGuardar, Permisos.PuedeInsertar);
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnQuitar, Permisos.PuedeEliminar);
        }

        private void ContabilidadMetCargar(bool Ejemplo)
        {
            _CargandoDatos = true;
            _Validacion.ContabilidadMetLimpiar();
            DataTable Anterior = _Detalle;
            _Detalle = _Controlador.ContabilidadFuncPresupuesto();
            if (!Ejemplo) _Detalle.Clear();
            _Paginacion.ContabilidadMetCargar(_Detalle);
            if (Anterior != null) Anterior.Dispose();
            ContabilidadDtpInicio.Value = new DateTime(2026, 10, 1);
            ContabilidadDtpCierre.Value = new DateTime(2026, 10, 31);
            ContabilidadDtpCierre.Checked = Ejemplo;
            ContabilidadNudGastado.Value = Ejemplo ? 21500m : 0m;
            ContabilidadTxtNombre.Clear();
            ContabilidadNudCantidad.Value = 0m;
            _CargandoDatos = false;
            _HayCambios = false;
            ContabilidadMetActualizar();
        }

        private void ContabilidadMetCambiarEncabezado(object Remitente, EventArgs Argumentos)
        {
            if (!_CargandoDatos) _HayCambios = true;
            ContabilidadMetActualizar();
        }

        private void ContabilidadMetAgregar(object Remitente, EventArgs Argumentos)
        {
            var Errores = _Controlador.ContabilidadFuncErroresPartida(ContabilidadTxtNombre.Text, ContabilidadCboModulo.Text, ContabilidadNudCantidad.Value, _Detalle);
            _Validacion.ContabilidadFuncValidar(ContabilidadTxtNombre, Errores.ContainsKey("Nombre") ? Errores["Nombre"] : null);
            _Validacion.ContabilidadFuncValidar(ContabilidadCboModulo, Errores.ContainsKey("Modulo") ? Errores["Modulo"] : null);
            _Validacion.ContabilidadFuncValidar(ContabilidadNudCantidad, Errores.ContainsKey("Cantidad") ? Errores["Cantidad"] : null);
            if (Errores.Count > 0) return;
            _Detalle.Rows.Add(ContabilidadTxtNombre.Text.Trim(), ContabilidadCboModulo.Text, ContabilidadNudCantidad.Value);
            _HayCambios = true;
            ContabilidadTxtNombre.Clear();
            ContabilidadNudCantidad.Value = 0m;
            ContabilidadMetActualizar();
        }

        private void ContabilidadMetActualizar()
        {
            if (_Detalle == null) return;
            decimal Total = _Controlador.ContabilidadFuncTotalPresupuesto(_Detalle);
            decimal Restante = _Controlador.ContabilidadFuncRestante(_Detalle, ContabilidadNudGastado.Value);
            ContabilidadLblResumen.Text = "Total: " + Diseno.ContabilidadFuncMoneda(Total)
                + "     Gastado: " + Diseno.ContabilidadFuncMoneda(ContabilidadNudGastado.Value)
                + "     Restante: " + Diseno.ContabilidadFuncMoneda(Restante) + (Restante < 0m ? " · Presupuesto excedido" : "");
            ContabilidadLblResumen.ForeColor = Restante < 0m ? Color.FromArgb(197, 143, 62) : Color.FromArgb(35, 50, 61);
            ContabilidadBtnQuitar.Enabled = _Detalle.Rows.Count > 0;
            ContabilidadBtnGuardar.Enabled = _HayCambios && _Detalle.Rows.Count > 0;
            _Paginacion.ContabilidadMetActualizar();
        }
    }
}
