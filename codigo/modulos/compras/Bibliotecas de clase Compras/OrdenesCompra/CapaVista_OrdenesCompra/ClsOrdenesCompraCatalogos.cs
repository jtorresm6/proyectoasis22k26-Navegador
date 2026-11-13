using System;
using System.Data;
using CapaVista_Componentes.Componentes;
namespace CapaVista_OrdenesCompra
{
    internal static class ClsOrdenesCompraCatalogos
    {
        internal static void ComprasProcVincular(ComprasUsrComboBox Combo, object Datos, string Valor, string Texto)
        {
            Combo.BeginUpdate();
            try
            {
                Combo.ValueMember = Valor;
                Combo.DisplayMember = Texto;
                Combo.DataSource = Datos;
                Combo.SelectedIndex = -1;
            }
            finally { Combo.EndUpdate(); }
        }

        internal static DataRow ComprasFuncFila(ComprasUsrComboBox Combo)
        {
            var Fila = Combo.SelectedItem as DataRowView;
            return Combo.SelectedIndex < 0 || Fila == null || Fila.Row.RowState == DataRowState.Deleted
                || !string.Equals(Combo.Text, Combo.GetItemText(Combo.SelectedItem), StringComparison.CurrentCultureIgnoreCase) ? null : Fila.Row;
        }
    }
}
