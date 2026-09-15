//Donald Estuardo Osorio Pérez 
//Carnet: 0901-23-17982
//14/09/2026

//aca comienza la creacion de mi codigo
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaEntidades_Navegador;

namespace CapaVista_Navegador
{
    // Clase encargada de la gestión integral del DataGridView en el navegador (creación, diseño y navegación)
    public class ClsCrudGrid
    {
        private Form Formulario;

        public DataGridView NavegadorDgvDatos { get; private set; }

        // Constructor que recibe el formulario padre donde se alojará el control
        public ClsCrudGrid(Form _Formulario)
        {
            this.Formulario = _Formulario;
        }

        // Carga la información desde un DataTable y asegura que la grilla sea solo de lectura
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

        // Oculta el DataGridView si se encuentra instanciado
        public void NavegadorMetOcultar()
        {
            if (NavegadorDgvDatos != null)
                NavegadorDgvDatos.Visible = false;
        }

        // Instancia y configura las propiedades visuales y de comportamiento del DataGridView dinámicamente
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

            Formulario.Controls.Add(NavegadorDgvDatos);
        }

        // Ajusta las dimensiones y la posición vertical del DataGridView según el tamaño del formulario
        public void NavegadorMetPosicionar(int PosicionY)
        {
            if (NavegadorDgvDatos == null || !NavegadorDgvDatos.Visible)
                return;

            int Margen = 10;

            NavegadorDgvDatos.Location = new Point(Margen, PosicionY);

            NavegadorDgvDatos.Size = new Size(
                Math.Max(100, Formulario.ClientSize.Width - (Margen * 2)),
                Math.Max(100, Formulario.ClientSize.Height - PosicionY - Margen));

            NavegadorDgvDatos.BringToFront();
        }

        // Busca la posición de una columna por su DataPropertyName o por su Name
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

        // Retorna el valor string de una celda específica evaluando posibles valores nulos
        public string NavegadorFuncObtenerValor(DataGridViewRow Fila, string Campo)
        {
            int Indice = NavegadorFuncObtenerIndiceColumna(Campo);

            if (Indice < 0 || Fila == null)
                return "";

            object Valor = Fila.Cells[Indice].Value;
            return Valor == null || Valor == DBNull.Value ? "" : Convert.ToString(Valor);
        }

        // Extrae las llaves primarias de la fila seleccionada guiándose por el esquema entregado
        public Dictionary<string, string> NavegadorFuncObtenerClavesPrimarias(List<ClsColumnaInfo> ClsEsquema, DataGridViewRow Fila)
        {
            Dictionary<string, string> Resultado = new Dictionary<string, string>();

            if (ClsEsquema == null || Fila == null)
                return Resultado;

            foreach (ClsColumnaInfo Col in ClsEsquema)
            {
                if (Col.EsPK)
                    Resultado[Col.Nombre] = NavegadorFuncObtenerValor(Fila, Col.Nombre);
            }

            return Resultado;
        }

        // Mueve la selección al primer registro
        public void NavegadorMetInicio()
        {
            NavegadorMetSeleccionar(0);
        }

        // Mueve la selección al registro anterior
        public void NavegadorMetAnterior()
        {
            if (NavegadorDgvDatos == null || NavegadorDgvDatos.Rows.Count == 0) return;
            NavegadorMetSeleccionar(Math.Max(0, NavegadorFuncIndiceActual() - 1));
        }

        // Mueve la selección al siguiente registro
        public void NavegadorMetSiguiente()
        {
            if (NavegadorDgvDatos == null || NavegadorDgvDatos.Rows.Count == 0) return;
            NavegadorMetSeleccionar(Math.Min(NavegadorDgvDatos.Rows.Count - 1, NavegadorFuncIndiceActual() + 1));
        }

        // Mueve la selección al último registro
        public void NavegadorMetFin()
        {
            if (NavegadorDgvDatos == null || NavegadorDgvDatos.Rows.Count == 0) return;
            NavegadorMetSeleccionar(NavegadorDgvDatos.Rows.Count - 1);
        }

        // Retorna el índice de la fila actualmente seleccionada
        private int NavegadorFuncIndiceActual()
        {
            return NavegadorDgvDatos != null && NavegadorDgvDatos.CurrentRow != null ? NavegadorDgvDatos.CurrentRow.Index : 0;
        }

        // Selecciona la fila indicada y ajusta el scroll para mantenerla visible en pantalla
        private void NavegadorMetSeleccionar(int Indice)
        {
            if (NavegadorDgvDatos == null || !NavegadorDgvDatos.Visible || NavegadorDgvDatos.Rows.Count == 0 || Indice < 0 || Indice >= NavegadorDgvDatos.Rows.Count)
                return;

            NavegadorDgvDatos.ClearSelection();
            NavegadorDgvDatos.Rows[Indice].Selected = true;
            NavegadorDgvDatos.CurrentCell = NavegadorDgvDatos.Rows[Indice].Cells[0];

            if (NavegadorDgvDatos.FirstDisplayedScrollingRowIndex > Indice ||
                NavegadorDgvDatos.FirstDisplayedScrollingRowIndex + NavegadorDgvDatos.DisplayedRowCount(false) <= Indice)
                NavegadorDgvDatos.FirstDisplayedScrollingRowIndex = Indice;
        }
    }
}

//aca finaliza mi creacion de codigo

//Donald Estuardo Osorio Pérez 
//Carnet: 0901-23-17982
//14/09/2026