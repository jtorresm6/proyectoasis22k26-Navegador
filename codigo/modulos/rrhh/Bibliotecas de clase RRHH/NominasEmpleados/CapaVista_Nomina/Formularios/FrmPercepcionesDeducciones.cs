using CapaControlador_Nomina.Modelos_de_controladores;
using CapaControlador_Nomina.ObjetosdeValor;
using CapaVista_Nomina.Ayudas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_Nomina.Formularios
{
    public partial class FrmPercepcionesDeducciones : Form
    {
        private ClsModeloPercepcionDeduccion _RrhhPercepcionDeduccion = new ClsModeloPercepcionDeduccion();

        private const string EFECTO_PERCEPCION = "Percepción";   // true  en BD
        private const string EFECTO_DEDUCCION = "Deducción";     // false en BD
        private bool _AjustandoAplicacion = false;
        public FrmPercepcionesDeducciones()
        {
              InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RrhhMetAjustarAPantalla();
        }

        // en pantallas de 1366x768 la ventana se reduce para que quepa completa
        private void RrhhMetAjustarAPantalla()
        {
            Rectangle Area = Screen.FromControl(this).WorkingArea;
            double Factor = Math.Min(1d, Math.Min((double)Area.Width / Width, (double)Area.Height / Height));
            if (Factor >= 1d)
                return;

            List<KeyValuePair<Control, Font>> Fuentes = new List<KeyValuePair<Control, Font>>();
            RrhhMetGuardarFuentes(this, Fuentes);

            AutoScaleMode = AutoScaleMode.None;
            Scale(new SizeF((float)Factor, (float)Factor));
            foreach (KeyValuePair<Control, Font> Par in Fuentes)
                Par.Key.Font = RrhhMetEscalarFuente(Par.Value, Factor);

            foreach (DataGridView Tabla in Fuentes.Select(Par => Par.Key).OfType<DataGridView>())
            {
                Tabla.ColumnHeadersDefaultCellStyle.Font = RrhhMetEscalarFuente(Tabla.ColumnHeadersDefaultCellStyle.Font, Factor);
                Tabla.DefaultCellStyle.Font = RrhhMetEscalarFuente(Tabla.DefaultCellStyle.Font, Factor);
            }

            if (Owner != null)
                CenterToParent();
            else
                CenterToScreen();
        }

        private void RrhhMetGuardarFuentes(Control Padre, List<KeyValuePair<Control, Font>> Fuentes)
        {
            Fuentes.Add(new KeyValuePair<Control, Font>(Padre, Padre.Font));
            foreach (Control Hijo in Padre.Controls)
                RrhhMetGuardarFuentes(Hijo, Fuentes);
        }

        private Font RrhhMetEscalarFuente(Font Original, double Factor)
        {
            if (Original == null)
                return null;
            return new Font(Original.FontFamily, (float)(Original.Size * Factor), Original.Style, Original.Unit);
        }

        private void RrhhMetConfigurarGrid()
        {
            RrhhDgvListaConceptos.AutoGenerateColumns = false;
            RrhhDgvListaConceptos.Columns.Clear();
            RrhhDgvListaConceptos.AllowUserToAddRows = false;
            RrhhDgvListaConceptos.AllowUserToDeleteRows = false;
            RrhhDgvListaConceptos.ReadOnly = true;
            RrhhDgvListaConceptos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            RrhhDgvListaConceptos.MultiSelect = false;

            RrhhDgvListaConceptos.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdPercepcionDeduccion", DataPropertyName = "IdPercepcionDeduccion", HeaderText = "Código" });
            RrhhDgvListaConceptos.Columns.Add(new DataGridViewTextBoxColumn { Name = "NombrePercepcionDeduccion", DataPropertyName = "NombrePercepcionDeduccion", HeaderText = "Nombre del Concepto", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            RrhhDgvListaConceptos.Columns.Add(new DataGridViewTextBoxColumn { Name = "PeriodoPercepcionDeduccion", DataPropertyName = "PeriodoPercepcionDeduccion", HeaderText = "Período" });
            RrhhDgvListaConceptos.Columns.Add(new DataGridViewTextBoxColumn { Name = "EfectoPercepcionDeduccion", DataPropertyName = "EfectoPercepcionDeduccion", HeaderText = "Efecto" });
            RrhhDgvListaConceptos.Columns.Add(new DataGridViewTextBoxColumn { Name = "TipoPercepcionDeduccion", DataPropertyName = "TipoPercepcionDeduccion", HeaderText = "Tipo de Cálculo" });
            RrhhDgvListaConceptos.Columns.Add(new DataGridViewTextBoxColumn { Name = "ValorTipoPercepcionDeduccion", DataPropertyName = "ValorTipoPercepcionDeduccion", HeaderText = "Valor", DefaultCellStyle = new DataGridViewCellStyle { Format = "N4", Alignment = DataGridViewContentAlignment.MiddleRight } });
            RrhhDgvListaConceptos.Columns.Add(new DataGridViewTextBoxColumn { Name = "AplicacionPercepcionDeduccion", DataPropertyName = "AplicacionPercepcionDeduccion", HeaderText = "Aplicación" });
            RrhhDgvListaConceptos.Columns.Add(new DataGridViewCheckBoxColumn { Name = "IsActive", DataPropertyName = "IsActive", HeaderText = "Activo" });
            RrhhDgvListaConceptos.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedAt", DataPropertyName = "CreatedAt", HeaderText = "Fecha Creación" });
            RrhhDgvListaConceptos.Columns.Add(new DataGridViewTextBoxColumn { Name = "UpdatedAt", DataPropertyName = "UpdatedAt", HeaderText = "Última Actualización" });

            RrhhDgvListaConceptos.CellFormatting += RrhhDgvListaConceptos_CellFormatting;
        }

        private void FrmNominas_Load(object sender, EventArgs e)
        {
            RrhhMetCargarCombos();
            RrhhMetConfigurarGrid();
            RrhhMetListarConceptos();
            RrhhMetReinicio();

        }

        

        // 3. Carga de datos y metodos auxiliares
        private void RrhhMetCargarCombos()
        {
            // Valores iguales a los ENUM de la base de datos
            RrhhCboPeriodo.DropDownStyle = ComboBoxStyle.DropDownList;
            RrhhCboPeriodo.Items.Clear();
            RrhhCboPeriodo.Items.AddRange(new object[] { "MENSUAL", "ANUAL" });

            RrhhCboEfecto.DropDownStyle = ComboBoxStyle.DropDownList;
            RrhhCboEfecto.Items.Clear();
            RrhhCboEfecto.Items.AddRange(new object[] { EFECTO_PERCEPCION, EFECTO_DEDUCCION });

            RrhhCboTipoCalculo.DropDownStyle = ComboBoxStyle.DropDownList;
            RrhhCboTipoCalculo.Items.Clear();
            RrhhCboTipoCalculo.Items.AddRange(new object[] { "Tabla", "%", "Monto" });

            // CheckedListBox con selección única
            RrhhClbAplicacion.CheckOnClick = true;
            RrhhClbAplicacion.Items.Clear();
            RrhhClbAplicacion.Items.AddRange(new object[] { "Todos", "Algunos" });
            RrhhClbAplicacion.ItemCheck += RrhhClbAplicacion_ItemCheck;
        }

        private void RrhhMetListarConceptos()
        {
            try
            {
                RrhhDgvListaConceptos.DataSource = _RrhhPercepcionDeduccion.RrhhMetObtenerTodos();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("No se pudo cargar la lista: " + Ex.Message);
            }
        }

        private void RrhhMetReinicio()
        {
            RrhhTxtNombreConcepto.Clear();
            RrhhCboPeriodo.SelectedIndex = -1;
            RrhhCboEfecto.SelectedIndex = -1;
            RrhhCboTipoCalculo.SelectedIndex = -1;
            RrhhTxtValor.Text = "0.0000";
            RrhhMetMarcarAplicacion(null);
            RrhhCboActivo.Checked = true;
            RrhhDgvListaConceptos.ClearSelection();
        }

        private string RrhhMetObtenerAplicacion()
        {
            return RrhhClbAplicacion.CheckedItems.Count > 0 ? RrhhClbAplicacion.CheckedItems[0].ToString() : null;
        }



        private void RrhhMetMarcarAplicacion(string Aplicacion)
        {
            _AjustandoAplicacion = true;
            for (int i = 0; i < RrhhClbAplicacion.Items.Count; i++)
                RrhhClbAplicacion.SetItemChecked(i, RrhhClbAplicacion.Items[i].ToString() == Aplicacion);
            _AjustandoAplicacion = false;
        }


        private ClsModeloPercepcionDeduccion RrhhMetObtenerSeleccionado()
        {
            if (RrhhDgvListaConceptos.SelectedRows.Count == 0)
                return null;
            return RrhhDgvListaConceptos.SelectedRows[0].DataBoundItem as ClsModeloPercepcionDeduccion;
        }

        private bool RrhhMetCopiarControlesAlModelo()
        {
            decimal Valor = 0;
            if (!string.IsNullOrWhiteSpace(RrhhTxtValor.Text) &&
                !decimal.TryParse(RrhhTxtValor.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out Valor))
            {
                MessageBox.Show("El campo Valor debe ser un número válido");
                return false;
            }

            _RrhhPercepcionDeduccion.NombrePercepcionDeduccion = RrhhTxtNombreConcepto.Text.Trim();
            _RrhhPercepcionDeduccion.PeriodoPercepcionDeduccion = RrhhCboPeriodo.SelectedItem?.ToString();
            _RrhhPercepcionDeduccion.EfectoPercepcionDeduccion = RrhhCboEfecto.SelectedItem?.ToString() == EFECTO_PERCEPCION;
            _RrhhPercepcionDeduccion.TipoPercepcionDeduccion = RrhhCboTipoCalculo.SelectedItem?.ToString();
            _RrhhPercepcionDeduccion.ValorTipoPercepcionDeduccion = Valor;
            _RrhhPercepcionDeduccion.AplicacionPercepcionDeduccion = RrhhMetObtenerAplicacion();
            _RrhhPercepcionDeduccion.IsActive = RrhhCboActivo.Checked;
            return true;
        }

        // Flujo de guardado: copiar controles, asignar Estado, validar, grabar, mensaje y refrescar
        private void RrhhMetEjecutarOperacion(EstadoEntidad Estado, int IdPercepcionDeduccion)
        {
            try
            {
                if (RrhhCboEfecto.SelectedIndex < 0)
                {
                    MessageBox.Show("Seleccione el Efecto");
                    return;
                }
                if (!RrhhMetCopiarControlesAlModelo())
                    return;

                _RrhhPercepcionDeduccion.IdPercepcionDeduccion = IdPercepcionDeduccion;
                _RrhhPercepcionDeduccion.Estado = Estado;

                bool Valido = new ClsValidacionDatos(_RrhhPercepcionDeduccion).RrhhMetValidar();
                if (Valido)
                {
                    string Resultado = _RrhhPercepcionDeduccion.RrhhMetGrabarCambios();
                    MessageBox.Show(Resultado);
                    RrhhTxtBuscar.Clear();
                    RrhhMetListarConceptos();
                    RrhhMetReinicio();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void NominaBtnEliminar_Click(object sender, EventArgs e)
        {
            var Seleccionado = RrhhMetObtenerSeleccionado();
            if (Seleccionado == null)
            {
                MessageBox.Show("Seleccione una fila");
                return;
            }

            var Confirmacion = MessageBox.Show("¿Desea eliminar el concepto seleccionado?", "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Confirmacion != DialogResult.Yes)
                return;

            try
            {
                _RrhhPercepcionDeduccion.IdPercepcionDeduccion = Seleccionado.IdPercepcionDeduccion;
                _RrhhPercepcionDeduccion.Estado = EstadoEntidad.Deleted;
                string Resultado = _RrhhPercepcionDeduccion.RrhhMetGrabarCambios();
                MessageBox.Show(Resultado);
                RrhhTxtBuscar.Clear();
                RrhhMetListarConceptos();
                RrhhMetReinicio();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }


        private void RrhhBtnAsignacionConceptos_Click(object sender, EventArgs e)
        {
            FrmAsignacionConceptos Conceptos = new FrmAsignacionConceptos();
            Conceptos.ShowDialog(this);
        }

        private void RrhhBtnRangos_Click(object sender, EventArgs e)
        {
            FrmRangos Rangos = new FrmRangos();
            Rangos.ShowDialog(this);
        }

        private void RrhhDedBtnGuardar_Click(object sender, EventArgs e)
        {
            RrhhMetEjecutarOperacion(EstadoEntidad.Added, 0);
        }

        private void RrhhDedBtnModificar_Click(object sender, EventArgs e)
        {
            var Seleccionado = RrhhMetObtenerSeleccionado();
            if (Seleccionado == null)
            {
                MessageBox.Show("Seleccione una fila");
                return;
            }
            RrhhMetEjecutarOperacion(EstadoEntidad.Modified, Seleccionado.IdPercepcionDeduccion);
        }

        private void RrhhDedBtnRefrescar_Click(object sender, EventArgs e)
        {
            RrhhTxtBuscar.Clear();
            RrhhMetListarConceptos();
            RrhhMetReinicio();
        }

        private void RrhhTxtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                RrhhDgvListaConceptos.DataSource = _RrhhPercepcionDeduccion
                    .RrhhMetBuscarPorNombre(RrhhTxtBuscar.Text.Trim())
                    .ToList();
                RrhhMetReinicio();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }

        }

        private void RrhhDgvListaConceptos_SelectionChanged(object sender, EventArgs e)
        {
            var Seleccionado = RrhhMetObtenerSeleccionado();
            if (Seleccionado == null)
                return;

            RrhhTxtNombreConcepto.Text = Seleccionado.NombrePercepcionDeduccion;
            RrhhCboPeriodo.SelectedItem = Seleccionado.PeriodoPercepcionDeduccion;
            RrhhCboEfecto.SelectedItem = Seleccionado.EfectoPercepcionDeduccion ? EFECTO_PERCEPCION : EFECTO_DEDUCCION;
            RrhhCboTipoCalculo.SelectedItem = Seleccionado.TipoPercepcionDeduccion;
            RrhhTxtValor.Text = Seleccionado.ValorTipoPercepcionDeduccion.ToString("0.0000", CultureInfo.CurrentCulture);
            RrhhMetMarcarAplicacion(Seleccionado.AplicacionPercepcionDeduccion);
            RrhhCboActivo.Checked = Seleccionado.IsActive;
        }

        private void RrhhClbAplicacion_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_AjustandoAplicacion || e.NewValue != CheckState.Checked)
                return;

            _AjustandoAplicacion = true;
            for (int i = 0; i < RrhhClbAplicacion.Items.Count; i++)
                if (i != e.Index)
                    RrhhClbAplicacion.SetItemChecked(i, false);
            _AjustandoAplicacion = false;

        }

        private void RrhhMetIrAFila(int Indice)
        {
            if (RrhhDgvListaConceptos.Rows.Count == 0)
                return;

            Indice = Math.Max(0, Math.Min(Indice, RrhhDgvListaConceptos.Rows.Count - 1));
            RrhhDgvListaConceptos.ClearSelection();
            RrhhDgvListaConceptos.Rows[Indice].Selected = true;
            RrhhDgvListaConceptos.CurrentCell = RrhhDgvListaConceptos.Rows[Indice].Cells[0];
        }

        private int RrhhMetFilaActual()
        {
            return RrhhDgvListaConceptos.SelectedRows.Count > 0 ? RrhhDgvListaConceptos.SelectedRows[0].Index : -1;
        }

        private void RrhhDgvListaConceptos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && RrhhDgvListaConceptos.Columns[e.ColumnIndex].Name == "EfectoPercepcionDeduccion" && e.Value is bool Efecto)
            {
                e.Value = Efecto ? EFECTO_PERCEPCION : EFECTO_DEDUCCION;
                e.FormattingApplied = true;
            }

        }

        private void RrhhDedBtnInicio_Click(object sender, EventArgs e)
        {
            RrhhMetIrAFila(0);
        }

        private void RrhhDedBtnAnterior_Click(object sender, EventArgs e)
        {
            RrhhMetIrAFila(RrhhMetFilaActual() - 1);
        }

        private void RrhhDedBtnSiguiente_Click(object sender, EventArgs e)
        {
            RrhhMetIrAFila(RrhhMetFilaActual() + 1);
        }

        private void RrhhDedBtnFin_Click(object sender, EventArgs e)
        {
            RrhhMetIrAFila(RrhhDgvListaConceptos.Rows.Count - 1);
        }
    }
}
