using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CapaControlador_DisponibilidadDiariaBancos;
using CapaModelo_DisponibilidadDiariaBancos.Entidades;
using CapaModelo_DisponibilidadDiariaBancos.Repositorio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTestDisponibilidadDiariaBancos
{
    [TestClass]
    public class PruebasDisponibilidadDiaria
    {
        [TestMethod]
        public void Disponible_ConDatosQuemados_CalculaSaldoCorrectamente()
        {
            // Arrange y Act
            var disponibilidad = CrearDisponibilidad(
                cuentaId: 1, moneda: "GTQ", saldoInicial: 10000m,
                ingresos: 2500m, egresos: 1200m, comprometido: 800m,
                restringido: 500m, tasa: 1m);

            // Assert: 10,000 + 2,500 - 1,200 - 800 - 500
            Assert.AreEqual(10000m, disponibilidad.Disponible);
        }

        [TestMethod]
        public void ValidarRegistro_UsdConTasaQuemada_AceptaDatosValidos()
        {
            // Arrange
            var disponibilidad = CrearDisponibilidad(
                cuentaId: 2, moneda: "USD", saldoInicial: 1000m,
                ingresos: 200m, egresos: 100m, comprometido: 50m,
                restringido: 25m, tasa: 7.75m);

            // Act y Assert: si hay excepción, MSTest marca la prueba como fallida.
            ClsRepositorioDisponibilidad.BancosMetValidarRegistro(disponibilidad);
            Assert.AreEqual(1025m, disponibilidad.Disponible);
        }

        [TestMethod]
        public void ValidarRegistro_UsdSinTipoCambio_RechazaElRegistro()
        {
            // Arrange
            var disponibilidad = CrearDisponibilidad(
                cuentaId: 2, moneda: "USD", saldoInicial: 1000m,
                ingresos: 0m, egresos: 0m, comprometido: 0m,
                restringido: 0m, tasa: null);

            // Act y Assert
            var error = Assert.ThrowsException<ArgumentException>(() =>
                ClsRepositorioDisponibilidad.BancosMetValidarRegistro(disponibilidad));
            StringAssert.Contains(error.Message, "tasa GTQ positiva");
        }

        [TestMethod]
        public async Task ConsultarAsync_TodasLasMonedas_ConsolidaLosImportesEnGtq()
        {
            // Arrange: Q1,000 y USD100 a Q7.75 producen Q1,775 consolidados.
            var repositorio = new RepositorioDisponibilidadQuemado();
            repositorio.Consulta.Detalle.Add(CrearDisponibilidad(
                1, "GTQ", 1000m, 0m, 0m, 0m, 0m, 1m));
            repositorio.Consulta.Detalle.Add(CrearDisponibilidad(
                2, "USD", 100m, 0m, 0m, 0m, 0m, 7.75m));
            var controlador = new ClsControladorDisponibilidad(
                repositorio, new ProveedorTipoCambioQuemado());

            // Act
            ClsConsultaDisponibilidad resultado = await controlador.BancosMetConsultarAsync(
                DateTime.Today, null, null, "Todas", CancellationToken.None);

            // Assert
            Assert.IsTrue(resultado.Resumen.EsConsolidado);
            Assert.IsTrue(resultado.Resumen.ConversionCompleta);
            Assert.AreEqual("GTQ", resultado.Resumen.Moneda);
            Assert.AreEqual(1775m, resultado.Resumen.SaldoInicial);
            Assert.AreEqual(1775m, resultado.Resumen.Disponible);
            Assert.AreEqual(0, resultado.Resumen.CuentasSinTasa);
        }

        [TestMethod]
        public async Task ConsultarAsync_CuentaExtranjeraSinTasa_NoInventaConsolidado()
        {
            // Arrange
            var repositorio = new RepositorioDisponibilidadQuemado();
            repositorio.Consulta.Detalle.Add(CrearDisponibilidad(
                2, "USD", 100m, 0m, 0m, 0m, 0m, null));
            var controlador = new ClsControladorDisponibilidad(
                repositorio, new ProveedorTipoCambioQuemado());

            // Act
            ClsConsultaDisponibilidad resultado = await controlador.BancosMetConsultarAsync(
                DateTime.Today, null, null, "Todas", CancellationToken.None);

            // Assert
            Assert.IsFalse(resultado.Resumen.ConversionCompleta);
            Assert.AreEqual(1, resultado.Resumen.CuentasSinTasa);
            Assert.IsNull(resultado.Resumen.Disponible);
            StringAssert.Contains(resultado.Mensaje, "Faltan tasas válidas");
        }

        [TestMethod]
        public async Task ObtenerTipoCambioAsync_ProveedorQuemado_NormalizaLaMoneda()
        {
            // Arrange
            var proveedor = new ProveedorTipoCambioQuemado(
                new KeyValuePair<string, decimal>("USD", 7.75m));
            var controlador = new ClsControladorDisponibilidad(
                new RepositorioDisponibilidadQuemado(), proveedor);

            // Act
            ClsCotizacionDisponibilidad resultado = await controlador.BancosMetObtenerTipoCambioAsync(
                " usd ", DateTime.Today, CancellationToken.None);

            // Assert
            Assert.AreEqual("USD", resultado.Moneda);
            Assert.AreEqual(7.75m, resultado.TasaAplicada);
            CollectionAssert.AreEqual(new[] { "USD" }, proveedor.MonedasSolicitadas);
        }

        [TestMethod]
        public async Task GuardarAsync_DatosQuemados_NormalizaYAsignaUsuarioSinBaseDeDatos()
        {
            // Arrange
            var repositorio = new RepositorioDisponibilidadQuemado();
            var controlador = new ClsControladorDisponibilidad(
                repositorio, new ProveedorTipoCambioQuemado());
            controlador.BancosMetEstablecerUsuarioRegistro(25, "  usuario.pruebas  ");
            var original = CrearDisponibilidad(
                2, " usd ", 1000m, 100m, 50m, 25m, 0m, 7.75m);

            // Act
            await controlador.BancosMetGuardarAsync(original, false, CancellationToken.None);

            // Assert
            Assert.AreEqual(1, repositorio.RegistrosGuardados.Count);
            ClsDisponibilidad guardado = repositorio.RegistrosGuardados[0];
            Assert.AreEqual("USD", guardado.Moneda);
            Assert.AreEqual(25, guardado.UsuarioRegistroId);
            Assert.AreEqual("usuario.pruebas", guardado.UsuarioRegistro);
            Assert.IsFalse(repositorio.Reemplazos[0]);
            Assert.AreNotSame(original, guardado);
        }

        [TestMethod]
        public async Task ActualizarAutomaticamenteAsync_TasaQuemada_CalculaYGuardaEquivalenteGtq()
        {
            // Arrange
            var repositorio = new RepositorioDisponibilidadQuemado();
            repositorio.Calculo.Registros.Add(CrearDisponibilidad(
                7, "USD", 1000m, 200m, 100m, 50m, 25m, null));
            repositorio.Consulta.Detalle.Clear();
            var proveedor = new ProveedorTipoCambioQuemado(
                new KeyValuePair<string, decimal>("USD", 7.75m));
            var controlador = new ClsControladorDisponibilidad(repositorio, proveedor);

            // Act
            ClsResultadoActualizacionDisponibilidad resultado =
                await controlador.BancosMetActualizarAutomaticamenteAsync(
                    DateTime.Today, CancellationToken.None);

            // Assert: disponible USD = 1,025; equivalente = Q7,943.75.
            Assert.AreEqual(1, resultado.CuentasCalculadas);
            Assert.AreEqual(1, resultado.CuentasActualizadas);
            Assert.AreEqual(0, resultado.CuentasSinCambios);
            Assert.AreEqual(1, repositorio.RegistrosGuardados.Count);
            Assert.AreEqual(7.75m, repositorio.RegistrosGuardados[0].TipoCambioAplicado);
            Assert.AreEqual(7943.75m, repositorio.RegistrosGuardados[0].DisponibleGtq);
            StringAssert.Contains(repositorio.RegistrosGuardados[0].Fuente, ";TC:");
        }

        private static ClsDisponibilidad CrearDisponibilidad(
            long cuentaId, string moneda, decimal saldoInicial, decimal ingresos,
            decimal egresos, decimal comprometido, decimal restringido, decimal? tasa)
        {
            decimal disponible = saldoInicial + ingresos - egresos - comprometido - restringido;
            return new ClsDisponibilidad
            {
                CuentaId = cuentaId,
                BancoId = 1,
                Banco = "Banco de prueba",
                NumeroCuenta = "CTA-" + cuentaId,
                TipoCuenta = "Monetaria",
                Moneda = moneda,
                FechaCorte = DateTime.Today,
                SaldoInicial = saldoInicial,
                Ingresos = ingresos,
                Egresos = egresos,
                Comprometido = comprometido,
                Restringido = restringido,
                TipoCambioAplicado = tasa,
                DisponibleGtq = tasa.HasValue
                    ? (decimal?)Math.Round(disponible * tasa.Value, 4, MidpointRounding.AwayFromZero)
                    : null,
                Fuente = "Datos quemados para prueba unitaria",
                FechaGeneracion = DateTime.Today,
                VersionRegistro = 0
            };
        }
    }
}
