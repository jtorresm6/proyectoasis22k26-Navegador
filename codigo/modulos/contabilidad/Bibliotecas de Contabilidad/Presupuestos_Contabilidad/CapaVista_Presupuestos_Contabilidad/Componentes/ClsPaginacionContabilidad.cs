/* Área: Contabilidad. Autor: Gabriel André Guillén Pocón. Carné: 0901-23-1998.
 * Fecha: 14/10/2026. Propósito: paginación y orden global para listados de consulta (EST-10 §8). */
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaVista_Contabilidad.Componentes
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    internal sealed class ClsPaginacionContabilidad : IDisposable
    {
        private readonly ClsTablaContabilidad _Tabla;
        private readonly Panel _Pie;
        private readonly Label _Resumen;
        private readonly ClsBotonContabilidad _Anterior;
        private readonly ClsBotonContabilidad _Siguiente;
        private DataView _Vista;
        private DataTable _PaginaVisible;
        private int _Pagina = 1;
        private const int _TamanoPagina = 10;
        private string _CampoOrden = string.Empty;
        private bool _Descendente;

        internal ClsPaginacionContabilidad(ClsTablaContabilidad Tabla)
        {
            _Tabla = Tabla;
            _Tabla.Height -= 34;
            _Pie = new Panel { Name = "ContabilidadPnlPaginacion" + Tabla.Name, BackColor = Color.White,
                Location = new Point(Tabla.Left, Tabla.Bottom + 4), Size = new Size(Tabla.Width, 28) };
            _Resumen = new Label { Name = "ContabilidadLblPaginacion" + Tabla.Name, Font = new Font("Tahoma", 9F),
                ForeColor = Color.FromArgb(35, 50, 61), Location = new Point(0, 5), Size = new Size(Tabla.Width - 250, 22) };
            _Anterior = new ClsBotonContabilidad { Name = "ContabilidadBtnAnterior" + Tabla.Name, Text = "Anterior", Font = new Font("Segoe UI", 9F),
                Size = new Size(108, 28), Location = new Point(Tabla.Width - 228, 0), Estilo = EstiloBotonContabilidad.Secundario };
            _Siguiente = new ClsBotonContabilidad { Name = "ContabilidadBtnSiguiente" + Tabla.Name, Text = "Siguiente", Font = new Font("Segoe UI", 9F),
                Size = new Size(108, 28), Location = new Point(Tabla.Width - 108, 0), Estilo = EstiloBotonContabilidad.Secundario };
            _Anterior.Click += ContabilidadMetAnterior;
            _Siguiente.Click += ContabilidadMetSiguiente;
            _Pie.Controls.AddRange(new Control[] { _Resumen, _Anterior, _Siguiente });
            Tabla.Parent.Controls.Add(_Pie);
            foreach (DataGridViewColumn Columna in Tabla.Columns) Columna.SortMode = DataGridViewColumnSortMode.Programmatic;
            Tabla.ColumnHeaderMouseClick += ContabilidadMetOrdenar;
        }

        internal void ContabilidadMetCargar(DataTable Datos)
        {
            if (_Vista != null) _Vista.Dispose();
            _Vista = new DataView(Datos);
            _CampoOrden = string.Empty;
            _Descendente = false;
            _Pagina = 1;
            ContabilidadMetActualizar();
        }

        internal DataRow ContabilidadFuncFilaActual()
        {
            if (_Vista == null || _Tabla.CurrentRow == null) return null;
            int Indice = (_Pagina - 1) * _TamanoPagina + _Tabla.CurrentRow.Index;
            return Indice < _Vista.Count ? _Vista[Indice].Row : null;
        }

        internal void ContabilidadMetLimpiar()
        {
            _Tabla.DataSource = null;
            if (_Vista != null) { _Vista.Dispose(); _Vista = null; }
            if (_PaginaVisible != null) { _PaginaVisible.Dispose(); _PaginaVisible = null; }
            _Resumen.Text = "Registros: 0 · Página 1 de 1";
            _Anterior.Enabled = false;
            _Siguiente.Enabled = false;
        }

        internal void ContabilidadMetActualizar()
        {
            if (_Vista == null) return;
            int Paginas = Math.Max(1, (_Vista.Count + _TamanoPagina - 1) / _TamanoPagina);
            _Pagina = Math.Min(_Pagina, Paginas);
            DataTable NuevaPagina = _Vista.Table.Clone();
            int Inicio = (_Pagina - 1) * _TamanoPagina;
            int Fin = Math.Min(Inicio + _TamanoPagina, _Vista.Count);
            for (int Indice = Inicio; Indice < Fin; Indice++) NuevaPagina.ImportRow(_Vista[Indice].Row);
            DataTable Anterior = _PaginaVisible;
            _PaginaVisible = NuevaPagina;
            _Tabla.DataSource = NuevaPagina;
            if (Anterior != null) Anterior.Dispose();
            foreach (DataGridViewColumn Columna in _Tabla.Columns)
                Columna.HeaderCell.SortGlyphDirection = Columna.DataPropertyName == _CampoOrden
                    ? (_Descendente ? SortOrder.Descending : SortOrder.Ascending) : SortOrder.None;
            _Resumen.Text = "Registros: " + _Vista.Count + " · Página " + _Pagina + " de " + Paginas;
            _Anterior.Enabled = _Pagina > 1;
            _Siguiente.Enabled = _Pagina < Paginas;
        }

        private void ContabilidadMetAnterior(object Remitente, EventArgs Argumentos)
        {
            if (_Pagina > 1) { _Pagina--; ContabilidadMetActualizar(); }
        }

        private void ContabilidadMetSiguiente(object Remitente, EventArgs Argumentos)
        {
            if (_Vista != null && _Pagina * _TamanoPagina < _Vista.Count) { _Pagina++; ContabilidadMetActualizar(); }
        }

        private void ContabilidadMetOrdenar(object Remitente, DataGridViewCellMouseEventArgs Argumentos)
        {
            if (_Vista == null || Argumentos.ColumnIndex < 0) return;
            string Campo = _Tabla.Columns[Argumentos.ColumnIndex].DataPropertyName;
            if (!_Vista.Table.Columns.Contains(Campo)) return;
            _Descendente = _CampoOrden == Campo && !_Descendente;
            _CampoOrden = Campo;
            _Vista.Sort = "[" + Campo.Replace("\\", "\\\\").Replace("]", "\\]") + "] " + (_Descendente ? "DESC" : "ASC");
            _Pagina = 1;
            ContabilidadMetActualizar();
        }

        public void Dispose()
        {
            _Tabla.ColumnHeaderMouseClick -= ContabilidadMetOrdenar;
            _Anterior.Click -= ContabilidadMetAnterior;
            _Siguiente.Click -= ContabilidadMetSiguiente;
            if (_Vista != null) _Vista.Dispose();
            if (_PaginaVisible != null) _PaginaVisible.Dispose();
            _Pie.Dispose();
        }
    }
}
