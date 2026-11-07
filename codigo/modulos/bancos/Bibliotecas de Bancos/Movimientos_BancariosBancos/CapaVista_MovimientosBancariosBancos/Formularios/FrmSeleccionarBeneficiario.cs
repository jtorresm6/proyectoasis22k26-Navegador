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
    public partial class FrmSeleccionarBeneficiario : Form
    {
        public long IdBeneficiarioSeleccionado { get; private set; }
        public string CodigoBeneficiarioSeleccionado { get; private set; }
        public string NombreBeneficiarioSeleccionado { get; private set; }

        private readonly ClsModeloBeneficiario _Controlador = new ClsModeloBeneficiario();
        private DataTable _TablaBeneficiarios;

        private readonly string _TipoBeneficiario;
        public FrmSeleccionarBeneficiario(string TipoBeneficiario)
        {
            InitializeComponent();
            _TipoBeneficiario = (TipoBeneficiario ?? "").Trim().ToUpperInvariant();
        }

        private void BancosDgbBeneficiarioBuscar_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow Fila =
                BancosDgbBeneficiarioBuscar.Rows[e.RowIndex];

            DataRowView Datos = Fila.DataBoundItem as DataRowView;

            if (Datos == null) return;

            IdBeneficiarioSeleccionado =
                Convert.ToInt64(Datos["ID"]);

            CodigoBeneficiarioSeleccionado =
                Convert.ToString(Datos["Codigo"]);

            NombreBeneficiarioSeleccionado =
                Convert.ToString(Datos["Nombre"]);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BancosMetConfigurarTitulo()
        {
            string TipoTexto;

            switch (_TipoBeneficiario)
            {
                case "PROVEEDOR":
                    TipoTexto = "Proveedor";
                    break;

                case "CLIENTE":
                    TipoTexto = "Cliente";
                    break;

                case "EMPLEADO":
                    TipoTexto = "Empleado";
                    break;

                default:
                    TipoTexto = "Desconocido";
                    break;
            }

            BancosLblBeneficiarioFiltrar.Text =
                $"Ingrese un Beneficiario ({TipoTexto}) a Filtrar";
        }

        private void BancosMetCargarBeneficiarios()
        {   
            _TablaBeneficiarios = _Controlador.BancosMetObtenerBeneficiariosTabla(_TipoBeneficiario);

            BancosDgbBeneficiarioBuscar.AutoGenerateColumns = true;
            BancosDgbBeneficiarioBuscar.DataSource = _TablaBeneficiarios;
            BancosDgbBeneficiarioBuscar.ReadOnly = true;
            BancosDgbBeneficiarioBuscar.AllowUserToAddRows = false;
            BancosDgbBeneficiarioBuscar.AllowUserToDeleteRows = false;
            BancosDgbBeneficiarioBuscar.MultiSelect = false;

            BancosDgbBeneficiarioBuscar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            BancosDgbBeneficiarioBuscar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void FrmSeleccionarBeneficiario_Load(object sender, EventArgs e)
        {
            try
            {
                BancosMetConfigurarTitulo();
                BancosMetCargarBeneficiarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar beneficiarios: " + ex.Message);
            }
        }
    }
}
