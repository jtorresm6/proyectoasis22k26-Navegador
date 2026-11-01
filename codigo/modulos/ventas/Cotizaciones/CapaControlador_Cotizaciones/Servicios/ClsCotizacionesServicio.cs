// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using CapaControlador_Cotizaciones.Validaciones;
using CapaModelo_Cotizaciones;
using System.Data;
using System.Linq;
using System;
using VentasValidaciones;

namespace CapaControlador_Cotizaciones.Servicios
{
    internal sealed class ClsCotizacionesServicio
    {
        // RegistroCotizaciones.
        public string VentasFuncGuardar(int IdCotizacion, string Numero, int Cliente, int Vendedor, int Moneda, int Usuario, DateTime Fecha, DateTime Vigencia, decimal TipoCambio, string Estado, DataTable Lineas)
        {
            Numero = ClsValidacionesVentas.VentasFuncTexto(Numero, 30, "numero de cotizacion", false);
            ClsValidacionesVentas.VentasMetExigir(Fecha.Date <= Vigencia.Date && Vigencia.Date >= DateTime.Today && Fecha.Date <= DateTime.Today, "Revise la emision y vigencia; una vencida requiere una nueva cotizacion.");
            ClsValidacionesVentas.VentasMetExigir(Estado == "BORRADOR" || Estado == "EMITIDA", "Solo puede guardar como Borrador o Vigente.");
            ClsValidacionesVentas.VentasMetExigir(TipoCambio > 0 && TipoCambio <= 999999999999.999999m && TipoCambio == Math.Round(TipoCambio, 6), "El tipo de cambio debe ser positivo y tener hasta seis decimales.");
            ClsValidacionesVentas.VentasMetExigir(Lineas != null && Lineas.Rows.Count > 0, "Agregue productos a la cotizacion.");
            using (var Repositorio = new ClsCotizacionesRepositorio())
            {
                Repositorio.VentasMetIniciar();
                Repositorio.VentasMetUsuario(Usuario);
                Repositorio.VentasMetBloquearMoneda(Moneda);
                ClsValidacionesVentas.VentasMetExigir(Convert.ToInt32(Repositorio.VentasFuncGuardarConsultarCliente(Cliente)) == 1, "Seleccione un cliente activo.");
                ClsValidacionesVentas.VentasMetExigir(Convert.ToInt32(Repositorio.VentasFuncGuardarConsultarVendedor(Vendedor)) == 1, "Seleccione un vendedor activo.");
                DataTable ListaPrecios = VentasFuncLista(Repositorio, Cliente, Moneda, Fecha);
                ClsValidacionesVentas.VentasMetExigir(ListaPrecios.Rows.Count == 1, "El cliente no tiene una lista de precios vigente para esta moneda.");
                if (IdCotizacion > 0)
                {
                    DataTable Actual = Repositorio.VentasFuncGuardarConsultarCotizacion(IdCotizacion);
                    ClsValidacionesVentas.VentasMetExigir(Actual.Rows.Count == 1 && (string)Actual.Rows[0]["estadoCotizacion"] == "BORRADOR" && Convert.ToDateTime(Actual.Rows[0]["vigenteHasta"]).Date >= DateTime.Today, "La cotizacion fue emitida, convertida, anulada o vencio. No puede modificarse; consulte nuevamente.");
                    Numero = (string)Actual.Rows[0]["numeroCotizacion"];
                }
                else if (Numero.Length == 0)
                    Numero = Repositorio.VentasFuncNumero("COT");
                decimal Subtotal = 0, Descuento = 0, Impuesto = 0, Total = 0;
                var Detalle = new DataTable();
                foreach (string Columna in new[]
                {
                    "idProducto",
                    "Cantidad",
                    "Precio",
                    "Descuento",
                    "Impuesto",
                    "Total",
                    "Tasa",
                    "Descripcion",
                    "Tipo"
                }

                )
                    Detalle.Columns.Add(Columna, Columna == "Descripcion" || Columna == "Tipo" ? typeof(string) : typeof(decimal));
                foreach (DataRow Fila in Lineas.Rows)
                {
                    int Producto = Convert.ToInt32(Fila["idProducto"]);
                    decimal Cantidad = Convert.ToDecimal(Fila["Cantidad"]), DescuentoLinea = Convert.ToDecimal(Fila["Descuento"]);
                    DataTable ProductosDisponibles = Repositorio.VentasFuncGuardarConsultarProductoventas(Producto, ListaPrecios.Rows[0]["idListaPrecio"]);
                    ClsValidacionesVentas.VentasMetExigir(ProductosDisponibles.Rows.Count == 1, "Un producto ya no esta disponible en la lista.");
                    decimal Precio = (decimal)ProductosDisponibles.Rows[0]["precioUnitario"], Tasa = (decimal)ProductosDisponibles.Rows[0]["porcentajeImpuesto"];
                    ClsValidacionesVentas.VentasMetExigir(Cantidad > 0 && Cantidad == Math.Round(Cantidad, 4) && Precio >= 0 && Tasa >= 0 && Tasa <= 100, "Cantidad, precio o tasa de impuesto invalida.");
                    ClsValidacionesVentas.VentasMetExigir(Precio == Convert.ToDecimal(Fila["Precio"]), "Un precio de la lista cambio. Recargue los productos antes de guardar.");
                    decimal Base = ClsValidacionesVentas.VentasFuncRedondear(Cantidad * Precio);
                    ClsValidacionesVentas.VentasMetExigir(DescuentoLinea >= 0 && DescuentoLinea <= Base && DescuentoLinea == ClsValidacionesVentas.VentasFuncRedondear(DescuentoLinea), "El descuento es un importe y no puede superar el subtotal.");
                    decimal ImpuestoLinea = ClsValidacionesVentas.VentasFuncRedondear((Base - DescuentoLinea) * Tasa / 100m), TotalLinea = Base - DescuentoLinea + ImpuestoLinea;
                    ClsValidacionesVentas.VentasMetExigir(Base <= ClsValidacionesVentas.ImporteMaximo && TotalLinea <= ClsValidacionesVentas.ImporteMaximo, "El importe de una linea supera la capacidad del documento.");
                    Subtotal += Base;
                    Descuento += DescuentoLinea;
                    Impuesto += ImpuestoLinea;
                    Total += TotalLinea;
                    Detalle.Rows.Add(Producto, Cantidad, Precio, DescuentoLinea, ImpuestoLinea, TotalLinea, Tasa, ProductosDisponibles.Rows[0]["nombreProducto"], ProductosDisponibles.Rows[0]["tipoItem"]);
                }

                ClsValidacionesVentas.VentasMetExigir(Subtotal <= ClsValidacionesVentas.ImporteMaximo && Total <= ClsValidacionesVentas.ImporteMaximo, "El total supera la capacidad del documento.");
                if (IdCotizacion == 0)
                {
                    Repositorio.VentasFuncGuardarInsertarCotizacion(Numero, Cliente, Vendedor, Moneda, Fecha.Date, Vigencia.Date, Estado, Usuario, ListaPrecios.Rows[0]["idListaPrecio"]);
                    IdCotizacion = Repositorio.VentasFuncObtenerId();
                }
                else
                {
                    Repositorio.VentasFuncGuardarActualizarCotizacion(Cliente, Vendedor, Moneda, Fecha.Date, Vigencia.Date, Estado, ListaPrecios.Rows[0]["idListaPrecio"], IdCotizacion);
                    Repositorio.VentasFuncGuardarEliminarCotizaciondetalle(IdCotizacion);
                }

                int NumeroLinea = 0;
                foreach (DataRow Fila in Detalle.Rows)
                    Repositorio.VentasFuncGuardarInsertarCotizaciondetalle(IdCotizacion, ++NumeroLinea, Fila["idProducto"], Fila["Descripcion"], Fila["Cantidad"], Fila["Precio"], Fila["Descuento"], Convert.ToDecimal(Fila["Tasa"]) / 100m);
                Repositorio.VentasMetAuditar("COTIZACION", IdCotizacion, "GUARDAR", Numero, Usuario);
                Repositorio.VentasMetConfirmar();
                return Numero;
            }
        }

