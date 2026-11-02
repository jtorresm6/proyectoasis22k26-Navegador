/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 14/10/2026
 * ==================================================================
 * Propósito :
 *  Tabla (DataGridView) reutilizable con el estilo de Contabilidad:
 *  encabezado #23323D, filas alternas #F6F7F8 y selección #D4DEE4.
 *  Sin filas nuevas automáticas ni encabezados de fila.
 * ===================================================================
*/

using System.Drawing;
using System.Windows.Forms;

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaVista_Contabilidad.Componentes
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
    internal class ClsTablaContabilidad : DataGridView
    // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
    {
        public ClsTablaContabilidad()
        {
            DoubleBuffered = true;
            BackgroundColor = Color.White;
            BorderStyle = BorderStyle.None;
            GridColor = Color.FromArgb(212, 222, 228);
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            RowHeadersVisible = false;
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToResizeRows = false;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            MultiSelect = false;
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            EnableHeadersVisualStyles = false;
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ColumnHeadersHeight = 38;
            ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 50, 61);
            ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 50, 61);
            ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 6, 0);

            RowTemplate.Height = 34;
            DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            DefaultCellStyle.ForeColor = Color.FromArgb(35, 50, 61);
            DefaultCellStyle.BackColor = Color.White;
            DefaultCellStyle.SelectionBackColor = Color.FromArgb(212, 222, 228);
            DefaultCellStyle.SelectionForeColor = Color.FromArgb(35, 50, 61);
            DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
            AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(246, 247, 248);
        }
    }
}
