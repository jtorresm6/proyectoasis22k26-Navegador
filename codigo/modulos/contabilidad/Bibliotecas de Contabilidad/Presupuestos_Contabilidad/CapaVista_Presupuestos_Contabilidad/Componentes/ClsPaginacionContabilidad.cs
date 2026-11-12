/* Área: Contabilidad. Autor: Gabriel André Guillén Pocón. Carné: 0901-23-1998.
 * Fecha: 14/10/2026. Propósito: listado completo con scroll y orden global para tablas de consulta (EST-10 §8). */
using System;
using System.Data;
using System.Windows.Forms;

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaVista_Contabilidad.Componentes
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    internal sealed class ClsPaginacionContabilidad : IDisposable
    {
        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        private readonly ClsTablaContabilidad _Tabla;
        private DataView _Vista;
        private string _CampoOrden = string.Empty;
        private bool _Descendente;
        // Botones Inicio, Anterior, Siguiente y Fin
        private Control[] _Navegacion = new Control[0];

        internal ClsPaginacionContabilidad(ClsTablaContabilidad Tabla)
        {
            _Tabla = Tabla;
            _Tabla.ScrollBars = ScrollBars.Both;
            foreach (DataGridViewColumn Columna in Tabla.Columns) Columna.SortMode = DataGridViewColumnSortMode.Programmatic;
            Tabla.ColumnHeaderMouseClick += ContabilidadMetOrdenar;
            Tabla.SelectionChanged += ContabilidadMetEstadoNavegacion;
            Tabla.DataBindingComplete += ContabilidadMetEstadoNavegacion;
        }

        // Conecta los botones de navegacion de la tabla
        internal void ContabilidadMetAsignarNavegacion(Control Inicio, Control Anterior, Control Siguiente, Control Fin)
        {
            _Navegacion = new[] { Inicio, Anterior, Siguiente, Fin };
            Inicio.Click += (Remitente, Argumentos) => ContabilidadMetIrAFila(0);
            Anterior.Click += (Remitente, Argumentos) => ContabilidadMetIrAFila(ContabilidadFuncIndiceActual() - 1);
            Siguiente.Click += (Remitente, Argumentos) => ContabilidadMetIrAFila(ContabilidadFuncIndiceActual() + 1);
            Fin.Click += (Remitente, Argumentos) => ContabilidadMetIrAFila(_Tabla.Rows.Count - 1);
            ContabilidadMetEstadoNavegacion(null, EventArgs.Empty);
        }

        // Fila seleccionada
        private int ContabilidadFuncIndiceActual()
        {
            return _Tabla.CurrentRow == null ? -1 : _Tabla.CurrentRow.Index;
        }

        // Mueve la seleccion a una fila
        private void ContabilidadMetIrAFila(int Indice)
        {
            if (_Tabla.Rows.Count == 0) return;
            Indice = Math.Max(0, Math.Min(Indice, _Tabla.Rows.Count - 1));
            DataGridViewColumn Columna = _Tabla.CurrentCell != null ? _Tabla.Columns[_Tabla.CurrentCell.ColumnIndex]
                : _Tabla.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
            if (Columna == null) return;
            _Tabla.CurrentCell = _Tabla.Rows[Indice].Cells[Columna.Index];
            _Tabla.Focus();
            ContabilidadMetEstadoNavegacion(null, EventArgs.Empty);
        }

        // Habilita los botones segun la fila actual
        private void ContabilidadMetEstadoNavegacion(object Remitente, EventArgs Argumentos)
        {
            if (_Navegacion.Length == 0) return;
            int Total = _Tabla.Rows.Count;
            int Indice = ContabilidadFuncIndiceActual();
            _Navegacion[0].Enabled = Total > 0 && Indice != 0;
            _Navegacion[1].Enabled = Total > 0 && Indice > 0;
            _Navegacion[2].Enabled = Total > 0 && Indice < Total - 1;
            _Navegacion[3].Enabled = Total > 0 && Indice != Total - 1;
        }

        internal void ContabilidadMetCargar(DataTable Datos)
        {
            DataView Anterior = _Vista;
            _Vista = new DataView(Datos);
            _CampoOrden = string.Empty;
            _Descendente = false;
            _Tabla.DataSource = _Vista;
            if (Anterior != null) Anterior.Dispose();
            ContabilidadMetActualizar();
        }

        internal DataRow ContabilidadFuncFilaActual()
        {
            if (_Vista == null || _Tabla.CurrentRow == null) return null;
            DataRowView Fila = _Tabla.CurrentRow.DataBoundItem as DataRowView;
            return Fila == null ? null : Fila.Row;
        }

        internal void ContabilidadMetLimpiar()
        {
            _Tabla.DataSource = null;
            if (_Vista != null) { _Vista.Dispose(); _Vista = null; }
            ContabilidadMetEstadoNavegacion(null, EventArgs.Empty);
        }

        internal void ContabilidadMetActualizar()
        {
            if (_Vista == null) return;
            foreach (DataGridViewColumn Columna in _Tabla.Columns)
                Columna.HeaderCell.SortGlyphDirection = Columna.DataPropertyName == _CampoOrden
                    ? (_Descendente ? SortOrder.Descending : SortOrder.Ascending) : SortOrder.None;
            ContabilidadMetEstadoNavegacion(null, EventArgs.Empty);
        }

        private void ContabilidadMetOrdenar(object Remitente, DataGridViewCellMouseEventArgs Argumentos)
        {
            if (_Vista == null || Argumentos.ColumnIndex < 0) return;
            string Campo = _Tabla.Columns[Argumentos.ColumnIndex].DataPropertyName;
            if (!_Vista.Table.Columns.Contains(Campo)) return;
            _Descendente = _CampoOrden == Campo && !_Descendente;
            _CampoOrden = Campo;
            _Vista.Sort = "[" + Campo.Replace("\\", "\\\\").Replace("]", "\\]") + "] " + (_Descendente ? "DESC" : "ASC");
            ContabilidadMetActualizar();
        }

        public void Dispose()
        {
            _Tabla.ColumnHeaderMouseClick -= ContabilidadMetOrdenar;
            _Tabla.SelectionChanged -= ContabilidadMetEstadoNavegacion;
            _Tabla.DataBindingComplete -= ContabilidadMetEstadoNavegacion;
            if (_Vista != null) _Vista.Dispose();
        }
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
    }
}
