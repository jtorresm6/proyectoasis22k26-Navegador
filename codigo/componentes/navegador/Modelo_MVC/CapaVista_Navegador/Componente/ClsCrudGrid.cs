using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaEntidades_Navegador;

namespace CapaVista_Navegador
{
    // Se encarga del DataGridView: crearlo, mostrarlo, moverse entre filas
    public class ClsCrudGrid
    {
        private Form _Formulario;

        public DataGridView NavegadorDgvDatos { get; private set; }

        public ClsCrudGrid(Form Formulario)
        {
            this._Formulario = Formulario;
        }

        public void NavegadorMetMostrar(DataTable Datos)
        {
            if (NavegadorDgvDatos == null)
                NavegadorMetCrearGrid();

            NavegadorDgvDatos.DataSource = Datos;
            NavegadorDgvDatos.Visible = true;
            NavegadorDgvDatos.ReadOnly = true;

            foreach (DataGridViewColumn Columna in NavegadorDgvDatos.Columns)
                Columna.ReadOnly = true;
        }

        public void NavegadorMetOcultar()
        {
            if (NavegadorDgvDatos != null)
                NavegadorDgvDatos.Visible = false;
        }

        private void NavegadorMetCrearGrid()
        {
            NavegadorDgvDatos = new DataGridView();
            NavegadorDgvDatos.Name = "NavegadorDgvDatos";
            NavegadorDgvDatos.AllowUserToAddRows = false;
            NavegadorDgvDatos.AllowUserToDeleteRows = false;
            NavegadorDgvDatos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            NavegadorDgvDatos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            NavegadorDgvDatos.MultiSelect = false;
            NavegadorDgvDatos.ReadOnly = true;
            NavegadorDgvDatos.BackgroundColor = Color.White;
            NavegadorDgvDatos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            _Formulario.Controls.Add(NavegadorDgvDatos);
        }

        public void NavegadorMetPosicionar(int PosicionY)
        {
            if (NavegadorDgvDatos == null || !NavegadorDgvDatos.Visible)
                return;

            int Margen = 10;

            NavegadorDgvDatos.Location = new Point(Margen, PosicionY);

            NavegadorDgvDatos.Size = new Size(
                Math.Max(100, _Formulario.ClientSize.Width - (Margen * 2)),
                Math.Max(100, _Formulario.ClientSize.Height - PosicionY - Margen));

            NavegadorDgvDatos.BringToFront();
        }

        public int NavegadorFuncObtenerIndiceColumna(string NombreCampo)
        {
            if (NavegadorDgvDatos == null)
                return -1;

            foreach (DataGridViewColumn Columna in NavegadorDgvDatos.Columns)
            {
                if (string.Equals(Columna.DataPropertyName, NombreCampo, StringComparison.OrdinalIgnoreCase))
                    return Columna.Index;

                if (string.Equals(Columna.Name, NombreCampo, StringComparison.OrdinalIgnoreCase))
                    return Columna.Index;
            }

            return -1;
        }

        public string NavegadorFuncObtenerValor(DataGridViewRow Fila, string Campo)
        {
            int Indice = NavegadorFuncObtenerIndiceColumna(Campo);

            if (Indice < 0 || Fila == null)
                return "";

            object Valor = Fila.Cells[Indice].Value;
            return Valor == null || Valor == DBNull.Value ? "" : Convert.ToString(Valor);
        }

        // Lee de la fila seleccionada solo las columnas marcadas como PK en el esquema
        public Dictionary<string, string> NavegadorFuncObtenerClavesPrimarias(List<ClsColumnaInfo> Esquema, DataGridViewRow Fila)
        {
            Dictionary<string, string> Resultado = new Dictionary<string, string>();

            if (Esquema == null || Fila == null)
                return Resultado;

            foreach (ClsColumnaInfo Columna in Esquema)
            {
                if (Columna.EsPK)
                    Resultado[Columna.Nombre] = NavegadorFuncObtenerValor(Fila, Columna.Nombre);
            }

            return Resultado;
        }

        public void NavegadorMetInicio()
        {
            NavegadorMetSeleccionar(0);
        }

        public void NavegadorMetAnterior()
        {
            if (NavegadorDgvDatos == null || NavegadorDgvDatos.Rows.Count == 0)
                return;

            NavegadorMetSeleccionar(Math.Max(0, NavegadorFuncIndiceActual() - 1));
        }

        public void NavegadorMetSiguiente()
        {
            if (NavegadorDgvDatos == null || NavegadorDgvDatos.Rows.Count == 0)
                return;

            NavegadorMetSeleccionar(
                Math.Min(NavegadorDgvDatos.Rows.Count - 1, NavegadorFuncIndiceActual() + 1));
        }

        public void NavegadorMetFin()
        {
            if (NavegadorDgvDatos == null || NavegadorDgvDatos.Rows.Count == 0)
                return;

            NavegadorMetSeleccionar(NavegadorDgvDatos.Rows.Count - 1);
        }

        private int NavegadorFuncIndiceActual()
        {
            return NavegadorDgvDatos != null && NavegadorDgvDatos.CurrentRow != null
                ? NavegadorDgvDatos.CurrentRow.Index
                : 0;
        }

        private void NavegadorMetSeleccionar(int Indice)
        {
            if (NavegadorDgvDatos == null ||
                !NavegadorDgvDatos.Visible ||
                NavegadorDgvDatos.Rows.Count == 0 ||
                Indice < 0 ||
                Indice >= NavegadorDgvDatos.Rows.Count)
                return;

            NavegadorDgvDatos.ClearSelection();
            NavegadorDgvDatos.Rows[Indice].Selected = true;
            NavegadorDgvDatos.CurrentCell = NavegadorDgvDatos.Rows[Indice].Cells[0];

            if (NavegadorDgvDatos.FirstDisplayedScrollingRowIndex > Indice ||
                NavegadorDgvDatos.FirstDisplayedScrollingRowIndex +
                NavegadorDgvDatos.DisplayedRowCount(false) <= Indice)
            {
                NavegadorDgvDatos.FirstDisplayedScrollingRowIndex = Indice;
            }
        }
    }
}