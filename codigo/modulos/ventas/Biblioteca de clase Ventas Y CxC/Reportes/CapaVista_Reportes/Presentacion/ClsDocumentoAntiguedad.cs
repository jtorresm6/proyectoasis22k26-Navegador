// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using CapaControlador_Reportes;

namespace CapaVista_Reportes
{
    public static class ClsDocumentoAntiguedad
    {
        public static void VentasMetAbrir(Form Padre)
        {
            try
            {
                Padre.UseWaitCursor = true;
                DateTime Corte, Desde;
                int Vendedor;
                string NombreVendedor;
                var Controlador = new ClsAntiguedadControlador();
                using (var Filtro = new Form
                {
                    Text = "Ventas — Antigüedad de saldos", ClientSize = new Size(560, 370),
                    StartPosition = FormStartPosition.CenterParent, MinimizeBox = false, MaximizeBox = false,
                    FormBorderStyle = FormBorderStyle.FixedDialog, BackColor = Color.FromArgb(248, 245, 249),
                    Font = new Font("Tahoma", 10F), Icon = Properties.Resources.IconoVentas
                })
                {
                    var Inicio = new DateTimePicker
                    {
                        Location = new Point(28, 148), Width = 240, Value = new DateTime(2000, 1, 1), Format = DateTimePickerFormat.Short,
                        Name = "VentasDtpEmisionDesde"
                    };
                    var Fin = new DateTimePicker { Location = new Point(292, 148), Width = 240, Format = DateTimePickerFormat.Short, Name = "VentasDtpFechaCorte" };
                    var Lista = new ComboBox
                    {
                        Location = new Point(28, 222), Width = 504, DropDownStyle = ComboBoxStyle.DropDownList,
                        Name = "VentasCboVendedor"
                    };
                    var Vendedores = Controlador.VentasFuncVendedores();
                    var Todos = Vendedores.NewRow();
                    Todos["Id"] = 0;
                    Todos["Nombre"] = "Todos los vendedores";
                    Vendedores.Rows.InsertAt(Todos, 0);
                    Lista.DisplayMember = "Nombre";
                    Lista.ValueMember = "Id";
                    Lista.DataSource = Vendedores;
                    var Generar = new Button
                    {
                        Text = "Generar reporte", Location = new Point(362, 306), Size = new Size(170, 40),
                        Name = "VentasBtnGenerarReporte", FlatStyle = FlatStyle.Flat,
                        BackColor = Color.FromArgb(59, 29, 74), ForeColor = Color.White
                    };
                    Generar.FlatAppearance.BorderSize = 0;
                    Generar.Click += (Origen, Evento) =>
                    {
                        if (Inicio.Value.Date > Fin.Value.Date)
                        {
                            MessageBox.Show(Filtro, "La emisión inicial no puede superar la fecha de corte.", "Ventas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Inicio.Focus();
                            return;
                        }
                        Filtro.DialogResult = DialogResult.OK;
                    };
                    var Cancelar = new Button { Text = "Cancelar", Name = "VentasBtnCancelarReporte", Location = new Point(236, 306), Size = new Size(110, 40), FlatStyle = FlatStyle.Flat, ForeColor = Color.FromArgb(59, 29, 74), DialogResult = DialogResult.Cancel };
                    var Encabezado = new Panel { Dock = DockStyle.Top, Height = 94, BackColor = Color.FromArgb(59, 29, 74) };
                    Encabezado.Controls.Add(new Label { Text = "Antigüedad de saldos", Location = new Point(24, 16), AutoSize = true, Font = new Font("Tahoma", 19F, FontStyle.Bold), ForeColor = Color.White });
                    Encabezado.Controls.Add(new Label { Text = "VENTAS Y CUENTAS POR COBRAR", Location = new Point(28, 59), AutoSize = true, ForeColor = Color.FromArgb(201, 162, 74), Font = new Font("Tahoma", 9F) });
                    Filtro.Controls.AddRange(new Control[]
                    {
                        Encabezado,
                        new Label { Text = "Emisión desde", AutoSize = true, Location = new Point(28, 122) }, Inicio,
                        new Label { Text = "Fecha de corte", AutoSize = true, Location = new Point(292, 122) }, Fin,
                        new Label { Text = "Vendedor", AutoSize = true, Location = new Point(28, 196) }, Lista,
                        new Label { Text = "Saldos pendientes al corte · Importes en quetzales", AutoSize = true, Location = new Point(28, 266), Font = new Font("Tahoma", 9F), ForeColor = Color.FromArgb(100, 85, 110) },
                        Cancelar, Generar
                    });
                    Filtro.AcceptButton = Generar;
                    Filtro.CancelButton = Cancelar;
                    Padre.UseWaitCursor = false;
                    if (Filtro.ShowDialog(Padre) != DialogResult.OK) return;
                    Corte = Fin.Value.Date;
                    Desde = Inicio.Value.Date;
                    Vendedor = Convert.ToInt32(Lista.SelectedValue);
                    NombreVendedor = Lista.Text;
                }
                Padre.UseWaitCursor = true;
                DataTable Datos = Controlador.VentasFuncConsultar(Corte, 0, "Todos", Vendedor, Desde);
                using (var Visor = new FrmVistaAntiguedad(Datos, Corte, NombreVendedor, "Todos"))
                {
                    Padre.UseWaitCursor = false;
                    Visor.ShowDialog(Padre);
                }
            }
            catch (Exception Error)
            {
                MessageBox.Show(Padre, Error.Message, "Ventas - Antigüedad de saldos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Padre.UseWaitCursor = false;
            }
        }

        internal static void VentasMetConfigurar(LocalReport Reporte, DataTable Datos, DateTime Corte, string Cliente, string Estado)
        {
            Reporte.ReportEmbeddedResource = "CapaVista_Reportes.Reportes.RpReporteAntiguedad.rdlc";
            Reporte.DataSources.Clear();
            Reporte.DataSources.Add(new ReportDataSource("DsAntiguedad", Datos));
            Reporte.SetParameters(new[]
            {
                new ReportParameter("FechaCorte", Corte.ToString("dd/MM/yyyy")),
                new ReportParameter("Cliente", Cliente), new ReportParameter("Estado", Estado)
            });
            Reporte.DisplayName = "Ventas_Antiguedad_" + Corte.ToString("yyyyMMdd");
        }
    }
}
