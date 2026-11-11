using System;
using System.Drawing;
using System.Windows.Forms;
using CapaControlador_EnlaceB;
using CapaModelo_EnlaceB.Entidades;

namespace CapaVista_EnlaceB.Formularios
{
    public partial class FrmEnlaceOtrosModulos : Form
    {
        private readonly ClsControladorEnlaceB _Controlador;
        public FrmEnlaceOtrosModulos() : this(new ClsControladorEnlaceB()) { }
        public FrmEnlaceOtrosModulos(ClsControladorEnlaceB Controlador)
        {
            this._Controlador = Controlador ?? throw new ArgumentNullException(nameof(Controlador));
            InitializeComponent();
            foreach (Control Elemento in Controls) Elemento.Visible = false;
            ClientSize = new Size(1050, 690); AutoScaleMode = AutoScaleMode.None;
            Text = "8008 – EnlaceOtrosModulos"; StartPosition = FormStartPosition.CenterParent;
            BackgroundImage = Properties.Resources.FondoEnlaceotrosModulo; BackgroundImageLayout = ImageLayout.Stretch;
            var BancarioTlpLayout = new TableLayoutPanel { Dock = DockStyle.Fill, BackColor = Color.Transparent,
                Padding = new Padding(20), ColumnCount = 2, RowCount = 3 };
            BancarioTlpLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72)); BancarioTlpLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28));
            BancarioTlpLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 72)); BancarioTlpLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); BancarioTlpLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 94));
            var BancarioLblTitulo = new Label { Text = "Enlace con otros módulos", ForeColor = ClsEstiloEnlace.Verde,
                Font = new Font("Lucida Bright", 26, FontStyle.Bold), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            BancarioTlpLayout.Controls.Add(BancarioLblTitulo, 0, 0); BancarioTlpLayout.SetColumnSpan(BancarioLblTitulo, 2);
            var BancarioTlpLista = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 5, BackColor = Color.Transparent };
            BancarioTlpLista.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130)); BancarioTlpLista.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            string[] Nombres = { "Logística", "Compras", "Ventas", "Recursos Humanos", "Contabilidad" };
            string[] Detalles = { "Solicitudes de transporte, costos y órdenes bancarias.", "Órdenes de compra y recepción en Bancos.",
                "Depósitos y movimientos bancarios asociados.", "Planillas recibidas, beneficiarios y órdenes de pago.", "Partidas mensuales y traslado a Contabilidad." };
            for (int Indice = 0; Indice < Nombres.Length; Indice++)
            {
                int Modulo = Indice; BancarioTlpLista.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
                var BancarioBtnBoton = ClsEstiloEnlace.BancarioMetBoton(Nombres[Indice], (Remitente, Argumentos) => BancarioMetAbrir(Modulo));
                BancarioBtnBoton.Size = new Size(90, 90);
                BancarioBtnBoton.Anchor = AnchorStyles.Left; BancarioBtnBoton.Margin = new Padding(0, 2, 18, 2); BancarioBtnBoton.AccessibleName = "Abrir " + Nombres[Indice];
                BancarioTlpLista.Controls.Add(BancarioBtnBoton, 0, Indice);
                BancarioTlpLista.Controls.Add(new Label { Text = Detalles[Indice], Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft,
                    ForeColor = ClsEstiloEnlace.Verde, Font = new Font("Segoe UI", 11), Padding = new Padding(4) }, 1, Indice);
            }
            BancarioTlpLayout.Controls.Add(BancarioTlpLista, 0, 1);
            BancarioTlpLayout.Controls.Add(new PictureBox { Image = Properties.Resources.Mascota_bancaria, SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill, Margin = new Padding(20, 20, 0, 20), BackColor = Color.Transparent }, 1, 1);
            var BancarioBtnVolver = ClsEstiloEnlace.BancarioMetBoton("Cerrar", (Remitente, Argumentos) => Close()); BancarioBtnVolver.Anchor = AnchorStyles.Right;
            BancarioTlpLayout.Controls.Add(BancarioBtnVolver, 1, 2);
            BancarioTlpLayout.Controls.Add(new Label { Text = "Seleccione el módulo que desea consultar.", Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft, ForeColor = ClsEstiloEnlace.Verde, Font = new Font("Segoe UI", 10) }, 0, 2);
            Controls.Add(BancarioTlpLayout); BancarioTlpLayout.BringToFront();
            ClsEstiloEnlace.BancarioMetVentana(this);
        }
        private void BancarioMetAbrir(int Modulo)
        {
            if (Modulo == 4) { using (var Formulario = new FrmEnlaceContabilidad(_Controlador)) Formulario.ShowDialog(this); }
            else { using (var Formulario = new FrmEnlaceModulo((ModuloEnlace)Modulo, _Controlador)) Formulario.ShowDialog(this); }
        }
        private void BancarioMetBancDgvPartidaContable_CellContentClick(object Sender, DataGridViewCellEventArgs Argumentos) { }
    }
}
