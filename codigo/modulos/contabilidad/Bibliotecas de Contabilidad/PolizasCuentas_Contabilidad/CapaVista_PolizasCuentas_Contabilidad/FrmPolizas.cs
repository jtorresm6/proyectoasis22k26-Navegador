/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Oskar Cermeño
 * Carné : 0901-23-15379
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  Encabezados de pólizas (locales y de otros módulos) con período y
 *  número correlativo. Desde aquí se abre cada póliza con su detalle
 *  y se actualizan en batch los saldos de las pólizas validadas.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CapaControlador_Contabilidad;
using CapaVista_Contabilidad.Componentes;

namespace CapaVista_Contabilidad
{
    public partial class FrmPolizas : Form
    {
        private const string _Todos = "Todos";
        private readonly int _IdModulo;
        private readonly string _ModoActualizacion;
        private readonly ClsModeloPermisosPantallaContabilidad _Permisos;
        private readonly ClsControladorPolizasContabilidad _Controlador = new ClsControladorPolizasContabilidad();
        private readonly CultureInfo _Cultura = CultureInfo.GetCultureInfo("es-GT");
        private List<ClsModeloPolizaContabilidad> _Polizas = new List<ClsModeloPolizaContabilidad>();
        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026
        private readonly ClsValidacionContabilidad _Validacion = new ClsValidacionContabilidad();
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026

        public FrmPolizas() : this(ClsModeloPermisosPantallaContabilidad.Total, 1)
        {
        }

        public FrmPolizas(ClsModeloPermisosPantallaContabilidad Permisos, int IdModulo)
            : this(Permisos, IdModulo, new ClsControladorPolizasContabilidad().ContabilidadFuncModoActualizacion(IdModulo))
        {
        }

        public FrmPolizas(ClsModeloPermisosPantallaContabilidad Permisos, int IdModulo, string ModoActualizacion)
        {
            _Permisos = Permisos;
            _IdModulo = IdModulo;
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 02/11/2026
            _ModoActualizacion = ModoActualizacion == "LINEA" ? "LINEA" : "BATCH";
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 02/11/2026
            InitializeComponent();
            ClsPresentacionContabilidad.ContabilidadMetPrepararVentana(this, "Pólizas", new Size(1180, 688));
            ContabilidadLblModo.Text = "Actualización: ● " + _ModoActualizacion;
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 02/11/2026
            ContabilidadLblModo.ForeColor = _ModoActualizacion == "LINEA" ? Color.FromArgb(34, 139, 70) : Color.FromArgb(197, 143, 62);
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 02/11/2026
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnNueva, Permisos.PuedeInsertar);
            ClsPermisosMenuContabilidad.ContabilidadMetRestringir(ContabilidadBtnActualizarSaldos, Permisos.PuedeEditar);
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026
            foreach (Button Boton in new[] { ContabilidadBtnNueva, ContabilidadBtnAbrir, ContabilidadBtnActualizarSaldos })
                ClsPermisosMenuContabilidad.ContabilidadMetGrisAlDeshabilitar(Boton);
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026

            ContabilidadCboEstado.DataSource = new[] { _Todos, "BORRADOR", "VALIDADA", "APLICADA" };
            ContabilidadCboTipoPeriodo.DataSource = _Controlador.ContabilidadFuncObtenerTiposPeriodo();
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026
            ContabilidadDtpDesde.MaxDate = _Controlador.ContabilidadFuncFechaMaxima();
            ContabilidadDtpHasta.MaxDate = _Controlador.ContabilidadFuncFechaMaxima();
            _Validacion.ContabilidadFuncValidar(ContabilidadDtpDesde, null);
            _Validacion.ContabilidadFuncValidar(ContabilidadDtpHasta, null);
            ContabilidadMetAnchoError(ContabilidadDtpDesde, 380);
            ContabilidadMetAnchoError(ContabilidadDtpHasta, 440);
            Disposed += (Remitente, Argumentos) => _Validacion.Dispose();
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026
            ContabilidadDtpDesde.Value = DateTime.Today;
            ContabilidadMetCalcularRango(this, EventArgs.Empty);

