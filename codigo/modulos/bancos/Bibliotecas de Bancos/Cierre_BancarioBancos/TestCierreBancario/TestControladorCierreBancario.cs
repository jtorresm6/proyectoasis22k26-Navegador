/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Autor  : Andre De Jesus Gonzalez Camey 
 * Carné  : 9959-23-1224
 * Fecha  : 08/11/2026
 * ================================================================
 * Propósito :
 *  PRUEBAS UNITARIAS DE CIERRE BANCARIO 
 * ================================================================
 */
using System;
using System.Data;
using CapaControlador_CierreBancario;
using CapaModelo_CierreBancario.Repositorio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCierreBancario
{
    [TestClass]
    public class TestControladorCierreBancario
    {
        [TestMethod]
        public void Test_NavegacionInicioConRegistros()
        {
            int Indice = ClsNavegacionHistorialCierre.BancosMetInicio(5);

            Assert.AreEqual(0, Indice);
        }

        [TestMethod]
        public void Test_NavegacionInicioSinRegistros()
        {
            int Indice = ClsNavegacionHistorialCierre.BancosMetInicio(0);

            Assert.AreEqual(-1, Indice);
        }

        [TestMethod]
        public void Test_NavegacionAnteriorAvanzaUnaFilaHaciaAtras()
        {
            int Indice = ClsNavegacionHistorialCierre.BancosMetAnterior(3, 5);

            Assert.AreEqual(2, Indice);
        }

        [TestMethod]
        public void Test_NavegacionAnteriorPermaneceAlInicio()
        {
            int Indice = ClsNavegacionHistorialCierre.BancosMetAnterior(0, 5);

            Assert.AreEqual(0, Indice);
        }

        [TestMethod]
        public void Test_NavegacionAnteriorSinSeleccionColocaPrimeraFila()
        {
            int Indice = ClsNavegacionHistorialCierre.BancosMetAnterior(-1, 5);

            Assert.AreEqual(0, Indice);
        }

        [TestMethod]
        public void Test_NavegacionSiguienteAvanzaUnaFila()
        {
            int Indice = ClsNavegacionHistorialCierre.BancosMetSiguiente(2, 5);

            Assert.AreEqual(3, Indice);
        }

        [TestMethod]
        public void Test_NavegacionSiguientePermaneceAlFinal()
        {
            int Indice = ClsNavegacionHistorialCierre.BancosMetSiguiente(4, 5);

            Assert.AreEqual(4, Indice);
        }

        [TestMethod]
        public void Test_NavegacionSiguienteSinSeleccionColocaPrimeraFila()
        {
            int Indice = ClsNavegacionHistorialCierre.BancosMetSiguiente(-1, 5);

            Assert.AreEqual(0, Indice);
        }

        [TestMethod]
        public void Test_NavegacionFinSeleccionaUltimaFila()
        {
            int Indice = ClsNavegacionHistorialCierre.BancosMetFin(5);

            Assert.AreEqual(4, Indice);
        }

        [TestMethod]
        public void Test_NavegacionFinSinRegistros()
        {
            int Indice = ClsNavegacionHistorialCierre.BancosMetFin(0);

            Assert.AreEqual(-1, Indice);
        }

        [TestMethod]
        public void Test_ConsultarRechazaFechaInicialInvalida()
        {
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario();

            string Error = Controlador.BancosMetConsultar("31/02/2026", "28/02/2026", out System.Data.DataTable Resultado);

            Assert.AreEqual("Fecha inicial no es válida. Use el formato dd/MM/aaaa.", Error);
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_ConsultarRechazaFechaFinalInvalida()
        {
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario();

            string Error = Controlador.BancosMetConsultar("01/02/2026", "2026-02-28", out System.Data.DataTable Resultado);

            Assert.AreEqual("Fecha final no es válida. Use el formato dd/MM/aaaa.", Error);
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_ConsultarRechazaRangoInvertido()
        {
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario();

            string Error = Controlador.BancosMetConsultar("02/02/2026", "01/02/2026", out System.Data.DataTable Resultado);

            Assert.AreEqual("La fecha inicial no puede ser mayor que la fecha final.", Error);
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_ValidarCierreRechazaFechaInvalida()
        {
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario();

            string Error = Controlador.BancosMetValidarCierre("fecha", "01/02/2026", out bool PeriodoValido,
                out bool ConciliacionesVerificadas, out bool SaldosVerificados);

            Assert.AreEqual("Fecha inicial no es válida.", Error);
            Assert.IsFalse(PeriodoValido);
            Assert.IsFalse(ConciliacionesVerificadas);
            Assert.IsFalse(SaldosVerificados);
        }

        [TestMethod]
        public void Test_ValidarCierreRechazaRangoInvertido()
        {
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario();

            string Error = Controlador.BancosMetValidarCierre("02/02/2026", "01/02/2026", out bool PeriodoValido,
                out bool ConciliacionesVerificadas, out bool SaldosVerificados);

            Assert.AreEqual("La fecha inicial no puede ser mayor que la fecha final.", Error);
            Assert.IsFalse(PeriodoValido);
            Assert.IsFalse(ConciliacionesVerificadas);
            Assert.IsFalse(SaldosVerificados);
        }

        [TestMethod]
        public void Test_CerrarPeriodoRechazaRangoInvertido()
        {
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario();

            string Error = Controlador.BancosMetCerrarPeriodo("02/02/2026", "01/02/2026");

            Assert.AreEqual("La fecha inicial no puede ser mayor que la fecha final.", Error);
        }

        [TestMethod]
        public void Test_ConstructorRechazaRepositorioNulo()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ClsControladorCierreBancario(null));
        }

        [TestMethod]
        public void Test_ConsultarAceptaRangoValidoYDevuelveResultado()
        {
            RepositorioFalso Repositorio = new RepositorioFalso();
            DataTable DatosEsperados = new DataTable();
            Repositorio.ResultadoResumen = DatosEsperados;
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario(Repositorio);

            string Error = Controlador.BancosMetConsultar("01/02/2026", "28/02/2026", out DataTable Resultado);

            Assert.AreEqual(string.Empty, Error);
            Assert.AreSame(DatosEsperados, Resultado);
            Assert.AreEqual(new DateTime(2026, 2, 1), Repositorio.FechaInicial);
            Assert.AreEqual(new DateTime(2026, 2, 28), Repositorio.FechaFinal);
        }

        [TestMethod]
        public void Test_ConsultarDevuelveErrorSiRepositorioFalla()
        {
            RepositorioFalso Repositorio = new RepositorioFalso
            {
                ErrorResumen = new InvalidOperationException()
            };
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario(Repositorio);

            string Error = Controlador.BancosMetConsultar("01/02/2026", "28/02/2026", out DataTable Resultado);

            Assert.AreEqual("Error inesperado al consultar el período.", Error);
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_ValidarCierreRechazaFechaFinalInvalida()
        {
            RepositorioFalso Repositorio = new RepositorioFalso();
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario(Repositorio);

            string Error = Controlador.BancosMetValidarCierre("01/02/2026", "fecha", out bool PeriodoValido,
                out bool ConciliacionesVerificadas, out bool SaldosVerificados);

            Assert.AreEqual("Fecha final no es válida.", Error);
            Assert.IsFalse(PeriodoValido);
            Assert.IsFalse(ConciliacionesVerificadas);
            Assert.IsFalse(SaldosVerificados);
            Assert.AreEqual(0, Repositorio.LlamadasValidacion);
        }

        [TestMethod]
        public void Test_ValidarCierreDevuelveLosTresRequisitos()
        {
            RepositorioFalso Repositorio = new RepositorioFalso
            {
                PeriodoSinCierre = true,
                ConciliacionesCompletas = true,
                SaldosVerificados = true
            };
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario(Repositorio);

            string Error = Controlador.BancosMetValidarCierre("01/02/2026", "28/02/2026", out bool PeriodoValido,
                out bool ConciliacionesVerificadas, out bool SaldosVerificados);

            Assert.AreEqual(string.Empty, Error);
            Assert.IsTrue(PeriodoValido);
            Assert.IsTrue(ConciliacionesVerificadas);
            Assert.IsTrue(SaldosVerificados);
            Assert.AreEqual(3, Repositorio.LlamadasValidacion);
            Assert.AreEqual(new DateTime(2026, 2, 1), Repositorio.FechaInicial);
            Assert.AreEqual(new DateTime(2026, 2, 28), Repositorio.FechaFinal);
        }

        [TestMethod]
        public void Test_ValidarCierrePermiteVerificarRequisitosPendientes()
        {
            RepositorioFalso Repositorio = new RepositorioFalso
            {
                PeriodoSinCierre = true,
                ConciliacionesCompletas = false,
                SaldosVerificados = false
            };
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario(Repositorio);

            string Error = Controlador.BancosMetValidarCierre("01/02/2026", "28/02/2026", out bool PeriodoValido,
                out bool ConciliacionesVerificadas, out bool SaldosVerificados);

            Assert.AreEqual(string.Empty, Error);
            Assert.IsTrue(PeriodoValido);
            Assert.IsFalse(ConciliacionesVerificadas);
            Assert.IsFalse(SaldosVerificados);
        }

        [TestMethod]
        public void Test_ValidarCierreDevuelveErrorSiRepositorioFalla()
        {
            RepositorioFalso Repositorio = new RepositorioFalso
            {
                ErrorValidacion = new InvalidOperationException()
            };
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario(Repositorio);

            string Error = Controlador.BancosMetValidarCierre("01/02/2026", "28/02/2026", out bool PeriodoValido,
                out bool ConciliacionesVerificadas, out bool SaldosVerificados);

            Assert.AreEqual("Error inesperado al validar el período.", Error);
            Assert.IsFalse(PeriodoValido);
            Assert.IsFalse(ConciliacionesVerificadas);
            Assert.IsFalse(SaldosVerificados);
        }

        [TestMethod]
        public void Test_CerrarPeriodoRechazaFechaInicialInvalida()
        {
            RepositorioFalso Repositorio = new RepositorioFalso();
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario(Repositorio);

            string Error = Controlador.BancosMetCerrarPeriodo("fecha", "28/02/2026");

            Assert.AreEqual("Fecha inicial no es válida.", Error);
            Assert.AreEqual(0, Repositorio.LlamadasCierre);
        }

        [TestMethod]
        public void Test_CerrarPeriodoRechazaFechaFinalInvalida()
        {
            RepositorioFalso Repositorio = new RepositorioFalso();
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario(Repositorio);

            string Error = Controlador.BancosMetCerrarPeriodo("01/02/2026", "fecha");

            Assert.AreEqual("Fecha final no es válida.", Error);
            Assert.AreEqual(0, Repositorio.LlamadasCierre);
        }

        [TestMethod]
        public void Test_CerrarPeriodoAceptaRangoValido()
        {
            RepositorioFalso Repositorio = new RepositorioFalso();
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario(Repositorio);

            string Error = Controlador.BancosMetCerrarPeriodo("01/02/2026", "28/02/2026");

            Assert.AreEqual(string.Empty, Error);
            Assert.AreEqual(1, Repositorio.LlamadasCierre);
            Assert.AreEqual(new DateTime(2026, 2, 1), Repositorio.FechaInicial);
            Assert.AreEqual(new DateTime(2026, 2, 28), Repositorio.FechaFinal);
        }

        [TestMethod]
        public void Test_CerrarPeriodoDevuelveRechazoDelRepositorio()
        {
            RepositorioFalso Repositorio = new RepositorioFalso
            {
                ResultadoCierre = "Hay saldos pendientes o diferencias sin resolver."
            };
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario(Repositorio);

            string Error = Controlador.BancosMetCerrarPeriodo("01/02/2026", "28/02/2026");

            Assert.AreEqual("Hay saldos pendientes o diferencias sin resolver.", Error);
        }

        [TestMethod]
        public void Test_CerrarPeriodoDevuelveErrorSiRepositorioFalla()
        {
            RepositorioFalso Repositorio = new RepositorioFalso
            {
                ErrorCierre = new InvalidOperationException()
            };
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario(Repositorio);

            string Error = Controlador.BancosMetCerrarPeriodo("01/02/2026", "28/02/2026");

            Assert.AreEqual("Error inesperado al cerrar el período.", Error);
        }

        [TestMethod]
        public void Test_ConsultarHistoricoDevuelveLosRegistros()
        {
            RepositorioFalso Repositorio = new RepositorioFalso();
            DataTable DatosEsperados = new DataTable();
            Repositorio.ResultadoHistorico = DatosEsperados;
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario(Repositorio);

            string Error = Controlador.BancosMetConsultarHistorico(out DataTable Resultado);

            Assert.AreEqual(string.Empty, Error);
            Assert.AreSame(DatosEsperados, Resultado);
        }

        [TestMethod]
        public void Test_ConsultarHistoricoDevuelveErrorSiRepositorioFalla()
        {
            RepositorioFalso Repositorio = new RepositorioFalso
            {
                ErrorHistorico = new InvalidOperationException()
            };
            ClsControladorCierreBancario Controlador = new ClsControladorCierreBancario(Repositorio);

            string Error = Controlador.BancosMetConsultarHistorico(out DataTable Resultado);

            Assert.AreEqual("Error inesperado al consultar el historial.", Error);
            Assert.IsNull(Resultado);
        }
    }

    internal sealed class RepositorioFalso : IClsRepositorioCierreBancario
    {
        public DataTable ResultadoResumen { get; set; } = new DataTable();
        public DataTable ResultadoHistorico { get; set; } = new DataTable();
        public bool PeriodoSinCierre { get; set; } = true;
        public bool ConciliacionesCompletas { get; set; } = true;
        public bool SaldosVerificados { get; set; } = true;
        public string ResultadoCierre { get; set; } = string.Empty;
        public Exception ErrorResumen { get; set; }
        public Exception ErrorValidacion { get; set; }
        public Exception ErrorCierre { get; set; }
        public Exception ErrorHistorico { get; set; }
        public DateTime FechaInicial { get; private set; }
        public DateTime FechaFinal { get; private set; }
        public int LlamadasValidacion { get; private set; }
        public int LlamadasCierre { get; private set; }

        public DataTable BancosMetConsultarResumenCuentas(DateTime FechaInicial, DateTime FechaFinal)
        {
            GuardarFechas(FechaInicial, FechaFinal);
            if (ErrorResumen != null) throw ErrorResumen;
            return ResultadoResumen;
        }

        public bool BancosMetValidarPeriodoSinCierre(DateTime FechaInicial, DateTime FechaFinal)
        {
            GuardarFechas(FechaInicial, FechaFinal);
            if (ErrorValidacion != null) throw ErrorValidacion;
            LlamadasValidacion++;
            return PeriodoSinCierre;
        }

        public bool BancosMetValidarConciliacionesCompletas(DateTime FechaInicial, DateTime FechaFinal)
        {
            GuardarFechas(FechaInicial, FechaFinal);
            if (ErrorValidacion != null) throw ErrorValidacion;
            LlamadasValidacion++;
            return ConciliacionesCompletas;
        }

        public bool BancosMetValidarSaldosVerificados(DateTime FechaInicial, DateTime FechaFinal)
        {
            GuardarFechas(FechaInicial, FechaFinal);
            if (ErrorValidacion != null) throw ErrorValidacion;
            LlamadasValidacion++;
            return SaldosVerificados;
        }

        public string BancosMetCerrarPeriodo(DateTime FechaInicial, DateTime FechaFinal)
        {
            GuardarFechas(FechaInicial, FechaFinal);
            LlamadasCierre++;
            if (ErrorCierre != null) throw ErrorCierre;
            return ResultadoCierre;
        }

        public DataTable BancosMetConsultarHistorico()
        {
            if (ErrorHistorico != null) throw ErrorHistorico;
            return ResultadoHistorico;
        }

        private void GuardarFechas(DateTime Inicio, DateTime Fin)
        {
            FechaInicial = Inicio;
            FechaFinal = Fin;
        }
    }
}
