using System;
using System.Data;
using System.Windows.Forms;

namespace CapaVista_EnlaceB.Formularios
{
    internal sealed class ClsPaginadorEnlace
    {
        private readonly DataGridView _BancarioDgvTabla;
        private readonly Button _BancarioBtnAnterior;
        private readonly Button _BancarioBtnSiguiente;
        private readonly Label _BancarioLblResumen;
        private int _Pagina;
        private string _Orden;
        private bool _Ascendente;
        internal DataTable Datos { get; private set; }

        internal ClsPaginadorEnlace(DataGridView Tabla, Button Anterior, Button Siguiente, Label Resumen)
        {
            _BancarioDgvTabla = Tabla;
            _BancarioBtnAnterior = Anterior;
            _BancarioBtnSiguiente = Siguiente;
            _BancarioLblResumen = Resumen;
            ClsEstiloEnlace.BancarioMetTabla(Tabla);
            Anterior.Click += BancarioMetAnterior;
            Siguiente.Click += BancarioMetSiguiente;
            Tabla.ColumnHeaderMouseClick += BancarioMetOrdenar;
            BancarioMetCargar(null);
        }

        internal void BancarioMetCargar(DataTable Registros)
        {
            Datos = Registros;
            _Pagina = 0;
            _Orden = "";
            BancarioMetMostrarPagina();
        }

        private void BancarioMetAnterior(object Remitente, EventArgs Argumentos)
        {
            if (_Pagina == 0)
                return;
            _Pagina--;
            BancarioMetMostrarPagina();
        }

        private void BancarioMetSiguiente(object Remitente, EventArgs Argumentos)
        {
            if (Datos == null || (_Pagina + 1) * 25 >= Datos.Rows.Count)
                return;
            _Pagina++;
            BancarioMetMostrarPagina();
        }

        private void BancarioMetOrdenar(object Remitente, DataGridViewCellMouseEventArgs Argumentos)
        {
            if (Datos == null || Argumentos.ColumnIndex < 0)
                return;
            string Columna = _BancarioDgvTabla.Columns[Argumentos.ColumnIndex].DataPropertyName;
            _Ascendente = _Orden != Columna || !_Ascendente;
            _Orden = Columna;
            _Pagina = 0;
            BancarioMetMostrarPagina();
        }

        private void BancarioMetMostrarPagina()
        {
            int Total = Datos == null ? 0 : Datos.Rows.Count;
            if (Datos != null)
            {
                var Vista = new DataView(Datos);
                if (!string.IsNullOrEmpty(_Orden))
                    Vista.Sort = "[" + _Orden.Replace("\\", "\\\\").Replace("]", "\\]") + "] " + (_Ascendente ? "ASC" : "DESC");
                var Pagina = Datos.Clone();
                for (int Indice = _Pagina * 25; Indice < Math.Min((_Pagina + 1) * 25, Vista.Count); Indice++)
                    Pagina.ImportRow(Vista[Indice].Row);
                _BancarioDgvTabla.DataSource = Pagina;
                foreach (DataGridViewColumn Columna in _BancarioDgvTabla.Columns)
                    Columna.HeaderCell.SortGlyphDirection = Columna.DataPropertyName == _Orden ? (_Ascendente ? SortOrder.Ascending : SortOrder.Descending) : SortOrder.None;
            }
            else
                _BancarioDgvTabla.DataSource = null;
            _BancarioBtnAnterior.Enabled = _Pagina > 0;
            _BancarioBtnSiguiente.Enabled = (_Pagina + 1) * 25 < Total;
            _BancarioLblResumen.Text = Total + " registros · Página " + (Total == 0 ? 0 : _Pagina + 1) + " de " + ((Total + 24) / 25);
        }
    }
}
