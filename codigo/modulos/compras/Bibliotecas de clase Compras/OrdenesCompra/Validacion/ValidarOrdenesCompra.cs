using System;
using System.Data;
using CapaControlador_OrdenesCompra;
using CapaModelo_OrdenesCompra.Contratos;
using CapaModelo_OrdenesCompra.Entidades;

internal static class ValidarOrdenesCompra
{
    private static int _Pruebas;
    private static void ComprasProcVerificar(bool Condicion)
    {
        if (!Condicion) throw new Exception("Falló la prueba " + (_Pruebas + 1));
        _Pruebas++;
    }
    private static void ComprasProcRechazar<T>(Action Operacion) where T : Exception
    {
        try { Operacion(); }
        catch (T) { _Pruebas++; return; }
        throw new Exception("Se esperaba " + typeof(T).Name);
    }
    internal static DataTable ComprasFuncDatos()
    {
        var Datos = new DataTable();
        Datos.Columns.Add("idOrdenCompra", typeof(int));
        Datos.Columns.Add("codigoOrdenCompra", typeof(string));
        Datos.Columns.Add("idProveedor", typeof(int));
        Datos.Columns.Add("idPlazo", typeof(int));
        Datos.Columns.Add("idMoneda", typeof(int));
        Datos.Columns.Add("fechaEmisionOrdenCompra", typeof(DateTime));
        Datos.Columns.Add("fechaVencimientoOrdenCompra", typeof(DateTime));
        Datos.Columns.Add("tipoCambioOrdenCompra", typeof(decimal));
        Datos.Columns.Add("subtotalOrdenCompra", typeof(decimal));
        Datos.Columns.Add("descuentoOrdenCompra", typeof(decimal));
        Datos.Columns.Add("totalOrdenCompra", typeof(decimal));
        foreach (string Campo in new[] { "comentarioOrdenCompra", "estadoOrdenCompra", "nombreProveedor",
            "nitProveedor", "telefonoProveedor", "correoProveedor", "direccionProveedor", "nombrePlazo" })
            Datos.Columns.Add(Campo, typeof(string));
        Datos.Columns.Add("estadoProveedor", typeof(bool));
        Datos.Columns.Add("diasPlazo", typeof(int));
        foreach (string Estado in new[] { "Pendiente", "Autorizada", "Rechazada", "Anulada", "Recibida" })
        {
            Datos.Rows.Add(Datos.Rows.Count + 1, "OC-2026-" + (Datos.Rows.Count + 1).ToString("D3"), 3, 2, 1, new DateTime(2026, 10, 1), new DateTime(2026, 10, 31),
                1.234567M, 100.1234M, 10M, 90.1234M, "Prueba", Estado, "José", "123", "456", "a@b.c", "Dirección", "30 días", true, 30);
        }
        Datos.Columns.Add("impuestoOrdenCompra", typeof(decimal)).DefaultValue = 0M;
        Datos.Columns.Add("bloqueadoOrdenCompra", typeof(bool)).DefaultValue = false;
        foreach (DataRow Fila in Datos.Rows)
        {
            Fila["impuestoOrdenCompra"] = 0M;
            Fila["bloqueadoOrdenCompra"] = false;
        }
        return Datos;
    }
    private static void Main()
    {
        using (var Datos = ComprasFuncDatos())
        {
            var Repositorio = new RepositorioPrueba(Datos);
            var Controlador = new ClsOrdenesCompra(Repositorio);
            using (var Todas = Controlador.ComprasFuncListar())
                ComprasProcVerificar(Todas.Rows.Count == 5);
            using (var Todas = Controlador.ComprasFuncListar(" Todos "))
                ComprasProcVerificar(Todas.Rows.Count == 5);
            using (var Todas = Controlador.ComprasFuncListar(null))
                ComprasProcVerificar(Todas.Rows.Count == 5);
            using (var Pendientes = Controlador.ComprasFuncListar(" pendiente "))
                ComprasProcVerificar(Pendientes.Rows.Count == 1);
            ComprasProcRechazar<ArgumentException>(() => Controlador.ComprasFuncListar("Pagada"));
            ComprasProcRechazar<ArgumentException>(() => Controlador.ComprasProcCambiarEstado(1, "Todos"));
            ComprasProcRechazar<ArgumentException>(() => Controlador.ComprasProcCambiarEstado(1, null));
            ComprasProcRechazar<ArgumentOutOfRangeException>(() => Controlador.ComprasProcCambiarEstado(0, "Autorizada"));
            ComprasProcRechazar<InvalidOperationException>(() => Controlador.ComprasProcCambiarEstado(99, "Autorizada"));
            Controlador.ComprasProcCambiarEstado(1, " autorizada ");
            ComprasProcVerificar(Controlador.ComprasFuncObtener(1).Estado == "Autorizada");
            Controlador.ComprasProcCambiarEstado(1, "Autorizada");
            ComprasProcVerificar(Controlador.ComprasFuncObtener(1).Estado == "Autorizada");
            Controlador.ComprasProcCambiarEstado(1, "Pendiente");
            foreach (string Estado in new[] { "Pendiente", "Autorizada", "Rechazada", "Anulada", "Recibida" })
            {
                using (var Resultado = Controlador.ComprasFuncFiltrar(Datos, "jose", Estado, null, null))
                    ComprasProcVerificar(Resultado.Rows.Count == 1);
            }
            using (var Resultado = Controlador.ComprasFuncFiltrar(Datos, "2", "Todos", null, null))
                ComprasProcVerificar(Resultado.Rows.Count == 5);
            using (var Resultado = Controlador.ComprasFuncFiltrar(Datos, "", "Todos", new DateTime(2026, 10, 2), new DateTime(2026, 10, 30)))
                ComprasProcVerificar(Resultado.Rows.Count == 0);
            ComprasProcRechazar<ArgumentException>(() => Controlador.ComprasFuncFiltrar(Datos, "", "Pagada", null, null));
            ComprasProcRechazar<ArgumentException>(() => Controlador.ComprasFuncFiltrar(Datos, "", "Todos", DateTime.Today, null));
            ComprasProcRechazar<ArgumentException>(() => Controlador.ComprasFuncFiltrar(Datos, "", "Todos", DateTime.Today, DateTime.Today.AddDays(-1)));
            ComprasProcRechazar<ArgumentOutOfRangeException>(() => Controlador.ComprasFuncObtener(0));
            using (var Resultado = Controlador.ComprasFuncFiltrar(Datos, "oc-2026-003", "Todos", null, null))
                ComprasProcVerificar(Resultado.Rows.Count == 1 && Convert.ToInt32(Resultado.Rows[0]["idOrdenCompra"]) == 3);
            var Orden = Controlador.ComprasFuncObtener(1);
            ComprasProcVerificar(Orden.CodigoOrdenCompra == "1" && Orden.NumeroOrden == Orden.CodigoOrdenCompra);
            ComprasProcVerificar(Orden.IdOrdenCompra == 1 && Orden.Subtotal == 100.1234M && Orden.Descuento == 10M);
            ComprasProcVerificar(Orden.NombrePlazo == "30 días" && Orden.TipoCambio == 1.234567M && Orden.Comentario == "Prueba");
            ComprasProcRechazar<ArgumentException>(() => Controlador.ComprasProcActualizarVencimiento(1, new DateTime(2026, 9, 30)));
            Controlador.ComprasProcActualizarVencimiento(1, new DateTime(2026, 11, 1));
            ComprasProcVerificar(Repositorio.Vencimiento == new DateTime(2026, 11, 1));
            Datos.Rows[0]["estadoOrdenCompra"] = "Recibida";
            ComprasProcRechazar<InvalidOperationException>(() => Controlador.ComprasProcActualizarVencimiento(1, DateTime.Today));
            ComprasProcRechazar<ArgumentOutOfRangeException>(() => Controlador.ComprasFuncCrear("OC-2026-001", 1, 1, 1, DateTime.Today, DateTime.Today, 1M, 100M, 101M, ""));
            ComprasProcRechazar<ArgumentException>(() => Controlador.ComprasFuncCrear("OC-2026-001", 1, 1, 1, DateTime.Today, DateTime.Today, 0M, 100M, 0M, ""));
            ComprasProcRechazar<ArgumentOutOfRangeException>(() => Controlador.ComprasFuncCrear("OC-2026-001", 1, 1, 1, DateTime.Today, DateTime.Today, 1.0000001M, 100M, 0M, ""));
            ComprasProcRechazar<ArgumentOutOfRangeException>(() => Controlador.ComprasFuncCrear("OC-2026-001", 1, 1, 1, DateTime.Today, DateTime.Today, 1M, 100.00001M, 0M, ""));
            ComprasProcRechazar<ArgumentException>(() => Controlador.ComprasFuncCrear("OC-2026-001", 1, 1, 1, DateTime.Today, DateTime.Today, 1M, 100M, 0M, new string('x', 501)));
            ComprasProcVerificar(Controlador.ComprasFuncCrear("OC-2026-001", 1, 1, 1, DateTime.Today, DateTime.Today, 1.123456M, 99999999.9999M, 0M, " prueba ") == 42);
            ComprasProcVerificar(Repositorio.Comentario == "prueba");
            ComprasProcVerificar(Repositorio.CodigoCreado == string.Empty);
            ComprasProcRechazar<ArgumentException>(() => Controlador.ComprasFuncCrear(null, 1, 1, 1, DateTime.Today, DateTime.Today, 1M, 100M, 0M, ""));
            ComprasProcRechazar<ArgumentException>(() => Controlador.ComprasFuncCrear("   ", 1, 1, 1, DateTime.Today, DateTime.Today, 1M, 100M, 0M, ""));
            ComprasProcRechazar<ArgumentException>(() => Controlador.ComprasFuncCrear(new string('A', 31), 1, 1, 1, DateTime.Today, DateTime.Today, 1M, 100M, 0M, ""));
            Controlador.ComprasFuncCrear("  000-OC-001  ", 1, 1, 1, DateTime.Today, DateTime.Today, 1M, 100M, 0M, "");
            ComprasProcVerificar(Repositorio.CodigoCreado == string.Empty);
            Controlador.ComprasFuncCrear(new string('A', 30), 1, 1, 1, DateTime.Today, DateTime.Today, 1M, 100M, 0M, "");
            ComprasProcVerificar(Repositorio.CodigoCreado == string.Empty);
            Datos.Rows[0]["estadoOrdenCompra"] = "Pendiente";
            ComprasProcVerificar(Controlador.ComprasFuncAnular(1).Estado == "Anulada");
        }
        Console.WriteLine(_Pruebas + " pruebas correctas.");
    }

