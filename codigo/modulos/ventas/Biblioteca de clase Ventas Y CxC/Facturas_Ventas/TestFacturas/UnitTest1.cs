using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_Facturas_Ventas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace TestFacturas
{
    [TestClass]
    public class TestFacturaUnitaria
    {
        [DataTestMethod]
        [DataRow("CF",true)] [DataRow(" cf ",true)] [DataRow("1234567-8",true)] [DataRow("1234567K",true)] [DataRow("1234567890123",true)]
        [DataRow("",false)] [DataRow("ABC",false)] [DataRow("12--3",false)] [DataRow("CF123",false)] [DataRow("123456789012345",false)]
        public void VentasMetVerificarIdentificacion(string Texto,bool Esperado){Assert.AreEqual(Esperado,ClsReglasFactura.VentasFuncIdentificacionValida(Texto));}
        [TestMethod] public void VentasMetNormaliza(){Assert.AreEqual("CF",ClsReglasFactura.VentasFuncNormalizarIdentificacion(" cf "));Assert.AreEqual("",ClsReglasFactura.VentasFuncNormalizarIdentificacion(null));}
        [TestMethod] public void VentasMetImpuestoProvieneDelParametro(){CollectionAssert.AreEqual(new[]{85.7143m,4.2857m,90m},ClsReglasFactura.VentasFuncCalcular(3,35,15,.05m));CollectionAssert.AreEqual(new[]{80.3571m,9.6429m,90m},ClsReglasFactura.VentasFuncCalcular(3,35,15,.12m));}
        [TestMethod] public void VentasMetExento(){CollectionAssert.AreEqual(new[]{30m,0m,30m},ClsReglasFactura.VentasFuncCalcular(1,30,0,0));}
        [TestMethod] public void VentasMetPrecisionIgualQueMySql(){CollectionAssert.AreEqual(new[]{1.1022m,.1323m,1.2345m},ClsReglasFactura.VentasFuncCalcular(1,1.2345m,0,.12m));Assert.AreEqual(1.2345m,ClsReglasFactura.VentasFuncRedondear(1.23445m));}
        [TestMethod] public void VentasMetCantidadesInvalidas(){foreach(decimal Cliente in new[]{0m,-1m,1.5m})Assert.ThrowsException<ArgumentException>(()=>ClsReglasFactura.VentasFuncCalcular(Cliente,10,0,.12m));}
        [TestMethod] public void VentasMetDescuentosInvalidos(){Assert.ThrowsException<ArgumentException>(()=>ClsReglasFactura.VentasFuncCalcular(1,10,-1,.12m));Assert.ThrowsException<ArgumentException>(()=>ClsReglasFactura.VentasFuncCalcular(1,10,11,.12m));Assert.ThrowsException<ArgumentException>(()=>ClsReglasFactura.VentasFuncCalcular(1,-10,0,.12m));}
        [TestMethod] public void VentasMetTasaIncompatible(){foreach(decimal Tasa in new[]{-1m,1m,.125m})Assert.ThrowsException<ArgumentException>(()=>ClsReglasFactura.VentasFuncCalcular(1,10,0,Tasa));}
        [TestMethod] public void VentasMetRecalculaSinConfiarEnTotalesDeVista(){var Tabla=ClsReglasFactura.VentasFuncCrearDetalle();Tabla.Rows.Add(1,"Jamón",2m,30m,5m,.12m,999m,999m,999m);ClsReglasFactura.VentasMetRecalcular(Tabla);Assert.AreEqual(49.1071m,Tabla.Rows[0]["Subtotal"]);Assert.AreEqual(5.8929m,Tabla.Rows[0]["Impuesto"]);Assert.AreEqual(55m,Tabla.Rows[0]["Total"]);}
        [TestMethod] public void VentasMetPrecisionDeImportes(){Assert.ThrowsException<ArgumentException>(()=>ClsReglasFactura.VentasFuncCalcular(1,1.12345m,0,.12m));Assert.ThrowsException<ArgumentException>(()=>ClsReglasFactura.VentasFuncCalcular(1,10,.00001m,.12m));}
        [TestMethod] public void VentasMetDetalleVacio(){Assert.ThrowsException<ArgumentException>(()=>ClsReglasFactura.VentasMetRecalcular(ClsReglasFactura.VentasFuncCrearDetalle()));}
        [TestMethod] public void VentasMetUuidPorFactura(){var Ids=new HashSet<string>();for(int I=0;I<10000;I++){var Solicitud=new ClsSolicitudFactura();Assert.IsTrue(Guid.TryParse(Solicitud.Uuid,out _));Assert.IsTrue(Ids.Add(Solicitud.Uuid));}}
        [TestMethod] public void VentasMetRegistrarRequiereSolicitud(){Assert.ThrowsException<ArgumentNullException>(()=>new ClsFacturaOrigenRepositorio().VentasFuncRegistrar(null,Factura=>{}));}
        [TestMethod] public void VentasMetRegistrarRequiereSalida(){Assert.ThrowsException<ArgumentNullException>(()=>new ClsFacturaOrigenRepositorio().VentasFuncRegistrar(new ClsSolicitudFactura(),null));}
        [TestMethod] public void VentasMetCondicionInvalidaAntesDeAccederABD(){Assert.ThrowsException<ArgumentException>(()=>new ClsFacturaOrigenRepositorio().VentasFuncRegistrar(new ClsSolicitudFactura{Condicion="OTRO"},Factura=>{}));}

        [DataTestMethod]
        [DataRow(1,38,0)] [DataRow(2,38,6)] [DataRow(20,18,0)] [DataRow(3,35,15)]
        public void VentasMetPersistenciaCoincideConIvaIncluido(int Cantidad, int Precio, int Descuento)
        {
            decimal[] Importes=ClsReglasFactura.VentasFuncCalcular(Cantidad,Precio,Descuento,.12m);
            decimal[] Base=ClsReglasFactura.VentasFuncConvertirDetallePersistido(Cantidad,Precio,Descuento,.12m);
            decimal Subtotal=ClsReglasFactura.VentasFuncRedondear(Cantidad*Base[0]-Base[1]);
            decimal Impuesto=ClsReglasFactura.VentasFuncRedondear(Subtotal*.12m);
            Assert.AreEqual(Importes[0],Subtotal);
            Assert.AreEqual(Importes[1],Impuesto);
            Assert.AreEqual(Cantidad*Precio-Descuento,Subtotal+Impuesto);
        }
        [TestMethod]
        public void VentasMetDescuentoTotalNoGeneraImpuesto()
        {
            CollectionAssert.AreEqual(new[]{0m,0m,0m},ClsReglasFactura.VentasFuncCalcular(2,38,76,.12m));
            decimal[] Base=ClsReglasFactura.VentasFuncConvertirDetallePersistido(2,38,76,.12m);
            Assert.AreEqual(0m,2*Base[0]-Base[1]);
        }
        [TestMethod]
        public void VentasMetPrecisionNoRepresentableNoSeGuardaConDiferencias()
        {
            Assert.ThrowsException<ArgumentException>(()=>ClsReglasFactura.VentasFuncConvertirDetallePersistido(1,.0005m,0,.12m));
        }
        [TestMethod]
        public void VentasMetControladorValidaSalidaAntesDelModelo()
        {
            Assert.ThrowsException<ArgumentNullException>(()=>new CapaControlador_Facturas_Ventas.ClsFacturaOrigenControlador().VentasFuncRegistrar(null,Factura=>{}));
            Assert.ThrowsException<ArgumentNullException>(()=>new CapaControlador_Facturas_Ventas.ClsFacturaOrigenControlador().VentasFuncRegistrar(new CapaControlador_Facturas_Ventas.ClsSolicitudFactura(),null));
        }
    }
}
