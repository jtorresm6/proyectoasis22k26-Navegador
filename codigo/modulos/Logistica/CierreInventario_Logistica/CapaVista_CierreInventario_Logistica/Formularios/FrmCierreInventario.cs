/* ============================================================================
 * MÓDULO: LOGÍSTICA | FORMULARIO: FrmCierreInventario
 * DESCRIPCIÓN: Validación y ejecución de cierre mensual mediante MVC.
 * RESPONSABLE: Nills Berducido Gómez
 * ============================================================================ */
using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using CapaControlador_CierreInventario_Logistica;
using CapaModelo_CierreInventario_Logistica.Entidad;


namespace CapaVista_CierreInventario_Logistica.Formularios
{
    public partial class FrmCierreInventario : Form
    {
        private readonly ClsControladorCierreInventario controlador =
            new ClsControladorCierreInventario();

        private bool periodoValidado = false;

        private bool cargandoFormulario = false;

        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public FrmCierreInventario()
        {
            InitializeComponent();

            Load += FrmCierreInventario_Load;

            LogisticaBtnValidarPeriodo.Click +=
                LogisticaBtnValidarPeriodo_Click;

            LogisticaBtnConfirmarCierre.Click +=
                LogisticaBtnConfirmarCierre_Click;

            LogisticaBtnLimpiar.Click +=
                LogisticaBtnLimpiar_Click;

            LogisticaCboSucursal.SelectedIndexChanged +=
                Seleccion_Modificada;

            LogisticaCboMesCierre.SelectedIndexChanged +=
                Seleccion_Modificada;

            LogisticaCboAñoCierre.SelectedIndexChanged +=
                Seleccion_Modificada;

            LogisticaBtnConfirmarCierre.Enabled = false;
        }

        // ============================================================
        // CARGAR FORMULARIO
        // ============================================================

        private void FrmCierreInventario_Load(
            object sender, EventArgs e)
        {
            cargandoFormulario = true;

