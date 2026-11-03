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
    public class TestComisionVendedores
    {
        private static readonly DateTime Inicio = new DateTime(2026, 1, 1);
        private static readonly DateTime Fin = new DateTime(2026, 1, 31);

        [TestMethod]
        public void Calcular_AbonosDeMismaVenta_AgrupaYAcumula()
        {
            // Arrange
            var Repositorio = new RepositorioFalso();
            Repositorio.Ventas.Add(Venta(10m, 1m, 3));
            Repositorio.Ventas.Add(Venta(20m, 2m, 3));
            var Controlador = new ClsControladorComision(Repositorio);

            // Act
            var Resultado = Controlador.ComisionVendedoresMetCalcular(
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
        public void Calcular_FechaInicialPosteriorAFechaFinal_Rechaza()
        {
            // Arrange
            var Controlador = new ClsControladorComision(new RepositorioFalso());
            // Act
            Action Operacion = () => Controlador.ComisionVendedoresMetCalcular(
                EnumMetodoComision.Vendedor, EnumBaseComision.Cobrado, Fin, Inicio);
            // Assert
            Assert.ThrowsException<ArgumentException>(Operacion);
        }

        [TestMethod]
        public void Calcular_BaseVentasNoSoportada_Rechaza()
        {
            // Arrange
            var Controlador = new ClsControladorComision(new RepositorioFalso());
            // Act
            Action Operacion = () => Controlador.ComisionVendedoresMetCalcular(
                EnumMetodoComision.Vendedor, EnumBaseComision.Ventas, Inicio, Fin);
            // Assert
            Assert.ThrowsException<NotSupportedException>(Operacion);
        }

        [TestMethod]
        public void Guardar_SinResultados_NoLlamaRepositorio()
        {
            // Arrange
            var Repositorio = new RepositorioFalso();
            var Controlador = new ClsControladorComision(Repositorio);
            // Act
            int Guardadas = Controlador.ComisionVendedoresMetGuardar(new List<ClsComisionVendedorResultado>(), Inicio, Fin, EnumMetodoComision.Vendedor, EnumBaseComision.Cobrado);
            // Assert
            Assert.AreEqual(0, Guardadas);
            Assert.AreEqual(0, Repositorio.LlamadasGuardar);
        }

        [TestMethod]
        public void Guardar_ResultadoSinMovimientos_Rechaza()
        {
            // Arrange
            var Controlador = new ClsControladorComision(new RepositorioFalso());
            var Resultados = new List<ClsComisionVendedorResultado> { new ClsComisionVendedorResultado() };
            // Act
            Action Operacion = () => Controlador.ComisionVendedoresMetGuardar(Resultados, Inicio, Fin, EnumMetodoComision.Vendedor, EnumBaseComision.Cobrado);
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public void ActualizarPorcentaje_ClasificacionInvalida_Rechaza(int Id)
        {
            // Arrange
            var Controlador = new ClsControladorComision(new RepositorioFalso());
            // Act
            Action Operacion = () => Controlador.ComisionVendedoresMetActualizarPorcentaje(EnumMetodoComision.Marca, Id, 5m);
            // Assert
            Assert.ThrowsException<ArgumentException>(Operacion);
        }

        [DataTestMethod]
        [DataRow(-0.01)]
        [DataRow(99.01)]
        public void ActualizarPorcentaje_FueraDeRango_Rechaza(double Porcentaje)
        {
            // Arrange
            var Controlador = new ClsControladorComision(new RepositorioFalso());
            // Act
            Action Operacion = () => Controlador.ComisionVendedoresMetActualizarPorcentaje(EnumMetodoComision.Marca, 1, (decimal)Porcentaje);
            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(Operacion);
        }

        [TestMethod]
        public void ActualizarPorcentaje_Valido_DelegaAlRepositorio()
        {
            // Arrange
            var Repositorio = new RepositorioFalso();
            var Controlador = new ClsControladorComision(Repositorio);
            // Act
            int Actualizadas = Controlador.ComisionVendedoresMetActualizarPorcentaje(EnumMetodoComision.Producto, 7, 12.5m);
            // Assert
            Assert.AreEqual(1, Actualizadas);
            Assert.AreEqual("Producto", Repositorio.MetodoActualizado);
            Assert.AreEqual(7, Repositorio.IdActualizado);
            Assert.AreEqual(12.5m, Repositorio.PorcentajeActualizado);
        }

        [TestMethod]
        public void CalcularFactura_AbonoParcial_CalculaComisionDevengada()
        {
            // Arrange
            var Repositorio = new RepositorioFalso();
            Repositorio.Factura = new ClsFacturaComision { IdFactura = 4, Total = 100m, TotalPagado = 50m, UltimoAbono = 20m };
            Repositorio.Vendedor = new ClsVendedorComision { IdVendedor = 8 };
            Repositorio.Detalles.Add(new ClsDetalleProductoComision { IdFacturaDetalle = 1, IdProductoLogistica = 2, IdClasificacionLogistica = 3, BaseComisionable = 100m, Porcentaje = 10m, PorcentajeProducto = 10m, NombreProducto = "Producto" });
            var Selecciones = new List<ClsComisionFacturaDetalle> { new ClsComisionFacturaDetalle { IdFacturaDetalle = 1, MetodoComision = "Producto" } };

            // Act
            var Resultado = new ClsControladorComision(Repositorio)
                .ComisionVendedoresMetCalcularFactura(4, 8, Selecciones).Single();

            // Assert
            Assert.AreEqual(10m, Resultado.ComisionTotal);
            Assert.AreEqual(5m, Resultado.ComisionDisponible);
            Assert.AreEqual(2m, Resultado.ComisionUltimoAbono);
            Assert.AreEqual(5m, Resultado.ComisionPendiente);
            Assert.AreEqual(50m, Resultado.VentaCobrado);
            Assert.AreEqual(1, Repositorio.LlamadasSincronizar);
        }

        [TestMethod]
        public void CalcularFactura_MetodoVendedor_Rechaza()
        {
            // Arrange
            var Controlador = new ClsControladorComision(new RepositorioFalso());
            // Act
            Action Operacion = () => Controlador.ComisionVendedoresMetCalcularFactura(1, 1, EnumMetodoComision.Vendedor);
            // Assert
            Assert.ThrowsException<ArgumentException>(Operacion);
        }

        [TestMethod]
        public void RegistrarAbono_MontoCero_RechazaAntesDeAccederRepositorio()
        {
            // Arrange
            var Repositorio = new RepositorioFalso();
            var Controlador = new ClsControladorComision(Repositorio);
            // Act
            Action Operacion = () => Controlador.ComisionVendedoresMetRegistrarAbonoFactura(1, 0m, 1, Inicio, "", 1);
            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(Operacion);
            Assert.AreEqual(0, Repositorio.LlamadasRegistrarAbono);
        }

        private static ClsVentaVendedorBase Venta(decimal Base, decimal Comision, int Detalle)
        {
            return new ClsVentaVendedorBase { IdVendedor = 2, IdMoneda = 1, IdFactura = 5, IdFacturaDetalle = Detalle, NombreVendedor = "Vendedor", BaseVentas = Base, MontoComision = Comision };
        }

        private sealed class RepositorioFalso : IRepositorioComision
        {
            public readonly List<ClsVentaVendedorBase> Ventas = new List<ClsVentaVendedorBase>();
            public readonly List<ClsDetalleProductoComision> Detalles = new List<ClsDetalleProductoComision>();
            public ClsFacturaComision Factura;
            public ClsVendedorComision Vendedor;
            public DateTime FechaInicio, FechaFinExclusiva;
            public int LlamadasGuardar, LlamadasSincronizar, LlamadasRegistrarAbono, IdActualizado;
            public string MetodoActualizado;
            public decimal PorcentajeActualizado;

            public List<ClsVentaVendedorBase> ComisionVendedoresMetObtenerVentasPorMetodo(string Metodo, DateTime FechaInicio, DateTime FechaFinExclusiva) { this.FechaInicio = FechaInicio; this.FechaFinExclusiva = FechaFinExclusiva; return Ventas; }
            public List<ClsProductoComision> ComisionVendedoresMetConsultarPorcentajes(string Metodo) { return new List<ClsProductoComision>(); }
            public int ComisionVendedoresMetActualizarPorcentaje(string Metodo, int IdClasificacion, decimal Porcentaje) { MetodoActualizado = Metodo; IdActualizado = IdClasificacion; PorcentajeActualizado = Porcentaje; return 1; }
            public int ComisionVendedoresMetGuardarComisiones(List<ClsVentaVendedorBase> Comisiones, DateTime FechaInicio, DateTime FechaFin, string Metodo) { LlamadasGuardar++; return Comisiones.Count; }
            public List<ClsFacturaComision> ComisionVendedoresMetObtenerFacturas() { return new List<ClsFacturaComision>(); }
            public List<ClsVendedorComision> ComisionVendedoresMetObtenerVendedores() { return new List<ClsVendedorComision>(); }
            public ClsFacturaComision ComisionVendedoresMetObtenerFactura(int IdFactura) { return Factura; }
            public ClsVendedorComision ComisionVendedoresMetObtenerVendedor(int IdVendedor) { return Vendedor; }
            public List<ClsDetalleProductoComision> ComisionVendedoresMetObtenerDetallesFactura(int IdFactura, string Metodo) { return Detalles; }
            public int ComisionVendedoresMetAsignarVendedorFactura(int IdFactura, int IdVendedor) { return 1; }
            public int ComisionVendedoresMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, string Metodo) { return 1; }
            public int ComisionVendedoresMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, List<ClsDetalleProductoComision> Selecciones) { return Selecciones.Count; }
            public int ComisionVendedoresMetGuardarComisionFactura(int IdFactura, int IdVendedor, string Metodo) { return 1; }
            public int ComisionVendedoresMetGuardarComisionFacturaPorProducto(int IdFactura, int IdVendedor, List<ClsDetalleProductoComision> Selecciones) { return Selecciones.Count; }
            public void ComisionVendedoresMetSincronizarAbonos(int? IdFactura) { LlamadasSincronizar++; }
            public List<ClsFacturaPago> ComisionVendedoresMetObtenerFacturasConSaldo() { return new List<ClsFacturaPago>(); }
            public List<ClsFormaPagoVenta> ComisionVendedoresMetObtenerFormasPagoVenta() { return new List<ClsFormaPagoVenta>(); }
            public decimal ComisionVendedoresMetRegistrarAbonoFactura(int IdFactura, decimal Monto, int IdFormaPago, DateTime Fecha, string Referencia, int IdUsuario) { LlamadasRegistrarAbono++; return Monto; }
        }
    }
}