        public void VentasMetAnular(int IdCotizacion, int Usuario)
        {
            using (var Repositorio = new ClsCotizacionesRepositorio())
            {
                Repositorio.VentasMetIniciar();
                Repositorio.VentasMetUsuario(Usuario);
                DataTable CotizacionConsultada = Repositorio.VentasFuncAnularConsultarCotizacion(IdCotizacion);
                ClsValidacionesVentas.VentasMetExigir(CotizacionConsultada.Rows.Count == 1 && new[] { "BORRADOR", "EMITIDA", "ACEPTADA" }.Contains((string)CotizacionConsultada.Rows[0][0]), "La cotizacion no se puede anular.");
                Repositorio.VentasFuncAnularActualizarCotizacion(IdCotizacion);
                Repositorio.VentasMetAuditar("COTIZACION", IdCotizacion, "ANULAR", "Anulacion de cotizacion", Usuario);
                Repositorio.VentasMetConfirmar();
            }
        }

        // OperacionCotizaciones.
        private static DataTable VentasFuncLista(ClsCotizacionesRepositorio Repositorio, int Cliente, int Moneda, DateTime Fecha)
        {
            return Repositorio.Consultas.VentasFuncListaConsultarClientelistaprecios(Cliente, Moneda, Fecha.Date, Fecha.Date);
        }

