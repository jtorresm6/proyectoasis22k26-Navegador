using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Windows.Forms;

namespace CapaVista_DisponibilidadDiaria.FrmReportes
{
    public partial class FrmReporteDisponibilidadDiaria : Form
    {
        private readonly DataTable _Tabla;
        private readonly string _Titulo;
        private readonly ReportViewer _Visor;

        public FrmReporteDisponibilidadDiaria(DataTable tabla, string titulo)
        {
            _Tabla = PrepararTabla(tabla ?? throw new ArgumentNullException(nameof(tabla)));
            _Titulo = string.IsNullOrWhiteSpace(titulo) ? "Reporte de Disponibilidad Diaria" : titulo;
            InitializeComponent();
            _Visor = new ReportViewer { Dock = DockStyle.Fill, ProcessingMode = ProcessingMode.Local };
            Controls.Add(_Visor);
            _Visor.BringToFront();
            Load += BancosFrmReporteDisponibilidadDiaria_Load;
        }

        private void BancosFrmReporteDisponibilidadDiaria_Load(object sender, EventArgs e)
        {
            try
            {
                Text = "FrmReporteDisponibilidadDiaria";
                _Visor.LocalReport.ReportEmbeddedResource = "CapaVista_DisponibilidadDiaria.Reportes.RpReporteDisponibilidadDiaria.rdlc";
                _Visor.LocalReport.DataSources.Clear();
                _Visor.LocalReport.DataSources.Add(new ReportDataSource("ReporteDisponibilidadDiaria", _Tabla));
                _Visor.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar reporte:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static DataTable PrepararTabla(DataTable origen)
        {
            var tabla = CrearTabla("ReporteDisponibilidadDiaria");
            for (int i = 0; i < origen.Rows.Count; i++)
            {
                DataRow fila = origen.Rows[i];
                tabla.Rows.Add(i + 1, Valor(fila, "Cuenta"), Valor(fila, "Banco"), Valor(fila, "Tipo de cuenta"),
                    DecimalValor(fila, "Disponible"), FechaValor(fila, "Fecha"), Valor(fila, "Moneda"),
                    "Saldo inicial: " + Valor(fila, "Saldo inicial") + " · Ingresos: " + Valor(fila, "Ingresos") + " · Egresos: " + Valor(fila, "Egresos"), Valor(fila, "Fuente"));
            }
            return tabla;
        }

        private static DataTable CrearTabla(string nombre)
        {
            var tabla = new DataTable(nombre);
            tabla.Columns.Add("ID", typeof(int)); tabla.Columns.Add("Referencia", typeof(string));
            tabla.Columns.Add("Concepto", typeof(string)); tabla.Columns.Add("Naturaleza", typeof(string));
            tabla.Columns.Add("Monto", typeof(decimal)); tabla.Columns.Add("Fecha", typeof(DateTime));
            tabla.Columns.Add("CuentaEmpresa", typeof(string)); tabla.Columns.Add("TipoTransaccion", typeof(string));
            tabla.Columns.Add("Beneficiario", typeof(string)); return tabla;
        }

        private static string Valor(DataRow fila, string columna) => fila.Table.Columns.Contains(columna) && fila[columna] != DBNull.Value ? Convert.ToString(fila[columna]) : string.Empty;
        private static decimal DecimalValor(DataRow fila, string columna) => fila.Table.Columns.Contains(columna) && fila[columna] != DBNull.Value ? Convert.ToDecimal(fila[columna]) : 0m;
        private static DateTime FechaValor(DataRow fila, string columna) => fila.Table.Columns.Contains(columna) && fila[columna] != DBNull.Value ? Convert.ToDateTime(fila[columna]) : DateTime.Today;
    }
}