            try
            {
                // Configurar ComboBox

                LogisticaCboSucursal.DropDownStyle =
                    ComboBoxStyle.DropDownList;

                LogisticaCboMesCierre.DropDownStyle =
                    ComboBoxStyle.DropDownList;

                LogisticaCboAñoCierre.DropDownStyle =
                    ComboBoxStyle.DropDownList;

                // ================================================
                // CARGAR SUCURSALES
                // ================================================

                DataTable sucursales =
                    controlador.ObtenerSucursales();

                LogisticaCboSucursal.DataSource = null;

                LogisticaCboSucursal.DisplayMember =
                    "nombreSucursal";

                LogisticaCboSucursal.ValueMember =
                    "idSucursal";

                LogisticaCboSucursal.DataSource = sucursales;

                if (sucursales.Rows.Count > 0)
                {
                    LogisticaCboSucursal.SelectedIndex = 0;
                }
                else
                {
                    LogisticaCboSucursal.SelectedIndex = -1;

                    MessageBox.Show(
                        "No existen sucursales registradas " +
                        "en la base de datos configurada.",
                        "Cierre de Inventario",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }

                // ================================================
                // CARGAR MESES
                // ================================================

                LogisticaCboMesCierre.Items.Clear();

                CultureInfo cultura =
                    new CultureInfo("es-GT");

                for (int mes = 1; mes <= 12; mes++)
                {
                    string nombreMes =
                        cultura.DateTimeFormat.GetMonthName(mes);

                    LogisticaCboMesCierre.Items.Add(nombreMes);
                }

                // ================================================
                // CARGAR AÑOS
                // ================================================

                LogisticaCboAñoCierre.Items.Clear();

                DateTime ultimoMes =
                    DateTime.Today.AddMonths(-1);

                for (int anio = DateTime.Today.Year - 5;
                     anio <= DateTime.Today.Year;
                     anio++)
                {
                    LogisticaCboAñoCierre.Items.Add(anio);
                }

                // Seleccionar el último mes finalizado.

                LogisticaCboMesCierre.SelectedIndex =
                    ultimoMes.Month - 1;

                LogisticaCboAñoCierre.SelectedItem =
                    ultimoMes.Year;

                // ================================================
                // CONFIGURAR DATAGRIDVIEW
                // ================================================

                LogisticaDgvDetalleCierre.ReadOnly = true;

                LogisticaDgvDetalleCierre.AllowUserToAddRows =
                    false;

                LogisticaDgvDetalleCierre.AllowUserToDeleteRows =
                    false;

                LogisticaDgvDetalleCierre.SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect;

                LogisticaDgvDetalleCierre.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.DisplayedCells;

                LogisticaDgvDetalleCierre.MultiSelect = false;

                ReiniciarResultados();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
            finally
            {
                cargandoFormulario = false;
            }
        }

        // ============================================================
        // CAMBIO DE SELECCIÓN
        // ============================================================

        private void Seleccion_Modificada(
            object sender, EventArgs e)
        {
            if (cargandoFormulario)
                return;

            ReiniciarResultados();
        }

        // ============================================================
        // REINICIAR RESULTADOS
        // ============================================================

        private void ReiniciarResultados()
        {
            periodoValidado = false;

            LogisticaBtnConfirmarCierre.Enabled = false;

            LogisticaDgvDetalleCierre.DataSource = null;

            LogisticaLblProductosRegistrados.Text = "0";

            LogisticaLblBodegasIncluidas.Text = "0";

            LogisticaLblMovimientoMes.Text = "0";

            LogisticaLblProductosDiferencias.Text = "0";
        }

        // ============================================================
        // VALIDAR SELECCIÓN
        // ============================================================

        private bool SeleccionValida(
            out int sucursal, out int anio, out int mes)
        {
            sucursal = 0;
            anio = 0;
            mes = 0;

            if (LogisticaCboSucursal.SelectedValue == null ||
                LogisticaCboSucursal.SelectedValue is DataRowView ||
                LogisticaCboMesCierre.SelectedIndex < 0 ||
                LogisticaCboAñoCierre.SelectedItem == null)
            {
                MessageBox.Show(
                    "Seleccione una sucursal, mes y año válidos.",
                    "Cierre de Inventario",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            try
            {
                sucursal = Convert.ToInt32(
                    LogisticaCboSucursal.SelectedValue);

                mes = LogisticaCboMesCierre.SelectedIndex + 1;

                anio = Convert.ToInt32(
                    LogisticaCboAñoCierre.SelectedItem);

                if (sucursal <= 0)
                {
                    MessageBox.Show(
                        "Seleccione una sucursal válida.",
                        "Cierre de Inventario",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MostrarError(ex);
                return false;
            }
        }

        // ============================================================
        // BOTÓN VALIDAR PERÍODO
        // ============================================================

        private void LogisticaBtnValidarPeriodo_Click(
            object sender, EventArgs e)
        {
            int sucursal;
            int anio;
            int mes;

            if (!SeleccionValida(
                out sucursal, out anio, out mes))
                return;

            ReiniciarResultados();

            try
            {
                ResultadoCierre resultado =
                    controlador.ValidarPeriodo(
                        sucursal, anio, mes);

                // Mostrar los registros de inventario.

                if (resultado.Detalle != null)
                {
                    LogisticaDgvDetalleCierre.DataSource =
                        resultado.Detalle;
                }

                // Ocultar identificadores internos.

                if (LogisticaDgvDetalleCierre.Columns
                    .Contains("idStock"))
                {
                    LogisticaDgvDetalleCierre
                        .Columns["idStock"].Visible = false;
                }

                if (LogisticaDgvDetalleCierre.Columns
                    .Contains("IdBodega"))
                {
                    LogisticaDgvDetalleCierre
                        .Columns["IdBodega"].Visible = false;
                }

                if (LogisticaDgvDetalleCierre.Columns
                    .Contains("IdProducto"))
                {
                    LogisticaDgvDetalleCierre
                        .Columns["IdProducto"].Visible = false;
                }

                // ================================================
                // ACTUALIZAR INDICADORES
                // ================================================

                LogisticaLblProductosRegistrados.Text =
                    resultado.Productos.ToString();

                LogisticaLblBodegasIncluidas.Text =
                    resultado.Bodegas.ToString();

                LogisticaLblMovimientoMes.Text =
                    resultado.Movimientos.ToString();

                LogisticaLblProductosDiferencias.Text =
                    resultado.Diferencias.ToString();

                // ================================================
                // COMPROBAR RESULTADO
                // ================================================

                periodoValidado = resultado.PuedeCerrar;

                LogisticaBtnConfirmarCierre.Enabled =
                    periodoValidado;

                if (resultado.Detalle == null ||
                    resultado.Detalle.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "No se encontraron existencias de " +
                        "inventario para la sucursal seleccionada." +
                        "\n\nVerifique que tblStock tenga registros " +
                        "asociados a esa sucursal.",
                        "Inventario sin datos",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                if (resultado.PuedeCerrar)
                {
                    MessageBox.Show(
                        "Se encontraron " +
                        resultado.Detalle.Rows.Count +
                        " registros de inventario." +
                        "\n\nEl período está cuadrado." +
                        "\nPuede confirmar el cierre.",
                        "Validación exitosa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "Se encontraron " +
                        resultado.Detalle.Rows.Count +
                        " registros de inventario." +
                        "\n\nNo se puede confirmar el cierre:" +
                        "\n" + resultado.Mensaje,
                        "Validación del cierre",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                ReiniciarResultados();
                MostrarError(ex);
            }
        }

        // ============================================================
        // BOTÓN CONFIRMAR CIERRE
        // ============================================================

        private void LogisticaBtnConfirmarCierre_Click(
            object sender, EventArgs e)
        {
            if (!periodoValidado)
            {
                MessageBox.Show(
                    "Primero debe validar correctamente el período.",
                    "Cierre de Inventario",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            int sucursal;
            int anio;
            int mes;

            if (!SeleccionValida(
                out sucursal, out anio, out mes))
                return;

            DialogResult respuesta = MessageBox.Show(
                "El cierre de inventario es DEFINITIVO." +
                "\nNo podrá modificarse ni anularse." +
                "\n\n¿Desea realizar el cierre?",
                "Confirmar Cierre",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (respuesta != DialogResult.Yes)
                return;

            LogisticaBtnConfirmarCierre.Enabled = false;

            try
            {
                // El modelo valida nuevamente
                // dentro de la transacción.

                int codigo =
                    controlador.ConfirmarCierre(
                        sucursal, anio, mes);

                MessageBox.Show(
                    "El cierre de inventario se realizó " +
                    "correctamente." +
                    "\n\nCódigo de cierre: " + codigo,
                    "Cierre realizado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ReiniciarResultados();
            }
            catch (Exception ex)
            {
                ReiniciarResultados();
                MostrarError(ex);
            }
        }

        // ============================================================
        // BOTÓN LIMPIAR
        // ============================================================

        private void LogisticaBtnLimpiar_Click(
            object sender, EventArgs e)
        {
            cargandoFormulario = true;

            try
            {
                ReiniciarResultados();

                if (LogisticaCboSucursal.Items.Count > 0)
                {
                    LogisticaCboSucursal.SelectedIndex = 0;
                }

                DateTime ultimoMes =
                    DateTime.Today.AddMonths(-1);

                LogisticaCboMesCierre.SelectedIndex =
                    ultimoMes.Month - 1;

                LogisticaCboAñoCierre.SelectedItem =
                    ultimoMes.Year;
            }
            finally
            {
                cargandoFormulario = false;
            }
        }

        // ============================================================
        // BOTÓN CONSULTAR CIERRES
        // ============================================================

        private void LogisticaBtnConsultarCierre_Click_1(
            object sender, EventArgs e)
        {
            using (FrmConsultaCierres consulta =
                new FrmConsultaCierres())
            {
                consulta.ShowDialog(this);
            }
        }

        // ============================================================
        // MOSTRAR ERRORES
        // ============================================================

        private void MostrarError(Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Cierre de Inventario",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
