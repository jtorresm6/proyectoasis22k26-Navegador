/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 30/10/2026
 * ==================================================================
 * Propósito :
 *  Pruebas unitarias MSTest de la capa controlador de los estados
 *  financieros: tipos de estado, niveles de detalle, filtro por nivel,
 *  clasificación del flujo de caja, monedas, validación de fechas,
 *  conversión de moneda y totalización recursiva del árbol de cuentas.
 * Reglas especificas:
 *  Cada prueba sigue Arrange, Act y Assert. Ninguna usa la base de
 *  datos; los métodos privados se prueban por reflexión.
 * ===================================================================
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_Contabilidad;
using CapaModelo_Contabilidad.Entidades;

namespace TestEstadosFinancieros
{
    [TestClass]
    public class TestEstadosFinancieros
    {
        private const BindingFlags _Privado = BindingFlags.NonPublic | BindingFlags.Static;

        [TestMethod]
        public void Test_TiposEstado()
        {
            // Arrange
            ClsControladorEstadosFinancierosContabilidad Controlador = new ClsControladorEstadosFinancierosContabilidad();
            // Act
            string[] Tipos = Controlador.ContabilidadFuncTiposEstado();
            // Assert
            CollectionAssert.AreEqual(new[] { "Balance general", "Estado de resultados", "Flujo de caja" }, Tipos);
        }

        [TestMethod]
        public void Test_Niveles()
        {
            // Arrange
            ClsControladorEstadosFinancierosContabilidad Controlador = new ClsControladorEstadosFinancierosContabilidad();
            // Act
            string[] Niveles = Controlador.ContabilidadFuncNiveles();
            // Assert
            Assert.AreEqual(4, Niveles.Length);
            Assert.AreEqual("Nivel 4 - Detalle completo", Niveles[3]);
        }

        [TestMethod]
        public void Test_FiltrarNivel()
        {
            // Arrange
            ClsControladorEstadosFinancierosContabilidad Controlador = new ClsControladorEstadosFinancierosContabilidad();
            DataTable Filas = new DataTable();
            Filas.Columns.Add("Cuenta", typeof(string));
            Filas.Columns.Add("Nivel", typeof(int));
            Filas.Columns.Add("EsTotal", typeof(bool));
            Filas.Rows.Add("Activo", 0, false);
            Filas.Rows.Add("Activo Corriente", 1, false);
            Filas.Rows.Add("Bancos", 2, false);
            Filas.Rows.Add("Banco Industrial", 3, false);
            Filas.Rows.Add("Total activo", 0, true);
            // Act
            DataTable Resultado = Controlador.ContabilidadFuncFiltrarNivel(Filas, 2);
            // Assert
            Assert.AreEqual(3, Resultado.Rows.Count);
            Assert.AreEqual("Total activo", Resultado.Rows[2]["Cuenta"]);
        }

        [TestMethod]
        public void Test_ActividadFlujoCaja()
        {
            // Arrange
            string Mobiliario = "1.02.01.001";
            string Prestamo = "2.02.01.001";
            string Capital = "3.01.01";
            string Ventas = "4.01.01.001";
            // Act
            string Inversion = ClsControladorEstadosFinancierosContabilidad.ContabilidadFuncActividad(Mobiliario);
            string Financiamiento = ClsControladorEstadosFinancierosContabilidad.ContabilidadFuncActividad(Prestamo);
            string Aportes = ClsControladorEstadosFinancierosContabilidad.ContabilidadFuncActividad(Capital);
            string Operacion = ClsControladorEstadosFinancierosContabilidad.ContabilidadFuncActividad(Ventas);
            // Assert
            Assert.AreEqual("INVERSIÓN", Inversion);
            Assert.AreEqual("FINANCIAMIENTO", Financiamiento);
            Assert.AreEqual("FINANCIAMIENTO", Aportes);
            Assert.AreEqual("OPERACIÓN", Operacion);
        }

        [TestMethod]
        public void Test_Monedas()
        {
            // Arrange
            ClsControladorEstadosFinancierosContabilidad Controlador = new ClsControladorEstadosFinancierosContabilidad();
            DataTable Paises = new DataTable();
            Paises.Columns.Add("codigoMonedaPais", typeof(string));
            Paises.Rows.Add("USD");
            Paises.Rows.Add("GTQ");
            Paises.Rows.Add("USD");
            Paises.Rows.Add("HNL");
            // Act
            string[] Monedas = Controlador.ContabilidadFuncMonedas(Paises);
            // Assert
            CollectionAssert.AreEqual(new[] { "USD", "GTQ", "HNL" }, Monedas);
        }

