using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_EnlaceB;
using CapaModelo_EnlaceB.Entidades;

namespace CapaVista_EnlaceB.Formularios
{
    public sealed class FrmEnlaceModulo : Form
    {
        private readonly ModuloEnlace _Modulo;
        private readonly ClsControladorEnlaceB _Controlador;
        private readonly DateTimePicker _BancarioDtpInicio = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 125 };
        private readonly DateTimePicker _BancarioDtpFin = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 125 };
        private readonly DataGridView _BancarioDgvTabla = new DataGridView { Dock = DockStyle.Fill };
        private readonly Label _BancarioLblEstado = new Label { Dock = DockStyle.Fill, AutoEllipsis = true, TextAlign = ContentAlignment.MiddleLeft };
        private readonly FlowLayoutPanel _BancarioFlpFiltros = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, AutoScroll = true };
        private bool _Ocupado;

        public FrmEnlaceModulo(ModuloEnlace Modulo, ClsControladorEnlaceB Controlador)
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = true;
            SizeGripStyle = SizeGripStyle.Hide;
            this._Modulo = Modulo; this._Controlador = Controlador;
            string Nombre = Modulo == ModuloEnlace.RecursosHumanos ? "Recursos Humanos" : Modulo == ModuloEnlace.Logistica ? "Logística" : Modulo.ToString();
            Text = "8008 – Enlace" + Modulo; ClientSize = new Size(1120, 680);
            Icon = (Icon)new System.ComponentModel.ComponentResourceManager(typeof(FrmEnlaceOtrosModulos)).GetObject("$this.Icon");
            StartPosition = FormStartPosition.CenterParent; BackColor = ClsEstiloEnlace.Fondo;
            BackgroundImage = Properties.Resources.FondoEnlaceotrosModulo; BackgroundImageLayout = ImageLayout.Stretch;
            Font = new Font("Segoe UI", 10); ForeColor = ClsEstiloEnlace.Verde;
            var BancarioTlpLayout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(20), RowCount = 5, ColumnCount = 1, BackColor = Color.Transparent };
            foreach (int AlturaFila in new[] { 70, 108, 46 }) BancarioTlpLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, AlturaFila));
            BancarioTlpLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); BancarioTlpLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            var BancarioPnlCabecera = new Panel { Dock = DockStyle.Fill };
            BancarioPnlCabecera.Controls.Add(new Label { Text = "Enlace con " + Nombre, Dock = DockStyle.Fill, Font = new Font("Lucida Bright", 23, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft });
            BancarioPnlCabecera.Controls.Add(new PictureBox { Image = Properties.Resources.BalanzaOro, SizeMode = PictureBoxSizeMode.Zoom, Dock = DockStyle.Right, Width = 70 });
            BancarioTlpLayout.Controls.Add(BancarioPnlCabecera, 0, 0);
            _BancarioDtpInicio.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); _BancarioDtpFin.Value = DateTime.Today;
            _BancarioDtpInicio.Margin = new Padding(4, 32, 8, 0); _BancarioDtpFin.Margin = new Padding(4, 32, 12, 0);
            _BancarioFlpFiltros.Controls.Add(ClsEstiloEnlace.BancarioMetEtiquetaObligatoria("Desde")); _BancarioFlpFiltros.Controls.Add(ClsEstiloEnlace.BancarioMetCampo(_BancarioDtpInicio));
            _BancarioFlpFiltros.Controls.Add(ClsEstiloEnlace.BancarioMetEtiquetaObligatoria("Hasta")); _BancarioFlpFiltros.Controls.Add(ClsEstiloEnlace.BancarioMetCampo(_BancarioDtpFin));
            _BancarioDtpInicio.ValueChanged += (Remitente, Argumentos) => ClsEstiloEnlace.BancarioMetValidacion(_BancarioDtpInicio, "");
            _BancarioDtpFin.ValueChanged += (Remitente, Argumentos) => ClsEstiloEnlace.BancarioMetValidacion(_BancarioDtpFin, "");
            _BancarioFlpFiltros.Controls.Add(ClsEstiloEnlace.BancarioMetBoton("Consultar", async (Remitente, Argumentos) => await BancarioMetConsultar()));
            _BancarioFlpFiltros.Controls.Add(ClsEstiloEnlace.BancarioMetBoton("Limpiar", (Remitente, Argumentos) => { _BancarioDgvTabla.DataSource = null; _BancarioLblEstado.Text = "Sin consultar."; }));
            _BancarioFlpFiltros.Controls.Add(ClsEstiloEnlace.BancarioMetBoton("Exportar CSV", (Remitente, Argumentos) => BancarioMetExportar())); BancarioTlpLayout.Controls.Add(_BancarioFlpFiltros, 0, 1);
            string Nota = Modulo == ModuloEnlace.Compras ? "Órdenes de Compras. La recepción bancaria usa una referencia externa, sin FK directa a estas órdenes. Fecha: emisión." :
                Modulo == ModuloEnlace.Logistica ? "Fecha: contratación del servicio. Una solicitud puede tener varias órdenes; cada relación se muestra por separado." :
                Modulo == ModuloEnlace.Ventas ? "Fecha: depósito. La asociación de un movimiento no acredita por sí sola la conciliación." :
                "Fecha: inicio de planilla recibida. Cada beneficiario y orden se muestra por separado.";
            BancarioTlpLayout.Controls.Add(new Label { Text = Nota, Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9), Padding = new Padding(0, 4, 0, 0) }, 0, 2);
            ClsEstiloEnlace.BancarioMetTabla(_BancarioDgvTabla); BancarioTlpLayout.Controls.Add(ClsEstiloEnlace.BancarioMetPaginar(_BancarioDgvTabla), 0, 3);
            var BancarioTlpPie = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            BancarioTlpPie.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); BancarioTlpPie.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            _BancarioLblEstado.Text = "Seleccione las fechas y pulse Consultar. Esta ventana no ejecuta pagos.";
            BancarioTlpPie.Controls.Add(_BancarioLblEstado, 0, 0); BancarioTlpPie.Controls.Add(ClsEstiloEnlace.BancarioMetBoton("Volver", (Remitente, Argumentos) => Close()), 1, 0);
            BancarioTlpLayout.Controls.Add(BancarioTlpPie, 0, 4); Controls.Add(BancarioTlpLayout);
            ClsEstiloEnlace.BancarioMetVentana(this);
        }
        private async Task BancarioMetConsultar()
        {
            if (_Ocupado) return;
            DateTime Desde = _BancarioDtpInicio.Value.Date, Hasta = _BancarioDtpFin.Value.Date;
            if (Desde > Hasta || Hasta.Year >= 9999)
            { ClsEstiloEnlace.BancarioMetValidacion(_BancarioDtpFin, "Revise el rango de fechas."); _BancarioLblEstado.Text = "Corrija la fecha final."; _BancarioDtpFin.Focus(); return; }
            _Ocupado = true; _BancarioFlpFiltros.Enabled = false; UseWaitCursor = true; _BancarioLblEstado.Text = "Consultando…";
            try
            {
                var Datos = await Task.Run(() => _Controlador.BancarioMetConsultarModulo(_Modulo, Desde, Hasta)); _BancarioDgvTabla.DataSource = Datos;
                _BancarioLblEstado.Text = Datos.Rows.Count == 0 ? "No hay registros en el período seleccionado." : Datos.Rows.Count + " filas. No se suman monedas ni documentos repetidos.";
            }
            catch (Exception Ex) { _BancarioLblEstado.Text = "No se pudo consultar."; ClsEstiloEnlace.BancarioMetError(this, Ex); }
            finally { _Ocupado = false; _BancarioFlpFiltros.Enabled = true; UseWaitCursor = false; }
        }
        private void BancarioMetExportar()
        {
            var Datos = ClsEstiloEnlace.BancarioMetDatosCompletos(_BancarioDgvTabla);
            if (Datos == null || Datos.Rows.Count == 0) { MessageBox.Show(this, "Consulte registros antes de exportar."); return; }
            using (var BancarioSfdDialogo = new SaveFileDialog { Filter = "Archivo CSV|*.csv", FileName = "Enlace_" + _Modulo + ".csv" })
            {
                if (BancarioSfdDialogo.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    using (var Writer = new StreamWriter(BancarioSfdDialogo.FileName, false, new UTF8Encoding(true)))
                    {
                        var Campos = new string[Datos.Columns.Count];
                        for (int Indice = 0; Indice < Campos.Length; Indice++) Campos[Indice] = BancarioMetCsv(Datos.Columns[Indice].ColumnName);
                        Writer.WriteLine(string.Join(",", Campos));
                        foreach (DataRow Fila in Datos.Rows)
                        {
                            for (int Indice = 0; Indice < Campos.Length; Indice++) Campos[Indice] = BancarioMetCsv(Fila[Indice] is DateTime ? ((DateTime)Fila[Indice]).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : Convert.ToString(Fila[Indice], CultureInfo.InvariantCulture));
                            Writer.WriteLine(string.Join(",", Campos));
                        }
                    }
                    _BancarioLblEstado.Text = "Archivo exportado.";
                    ClsEstiloEnlace.BancarioMetExito(this, "El archivo CSV se exportó correctamente.");
                }
                catch (Exception Ex) { ClsEstiloEnlace.BancarioMetError(this, Ex); }
            }
        }
        private static string BancarioMetCsv(string Valor)
        {
            if (!string.IsNullOrEmpty(Valor) && "=+-@\t\r".IndexOf(Valor[0]) >= 0) Valor = "'" + Valor;
            return "\"" + (Valor ?? "").Replace("\"", "\"\"") + "\"";
        }
        protected override void OnFormClosing(FormClosingEventArgs Argumentos)
        { if (_Ocupado) Argumentos.Cancel = true; base.OnFormClosing(Argumentos); }
    }
}
