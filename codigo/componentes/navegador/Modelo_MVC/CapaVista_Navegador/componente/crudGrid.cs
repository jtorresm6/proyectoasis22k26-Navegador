using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaEntidades_Navegador;

namespace CapaVista_Navegador
{
    // Se encarga del DataGridView: crearlo, mostrarlo, moverse entre filas
    public class crudGrid
    {
        private Form formulario;

        public DataGridView Grid { get; private set; }

        public crudGrid(Form formulario)
        {
            this.formulario = formulario;
        }

        public void Mostrar(DataTable datos)
        {
            if (Grid == null)
                CrearGrid();

            Grid.DataSource = datos;
            Grid.Visible = true;
            Grid.ReadOnly = true;

            foreach (DataGridViewColumn columna in Grid.Columns)
                columna.ReadOnly = true;
        }

        public void Ocultar()
        {
            if (Grid != null)
                Grid.Visible = false;
        }

        private void CrearGrid()
        {
            Grid = new DataGridView();
            Grid.Name = "dgvDatos";
            Grid.AllowUserToAddRows = false;
            Grid.AllowUserToDeleteRows = false;
            Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            Grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Grid.MultiSelect = false;
            Grid.ReadOnly = true;
            Grid.BackgroundColor = Color.White;
            Grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            formulario.Controls.Add(Grid);
        }

        public void Posicionar(int posicionY)
        {
            if (Grid == null || !Grid.Visible)
                return;

            int margen = 10;

            Grid.Location = new Point(margen, posicionY);

            Grid.Size = new Size(
                Math.Max(100, formulario.ClientSize.Width - (margen * 2)),
                Math.Max(100, formulario.ClientSize.Height - posicionY - margen));

            Grid.BringToFront();
        }

        public int ObtenerIndiceColumna(string nombreCampo)
        {
            if (Grid == null)
                return -1;

            foreach (DataGridViewColumn columna in Grid.Columns)
            {
                if (string.Equals(columna.DataPropertyName, nombreCampo, StringComparison.OrdinalIgnoreCase))
                    return columna.Index;

                if (string.Equals(columna.Name, nombreCampo, StringComparison.OrdinalIgnoreCase))
                    return columna.Index;
            }

            return -1;
        }

        public string ObtenerValor(DataGridViewRow fila, string campo)
        {
            int indice = ObtenerIndiceColumna(campo);

            if (indice < 0 || fila == null)
                return "";

            object valor = fila.Cells[indice].Value;
            return valor == null || valor == DBNull.Value ? "" : Convert.ToString(valor);
        }

        // Lee de la fila seleccionada solo las columnas marcadas como PK en el esquema
        public Dictionary<string, string> ObtenerClavesPrimarias(List<columnaInfo> esquema, DataGridViewRow fila)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();

            if (esquema == null || fila == null)
                return resultado;

            foreach (columnaInfo col in esquema)
            {
                if (col.EsPK)
                    resultado[col.Nombre] = ObtenerValor(fila, col.Nombre);
            }

            return resultado;
        }

        public void Inicio()
        {
            Seleccionar(0);
        }

        public void Anterior()
        {
            if (Grid == null || Grid.Rows.Count == 0) return;
            Seleccionar(Math.Max(0, IndiceActual() - 1));
        }

        public void Siguiente()
        {
            if (Grid == null || Grid.Rows.Count == 0) return;
            Seleccionar(Math.Min(Grid.Rows.Count - 1, IndiceActual() + 1));
        }

        public void Fin()
        {
            if (Grid == null || Grid.Rows.Count == 0) return;
            Seleccionar(Grid.Rows.Count - 1);
        }

        private int IndiceActual()
        {
            return Grid != null && Grid.CurrentRow != null ? Grid.CurrentRow.Index : 0;
        }

        private void Seleccionar(int indice)
        {
            if (Grid == null || !Grid.Visible || Grid.Rows.Count == 0 || indice < 0 || indice >= Grid.Rows.Count)
                return;

            Grid.ClearSelection();
            Grid.Rows[indice].Selected = true;
            Grid.CurrentCell = Grid.Rows[indice].Cells[0];

            if (Grid.FirstDisplayedScrollingRowIndex > indice ||
                Grid.FirstDisplayedScrollingRowIndex + Grid.DisplayedRowCount(false) <= indice)
                Grid.FirstDisplayedScrollingRowIndex = indice;
        }
    }
}