using System;
using System.Collections.Generic;
using System.Data;
using CapaControlador_Contabilidad;
using CapaControlador_Polizas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PruebaUnitaria_Polizas
{
    [TestClass]
    public class TestControladorPolizasCompras
    {
        private IntegracionFalsa _Integracion;
        private RepositorioFalso _Repositorio;
        private ClsControladorPolizasCompras _Controlador;

        [TestInitialize]
        public void Preparar()
        {
            _Integracion = new IntegracionFalsa();
            _Repositorio = new RepositorioFalso();
            _Controlador = new ClsControladorPolizasCompras(_Integracion, _Repositorio);
        }

        [TestMethod]
        public void CuentasCompras_ConsultaModuloYMapeaResultados()
        {
            _Integracion.Cuentas.Add(new ClsModeloCuentaPolizaContabilidad { IdCuenta = 12, Codigo = "1.01.06.001", Nombre = "Materias primas" });

            List<ClsModeloCuentaPolizaCompras> Resultado = _Controlador.ContabilidadFuncCuentasCompras();

            Assert.AreEqual("MOD-COMPR", _Integracion.ModuloConsultado);
            Assert.AreEqual(1, Resultado.Count);
            Assert.AreEqual("1.01.06.001", Resultado[0].Codigo);
            Assert.AreEqual("Materias primas", Resultado[0].Nombre);
            Assert.AreEqual("1.01.06.001 - Materias primas", Resultado[0].Texto);
        }

        [TestMethod]
        public void ConsultarCuentasPorPagar_UsaRepositorio()
        {
            DataTable Esperada = new DataTable("CuentasPorPagar");
            _Repositorio.Resultado = Esperada;

            Assert.AreSame(Esperada, _Controlador.ComprasFuncConsultarCuentasPorPagar());
            Assert.AreEqual(1, _Repositorio.Consultas);
        }

        [TestMethod]
        public void EnviarPoliza_ValidaMapeaLineasYDevuelveRespuesta()
        {
            ClsModeloPolizaCompras Poliza = CrearPolizaValida();

            bool Resultado = _Controlador.ContabilidadFuncEnviarPoliza(Poliza, out string Codigo, out string Estado, out string Mensaje);

            Assert.IsTrue(Resultado);
            Assert.AreEqual("POL-TEST-001", Codigo);
            Assert.AreEqual("VALIDADA", Estado);
            Assert.AreEqual("Recibida", Mensaje);
            Assert.AreEqual("MOD-COMPR", _Integracion.ModuloEnviado);
            Assert.AreEqual("FAC-001", _Integracion.DocumentoEnviado);
            Assert.AreEqual(2, _Integracion.LineasEnviadas.Count);
            Assert.AreEqual("1.01.06.001", _Integracion.LineasEnviadas[0].CodigoCuenta);
            Assert.AreEqual(100m, _Integracion.LineasEnviadas[0].Debe);
            Assert.AreEqual(100m, _Integracion.LineasEnviadas[1].Haber);
        }

        [TestMethod]
        public void EnviarPoliza_RechazaDocumentoVacioSinLlamarContabilidad()
        {
            ClsModeloPolizaCompras Poliza = CrearPolizaValida();
            Poliza.DocumentoOrigen = "   ";

            bool Resultado = _Controlador.ContabilidadFuncEnviarPoliza(Poliza, out string Codigo, out string Estado, out string Mensaje);

            Assert.IsFalse(Resultado);
            Assert.AreEqual(string.Empty, Codigo);
            Assert.IsTrue(Mensaje.Contains("documento de origen"));
            Assert.AreEqual(0, _Integracion.Envios);
        }

        [TestMethod]
        public void EnviarPoliza_RechazaPolizaDescuadrada()
        {
            ClsModeloPolizaCompras Poliza = CrearPolizaValida();
            Poliza.Lineas[1].Haber = 99m;

            bool Resultado = _Controlador.ContabilidadFuncEnviarPoliza(Poliza, out string Codigo, out string Estado, out string Mensaje);

            Assert.IsFalse(Resultado);
            Assert.IsTrue(Mensaje.Contains("no está cuadrada"));
            Assert.AreEqual(0, _Integracion.Envios);
        }

        [TestMethod]
        public void EnviarPoliza_RechazaLineaConDebeYHaber()
        {
            ClsModeloPolizaCompras Poliza = CrearPolizaValida();
            Poliza.Lineas[0].Haber = 1m;

            bool Resultado = _Controlador.ContabilidadFuncEnviarPoliza(Poliza, out string Codigo, out string Estado, out string Mensaje);

            Assert.IsFalse(Resultado);
            Assert.IsTrue(Mensaje.Contains("Debe o Haber"));
            Assert.AreEqual(0, _Integracion.Envios);
        }

        [TestMethod]
        public void EnviarPoliza_PropagaRechazoDeContabilidad()
        {
            _Integracion.Respuesta = false;
            _Integracion.Mensaje = "Cuenta no permitida";

            bool Resultado = _Controlador.ContabilidadFuncEnviarPoliza(CrearPolizaValida(), out string Codigo, out string Estado, out string Mensaje);

            Assert.IsFalse(Resultado);
            Assert.AreEqual("Cuenta no permitida", Mensaje);
            Assert.AreEqual(1, _Integracion.Envios);
        }

        private static ClsModeloPolizaCompras CrearPolizaValida()
        {
            return new ClsModeloPolizaCompras
            {
                DocumentoOrigen = " FAC-001 ", Fecha = new DateTime(2026, 11, 3), Descripcion = "Compra de prueba",
                Lineas = new List<ClsModeloLineaPolizaCompras>
                {
                    new ClsModeloLineaPolizaCompras { CodigoCuenta = " 1.01.06.001 ", Debe = 100m, Descripcion = "Inventario" },
                    new ClsModeloLineaPolizaCompras { CodigoCuenta = "2.01.01.001", Haber = 100m, Descripcion = "Proveedor" }
                }
            };
        }

        private sealed class IntegracionFalsa : IIntegracionContabilidadPolizasCompras
        {
            public readonly List<ClsModeloCuentaPolizaContabilidad> Cuentas = new List<ClsModeloCuentaPolizaContabilidad>();
            public string ModuloConsultado;
            public string ModuloEnviado;
            public string DocumentoEnviado;
            public List<ClsModeloLineaIntegracionContabilidad> LineasEnviadas;
            public int Envios;
            public bool Respuesta = true;
            public string Mensaje = "Recibida";

            public List<ClsModeloCuentaPolizaContabilidad> ContabilidadFuncCuentasPermitidas(string CodigoModulo)
            {
                ModuloConsultado = CodigoModulo;
                return Cuentas;
            }

            public bool ContabilidadFuncRecibirPoliza(string CodigoModulo, string DocumentoOrigen, DateTime Fecha, string Descripcion,
                List<ClsModeloLineaIntegracionContabilidad> Lineas, out string CodigoPoliza, out string Estado, out string MensajeSalida)
            {
                Envios++;
                ModuloEnviado = CodigoModulo;
                DocumentoEnviado = DocumentoOrigen;
                LineasEnviadas = Lineas;
                CodigoPoliza = "POL-TEST-001";
                Estado = "VALIDADA";
                MensajeSalida = Mensaje;
                return Respuesta;
            }
        }

        private sealed class RepositorioFalso : IRepositorioPolizasCompras
        {
            public DataTable Resultado = new DataTable();
            public int Consultas;

            public DataTable ComprasFuncConsultarCuentasPorPagar()
            {
                Consultas++;
                return Resultado;
            }
        }
    }
}
