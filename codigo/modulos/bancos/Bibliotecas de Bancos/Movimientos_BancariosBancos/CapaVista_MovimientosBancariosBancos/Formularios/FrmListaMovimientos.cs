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
    public partial class FrmListaMovimientos : Form
    {
        private readonly ClsModeloMovimientoBancario
            _ControladorMovimiento =
                new ClsModeloMovimientoBancario();

        public FrmListaMovimientos()
        {
            InitializeComponent();

            this.Load += FrmListaMovimientos_Load;
        }

        private void FrmListaMovimientos_Load(
            object sender, EventArgs e)
        {
            try
            {
                BancosDgvMovimientos.DataSource =
                    _ControladorMovimiento
                        .BancosMetObtenerMovimientosTabla();

                BancosDgvMovimientos.ReadOnly = true;

                BancosDgvMovimientos.AllowUserToAddRows = false;

                BancosDgvMovimientos.AllowUserToDeleteRows = false;

                BancosDgvMovimientos.SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect;

                BancosDgvMovimientos.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                if (BancosDgvMovimientos.Columns["Monto"] != null)
                {
                    BancosDgvMovimientos.Columns["Monto"]
                        .DefaultCellStyle.Format = "N2";
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar los movimientos:\n" +
                    Ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
