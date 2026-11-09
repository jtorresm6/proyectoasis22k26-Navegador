/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  Pruebas unitarias MSTest de Activos fijos: validaciones del modelo,
 *  reglas del controlador y enlace con Logística.
 * Reglas especificas:
 *  Cada prueba sigue Arrange, Act y Assert. Ninguna usa la base de
 *  datos.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_Contabilidad;
using CapaModelo_Contabilidad.Entidades;

// Inicio de cambio: Gabriel Guillén - 0901-23-1998 - 09/11/2026
namespace TestActivosFijos
{
    // Pruebas del modelo
    [TestClass]
    public class TestModeloActivosFijos
    {
        [TestMethod]
        public void Test_Modelo_DatosValidos_SinErrores()
        {
            // Arrange
            ClsModeloActivoFijoContabilidad Modelo = new ClsModeloActivoFijoContabilidad
            {
                Descripcion = "Computadora",
                Adquisicion = new DateTime(2026, 1, 1),
                Inicio = new DateTime(2026, 2, 1),
                Compra = 5000m,
                Residual = 500m,
                Meses = 36
            };
            ValidationContext Contexto = new ValidationContext(Modelo);

            // Act
            List<ValidationResult> Errores = Modelo.Validate(Contexto).ToList();

            // Assert
            Assert.AreEqual(0, Errores.Count);
        }

        [TestMethod]
        public void Test_Modelo_InicioAnteriorAAdquisicion_ErrorInicio()
        {
            // Arrange
            ClsModeloActivoFijoContabilidad Modelo = new ClsModeloActivoFijoContabilidad
            {
                Descripcion = "Computadora",
                Adquisicion = new DateTime(2026, 5, 1),
                Inicio = new DateTime(2026, 4, 1),
                Compra = 5000m,
                Residual = 500m,
                Meses = 36
            };
            ValidationContext Contexto = new ValidationContext(Modelo);

            // Act
            List<ValidationResult> Errores = Modelo.Validate(Contexto).ToList();

            // Assert
            Assert.AreEqual(1, Errores.Count);
            Assert.IsTrue(Errores[0].MemberNames.Contains("Inicio"));
        }

        [TestMethod]
        public void Test_Modelo_CompraMenorOIgualACero_ErrorCompra()
        {
            // Arrange
            ClsModeloActivoFijoContabilidad Modelo = new ClsModeloActivoFijoContabilidad
            {
                Descripcion = "Computadora",
                Adquisicion = new DateTime(2026, 1, 1),
                Inicio = new DateTime(2026, 2, 1),
                Compra = 0m,
                Residual = 0m,
                Meses = 36
            };
            ValidationContext Contexto = new ValidationContext(Modelo);

            // Act
            List<ValidationResult> Errores = Modelo.Validate(Contexto).ToList();

            // Assert
            Assert.AreEqual(1, Errores.Count);
            Assert.IsTrue(Errores[0].MemberNames.Contains("Compra"));
        }

        [TestMethod]
        public void Test_Modelo_ResidualMayorQueCompra_ErrorResidual()
        {
            // Arrange
            ClsModeloActivoFijoContabilidad Modelo = new ClsModeloActivoFijoContabilidad
            {
                Descripcion = "Computadora",
                Adquisicion = new DateTime(2026, 1, 1),
                Inicio = new DateTime(2026, 2, 1),
                Compra = 1000m,
                Residual = 1500m,
                Meses = 36
            };
            ValidationContext Contexto = new ValidationContext(Modelo);

            // Act
            List<ValidationResult> Errores = Modelo.Validate(Contexto).ToList();

            // Assert
            Assert.AreEqual(1, Errores.Count);
            Assert.IsTrue(Errores[0].MemberNames.Contains("Residual"));
        }
    }

    // Pruebas del controlador
    [TestClass]
    public class TestControladorActivosFijos
    {
        // Controlador probado
        private ClsControladorActivosFijosContabilidad _Controlador = null!;

        // Crea el controlador
        [TestInitialize]
        public void ContabilidadMetIniciar()
        {
            _Controlador = new ClsControladorActivosFijosContabilidad();
        }

        [TestMethod]
        public void Test_ErroresActivo_DatosValidos_DiccionarioVacio()
        {
            // Arrange
            string Descripcion = "Laptop";
            DateTime Adquisicion = new DateTime(2026, 1, 15);
            DateTime Inicio = new DateTime(2026, 2, 1);
            decimal Compra = 5000m;
            decimal Residual = 500m;
            int Meses = 36;

            // Act
            Dictionary<string, string> Errores = _Controlador.ContabilidadFuncErroresActivo(
                Descripcion, Adquisicion, Inicio, Compra, Residual, Meses);

            // Assert
            Assert.AreEqual(0, Errores.Count);
        }