            ContabilidadCboPeriodo.SelectedIndexChanged += ContabilidadMetFiltrar;
            ContabilidadCboEstado.SelectedIndexChanged += ContabilidadMetFiltrar;
            ContabilidadChkPendientes.CheckedChanged += ContabilidadMetFiltrar;
            ContabilidadCboTipoPeriodo.SelectedIndexChanged += ContabilidadMetCalcularRango;
            ContabilidadDtpDesde.ValueChanged += ContabilidadMetCalcularRango;
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026
            ContabilidadDtpHasta.ValueChanged += (Remitente, Argumentos) => ContabilidadFuncValidarFechas();
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026
            ContabilidadDgvEncabezados.CellDoubleClick += (Remitente, Argumentos) => { if (Argumentos.RowIndex >= 0) ContabilidadMetAbrirSeleccionada(Remitente, Argumentos); };
            ContabilidadDgvEncabezados.CellFormatting += ContabilidadMetColorearEstado;
            ContabilidadBtnAbrir.Click += ContabilidadMetAbrirSeleccionada;
            ContabilidadBtnNueva.Click += (Remitente, Argumentos) => ContabilidadMetAbrir(0);
            ContabilidadBtnActualizarSaldos.Click += ContabilidadMetActualizarSaldos;
            ContabilidadMetCargar();
        }

        private void ContabilidadMetCargar()
        {
            try
            {
                _Polizas = _Controlador.ContabilidadFuncObtenerPolizas();
            }
            catch (Exception Excepcion)
            {
                _Polizas = new List<ClsModeloPolizaContabilidad>();
                ClsDialogosContabilidad.ContabilidadMetError(this, "No se pudieron consultar las pólizas: " + Excepcion.Message);
            }
            string Periodo = Convert.ToString(ContabilidadCboPeriodo.SelectedItem);
            List<string> Periodos = new List<string> { _Todos };
            Periodos.AddRange(_Polizas.Select(Poliza => Poliza.Periodo).Distinct().OrderByDescending(Elemento => Elemento));
            ContabilidadCboPeriodo.SelectedIndexChanged -= ContabilidadMetFiltrar;
            ContabilidadCboPeriodo.DataSource = Periodos;
            ContabilidadCboPeriodo.SelectedItem = Periodos.Contains(Periodo) ? Periodo : _Todos;
            ContabilidadCboPeriodo.SelectedIndexChanged += ContabilidadMetFiltrar;
            ContabilidadMetFiltrar(this, EventArgs.Empty);
        }

