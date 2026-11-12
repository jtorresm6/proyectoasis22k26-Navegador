using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_Traslados_Logistica.Contratos;
using CapaModelo_Traslados_Logistica.Entidades;
using CapaModelo_Traslados_Logistica.Repositorios;

/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 05/11/2026
 * ==================================================================
 * Propósito :
 * Modelo controlador de Traslados. Contiene las validaciones
 * necesarias para registrar, consultar y actualizar traslados
 * entre bodegas, asignando conductor segun el tipo de servicio.
 * ===================================================================
 */

namespace CapaControlador_Traslados_Logistica
{
    public partial class ClsModeloTraslado
    {
        private readonly IRepositorioTraslado _RepositorioTraslado;
        private readonly List<ClsProductoTrasladoDetalle> _Detalles;
        private DataTable _TablaTrasladosCompleta;
        private DataTable _TablaTraslados;
        private int _IndiceActual;

        public ClsModeloTraslado()
            : this(new ClsRepositorioTraslado())
        {
        }

        public ClsModeloTraslado(IRepositorioTraslado RepositorioTraslado)
        {
            if (RepositorioTraslado == null)
            {
                throw new ArgumentNullException("RepositorioTraslado");
            }

            _RepositorioTraslado = RepositorioTraslado;
            _Detalles = new List<ClsProductoTrasladoDetalle>();
            _TablaTrasladosCompleta = new DataTable();
            _TablaTraslados = new DataTable();
            _IndiceActual = -1;
        }

        public DataTable LogisticaMetObtenerBodegas()
        {
            return _RepositorioTraslado.LogisticaMetObtenerBodegas();
        }

        public DataTable LogisticaMetObtenerTiposTransporte()
        {
            return _RepositorioTraslado.LogisticaMetObtenerTiposTransporte();
        }

