using CapaControlador_Nomina.Modelos_de_controladores;
//using CapaModelo_Nomina.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_Nomina.Formularios
{
    public partial class FrmReporteNomina : Form
    {
        private readonly ClsModeloReporteNomina
        _RrhhReporte = new ClsModeloReporteNomina();

        private bool _Cargando;

        public FrmReporteNomina()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _Cargando = true;

            try
            {
                RrhhMetConfigurarControles();
                RrhhMetConfigurarGrid();

                DateTime PrimerDiaMes = new DateTime(
                    DateTime.Today.Year,
                    DateTime.Today.Month,
                    1);

                RrhhDtpFechaInicio.Value = PrimerDiaMes;

                RrhhDtpFechaFin.Value =
                    PrimerDiaMes.AddMonths(1).AddDays(-1);

                RrhhMetConectarEventos();
            }
            finally
            {
                _Cargando = false;
            }

            RrhhMetConsultar();
        }

        private void RrhhMetConfigurarControles()
        {
            RrhhDtpFechaInicio.Format =
                DateTimePickerFormat.Custom;

            RrhhDtpFechaInicio.CustomFormat = "dd/MM/yyyy";

            RrhhDtpFechaFin.Format =
                DateTimePickerFormat.Custom;

            RrhhDtpFechaFin.CustomFormat = "dd/MM/yyyy";

            TextBox[] Totales =
            {
                RrhhTxtTotalPercepciones,
                RrhhTxtTotalDeducciones,
                RrhhTxtTotalNeto
            };

            foreach (TextBox Caja in Totales)
            {
                Caja.ReadOnly = true;
                Caja.TextAlign = HorizontalAlignment.Right;
                Caja.TabStop = false;
                Caja.Text = "0.00";
            }

            RrhhBtnConsultar.Text = "Consultar";
            RrhhBtnRefrescar.Text = "Refrescar";
            RrhhBtnSalir.Text = "Salir";
        }

        private void RrhhMetConectarEventos()
        {
            RrhhBtnConsultar.Click -=
                RrhhBtnConsultar_Click;

            RrhhBtnConsultar.Click +=
                RrhhBtnConsultar_Click;

            RrhhBtnRefrescar.Click -=
                RrhhBtnRefrescar_Click;

            RrhhBtnRefrescar.Click +=
                RrhhBtnRefrescar_Click;

            RrhhBtnSalir.Click -=
                RrhhBtnSalir_Click;

            RrhhBtnSalir.Click +=
                RrhhBtnSalir_Click;

            RrhhDtpFechaInicio.ValueChanged -=
                RrhhDtpFecha_ValueChanged;

            RrhhDtpFechaInicio.ValueChanged +=
                RrhhDtpFecha_ValueChanged;

            RrhhDtpFechaFin.ValueChanged -=
                RrhhDtpFecha_ValueChanged;

            RrhhDtpFechaFin.ValueChanged +=
                RrhhDtpFecha_ValueChanged;
        }

        private void RrhhMetConfigurarGrid()
        {
            var Grid = RrhhDgvListaReporte;

            Grid.DataSource = null;
            Grid.AutoGenerateColumns = false;
            Grid.Columns.Clear();

            Grid.AllowUserToAddRows = false;
            Grid.AllowUserToDeleteRows = false;
            Grid.AllowUserToOrderColumns = true;
            Grid.AllowUserToResizeColumns = true;

            Grid.ReadOnly = true;
            Grid.MultiSelect = false;

            Grid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            Grid.ScrollBars = ScrollBars.Both;
            Grid.RowHeadersVisible = false;

            Grid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.None;

            Grid.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            Grid.ColumnHeadersDefaultCellStyle.WrapMode =
                DataGridViewTriState.True;

            Grid.EnableHeadersVisualStyles = false;

            Grid.BackgroundColor =
                ColorTranslator.FromHtml("#05071A");

            Grid.ColumnHeadersDefaultCellStyle.BackColor =
                ColorTranslator.FromHtml("#8A57E7");

            Grid.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            Grid.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                ColorTranslator.FromHtml("#8A57E7");

            Grid.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                Color.White;

            Grid.DefaultCellStyle.BackColor =
                ColorTranslator.FromHtml("#0A0E2B");

            Grid.DefaultCellStyle.ForeColor =
                Color.White;

            Grid.DefaultCellStyle.SelectionBackColor =
                ColorTranslator.FromHtml("#628FFE");

            Grid.DefaultCellStyle.SelectionForeColor =
                Color.White;

            Grid.GridColor =
                ColorTranslator.FromHtml("#797CFE");

            RrhhMetAgregarColumna(
                "IdNominaEncabezado",
                "Nómina",
                75);

            RrhhMetAgregarColumna(
                "IdEmpleado",
                "Código empleado",
                85);

            RrhhMetAgregarColumna(
                "NombreEmpleado",
                "Empleado",
                230);

            RrhhMetAgregarColumna(
                "FechaInicio",
                "Fecha inicio",
                100,
                "dd/MM/yyyy");

            RrhhMetAgregarColumna(
                "FechaFinal",
                "Fecha final",
                100,
                "dd/MM/yyyy");

            RrhhMetAgregarColumna(
                "SalarioBase",
                "Salario base",
                115,
                "N2");

            RrhhMetAgregarColumna(
                "TotalPercepciones",
                "Percepciones",
                115,
                "N2");

            RrhhMetAgregarColumna(
                "TotalDeducciones",
                "Deducciones",
                115,
                "N2");

            RrhhMetAgregarColumna(
                "TotalNeto",
                "Neto a pagar",
                115,
                "N2");

            RrhhMetAgregarColumna(
                "EstadoNomina",
                "Estado",
                100);
        }

        private void RrhhMetAgregarColumna(
            string Propiedad,
            string Encabezado,
            int Ancho,
            string Formato = null)
        {
            var Columna = new DataGridViewTextBoxColumn
            {
                Name = Propiedad,
                DataPropertyName = Propiedad,
                HeaderText = Encabezado,
                Width = Ancho,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            if (Formato != null)
            {
                Columna.DefaultCellStyle.Format = Formato;
            }

            if (Formato == "N2")
            {
                Columna.DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;
            }
            else if (Formato == "dd/MM/yyyy")
            {
                Columna.DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
            }

            RrhhDgvListaReporte.Columns.Add(Columna);
        }

        private void RrhhMetLimpiarConsulta()
        {
            RrhhDgvListaReporte.DataSource = null;

            RrhhTxtTotalPercepciones.Text = "0.00";
            RrhhTxtTotalDeducciones.Text = "0.00";
            RrhhTxtTotalNeto.Text = "0.00";
        }

        private void RrhhMetMostrarTotales(
            List<ClsReporteNomina> Lista)
        {
            RrhhTxtTotalPercepciones.Text =
                Lista.Sum(Nomina => Nomina.TotalPercepciones)
                    .ToString("N2");

            RrhhTxtTotalDeducciones.Text =
                Lista.Sum(Nomina => Nomina.TotalDeducciones)
                    .ToString("N2");

            RrhhTxtTotalNeto.Text =
                Lista.Sum(Nomina => Nomina.TotalNeto)
                    .ToString("N2");
        }

        private void RrhhMetConsultar()
        {
            if (_Cargando)
                return;

            _Cargando = true;

            RrhhBtnConsultar.Enabled = false;
            RrhhBtnRefrescar.Enabled = false;

            try
            {
                RrhhMetLimpiarConsulta();

                DateTime FechaInicio =
                    RrhhDtpFechaInicio.Value.Date;

                DateTime FechaFinal =
                    RrhhDtpFechaFin.Value.Date;

                if (FechaFinal < FechaInicio)
                {
                    MessageBox.Show(
                        this,
                        "La fecha final no puede ser menor " +
                        "a la fecha inicial.",
                        "Período inválido",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    RrhhDtpFechaFin.Focus();
                    return;
                }

                UseWaitCursor = true;

                List<ClsReporteNomina> Lista =
                    _RrhhReporte.RrhhMetObtenerPorPeriodo(
                        FechaInicio,
                        FechaFinal);

                RrhhDgvListaReporte.DataSource = Lista;

                RrhhMetMostrarTotales(Lista);

                RrhhDgvListaReporte.ClearSelection();

                if (Lista.Count == 0)
                {
                    UseWaitCursor = false;

                    MessageBox.Show(
                        this,
                        "No hay nóminas aprobadas o pagadas " +
                        "comprendidas en las fechas seleccionadas.",
                        "Consulta de nóminas",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception Ex)
            {
                RrhhMetLimpiarConsulta();
                UseWaitCursor = false;

                MessageBox.Show(
                    this,
                    "No se pudo consultar la nómina: " +
                    Ex.Message,
                    "Consulta de nóminas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;

                RrhhBtnConsultar.Enabled = true;
                RrhhBtnRefrescar.Enabled = true;

                _Cargando = false;
            }
        }

        private void RrhhBtnConsultar_Click(object sender, EventArgs e)
        {
            RrhhMetConsultar();
        }

        private void RrhhBtnRefrescar_Click(object sender, EventArgs e)
        {
            RrhhMetConsultar();
        }

        private void RrhhBtnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void RrhhDtpFecha_ValueChanged(
            object sender,
            EventArgs e)
        {
            if (_Cargando)
                return;


            RrhhMetLimpiarConsulta();
        }
    }
}
