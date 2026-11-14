// Autor: Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
// Prueba las validaciones, los cálculos y la comunicación con el repositorio mediante datos ficticios.

using System;
using System.Collections.Generic;
using System.Linq;
using CapaControlador_ComisionVendedores.Controladores;
using CapaControlador_ComisionVendedores.DTO;
using CapaControlador_ComisionVendedores.Enumeraciones;
using CapaModelo_ComisionVendedores.Contratos;
using CapaModelo_ComisionVendedores.Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestComisionVendedores
{
    [TestClass]
    public class ClsPruebasComisionVendedores
    {
        private static readonly DateTime Inicio = new DateTime(2026, 1, 1);
        private static readonly DateTime Fin = new DateTime(2026, 1, 31);

        [TestMethod]
        public void VentasMetCalcularAbonosDeMismaVentaAgrupaYAcumula()
        {
            // Arrange
            var Repositorio = new ClsRepositorioFalso();
            Repositorio.Ventas.Add(VentasMetCrearVenta(10m, 1m, 3));
            Repositorio.Ventas.Add(VentasMetCrearVenta(20m, 2m, 3));
            var Controlador = new ClsControladorComision(Repositorio);

            // Act
            var Resultado = Controlador.VentasMetCalcular(
                EnumMetodoComision.Vendedor, EnumBaseComision.Cobrado, Inicio, Fin);

            // Assert
            Assert.AreEqual(1, Resultado.Count);
            Assert.AreEqual(30m, Resultado[0].BaseVentas);
            Assert.AreEqual(3m, Resultado[0].MontoComision);
            Assert.AreEqual(2, Resultado[0].Movimientos.Count);
            Assert.AreEqual(Inicio, Repositorio.FechaInicio);
            Assert.AreEqual(Fin.AddDays(1), Repositorio.FechaFinExclusiva);
        }

        [TestMethod]
        public void VentasMetCalcularFechaInicialPosteriorAFechaFinalRechaza()
        {
            // Arrange
            var Controlador = new ClsControladorComision(new ClsRepositorioFalso());
            // Act
            Action Operacion = () => Controlador.VentasMetCalcular(
                EnumMetodoComision.Vendedor, EnumBaseComision.Cobrado, Fin, Inicio);
            // Assert
            Assert.ThrowsException<ArgumentException>(Operacion);
        }

        [TestMethod]
        public void VentasMetCalcularBaseVentasNoSoportadaRechaza()
        {
            // Arrange
            var Controlador = new ClsControladorComision(new ClsRepositorioFalso());
            // Act
            Action Operacion = () => Controlador.VentasMetCalcular(
                EnumMetodoComision.Vendedor, EnumBaseComision.Ventas, Inicio, Fin);
            // Assert
            Assert.ThrowsException<NotSupportedException>(Operacion);
        }

        [TestMethod]
        public void VentasMetGuardarSinResultadosNoLlamaRepositorio()
        {
            // Arrange
            var Repositorio = new ClsRepositorioFalso();
            var Controlador = new ClsControladorComision(Repositorio);
            // Act
            int Guardadas = Controlador.VentasMetGuardar(new List<ClsComisionVendedorResultado>(), Inicio, Fin, EnumMetodoComision.Vendedor, EnumBaseComision.Cobrado);
            // Assert
            Assert.AreEqual(0, Guardadas);
            Assert.AreEqual(0, Repositorio.LlamadasGuardar);
        }

        [TestMethod]
        public void VentasMetGuardarResultadoSinMovimientosRechaza()
        {
            // Arrange
            var Controlador = new ClsControladorComision(new ClsRepositorioFalso());
            var Resultados = new List<ClsComisionVendedorResultado> { new ClsComisionVendedorResultado() };
            // Act
            Action Operacion = () => Controlador.VentasMetGuardar(Resultados, Inicio, Fin, EnumMetodoComision.Vendedor, EnumBaseComision.Cobrado);
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public void VentasMetActualizarPorcentajeClasificacionInvalidaRechaza(int Id)
        {
            // Arrange
            var Controlador = new ClsControladorComision(new ClsRepositorioFalso());
            // Act
            Action Operacion = () => Controlador.VentasMetActualizarPorcentaje(EnumMetodoComision.Marca, Id, 5m);
            // Assert
            Assert.ThrowsException<ArgumentException>(Operacion);
        }

        [DataTestMethod]
        [DataRow(-0.01)]
        [DataRow(99.01)]
        public void VentasMetActualizarPorcentajeFueraDeRangoRechaza(double Porcentaje)
        {
            // Arrange
            var Controlador = new ClsControladorComision(new ClsRepositorioFalso());
            // Act
            Action Operacion = () => Controlador.VentasMetActualizarPorcentaje(EnumMetodoComision.Marca, 1, (decimal)Porcentaje);
            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(Operacion);
        }

        [TestMethod]
        public void VentasMetActualizarPorcentajeValidoDelegaAlRepositorio()
        {
            // Arrange
            var Repositorio = new ClsRepositorioFalso();
            var Controlador = new ClsControladorComision(Repositorio);
            // Act
            int Actualizadas = Controlador.VentasMetActualizarPorcentaje(EnumMetodoComision.Producto, 7, 12.5m);
            // Assert
            Assert.AreEqual(1, Actualizadas);
            Assert.AreEqual("Producto", Repositorio.MetodoActualizado);
            Assert.AreEqual(7, Repositorio.IdActualizado);
            Assert.AreEqual(12.5m, Repositorio.PorcentajeActualizado);
        }

        [TestMethod]
        public void VentasMetCalcularFacturaAbonoParcialCalculaComisionDevengada()
        {
            // Arrange
            var Repositorio = new ClsRepositorioFalso();
            Repositorio.Factura = new ClsFacturaComision { IdFactura = 4, Total = 100m, TotalPagado = 50m, UltimoAbono = 20m };
            Repositorio.Vendedor = new ClsVendedorComision { IdVendedor = 8 };
            Repositorio.Detalles.Add(new ClsDetalleProductoComision { IdFacturaDetalle = 1, IdProductoLogistica = 2, IdClasificacionLogistica = 3, BaseComisionable = 100m, Porcentaje = 10m, PorcentajeProducto = 10m, NombreProducto = "Producto" });
            var Selecciones = new List<ClsComisionFacturaDetalle> { new ClsComisionFacturaDetalle { IdFacturaDetalle = 1, MetodoComision = "Producto" } };

            // Act
            var Resultado = new ClsControladorComision(Repositorio)
                .VentasMetCalcularFactura(4, 8, Selecciones).Single();

            // Assert
            Assert.AreEqual(10m, Resultado.ComisionTotal);
            Assert.AreEqual(5m, Resultado.ComisionDisponible);
            Assert.AreEqual(2m, Resultado.ComisionUltimoAbono);
            Assert.AreEqual(5m, Resultado.ComisionPendiente);
            Assert.AreEqual(50m, Resultado.VentaCobrado);
            Assert.AreEqual(1, Repositorio.LlamadasSincronizar);
        }

        [TestMethod]
        public void VentasMetCalcularFacturaMetodoVendedorRechaza()
        {
            // Arrange
            var Controlador = new ClsControladorComision(new ClsRepositorioFalso());
            // Act
            Action Operacion = () => Controlador.VentasMetCalcularFactura(1, 1, EnumMetodoComision.Vendedor);
            // Assert
            Assert.ThrowsException<ArgumentException>(Operacion);
        }

        [TestMethod]
        public void VentasMetRegistrarAbonoMontoCeroRechazaAntesDeAccederRepositorio()
        {
            // Arrange
            var Repositorio = new ClsRepositorioFalso();
            var Controlador = new ClsControladorComision(Repositorio);
            // Act
            Action Operacion = () => Controlador.VentasMetRegistrarAbonoFactura(1, 0m, 1, Inicio, "", 1);
            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(Operacion);
            Assert.AreEqual(0, Repositorio.LlamadasRegistrarAbono);
        }

        [TestMethod]
        public void VentasMetCalcularFacturaDosProductosDelMismoMetodoConsultaUnaVez()
        {
            var Repositorio = new ClsRepositorioFalso
            {
                Factura = new ClsFacturaComision { IdFactura = 4, Total = 100m },
                Vendedor = new ClsVendedorComision { IdVendedor = 8 }
            };
            for (int Id = 1; Id <= 2; Id++)
                Repositorio.Detalles.Add(new ClsDetalleProductoComision
                {
                    IdFacturaDetalle = Id, IdProductoLogistica = Id, IdClasificacionLogistica = Id,
                    BaseComisionable = 50m, Porcentaje = 10m
                });
            var Selecciones = Repositorio.Detalles.Select(Detalle => new ClsComisionFacturaDetalle
            {
                IdFacturaDetalle = Detalle.IdFacturaDetalle, MetodoComision = "Producto"
            }).ToList();

            var Resultado = new ClsControladorComision(Repositorio)
                .VentasMetCalcularFactura(4, 8, Selecciones);

            Assert.AreEqual(2, Resultado.Count);
            Assert.AreEqual(10m, Resultado.Sum(Detalle => Detalle.ComisionTotal));
            Assert.AreEqual(1, Repositorio.LlamadasConsultarDetalles);
        }

        private static ClsVentaVendedorBase VentasMetCrearVenta(decimal Base, decimal Comision, int Detalle)
        {
            return new ClsVentaVendedorBase { IdVendedor = 2, IdMoneda = 1, IdFactura = 5, IdFacturaDetalle = Detalle, NombreVendedor = "Vendedor", BaseVentas = Base, MontoComision = Comision };
        }

        // Simula el repositorio para verificar resultados y llamadas sin acceder a la base.
        private sealed class ClsRepositorioFalso : IRepositorioComision
        {
            public readonly List<ClsVentaVendedorBase> Ventas = new List<ClsVentaVendedorBase>();
            public readonly List<ClsDetalleProductoComision> Detalles = new List<ClsDetalleProductoComision>();
            public ClsFacturaComision Factura;
            public ClsVendedorComision Vendedor;
            public DateTime FechaInicio, FechaFinExclusiva;
            public int LlamadasConsultarDetalles;
            public int LlamadasGuardar, LlamadasSincronizar, LlamadasRegistrarAbono, IdActualizado;
            public string MetodoActualizado;
            public decimal PorcentajeActualizado;

            public List<ClsVentaVendedorBase> VentasMetObtenerVentasPorMetodo(string Metodo, DateTime FechaInicio, DateTime FechaFinExclusiva) { this.FechaInicio = FechaInicio; this.FechaFinExclusiva = FechaFinExclusiva; return Ventas; }
            public List<ClsProductoComision> VentasMetConsultarPorcentajes(string Metodo) { return new List<ClsProductoComision>(); }
            public int VentasMetActualizarPorcentaje(string Metodo, int IdClasificacion, decimal Porcentaje) { MetodoActualizado = Metodo; IdActualizado = IdClasificacion; PorcentajeActualizado = Porcentaje; return 1; }
            public int VentasMetGuardarComisiones(List<ClsVentaVendedorBase> Comisiones, DateTime FechaInicio, DateTime FechaFin, string Metodo) { LlamadasGuardar++; return Comisiones.Count; }
            public List<ClsFacturaComision> VentasMetObtenerFacturas() { return new List<ClsFacturaComision>(); }
            public List<ClsVendedorComision> VentasMetObtenerVendedores() { return new List<ClsVendedorComision>(); }
            public ClsFacturaComision VentasMetObtenerFactura(int IdFactura) { return Factura; }
            public ClsVendedorComision VentasMetObtenerVendedor(int IdVendedor) { return Vendedor; }
            public List<ClsDetalleProductoComision> VentasMetObtenerDetallesFactura(int IdFactura, string Metodo) { LlamadasConsultarDetalles++; return Detalles; }
            public int VentasMetAsignarVendedorFactura(int IdFactura, int IdVendedor) { return 1; }
            public int VentasMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, string Metodo) { return 1; }
            public int VentasMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, List<ClsDetalleProductoComision> Selecciones) { return Selecciones.Count; }
            public int VentasMetGuardarComisionFactura(int IdFactura, int IdVendedor, string Metodo) { return 1; }
            public int VentasMetGuardarComisionFacturaPorProducto(int IdFactura, int IdVendedor, List<ClsDetalleProductoComision> Selecciones) { return Selecciones.Count; }
            public void VentasMetSincronizarAbonos(int? IdFactura) { LlamadasSincronizar++; }
            public List<ClsFacturaPago> VentasMetObtenerFacturasConSaldo() { return new List<ClsFacturaPago>(); }
            public List<ClsFormaPagoVenta> VentasMetObtenerFormasPagoVenta() { return new List<ClsFormaPagoVenta>(); }
            public decimal VentasMetRegistrarAbonoFactura(int IdFactura, decimal Monto, int IdFormaPago, DateTime Fecha, string Referencia, int IdUsuario) { LlamadasRegistrarAbono++; return Monto; }
            public List<ClsComisionNomina> VentasMetObtenerComisionesNomina() { return new List<ClsComisionNomina>(); }
        }
    }
}
