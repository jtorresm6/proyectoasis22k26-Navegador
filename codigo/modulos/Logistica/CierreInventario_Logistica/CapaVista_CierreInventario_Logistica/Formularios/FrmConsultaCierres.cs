/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA
 * FORMULARIO: FrmConsultaCierre
 *
 * DESCRIPCIÓN:
 * Formulario que aborda la interfafz del cierre de inventario
 *
 * RESPONSABLE: Nills Berducido Gómez
 * CARNÉ: 0901 23 14275
 * ============================================================================
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_CierreInventario_Logistica;

namespace CapaVista_CierreInventario_Logistica.Formularios
{
    public partial class FrmConsultaCierres : Form
    {
        // ============================================================
        // CONTROLADOR
        // ============================================================

        private readonly ClsControladorCierreInventario controlador =
            new ClsControladorCierreInventario();

        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public FrmConsultaCierres()
        {
            InitializeComponent();

            Load += FrmConsultaCierres_Load;

            LogisticaBtnBuscar.Click -= LogisticaBtnBuscar_Click;
            LogisticaBtnBuscar.Click += LogisticaBtnBuscar_Click;
        }

        // ============================================================
        // CARGAR FORMULARIO
        // ============================================================

        private void FrmConsultaCierres_Load(object sender, EventArgs e)
        {
            try
            {
                // Configurar sucursales
                LogisticaCboSucursal.DropDownStyle =
                    ComboBoxStyle.DropDownList;

                DataTable sucursales =
                    controlador.ObtenerSucursales();

                // Agregar opción para consultar todas las sucursales
                DataRow fila = sucursales.NewRow();

                fila["idSucursal"] = 0;
                fila["nombreSucursal"] = "Todas las sucursales";

                sucursales.Rows.InsertAt(fila, 0);

                LogisticaCboSucursal.DisplayMember =
                    "nombreSucursal";

                LogisticaCboSucursal.ValueMember =
                    "idSucursal";

                LogisticaCboSucursal.DataSource =
                    sucursales;

                LogisticaCboSucursal.SelectedIndex = 0;

                // Configurar fechas
                LogisticaDtpFechaInicialCierre.Format =
                    DateTimePickerFormat.Short;

                LogisticaDtpFechaFinalCierre.Format =
                    DateTimePickerFormat.Short;

                LogisticaDtpFechaInicialCierre.Value =
                    new DateTime(DateTime.Today.Year, 1, 1);

                LogisticaDtpFechaFinalCierre.Value =
                    DateTime.Today;

                // Configurar tabla
                LogisticaDvgCierre.ReadOnly = true;

                LogisticaDvgCierre.AllowUserToAddRows = false;

                LogisticaDvgCierre.AllowUserToDeleteRows = false;

                LogisticaDvgCierre.SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect;

                LogisticaDvgCierre.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                LogisticaDvgCierre.MultiSelect = false;

                LogisticaDvgCierre.DataSource = null;
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        // ============================================================
        // BOTÓN BUSCAR
        // ============================================================

        private void LogisticaBtnBuscar_Click(
            object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicial =
                    LogisticaDtpFechaInicialCierre.Value.Date;

                DateTime fechaFinal =
                    LogisticaDtpFechaFinalCierre.Value.Date;

                if (fechaInicial > fechaFinal)
                {
                    MessageBox.Show(
                        "La fecha inicial no puede ser mayor que la fecha final.",
                        "Consulta de Cierres",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                int? sucursal = null;

                if (LogisticaCboSucursal.SelectedValue != null)
                {
                    int codigo = Convert.ToInt32(
                        LogisticaCboSucursal.SelectedValue);

                    if (codigo > 0)
                    {
                        sucursal = codigo;
                    }
                }

                string nombre =
                    LogisticaTxtNombreCierre.Text.Trim();

                DataTable resultado =
                    controlador.ConsultarCierres(
                        sucursal,
                        fechaInicial,
                        fechaFinal,
                        nombre);

                LogisticaDvgCierre.DataSource = resultado;

                if (resultado.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "No se encontraron cierres de inventario con los filtros seleccionados.",
                        "Consulta de Cierres",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        // ============================================================
        // MOSTRAR ERRORES
        // ============================================================

        private void MostrarError(Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Consulta de Cierres",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }


    }
}

