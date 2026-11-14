/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 10/11/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Operar el cierre de movimientos desde el formulario.
 */
using CapaControlador_CierreMovimientos_Ventas;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CapaVista_CierreMovimientos_Ventas
{
    public partial class FrmCierreMovimientosVentas
    {

        internal void VentasMetConectarEventos()
        {
            foreach (var Grid in new[] { this.VentasDgvRevisionCierre, this.VentasDgvHistorialCierre, this.VentasDgvPoliza })
                VentasMetFormatearFechas(Grid);
            this.Load += (Origen, Evento) => VentasMetEjecutar(this, this.VentasMetCargar);
            VentasMetConectarBoton(this.VentasBtnIngresar, this.VentasMetNuevo);
            VentasMetConectarBoton(this.VentasBtnRevisar, this.VentasMetConsultarCierre);
            VentasMetConectarBoton(this.VentasBtnGuardarBorrador, this.VentasMetGuardarBorrador);
            VentasMetConectarBoton(this.VentasBtnModificar, this.VentasMetModificarBorrador);
            VentasMetConectarBoton(this.VentasBtnEliminar, this.VentasMetAnularBorrador);
            VentasMetConectarBoton(this.VentasBtnRefrescar, this.VentasMetRevisar);
            this.VentasCboClienteHistorial.SelectedIndexChanged += (Origen, Evento) =>
            {
                if (!this.Cargando && this.Moneda > 0 && this.VentasTbcCierre.SelectedTab == this.VentasTbpHistorial)
                    VentasMetEjecutar(this, this.VentasMetHistorial);
            };
            this.VentasTxtPeriodoHistorial.KeyDown += (Origen, Evento) =>
            {
                if (Evento.KeyCode == Keys.Enter)
                {
                    Evento.SuppressKeyPress = true;
                    VentasMetEjecutar(this, this.VentasMetHistorial);
                }
            };
            VentasMetConectarBoton(this.VentasBtnConfirmar, this.VentasMetConfirmar);
            VentasMetConectarBoton(this.VentasBtnIrSiguientePeriodo, this.VentasMetIrAlSiguientePeriodo);
            this.VentasBtnCancelar.Click += (Origen, Evento) => this.VentasMetLimpiarRevision();
            this.VentasBtnCerrar.Click += (Origen, Evento) => this.Close();
            VentasMetConectarBoton(this.VentasBtnTrasladarContabilidad, this.VentasMetEnviarPoliza);
            VentasMetConectarEventosRestantes();
        }

        internal void VentasMetEnviarPoliza()
        {
                this.VentasMetMostrarPartida();
                // Natali Sofía Montenegro Portillo | 0901-23-10017 | Explica por qué una póliza todavía no se puede enviar.
                if (this.IdCierre <= 0)
                    throw new InvalidOperationException("No hay cierre para el mes y moneda elegidos arriba. Abra Revisar y confirmar, marque la constancia y confirme el cierre.");
                if (this._Controlador.VentasFuncEstadoCierre(this.IdCierre) != "CONFIRMADO")
                {
                    DateTime Siguiente = this._Controlador.VentasFuncSiguientePeriodo(this.Moneda, this.VentasDtpDesdeCierre.Value.Date);
                    if (Siguiente < this.VentasDtpDesdeCierre.Value.Date)
                        throw new InvalidOperationException("Este cierre sigue en borrador. Use el botón principal para ir a " + Siguiente.ToString("MMMM yyyy", new System.Globalization.CultureInfo("es-GT")) + " y confirme los meses pendientes.");
                    throw new InvalidOperationException("El cierre de este mes está en borrador. Abra Revisar y confirmar, marque la constancia y pulse Confirmar cierre.");
                }
                if (this.Partida == null || this.Partida.Rows.Count == 0)
                    throw new InvalidOperationException("Este cierre no tiene líneas de póliza. Revise los movimientos y las cuentas contables configuradas.");
                if (this.Partida.AsEnumerable().Any(Fila => Convert.ToString(Fila["Cuenta"]).StartsWith("PENDIENTE", StringComparison.Ordinal)))
                    throw new InvalidOperationException("Hay cuentas contables pendientes de configurar para Ventas. Corrija las cuentas antes de enviar la póliza.");
                this._Controlador.VentasMetValidarCaptura(this.PaisContable > 0, "Seleccione el país de la póliza antes de enviarla a Contabilidad.");
                if (MessageBox.Show(this, "Se enviará la póliza de " + this.VentasDtpDesdeCierre.Value.ToString("MMMM yyyy", new System.Globalization.CultureInfo("es-GT"))
                    + " a Contabilidad para " + this.VentasCboPaisContable.Text + ". ¿Desea continuar?", "Enviar póliza", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                string Referencia = this._Controlador.VentasFuncTrasladar(this.IdCierre, this.VentasIdUsuario, this.EnlaceContable);
                this.VentasMetMostrarPartida();
                VentasMetInformar(this, "Contabilidad confirmó la póliza: " + Referencia);
        }

        internal void VentasMetConectarEventosRestantes()
        {
            VentasMetConectarBoton(this.VentasBtnImprimir, this.VentasMetImprimirCierre);
            this.VentasCboMonedaCierre.SelectedIndexChanged += (Origen, Evento) =>
            {
                if (!this.Cargando)
                {
                    this.IdBorrador = 0;
                    this.EditandoBorrador = true;
                    this.VentasMetLimpiarRevision();
                    this.VentasMetActualizarAccionesCierre();
                    if (this.VentasTbcCierre.SelectedTab == this.VentasTbpContabilidad && this.Moneda > 0)
                        VentasMetEjecutar(this, this.VentasMetMostrarPartida);
                }
            };
            this.VentasChkRevisionDocumentos.CheckedChanged += (Origen, Evento) => this.VentasMetActualizarConfirmacion();
            this.VentasTbcCierre.SelectedIndexChanged += (Origen, Evento) =>
            {
                if (!this.Cargando && this.Moneda > 0)
                    VentasMetEjecutar(this, this.VentasMetRevisar);
            };
            this.VentasDgvHistorialCierre.SelectionChanged += (Origen, Evento) =>
            {
                if (this.Cargando || this.VentasDgvHistorialCierre.CurrentRow == null)
                    return;
                var Fila = this.VentasDgvHistorialCierre.CurrentRow.Tag as DataRow;
                if (Fila != null)
                {
                    this.VentasMetElegirCierreHistorial(Fila);
                    VentasMetEjecutar(this, this.VentasMetMostrarPartida);
                }
            };
            this.VentasDtpDesdeCierre.Enabled = false;
            this.VentasDtpHastaCierre.Enabled = false;
            this.VentasDtpCorteCierre.Enabled = false;
            this.VentasTxtPeriodoHistorial.MaxLength = 7;
            this.VentasBtnTrasladarContabilidad.Enabled = this.EnlaceContable != null;
            this.VentasLblTrasladoCierre.Text = "Elija el mes arriba. Revise y confirme el cierre; después abra Ver póliza y enviar.";
        }
        private void VentasMetConectarBoton(Button Boton, Action Accion)
        {
            Boton.Click += (Origen, Evento) => VentasMetEjecutar(this, Accion);
        }

        private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("es-GT");

        private static int VentasFuncObtenerId(ComboBox Control)
        {
            return Control.SelectedValue == null || Control.SelectedValue is DataRowView
                ? 0 : Convert.ToInt32(Control.SelectedValue);
        }

        private static void VentasMetCargarOpciones(ComboBox Control, DataTable Datos, int Id = 0)
        {
            Control.DataSource = null;
            Control.Items.Clear();
            if (!Datos.Columns.Contains("Id")) return;
            Control.DisplayMember = "Nombre";
            Control.ValueMember = "Id";
            Control.DataSource = Datos;
            if (Id > 0) Control.SelectedValue = Id;
            else Control.SelectedIndex = -1;
        }

        private static void VentasMetFormatearFechas(DataGridView Grid)
        {
            Grid.DefaultCellStyle.FormatProvider = Cultura;
            Grid.CellFormatting += (Origen, Evento) =>
            {
                if (!(Evento.Value is DateTime)) return;
                Evento.Value = ((DateTime)Evento.Value).ToString("dd/MM/yyyy");
                Evento.FormattingApplied = true;
            };
        }

        private static void VentasMetEjecutar(Form Ventana, Action Accion)
        {
            Ventana.UseWaitCursor = true;
            try { Accion(); }
            catch (Exception Error)
            {
                MessageBox.Show(Ventana, Error.Message, "Ventas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally { Ventana.UseWaitCursor = false; }
        }

        private static void VentasMetInformar(Form Ventana, string Mensaje)
        {
            MessageBox.Show(Ventana, Mensaje, "Ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static DataRow VentasFuncSeleccionar(Form Padre, string Titulo, DataTable Datos)
        {
            using (var Dialogo = new Form { Text = Titulo, Width = 1000, Height = 480,
                StartPosition = FormStartPosition.CenterParent, BackColor = Color.FromArgb(248, 245, 249),
                MinimizeBox = false, MaximizeBox = false, ShowInTaskbar = false })
            {
                var Buscar = new TextBox { Name = "VentasTxtBuscar", Dock = DockStyle.Top, Font = new Font("Tahoma", 11) };
                var Tabla = new DataGridView { Name = "VentasDgvSeleccion", Dock = DockStyle.Fill, ReadOnly = true,
                    AutoGenerateColumns = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White,
                    RowHeadersVisible = false, EnableHeadersVisualStyles = false };
                var Pie = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 44,
                    FlowDirection = FlowDirection.RightToLeft };
                var Aceptar = new Button { Name = "VentasBtnSeleccionar", Text = "Seleccionar", Width = 110, Height = 32,
                    BackColor = Color.FromArgb(123, 63, 147), ForeColor = Color.White };
                var Cancelar = new Button { Name = "VentasBtnCancelar", Text = "Cancelar", Width = 100, Height = 32,
                    DialogResult = DialogResult.Cancel };
                Tabla.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(59, 29, 74);
                Tabla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                Tabla.DefaultCellStyle.SelectionBackColor = Color.FromArgb(228, 212, 234);
                Tabla.DefaultCellStyle.SelectionForeColor = Color.FromArgb(59, 29, 74);
                Tabla.DataSource = Datos;
                foreach (DataGridViewColumn Columna in Tabla.Columns)
                    if (Columna.Name == "Id" || Columna.Name.StartsWith("id")) Columna.Visible = false;
                DataRow Elegida = null;
                Action Elegir = () =>
                {
                    var Fila = Tabla.CurrentRow?.DataBoundItem as DataRowView;
                    if (Fila == null) return;
                    Elegida = Fila.Row;
                    Dialogo.DialogResult = DialogResult.OK;
                };
                Aceptar.Click += (Origen, Evento) => Elegir();
                Tabla.CellDoubleClick += (Origen, Evento) => { if (Evento.RowIndex >= 0) Elegir(); };
                Buscar.TextChanged += (Origen, Evento) =>
                {
                    var Filtrada = Datos.Clone();
                    foreach (DataRow Fila in Datos.Rows)
                        if (Fila.ItemArray.Any(Valor => Convert.ToString(Valor).IndexOf(Buscar.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0))
                            Filtrada.ImportRow(Fila);
                    Tabla.DataSource = Filtrada;
                };
                Pie.Controls.AddRange(new Control[] { Aceptar, Cancelar });
                Dialogo.Controls.AddRange(new Control[] { Tabla, Buscar, Pie });
                Dialogo.AcceptButton = Aceptar;
                Dialogo.CancelButton = Cancelar;
                Dialogo.ShowDialog(Padre);
                return Elegida;
            }
        }

        private static string VentasFuncMotivo(Form Padre, string Titulo)
        {
            using (var Dialogo = new Form { Text = Titulo, Width = 520, Height = 205,
                StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false, MaximizeBox = false, ShowInTaskbar = false,
                BackColor = Color.FromArgb(248, 245, 249) })
            {
                var Etiqueta = new Label { Name = "VentasLblMotivo", Text = "Motivo de anulación", Left = 16, Top = 16, Width = 460 };
                var Motivo = new TextBox { Name = "VentasTxtMotivo", Left = 16, Top = 44, Width = 470, Height = 55,
                    Multiline = true, MaxLength = 180 };
                var Aceptar = new Button { Name = "VentasBtnAnular", Text = "Anular", Left = 260, Top = 112, Width = 110,
                    BackColor = Color.FromArgb(123, 63, 147), ForeColor = Color.White,
                    DialogResult = DialogResult.OK };
                var Cancelar = new Button { Name = "VentasBtnCancelar", Text = "Cancelar", Left = 380, Top = 112, Width = 105,
                    DialogResult = DialogResult.Cancel };
                Dialogo.Controls.AddRange(new Control[] { Etiqueta, Motivo, Aceptar, Cancelar });
                Dialogo.AcceptButton = Aceptar;
                Dialogo.CancelButton = Cancelar;
                return Dialogo.ShowDialog(Padre) == DialogResult.OK ? Motivo.Text.Trim() : null;
            }
        }

    }
}
