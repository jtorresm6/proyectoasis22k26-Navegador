
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaControlador_TransporteExterno_Logistica;

namespace CapaVista_TransporteExterno_Logistica.Formularios
{
    public partial class FrmListadoTransporteExterno : Form
    {
        private readonly ClsControladorTransporteExterno _Controlador;

        public FrmListadoTransporteExterno()
        {
            InitializeComponent();

            _Controlador = new ClsControladorTransporteExterno();

            this.Load += ListadoTransporteExterno_Load;
            this.Activated += ListadoTransporteExterno_Activated;
        }

        private void ListadoTransporteExterno_Load(object sender, EventArgs e)
        {
            LogisticaMetConfigurarListado();
            LogisticaMetCargarTransportes();
        }

        private void ListadoTransporteExterno_Activated(object sender, EventArgs e)
        {
            LogisticaMetCargarTransportes();
        }

        private void LogisticaMetConfigurarListado()
        {
            DataGridView dgv = LogisticaDvglListadoTransportes;

            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.RowTemplate.Height = 42;

            dgv.BackgroundColor = Color.FromArgb(251, 246, 242);
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(225, 218, 208);

            dgv.EnableHeadersVisualStyles = false;

            dgv.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(35, 87, 91);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font =
                new Font("Tahoma", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8);
            dgv.ColumnHeadersDefaultCellStyle.WrapMode =
                DataGridViewTriState.True;

            dgv.ColumnHeadersHeight = 48;
            dgv.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(55, 55, 55);
            dgv.DefaultCellStyle.Font =
                new Font("Tahoma", 9.5F, FontStyle.Regular);
            dgv.DefaultCellStyle.Padding = new Padding(8, 5, 8, 5);
            dgv.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgv.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(245, 240, 232);

            dgv.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(215, 229, 226);
            dgv.DefaultCellStyle.SelectionForeColor =
                Color.FromArgb(35, 55, 55);

            dgv.RowHeadersVisible = false;
        }

        public void LogisticaMetCargarTransportes()
        {
            try
            {
                DataTable tabla =
                    _Controlador.LogisticaFuncObtenerTransportes();

                LogisticaDvglListadoTransportes.DataSource = null;
                LogisticaDvglListadoTransportes.DataSource = tabla;

                LogisticaMetConfigurarListado();

                foreach (DataGridViewColumn columna in
                    LogisticaDvglListadoTransportes.Columns)
                {
                    columna.SortMode =
                        DataGridViewColumnSortMode.Automatic;
                }

                LogisticaDvglListadoTransportes.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.AllCells;

                LogisticaDvglListadoTransportes.AutoResizeColumns(
                    DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar el listado de transportes:\n" +
                    ex.Message,
                    "Transporte Externo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LogisticaDvglListadoTransportes_CellContentClick(
            object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
