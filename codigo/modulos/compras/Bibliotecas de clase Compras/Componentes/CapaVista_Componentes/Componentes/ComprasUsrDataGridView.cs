using System;
using System.ComponentModel;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Presenta datos tabulares y administra la selección, los eventos y la altura de las filas.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    [DefaultProperty("DataSource")]
    [DefaultEvent("CellClick")]
    public partial class ComprasUsrDataGridView : UserControl
    {

        private int _FilaHover = -1;
        private bool _ResaltarFilasAlPasar;
        private Cursor _CursorHabitual;

        [Category("Comportamiento")]
        [DefaultValue(false)]
        public bool ResaltarFilasAlPasar
        {
            get { return _ResaltarFilasAlPasar; }
            set { _ResaltarFilasAlPasar = value; ComprasProcHover(-1); }
        }

        private void ComprasProcHover(int Fila)
        {
            int Anterior = _FilaHover;
            _FilaHover = Fila;
            ComprasDgvMain.Cursor = Fila >= 0 ? Cursors.Hand : _CursorHabitual;
            if (Anterior >= 0 && Anterior < ComprasDgvMain.Rows.Count) ComprasDgvMain.InvalidateRow(Anterior);
            if (Fila >= 0 && Fila < ComprasDgvMain.Rows.Count) ComprasDgvMain.InvalidateRow(Fila);
        }

        private const int _AlturaMinimaFila = 28;
        private const int _AlturaMaximaFila = 40;

        public ComprasUsrDataGridView()
        {
            InitializeComponent();
            ComprasProcInicializarPaginacion();
            Enter += (Remitente, Evento) => ComprasDgvMain.Focus();
            ComprasDgvMain.KeyDown += (Remitente, Evento) => OnKeyDown(Evento);
            ComprasDgvMain.KeyPress += (Remitente, Evento) => OnKeyPress(Evento);
            ComprasDgvMain.KeyUp += (Remitente, Evento) => OnKeyUp(Evento);
            _CursorHabitual = ComprasDgvMain.Cursor;
            ComprasDgvMain.MouseMove += (Remitente, Evento) =>
            {
                DataGridView.HitTestInfo Zona = ComprasDgvMain.HitTest(Evento.X, Evento.Y);
                int Fila = ResaltarFilasAlPasar && Zona.Type == DataGridViewHitTestType.Cell && Zona.RowIndex >= 0 && !ComprasDgvMain.Rows[Zona.RowIndex].IsNewRow ? Zona.RowIndex : -1;
                if (_FilaHover != Fila) ComprasProcHover(Fila);
            };
            ComprasDgvMain.MouseLeave += (Remitente, Evento) => ComprasProcHover(-1);
            ComprasDgvMain.DataBindingComplete += (Remitente, Evento) => ComprasProcHover(-1);
            ComprasDgvMain.CellPainting += (Remitente, Evento) =>
            {
                if (ResaltarFilasAlPasar && Evento.RowIndex == _FilaHover && Evento.RowIndex >= 0 && Evento.ColumnIndex >= 0)
                {
                    System.Drawing.Color Fondo = Evento.CellStyle.BackColor;
                    System.Drawing.Color Texto = Evento.CellStyle.ForeColor;
                    try
                    {
                        Evento.CellStyle.BackColor = Evento.CellStyle.SelectionBackColor;
                        Evento.CellStyle.ForeColor = Evento.CellStyle.SelectionForeColor;
                        Evento.Paint(Evento.ClipBounds, Evento.PaintParts);
                        Evento.Handled = true;
                    }
                    finally
                    {
                        Evento.CellStyle.BackColor = Fondo;
                        Evento.CellStyle.ForeColor = Texto;
                    }
                }
            };

            ComprasDgvMain.Resize += ComprasMetActualizarAlturaFilas;
            ComprasDgvMain.RowsAdded += ComprasMetFilasAgregadas;
            ComprasDgvMain.RowsRemoved += ComprasMetFilasEliminadas;
            ComprasDgvMain.DataBindingComplete += ComprasMetEnlaceCompletado;
            ComprasDgvMain.HandleCreated += ComprasMetTablaCreada;

            ComprasMetActivarDobleBuffer();
        }

        private void ComprasMetTablaCreada(object Remitente, EventArgs Evento)
        {
            ComprasMetActivarDobleBuffer();
            ComprasMetAjustarAlturaFilas();
        }
        private void ComprasMetActivarDobleBuffer()
        {
            System.Reflection.PropertyInfo PropiedadDobleBuffer =
                typeof(DataGridView).GetProperty(
                    "DoubleBuffered",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic);

            if (PropiedadDobleBuffer != null)
            {
                PropiedadDobleBuffer.SetValue(ComprasDgvMain, true, null);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridView Tabla
        {
            get { return ComprasDgvMain; }
        }

        [Category("Datos")]
        [AttributeProvider(typeof(IListSource))]
        [DefaultValue(null)]
        public object DataSource
        {
            get { return _OrigenDatos; }
            set { ComprasProcAsignarDatos(value); }
        }

        [Category("Datos")]
        [DefaultValue("")]
        public string DataMember
        {
            get { return ComprasDgvMain.DataMember; }
            set { ComprasDgvMain.DataMember = value; }
        }

        [Category("Datos")]
        [DefaultValue(true)]
        public bool AutoGenerateColumns
        {
            get { return ComprasDgvMain.AutoGenerateColumns; }
            set { ComprasDgvMain.AutoGenerateColumns = value; }
        }

        [Category("Comportamiento")]
        [DefaultValue(true)]
        public bool ReadOnly
        {
            get { return ComprasDgvMain.ReadOnly; }
            set { ComprasDgvMain.ReadOnly = value; }
        }

        [Category("Comportamiento")]
        [DefaultValue(false)]
        public bool AllowUserToAddRows
        {
            get { return ComprasDgvMain.AllowUserToAddRows; }
            set { ComprasDgvMain.AllowUserToAddRows = value; }
        }

        [Category("Comportamiento")]
        [DefaultValue(false)]
        public bool AllowUserToDeleteRows
        {
            get { return ComprasDgvMain.AllowUserToDeleteRows; }
            set { ComprasDgvMain.AllowUserToDeleteRows = value; }
        }

        [Category("Comportamiento")]
        [DefaultValue(false)]
        public bool AllowUserToResizeRows
        {
            get { return ComprasDgvMain.AllowUserToResizeRows; }
            set { ComprasDgvMain.AllowUserToResizeRows = value; }
        }

        [Category("Comportamiento")]
        [DefaultValue(false)]
        public bool MultiSelect
        {
            get { return ComprasDgvMain.MultiSelect; }
            set { ComprasDgvMain.MultiSelect = value; }
        }

        [Category("Comportamiento")]
        [DefaultValue(DataGridViewSelectionMode.FullRowSelect)]
        public DataGridViewSelectionMode SelectionMode
        {
            get { return ComprasDgvMain.SelectionMode; }
            set { ComprasDgvMain.SelectionMode = value; }
        }

        [Category("Comportamiento")]
        [DefaultValue(DataGridViewEditMode.EditProgrammatically)]
        public DataGridViewEditMode EditMode
        {
            get { return ComprasDgvMain.EditMode; }
            set { ComprasDgvMain.EditMode = value; }
        }

        [Category("Apariencia")]
        [DefaultValue(DataGridViewAutoSizeColumnsMode.Fill)]
        public DataGridViewAutoSizeColumnsMode AutoSizeColumnsMode
        {
            get { return ComprasDgvMain.AutoSizeColumnsMode; }
            set { ComprasDgvMain.AutoSizeColumnsMode = value; }
        }

        [Category("Apariencia")]
        [DefaultValue(false)]
        public bool RowHeadersVisible
        {
            get { return ComprasDgvMain.RowHeadersVisible; }
            set { ComprasDgvMain.RowHeadersVisible = value; }
        }

        [Category("Apariencia")]
        [DefaultValue(true)]
        public bool ColumnHeadersVisible
        {
            get { return ComprasDgvMain.ColumnHeadersVisible; }
            set
            {
                ComprasDgvMain.ColumnHeadersVisible = value;
                ComprasMetAjustarAlturaFilas();
            }
        }

        [Category("Datos")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DataGridViewColumnCollection Columns
        {
            get { return ComprasDgvMain.Columns; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewRowCollection Rows
        {
            get { return ComprasDgvMain.Rows; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewSelectedRowCollection SelectedRows
        {
            get { return ComprasDgvMain.SelectedRows; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewSelectedCellCollection SelectedCells
        {
            get { return ComprasDgvMain.SelectedCells; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewRow CurrentRow
        {
            get { return ComprasDgvMain.CurrentRow; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewCell CurrentCell
        {
            get { return ComprasDgvMain.CurrentCell; }
            set { ComprasDgvMain.CurrentCell = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RowCount
        {
            get { return ComprasDgvMain.RowCount; }
            set { ComprasDgvMain.RowCount = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ColumnCount
        {
            get { return ComprasDgvMain.ColumnCount; }
            set { ComprasDgvMain.ColumnCount = value; }
        }

        public DataGridViewCell this[int IndiceColumna, int IndiceFila]
        {
            get { return ComprasDgvMain[IndiceColumna, IndiceFila]; }
            set { ComprasDgvMain[IndiceColumna, IndiceFila] = value; }
        }

        public DataGridViewCell this[string NombreColumna, int IndiceFila]
        {
            get { return ComprasDgvMain[NombreColumna, IndiceFila]; }
            set { ComprasDgvMain[NombreColumna, IndiceFila] = value; }
        }

        [Category("Acción")]
        public event DataGridViewCellEventHandler CellClick
        {
            add { ComprasDgvMain.CellClick += value; }
            remove { ComprasDgvMain.CellClick -= value; }
        }

        [Category("Acción")]
        public event DataGridViewCellEventHandler CellDoubleClick
        {
            add { ComprasDgvMain.CellDoubleClick += value; }
            remove { ComprasDgvMain.CellDoubleClick -= value; }
        }

        [Category("Acción")]
        public event DataGridViewCellEventHandler CellContentClick
        {
            add { ComprasDgvMain.CellContentClick += value; }
            remove { ComprasDgvMain.CellContentClick -= value; }
        }

        [Category("Acción")]
        public event EventHandler SelectionChanged
        {
            add { ComprasDgvMain.SelectionChanged += value; }
            remove { ComprasDgvMain.SelectionChanged -= value; }
        }

        [Category("Datos")]
        public event DataGridViewBindingCompleteEventHandler DataBindingComplete
        {
            add { ComprasDgvMain.DataBindingComplete += value; }
            remove { ComprasDgvMain.DataBindingComplete -= value; }
        }

        [Category("Datos")]
        public event DataGridViewRowsAddedEventHandler RowsAdded
        {
            add { ComprasDgvMain.RowsAdded += value; }
            remove { ComprasDgvMain.RowsAdded -= value; }
        }

        [Category("Datos")]
        public event DataGridViewRowsRemovedEventHandler RowsRemoved
        {
            add { ComprasDgvMain.RowsRemoved += value; }
            remove { ComprasDgvMain.RowsRemoved -= value; }
        }

        [Category("Datos")]
        public event DataGridViewCellEventHandler CellValueChanged
        {
            add { ComprasDgvMain.CellValueChanged += value; }
            remove { ComprasDgvMain.CellValueChanged -= value; }
        }

        [Category("Datos")]
        public event DataGridViewCellValidatingEventHandler CellValidating
        {
            add { ComprasDgvMain.CellValidating += value; }
            remove { ComprasDgvMain.CellValidating -= value; }
        }

        [Category("Datos")]
        public event DataGridViewDataErrorEventHandler DataError
        {
            add { ComprasDgvMain.DataError += value; }
            remove { ComprasDgvMain.DataError -= value; }
        }

        protected override void OnCausesValidationChanged(EventArgs Evento)
        {
            base.OnCausesValidationChanged(Evento);
            if (ComprasDgvMain != null) ComprasDgvMain.CausesValidation = CausesValidation;
        }

        public void ClearSelection()
        {
            ComprasDgvMain.ClearSelection();
        }

        public bool BeginEdit(bool SeleccionarTodo)
        {
            return ComprasDgvMain.BeginEdit(SeleccionarTodo);
        }

        public bool EndEdit()
        {
            return ComprasDgvMain.EndEdit();
        }

        public bool CancelEdit()
        {
            return ComprasDgvMain.CancelEdit();
        }

        public void ComprasMetEnfocarTabla()
        {
            ComprasDgvMain.Focus();
        }

        private void ComprasMetActualizarAlturaFilas(object Remitente, EventArgs Evento)
        {
            ComprasMetAjustarAlturaFilas();
        }

        private void ComprasMetFilasAgregadas(object Remitente, DataGridViewRowsAddedEventArgs Evento)
        {
            ComprasMetAjustarAlturaFilas();
        }

        private void ComprasMetFilasEliminadas(object Remitente, DataGridViewRowsRemovedEventArgs Evento)
        {
            ComprasMetAjustarAlturaFilas();
        }

        private void ComprasMetEnlaceCompletado(object Remitente, DataGridViewBindingCompleteEventArgs Evento)
        {
            ComprasMetAjustarAlturaFilas();
        }

        internal bool ComprasFilasUniformes { get; set; }

        private void ComprasMetAjustarAlturaFilas()
        {
            if (ComprasDgvMain.AutoSizeRowsMode != DataGridViewAutoSizeRowsMode.None)
            {
                return;
            }

            int CantidadFilas = 0;

            foreach (DataGridViewRow Fila in ComprasDgvMain.Rows)
            {
                if (Fila.Visible)
                {
                    CantidadFilas++;
                }
            }

            if (CantidadFilas == 0)
            {
                return;
            }

            int AlturaEncabezado = ComprasDgvMain.ColumnHeadersVisible
                ? ComprasDgvMain.ColumnHeadersHeight
                : 0;

            int EspacioDisponible = ComprasDgvMain.ClientSize.Height - AlturaEncabezado;

            foreach (Control ControlInterno in ComprasDgvMain.Controls)
            {
                if (ControlInterno is HScrollBar && ControlInterno.Visible)
                {
                    EspacioDisponible -= ControlInterno.Height;
                }
            }

            if (EspacioDisponible <= 0)
            {
                return;
            }

            int AlturaCalculada = EspacioDisponible / CantidadFilas;
            int AlturaBase = Math.Max(_AlturaMinimaFila, Math.Min(_AlturaMaximaFila, AlturaCalculada));
            int Sobrante = !ComprasFilasUniformes && AlturaCalculada >= _AlturaMinimaFila && AlturaCalculada < _AlturaMaximaFila
                ? EspacioDisponible % CantidadFilas
                : 0;

            foreach (DataGridViewRow Fila in ComprasDgvMain.Rows)
            {
                if (!Fila.Visible)
                {
                    continue;
                }

                int AlturaFila = Math.Max(Fila.MinimumHeight, AlturaBase + (Sobrante > 0 ? 1 : 0));

                if (Fila.Height != AlturaFila)
                {
                    Fila.Height = AlturaFila;
                }

                if (Sobrante > 0)
                {
                    Sobrante--;
                }
            }

        }
    }
}
