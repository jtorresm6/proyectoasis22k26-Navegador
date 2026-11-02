using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Administra las páginas de la tabla y los controles para recorrer los registros disponibles.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    public partial class ComprasUsrDataGridView
    {
        private object _OrigenDatos;
        private DataView _VistaDatos;
        private DataTable _TablaPagina;
        private FlowLayoutPanel ComprasFlpPaginacion;
        private ClsButtonCompras ComprasBtnAnterior;
        private ClsButtonCompras ComprasBtnSiguiente;
        private Label ComprasLblPagina;
        private int _PaginaActual;
        private const int _RegistrosPorPagina = 15;
        private bool _ActualizandoPagina;
        private bool _PaginaPendiente;
        private string _CampoOrden;
        private bool _OrdenDescendente;
        private bool _PaginacionHabilitada = true;

        [Category("Compras"), DefaultValue(true)]
        public bool PaginacionHabilitada
        {
            get { return _PaginacionHabilitada; }
            set
            {
                _PaginacionHabilitada = value;
                _PaginaActual = 0;
                if (ComprasFlpPaginacion != null) ComprasFlpPaginacion.Visible = value;
                ComprasProcMostrarPagina();
            }
        }

        private void ComprasProcInicializarPaginacion()
        {
            var Recursos = new ComponentResourceManager(typeof(ComprasUsrDataGridView));
            ComprasFlpPaginacion = new FlowLayoutPanel
            {
                Name = "ComprasFlpPaginacion", Dock = DockStyle.Bottom, Height = 46,
                WrapContents = false, BackColor = ClsTemaCompras.FondoGeneral,
                Visible = _PaginacionHabilitada
            };
            ComprasBtnAnterior = new ClsButtonCompras
            {
                Name = "ComprasBtnAnterior", TipoBoton = ComprasTipoBoton.Libre,
                Text = string.Empty, AccessibleName = "Anterior", Size = new System.Drawing.Size(40, 40),
                BackgroundImage = (System.Drawing.Image)Recursos.GetObject("Anterior"),
                ImagenDeshabilitado = (System.Drawing.Image)Recursos.GetObject("Anterior_gris"),
                BackgroundImageLayout = ImageLayout.Zoom
            };
            ComprasBtnSiguiente = new ClsButtonCompras
            {
                Name = "ComprasBtnSiguiente", TipoBoton = ComprasTipoBoton.Libre,
                Text = string.Empty, AccessibleName = "Siguiente", Size = new System.Drawing.Size(40, 40),
                BackgroundImage = (System.Drawing.Image)Recursos.GetObject("Siguiente"),
                ImagenDeshabilitado = (System.Drawing.Image)Recursos.GetObject("Siguiente_gris"),
                BackgroundImageLayout = ImageLayout.Zoom
            };
            ComprasLblPagina = new Label
            {
                Name = "ComprasLblPagina", AutoSize = true, Padding = new Padding(4, 13, 0, 0),
                Font = new System.Drawing.Font("Tahoma", 9F)
            };
            ComprasFlpPaginacion.Controls.AddRange(new Control[] { ComprasBtnAnterior, ComprasBtnSiguiente, ComprasLblPagina });
            Controls.Add(ComprasFlpPaginacion);
            ComprasBtnAnterior.Click += (Remitente, Evento) => { _PaginaActual--; ComprasProcMostrarPagina(); };
            ComprasBtnSiguiente.Click += (Remitente, Evento) => { _PaginaActual++; ComprasProcMostrarPagina(); };
            ComprasDgvMain.RowsAdded += (Remitente, Evento) => ComprasProcProgramarPagina();
            ComprasDgvMain.RowsRemoved += (Remitente, Evento) => ComprasProcProgramarPagina();
            ComprasDgvMain.Sorted += (Remitente, Evento) => { _PaginaActual = 0; ComprasProcMostrarPagina(); };
            ComprasDgvMain.HandleCreated += (Remitente, Evento) => ComprasProcProgramarPagina();
            ComprasDgvMain.DataBindingComplete += (Remitente, Evento) => ComprasProcConfigurarOrden();
            ComprasDgvMain.ColumnHeaderMouseClick += ComprasMetOrdenarPagina;
            Disposed += (Remitente, Evento) =>
            {
                if (_VistaDatos != null) _VistaDatos.ListChanged -= ComprasMetOrigenActualizado;
                _TablaPagina?.Dispose();
            };
            ComprasProcMostrarPagina();
        }

        private void ComprasProcAsignarDatos(object Origen)
        {
            if (_VistaDatos != null) _VistaDatos.ListChanged -= ComprasMetOrigenActualizado;
            _OrigenDatos = Origen;
            _VistaDatos = Origen is DataTable ? ((DataTable)Origen).DefaultView : Origen as DataView;
            _PaginaActual = 0;
            _CampoOrden = null;
            if (_VistaDatos == null) ComprasDgvMain.DataSource = Origen;
            else _VistaDatos.ListChanged += ComprasMetOrigenActualizado;
            ComprasProcMostrarPagina();
        }

        private void ComprasMetOrigenActualizado(object Remitente, ListChangedEventArgs Evento)
        {
            if (!_ActualizandoPagina) ComprasProcMostrarPagina();
        }

        private void ComprasProcProgramarPagina()
        {
            if (_OrigenDatos != null || _ActualizandoPagina || _PaginaPendiente || !IsHandleCreated || IsDisposed) return;
            _PaginaPendiente = true;
            BeginInvoke(new Action(() =>
            {
                _PaginaPendiente = false;
                if (!IsDisposed) ComprasProcMostrarPagina();
            }));
        }

        private void ComprasProcConfigurarOrden()
        {
            foreach (DataGridViewColumn Columna in ComprasDgvMain.Columns)
            {
                if (Columna.SortMode == DataGridViewColumnSortMode.NotSortable) continue;
                if (_VistaDatos != null)
                    Columna.SortMode = DataGridViewColumnSortMode.Programmatic;
                Columna.HeaderCell.SortGlyphDirection = Columna.DataPropertyName == _CampoOrden
                    ? (_OrdenDescendente ? SortOrder.Descending : SortOrder.Ascending) : SortOrder.None;
            }
        }

        private void ComprasMetOrdenarPagina(object Remitente, DataGridViewCellMouseEventArgs Evento)
        {
            if (_VistaDatos == null || Evento.ColumnIndex < 0) return;
            DataGridViewColumn Columna = ComprasDgvMain.Columns[Evento.ColumnIndex];
            string Campo = Columna.DataPropertyName;
            if (Columna.SortMode == DataGridViewColumnSortMode.NotSortable || !_VistaDatos.Table.Columns.Contains(Campo)) return;
            _OrdenDescendente = _CampoOrden == Campo && !_OrdenDescendente;
            _CampoOrden = Campo;
            _PaginaActual = 0;
            _ActualizandoPagina = true;
            try { _VistaDatos.Sort = "[" + Campo.Replace("\\", "\\\\").Replace("]", "\\]") + "] " + (_OrdenDescendente ? "DESC" : "ASC"); }
            finally { _ActualizandoPagina = false; }
            ComprasProcMostrarPagina();
        }

        private void ComprasProcMostrarPagina()
        {
            if (_ActualizandoPagina || ComprasLblPagina == null || IsDisposed) return;
            _ActualizandoPagina = true;
            try
            {
                int Total = _VistaDatos != null ? _VistaDatos.Count : ComprasDgvMain.Rows.Count - (ComprasDgvMain.NewRowIndex >= 0 ? 1 : 0);
                int Paginas = Math.Max(1, (Total + _RegistrosPorPagina - 1) / _RegistrosPorPagina);
                _PaginaActual = Math.Max(0, Math.Min(_PaginaActual, Paginas - 1));
                int Inicio = _PaginacionHabilitada ? _PaginaActual * _RegistrosPorPagina : 0;
                int Fin = _PaginacionHabilitada ? Math.Min(Total, Inicio + _RegistrosPorPagina) : Total;
                if (_VistaDatos != null)
                {
                    DataTable Anterior = _TablaPagina;
                    _TablaPagina = _VistaDatos.Table.Clone();
                    for (int Indice = Inicio; Indice < Fin; Indice++) _TablaPagina.ImportRow(_VistaDatos[Indice].Row);
                    ComprasDgvMain.DataSource = _TablaPagina;
                    Anterior?.Dispose();
                    ComprasProcConfigurarOrden();
                }
                else if (_OrigenDatos == null)
                {
                    ComprasDgvMain.CurrentCell = null;
                    for (int Indice = 0; Indice < Total; Indice++)
                        ComprasDgvMain.Rows[Indice].Visible = Indice >= Inicio && Indice < Fin;
                }
                ComprasLblPagina.Text = "Página " + (_PaginaActual + 1) + " de " + Paginas + " · Total: " + Total;
                ComprasBtnAnterior.Enabled = _PaginaActual > 0;
                ComprasBtnSiguiente.Enabled = _PaginaActual + 1 < Paginas;
                ComprasMetAjustarAlturaFilas();
            }
            finally { _ActualizandoPagina = false; }
        }
    }
}