        // ConversionCotizaciones.
        public string VentasFuncConvertir(int IdCotizacion, bool AFactura, int Establecimiento, int Usuario)
        {
            using (var Repositorio = new ClsCotizacionesRepositorio())
            {
                Repositorio.VentasMetIniciar();
                Repositorio.VentasMetUsuario(Usuario);
                object Moneda = Repositorio.Conversion.VentasFuncConvertirConsultarCotizacion(IdCotizacion);
                ClsValidacionesVentas.VentasMetExigir(Moneda != null, "Primero guarde y emita la cotizacion.");
                Repositorio.VentasMetBloquearMoneda(Convert.ToInt32(Moneda));
                DataTable CotizacionConsultada = Repositorio.Conversion.VentasFuncConvertirConsultarCotizacion2(IdCotizacion);
                ClsValidacionesVentas.VentasMetExigir(CotizacionConsultada.Rows.Count == 1, "La cotizacion no tiene una lista de precios registrada.");
                DataRow CabeceraCotizacion = CotizacionConsultada.Rows[0];
                string Columna = AFactura ? "idFactura" : "idPedido";
                if (CabeceraCotizacion[Columna] != DBNull.Value)
                {
                    string Existente = Convert.ToString((AFactura ? Repositorio.Conversion.VentasFuncConvertirConsultarFactura(CabeceraCotizacion[Columna]) : Repositorio.Conversion.VentasFuncConvertirConsultarPedido(CabeceraCotizacion[Columna])));
                    Repositorio.VentasMetConfirmar();
                    return Existente;
                }

                ClsValidacionesVentas.VentasMetExigir(CabeceraCotizacion[AFactura ? "idPedido" : "idFactura"] == DBNull.Value, "La cotizacion ya fue convertida a otro documento.");
                ClsValidacionesVentas.VentasMetExigir(new[] { "EMITIDA", "ACEPTADA" }.Contains((string)CabeceraCotizacion["estadoCotizacion"]) && Convert.ToDateTime(CabeceraCotizacion["fechaEmision"]).Date <= DateTime.Today && Convert.ToDateTime(CabeceraCotizacion["vigenteHasta"]).Date >= DateTime.Today, "Solo se convierte una cotizacion emitida y vigente. Una vencida requiere una nueva con precios actuales.");
                string Numero = Repositorio.VentasFuncNumero(AFactura ? "FCOT" : "PCOT");
                int Id;
                if (!AFactura)
                {
                    Repositorio.Conversion.VentasFuncConvertirInsertarPedido(Numero, IdCotizacion, CabeceraCotizacion["idCliente"], CabeceraCotizacion["idVendedor"], CabeceraCotizacion["idMoneda"], DateTime.Today, Usuario);
                    Id = Repositorio.VentasFuncObtenerId();
                    Repositorio.Conversion.VentasFuncConvertirInsertarPedidodetalle(Id, IdCotizacion);
                }
                else
                {
                    ClsValidacionesVentas.VentasMetExigir(Convert.ToInt32(Repositorio.Conversion.VentasFuncConvertirConsultarEstablecimientoventas(Establecimiento)) == 1, "Seleccione un establecimiento emisor activo.");
                    DataTable Clientes = Repositorio.Conversion.VentasFuncConvertirConsultarCliente(CabeceraCotizacion["idCliente"]);
                    DataRow Cliente = Clientes.Rows[0];
                    int Dias = Convert.ToInt32(Cliente["diasCredito"]);
                    int VersionCliente = Repositorio.Conversion.VentasFuncConvertirCrearVersionCliente(Convert.ToInt32(CabeceraCotizacion["idCliente"]));
                    // Borrador con precios congelados: emitir, certificar y afectar inventarios corresponde a Facturas.
                    Repositorio.Conversion.VentasFuncConvertirInsertarFactura(Numero, Establecimiento, CabeceraCotizacion["idVendedor"], CabeceraCotizacion["idMoneda"], DateTime.Today, DateTime.Today.AddDays(Dias), Dias == 0 ? "CONTADO" : "CREDITO", "Origen cotizacion " + CabeceraCotizacion["numeroCotizacion"], Usuario, VersionCliente, IdCotizacion);
                    Id = Repositorio.VentasFuncObtenerId();
                    Repositorio.Conversion.VentasFuncConvertirInsertarFacturadetalle(Id, IdCotizacion);
                }

                Repositorio.Conversion.VentasFuncConvertirActualizarCotizacion(IdCotizacion);
                Repositorio.VentasMetConfirmar();
                return AFactura ? "COT-" + Numero : Numero;
            }
        }