    internal sealed class RepositorioPrueba : IRepositorioOrdenesCompra
    {
        private readonly DataTable _Datos;
        public string EstadoConsultado { get; private set; }
        public bool RechazarCambio { get; set; }
        public bool RechazarConsulta { get; set; }
        public bool RechazarCatalogos { get; set; }
        public bool RechazarDetalles { get; set; }
        public int ConsultasProductos { get; private set; }
        public int ConsultasDetalles { get; private set; }
        public DateTime Vencimiento { get; private set; }
        public string Comentario { get; private set; }
        public string CodigoCreado { get; private set; }
        public RepositorioPrueba(DataTable Datos) { _Datos = Datos; }
        public DataTable ComprasFuncListar()
        {
            if (RechazarConsulta)
                throw new InvalidOperationException("Consulta rechazada para la prueba.");
            return _Datos.Copy();
        }
        public DataTable ComprasFuncListar(string Estado)
        {
            EstadoConsultado = Estado;
            if (RechazarConsulta)
                throw new InvalidOperationException("Consulta rechazada para la prueba.");
            var Resultado = _Datos.Clone();
            foreach (DataRow Fila in _Datos.Rows)
                if (Convert.ToString(Fila["estadoOrdenCompra"]) == Estado)
                    Resultado.ImportRow(Fila);
            return Resultado;
        }
        public void ComprasProcCambiarEstado(int Id, string Estado)
        {
            if (RechazarCambio)
                throw new InvalidOperationException("Cambio rechazado para la prueba.");
            if (Id > _Datos.Rows.Count)
                throw new InvalidOperationException("La orden de compra no existe.");
            _Datos.Rows[Id - 1]["estadoOrdenCompra"] = Estado;
        }
        public DataTable ComprasFuncListarDetalles(int Id)
        {
            ConsultasDetalles++;
            if (RechazarDetalles) throw new InvalidOperationException("Detalles rechazados para la prueba.");
            return new DataTable();
        }
        public DataTable ComprasFuncListarSucursales()
        {
            var Datos = new DataTable();
            Datos.Columns.Add("idSucursal", typeof(int));
            Datos.Columns.Add("nombreSucursal", typeof(string));
            Datos.Rows.Add(1, "Central");
            Datos.Rows.Add(2, "Norte");
            return Datos;
        }

