/* Área: Contabilidad. Autor: Dylan Hernández. Carné: 0901-23-519.
 * Fecha: 14/10/2026. Propósito: registrar, consultar, modificar y dar de baja activos fijos. Responsable: Dylan. */
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
    public partial class FrmActivosFijos : Form
    {
        private readonly ClsControladorActivosFijosContabilidad _Controlador = new ClsControladorActivosFijosContabilidad();
        private DataTable _Catalogo;
        // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        private DataTable _Activos;
        private int _IdActivo;
        // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        private readonly ClsValidacionContabilidad _Validacion = new ClsValidacionContabilidad();
        private readonly ClsPaginacionContabilidad _Paginacion;
        private bool _CargandoDatos;

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public FrmActivosFijos() : this(ClsModeloPermisosPantallaContabilidad.Total)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public FrmActivosFijos(ClsModeloPermisosPantallaContabilidad Permisos)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            InitializeComponent();
            ContabilidadMetAplicarPermisos(Permisos);
            ClsPresentacionContabilidad.ContabilidadMetPrepararVentana(this, "Activos fijos", new Size(1180, 688));
            _Paginacion = new ClsPaginacionContabilidad(ContabilidadDgvCatalogo);
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            foreach (Control Campo in new Control[] { ContabilidadTxtDescripcion, ContabilidadCboModulo,
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                ContabilidadCboTipo, ContabilidadDtpAdquisicion, ContabilidadDtpInicio, ContabilidadNudCompra, ContabilidadNudMeses })
                _Validacion.ContabilidadMetObligatorio(Campo);
            Disposed += (Remitente, Argumentos) => { _Validacion.Dispose(); _Paginacion.Dispose(); };
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            try
            {
                ContabilidadCboModulo.DataSource = _Controlador.ContabilidadFuncModulos();
                _Catalogo = _Controlador.ContabilidadFuncVidaUtil();
            }
            catch (Exception Excepcion)
            {
                _Catalogo = new DataTable();
                ClsDialogosContabilidad.ContabilidadMetError(this, "No se pudieron cargar los catálogos: " + Excepcion.Message);
            }
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            ContabilidadCboTipo.DisplayMember = "Nombre";
            ContabilidadCboTipo.ValueMember = "Tipo";
            ContabilidadCboTipo.DataSource = _Catalogo;
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            ContabilidadDgvCatalogo.CellClick += (Remitente, Argumentos) =>
            {
                if (Argumentos.RowIndex < 0) return;
                DataRow Fila = _Paginacion.ContabilidadFuncFilaActual();
                if (Fila != null) ContabilidadMetCargar(Fila);
            };
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            ContabilidadCboTipo.SelectedIndexChanged += (Remitente, Argumentos) => ContabilidadMetTipo();
            ContabilidadNudCompra.ValueChanged += (Remitente, Argumentos) => ContabilidadMetActualizar();
            ContabilidadNudResidual.ValueChanged += (Remitente, Argumentos) => ContabilidadMetActualizar();
            ContabilidadNudMeses.ValueChanged += (Remitente, Argumentos) => ContabilidadMetActualizar();
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            ContabilidadBtnNueva.Click += (Remitente, Argumentos) => ContabilidadMetCargar(null);
            ContabilidadBtnEliminar.Click += ContabilidadMetEliminar;
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            ContabilidadBtnGuardar.Click += ContabilidadMetValidar;
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            foreach (Control Campo in new Control[] { ContabilidadTxtDescripcion, ContabilidadCboModulo, ContabilidadCboTipo,
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                ContabilidadDtpAdquisicion, ContabilidadDtpInicio, ContabilidadNudCompra, ContabilidadNudResidual, ContabilidadNudMeses })
            {
                if (Campo is DateTimePicker) ((DateTimePicker)Campo).ValueChanged += ContabilidadMetCambiarFicha;
                else if (Campo is NumericUpDown) ((NumericUpDown)Campo).ValueChanged += ContabilidadMetCambiarFicha;
                else if (Campo is ComboBox) ((ComboBox)Campo).SelectedIndexChanged += ContabilidadMetCambiarFicha;
                else Campo.TextChanged += ContabilidadMetCambiarFicha;
            }
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            ContabilidadMetCargarLista();
            ContabilidadMetCargar(null);
        }

        private void ContabilidadMetCargarLista()
        {
            DataTable Anterior = _Activos;
            try
            {
                _Activos = _Controlador.ContabilidadFuncActivos();
            }
            catch (Exception Excepcion)
            {
                _Activos = new DataTable();
                ClsDialogosContabilidad.ContabilidadMetError(this, "No se pudieron consultar los activos: " + Excepcion.Message);
            }
            _Paginacion.ContabilidadMetCargar(_Activos);
            if (Anterior != null) Anterior.Dispose();
        }

        private void ContabilidadMetEliminar(object Remitente, EventArgs Argumentos)
        {
            if (_IdActivo == 0) return;
            if (!ClsDialogosContabilidad.ContabilidadFuncConfirmar(this,
                    "¿Desea eliminar el activo " + ContabilidadTxtDescripcion.Text + "? Se dará de baja del registro.", "Sí, eliminar"))
                return;
            string Mensaje;
            int IdActivo = _IdActivo;
            bool Eliminado = _Controlador.ContabilidadFuncGrabarCambios(EstadoEntidadContabilidad.Borrar, ref IdActivo, ContabilidadTxtDescripcion.Text,
                ContabilidadCboModulo.Text, Convert.ToString(ContabilidadCboTipo.SelectedValue),
                ContabilidadDtpAdquisicion.Value, ContabilidadDtpInicio.Value, ContabilidadNudCompra.Value,
                ContabilidadNudResidual.Value, (int)ContabilidadNudMeses.Value, out Mensaje);
            Diseno.ContabilidadMetAviso(this, Eliminado ? string.Empty : Mensaje, Mensaje);
            if (!Eliminado) return;
            ContabilidadMetCargarLista();
            ContabilidadMetCargar(null);
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        private void ContabilidadMetAplicarPermisos(ClsModeloPermisosPantallaContabilidad Permisos)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnNueva, Permisos.PuedeInsertar);
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnEliminar, Permisos.PuedeEliminar);
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnGuardar, Permisos.PuedeInsertar || Permisos.PuedeEditar);
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        }

        private void ContabilidadMetCambiarFicha(object Remitente, EventArgs Argumentos)
        {
            if (!_CargandoDatos) ContabilidadBtnGuardar.Enabled = true;
        }

        private void ContabilidadMetTipo()
        {
            DataRowView Tipo = ContabilidadCboTipo.SelectedItem as DataRowView;
            if (Tipo != null) ContabilidadNudMeses.Value = (int)Tipo["Meses"];
        }

        private void ContabilidadMetValidar(object Remitente, EventArgs Argumentos)
        {
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            var Errores = _Controlador.ContabilidadFuncErroresActivo(ContabilidadTxtDescripcion.Text,
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                ContabilidadDtpAdquisicion.Value, ContabilidadDtpInicio.Value, ContabilidadNudCompra.Value,
                ContabilidadNudResidual.Value, (int)ContabilidadNudMeses.Value);
            var Campos = new System.Collections.Generic.Dictionary<string, Control>
            {
                // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                { "Descripcion", ContabilidadTxtDescripcion }, { "Inicio", ContabilidadDtpInicio },
                // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                { "Compra", ContabilidadNudCompra }, { "Residual", ContabilidadNudResidual }, { "Meses", ContabilidadNudMeses }
            };
            foreach (var Campo in Campos) _Validacion.ContabilidadFuncValidar(Campo.Value, Errores.ContainsKey(Campo.Key) ? Errores[Campo.Key] : null);
            if (Errores.Count > 0) return;
            string Mensaje;
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            int IdActivo = _IdActivo;
            bool Grabado = _Controlador.ContabilidadFuncGrabarCambios(
                _IdActivo == 0 ? EstadoEntidadContabilidad.Agregar : EstadoEntidadContabilidad.Modificar, ref IdActivo, ContabilidadTxtDescripcion.Text,
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                ContabilidadCboModulo.Text, Convert.ToString(ContabilidadCboTipo.SelectedValue),
                // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                ContabilidadDtpAdquisicion.Value, ContabilidadDtpInicio.Value, ContabilidadNudCompra.Value,
                ContabilidadNudResidual.Value, (int)ContabilidadNudMeses.Value, out Mensaje);
            Diseno.ContabilidadMetAviso(this, Grabado ? string.Empty : Mensaje, Mensaje);
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            if (!Grabado) return;
            ContabilidadMetCargarLista();
            DataRow[] Guardado = _Activos.Select("IdActivo = " + IdActivo);
            ContabilidadMetCargar(Guardado.Length > 0 ? Guardado[0] : null);
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        }

        // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        private void ContabilidadMetCargar(DataRow Activo)
        // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        {
            _CargandoDatos = true;
            _Validacion.ContabilidadMetLimpiar();
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            _IdActivo = Activo == null ? 0 : (int)Activo["IdActivo"];
            DateTime Hoy = DateTime.Today;
            ContabilidadTxtDescripcion.Text = Activo == null ? string.Empty : (string)Activo["Descripcion"];
            if (Activo == null) ContabilidadCboModulo.SelectedIndex = ContabilidadCboModulo.Items.Count > 0 ? 0 : -1;
            else ContabilidadCboModulo.SelectedItem = (string)Activo["Modulo"];
            if (Activo == null) ContabilidadCboTipo.SelectedIndex = ContabilidadCboTipo.Items.Count > 0 ? 0 : -1;
            else ContabilidadCboTipo.SelectedValue = (string)Activo["TipoCodigo"];
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            ContabilidadMetTipo();
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            ContabilidadDtpAdquisicion.Value = Activo == null ? Hoy : (DateTime)Activo["Adquisicion"];
            ContabilidadDtpInicio.Value = Activo == null ? Hoy : (DateTime)Activo["Inicio"];
            ContabilidadNudCompra.Value = Activo == null ? 0m : (decimal)Activo["Compra"];
            ContabilidadNudResidual.Value = Activo == null ? 0m : (decimal)Activo["Residual"];
            if (Activo != null) ContabilidadNudMeses.Value = (int)Activo["Meses"];
            ContabilidadBtnEliminar.Enabled = _IdActivo > 0;
            ContabilidadBtnGuardar.Text = _IdActivo > 0 ? "Guardar cambios" : "Guardar activo";
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            _CargandoDatos = false;
            ContabilidadBtnGuardar.Enabled = false;
            ContabilidadMetActualizar();
        }

        private void ContabilidadMetActualizar()
        {
            decimal Compra = ContabilidadNudCompra.Value;
            decimal Residual = ContabilidadNudResidual.Value;
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            if (_CargandoDatos) return;
            _Validacion.ContabilidadFuncValidar(ContabilidadNudResidual,
                Residual > Compra ? "El valor residual no puede superar el valor de compra." : null);
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        }
    }
}
