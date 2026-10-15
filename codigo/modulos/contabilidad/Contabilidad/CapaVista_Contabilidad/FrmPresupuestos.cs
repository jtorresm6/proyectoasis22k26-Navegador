/* Área: Contabilidad. Autor: Gabriel André Guillén Pocón. Carné: 0901-23-1998.
 * Fecha: 14/10/2026. Propósito: presupuesto de ejemplo sin persistencia. Responsable: Mario. */
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaControlador_Contabilidad;
using CapaVista_Contabilidad.Componentes;
using Diseno = CapaVista_Contabilidad.Componentes.ClsDisenoPrototiposContabilidad;

namespace CapaVista_Contabilidad
{
    public partial class FrmPresupuestos : Form
    {
        private readonly ClsControladorPrototiposContabilidad _Controlador = new ClsControladorPrototiposContabilidad();
        private DataTable _Detalle;
        private readonly ClsValidacionContabilidad _Validacion = new ClsValidacionContabilidad();
        private readonly ClsPaginacionContabilidad _Paginacion;
        private bool _CargandoDatos;
        private bool _HayCambios;

        public FrmPresupuestos()
        {
            InitializeComponent();
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
                string Error = _Controlador.ContabilidadFuncValidarPresupuesto(ContabilidadDtpInicio.Value,
                    ContabilidadDtpCierre.Checked ? (DateTime?)ContabilidadDtpCierre.Value : null, _Detalle);
                string ErrorFecha = ContabilidadDtpCierre.Checked ? _Controlador.ContabilidadFuncErrorFechas(ContabilidadDtpInicio.Value, ContabilidadDtpCierre.Value) : string.Empty;
                if (!_Validacion.ContabilidadFuncValidar(ContabilidadDtpCierre, ErrorFecha)) return;
                Diseno.ContabilidadMetAviso(this, Error, "Presupuesto validado correctamente.");
            };
            ContabilidadMetCargar(true);
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