        public DataTable ComprasFuncListarBodegas()
        {
            var Datos = new DataTable();
            Datos.Columns.Add("idBodega", typeof(int));
            Datos.Columns.Add("nombreBodega", typeof(string));
            Datos.Columns.Add("Fk_Sucursal_Codigo", typeof(int));
            Datos.Columns.Add("paisBodega", typeof(string));
            Datos.Rows.Add(11, "Bodega Central", 1, "Guatemala");
            Datos.Rows.Add(12, "Bodega Norte", 2, "Mexico");
            return Datos;
        }

        public DataTable ComprasFuncListarProductos()
        {
            ConsultasProductos++;
            if (RechazarCatalogos) throw new InvalidOperationException("Catalogos rechazados para la prueba.");
            var Datos = new DataTable();
            Datos.Columns.Add("idProducto", typeof(int));
            Datos.Columns.Add("skuProducto", typeof(string));
            Datos.Columns.Add("nombreProducto", typeof(string));
            Datos.Columns.Add("descripcionProducto", typeof(string));
            Datos.Columns.Add("pesoProducto", typeof(decimal));
            Datos.Columns.Add("precioCostoProducto", typeof(decimal));
            Datos.Rows.Add(1, "SKU-001", "Producto Uno", "Descripcion Uno", 1.5M, 77M);
            Datos.Rows.Add(2, "SKU-002", "Producto Dos", "Descripcion Dos", 2M, 154M);
            return Datos;
        }

