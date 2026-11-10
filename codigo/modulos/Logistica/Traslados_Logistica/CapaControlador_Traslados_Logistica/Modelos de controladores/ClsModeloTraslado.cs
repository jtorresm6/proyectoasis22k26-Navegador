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
    public class ClsModeloTraslado
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
            return _RepositorioTraslado.LogisticaMetObtenerConductoresDisponibles(
                CodigoTrasladoExcluir);
        }

        public DataTable LogisticaMetObtenerTransportesDisponibles(
            int CodigoTipoTransporte, string CodigoTrasladoExcluir)
        {
            if (CodigoTipoTransporte <= 0)
            {
                return LogisticaMetCrearTablaVacia("CodigoTransporte", "PlacaTransporte");
            }

            return _RepositorioTraslado.LogisticaMetObtenerTransportesDisponibles(
                CodigoTipoTransporte, CodigoTrasladoExcluir);
        }

        public DataTable LogisticaMetObtenerProductosDisponibles(
            int CodigoBodega, string CodigoTrasladoExcluir)
        {
            if (CodigoBodega <= 0)
            {
                return LogisticaMetCrearTablaVacia(
                    "CodigoProducto", "SKU", "Nombre", "Descripcion", "PesoUnitario",
                    "ExistenciasDisponibles", "ProductoMostrar");
            }

            return _RepositorioTraslado.LogisticaMetObtenerProductosDisponibles(
                CodigoBodega, CodigoTrasladoExcluir);
        }

        public void LogisticaMetCargarDetalleTraslado(string CodigoTraslado)
        {
            if (string.IsNullOrWhiteSpace(CodigoTraslado))
            {
                throw new ArgumentException("Seleccione un traslado válido.", "CodigoTraslado");
            }

            DataTable TablaDetalle = _RepositorioTraslado.LogisticaMetObtenerDetalleTraslado(
                CodigoTraslado);
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
                    PesoUnitario = Convert.ToDecimal(Fila["PesoUnitario"]),
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

            return _RepositorioTraslado.LogisticaMetObtenerDetalleTraslado(CodigoTraslado);
        }

        public DataTable LogisticaMetObtenerDetalle()
        {
            var Tabla = new DataTable();
            Tabla.Columns.Add("CodigoProducto", typeof(int));
            Tabla.Columns.Add("SKU", typeof(string));
            Tabla.Columns.Add("Nombre", typeof(string));
            Tabla.Columns.Add("Descripcion", typeof(string));
            Tabla.Columns.Add("Cantidad", typeof(int));
            Tabla.Columns.Add("PesoTotal", typeof(decimal));

            foreach (ClsProductoTrasladoDetalle Detalle in _Detalles)
            {
                Tabla.Rows.Add(
                    Detalle.CodigoProducto,
                    Detalle.SKU,
                    Detalle.Nombre,
                    Detalle.Descripcion,
                    Detalle.Cantidad,
                    Detalle.PesoTotal);
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
                    PesoUnitario = Convert.ToDecimal(Producto["PesoUnitario"]),
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

        public void LogisticaMetGuardarTraslado(
            string CodigoTraslado, int BodegaOrigen, int BodegaDestino,
            int CodigoTipoTransporte, int CodigoTransporte, int CodigoConductor,
            string Observaciones)
        {
            ClsTrasladoSolicitud Solicitud = LogisticaMetCrearSolicitud(
                CodigoTraslado, BodegaOrigen, BodegaDestino, CodigoTipoTransporte,
                CodigoTransporte, CodigoConductor, Observaciones,
                ClsReglasEstadoTraslado.Solicitado);
            _RepositorioTraslado.LogisticaMetGuardarTraslado(Solicitud);
        }

        public void LogisticaMetActualizarTraslado(
            string CodigoTraslado, int BodegaOrigen, int BodegaDestino,
            int CodigoTipoTransporte, int CodigoTransporte, int CodigoConductor,
            string Observaciones, string Estado)
        {
            ClsTrasladoSolicitud Solicitud = LogisticaMetCrearSolicitud(
                CodigoTraslado, BodegaOrigen, BodegaDestino, CodigoTipoTransporte,
                CodigoTransporte, CodigoConductor, Observaciones, Estado);
            _RepositorioTraslado.LogisticaMetActualizarTraslado(Solicitud);
        }

        private ClsTrasladoSolicitud LogisticaMetCrearSolicitud(
            string CodigoTraslado, int BodegaOrigen, int BodegaDestino,
            int CodigoTipoTransporte, int CodigoTransporte, int CodigoConductor,
            string Observaciones, string Estado)
        {
            if (string.IsNullOrWhiteSpace(CodigoTraslado) || CodigoTraslado.Trim().Length > 30)
            {
                throw new InvalidOperationException(
                    "Ingrese un código de traslado de hasta 30 caracteres.");
            }

            if (BodegaOrigen <= 0 || BodegaDestino <= 0)
            {
                throw new InvalidOperationException("Seleccione las bodegas de origen y destino.");
            }

            if (BodegaOrigen == BodegaDestino)
            {
                throw new InvalidOperationException(
                    "La bodega de origen debe ser diferente a la bodega de destino.");
            }

            if (ClsReglasEstadoTraslado.LogisticaMetRequiereTransporte(Estado) &&
                (CodigoTipoTransporte <= 0 || CodigoTransporte <= 0))
            {
                throw new InvalidOperationException(
                    "Para despachar o recibir el traslado debe asignar un transporte disponible.");
            }

            if (_Detalles.Count == 0)
            {
                throw new InvalidOperationException(
                    "Agregue al menos un producto al detalle del traslado.");
            }

            if (Observaciones != null && Observaciones.Length > 500)
            {
                throw new InvalidOperationException(
                    "Las observaciones no pueden superar los 500 caracteres.");
            }

            var Solicitud = new ClsTrasladoSolicitud
            {
                CodigoTraslado = CodigoTraslado.Trim(),
                BodegaOrigen = BodegaOrigen,
                BodegaDestino = BodegaDestino,
                CodigoTipoTransporte = CodigoTipoTransporte,
                CodigoTransporte = CodigoTransporte,
                CodigoConductor = CodigoConductor,
                Observaciones = Observaciones ?? string.Empty,
                Estado = Estado
            };
            foreach (ClsProductoTrasladoDetalle Detalle in _Detalles)
            {
                Solicitud.Detalles.Add(new ClsProductoTrasladoDetalle
                {
                    CodigoProducto = Detalle.CodigoProducto,
                    SKU = Detalle.SKU,
                    Nombre = Detalle.Nombre,
                    Descripcion = Detalle.Descripcion,
                    Cantidad = Detalle.Cantidad,
                    ExistenciasDisponibles = Detalle.ExistenciasDisponibles,
                    PesoUnitario = Detalle.PesoUnitario
                });
            }

            return Solicitud;
        }

        public DataTable LogisticaMetCargarTraslados()
        {
            _TablaTrasladosCompleta = _RepositorioTraslado.LogisticaMetObtenerTraslados();
            if (_TablaTrasladosCompleta == null)
            {
                throw new DataException("El repositorio devolvió una lista de traslados nula.");
            }

            _TablaTraslados = _TablaTrasladosCompleta.Copy();
            _IndiceActual = _TablaTraslados.Rows.Count == 0 ? -1 : 0;
            return _TablaTraslados;
        }

        public DataTable LogisticaMetObtenerTrasladosParaReporte()
        {
            DataTable Traslados = _RepositorioTraslado.LogisticaMetObtenerTraslados();
            if (Traslados == null)
            {
                throw new DataException("El repositorio devolvió una lista de traslados nula.");
            }

            return Traslados;
        }

        public DataTable LogisticaMetFiltrarTraslados(
            string Codigo, DateTime? FechaSolicitud, string Estado)
        {
            DataTable Resultados = _TablaTrasladosCompleta.Clone();
            string CodigoBuscado = (Codigo ?? string.Empty).Trim();
            string EstadoBuscado = (Estado ?? string.Empty).Trim();

            foreach (DataRow Traslado in _TablaTrasladosCompleta.Rows)
            {
                if (CodigoBuscado.Length > 0 &&
                    Convert.ToString(Traslado["CodigoTraslado"]).IndexOf(
                        CodigoBuscado, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                if (FechaSolicitud.HasValue &&
                    (Traslado.IsNull("FechaSolicitud") ||
                    Convert.ToDateTime(Traslado["FechaSolicitud"]).Date != FechaSolicitud.Value.Date))
                {
                    continue;
                }

                if (EstadoBuscado.Length > 0 &&
                    !string.Equals(EstadoBuscado, "Todos", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(
                        Convert.ToString(Traslado["Estado"]), EstadoBuscado,
                        StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                Resultados.ImportRow(Traslado);
            }

            _TablaTraslados = Resultados;
            _IndiceActual = _TablaTraslados.Rows.Count == 0 ? -1 : 0;
            return _TablaTraslados;
        }

        public DataRow LogisticaMetIrAInicio()
        {
            return LogisticaMetObtenerFila(0);
        }

        public DataRow LogisticaMetIrAlAnterior()
        {
            return LogisticaMetObtenerFila(Math.Max(0, _IndiceActual - 1));
        }

        public DataRow LogisticaMetIrAlSiguiente()
        {
            return LogisticaMetObtenerFila(Math.Min(_TablaTraslados.Rows.Count - 1, _IndiceActual + 1));
        }

        public DataRow LogisticaMetIrAlFin()
        {
            return LogisticaMetObtenerFila(_TablaTraslados.Rows.Count - 1);
        }

        public DataRow LogisticaMetSeleccionarTraslado(DataRow Traslado)
        {
            if (Traslado == null)
            {
                throw new ArgumentNullException("Traslado");
            }

            int Indice = _TablaTraslados.Rows.IndexOf(Traslado);
            if (Indice < 0)
            {
                throw new ArgumentException("El traslado no pertenece a la lista cargada.", "Traslado");
            }

            _IndiceActual = Indice;
            return Traslado;
        }

        private DataRow LogisticaMetObtenerFila(int Indice)
        {
            if (_TablaTraslados.Rows.Count == 0)
            {
                _IndiceActual = -1;
                return null;
            }

            _IndiceActual = Indice;
            return _TablaTraslados.Rows[_IndiceActual];
        }

        private DataTable LogisticaMetCrearTablaVacia(params string[] Columnas)
        {
            var Tabla = new DataTable();
            foreach (string Columna in Columnas)
            {
                Tabla.Columns.Add(Columna);
            }

            return Tabla;
        }
    }
}
