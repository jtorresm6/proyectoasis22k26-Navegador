using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_Facturas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace TestFacturas
{
    [TestClass]
    public class TestFacturaUnitaria
    {
        [DataTestMethod]
        [DataRow("CF",true)] [DataRow(" cf ",true)] [DataRow("1234567-8",true)] [DataRow("1234567K",true)] [DataRow("1234567890123",true)]
        [DataRow("",false)] [DataRow("ABC",false)] [DataRow("12--3",false)] [DataRow("CF123",false)] [DataRow("123456789012345",false)]
        public void Identificacion(string texto,bool esperado){Assert.AreEqual(esperado,ReglasFactura.IdentificacionValida(texto));}
        [TestMethod] public void Normaliza(){Assert.AreEqual("CF",ReglasFactura.NormalizarIdentificacion(" cf "));Assert.AreEqual("",ReglasFactura.NormalizarIdentificacion(null));}
        [TestMethod] public void ImpuestoProvieneDelParametro(){CollectionAssert.AreEqual(new[]{90m,4.5m,94.5m},ReglasFactura.Calcular(3,35,15,.05m));CollectionAssert.AreEqual(new[]{90m,10.8m,100.8m},ReglasFactura.Calcular(3,35,15,.12m));}
        [TestMethod] public void Exento(){CollectionAssert.AreEqual(new[]{30m,0m,30m},ReglasFactura.Calcular(1,30,0,0));}
        [TestMethod] public void PrecisionIgualQueMySql(){CollectionAssert.AreEqual(new[]{1.2345m,.1481m,1.3826m},ReglasFactura.Calcular(1,1.2345m,0,.12m));Assert.AreEqual(1.2345m,ReglasFactura.Redondear(1.23445m));}
        [TestMethod] public void CantidadesInvalidas(){foreach(decimal c in new[]{0m,-1m,1.5m})Assert.ThrowsException<ArgumentException>(()=>ReglasFactura.Calcular(c,10,0,.12m));}
        [TestMethod] public void DescuentosInvalidos(){Assert.ThrowsException<ArgumentException>(()=>ReglasFactura.Calcular(1,10,-1,.12m));Assert.ThrowsException<ArgumentException>(()=>ReglasFactura.Calcular(1,10,11,.12m));Assert.ThrowsException<ArgumentException>(()=>ReglasFactura.Calcular(1,-10,0,.12m));}
        [TestMethod] public void TasaIncompatible(){foreach(decimal tasa in new[]{-1m,1m,.125m})Assert.ThrowsException<ArgumentException>(()=>ReglasFactura.Calcular(1,10,0,tasa));}
        [TestMethod] public void RecalculaSinConfiarEnTotalesDeVista(){var t=ReglasFactura.CrearDetalle();t.Rows.Add(1,"Jamón",2m,30m,5m,.12m,999m,999m,999m);ReglasFactura.Recalcular(t);Assert.AreEqual(55m,t.Rows[0]["Subtotal"]);Assert.AreEqual(6.6m,t.Rows[0]["Impuesto"]);Assert.AreEqual(61.6m,t.Rows[0]["Total"]);}
        [TestMethod] public void PrecisionDeImportes(){Assert.ThrowsException<ArgumentException>(()=>ReglasFactura.Calcular(1,1.12345m,0,.12m));Assert.ThrowsException<ArgumentException>(()=>ReglasFactura.Calcular(1,10,.00001m,.12m));}
        [TestMethod] public void DetalleVacio(){Assert.ThrowsException<ArgumentException>(()=>ReglasFactura.Recalcular(ReglasFactura.CrearDetalle()));}
        [TestMethod] public void UuidPorFactura(){var ids=new HashSet<string>();for(int i=0;i<10000;i++){var s=new SolicitudFactura();Assert.IsTrue(Guid.TryParse(s.Uuid,out _));Assert.IsTrue(ids.Add(s.Uuid));}}
        [TestMethod] public void RegistrarRequiereSolicitud(){Assert.ThrowsException<ArgumentNullException>(()=>new ClsFacturaOrigenRepositorio().Registrar(null,f=>{}));}
        [TestMethod] public void RegistrarRequiereSalida(){Assert.ThrowsException<ArgumentNullException>(()=>new ClsFacturaOrigenRepositorio().Registrar(new SolicitudFactura(),null));}
        [TestMethod] public void CondicionInvalidaAntesDeAccederABD(){Assert.ThrowsException<ArgumentException>(()=>new ClsFacturaOrigenRepositorio().Registrar(new SolicitudFactura{Condicion="OTRO"},f=>{}));}
    }
}
