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
    public partial class FrmRangos : Form
    {
        private ClsModeloPercepcionDeduccionRango _RrhhRango = new ClsModeloPercepcionDeduccionRango();

        private const string TIPO_TABLA = "Tabla";
        private const string MODO_PORCENTAJE = "%";
        private const string MODO_MONTO = "Monto";
        private bool _AjustandoModoValor = false;
        private bool _Inicializado = false;

        private List<ClsModeloPercepcionDeduccionRango> _ListaRangos = new List<ClsModeloPercepcionDeduccionRango>();

        
        private Dictionary<int, string> _NombresConceptos = new Dictionary<int, string>();
        public FrmRangos()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RrhhMetInicializar();
            RrhhMetAjustarAPantalla();
        }

        private void RrhhMetInicializar()
        {
            if (_Inicializado)
                return;
            _Inicializado = true;

            try
            {
                RrhhMetCargarCombos();
                RrhhMetConfigurarGrid();
                RrhhMetListarRangos();
                RrhhMetReinicio();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error al cargar el formulario: " + Ex.Message);
            }
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

        private void FrmRangos_Load(object sender, EventArgs e)
        {

        }

        private void RrhhMetConfigurarGrid()
        {
            RrhhDgvListaRangos.AutoGenerateColumns = false;
            RrhhDgvListaRangos.Columns.Clear();
            RrhhDgvListaRangos.AllowUserToAddRows = false;
            RrhhDgvListaRangos.AllowUserToDeleteRows = false;
            RrhhDgvListaRangos.ReadOnly = true;
            RrhhDgvListaRangos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            RrhhDgvListaRangos.MultiSelect = false;

            var EstiloDecimal = new DataGridViewCellStyle { Format = "N4", Alignment = DataGridViewContentAlignment.MiddleRight };

            RrhhDgvListaRangos.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdPercepcionDeduccionRango", DataPropertyName = "IdPercepcionDeduccionRango", HeaderText = "Código" });
            // La columna de concepto muestra el nombre (ver CellFormatting)
            RrhhDgvListaRangos.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdPercepcionDeduccion", DataPropertyName = "IdPercepcionDeduccion", HeaderText = "Concepto", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 200 });
            RrhhDgvListaRangos.Columns.Add(new DataGridViewTextBoxColumn { Name = "SalarioDesdePercepcionDeduccionRango", DataPropertyName = "SalarioDesdePercepcionDeduccionRango", HeaderText = "Salario Inicial", DefaultCellStyle = EstiloDecimal });
            RrhhDgvListaRangos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SalarioHastaPercepcionDeduccionRango",
                DataPropertyName = "SalarioHastaPercepcionDeduccionRango",
                HeaderText = "Salario Final",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N4", Alignment = DataGridViewContentAlignment.MiddleRight, NullValue = "Sin límite" }
            });
            RrhhDgvListaRangos.Columns.Add(new DataGridViewTextBoxColumn { Name = "ModoValorPercepcionDeduccionRango", DataPropertyName = "ModoValorPercepcionDeduccionRango", HeaderText = "Modo" });
            RrhhDgvListaRangos.Columns.Add(new DataGridViewTextBoxColumn { Name = "ValorPercepcionDeduccionRango", DataPropertyName = "ValorPercepcionDeduccionRango", HeaderText = "Valor", DefaultCellStyle = EstiloDecimal });
            RrhhDgvListaRangos.Columns.Add(new DataGridViewTextBoxColumn { Name = "MontoFijoBasePercepcionDeduccionRango", DataPropertyName = "MontoFijoBasePercepcionDeduccionRango", HeaderText = "Monto Fijo", DefaultCellStyle = EstiloDecimal });
            RrhhDgvListaRangos.Columns.Add(new DataGridViewCheckBoxColumn { Name = "IsActive", DataPropertyName = "IsActive", HeaderText = "Activo" });
            RrhhDgvListaRangos.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedAt", DataPropertyName = "CreatedAt", HeaderText = "Fecha Creación" });
            RrhhDgvListaRangos.Columns.Add(new DataGridViewTextBoxColumn { Name = "UpdatedAt", DataPropertyName = "UpdatedAt", HeaderText = "Última Actualización" });

            // Columnas angostas para dejarle espacio a Concepto
            RrhhDgvListaRangos.Columns["IdPercepcionDeduccionRango"].Width = 60;
            RrhhDgvListaRangos.Columns["ModoValorPercepcionDeduccionRango"].Width = 70;
            RrhhDgvListaRangos.Columns["IsActive"].Width = 60;
            RrhhDgvListaRangos.Columns["CreatedAt"].Width = 130;
            RrhhDgvListaRangos.Columns["UpdatedAt"].Width = 130;

            RrhhDgvListaRangos.CellFormatting += RrhhDgvListaRangos_CellFormatting;
        }

        // 3. Carga de datos y metodos auxiliares
        private void RrhhMetCargarCombos()
        {
            // Todos los conceptos para los nombres del grid, pero el combo solo muestra los de tipo "Tabla"
            var TodosConceptos = new ClsModeloPercepcionDeduccion().RrhhMetObtenerTodos();
            _NombresConceptos = TodosConceptos.ToDictionary(c => c.IdPercepcionDeduccion, c => c.NombrePercepcionDeduccion);

            RrhhCboConcepto.DropDownStyle = ComboBoxStyle.DropDownList;
            RrhhCboConcepto.DisplayMember = "NombrePercepcionDeduccion";
            RrhhCboConcepto.ValueMember = "IdPercepcionDeduccion";
            RrhhCboConcepto.DataSource = TodosConceptos
                .Where(c => c.TipoPercepcionDeduccion == TIPO_TABLA)
                .ToList();

            // CheckedListBox con selección única (valores iguales al ENUM de la base de datos)
            RrhhClbModoValor.CheckOnClick = true;
            RrhhClbModoValor.Items.Clear();
            RrhhClbModoValor.Items.AddRange(new object[] { MODO_PORCENTAJE, MODO_MONTO });
            RrhhClbModoValor.ItemCheck += RrhhClbModoValor_ItemCheck;
        }

        private void RrhhMetListarRangos()
        {
            try
            {
                _ListaRangos = _RrhhRango.RrhhMetObtenerTodos();
                RrhhDgvListaRangos.DataSource = _ListaRangos;
            }
            catch (Exception Ex)
            {
                MessageBox.Show("No se pudo cargar la lista: " + Ex.Message);
            }
        }

        private void RrhhMetReinicio()
        {
            RrhhCboConcepto.SelectedIndex = -1;
            RrhhTxtSalarioInicial.Clear();
            RrhhTxtSalarioFinal.Clear();
            RrhhMetMarcarModoValor(null);
            RrhhTxtValor.Text = "0.0000";
            RrhhTxtMontoFijo.Text = "0.0000";
            RrhhCboActivo.Checked = true;
            RrhhDgvListaRangos.ClearSelection();
        }

        private string RrhhMetObtenerModoValor()
        {
            return RrhhClbModoValor.CheckedItems.Count > 0 ? RrhhClbModoValor.CheckedItems[0].ToString() : null;
        }

        private void RrhhMetMarcarModoValor(string Modo)
        {
            _AjustandoModoValor = true;
            for (int i = 0; i < RrhhClbModoValor.Items.Count; i++)
                RrhhClbModoValor.SetItemChecked(i, RrhhClbModoValor.Items[i].ToString() == Modo);
            _AjustandoModoValor = false;
        }

        private string RrhhMetObtenerNombreConcepto(int IdPercepcionDeduccion)
        {
            string Nombre;
            return _NombresConceptos.TryGetValue(IdPercepcionDeduccion, out Nombre) ? Nombre : IdPercepcionDeduccion.ToString();
        }

        private ClsModeloPercepcionDeduccionRango RrhhMetObtenerSeleccionado()
        {
            if (RrhhDgvListaRangos.SelectedRows.Count == 0)
                return null;
            return RrhhDgvListaRangos.SelectedRows[0].DataBoundItem as ClsModeloPercepcionDeduccionRango;
        }

        // Lee un decimal de un TextBox. Si esta vacio: error si es obligatorio, o Valor = null si es opcional
        private bool RrhhMetLeerDecimal(TextBox Caja, string NombreCampo, bool Obligatorio, out decimal? Valor)
        {
            Valor = null;
            if (string.IsNullOrWhiteSpace(Caja.Text))
            {
                if (Obligatorio)
                {
                    MessageBox.Show("El campo " + NombreCampo + " es requerido");
                    return false;
                }
                return true;
            }

            decimal Numero;
            if (!decimal.TryParse(Caja.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out Numero))
            {
                MessageBox.Show("El campo " + NombreCampo + " debe ser un número válido");
                return false;
            }
            Valor = Numero;
            return true;
        }

        private bool RrhhMetCopiarControlesAlModelo()
        {
            if (!(RrhhCboConcepto.SelectedValue is int IdConcepto))
            {
                MessageBox.Show("Seleccione la percepción/deducción");
                return false;
            }

            string Modo = RrhhMetObtenerModoValor();
            if (Modo == null)
            {
                MessageBox.Show("Seleccione el modo de valor (% o Monto)");
                return false;
            }

            decimal? Desde, Hasta, Valor, MontoFijo;
            if (!RrhhMetLeerDecimal(RrhhTxtSalarioInicial, "Salario inicial", true, out Desde))
                return false;
            if (!RrhhMetLeerDecimal(RrhhTxtSalarioFinal, "Salario final", false, out Hasta))
                return false;
            if (!RrhhMetLeerDecimal(RrhhTxtValor, "Valor", false, out Valor))
                return false;
            if (!RrhhMetLeerDecimal(RrhhTxtMontoFijo, "Monto fijo", false, out MontoFijo))
                return false;

            _RrhhRango.IdPercepcionDeduccion = IdConcepto;
            _RrhhRango.SalarioDesdePercepcionDeduccionRango = Desde.Value;
            _RrhhRango.SalarioHastaPercepcionDeduccionRango = Hasta;               // vacio = sin limite superior
            _RrhhRango.ModoValorPercepcionDeduccionRango = Modo;
            _RrhhRango.ValorPercepcionDeduccionRango = Valor ?? 0;
            _RrhhRango.MontoFijoBasePercepcionDeduccionRango = MontoFijo ?? 0;
            _RrhhRango.IsActive = RrhhCboActivo.Checked;
            return true;
        }

        // Flujo de guardado: copiar controles, asignar Estado, validar, grabar, mensaje y refrescar
        private void RrhhMetEjecutarOperacion(EstadoEntidad Estado, int IdRango)
        {
            try
            {
                if (!RrhhMetCopiarControlesAlModelo())
                    return;

                _RrhhRango.IdPercepcionDeduccionRango = IdRango;
                _RrhhRango.Estado = Estado;

                bool Valido = new ClsValidacionDatos(_RrhhRango).RrhhMetValidar();
                if (Valido)
                {
                    string Resultado = _RrhhRango.RrhhMetGrabarCambios();
                    MessageBox.Show(Resultado);
                    RrhhTxtBuscar.Clear();
                    RrhhMetListarRangos();
                    RrhhMetReinicio();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void RrhhDedBtnEliminar_Click(object sender, EventArgs e)
        {
            var Seleccionado = RrhhMetObtenerSeleccionado();
            if (Seleccionado == null)
            {
                MessageBox.Show("Seleccione una fila");
                return;
            }

            var Confirmacion = MessageBox.Show("¿Desea eliminar el rango seleccionado?", "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Confirmacion != DialogResult.Yes)
                return;

            try
            {
                _RrhhRango.IdPercepcionDeduccionRango = Seleccionado.IdPercepcionDeduccionRango;
                _RrhhRango.Estado = EstadoEntidad.Deleted;
                string Resultado = _RrhhRango.RrhhMetGrabarCambios();
                MessageBox.Show(Resultado);
                RrhhTxtBuscar.Clear();
                RrhhMetListarRangos();
                RrhhMetReinicio();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
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
            RrhhMetEjecutarOperacion(EstadoEntidad.Modified, Seleccionado.IdPercepcionDeduccionRango);

        }

        private void RrhhDedBtnRefrescar_Click(object sender, EventArgs e)
        {
            RrhhTxtBuscar.Clear();
            RrhhMetListarRangos();
            RrhhMetReinicio();
        }

        private void RrhhTxtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string Texto = RrhhTxtBuscar.Text.Trim().ToUpper();
                RrhhDgvListaRangos.DataSource = _ListaRangos.FindAll(r =>
                    RrhhMetObtenerNombreConcepto(r.IdPercepcionDeduccion).ToUpper().Contains(Texto));
                RrhhMetReinicio();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }

        }

        private void RrhhDgvListaRangos_SelectionChanged(object sender, EventArgs e)
        {
            var Seleccionado = RrhhMetObtenerSeleccionado();
            if (Seleccionado == null)
                return;

            // Si el concepto ya no es de tipo "Tabla" no esta en el combo y queda sin seleccion
            RrhhCboConcepto.SelectedValue = Seleccionado.IdPercepcionDeduccion;
            RrhhTxtSalarioInicial.Text = Seleccionado.SalarioDesdePercepcionDeduccionRango.ToString("0.0000", CultureInfo.CurrentCulture);
            RrhhTxtSalarioFinal.Text = Seleccionado.SalarioHastaPercepcionDeduccionRango.HasValue
                ? Seleccionado.SalarioHastaPercepcionDeduccionRango.Value.ToString("0.0000", CultureInfo.CurrentCulture)
                : string.Empty;
            RrhhMetMarcarModoValor(Seleccionado.ModoValorPercepcionDeduccionRango);
            RrhhTxtValor.Text = Seleccionado.ValorPercepcionDeduccionRango.ToString("0.0000", CultureInfo.CurrentCulture);
            RrhhTxtMontoFijo.Text = Seleccionado.MontoFijoBasePercepcionDeduccionRango.ToString("0.0000", CultureInfo.CurrentCulture);
            RrhhCboActivo.Checked = Seleccionado.IsActive;
        }

        private void RrhhClbModoValor_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_AjustandoModoValor || e.NewValue != CheckState.Checked)
                return;

            _AjustandoModoValor = true;
            for (int i = 0; i < RrhhClbModoValor.Items.Count; i++)
                if (i != e.Index)
                    RrhhClbModoValor.SetItemChecked(i, false);
            _AjustandoModoValor = false;
        }

        private void RrhhMetIrAFila(int Indice)
        {
            if (RrhhDgvListaRangos.Rows.Count == 0)
                return;

            Indice = Math.Max(0, Math.Min(Indice, RrhhDgvListaRangos.Rows.Count - 1));
            RrhhDgvListaRangos.ClearSelection();
            RrhhDgvListaRangos.Rows[Indice].Selected = true;
            RrhhDgvListaRangos.CurrentCell = RrhhDgvListaRangos.Rows[Indice].Cells[0];
        }

        private int RrhhMetFilaActual()
        {
            return RrhhDgvListaRangos.SelectedRows.Count > 0 ? RrhhDgvListaRangos.SelectedRows[0].Index : -1;
        }

        private void RrhhDgvListaRangos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 &&
                RrhhDgvListaRangos.Columns[e.ColumnIndex].Name == "IdPercepcionDeduccion" &&
                e.Value is int IdConcepto)
            {
                e.Value = RrhhMetObtenerNombreConcepto(IdConcepto);
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
            RrhhMetIrAFila(RrhhDgvListaRangos.Rows.Count - 1);
        }
    }
}
