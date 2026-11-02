using System;
using System.Data;
using CapaModelo_OrdenesCompra.Contratos;
using CapaModelo_OrdenesCompra.Entidades;

namespace PruebaUnitaria_OrdenesCompra.Apoyo
{
    internal sealed class ClsRepositorioOrdenesCompraPrueba : IRepositorioOrdenesCompra, IDisposable
    {
        internal DataTable Datos { get; }
        internal string EstadoSolicitado { get; private set; }
        internal decimal ImpuestoCreado { get; private set; }
        internal string CodigoCreado { get; private set; }
        internal string ComentarioCreado { get; private set; }
        internal DateTime EmisionCreada { get; private set; }
        internal DateTime VencimientoCreado { get; private set; }
        internal DateTime VencimientoActualizado { get; private set; }
        internal bool OmitirAnulacion { get; set; }

        internal ClsRepositorioOrdenesCompraPrueba()
        {
            Datos = ComprasFuncCrearDatos();
        }

        public DataTable ComprasFuncListar() => Datos.Copy();

        public DataTable ComprasFuncListar(string Estado)
        {
            EstadoSolicitado = Estado;
            var Resultado = Datos.Clone();
            foreach (DataRow Fila in Datos.Rows)
                if (Convert.ToString(Fila["estadoOrdenCompra"]) == Estado)
                    Resultado.ImportRow(Fila);
            return Resultado;
        }

        public ClsOrdenCompra ComprasFuncObtener(int Id)
        {
            DataRow Fila = Datos.Rows.Find(Id);
            return Fila == null ? null : new ClsOrdenCompra(Fila);
        }

        public DataTable ComprasFuncListarDetalles(int Id)
        {
            var Resultado = new DataTable();
            Resultado.Columns.Add("idProducto", typeof(int));
            Resultado.Columns.Add("cantidadDetalleOrdenCompra", typeof(decimal));
            if (Datos.Rows.Find(Id) != null)
                Resultado.Rows.Add(11, 2M);
            return Resultado;
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
            using (var Vista = new DataView(Datos, "estadoProveedor = true", "nombreProveedor", DataViewRowState.CurrentRows))
                return Vista.ToTable(true, "idProveedor", "nombreProveedor", "nitProveedor", "telefonoProveedor", "correoProveedor", "direccionProveedor", "estadoProveedor", "saldoActualProveedor", "idPlazo");
        }

        public void ComprasProcCambiarEstado(int Id, string Estado)
        {
            DataRow Fila = Datos.Rows.Find(Id);
            if (Fila == null)
                throw new InvalidOperationException("La orden de compra no existe.");
            Fila["estadoOrdenCompra"] = Estado;
        }

        public void ComprasProcAnular(int Id)
        {
            if (!OmitirAnulacion)
                ComprasProcCambiarEstado(Id, "Anulada");
        }

        public void ComprasProcActualizarVencimiento(int Id, DateTime Fecha)
        {
            VencimientoActualizado = Fecha;
        }

        public int ComprasFuncCrear(int IdProveedor, int IdPlazo, int IdMoneda,
            DateTime Emision, DateTime Vencimiento, decimal TipoCambio, decimal Subtotal, decimal Descuento, string Comentario, decimal Impuesto = 0M)
        {
            ImpuestoCreado = Impuesto;
            CodigoCreado = string.Empty;
            ComentarioCreado = Comentario;
            EmisionCreada = Emision;
            VencimientoCreado = Vencimiento;
            return 900;
        }

        public void Dispose()
        {
            Datos.Dispose();
        }

        private static DataTable ComprasFuncCrearDatos()
        {
            var Tabla = new DataTable();
            foreach (string Campo in new[] { "idOrdenCompra", "idProveedor", "idPlazo", "idMoneda", "diasPlazo" })
                Tabla.Columns.Add(Campo, typeof(int));
            foreach (string Campo in new[] { "codigoOrdenCompra", "estadoOrdenCompra", "comentarioOrdenCompra",
                "nombreProveedor", "nitProveedor", "telefonoProveedor", "correoProveedor", "direccionProveedor", "nombrePlazo" })
                Tabla.Columns.Add(Campo, typeof(string));
            Tabla.Columns.Add("fechaEmisionOrdenCompra", typeof(DateTime));
            Tabla.Columns.Add("fechaVencimientoOrdenCompra", typeof(DateTime));
            foreach (string Campo in new[] { "tipoCambioOrdenCompra", "subtotalOrdenCompra", "descuentoOrdenCompra", "totalOrdenCompra" })
                Tabla.Columns.Add(Campo, typeof(decimal));
            Tabla.Columns.Add("estadoProveedor", typeof(bool));
            Tabla.Columns.Add("saldoActualProveedor", typeof(decimal)).DefaultValue = 0M;
            Tabla.Columns.Add("impuestoOrdenCompra", typeof(decimal)).DefaultValue = 0M;
            Tabla.Columns.Add("bloqueadoOrdenCompra", typeof(bool)).DefaultValue = false;
            Tabla.PrimaryKey = new[] { Tabla.Columns["idOrdenCompra"] };
            string[] Estados = { "Pendiente", "Autorizada", "Rechazada", "Anulada", "Recibida" };
            string[] Proveedores = { "José Cárnicos", "Empaques", "José Cárnicos", "Insumos", "Empaques" };
            int[] Dias = { 1, 5, 10, 15, 1 };
            for (int Indice = 0; Indice < Estados.Length; Indice++)
            {
                DataRow Fila = Tabla.NewRow();
                Fila["idOrdenCompra"] = 101 + Indice;
                Fila["codigoOrdenCompra"] = "OC-" + (Indice + 1).ToString("D3");
                Fila["idProveedor"] = Proveedores[Indice] == "José Cárnicos" ? 3 : Proveedores[Indice] == "Empaques" ? 4 : 5;
                Fila["idPlazo"] = 2;
                Fila["idMoneda"] = 1;
                Fila["diasPlazo"] = 30;
                Fila["estadoOrdenCompra"] = Estados[Indice];
                Fila["comentarioOrdenCompra"] = "Insumos de prueba";
                Fila["nombreProveedor"] = Proveedores[Indice];
                Fila["nitProveedor"] = "NIT-" + Convert.ToString(Fila["idProveedor"]);
                Fila["telefonoProveedor"] = "5555-0101";
                Fila["correoProveedor"] = "prueba@example.com";
                Fila["direccionProveedor"] = "Dirección ficticia";
                Fila["nombrePlazo"] = "30 dias";
                Fila["fechaEmisionOrdenCompra"] = new DateTime(2026, Indice == 4 ? 11 : 10, Dias[Indice]);
                Fila["fechaVencimientoOrdenCompra"] = ((DateTime)Fila["fechaEmisionOrdenCompra"]).AddDays(30);
                Fila["tipoCambioOrdenCompra"] = 1.234567M;
                Fila["subtotalOrdenCompra"] = 100.1234M;
                Fila["descuentoOrdenCompra"] = 10M;
                Fila["totalOrdenCompra"] = 90.1234M;
                Fila["estadoProveedor"] = Indice != 4;
                Tabla.Rows.Add(Fila);
            }
            Tabla.AcceptChanges();
            return Tabla;
        }
    }
}