        [TestMethod]
        public void Test_ErrorFechas()
        {
            // Arrange
            ClsControladorEstadosFinancierosContabilidad Controlador = new ClsControladorEstadosFinancierosContabilidad();
            DateTime Inicio = new DateTime(2026, 10, 1);
            DateTime Fin = new DateTime(2026, 9, 1);
            // Act
            string Invertidas = Controlador.ContabilidadFuncErrorFechas(Inicio, Fin);
            string Correctas = Controlador.ContabilidadFuncErrorFechas(Fin, Inicio);
            // Assert
            Assert.AreNotEqual(string.Empty, Invertidas);
            Assert.AreEqual(string.Empty, Correctas);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_EstadoResultadosFechasInvertidas()
        {
            // Arrange
            ClsControladorEstadosFinancierosContabilidad Controlador = new ClsControladorEstadosFinancierosContabilidad();
            DateTime Desde = new DateTime(2026, 10, 1);
            DateTime Hasta = new DateTime(2026, 9, 1);
            // Act
            Controlador.ContabilidadFuncEstadoResultados(Desde, Hasta, 1, "GTQ");
            // Assert
            Assert.Fail("Debió rechazar un rango con el mes final antes del inicial.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_FlujoCajaFechasInvertidas()
        {
            // Arrange
            ClsControladorEstadosFinancierosContabilidad Controlador = new ClsControladorEstadosFinancierosContabilidad();
            DateTime Desde = new DateTime(2026, 10, 1);
            DateTime Hasta = new DateTime(2026, 9, 1);
            // Act
            Controlador.ContabilidadFuncFlujoCaja(Desde, Hasta, 1, "GTQ");
            // Assert
            Assert.Fail("Debió rechazar un rango con el mes final antes del inicial.");
        }

        [TestMethod]
        public void Test_FactorConversion()
        {
            // Arrange
            Dictionary<string, decimal> Tipos = new Dictionary<string, decimal> { { "USD>GTQ", 7.6650m }, { "USD>HNL", 26.2400m } };
            MethodInfo Factor = typeof(ClsControladorEstadosFinancierosContabilidad).GetMethod("ContabilidadFuncFactor", _Privado);
            // Act
            decimal? Misma = (decimal?)Factor.Invoke(null, new object[] { Tipos, "GTQ", "GTQ" });
            decimal? Directa = (decimal?)Factor.Invoke(null, new object[] { Tipos, "USD", "GTQ" });
            decimal? Inversa = (decimal?)Factor.Invoke(null, new object[] { Tipos, "GTQ", "USD" });
            decimal? Cruzada = (decimal?)Factor.Invoke(null, new object[] { Tipos, "HNL", "GTQ" });
            decimal? SinTipo = (decimal?)Factor.Invoke(null, new object[] { Tipos, "GTQ", "EUR" });
            // Assert
            Assert.AreEqual(1m, Misma);
            Assert.AreEqual(7.6650m, Directa);
            Assert.AreEqual(1m / 7.6650m, Inversa);
            Assert.AreEqual(Math.Round(7.6650m / 26.2400m, 6), Math.Round(Cruzada.Value, 6));
            Assert.IsNull(SinTipo);
        }

        [TestMethod]
        public void Test_TotalizacionRecursiva()
        {
            // Arrange
            List<ClsCuentaSaldoContabilidad> Cuentas = new List<ClsCuentaSaldoContabilidad>
            {
                new ClsCuentaSaldoContabilidad { IdCuenta = 1, Codigo = "1", Nombre = "Activo", Tipo = "DEUDORA", Clasificacion = "MAYOR" },
                new ClsCuentaSaldoContabilidad { IdCuenta = 2, IdCuentaPadre = 1, Codigo = "1.01", Nombre = "Activo Corriente", Tipo = "DEUDORA", Clasificacion = "MAYOR" },
                new ClsCuentaSaldoContabilidad { IdCuenta = 3, IdCuentaPadre = 2, Codigo = "1.01.01", Nombre = "Caja", Tipo = "DEUDORA", Clasificacion = "DETALLE", SaldoFinal = 100m },
                new ClsCuentaSaldoContabilidad { IdCuenta = 4, IdCuentaPadre = 2, Codigo = "1.01.02", Nombre = "Bancos", Tipo = "DEUDORA", Clasificacion = "DETALLE", SaldoFinal = 50m },
                new ClsCuentaSaldoContabilidad { IdCuenta = 5, IdCuentaPadre = 1, Codigo = "1.02", Nombre = "Mobiliario", Tipo = "DEUDORA", Clasificacion = "DETALLE", SaldoFinal = 30m }
            };
            Func<ClsCuentaSaldoContabilidad, decimal> Saldo = Cuenta => Cuenta.SaldoFinal;
            MethodInfo Arbol = typeof(ClsControladorEstadosFinancierosContabilidad).GetMethod("ContabilidadFuncArbol", _Privado);
            // Act
            IDictionary Nodos = (IDictionary)Arbol.Invoke(null, new object[] { Cuentas, Saldo });
            // Assert
            Assert.AreEqual(180m, ContabilidadFuncTotal(Nodos["1"]));
            Assert.AreEqual(150m, ContabilidadFuncTotal(Nodos["1.01"]));
            Assert.AreEqual(30m, ContabilidadFuncTotal(Nodos["1.02"]));
        }

        private static decimal ContabilidadFuncTotal(object Nodo)
        {
            return (decimal)Nodo.GetType().GetField("Total").GetValue(Nodo);
        }
    }
}
