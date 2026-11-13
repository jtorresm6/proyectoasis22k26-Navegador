using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_MovimientosProveedores;
using CapaVista_Componentes.Componentes;
using Microsoft.Reporting.WinForms;

namespace CapaVista_MovimientosProveedores
{
    public partial class FrmHistorialMovimientosProveedores : Form
    {
        private readonly int? _IdProveedorInicial;
        private readonly DateTimePicker ComprasDtpFechaCorte = new DateTimePicker();
        private readonly ComprasUsrComboBox ComprasCboProveedor = new ComprasUsrComboBox();
        private readonly CheckBox ComprasChkTodos = new CheckBox();
        private readonly LinkLabel ComprasLnkCargarProveedores = new LinkLabel();

        public FrmHistorialMovimientosProveedores() : this(null) { }

        public FrmHistorialMovimientosProveedores(int? IdProveedor)
        {
            InitializeComponent();
            _IdProveedorInicial = IdProveedor;
            BackColor = ClsTemaCompras.FondoGeneral;
            ComprasProcCrearFiltros();
            ComprasUsrEncabezadoFormulario.PrepararReporte += ComprasMetPrepararReporte;
            Load += async (Remitente, Evento) => {
                ClsPresentacionCompras.ComprasMetAjustarAreaFormulario(this,
                    Screen.FromControl(Owner ?? this).WorkingArea);
                await ComprasProcCargarProveedores();
            };
        }

        private void ComprasProcCrearFiltros()
        {
            var Filtros = new FlowLayoutPanel {
                Dock = DockStyle.Top, AutoSize = true, FlowDirection = FlowDirection.TopDown,
                WrapContents = false, Padding = new Padding(24)
            };
            Filtros.Controls.Add(new Label { Text = "Fecha de corte", AutoSize = true });
            ComprasDtpFechaCorte.Name = "ComprasDtpFechaCorte";
            ComprasDtpFechaCorte.Format = DateTimePickerFormat.Custom;
            ComprasDtpFechaCorte.CustomFormat = "dd/MM/yyyy";
            ComprasDtpFechaCorte.MaxDate = DateTime.Today;
            ComprasDtpFechaCorte.Value = DateTime.Today;
            ComprasDtpFechaCorte.Width = 220;
            Filtros.Controls.Add(ComprasDtpFechaCorte);
            ComprasChkTodos.Text = "Todos los proveedores";
            ComprasChkTodos.AutoSize = true;
            ComprasChkTodos.Checked = !_IdProveedorInicial.HasValue;
            ComprasChkTodos.Margin = new Padding(3, 20, 3, 8);
            ComprasChkTodos.CheckedChanged += (Remitente, Evento) =>
                ComprasCboProveedor.Enabled = !ComprasChkTodos.Checked;
            Filtros.Controls.Add(ComprasChkTodos);
            ComprasCboProveedor.Name = "ComprasCboProveedor";
            ComprasCboProveedor.Size = new Size(500, 56);
            ComprasCboProveedor.Enabled = !ComprasChkTodos.Checked;
            Filtros.Controls.Add(ComprasCboProveedor);
            ComprasLnkCargarProveedores.Text = "Recargar proveedores";
            ComprasLnkCargarProveedores.AutoSize = true;
            ComprasLnkCargarProveedores.LinkClicked += async (Remitente, Evento) => await ComprasProcCargarProveedores();
            Filtros.Controls.Add(ComprasLnkCargarProveedores);
            Filtros.Controls.Add(new Label {
                Text = "Seleccione los filtros y pulse Reportes en el encabezado.\n" +
                    "Rangos: vigente, 1–30, 31–60, 61–90 y más de 90 días vencidos.\n" +
                    "Los importes y totales se presentan por moneda del documento.",
                AutoSize = true, Margin = new Padding(3, 24, 3, 3)
            });
            ComprasPnlContenido.Controls.Add(Filtros);

            // El alto depende de los filtros; el contenedor no reserva una fila vacía.
            ComprasTlpFormulario.AutoSize = true;
            ComprasTlpFormulario.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ComprasTlpFormulario.RowStyles[1].SizeType = SizeType.AutoSize;
            ComprasPnlContenido.AutoSize = true;
            ComprasPnlContenido.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ComprasPnlContenido.Dock = DockStyle.Top;
            Filtros.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(1000, 150 + Filtros.GetPreferredSize(new Size(1000, 0)).Height);
            StartPosition = FormStartPosition.CenterParent;
        }

        private async Task ComprasProcCargarProveedores()
        {
            ComprasLnkCargarProveedores.Enabled = false;
            try
            {
                object Seleccion = ComprasCboProveedor.SelectedValue ?? (object)_IdProveedorInicial;
                var Tabla = await Task.Run(() => new ClsModeloNotaProveedor().ComprasMetObtenerProveedores());
                ComprasCboProveedor.ComprasMetCargarDatos(EnmTipoDatosCompras.Proveedores, Tabla);
                ComprasCboProveedor.SelectedIndex = -1;
                if (Seleccion != null) ComprasCboProveedor.SelectedValue = Seleccion;
            }
            catch (Exception Error)
            {
                MessageBox.Show(this, "No se pudieron cargar los proveedores. Use Recargar proveedores.\n" + Error.Message,
                    "Antigüedad de saldos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { ComprasLnkCargarProveedores.Enabled = true; }
        }

        private void ComprasMetPrepararReporte(object Remitente, ComprasReporteEventArgs Evento)
        {
            int? IdProveedor = null;
            int Id;
            if (!ComprasChkTodos.Checked)
            {
                if (!int.TryParse(Convert.ToString(ComprasCboProveedor.SelectedValue), out Id) || Id <= 0)
                    throw new InvalidOperationException("Seleccione un proveedor o marque Todos los proveedores.");
                IdProveedor = Id;
            }
            try
            {
                UseWaitCursor = true;
                var Filas = new ClsControladorBalanceAntiguedadProveedor()
                    .ComprasMetConsultarBalanceAntiguedad(ComprasDtpFechaCorte.Value.Date, IdProveedor);
                Evento.Reporte.DataSources.Clear();
                Evento.Reporte.DataSources.Add(new ReportDataSource("DsAntiguedad", Filas));
                Evento.Reporte.SetParameters(new[] {
                    new ReportParameter("FechaCorte", ComprasDtpFechaCorte.Value.ToString("dd/MM/yyyy")),
                    new ReportParameter("FiltroProveedor", IdProveedor.HasValue ? ComprasCboProveedor.Text : "Todos los proveedores")
                });
            }
            finally { UseWaitCursor = false; }
        }
    }
}