        public DataTable ComprasFuncListarMonedas()
        {
            var Datos = new DataTable();
            Datos.Columns.Add("idMoneda", typeof(long));
            Datos.Columns.Add("codigoMoneda", typeof(string));
            Datos.Columns.Add("nombreMoneda", typeof(string));
            Datos.Columns.Add("simboloMoneda", typeof(string));
            Datos.Rows.Add(10L, "GTQ", "Quetzal", "Q");
            Datos.Rows.Add(20L, "USD", "Dolar", "$");
            Datos.Rows.Add(30L, "EUR", "Euro", "EUR");
            Datos.Rows.Add(40L, "JPY", "Yen", "JPY");
            return Datos;
        }

        public DataTable ComprasFuncListarPlazos()
        {
            var Datos = new DataTable();
            Datos.Columns.Add("idPlazo", typeof(int));
            Datos.Columns.Add("nombrePlazo", typeof(string));
            Datos.Columns.Add("diasPlazo", typeof(int));
            Datos.Rows.Add(1, "Contado", 0);
            Datos.Rows.Add(2, "30 dias", 30);
            Datos.Rows.Add(3, "60 dias", 60);
            return Datos;
        }

        public DataTable ComprasFuncObtenerTipoCambio(long IdMoneda, DateTime Fecha)
        {
            var Datos = new DataTable();
            Datos.Columns.Add("idTipoCambio", typeof(long));
            Datos.Columns.Add("valorTipoCambio", typeof(decimal));
            Datos.Columns.Add("fechaTipoCambio", typeof(DateTime));
            if (IdMoneda == 20L) Datos.Rows.Add(901L, 7.7M, Fecha.Date);
            if (IdMoneda == 30L) Datos.Rows.Add(902L, 8.4M, Fecha.Date);
            return Datos;
        }

        public DataTable ComprasFuncListarProveedores()
        {
            var Datos = new DataTable();
            Datos.Columns.Add("idProveedor", typeof(int));
            foreach (string Campo in new[] { "nombreProveedor", "nitProveedor", "telefonoProveedor", "correoProveedor", "direccionProveedor" })
                Datos.Columns.Add(Campo, typeof(string));
            Datos.Columns.Add("estadoProveedor", typeof(bool));
            Datos.Columns.Add("saldoActualProveedor", typeof(decimal));
            Datos.Columns.Add("idPlazo", typeof(int));
            Datos.Rows.Add(3, "Proveedor Uno", "1234567890", "5555-1111", "uno@example.com", "Direccion Uno", true, 125.5M, 2);
            Datos.Rows.Add(4, "Proveedor Dos", "0987654321", "5555-2222", "dos@example.com", "Direccion Dos", true, 0M, 1);
            Datos.Rows.Add(5, "Proveedor Sin Plazo", "5555555555", "5555-3333", "tres@example.com", "Direccion Tres", true, 0M, DBNull.Value);
            return Datos;
        }
        public ClsOrdenCompra ComprasFuncObtener(int Id) => new ClsOrdenCompra(_Datos.Rows[Id - 1]);
        public void ComprasProcAnular(int Id) { _Datos.Rows[Id - 1]["estadoOrdenCompra"] = "Anulada"; }
        public void ComprasProcActualizarVencimiento(int Id, DateTime Fecha) { Vencimiento = Fecha; }
        public int ComprasFuncCrear(int IdProveedor, int IdPlazo, int IdMoneda, DateTime Emision,
            DateTime Vencimiento, decimal TipoCambio, decimal Subtotal, decimal Descuento, string Comentario, decimal Impuesto = 0M)
        {
            CodigoCreado = string.Empty;
            this.Comentario = Comentario;
            return 42;
        }
    }
}
