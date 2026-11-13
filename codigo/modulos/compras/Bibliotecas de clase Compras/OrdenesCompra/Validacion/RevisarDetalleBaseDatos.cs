using System;
using CapaModelo_OrdenesCompra.Repositorios;

class RevisarDetalleBaseDatos
{
    static int Main()
    {
        try
        {
            var Repositorio = new ClsOrdenesCompraRepositorio();
            using (var Bodegas = Repositorio.ComprasFuncListarBodegas())
            {
                if (!Bodegas.Columns.Contains("nombreSucursal")) throw new Exception("Falta el nombre de sucursal en la consulta de bodegas.");
                Console.WriteLine("Bodegas consultadas con nombre de sucursal: " + Bodegas.Rows.Count);
            }
            using (var Ordenes = Repositorio.ComprasFuncListar())
            {
                Console.WriteLine("Encabezados consultados: " + Ordenes.Rows.Count);
                int Id = Ordenes.Rows.Count == 0 ? 1 : Convert.ToInt32(Ordenes.Rows[0]["idOrdenCompra"]);
                using (var Detalles = Repositorio.ComprasFuncListarDetalles(Id))
                    Console.WriteLine("Consulta de detalles correcta. Filas: " + Detalles.Rows.Count);
            }
            return 0;
        }
        catch (Exception Error) { Console.WriteLine(Error.Message); return 1; }
    }
}