        private void ContabilidadMetFiltrar(object Remitente, EventArgs Argumentos)
        {
            string Periodo = Convert.ToString(ContabilidadCboPeriodo.SelectedItem);
            ContabilidadCboEstado.Enabled = !ContabilidadChkPendientes.Checked;
            string Estado = ContabilidadChkPendientes.Checked ? "VALIDADA" : Convert.ToString(ContabilidadCboEstado.SelectedItem);
            IEnumerable<ClsModeloPolizaContabilidad> Filtradas = _Polizas
                .Where(Poliza => string.IsNullOrEmpty(Periodo) || Periodo == _Todos || Poliza.Periodo == Periodo)
                .Where(Poliza => string.IsNullOrEmpty(Estado) || Estado == _Todos || Poliza.Estado == Estado)
                .OrderByDescending(Poliza => Poliza.Periodo)
                .ThenByDescending(Poliza => _Controlador.ContabilidadFuncNumeroPoliza(Poliza.CodigoPoliza));

            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("Id", typeof(int));
            Tabla.Columns.Add("Período", typeof(string));
            Tabla.Columns.Add("Número", typeof(int));
            Tabla.Columns.Add("Código", typeof(string));
            Tabla.Columns.Add("Fecha", typeof(string));
            Tabla.Columns.Add("Módulo", typeof(string));
            Tabla.Columns.Add("Estado", typeof(string));
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            Tabla.Columns.Add("Total", typeof(string));
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            Tabla.Columns.Add("Descripción", typeof(string));
            foreach (ClsModeloPolizaContabilidad Poliza in Filtradas)
                Tabla.Rows.Add(Poliza.IdPoliza, Poliza.Periodo, _Controlador.ContabilidadFuncNumeroPoliza(Poliza.CodigoPoliza), Poliza.CodigoPoliza,
                    Poliza.Fecha.ToString("dd/MM/yyyy"), Poliza.Modulo, Poliza.Estado,
                    // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                    _Controlador.ContabilidadFuncMonedaPais(Poliza.IdPais) + " " + _Controlador.ContabilidadFuncTotalDebe(Poliza.Lineas).ToString("N2", _Cultura),
                    Poliza.Descripcion);
                    // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026

            ContabilidadDgvEncabezados.DataSource = Tabla;
            ContabilidadDgvEncabezados.Columns["Id"].Visible = false;
            ContabilidadDgvEncabezados.Columns["Período"].FillWeight = 60;
            ContabilidadDgvEncabezados.Columns["Número"].FillWeight = 50;
            ContabilidadDgvEncabezados.Columns["Código"].FillWeight = 100;
            ContabilidadDgvEncabezados.Columns["Fecha"].FillWeight = 65;
            ContabilidadDgvEncabezados.Columns["Módulo"].FillWeight = 80;
            ContabilidadDgvEncabezados.Columns["Estado"].FillWeight = 70;
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            ContabilidadDgvEncabezados.Columns["Total"].FillWeight = 75;
            ContabilidadDgvEncabezados.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            ContabilidadDgvEncabezados.Columns["Descripción"].FillWeight = 200;
            int Pendientes = _Polizas.Count(Poliza => Poliza.Estado == "VALIDADA");
            ContabilidadLblTotal.Text = Tabla.Rows.Count + (Tabla.Rows.Count == 1 ? " póliza" : " pólizas") +
                "  ·  " + Pendientes + " validadas pendientes de actualizar saldos";
            ContabilidadBtnAbrir.Enabled = Tabla.Rows.Count > 0;
        }

        private void ContabilidadMetColorearEstado(object Remitente, DataGridViewCellFormattingEventArgs Argumentos)
        {
            if (Argumentos.RowIndex < 0 || ContabilidadDgvEncabezados.Columns[Argumentos.ColumnIndex].Name != "Estado") return;
            string Estado = Convert.ToString(Argumentos.Value);
            Argumentos.CellStyle.ForeColor = Estado == "BORRADOR" ? Color.FromArgb(34, 139, 70)
                : Estado == "VALIDADA" ? Color.FromArgb(197, 143, 62) : Color.Firebrick;
            Argumentos.CellStyle.SelectionForeColor = Argumentos.CellStyle.ForeColor;
            Argumentos.CellStyle.Font = new Font(ContabilidadDgvEncabezados.Font, FontStyle.Bold);
        }

        private void ContabilidadMetAbrirSeleccionada(object Remitente, EventArgs Argumentos)
        {
            if (ContabilidadDgvEncabezados.CurrentRow == null) return;
            ContabilidadMetAbrir(Convert.ToInt32(ContabilidadDgvEncabezados.CurrentRow.Cells["Id"].Value));
        }

        private void ContabilidadMetAbrir(int IdPoliza)
        {
            ClsModeloPermisosPantallaContabilidad PermisosDetalle = new ClsControladorSeguridadContabilidad().ContabilidadFuncObtenerPermisos(10006);
            if (!PermisosDetalle.TieneAcceso)
            {
                ClsDialogosContabilidad.ContabilidadMetError(this, "No tiene permiso para abrir el detalle de pólizas (10006).");
                return;
            }
            List<int> Orden = ContabilidadDgvEncabezados.Rows.Cast<DataGridViewRow>()
                .Select(Fila => Convert.ToInt32(Fila.Cells["Id"].Value)).ToList();
            using (FrmPolizasLocales Formulario = new FrmPolizasLocales(PermisosDetalle, _IdModulo, _ModoActualizacion, Orden, IdPoliza))
                Formulario.ShowDialog(this);
            ContabilidadMetCargar();
        }