        public DataTable LogisticaMetObtenerConductoresDisponibles(string CodigoTrasladoExcluir)
        {
            var Disponibles = new DataTable();
            Disponibles.Columns.Add("CodigoConductor", typeof(int));
            Disponibles.Columns.Add("NombreConductor", typeof(string));
            DataTable Conductores = _RepositorioTraslado.LogisticaMetObtenerConductores();
            var Agregados = new HashSet<int>();

            foreach (DataRow Conductor in Conductores.Rows)
            {
                if (!string.Equals(Convert.ToString(Conductor["EstadoConductor"]),
                    "DISPONIBLE", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(Convert.ToString(Conductor["EstadoViaje"]),
                        "EN_CURSO", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                int CodigoConductor = Convert.ToInt32(Conductor["CodigoConductor"]);
                if (Agregados.Add(CodigoConductor))
                {
                    Disponibles.Rows.Add(CodigoConductor, Conductor["NombreConductor"]);
                }
            }

            return Disponibles;
        }

        public DataTable LogisticaMetObtenerTransportesDisponibles(
            int CodigoTipoTransporte, string CodigoTrasladoExcluir)
        {
            var Disponibles = new DataTable();
            Disponibles.Columns.Add("CodigoTransporte", typeof(int));
            Disponibles.Columns.Add("PlacaTransporte", typeof(string));
            if (CodigoTipoTransporte <= 0)
            {
                return Disponibles;
            }

            DataTable Transportes = _RepositorioTraslado.LogisticaMetObtenerTransportes();
            var Agregados = new HashSet<int>();
            foreach (DataRow Transporte in Transportes.Rows)
            {
                int Codigo = Convert.ToInt32(Transporte["CodigoTransporte"]);
                if (!Agregados.Add(Codigo) ||
                    Convert.ToInt32(Transporte["CodigoTipoTransporte"]) != CodigoTipoTransporte)
                {
                    continue;
                }

                bool Disponible = string.Equals(
                    Convert.ToString(Transporte["EstadoTransporte"]),
                    "DISPONIBLE", StringComparison.OrdinalIgnoreCase);
                bool AsignadoAlTraslado = false;
                bool TieneOtroTrasladoActivo = false;
                foreach (DataRow Asignacion in Transportes.Rows)
                {
                    if (Convert.ToInt32(Asignacion["CodigoTransporte"]) != Codigo)
                    {
                        continue;
                    }

                    string CodigoTrasladoAsignado =
                        Convert.ToString(Asignacion["CodigoTrasladoAsignado"]);
                    if (!string.IsNullOrWhiteSpace(CodigoTrasladoExcluir) &&
                        string.Equals(CodigoTrasladoAsignado, CodigoTrasladoExcluir,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        AsignadoAlTraslado = true;
                    }

                    if (LogisticaMetEsEstadoActivo(
                        Convert.ToString(Asignacion["EstadoTrasladoAsignado"])) &&
                        !string.Equals(CodigoTrasladoAsignado, CodigoTrasladoExcluir,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        TieneOtroTrasladoActivo = true;
                        break;
                    }
                }

                if ((Disponible || AsignadoAlTraslado) && !TieneOtroTrasladoActivo)
                {
                    Disponibles.Rows.Add(Codigo, Transporte["PlacaTransporte"]);
                }
            }

            return Disponibles;
        }

        public DataTable LogisticaMetObtenerProductosDisponibles(
            int CodigoBodega, string CodigoTrasladoExcluir)
        {
            var Disponibles = new DataTable();
            Disponibles.Columns.Add("CodigoProducto", typeof(int));
            Disponibles.Columns.Add("SKU", typeof(string));
            Disponibles.Columns.Add("Nombre", typeof(string));
            Disponibles.Columns.Add("Descripcion", typeof(string));
            Disponibles.Columns.Add("PesoUnitario", typeof(double));
            Disponibles.Columns.Add("ExistenciasDisponibles", typeof(int));
            Disponibles.Columns.Add("ProductoMostrar", typeof(string));
            if (CodigoBodega <= 0)
            {
                return Disponibles;
            }

            DataTable Productos = _RepositorioTraslado.LogisticaMetObtenerStockProductos(
                CodigoBodega, CodigoTrasladoExcluir);
            foreach (DataRow Producto in Productos.Rows)
            {
                int Existencias = Convert.ToInt32(Producto["StockActual"]) -
                    Convert.ToInt32(Producto["CantidadApartada"]) +
                    Convert.ToInt32(Producto["CantidadReservada"]);
                if (Existencias <= 0)
                {
                    continue;
                }

                string SKU = Convert.ToString(Producto["SKU"]);
                string Nombre = Convert.ToString(Producto["Nombre"]);
                Disponibles.Rows.Add(
                    Convert.ToInt32(Producto["CodigoProducto"]), SKU, Nombre,
                    Convert.ToString(Producto["Descripcion"]),
                    Convert.ToDouble(Producto["PesoUnitario"]), Existencias,
                    SKU + " - " + Nombre);
            }

            return Disponibles;
        }

        public void LogisticaMetCargarDetalleTraslado(string CodigoTraslado)
        {
            if (string.IsNullOrWhiteSpace(CodigoTraslado))
            {
                throw new ArgumentException("Seleccione un traslado válido.", "CodigoTraslado");
            }

            DataTable TablaDetalle = LogisticaMetObtenerDetalleTraslado(CodigoTraslado);
            _Detalles.Clear();
            foreach (DataRow Fila in TablaDetalle.Rows)
            {
                _Detalles.Add(new ClsProductoTrasladoDetalle
                {
                    CodigoProducto = Convert.ToInt32(Fila["CodigoProducto"]),
                    SKU = Convert.ToString(Fila["SKU"]),
                    Nombre = Convert.ToString(Fila["Nombre"]),
                    Descripcion = Convert.ToString(Fila["Descripcion"]),
                    Cantidad = Convert.ToInt32(Fila["Cantidad"]),
                    PesoUnitario = Convert.ToDouble(Fila["PesoUnitario"]),
                    ExistenciasDisponibles = Convert.ToInt32(Fila["ExistenciasDisponibles"])
                });
            }
        }

        public DataTable LogisticaMetObtenerDetalleTraslado(string CodigoTraslado)
        {
            if (string.IsNullOrWhiteSpace(CodigoTraslado))
            {
                throw new ArgumentException("Seleccione un traslado válido.", "CodigoTraslado");
            }

            DataTable DatosDetalle = _RepositorioTraslado.LogisticaMetObtenerDetalleTraslado(
                CodigoTraslado);
            var Detalle = new DataTable();
            Detalle.Columns.Add("CodigoProducto", typeof(int));
            Detalle.Columns.Add("SKU", typeof(string));
            Detalle.Columns.Add("Nombre", typeof(string));
            Detalle.Columns.Add("Descripcion", typeof(string));
            Detalle.Columns.Add("Cantidad", typeof(int));
            Detalle.Columns.Add("PesoUnitario", typeof(double));
            Detalle.Columns.Add("PesoTotal", typeof(double));
            Detalle.Columns.Add("PrecioUnitario", typeof(decimal));
            Detalle.Columns.Add("PrecioTotal", typeof(decimal));
            Detalle.Columns.Add("ExistenciasDisponibles", typeof(int));

            foreach (DataRow Fila in DatosDetalle.Rows)
            {
                int Cantidad = Convert.ToInt32(Fila["Cantidad"]);
                double PesoUnitario = Convert.ToDouble(Fila["PesoUnitario"]);
                decimal PrecioUnitario = Convert.ToDecimal(Fila["PrecioUnitario"]);
                int ExistenciasDisponibles = Convert.ToInt32(Fila["StockActual"]) -
                    Convert.ToInt32(Fila["CantidadApartada"]) + Cantidad;
                Detalle.Rows.Add(
                    Convert.ToInt32(Fila["CodigoProducto"]),
                    Convert.ToString(Fila["SKU"]),
                    Convert.ToString(Fila["Nombre"]),
                    Convert.ToString(Fila["Descripcion"]),
                    Cantidad,
                    PesoUnitario,
                    PesoUnitario * Cantidad,
                    PrecioUnitario,
                    PrecioUnitario * Cantidad,
                    ExistenciasDisponibles);
            }

            return Detalle;
        }

        public DataTable LogisticaMetObtenerDetalle()
        {
            var Tabla = new DataTable();
            Tabla.Columns.Add("CodigoProducto", typeof(int));
            Tabla.Columns.Add("SKU", typeof(string));
            Tabla.Columns.Add("Nombre", typeof(string));
            Tabla.Columns.Add("Descripcion", typeof(string));
            Tabla.Columns.Add("Cantidad", typeof(int));
            Tabla.Columns.Add("PesoTotal", typeof(double));

            foreach (ClsProductoTrasladoDetalle Detalle in _Detalles)
            {
                Tabla.Rows.Add(
                    Detalle.CodigoProducto,
                    Detalle.SKU,
                    Detalle.Nombre,
                    Detalle.Descripcion,
                    Detalle.Cantidad,
                    LogisticaMetCalcularPesoTotal(Detalle));
            }

            return Tabla;
        }

        public void LogisticaMetAgregarProducto(DataRow Producto, int Cantidad)
        {
            if (Producto == null)
            {
                throw new ArgumentNullException("Producto");
            }

            if (Cantidad <= 0)
            {
                throw new ArgumentOutOfRangeException("Cantidad", "La cantidad debe ser mayor que cero.");
            }

            int CodigoProducto = Convert.ToInt32(Producto["CodigoProducto"]);
            int ExistenciasDisponibles = Convert.ToInt32(Producto["ExistenciasDisponibles"]);
            ClsProductoTrasladoDetalle Detalle =
                _Detalles.Find(ProductoActual => ProductoActual.CodigoProducto == CodigoProducto);
            int CantidadTotal = Cantidad + (Detalle == null ? 0 : Detalle.Cantidad);

            if (CantidadTotal > ExistenciasDisponibles)
            {
                throw new InvalidOperationException(
                    "La cantidad supera las existencias disponibles (" +
                    ExistenciasDisponibles + ").");
            }

            if (Detalle == null)
            {
                _Detalles.Add(new ClsProductoTrasladoDetalle
                {
                    CodigoProducto = CodigoProducto,
                    SKU = Convert.ToString(Producto["SKU"]),
                    Nombre = Convert.ToString(Producto["Nombre"]),
                    Descripcion = Convert.ToString(Producto["Descripcion"]),
                    PesoUnitario = Convert.ToDouble(Producto["PesoUnitario"]),
                    ExistenciasDisponibles = ExistenciasDisponibles,
                    Cantidad = Cantidad
                });
            }
            else
            {
                Detalle.Cantidad = CantidadTotal;
            }
        }

        public void LogisticaMetEliminarProducto(int CodigoProducto)
        {
            ClsProductoTrasladoDetalle Detalle =
                _Detalles.Find(Producto => Producto.CodigoProducto == CodigoProducto);
            if (Detalle == null)
            {
                throw new ArgumentException("Seleccione un producto del detalle.", "CodigoProducto");
            }

            _Detalles.Remove(Detalle);
        }

        public void LogisticaMetLimpiarDetalle()
        {
            _Detalles.Clear();
        }

    }
}
