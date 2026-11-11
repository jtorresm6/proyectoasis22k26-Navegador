using System;
using System.Data;
using System.Linq;
using CapaModelo_EnlaceB.Contratos;
using CapaModelo_EnlaceB.Entidades;
using CapaControlador_EnlaceB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestEnlaceBancos
{
    [TestClass]
    public class ClsTestControladorEnlace
    {
        private static ClsContextoPartida BancarioMetContexto()
        { return new ClsContextoPartida { Empresa = 1, Pais = 1, ModuloContable = 1, Responsable = "usuario", Inicio = new DateTime(2026, 11, 1), Fin = new DateTime(2026, 11, 30) }; }
        private static DataTable BancarioMetDetalle(decimal ImporteDebe, decimal ImporteHaber)
        {
            var TablaDatos = new DataTable(); TablaDatos.Columns.Add("CodigoCuentaDetallePartida"); TablaDatos.Columns.Add("NombreCuentaDetallePartida");
            TablaDatos.Columns.Add("DebeDetallePartida", typeof(decimal)); TablaDatos.Columns.Add("HaberDetallePartida", typeof(decimal));
            TablaDatos.Rows.Add("1.01.01.001", "Banco", ImporteDebe, ImporteHaber); return TablaDatos;
        }
        [TestMethod] public void BancarioMetFechasInvertidas_NoConsultaRepositorio()
        {
            var Registro = new ClsRepositorioFalso(); var Elemento = new ClsControladorEnlaceB(Registro);
            Assert.ThrowsException<ArgumentException>(() => Elemento.BancarioMetConsultarModulo(ModuloEnlace.Logistica, new DateTime(2026, 11, 2), new DateTime(2026, 11, 1)));
            Assert.AreEqual(0, Registro.Llamadas);
        }
        [TestMethod] public void BancarioMetModuloInvalido_NoConsultaRepositorio()
        {
            var Registro = new ClsRepositorioFalso(); var Elemento = new ClsControladorEnlaceB(Registro);
            Assert.ThrowsException<ArgumentException>(() => Elemento.BancarioMetConsultarModulo((ModuloEnlace)90, DateTime.Today, DateTime.Today)); Assert.AreEqual(0, Registro.Llamadas);
        }
        [TestMethod] public void BancarioMetConsultaValida_EnviaModuloYFechasSinHora()
        {
            var Registro = new ClsRepositorioFalso(); var Elemento = new ClsControladorEnlaceB(Registro);
            var TablaDatos = Elemento.BancarioMetConsultarModulo(ModuloEnlace.Ventas, new DateTime(2026, 11, 1, 12, 0, 0), new DateTime(2026, 11, 2, 12, 0, 0));
            Assert.AreSame(Registro.Datos, TablaDatos); Assert.AreEqual(ModuloEnlace.Ventas, Registro.Modulo); Assert.AreEqual(TimeSpan.Zero, Registro.Inicio.TimeOfDay); Assert.AreEqual(1, Registro.Llamadas);
        }
        [TestMethod] public void BancarioMetErrorRepositorio_NoSeConvierteEnExito()
        {
            var Registro = new ClsRepositorioFalso { Error = true }; var Elemento = new ClsControladorEnlaceB(Registro);
            Assert.ThrowsException<InvalidOperationException>(() => Elemento.BancarioMetConsultarModulo(ModuloEnlace.Compras, DateTime.Today, DateTime.Today));
        }
        [TestMethod] public void BancarioMetGenerarSinContexto_NoLlamaRepositorio()
        { var Registro = new ClsRepositorioFalso(); Assert.ThrowsException<ArgumentException>(() => new ClsControladorEnlaceB(Registro).BancarioMetGenerar(null)); Assert.AreEqual(0, Registro.Llamadas); }
        [TestMethod] public void BancarioMetGenerarMesIncompleto_NoLlamaRepositorio()
        { var Registro = new ClsRepositorioFalso(); var PosicionX = BancarioMetContexto(); PosicionX.Inicio = PosicionX.Inicio.AddDays(1); Assert.ThrowsException<ArgumentException>(() => new ClsControladorEnlaceB(Registro).BancarioMetGenerar(PosicionX)); Assert.AreEqual(0, Registro.Llamadas); }
        [TestMethod] public void BancarioMetGenerarAnual_NoLlamaRepositorio()
        { var Registro = new ClsRepositorioFalso(); var PosicionX = BancarioMetContexto(); PosicionX.Inicio = new DateTime(2026, 1, 1); PosicionX.Fin = new DateTime(2026, 12, 31); Assert.ThrowsException<ArgumentException>(() => new ClsControladorEnlaceB(Registro).BancarioMetGenerar(PosicionX)); Assert.AreEqual(0, Registro.Llamadas); }
        [TestMethod] public void BancarioMetFebreroBisiesto_AceptaMesCompleto()
        { ClsReglasEnlace.BancarioMetValidarFechas(new DateTime(2024, 2, 1), new DateTime(2024, 2, 29), true); }
        [TestMethod] public void BancarioMetGenerarSinResponsable_NoLlamaRepositorio()
        { var Registro = new ClsRepositorioFalso(); var PosicionX = BancarioMetContexto(); PosicionX.Responsable = "  "; Assert.ThrowsException<ArgumentException>(() => new ClsControladorEnlaceB(Registro).BancarioMetGenerar(PosicionX)); Assert.AreEqual(0, Registro.Llamadas); }
        [TestMethod] public void BancarioMetGenerarSinPais_NoLlamaRepositorio()
        { var Registro = new ClsRepositorioFalso(); var PosicionX = BancarioMetContexto(); PosicionX.Pais = 0; Assert.ThrowsException<ArgumentException>(() => new ClsControladorEnlaceB(Registro).BancarioMetGenerar(PosicionX)); Assert.AreEqual(0, Registro.Llamadas); }
        [TestMethod] public void BancarioMetGenerarValido_ConservaIdentidadDevuelta()
        { var Registro = new ClsRepositorioFalso(); var Parametro = new ClsControladorEnlaceB(Registro).BancarioMetGenerar(BancarioMetContexto()); Assert.AreEqual(25L, Parametro.Id); Assert.AreEqual(1, Registro.Llamadas); }
        [TestMethod] public void BancarioMetEnviarSinPartida_NoLlamaRepositorio()
        { var Registro = new ClsRepositorioFalso(); Assert.ThrowsException<ArgumentException>(() => new ClsControladorEnlaceB(Registro).BancarioMetEnviar(0, BancarioMetContexto())); Assert.AreEqual(0, Registro.Llamadas); }
        [TestMethod] public void BancarioMetEnviarValido_DevuelveReferenciaReal()
        { var Registro = new ClsRepositorioFalso(); Assert.AreEqual("BANC-PC-25", new ClsControladorEnlaceB(Registro).BancarioMetEnviar(25, BancarioMetContexto())); Assert.AreEqual(25L, Registro.Id); }
        [TestMethod] public void BancarioMetObtenerSinSeleccion_NoLlamaRepositorio()
        { var Registro = new ClsRepositorioFalso(); Assert.ThrowsException<ArgumentException>(() => new ClsControladorEnlaceB(Registro).BancarioMetObtener(-1)); Assert.AreEqual(0, Registro.Llamadas); }
        [TestMethod] public void BancarioMetLineaConDosLados_SeparaYConservaTotales()
        {
            var TablaDatos = BancarioMetDetalle(1000, 200); TablaDatos.Rows.Add("4.01.01.001", "Ingreso", 0m, 1000m); TablaDatos.Rows.Add("6.01.01.001", "Gasto", 200m, 0m);
            var Registro = ClsReglasEnlace.BancarioMetPrepararLineasContables(TablaDatos);
            Assert.AreEqual(4, Registro.Rows.Count); Assert.AreEqual(1200m, Registro.Rows.Cast<DataRow>().Sum(PosicionX => (decimal)PosicionX[1]));
            Assert.AreEqual(1200m, Registro.Rows.Cast<DataRow>().Sum(PosicionX => (decimal)PosicionX[2])); Assert.IsTrue(Registro.Rows.Cast<DataRow>().All(PosicionX => (decimal)PosicionX[1] == 0 || (decimal)PosicionX[2] == 0));
        }
        [TestMethod] public void BancarioMetDetalleDescuadrado_SeRechaza()
        { Assert.ThrowsException<ArgumentException>(() => ClsReglasEnlace.BancarioMetPrepararLineasContables(BancarioMetDetalle(10, 9))); }
        [TestMethod] public void BancarioMetDetalleNegativo_SeRechaza()
        { Assert.ThrowsException<ArgumentException>(() => ClsReglasEnlace.BancarioMetPrepararLineasContables(BancarioMetDetalle(-10, 0))); }
        [TestMethod] public void BancarioMetDetalleCero_SeRechaza()
        { Assert.ThrowsException<ArgumentException>(() => ClsReglasEnlace.BancarioMetPrepararLineasContables(BancarioMetDetalle(0, 0))); }
        [TestMethod] public void BancarioMetCodigoDuplicado_SeRechaza()
        { var TablaDatos = BancarioMetDetalle(10, 0); TablaDatos.Rows.Add("1.01.01.001", "Banco", 0m, 10m); Assert.ThrowsException<ArgumentException>(() => ClsReglasEnlace.BancarioMetPrepararLineasContables(TablaDatos)); }
        [TestMethod] public void BancarioMetImporteFueraDeCapacidad_SeRechaza()
        { Assert.ThrowsException<ArgumentException>(() => ClsReglasEnlace.BancarioMetPrepararLineasContables(BancarioMetDetalle(100000000m, 100000000m))); }
        [TestMethod] public void BancarioMetDetalleVacio_SeRechaza()
        { var TablaDatos = BancarioMetDetalle(1, 1); TablaDatos.Clear(); Assert.ThrowsException<ArgumentException>(() => ClsReglasEnlace.BancarioMetPrepararLineasContables(TablaDatos)); }


        [TestMethod] public void BancarioMetCodigoConLetrasSeRechaza()
        { var Detalle = BancarioMetDetalle(10, 0); Detalle.Rows[0][0] = "TST-ENL-BANCO"; Assert.ThrowsException<ArgumentException>(() => ClsReglasEnlace.BancarioMetPrepararLineasContables(Detalle)); }
        [TestMethod] public void BancarioMetCodigoSinCerosSeRechaza()
        { var Detalle = BancarioMetDetalle(10, 0); Detalle.Rows[0][0] = "1.1.1.1"; Assert.ThrowsException<ArgumentException>(() => ClsReglasEnlace.BancarioMetPrepararLineasContables(Detalle)); }
        [TestMethod] public void BancarioMetClaseFueraDelCatalogoSeRechaza()
        { var Detalle = BancarioMetDetalle(10, 0); Detalle.Rows[0][0] = "7.01.01.001"; Assert.ThrowsException<ArgumentException>(() => ClsReglasEnlace.BancarioMetPrepararLineasContables(Detalle)); }

        [TestMethod] public void BancarioMetCodigoConEspaciosSeRechaza()
        { var Detalle = BancarioMetDetalle(10, 0); Detalle.Rows[0][0] = " 1.01.01.001 "; Assert.ThrowsException<ArgumentException>(() => ClsReglasEnlace.BancarioMetPrepararLineasContables(Detalle)); }

        private sealed class ClsRepositorioFalso : IClsRepositorioEnlaceB
        {
            internal int Llamadas; internal bool Error; internal ModuloEnlace Modulo; internal DateTime Inicio; internal long Id;
            internal DataTable Datos = new DataTable();
            public DataTable BancarioMetConsultarModulo(ModuloEnlace Modulo, DateTime Indice, DateTime Formulario)
            { Llamadas++; Modulo = Modulo; Inicio = Indice; if (Error) throw new InvalidOperationException("Fallo de prueba"); return Datos; }
            public DataTable BancarioMetConsultarCatalogo(string Nombre) { Llamadas++; return Datos; }
            public DataTable BancarioMetConsultarPolizas(DateTime Indice, DateTime Formulario) { Llamadas++; return Datos; }
            public DataTable BancarioMetConsultarHistorico() { Llamadas++; return Datos; }
            public ClsPartidaEnlace BancarioMetGenerarPartida(ClsContextoPartida Elemento) { Llamadas++; return new ClsPartidaEnlace { Id = 25, Estado = "VALIDADA", Detalle = Datos }; }
            public ClsPartidaEnlace BancarioMetObtenerPartida(long Id) { Llamadas++; return new ClsPartidaEnlace { Id = Id, Detalle = Datos }; }
            public string BancarioMetEnviarPartida(long Id, ClsContextoPartida Elemento) { Llamadas++; Id = Id; return "BANC-PC-25"; }
        }
    }
}
