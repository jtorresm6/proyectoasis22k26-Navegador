/*Oscar Emilio Morales Lemus; 9959-23-3070*/
using CapaControlador_MovimientosBancarios;
using CapaControlador_MovimientosBancarios.Modelo_de_Controladores;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_MovimientosBancarios.Formularios
{
    public partial class FrmSeleccionarConcepto : Form
    {
        private readonly ClsModeloConceptoBancario _Controlador = new ClsModeloConceptoBancario();
        public FrmSeleccionarConcepto()
        {
            InitializeComponent();
            this.Load += FrmSeleccionarConcepto_Load;
        }

        private void FrmSeleccionarConcepto_Load(object sender, EventArgs e)
        {
            try
            {
                BancosMetCargarConceptos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los conceptos: " + ex.Message, "Movimientos Bancarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable _TablaConceptos;
        private void BancosMetCargarConceptos()
        {
            _TablaConceptos = _Controlador.BancosMetObtenerConceptosTabla();

            BancosDgbConceptoBuscar.DataSource = _TablaConceptos;
            BancosDgbConceptoBuscar.ReadOnly = true;
            BancosDgbConceptoBuscar.AllowUserToAddRows = false;
            BancosDgbConceptoBuscar.AllowUserToDeleteRows = false;
            BancosDgbConceptoBuscar.MultiSelect = false;
            BancosDgbConceptoBuscar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            BancosDgbConceptoBuscar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void BancoTxtConceptoBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_TablaConceptos == null)
                    return;
                string Texto = BancoTxtConceptoBuscar.Text.Trim();
                if (string.IsNullOrWhiteSpace(Texto))
                {
                    _TablaConceptos.DefaultView.RowFilter = "";
                    return;
                }
                Texto = Texto.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]");
                _TablaConceptos.DefaultView.RowFilter = $"Convert(ID, 'System.String') LIKE '%{Texto}%' OR " + $"Codigo LIKE '%{Texto}%' OR " + $"Concepto LIKE '%{Texto}%'";
            }
            catch (EvaluateException)
            {
                _TablaConceptos.DefaultView.RowFilter = "";
            }
        }

        public long IdConceptoSeleccionado { get; private set; }
        public string NombreConceptoSeleccionado { get; private set; }
        public string DescripcionSeleccionada { get; private set; }
        public string NaturalezaSeleccionada { get; private set; }
        public string TipoBeneficiarioSeleccionado { get; private set; }

        private void BancosDgbConceptoBuscar_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow Fila = BancosDgbConceptoBuscar.Rows[e.RowIndex];
            IdConceptoSeleccionado = Convert.ToInt64(Fila.Cells["ID"].Value);
            NombreConceptoSeleccionado = Convert.ToString(Fila.Cells["Concepto"].Value);
            DescripcionSeleccionada = Convert.ToString(Fila.Cells["Descripcion"].Value);
            NaturalezaSeleccionada = Convert.ToString(Fila.Cells["Naturaleza"].Value);
            TipoBeneficiarioSeleccionado = Convert.ToString(Fila.Cells["TipoTercero"].Value);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
