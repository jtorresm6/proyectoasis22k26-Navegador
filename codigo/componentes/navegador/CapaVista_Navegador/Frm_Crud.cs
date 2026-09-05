using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CapaControlador_Navegador;

namespace CapaVista_Navegador
{
    public partial class Frm_Crud : Form
    {
        private string Prv_nombreTabla = "tbl_empleados";
        private cls_ControladorNavegador Prv_controlador = new cls_ControladorNavegador();

        public Frm_Crud()
        {
            InitializeComponent();
            met_CargarDatos();
        }

        private void met_CargarDatos()
        {
            try
            {
                DataTable dt = Prv_controlador.met_LlenarDgv(Prv_nombreTabla);
                Dgv_datos.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- MÉTODO PARA RECOLECTAR DATOS (se implementará después con campos dinámicos) ---
        private Dictionary<string, string> met_RecolectarDatos()
        {

            return new Dictionary<string, string>();
        }

        // --- GUARDAR (con validación) ---
        private void Btn_Guardar_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> datos = met_RecolectarDatos();


                if (datos.Count == 0)
                {
                    MessageBox.Show("No hay datos para guardar. Los campos aún no están implementados.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                List<string> errores = Prv_controlador.met_ValidarRegistro(datos, Prv_nombreTabla);

                if (errores.Count > 0)
                {
                    MessageBox.Show("Errores:\n" + string.Join("\n", errores), "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show("Datos válidos. Registro guardado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                met_CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- CONSULTAR (refrescar grid) ---
        private void Btn_Consultar_Click(object sender, EventArgs e)
        {
            met_CargarDatos();
        }
    }
}