        private void ContabilidadMetCalcularRango(object Remitente, EventArgs Argumentos)
        {
            string Tipo = Convert.ToString(ContabilidadCboTipoPeriodo.SelectedItem);
            bool Rango = Tipo == "RANGO";
            ContabilidadDtpHasta.Enabled = Rango;
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026
            if (Rango)
            {
                ContabilidadFuncValidarFechas();
                return;
            }
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026
            DateTime Desde, Hasta;
            DateTime Referencia = Remitente == ContabilidadDtpDesde ? ContabilidadDtpDesde.Value : DateTime.Today;
            _Controlador.ContabilidadMetCalcularRango(Tipo, Referencia, out Desde, out Hasta);
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026
            if (Hasta > ContabilidadDtpHasta.MaxDate) Hasta = ContabilidadDtpHasta.MaxDate;
            if (Desde < ClsControladorPolizasContabilidad.InicioSistema && Hasta >= ClsControladorPolizasContabilidad.InicioSistema)
                Desde = ClsControladorPolizasContabilidad.InicioSistema;
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026
            ContabilidadDtpDesde.ValueChanged -= ContabilidadMetCalcularRango;
            ContabilidadDtpDesde.Value = Desde;
            ContabilidadDtpHasta.Value = Hasta;
            ContabilidadDtpDesde.ValueChanged += ContabilidadMetCalcularRango;
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026
            ContabilidadFuncValidarFechas();
        }

        // Marca en rojo la fecha que no cuadra
        private bool ContabilidadFuncValidarFechas()
        {
            string Error = _Controlador.ContabilidadFuncErrorFechas(ContabilidadDtpDesde.Value, ContabilidadDtpHasta.Value);
            bool DesdeMal = Error != string.Empty && ContabilidadDtpDesde.Value.Date < ClsControladorPolizasContabilidad.InicioSistema;
            _Validacion.ContabilidadFuncValidar(ContabilidadDtpDesde, DesdeMal ? Error : null);
            _Validacion.ContabilidadFuncValidar(ContabilidadDtpHasta, DesdeMal ? null : Error);
            return Error == string.Empty;
        }

        // Da espacio al aviso de error bajo la fecha
        private static void ContabilidadMetAnchoError(Control Campo, int Ancho)
        {
            Control Error = Campo.Parent.Controls["ContabilidadLblError" + Campo.Name];
            if (Error != null) Error.Width = Ancho;
        }
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026

        private void ContabilidadMetActualizarSaldos(object Remitente, EventArgs Argumentos)
        {
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026
            if (!ContabilidadFuncValidarFechas()) return;
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026
            DateTime Desde = ContabilidadDtpDesde.Value.Date;
            DateTime Hasta = ContabilidadDtpHasta.Value.Date;
            if (!ClsDialogosContabilidad.ContabilidadFuncConfirmar(this,
                    "¿Desea actualizar los saldos con las pólizas validadas del " + Desde.ToString("dd/MM/yyyy") + " al " +
                    Hasta.ToString("dd/MM/yyyy") + "? Las pólizas quedarán aplicadas y ya no se podrán anular.", "Sí, actualizar"))
                return;
            string Mensaje;
            int Procesadas = _Controlador.ContabilidadFuncActualizarSaldos(Convert.ToString(ContabilidadCboTipoPeriodo.SelectedItem), Desde, Hasta, out Mensaje);
            if (Procesadas > 0) ClsDialogosContabilidad.ContabilidadMetExito(this, Mensaje);
            else if (Procesadas == 0) MessageBox.Show(this, Mensaje, "Actualizar saldos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else ClsDialogosContabilidad.ContabilidadMetError(this, Mensaje);
            ContabilidadMetCargar();
        }
    }
}