        // ConsultaCotizaciones.
        public DataTable VentasFuncCatalogo(string Nombre)
        {
            using (var Repositorio = new ClsCotizacionesRepositorio())
                return Repositorio.VentasFuncCatalogo(Nombre);
        }

        public DataTable VentasFuncListaCliente(int Cliente, int Moneda, DateTime Fecha)
        {
            using (var Repositorio = new ClsCotizacionesRepositorio())
                return VentasFuncLista(Repositorio, Cliente, Moneda, Fecha);
        }

        public DataTable VentasFuncProductos(int Cliente, int Moneda, DateTime Fecha)
        {
            using (var Repositorio = new ClsCotizacionesRepositorio())
            {
                DataTable ListaPrecios = VentasFuncLista(Repositorio, Cliente, Moneda, Fecha);
                if (ListaPrecios.Rows.Count != 1)
                    return new DataTable();
                return Repositorio.Consultas.VentasFuncProductosConsultarProductoventas(ListaPrecios.Rows[0]["idListaPrecio"]);
            }
        }

        public DataSet VentasFuncConsultar(string Numero)
        {
            using (var Repositorio = new ClsCotizacionesRepositorio())
            {
                var Datos = new DataSet();
                DataTable Encabezado = Repositorio.Consultas.VentasFuncGeneralesConsultarConsultarCotizacion(Numero.Trim());
                ClsValidacionesVentas.VentasMetExigir(Encabezado.Rows.Count == 1, "No se encontro la cotizacion.");
                Datos.Tables.Add(Encabezado);
                Datos.Tables.Add(Repositorio.Consultas.VentasFuncGeneralesConsultarConsultarCotizaciondetalle(Encabezado.Rows[0]["idCotizacion"]));
                return Datos;
            }
        }

        public DataTable VentasFuncListar(string Buscar)
        {
            Buscar = (Buscar ?? "").Trim();
            using (var Repositorio = new ClsCotizacionesRepositorio())
                return Repositorio.Consultas.VentasFuncListarConsultarCotizacion(Buscar, Buscar, Buscar);
        }
    }
}
