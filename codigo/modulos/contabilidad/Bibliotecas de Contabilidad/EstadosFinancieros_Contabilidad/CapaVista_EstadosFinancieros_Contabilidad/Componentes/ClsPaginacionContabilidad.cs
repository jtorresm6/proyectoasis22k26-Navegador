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
        // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 28/10/2026
        private readonly ClsTablaContabilidad _Tabla;
        private DataView _Vista;
        private string _CampoOrden = string.Empty;
        private bool _Descendente;

        internal ClsPaginacionContabilidad(ClsTablaContabilidad Tabla)
        {
            _Tabla = Tabla;
            _Tabla.ScrollBars = ScrollBars.Both;
            foreach (DataGridViewColumn Columna in Tabla.Columns) Columna.SortMode = DataGridViewColumnSortMode.Programmatic;
            Tabla.ColumnHeaderMouseClick += ContabilidadMetOrdenar;
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
        }

        internal void ContabilidadMetActualizar()
        {
            if (_Vista == null) return;
            foreach (DataGridViewColumn Columna in _Tabla.Columns)
                Columna.HeaderCell.SortGlyphDirection = Columna.DataPropertyName == _CampoOrden
                    ? (_Descendente ? SortOrder.Descending : SortOrder.Ascending) : SortOrder.None;
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
            if (_Vista != null) _Vista.Dispose();
        }
        // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 28/10/2026
    }
}
