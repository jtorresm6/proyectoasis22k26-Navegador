using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace CapaVista_DisponibilidadDiaria.Formularios.frmReportes
{
    public partial class FrmReporteDisponibilidadDiaria : Form
    {
        private readonly DataTable _tabla;
        private readonly string _titulo;
        private readonly PrintDocument _documento = new PrintDocument();

        public FrmReporteDisponibilidadDiaria(DataTable tabla, string titulo)
        {
            _tabla = tabla ?? throw new ArgumentNullException(nameof(tabla));
            _titulo = titulo ?? "Reporte";
            InitializeComponent();
            _documento.BeginPrint += Documento_BeginPrint;
            _documento.PrintPage += Documento_PrintPage;
        }

        private void Documento_BeginPrint(object sender, PrintEventArgs e)
        {
            // reset any state if needed
        }

        private void Documento_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Simplified print logic: draw table as text
            var fuente = new Font("Arial", 8);
            var negrita = new Font("Arial", 8, FontStyle.Bold);
            var tituloFuente = new Font("Arial", 12, FontStyle.Bold);
            var margen = e.MarginBounds;
            float y = margen.Top;
            e.Graphics.DrawString(_titulo, tituloFuente, Brushes.Black, margen.Left, y);
            y += 30;
            // header
            float x = margen.Left;
            foreach (DataColumn col in _tabla.Columns)
            {
                e.Graphics.DrawString(col.ColumnName, negrita, Brushes.Black, x, y);
                x += 100; // fixed width for simplicity
            }
            y += 20;
            // rows
            foreach (DataRow row in _tabla.Rows)
            {
                x = margen.Left;
                foreach (var item in row.ItemArray)
                {
                    string texto = item == null || item == DBNull.Value ? "-" : item.ToString();
                    e.Graphics.DrawString(texto, fuente, Brushes.Black, x, y);
                    x += 100;
                }
                y += 18;
                if (y > margen.Bottom - 40)
                {
                    e.HasMorePages = true;
                    return;
                }
            }
            e.HasMorePages = false;
        }

        private void MostrarVistaPrevia()
        {
            using var vista = new PrintPreviewDialog { Document = _documento, Width = 1100, Height = 750 };
            vista.ShowDialog(this);
        }

        private void BtnVistaPrevia_Click(object sender, EventArgs e)
        {
            MostrarVistaPrevia();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FrmReporteDisponibilidadDiaria
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "FrmReporteDisponibilidadDiaria";
            this.Text = _titulo;
            var btn = new Button
            {
                Text = "Vista Previa",
                Location = new System.Drawing.Point(10, 10),
                AutoSize = true
            };
            btn.Click += BtnVistaPrevia_Click;
            this.Controls.Add(btn);
            this.ResumeLayout(false);
        }
    }
}
