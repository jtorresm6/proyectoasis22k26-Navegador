using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CapaControlador_PagosProveedores;
using CapaModelo_PagosProveedores.Contratos;
using CapaModelo_PagosProveedores.Entidades;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diana Mishel Loeiza Ramírez
 * Carné : 9959-23-3457
 * Fecha : 04/11/2026
 * ==================================================================
 * Propósito :
 *  Repositorio en memoria para probar el controlador de pagos a
 *  proveedores sin conexión a la base de datos.
 * ===================================================================
*/

namespace PruebaUnitaria_PagosProveedores.Apoyo
{
    internal sealed class ClsRepositorioPagosProveedorPrueba : IRepositorioPagoProveedor
    {
        internal List<ClsEncabezadoPagoProveedor> Datos { get; } = new List<ClsEncabezadoPagoProveedor>();
        internal int ProximoId { get; set; } = 1;

        internal long? TipoCambioIdADevolver { get; set; }
        internal decimal TipoCambioValorADevolver { get; set; } = 1M;
        internal Exception ExcepcionAlGrabar { get; set; }

        internal int LlamadasAgregar { get; private set; }
        internal int LlamadasEditar { get; private set; }
        internal int LlamadasRemover { get; private set; }
        internal ClsEncabezadoPagoProveedor UltimoGuardado { get; private set; }
        internal int? UltimoFiltroProveedor { get; private set; }
        internal string UltimoFiltroEstado { get; private set; }
        internal DateTime? UltimoFiltroDesde { get; private set; }
        internal DateTime? UltimoFiltroHasta { get; private set; }

        internal DataTable Proveedores { get; } = new DataTable();
        internal DataTable MetodosPago { get; } = new DataTable();
        internal DataTable Monedas { get; } = new DataTable();
        internal DataTable CuentasPendientes { get; } = new DataTable();

        public void ComprasMetResolverTipoCambio(long IdMoneda, DateTime Fecha, out long? IdTipoCambio, out decimal Valor)
        {
            IdTipoCambio = TipoCambioIdADevolver;
            Valor = TipoCambioValorADevolver;
        }

        public int ComprasMetAgregar(ClsEncabezadoPagoProveedor Entidad)
        {
            if (ExcepcionAlGrabar != null) throw ExcepcionAlGrabar;
            LlamadasAgregar++;
            Entidad.IdPagoProveedor = ProximoId++;
            UltimoGuardado = Entidad;
            Datos.Add(Entidad);
            return Entidad.IdPagoProveedor;
        }

        public void ComprasMetEditar(ClsEncabezadoPagoProveedor Entidad)
        {
            if (ExcepcionAlGrabar != null) throw ExcepcionAlGrabar;
            LlamadasEditar++;
            UltimoGuardado = Entidad;
        }

        public void ComprasMetRemover(ClsEncabezadoPagoProveedor Entidad)
        {
            if (ExcepcionAlGrabar != null) throw ExcepcionAlGrabar;
            LlamadasRemover++;
            UltimoGuardado = Entidad;
        }

        public IEnumerable<ClsEncabezadoPagoProveedor> ComprasMetObtenerTodos() => Datos.ToList();

        public List<ClsEncabezadoPagoProveedor> ComprasMetConsultar(int? IdProveedor, string Estado, DateTime? Desde, DateTime? Hasta)
        {
            UltimoFiltroProveedor = IdProveedor;
            UltimoFiltroEstado = Estado;
            UltimoFiltroDesde = Desde;
            UltimoFiltroHasta = Hasta;

            return Datos.Where(Pago =>
                (!IdProveedor.HasValue || Pago.IdProveedor == IdProveedor.Value) &&
                (Estado == null || Pago.EstadoPagoProveedor == Estado) &&
                (!Desde.HasValue || Pago.FechaPagoProveedor >= Desde.Value) &&
                (!Hasta.HasValue || Pago.FechaPagoProveedor <= Hasta.Value)).ToList();
        }

        public ClsEncabezadoPagoProveedor ComprasMetObtenerPorId(int IdPago)
            => Datos.FirstOrDefault(Pago => Pago.IdPagoProveedor == IdPago);

        public DataTable ComprasMetObtenerProveedores() => Proveedores;
        public DataTable ComprasMetObtenerMetodosPago() => MetodosPago;
        public DataTable ComprasMetObtenerMonedas() => Monedas;
        public DataTable ComprasMetObtenerCuentasPendientes(int IdProveedor) => CuentasPendientes;

        // ---------- Datos de apoyo para las pruebas ----------
        internal static ClsEncabezadoPagoProveedor CrearEncabezado(int Id, int IdProveedor, string Estado, DateTime Fecha)
        {
            return new ClsEncabezadoPagoProveedor
            {
                IdPagoProveedor = Id,
                IdProveedor = IdProveedor,
                NombreProveedor = "Proveedor " + IdProveedor,
                EstadoPagoProveedor = Estado,
                FechaPagoProveedor = Fecha,
                MontoPagoProveedor = 100M
            };
        }

        internal static ClsModeloDetallePagoProveedor CrearDetalle(string Serie, string Numero, decimal Monto)
        {
            return new ClsModeloDetallePagoProveedor
            {
                SerieDocumento = Serie,
                NumeroDocumento = Numero,
                IdConcepto = 1,
                MontoAplicado = Monto
            };
        }

        // Pago nuevo y válido de Q100 aplicado a un solo documento
        internal static ClsModeloPagoProveedor CrearPagoValido(IRepositorioPagoProveedor Repositorio)
        {
            var Pago = new ClsModeloPagoProveedor(Repositorio)
            {
                Estado = EnmEstadoEntidadCompras.Added,
                IdProveedor = 1,
                FechaPagoProveedor = new DateTime(2026, 10, 1),
                MontoPagoProveedor = 100M,
                IdMetodoPago = 1,
                IdMoneda = 1,
                ReferenciaPagoProveedor = "REF-001",
                EstadoPagoProveedor = "Pagado"
            };
            Pago.Detalles.Add(CrearDetalle("A", "1", 100M));
            return Pago;
        }
    }
}