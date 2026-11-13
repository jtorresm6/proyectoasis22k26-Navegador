using System;
using System.Data;
using System.Globalization;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Filtra órdenes de compra por búsqueda, estado y rango de fechas.
 * ===================================================================
*/

namespace CapaControlador_OrdenesCompra.Servicios
{
    internal static class ClsEncabezadosOrdenesCompraFiltrar
    {
        private static readonly string[] _CamposBusqueda = { "codigoOrdenCompra", "nombreProveedor", "nitProveedor" };

        internal static DataTable ComprasFuncFiltrar(DataTable Datos, string Palabra, string Estado, DateTime? Inicio, DateTime? Fin)
        {
            if (Datos == null)
                throw new ArgumentNullException(nameof(Datos));
            if (Inicio.HasValue != Fin.HasValue || (Inicio.HasValue && Inicio.Value.Date > Fin.Value.Date))
                throw new ArgumentException("El rango de fechas no es válido.");
            bool FiltrarEstado = !string.IsNullOrEmpty(Estado) && Estado != "Todos";
            if (FiltrarEstado)
                ClsOrdenesCompraEstados.ComprasProcValidarFiltro(Estado);
            string Buscar = (Palabra ?? string.Empty).Trim();
            DataTable Resultado = Datos.Clone();
            foreach (DataRow Fila in Datos.Rows)
            {
                if (Fila.RowState == DataRowState.Deleted)
                    continue;
                if (FiltrarEstado && !string.Equals(Convert.ToString(Fila["estadoOrdenCompra"]), Estado, StringComparison.OrdinalIgnoreCase))
                    continue;
                DateTime Emision = Convert.ToDateTime(Fila["fechaEmisionOrdenCompra"]).Date;
                if (Inicio.HasValue && (Emision < Inicio.Value.Date || Emision > Fin.Value.Date))
                    continue;
                if (ComprasFuncCoincideBusqueda(Fila, Buscar))
                    Resultado.ImportRow(Fila);
            }
            return Resultado;
        }

        private static bool ComprasFuncCoincideBusqueda(DataRow Fila, string Buscar)
        {
            if (Buscar.Length == 0)
                return true;
            foreach (string Campo in _CamposBusqueda)
            {
                if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(Convert.ToString(Fila[Campo]), Buscar,
                    CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0)
                    return true;
            }
            return false;
        }
    }
}
