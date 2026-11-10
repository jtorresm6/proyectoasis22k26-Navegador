/* Área: Contabilidad. Autor: Mario Taracena. Carné: 0901-23-9335.
 * Fecha: 14/10/2026. Propósito: consulta de presupuestos y su ejecución por módulo; la edición se hace en FrmPresupuestoEdicion. */
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
        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        private readonly ClsModeloPermisosPantallaContabilidad _Permisos;
        private readonly ClsPaginacionContabilidad _Tabla;
        private System.Collections.Generic.List<ClsModeloPresupuestoContabilidad> _Presupuestos = new System.Collections.Generic.List<ClsModeloPresupuestoContabilidad>();
        private ClsModeloPresupuestoContabilidad _Actual;
        private DataTable _Ejecucion;
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026

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
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
            _Permisos = Permisos;
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
            ContabilidadMetAplicarPermisos(Permisos);
            ClsPresentacionContabilidad.ContabilidadMetPrepararVentana(this, "Presupuestos", new Size(1180, 688));
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
            _Tabla = new ClsPaginacionContabilidad(ContabilidadDgvEjecucion);
            // Navegacion de la tabla
            _Tabla.ContabilidadMetAsignarNavegacion(ContabilidadBtnInicio, ContabilidadBtnAnterior, ContabilidadBtnSiguiente, ContabilidadBtnFin);
            Disposed += (Remitente, Argumentos) => { _Tabla.Dispose(); if (_Ejecucion != null) _Ejecucion.Dispose(); };
            ContabilidadCboPresupuesto.SelectionChangeCommitted += (Remitente, Argumentos) =>
                ContabilidadMetMostrar(ContabilidadCboPresupuesto.SelectedItem as ClsModeloPresupuestoContabilidad);
            ContabilidadBtnNueva.Click += (Remitente, Argumentos) => ContabilidadMetEditar(null);
            ContabilidadBtnModificar.Click += (Remitente, Argumentos) => { if (_Actual != null) ContabilidadMetEditar(_Actual); };
            ContabilidadBtnEliminar.Click += ContabilidadMetEliminar;
            ContabilidadMetCargarLista(0);
        }

        private void ContabilidadMetCargarLista(int Seleccionar)
        {
            try
            {
                _Presupuestos = _Controlador.ContabilidadFuncPresupuestos();
            }
            catch (Exception Excepcion)
            {
                _Presupuestos = new System.Collections.Generic.List<ClsModeloPresupuestoContabilidad>();
                ClsDialogosContabilidad.ContabilidadMetError(this, "No se pudieron consultar los presupuestos: " + Excepcion.Message);
            }
            ContabilidadCboPresupuesto.DisplayMember = "Nombre";
            ContabilidadCboPresupuesto.DataSource = null;
            ContabilidadCboPresupuesto.DataSource = _Presupuestos;
            ClsModeloPresupuestoContabilidad Elegido = _Presupuestos.Find(Presupuesto => Presupuesto.IdPresupuesto == Seleccionar)
                ?? (_Presupuestos.Count > 0 ? _Presupuestos[0] : null);
            ContabilidadCboPresupuesto.SelectedItem = Elegido;
            ContabilidadMetMostrar(Elegido);
        }

        private void ContabilidadMetMostrar(ClsModeloPresupuestoContabilidad Presupuesto)
        {
            _Actual = Presupuesto;
            DataTable Anterior = _Ejecucion;
            _Ejecucion = null;
            if (Presupuesto == null)
            {
                _Tabla.ContabilidadMetLimpiar();
                ContabilidadLblVigencia.Text = "No hay presupuestos guardados. Use Nuevo presupuesto para crear uno.";
                ContabilidadLblResumen.Text = string.Empty;
            }
            else
            {
                ContabilidadLblVigencia.Text = "Vigencia: del " + Presupuesto.FechaInicio.ToString("dd/MM/yyyy")
                    + (Presupuesto.FechaCierre.HasValue ? " al " + Presupuesto.FechaCierre.Value.ToString("dd/MM/yyyy") : " en adelante")
                    + "     Partidas: " + Presupuesto.Partidas.Count;
                try
                {
                    using (DataTable Detalle = _Controlador.ContabilidadFuncPresupuesto(Presupuesto))
                    {
                        _Ejecucion = _Controlador.ContabilidadFuncEjecucion(Presupuesto.FechaInicio, Presupuesto.FechaCierre, Detalle);
                        _Tabla.ContabilidadMetCargar(_Ejecucion);
                        ContabilidadMetResumen(_Controlador.ContabilidadFuncTotalPresupuesto(Detalle), _Controlador.ContabilidadFuncTotalEjecutado(_Ejecucion));
                    }
                }
                catch (Exception Excepcion)
                {
                    _Tabla.ContabilidadMetLimpiar();
                    ClsDialogosContabilidad.ContabilidadMetError(this, "No se pudo calcular lo ejecutado: " + Excepcion.Message);
                }
            }
            if (Anterior != null) Anterior.Dispose();
            ContabilidadBtnModificar.Enabled = Presupuesto != null;
            ContabilidadBtnEliminar.Enabled = Presupuesto != null;
        }

        private void ContabilidadMetResumen(decimal Asignado, decimal Ejecutado)
        {
            decimal Restante = Asignado - Ejecutado;
            ContabilidadLblResumen.Text = "Asignado: " + Diseno.ContabilidadFuncMoneda(Asignado)
                + "     Ejecutado: " + Diseno.ContabilidadFuncMoneda(Ejecutado) + " (" + (Asignado == 0m ? 0m : Ejecutado / Asignado).ToString("P1") + ")"
                + "     Restante: " + Diseno.ContabilidadFuncMoneda(Restante) + (Restante < 0m ? " · Presupuesto excedido" : string.Empty);
            ContabilidadLblResumen.ForeColor = Restante < 0m ? Color.FromArgb(197, 143, 62) : Color.FromArgb(35, 50, 61);
        }

        private void ContabilidadMetEditar(ClsModeloPresupuestoContabilidad Presupuesto)
        {
            using (FrmPresupuestoEdicion Edicion = new FrmPresupuestoEdicion(_Permisos, Presupuesto))
                if (Edicion.ShowDialog(this) == DialogResult.OK) ContabilidadMetCargarLista(Edicion.IdPresupuesto);
        }

        private void ContabilidadMetEliminar(object Remitente, EventArgs Argumentos)
        {
            if (_Actual == null) return;
            if (!ClsDialogosContabilidad.ContabilidadFuncConfirmar(this,
                    "¿Desea eliminar el presupuesto " + _Actual.Nombre + "? Se dará de baja con todas sus partidas.", "Sí, eliminar"))
                return;
            string Mensaje;
            int IdPresupuesto = _Actual.IdPresupuesto;
            bool Eliminado;
            using (DataTable Detalle = _Controlador.ContabilidadFuncPresupuesto(_Actual))
                Eliminado = _Controlador.ContabilidadFuncGrabarCambios(EstadoEntidadContabilidad.Borrar, ref IdPresupuesto,
                    _Actual.Nombre, _Actual.FechaInicio, null, 0m, Detalle, out Mensaje);
            Diseno.ContabilidadMetAviso(this, Eliminado ? string.Empty : Mensaje, Mensaje);
            if (Eliminado) ContabilidadMetCargarLista(0);
        }
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        private void ContabilidadMetAplicarPermisos(ClsModeloPermisosPantallaContabilidad Permisos)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnNueva, Permisos.PuedeInsertar);
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnModificar, Permisos.PuedeEditar);
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnEliminar, Permisos.PuedeEliminar);
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        }
    }
}
