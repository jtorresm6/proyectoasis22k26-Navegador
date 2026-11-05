/* Área: Contabilidad. Autor: Dylan Hernández. Carné: 0901-23-519.
 * Fecha: 14/10/2026. Propósito: ficha y cálculo de ejemplo sin persistencia. Responsable: Dylan. */
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
            foreach (Control Campo in new Control[] { ContabilidadTxtDescripcion, ContabilidadTxtCentro, ContabilidadCboModulo,
                ContabilidadCboTipo, ContabilidadDtpAdquisicion, ContabilidadDtpInicio, ContabilidadNudCompra, ContabilidadNudMeses })
                _Validacion.ContabilidadMetObligatorio(Campo);
            Disposed += (Remitente, Argumentos) => { _Validacion.Dispose(); _Paginacion.Dispose(); };
            ContabilidadCboModulo.DataSource = _Controlador.ContabilidadFuncModulos();
            _Catalogo = _Controlador.ContabilidadFuncVidaUtil();
            ContabilidadCboTipo.DisplayMember = "Nombre";
            ContabilidadCboTipo.ValueMember = "Tipo";
            ContabilidadCboTipo.DataSource = _Catalogo;
            _Paginacion.ContabilidadMetCargar(_Catalogo);
            ContabilidadCboTipo.SelectedIndexChanged += (Remitente, Argumentos) => ContabilidadMetTipo();
            ContabilidadNudCompra.ValueChanged += (Remitente, Argumentos) => ContabilidadMetActualizar();
            ContabilidadNudResidual.ValueChanged += (Remitente, Argumentos) => ContabilidadMetActualizar();
            ContabilidadNudMeses.ValueChanged += (Remitente, Argumentos) => ContabilidadMetActualizar();
            ContabilidadBtnNueva.Click += (Remitente, Argumentos) => ContabilidadMetCargar(false);
            ContabilidadBtnEjemplo.Click += (Remitente, Argumentos) => ContabilidadMetCargar(true);
            ContabilidadBtnGuardar.Click += ContabilidadMetValidar;
            foreach (Control Campo in new Control[] { ContabilidadTxtDescripcion, ContabilidadTxtCentro, ContabilidadCboModulo, ContabilidadCboTipo,
                ContabilidadDtpAdquisicion, ContabilidadDtpInicio, ContabilidadNudCompra, ContabilidadNudResidual, ContabilidadNudMeses })
            {
                if (Campo is DateTimePicker) ((DateTimePicker)Campo).ValueChanged += ContabilidadMetCambiarFicha;
                else if (Campo is NumericUpDown) ((NumericUpDown)Campo).ValueChanged += ContabilidadMetCambiarFicha;
                else if (Campo is ComboBox) ((ComboBox)Campo).SelectedIndexChanged += ContabilidadMetCambiarFicha;
                else Campo.TextChanged += ContabilidadMetCambiarFicha;
            }
            ContabilidadMetCargar(true);
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        private void ContabilidadMetAplicarPermisos(ClsModeloPermisosPantallaContabilidad Permisos)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnNueva, Permisos.PuedeInsertar);
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnEjemplo, Permisos.PuedeInsertar);
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnGuardar, Permisos.PuedeInsertar);
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
            var Errores = _Controlador.ContabilidadFuncErroresActivo(ContabilidadTxtDescripcion.Text, ContabilidadTxtCentro.Text,
                ContabilidadDtpAdquisicion.Value, ContabilidadDtpInicio.Value, ContabilidadNudCompra.Value,
                ContabilidadNudResidual.Value, (int)ContabilidadNudMeses.Value);
            var Campos = new System.Collections.Generic.Dictionary<string, Control>
            {
                { "Descripcion", ContabilidadTxtDescripcion }, { "Centro", ContabilidadTxtCentro }, { "Inicio", ContabilidadDtpInicio },
                { "Compra", ContabilidadNudCompra }, { "Residual", ContabilidadNudResidual }, { "Meses", ContabilidadNudMeses }
            };
            foreach (var Campo in Campos) _Validacion.ContabilidadFuncValidar(Campo.Value, Errores.ContainsKey(Campo.Key) ? Errores[Campo.Key] : null);
            if (Errores.Count > 0) return;
            string Mensaje;
            bool Grabado = _Controlador.ContabilidadFuncGrabarCambios(EstadoEntidadContabilidad.Agregar, ContabilidadTxtDescripcion.Text,
                ContabilidadTxtCentro.Text, ContabilidadCboModulo.Text, Convert.ToString(ContabilidadCboTipo.SelectedValue),
                ContabilidadDtpAdquisicion.Value, ContabilidadDtpInicio.Value, ContabilidadNudCompra.Value,
                ContabilidadNudResidual.Value, (int)ContabilidadNudMeses.Value, out Mensaje);
            Diseno.ContabilidadMetAviso(this, Grabado ? string.Empty : Mensaje, Mensaje);
        }

        private void ContabilidadMetCargar(bool Ejemplo)
        {
            _CargandoDatos = true;
            _Validacion.ContabilidadMetLimpiar();
            ContabilidadTxtDescripcion.Text = Ejemplo ? "Vehículo de reparto" : string.Empty;
            ContabilidadTxtCentro.Text = Ejemplo ? "DISTRIBUCION" : string.Empty;
            ContabilidadCboModulo.SelectedIndex = Ejemplo ? 1 : 0;
            ContabilidadCboTipo.SelectedIndex = 0;
            ContabilidadMetTipo();
            ContabilidadDtpAdquisicion.Value = new DateTime(2026, 10, 1);
            ContabilidadDtpInicio.Value = new DateTime(2026, 10, 2);
            ContabilidadNudCompra.Value = Ejemplo ? 120000m : 0m;
            ContabilidadNudResidual.Value = Ejemplo ? 12000m : 0m;
            _CargandoDatos = false;
            ContabilidadBtnGuardar.Enabled = false;
            ContabilidadMetActualizar();
        }

        private void ContabilidadMetActualizar()
        {
            decimal Compra = ContabilidadNudCompra.Value;
            decimal Residual = ContabilidadNudResidual.Value;
            string ErrorResidual;
            var Errores = _Controlador.ContabilidadFuncErroresActivo(ContabilidadTxtDescripcion.Text, ContabilidadTxtCentro.Text,
                ContabilidadDtpAdquisicion.Value, ContabilidadDtpInicio.Value, Compra, Residual, (int)ContabilidadNudMeses.Value);
            if (Errores.TryGetValue("Residual", out ErrorResidual))
            {
                ContabilidadLblResumen.Text = "El valor residual no puede superar el valor de compra.";
                ContabilidadLblResumen.ForeColor = Color.FromArgb(197, 143, 62);
                return;
            }
            ContabilidadLblResumen.ForeColor = Color.FromArgb(35, 50, 61);
            ContabilidadLblResumen.Text = "Base depreciable: " + Diseno.ContabilidadFuncMoneda(_Controlador.ContabilidadFuncBaseDepreciable(Compra, Residual))
                + "     Cuota mensual: " + Diseno.ContabilidadFuncMoneda(_Controlador.ContabilidadFuncDepreciacion(Compra, Residual, (int)ContabilidadNudMeses.Value))
                + "     Valor inicial en libros: " + Diseno.ContabilidadFuncMoneda(Compra);
        }
    }
}
