/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Mario Taracena
 * Carné : 0901-23-9335
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  Ventana para crear o modificar un presupuesto: nombre, vigencia y
 *  partidas asignadas por módulo.
 * Reglas especificas:
 *  Lo ejecutado no se captura: al guardar se toma de las pólizas
 *  aplicadas de cada módulo dentro de la vigencia. Usa los permisos de
 *  la pantalla de presupuestos (10002).
 * ===================================================================
*/

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaControlador_Contabilidad;
// Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
using CapaControlador_Navegador;
// Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
using CapaVista_Contabilidad.Componentes;
using Diseno = CapaVista_Contabilidad.Componentes.ClsDisenoContabilidad;

namespace CapaVista_Contabilidad
{
    public partial class FrmPresupuestoEdicion : Form
    {
        private readonly ClsControladorPresupuestosContabilidad _Controlador = new ClsControladorPresupuestosContabilidad();
        private readonly ClsValidacionContabilidad _Validacion = new ClsValidacionContabilidad();
        private readonly ClsPaginacionContabilidad _Tabla;
        private DataTable _Detalle;
        private int _IdPresupuesto;
        private bool _CargandoDatos;
        private bool _HayCambios;

        public int IdPresupuesto { get { return _IdPresupuesto; } }

        public FrmPresupuestoEdicion(ClsModeloPermisosPantallaContabilidad Permisos, ClsModeloPresupuestoContabilidad Presupuesto)
        {
            InitializeComponent();
            ClsPresentacionContabilidad.ContabilidadMetPrepararVentana(this, "Presupuesto", new Size(1000, 690));
            // Posicion estandar de la ventana
            StartPosition = FormStartPosition.CenterScreen;
            _Tabla = new ClsPaginacionContabilidad(ContabilidadDgvDetalle);
            // Navegacion de la tabla
            _Tabla.ContabilidadMetAsignarNavegacion(ContabilidadBtnInicio, ContabilidadBtnAnterior, ContabilidadBtnSiguiente, ContabilidadBtnFin);
            foreach (Control Campo in new Control[] { ContabilidadTxtNombrePresupuesto, ContabilidadDtpInicio, ContabilidadTxtNombre, ContabilidadCboModulo, ContabilidadNudCantidad })
                _Validacion.ContabilidadMetObligatorio(Campo);
            Disposed += (Remitente, Argumentos) => { _Tabla.Dispose(); _Validacion.Dispose(); };
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnAgregar, Permisos.PuedeInsertar || Permisos.PuedeEditar);
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnQuitar, Permisos.PuedeInsertar || Permisos.PuedeEditar);
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnGuardar, Permisos.PuedeInsertar || Permisos.PuedeEditar);
            try
            {
                ContabilidadCboModulo.DataSource = _Controlador.ContabilidadFuncModulos();
            }
            catch (Exception Excepcion)
            {
                ClsDialogosContabilidad.ContabilidadMetError(this, "No se pudieron cargar los módulos: " + Excepcion.Message);
            }
            ContabilidadTxtNombrePresupuesto.TextChanged += ContabilidadMetCambiar;
            ContabilidadDtpInicio.ValueChanged += ContabilidadMetCambiar;
            ContabilidadDtpCierre.ValueChanged += ContabilidadMetCambiar;
            ContabilidadBtnAgregar.Click += ContabilidadMetAgregar;
            ContabilidadBtnQuitar.Click += ContabilidadMetQuitar;
            ContabilidadBtnGuardar.Click += ContabilidadMetGuardar;
            ContabilidadBtnCancelar.Click += (Remitente, Argumentos) => Close();
            FormClosing += ContabilidadMetCerrando;
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            // Ayuda con el botón o con F1
            ContabilidadBtnAyuda.Click += ContabilidadMetMostrarAyuda;
            KeyPreview = true;
            KeyDown += (Remitente, Tecla) =>
            {
                if (Tecla.KeyCode != Keys.F1) return;
                Tecla.Handled = true;
                ContabilidadMetMostrarAyuda(this, EventArgs.Empty);
            };
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
            ContabilidadMetCargar(Presupuesto);
        }

        private void ContabilidadMetCargar(ClsModeloPresupuestoContabilidad Presupuesto)
        {
            _CargandoDatos = true;
            _IdPresupuesto = Presupuesto == null ? 0 : Presupuesto.IdPresupuesto;
            ContabilidadLblTitulo.Text = Presupuesto == null ? "Nuevo presupuesto" : "Modificar presupuesto";
            _Detalle = _Controlador.ContabilidadFuncPresupuesto(Presupuesto);
            _Tabla.ContabilidadMetCargar(_Detalle);
            DateTime Hoy = DateTime.Today;
            ContabilidadTxtNombrePresupuesto.Text = Presupuesto == null ? string.Empty : Presupuesto.Nombre;
            ContabilidadDtpInicio.Value = Presupuesto == null ? new DateTime(Hoy.Year, 1, 1) : Presupuesto.FechaInicio;
            ContabilidadDtpCierre.Value = Presupuesto == null || !Presupuesto.FechaCierre.HasValue
                ? new DateTime(Hoy.Year, 12, 31) : Presupuesto.FechaCierre.Value;
            ContabilidadDtpCierre.Checked = Presupuesto == null || Presupuesto.FechaCierre.HasValue;
            _CargandoDatos = false;
            _HayCambios = false;
            ContabilidadMetActualizar();
        }

        private void ContabilidadMetCambiar(object Remitente, EventArgs Argumentos)
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
            ContabilidadTxtNombre.Focus();
            ContabilidadMetActualizar();
        }

        private void ContabilidadMetQuitar(object Remitente, EventArgs Argumentos)
        {
            DataRow Fila = _Tabla.ContabilidadFuncFilaActual();
            if (Fila == null) return;
            _Detalle.Rows.Remove(Fila);
            _HayCambios = true;
            ContabilidadMetActualizar();
        }

        private void ContabilidadMetGuardar(object Remitente, EventArgs Argumentos)
        {
            DateTime? Cierre = ContabilidadDtpCierre.Checked ? (DateTime?)ContabilidadDtpCierre.Value : null;
            string ErrorFecha = ContabilidadDtpCierre.Checked ? _Controlador.ContabilidadFuncErrorFechas(ContabilidadDtpInicio.Value, ContabilidadDtpCierre.Value) : string.Empty;
            if (!_Validacion.ContabilidadFuncValidar(ContabilidadTxtNombrePresupuesto,
                string.IsNullOrWhiteSpace(ContabilidadTxtNombrePresupuesto.Text) ? "Escriba el nombre del presupuesto." : null)) return;
            if (!_Validacion.ContabilidadFuncValidar(ContabilidadDtpCierre, ErrorFecha)) return;
            decimal Ejecutado;
            try
            {
                using (DataTable Ejecucion = _Controlador.ContabilidadFuncEjecucion(ContabilidadDtpInicio.Value, Cierre, _Detalle))
                    Ejecutado = Math.Max(0m, _Controlador.ContabilidadFuncTotalEjecutado(Ejecucion));
            }
            catch (Exception Excepcion)
            {
                ClsDialogosContabilidad.ContabilidadMetError(this, "No se pudo calcular lo ejecutado: " + Excepcion.Message);
                return;
            }
            string Mensaje;
            int Id = _IdPresupuesto;
            bool Grabado = _Controlador.ContabilidadFuncGrabarCambios(
                _IdPresupuesto == 0 ? EstadoEntidadContabilidad.Agregar : EstadoEntidadContabilidad.Modificar, ref Id,
                ContabilidadTxtNombrePresupuesto.Text, ContabilidadDtpInicio.Value, Cierre, Ejecutado, _Detalle, out Mensaje);
            Diseno.ContabilidadMetAviso(this, Grabado ? string.Empty : Mensaje, Mensaje);
            if (!Grabado) return;
            _IdPresupuesto = Id;
            _HayCambios = false;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ContabilidadMetCerrando(object Remitente, FormClosingEventArgs Argumentos)
        {
            if (DialogResult == DialogResult.OK || !_HayCambios) return;
            if (!ClsDialogosContabilidad.ContabilidadFuncConfirmar(this, "Hay cambios sin guardar. ¿Desea descartarlos?", "Sí, descartar"))
                Argumentos.Cancel = true;
        }

        private void ContabilidadMetActualizar()
        {
            if (_Detalle == null) return;
            ContabilidadLblResumen.Text = "Partidas: " + _Detalle.Rows.Count + "     Total asignado: "
                + Diseno.ContabilidadFuncMoneda(_Controlador.ContabilidadFuncTotalPresupuesto(_Detalle));
            ContabilidadBtnQuitar.Enabled = _Detalle.Rows.Count > 0;
            ContabilidadBtnGuardar.Enabled = _HayCambios && _Detalle.Rows.Count > 0;
            _Tabla.ContabilidadMetActualizar();
        }

        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
        // Abre la página de ayuda de esta ventana
        private void ContabilidadMetMostrarAyuda(object Remitente, EventArgs Argumentos)
        {
            try
            {
                ClsDestinoAyuda Destino = new ClsCtrlAyuda().NavegadorFuncObtenerDestino(10002, GetType().Name);

                if (Destino.RutaArchivo == "")
                {
                    MessageBox.Show(this, Destino.Mensaje, "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Help.ShowHelp(this, Destino.RutaArchivo, HelpNavigator.Topic, "PresupuestoEdicion_Contabilidad.html");
                }
            }
            catch (Exception Excepcion)
            {
                MessageBox.Show(this, "No se pudo abrir la ayuda de Contabilidad.\n\n" + Excepcion.Message,
                    "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 11/11/2026
    }
}