        [TestMethod]
        public void Test_ErroresActivo_DescripcionVacia_ErrorDescripcion()
        {
            // Arrange
            string Descripcion = "";
            DateTime Adquisicion = new DateTime(2026, 1, 15);
            DateTime Inicio = new DateTime(2026, 2, 1);
            decimal Compra = 5000m;
            decimal Residual = 500m;
            int Meses = 36;

            // Act
            Dictionary<string, string> Errores = _Controlador.ContabilidadFuncErroresActivo(
                Descripcion, Adquisicion, Inicio, Compra, Residual, Meses);

            // Assert
            Assert.IsTrue(Errores.ContainsKey("Descripcion"));
        }

        [TestMethod]
        public void Test_ValidarActivo_DatosValidos_CadenaVacia()
        {
            // Arrange
            string Descripcion = "Computadora";
            DateTime Adquisicion = new DateTime(2026, 1, 15);
            DateTime Inicio = new DateTime(2026, 2, 1);
            decimal Compra = 5000m;
            decimal Residual = 500m;
            int Meses = 36;

            // Act
            string Mensaje = _Controlador.ContabilidadFuncValidarActivo(
                Descripcion, Adquisicion, Inicio, Compra, Residual, Meses);

            // Assert
            Assert.AreEqual(string.Empty, Mensaje);
        }

        [TestMethod]
        public void Test_ValidarActivo_DatosInvalidos_PrimerMensaje()
        {
            // Arrange
            string Descripcion = "";
            DateTime Adquisicion = new DateTime(2026, 1, 15);
            DateTime Inicio = new DateTime(2026, 2, 1);
            decimal Compra = 5000m;
            decimal Residual = 500m;
            int Meses = 36;

            // Act
            string Mensaje = _Controlador.ContabilidadFuncValidarActivo(
                Descripcion, Adquisicion, Inicio, Compra, Residual, Meses);

            // Assert
            Assert.IsNotNull(Mensaje);
            Assert.AreNotEqual(string.Empty, Mensaje);
        }

        [TestMethod]
        public void Test_GrabarCambios_DescripcionVacia_NoGraba()
        {
            // Arrange
            int IdActivo = 0;
            string Descripcion = "";
            string Modulo = "Logística";
            string Tipo = "VEHICULO";
            DateTime Adquisicion = new DateTime(2026, 1, 15);
            DateTime Inicio = new DateTime(2026, 2, 1);
            decimal Compra = 5000m;
            decimal Residual = 500m;
            int Meses = 36;
            string Mensaje;

            // Act
            bool Resultado = _Controlador.ContabilidadFuncGrabarCambios(
                EstadoEntidadContabilidad.Agregar, ref IdActivo, Descripcion, Modulo, Tipo,
                Adquisicion, Inicio, Compra, Residual, Meses, out Mensaje);

            // Assert
            Assert.IsFalse(Resultado);
            Assert.AreNotEqual(string.Empty, Mensaje);
        }

        [TestMethod]
        public void Test_GrabarCambios_ModuloVacio_NoGraba()
        {
            // Arrange
            int IdActivo = 0;
            string Descripcion = "Laptop";
            string Modulo = "";
            string Tipo = "EQ_COMPUTO";
            DateTime Adquisicion = new DateTime(2026, 1, 15);
            DateTime Inicio = new DateTime(2026, 2, 1);
            decimal Compra = 5000m;
            decimal Residual = 500m;
            int Meses = 36;
            string Mensaje;

            // Act
            bool Resultado = _Controlador.ContabilidadFuncGrabarCambios(
                EstadoEntidadContabilidad.Agregar, ref IdActivo, Descripcion, Modulo, Tipo,
                Adquisicion, Inicio, Compra, Residual, Meses, out Mensaje);

            // Assert
            Assert.IsFalse(Resultado);
            Assert.AreEqual("Seleccione el módulo y el tipo de activo.", Mensaje);
        }
    }

    // Pruebas del enlace con Logística
    [TestClass]
    public class TestEnlaceLogisticaActivosFijos
    {
        [TestMethod]
        public void Test_BuscarReceptor_SinDllLogistica_Nulo()
        {
            // Arrange
            Type Contrato = typeof(IReceptorActivoFijoContabilidad);

            // Act
            IReceptorActivoFijoContabilidad Receptor = ClsEnlaceLogisticaContabilidad.ContabilidadFuncBuscarReceptor();

            // Assert
            Assert.IsTrue(Contrato.IsInterface);
            Assert.IsNull(Receptor);
        }
    }
}
// Fin de cambio: Gabriel Guillén - 0901-23-1998 - 09/11/2026
