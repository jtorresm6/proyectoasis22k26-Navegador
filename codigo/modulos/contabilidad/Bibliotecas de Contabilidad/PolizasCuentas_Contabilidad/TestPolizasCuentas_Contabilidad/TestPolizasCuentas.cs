/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Oskar Cermeño
 * Carné : 0901-23-15379
 * Fecha : 03/11/2026
 * ==================================================================
 * Propósito :
 *  Pruebas unitarias de Pólizas y cuentas contables, únicamente de la
 *  capa Modelo (entidades y repositorios) y de la capa Vista.
 * Reglas especificas:
 *  Las clases de la Vista son internal; se acceden por reflexión para
 *  no modificar el código de la capa. De los repositorios se prueba
 *  solo la lógica que se ejecuta antes de abrir la conexión ODBC.
 * ===================================================================
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Windows.Forms;
using CapaModelo_Contabilidad.Contratos;
using CapaModelo_Contabilidad.Entidades;
using CapaModelo_Contabilidad.Repositorios;

namespace TestPolizasCuentas_Contabilidad
{
    [TestClass]
    public class TestPolizasCuentas
    {
        // ===================== Utilidades de reflexión =====================

        private const BindingFlags _Todos = BindingFlags.Public | BindingFlags.NonPublic |
                                            BindingFlags.Instance | BindingFlags.Static;

        private static readonly Assembly _Vista = typeof(CapaVista_Contabilidad.FrmPolizas).Assembly;

        private static Type ContabilidadFuncTipo(string Nombre)
        {
            return _Vista.GetType(Nombre, true);
        }

        private static object ContabilidadFuncCrear(string Nombre)
        {
            return Activator.CreateInstance(ContabilidadFuncTipo(Nombre), true);
        }

        private static object ContabilidadFuncInvocarTipo(Type Tipo, object Instancia, string Metodo, params object[] Argumentos)
        {
            try
            {
                return Tipo.GetMethod(Metodo, _Todos).Invoke(Instancia, Argumentos);
            }
            catch (TargetInvocationException Error)
            {
                // Se relanza la excepción original para que ExpectedException la reconozca.
                ExceptionDispatchInfo.Capture(Error.InnerException).Throw();
                throw;
            }
        }

        private static object ContabilidadFuncInvocar(object Instancia, string Metodo, params object[] Argumentos)
        {
            return ContabilidadFuncInvocarTipo(Instancia.GetType(), Instancia, Metodo, Argumentos);
        }

        private static object ContabilidadFuncInvocarEstatico(Type Tipo, string Metodo, params object[] Argumentos)
        {
            return ContabilidadFuncInvocarTipo(Tipo, null, Metodo, Argumentos);
        }

        private static object ContabilidadFuncLeer(object Instancia, string Propiedad)
        {
            return Instancia.GetType().GetProperty(Propiedad, _Todos).GetValue(Instancia);
        }

        private static void ContabilidadMetEscribir(object Instancia, string Propiedad, object Valor)
        {
            Instancia.GetType().GetProperty(Propiedad, _Todos).SetValue(Instancia, Valor);
        }

        private static object ContabilidadFuncEstiloBoton(string Nombre)
        {
            return Enum.Parse(ContabilidadFuncTipo("CapaVista_Contabilidad.Componentes.EstiloBotonContabilidad"), Nombre);
        }

        // Formularios de prueba: ClsPresentacionContabilidad busca el código por el nombre del tipo.
        private class FrmPolizas : Form { }
        private class FrmDesconocido : Form { }

        // ===================== Capa Modelo: ClsCuentaContabilidad =====================

        [TestMethod]
        public void Test_Cuenta_ToStringDevuelveNombre()
        {
            //Arrange: inicializar las variables
            ClsCuentaContabilidad Cuenta = new ClsCuentaContabilidad { IdCuenta = 1, Codigo = "1.1.01", Nombre = "Caja general" };
            //Act: ejecucion del metodo a probar
            string Resultado = Cuenta.ToString();
            //Assert: comprobacion de los resultados
            Assert.AreEqual("Caja general", Resultado);
        }

        [TestMethod]
        public void Test_Cuenta_ValoresPorDefecto()
        {
            //Arrange: inicializar las variables
            //Act: ejecucion del metodo a probar
            ClsCuentaContabilidad Cuenta = new ClsCuentaContabilidad();
            //Assert: comprobacion de los resultados
            Assert.AreEqual(0, Cuenta.IdCuenta);
            Assert.AreEqual(0, Cuenta.IdPadre);
            Assert.IsFalse(Cuenta.Activo);
            Assert.IsNull(Cuenta.Codigo);
            Assert.IsNull(Cuenta.ToString());
        }

        [TestMethod]
        public void Test_Cuenta_AsignacionDePropiedades()
        {
            //Arrange: inicializar las variables
            ClsCuentaContabilidad Cuenta = new ClsCuentaContabilidad();
            //Act: ejecucion del metodo a probar
            Cuenta.IdCuenta = 15;
            Cuenta.Codigo = "1.1.02";
            Cuenta.Nombre = "Bancos";
            Cuenta.Tipo = "DEUDORA";
            Cuenta.IdPadre = 3;
            Cuenta.Activo = true;
            //Assert: comprobacion de los resultados
            Assert.AreEqual(15, Cuenta.IdCuenta);
            Assert.AreEqual("1.1.02", Cuenta.Codigo);
            Assert.AreEqual("Bancos", Cuenta.Nombre);
            Assert.AreEqual("DEUDORA", Cuenta.Tipo);
            Assert.AreEqual(3, Cuenta.IdPadre);
            Assert.IsTrue(Cuenta.Activo);
        }

        // ===================== Capa Modelo: ClsPolizaContabilidad =====================

        [TestMethod]
        public void Test_Poliza_ValoresPorDefecto()
        {
            //Arrange: inicializar las variables
            //Act: ejecucion del metodo a probar
            ClsPolizaContabilidad Poliza = new ClsPolizaContabilidad();
            //Assert: comprobacion de los resultados
            Assert.AreEqual("BORRADOR", Poliza.Estado);
            Assert.AreEqual(1, Poliza.IdModulo);
            Assert.AreEqual(0, Poliza.IdPais);
            Assert.IsNotNull(Poliza.Detalle);
            Assert.AreEqual(0, Poliza.Detalle.Count);
        }

        [TestMethod]
        public void Test_Poliza_DetalleIndependientePorInstancia()
        {
            //Arrange: inicializar las variables
            ClsPolizaContabilidad Poliza1 = new ClsPolizaContabilidad();
            ClsPolizaContabilidad Poliza2 = new ClsPolizaContabilidad();
            //Act: ejecucion del metodo a probar
            Poliza1.Detalle.Add(new ClsPolizaDetalleContabilidad { Linea = 1, Debe = 100m });
            //Assert: comprobacion de los resultados
            Assert.AreEqual(1, Poliza1.Detalle.Count);
            Assert.AreEqual(0, Poliza2.Detalle.Count);
            Assert.AreNotSame(Poliza1.Detalle, Poliza2.Detalle);
        }

        [TestMethod]
        public void Test_Poliza_PartidaCuadrada()
        {
            //Arrange: inicializar las variables
            ClsPolizaContabilidad Poliza = new ClsPolizaContabilidad
            {
                CodigoPoliza = "POL-0001",
                Fecha = new DateTime(2026, 11, 3),
                PeriodoContable = "2026-11",
                Descripcion = "Compra de mobiliario"
            };
            Poliza.Detalle.Add(new ClsPolizaDetalleContabilidad { Linea = 1, IdCuenta = 10, Debe = 1500.75m, Haber = 0m });
            Poliza.Detalle.Add(new ClsPolizaDetalleContabilidad { Linea = 2, IdCuenta = 20, Debe = 0m, Haber = 1000.50m });
            Poliza.Detalle.Add(new ClsPolizaDetalleContabilidad { Linea = 3, IdCuenta = 30, Debe = 0m, Haber = 500.25m });
            //Act: ejecucion del metodo a probar
            decimal TotalDebe = Poliza.Detalle.Sum(Linea => Linea.Debe);
            decimal TotalHaber = Poliza.Detalle.Sum(Linea => Linea.Haber);
            //Assert: comprobacion de los resultados
            Assert.AreEqual(1500.75m, TotalDebe);
            Assert.AreEqual(1500.75m, TotalHaber);
            Assert.AreEqual(TotalDebe, TotalHaber);
        }

        [TestMethod]
        public void Test_Poliza_PartidaDescuadrada()
        {
            //Arrange: inicializar las variables
            ClsPolizaContabilidad Poliza = new ClsPolizaContabilidad();
            Poliza.Detalle.Add(new ClsPolizaDetalleContabilidad { Linea = 1, Debe = 250m });
            Poliza.Detalle.Add(new ClsPolizaDetalleContabilidad { Linea = 2, Haber = 200m });
            //Act: ejecucion del metodo a probar
            decimal Diferencia = Poliza.Detalle.Sum(Linea => Linea.Debe) - Poliza.Detalle.Sum(Linea => Linea.Haber);
            //Assert: comprobacion de los resultados
            Assert.AreEqual(50m, Diferencia);
        }

        // ===================== Capa Modelo: ClsPolizaDetalleContabilidad =====================

        [TestMethod]
        public void Test_Detalle_ImportesDecimalesExactos()
        {
            //Arrange: inicializar las variables
            ClsPolizaDetalleContabilidad Linea1 = new ClsPolizaDetalleContabilidad { Debe = 0.1m };
            ClsPolizaDetalleContabilidad Linea2 = new ClsPolizaDetalleContabilidad { Debe = 0.2m };
            //Act: ejecucion del metodo a probar
            decimal Resultado = Linea1.Debe + Linea2.Debe;
            //Assert: comprobacion de los resultados (con double daría 0.30000000000000004)
            Assert.AreEqual(0.3m, Resultado);
            Assert.AreEqual(typeof(decimal), typeof(ClsPolizaDetalleContabilidad).GetProperty("Debe").PropertyType);
            Assert.AreEqual(typeof(decimal), typeof(ClsPolizaDetalleContabilidad).GetProperty("Haber").PropertyType);
        }

        [TestMethod]
        public void Test_Detalle_ValoresPorDefecto()
        {
            //Arrange: inicializar las variables
            //Act: ejecucion del metodo a probar
            ClsPolizaDetalleContabilidad Linea = new ClsPolizaDetalleContabilidad();
            //Assert: comprobacion de los resultados
            Assert.AreEqual(0m, Linea.Debe);
            Assert.AreEqual(0m, Linea.Haber);
            Assert.AreEqual(0, Linea.Linea);
            Assert.IsNull(Linea.DescripcionDetalle);
        }

        // ===================== Capa Modelo: ClsRepositorioPolizasContabilidad =====================
        // Las validaciones de ContabilidadFuncAgregar ocurren antes de abrir la conexión,
        // por eso se prueban sin base de datos.

        private static ClsPolizaContabilidad ContabilidadFuncPolizaConLineas(params decimal[][] Lineas)
        {
            ClsPolizaContabilidad Poliza = new ClsPolizaContabilidad { Fecha = new DateTime(2026, 11, 3), Descripcion = "Prueba" };
            int Numero = 1;
            foreach (decimal[] Linea in Lineas)
                Poliza.Detalle.Add(new ClsPolizaDetalleContabilidad { Linea = Numero, IdCuenta = Numero++, Debe = Linea[0], Haber = Linea[1] });
            return Poliza;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_RepositorioPolizas_AgregarSinDetalle()
        {
            //Arrange: inicializar las variables
            ClsRepositorioPolizasContabilidad Repositorio = new ClsRepositorioPolizasContabilidad();
            ClsPolizaContabilidad Poliza = new ClsPolizaContabilidad { Detalle = null };
            //Act: ejecucion del metodo a probar
            Repositorio.ContabilidadFuncAgregar(Poliza);
            //Assert: comprobacion de los resultados
            Assert.Fail("Se esperaba InvalidOperationException para una póliza sin detalle.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_RepositorioPolizas_AgregarConUnaSolaLinea()
        {
            //Arrange: inicializar las variables
            ClsRepositorioPolizasContabilidad Repositorio = new ClsRepositorioPolizasContabilidad();
            ClsPolizaContabilidad Poliza = ContabilidadFuncPolizaConLineas(new[] { 100m, 0m });
            //Act: ejecucion del metodo a probar
            Repositorio.ContabilidadFuncAgregar(Poliza);
            //Assert: comprobacion de los resultados
            Assert.Fail("Se esperaba InvalidOperationException para una póliza de una línea.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_RepositorioPolizas_AgregarDescuadrada()
        {
            //Arrange: inicializar las variables
            ClsRepositorioPolizasContabilidad Repositorio = new ClsRepositorioPolizasContabilidad();
            ClsPolizaContabilidad Poliza = ContabilidadFuncPolizaConLineas(new[] { 250m, 0m }, new[] { 0m, 200m });
            //Act: ejecucion del metodo a probar
            Repositorio.ContabilidadFuncAgregar(Poliza);
            //Assert: comprobacion de los resultados
            Assert.Fail("Se esperaba InvalidOperationException para una póliza descuadrada.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_RepositorioPolizas_AgregarConTotalCero()
        {
            //Arrange: inicializar las variables
            ClsRepositorioPolizasContabilidad Repositorio = new ClsRepositorioPolizasContabilidad();
            ClsPolizaContabilidad Poliza = ContabilidadFuncPolizaConLineas(new[] { 0m, 0m }, new[] { 0m, 0m });
            //Act: ejecucion del metodo a probar
            Repositorio.ContabilidadFuncAgregar(Poliza);
            //Assert: comprobacion de los resultados
            Assert.Fail("Se esperaba InvalidOperationException para una póliza con total cero.");
        }

        [TestMethod]
        public void Test_RepositorioPolizas_MensajeDePolizaDescuadrada()
        {
            //Arrange: inicializar las variables
            ClsRepositorioPolizasContabilidad Repositorio = new ClsRepositorioPolizasContabilidad();
            ClsPolizaContabilidad Poliza = ContabilidadFuncPolizaConLineas(new[] { 100m, 0m }, new[] { 0m, 99.99m });
            string Mensaje = null;
            //Act: ejecucion del metodo a probar
            try { Repositorio.ContabilidadFuncAgregar(Poliza); }
            catch (InvalidOperationException Error) { Mensaje = Error.Message; }
            //Assert: comprobacion de los resultados
            Assert.IsNotNull(Mensaje);
            StringAssert.Contains(Mensaje, "no cuadra");
            Assert.AreEqual(0, Poliza.IdPoliza);
            Assert.IsNull(Poliza.CodigoPoliza);
        }

        [TestMethod]
        public void Test_RepositorioPolizas_ObtenerEstados()
        {
            //Arrange: inicializar las variables
            ClsRepositorioPolizasContabilidad Repositorio = new ClsRepositorioPolizasContabilidad();
            //Act: ejecucion del metodo a probar
            string[] Estados = Repositorio.ContabilidadFuncObtenerEstados();
            //Assert: comprobacion de los resultados
            CollectionAssert.AreEqual(new[] { "BORRADOR", "VALIDADA", "ANULADA" }, Estados);
        }

        [TestMethod]
        public void Test_RepositorioPolizas_ImplementaContrato()
        {
            //Arrange: inicializar las variables
            //Act: ejecucion del metodo a probar
            object Repositorio = new ClsRepositorioPolizasContabilidad();
            //Assert: comprobacion de los resultados
            Assert.IsInstanceOfType(Repositorio, typeof(IRepositorioPolizasContabilidad));
            Assert.IsInstanceOfType(Repositorio, typeof(ClsRepositorioMaestroContabilidad));
        }

        // ===================== Capa Modelo: ClsRepositorioCuentasContabilidad =====================

        private static DataTable ContabilidadFuncTablaCuentas()
        {
            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("idCuenta", typeof(int));
            Tabla.Columns.Add("codigoCuenta", typeof(string));
            Tabla.Columns.Add("nombreCuenta", typeof(string));
            Tabla.Columns.Add("tipoCuenta", typeof(string));
            Tabla.Columns.Add("idCuentaPadre", typeof(int));
            Tabla.Columns.Add("is_active", typeof(int));
            return Tabla;
        }

        [TestMethod]
        public void Test_RepositorioCuentas_MapearFilas()
        {
            //Arrange: inicializar las variables
            DataTable Tabla = ContabilidadFuncTablaCuentas();
            Tabla.Rows.Add(1, "1", "Activo", "DEUDORA", DBNull.Value, 1);
            Tabla.Rows.Add(2, "1.1", "Activo corriente", "DEUDORA", 1, 0);
            //Act: ejecucion del metodo a probar
            List<ClsCuentaContabilidad> Cuentas = (List<ClsCuentaContabilidad>)ContabilidadFuncInvocarEstatico(
                typeof(ClsRepositorioCuentasContabilidad), "ContabilidadFuncMapear", Tabla);
            //Assert: comprobacion de los resultados
            Assert.AreEqual(2, Cuentas.Count);
            Assert.AreEqual(1, Cuentas[0].IdCuenta);
            Assert.AreEqual("1", Cuentas[0].Codigo);
            Assert.AreEqual("Activo", Cuentas[0].Nombre);
            Assert.AreEqual("DEUDORA", Cuentas[0].Tipo);
            Assert.AreEqual(0, Cuentas[0].IdPadre);
            Assert.IsTrue(Cuentas[0].Activo);
            Assert.AreEqual(1, Cuentas[1].IdPadre);
            Assert.IsFalse(Cuentas[1].Activo);
        }

        [TestMethod]
        public void Test_RepositorioCuentas_MapearTablaVacia()
        {
            //Arrange: inicializar las variables
            DataTable Tabla = ContabilidadFuncTablaCuentas();
            //Act: ejecucion del metodo a probar
            List<ClsCuentaContabilidad> Cuentas = (List<ClsCuentaContabilidad>)ContabilidadFuncInvocarEstatico(
                typeof(ClsRepositorioCuentasContabilidad), "ContabilidadFuncMapear", Tabla);
            //Assert: comprobacion de los resultados
            Assert.IsNotNull(Cuentas);
            Assert.AreEqual(0, Cuentas.Count);
        }

        [TestMethod]
        public void Test_RepositorioCuentas_ImplementaContrato()
        {
            //Arrange: inicializar las variables
            //Act: ejecucion del metodo a probar
            object Repositorio = new ClsRepositorioCuentasContabilidad();
            //Assert: comprobacion de los resultados
            Assert.IsInstanceOfType(Repositorio, typeof(IRepositorioCuentasContabilidad));
            Assert.IsInstanceOfType(Repositorio, typeof(ClsRepositorioMaestroContabilidad));
        }

        [TestMethod]
        public void Test_RepositorioIntegracion_HeredaRepositorioMaestro()
        {
            //Arrange: inicializar las variables
            //Act: ejecucion del metodo a probar
            object Repositorio = new ClsRepositorioIntegracionContabilidad();
            //Assert: comprobacion de los resultados
            Assert.IsInstanceOfType(Repositorio, typeof(ClsRepositorioMaestroContabilidad));
        }

        // ===================== Capa Vista: ClsTablaContabilidad =====================

        [TestMethod]
        public void Test_Tabla_ConfiguracionDeComportamiento()
        {
            //Arrange: inicializar las variables
            //Act: ejecucion del metodo a probar
            using (DataGridView Tabla = (DataGridView)ContabilidadFuncCrear("CapaVista_Contabilidad.Componentes.ClsTablaContabilidad"))
            {
                //Assert: comprobacion de los resultados
                Assert.IsFalse(Tabla.AllowUserToAddRows);
                Assert.IsFalse(Tabla.AllowUserToDeleteRows);
                Assert.IsFalse(Tabla.AllowUserToResizeRows);
                Assert.IsFalse(Tabla.RowHeadersVisible);
                Assert.IsFalse(Tabla.MultiSelect);
                Assert.AreEqual(DataGridViewSelectionMode.FullRowSelect, Tabla.SelectionMode);
                Assert.AreEqual(DataGridViewAutoSizeColumnsMode.Fill, Tabla.AutoSizeColumnsMode);
            }
        }

        [TestMethod]
        public void Test_Tabla_ColoresDelEstandar()
        {
            //Arrange: inicializar las variables
            //Act: ejecucion del metodo a probar
            using (DataGridView Tabla = (DataGridView)ContabilidadFuncCrear("CapaVista_Contabilidad.Componentes.ClsTablaContabilidad"))
            {
                //Assert: comprobacion de los resultados
                Assert.AreEqual(Color.FromArgb(35, 50, 61).ToArgb(), Tabla.ColumnHeadersDefaultCellStyle.BackColor.ToArgb());
                Assert.AreEqual(Color.White.ToArgb(), Tabla.ColumnHeadersDefaultCellStyle.ForeColor.ToArgb());
                Assert.AreEqual(Color.FromArgb(246, 247, 248).ToArgb(), Tabla.AlternatingRowsDefaultCellStyle.BackColor.ToArgb());
                Assert.AreEqual(Color.FromArgb(212, 222, 228).ToArgb(), Tabla.DefaultCellStyle.SelectionBackColor.ToArgb());
                Assert.AreEqual(38, Tabla.ColumnHeadersHeight);
                Assert.AreEqual(34, Tabla.RowTemplate.Height);
            }
        }

        // ===================== Capa Vista: ClsBotonContabilidad =====================

        [TestMethod]
        public void Test_Boton_ValoresPorDefecto()
        {
            //Arrange: inicializar las variables
            //Act: ejecucion del metodo a probar
            using (Button Boton = (Button)ContabilidadFuncCrear("CapaVista_Contabilidad.Componentes.ClsBotonContabilidad"))
            {
                //Assert: comprobacion de los resultados
                Assert.AreEqual(ContabilidadFuncEstiloBoton("Principal"), ContabilidadFuncLeer(Boton, "Estilo"));
                Assert.AreEqual(10, ContabilidadFuncLeer(Boton, "Radio"));
                Assert.AreEqual(new Size(150, 40), Boton.Size);
                Assert.AreEqual(FlatStyle.Flat, Boton.FlatStyle);
                Assert.AreEqual(0, Boton.FlatAppearance.BorderSize);
            }
        }

        [TestMethod]
        public void Test_Boton_RadioNegativoSeAjustaACero()
        {
            //Arrange: inicializar las variables
            using (Button Boton = (Button)ContabilidadFuncCrear("CapaVista_Contabilidad.Componentes.ClsBotonContabilidad"))
            {
                //Act: ejecucion del metodo a probar
                ContabilidadMetEscribir(Boton, "Radio", -8);
                //Assert: comprobacion de los resultados
                Assert.AreEqual(0, ContabilidadFuncLeer(Boton, "Radio"));
            }
        }

        [TestMethod]
        public void Test_Boton_ColoresSegunEstilo()
        {
            //Arrange: inicializar las variables
            using (Button Boton = (Button)ContabilidadFuncCrear("CapaVista_Contabilidad.Componentes.ClsBotonContabilidad"))
            {
                //Act: ejecucion del metodo a probar
                Color RellenoPrincipal = (Color)ContabilidadFuncInvocar(Boton, "ContabilidadFuncColorRelleno");
                Color TextoPrincipal = (Color)ContabilidadFuncInvocar(Boton, "ContabilidadFuncColorTexto");
                ContabilidadMetEscribir(Boton, "Estilo", ContabilidadFuncEstiloBoton("Secundario"));
                Color RellenoSecundario = (Color)ContabilidadFuncInvocar(Boton, "ContabilidadFuncColorRelleno");
                Color TextoSecundario = (Color)ContabilidadFuncInvocar(Boton, "ContabilidadFuncColorTexto");
                ContabilidadMetEscribir(Boton, "Estilo", ContabilidadFuncEstiloBoton("Dorado"));
                Color RellenoDorado = (Color)ContabilidadFuncInvocar(Boton, "ContabilidadFuncColorRelleno");
                //Assert: comprobacion de los resultados
                Assert.AreEqual(Color.FromArgb(35, 50, 61).ToArgb(), RellenoPrincipal.ToArgb());
                Assert.AreEqual(Color.White.ToArgb(), TextoPrincipal.ToArgb());
                Assert.AreEqual(Color.FromArgb(212, 222, 228).ToArgb(), RellenoSecundario.ToArgb());
                Assert.AreEqual(Color.FromArgb(35, 50, 61).ToArgb(), TextoSecundario.ToArgb());
                Assert.AreEqual(Color.FromArgb(197, 143, 62).ToArgb(), RellenoDorado.ToArgb());
            }
        }

        [TestMethod]
        public void Test_Boton_ColoresDeshabilitado()
        {
            //Arrange: inicializar las variables
            using (Button Boton = (Button)ContabilidadFuncCrear("CapaVista_Contabilidad.Componentes.ClsBotonContabilidad"))
            {
                //Act: ejecucion del metodo a probar
                Boton.Enabled = false;
                Color Relleno = (Color)ContabilidadFuncInvocar(Boton, "ContabilidadFuncColorRelleno");
                Color Texto = (Color)ContabilidadFuncInvocar(Boton, "ContabilidadFuncColorTexto");
                //Assert: comprobacion de los resultados
                Assert.AreEqual(Color.FromArgb(230, 235, 239).ToArgb(), Relleno.ToArgb());
                Assert.AreEqual(Color.FromArgb(154, 169, 180).ToArgb(), Texto.ToArgb());
            }
        }

        // ===================== Capa Vista: ClsPanelRedondeadoContabilidad =====================

        [TestMethod]
        public void Test_Panel_ValoresPorDefecto()
        {
            //Arrange: inicializar las variables
            //Act: ejecucion del metodo a probar
            using (Panel Contenedor = (Panel)ContabilidadFuncCrear("CapaVista_Contabilidad.Componentes.ClsPanelRedondeadoContabilidad"))
            {
                //Assert: comprobacion de los resultados
                Assert.AreEqual(18, ContabilidadFuncLeer(Contenedor, "Radio"));
                Assert.AreEqual(Color.FromArgb(212, 222, 228).ToArgb(), ((Color)ContabilidadFuncLeer(Contenedor, "ColorBorde")).ToArgb());
                Assert.AreEqual(Color.White.ToArgb(), ((Color)ContabilidadFuncLeer(Contenedor, "ColorRelleno")).ToArgb());
                Assert.AreEqual(Color.Transparent, Contenedor.BackColor);
            }
        }

        [TestMethod]
        public void Test_Panel_RadioNegativoSeAjustaACero()
        {
            //Arrange: inicializar las variables
            using (Panel Contenedor = (Panel)ContabilidadFuncCrear("CapaVista_Contabilidad.Componentes.ClsPanelRedondeadoContabilidad"))
            {
                //Act: ejecucion del metodo a probar
                ContabilidadMetEscribir(Contenedor, "Radio", -1);
                //Assert: comprobacion de los resultados
                Assert.AreEqual(0, ContabilidadFuncLeer(Contenedor, "Radio"));
            }
        }

        // ===================== Capa Vista: ClsValidacionContabilidad =====================

        private static Panel ContabilidadFuncFormularioValidacion(out TextBox Campo)
        {
            // Panel sin padre: su visibilidad no depende de un formulario mostrado.
            Panel Contenedor = new Panel { Size = new Size(400, 300) };
            Label Etiqueta = new Label { Text = "Código", Location = new Point(10, 10), Size = new Size(80, 20) };
            Campo = new TextBox { Name = "TxtCodigo", Location = new Point(10, 40), Width = 200 };
            Contenedor.Controls.Add(Etiqueta);
            Contenedor.Controls.Add(Campo);
            return Contenedor;
        }

        [TestMethod]
        public void Test_Validacion_CampoSinMensajeEsValido()
        {
            //Arrange: inicializar las variables
            TextBox Campo;
            using (Panel Contenedor = ContabilidadFuncFormularioValidacion(out Campo))
            using (IDisposable Validacion = (IDisposable)ContabilidadFuncCrear("CapaVista_Contabilidad.Componentes.ClsValidacionContabilidad"))
            {
                //Act: ejecucion del metodo a probar
                bool Resultado = (bool)ContabilidadFuncInvocar(Validacion, "ContabilidadFuncValidar", Campo, null);
                //Assert: comprobacion de los resultados
                Assert.IsTrue(Resultado);
                Assert.IsFalse(Contenedor.Controls["ContabilidadLblErrorTxtCodigo"].Visible);
            }
        }

        [TestMethod]
        public void Test_Validacion_CampoConMensajeMuestraError()
        {
            //Arrange: inicializar las variables
            TextBox Campo;
            using (Panel Contenedor = ContabilidadFuncFormularioValidacion(out Campo))
            using (IDisposable Validacion = (IDisposable)ContabilidadFuncCrear("CapaVista_Contabilidad.Componentes.ClsValidacionContabilidad"))
            {
                //Act: ejecucion del metodo a probar
                bool Resultado = (bool)ContabilidadFuncInvocar(Validacion, "ContabilidadFuncValidar", Campo, "El código es obligatorio");
                //Assert: comprobacion de los resultados
                Control Error = Contenedor.Controls["ContabilidadLblErrorTxtCodigo"];
                Assert.IsFalse(Resultado);
                Assert.IsTrue(Error.Visible);
                Assert.AreEqual("El código es obligatorio", Error.Text);
                for (int Indice = 0; Indice < 4; Indice++)
                    Assert.IsTrue(Contenedor.Controls["ContabilidadLblBordeTxtCodigo" + Indice].Visible);
            }
        }

        [TestMethod]
        public void Test_Validacion_CorregirCampoOcultaError()
        {
            //Arrange: inicializar las variables
            TextBox Campo;
            using (Panel Contenedor = ContabilidadFuncFormularioValidacion(out Campo))
            using (IDisposable Validacion = (IDisposable)ContabilidadFuncCrear("CapaVista_Contabilidad.Componentes.ClsValidacionContabilidad"))
            {
                ContabilidadFuncInvocar(Validacion, "ContabilidadFuncValidar", Campo, "El código es obligatorio");
                //Act: ejecucion del metodo a probar
                Campo.Text = "1.1.01";
                //Assert: comprobacion de los resultados
                Assert.IsFalse(Contenedor.Controls["ContabilidadLblErrorTxtCodigo"].Visible);
                Assert.IsFalse(Contenedor.Controls["ContabilidadLblBordeTxtCodigo0"].Visible);
            }
        }

        [TestMethod]
        public void Test_Validacion_LimpiarOcultaTodosLosErrores()
        {
            //Arrange: inicializar las variables
            TextBox Campo;
            using (Panel Contenedor = ContabilidadFuncFormularioValidacion(out Campo))
            using (IDisposable Validacion = (IDisposable)ContabilidadFuncCrear("CapaVista_Contabilidad.Componentes.ClsValidacionContabilidad"))
            {
                ContabilidadFuncInvocar(Validacion, "ContabilidadFuncValidar", Campo, "El código es obligatorio");
                //Act: ejecucion del metodo a probar
                ContabilidadFuncInvocar(Validacion, "ContabilidadMetLimpiar");
                //Assert: comprobacion de los resultados
                Assert.IsFalse(Contenedor.Controls["ContabilidadLblErrorTxtCodigo"].Visible);
                Assert.IsFalse(Contenedor.Controls["ContabilidadLblBordeTxtCodigo3"].Visible);
            }
        }

        [TestMethod]
        public void Test_Validacion_ObligatorioAgregaAsteriscoUnaSolaVez()
        {
            //Arrange: inicializar las variables
            TextBox Campo;
            using (Panel Contenedor = ContabilidadFuncFormularioValidacion(out Campo))
            using (IDisposable Validacion = (IDisposable)ContabilidadFuncCrear("CapaVista_Contabilidad.Componentes.ClsValidacionContabilidad"))
            {
                //Act: ejecucion del metodo a probar
                ContabilidadFuncInvocar(Validacion, "ContabilidadMetObligatorio", Campo);
                ContabilidadFuncInvocar(Validacion, "ContabilidadMetObligatorio", Campo);
                //Assert: comprobacion de los resultados
                Control[] Asteriscos = Contenedor.Controls.Find("ContabilidadLblObligatorioTxtCodigo", false);
                Assert.AreEqual(1, Asteriscos.Length);
                Assert.AreEqual("*", Asteriscos[0].Text);
            }
        }

        [TestMethod]
        public void Test_Validacion_ObligatorioSinEtiquetaNoAgregaAsterisco()
        {
            //Arrange: inicializar las variables
            using (Panel Contenedor = new Panel { Size = new Size(400, 300) })
            using (IDisposable Validacion = (IDisposable)ContabilidadFuncCrear("CapaVista_Contabilidad.Componentes.ClsValidacionContabilidad"))
            {
                TextBox Campo = new TextBox { Name = "TxtNombre", Location = new Point(10, 40) };
                Contenedor.Controls.Add(Campo);
                //Act: ejecucion del metodo a probar
                ContabilidadFuncInvocar(Validacion, "ContabilidadMetObligatorio", Campo);
                //Assert: comprobacion de los resultados
                Assert.IsFalse(Contenedor.Controls.ContainsKey("ContabilidadLblObligatorioTxtNombre"));
                Assert.AreEqual(1, Contenedor.Controls.Count);
            }
        }

        // ===================== Capa Vista: ClsPermisosMenuContabilidad =====================

        [TestMethod]
        public void Test_Permisos_SinAccesoDeshabilitaYPoneGris()
        {
            //Arrange: inicializar las variables
            Bitmap Original = new Bitmap(4, 4);
            Original.SetPixel(0, 0, Color.Red);
            using (Button Boton = new Button { BackgroundImage = Original })
            using (IDisposable Permisos = (IDisposable)ContabilidadFuncCrear("CapaVista_Contabilidad.ClsPermisosMenuContabilidad"))
            {
                //Act: ejecucion del metodo a probar
                ContabilidadFuncInvocar(Permisos, "ContabilidadMetAplicar", Boton, false);
                //Assert: comprobacion de los resultados
                Bitmap Gris = (Bitmap)Boton.BackgroundImage;
                Color Pixel = Gris.GetPixel(0, 0);
                Assert.IsFalse(Boton.Enabled);
                Assert.AreNotSame(Original, Gris);
                Assert.AreEqual(Pixel.R, Pixel.G);
                Assert.AreEqual(Pixel.G, Pixel.B);
            }
        }

        [TestMethod]
        public void Test_Permisos_ConAccesoRestauraOriginal()
        {
            //Arrange: inicializar las variables
            Bitmap Original = new Bitmap(4, 4);
            using (Button Boton = new Button { BackgroundImage = Original })
            using (IDisposable Permisos = (IDisposable)ContabilidadFuncCrear("CapaVista_Contabilidad.ClsPermisosMenuContabilidad"))
            {
                ContabilidadFuncInvocar(Permisos, "ContabilidadMetAplicar", Boton, false);
                //Act: ejecucion del metodo a probar
                ContabilidadFuncInvocar(Permisos, "ContabilidadMetAplicar", Boton, true);
                //Assert: comprobacion de los resultados
                Assert.IsTrue(Boton.Enabled);
                Assert.AreSame(Original, Boton.BackgroundImage);
            }
        }

        [TestMethod]
        public void Test_Permisos_DisposeRestauraImagenOriginal()
        {
            //Arrange: inicializar las variables
            Bitmap Original = new Bitmap(4, 4);
            using (Button Boton = new Button { BackgroundImage = Original })
            {
                IDisposable Permisos = (IDisposable)ContabilidadFuncCrear("CapaVista_Contabilidad.ClsPermisosMenuContabilidad");
                ContabilidadFuncInvocar(Permisos, "ContabilidadMetAplicar", Boton, false);
                //Act: ejecucion del metodo a probar
                Permisos.Dispose();
                //Assert: comprobacion de los resultados
                Assert.AreSame(Original, Boton.BackgroundImage);
            }
        }

        [TestMethod]
        public void Test_Permisos_SinImagenSoloDeshabilita()
        {
            //Arrange: inicializar las variables
            using (Button Boton = new Button())
            using (IDisposable Permisos = (IDisposable)ContabilidadFuncCrear("CapaVista_Contabilidad.ClsPermisosMenuContabilidad"))
            {
                //Act: ejecucion del metodo a probar
                ContabilidadFuncInvocar(Permisos, "ContabilidadMetAplicar", Boton, false);
                //Assert: comprobacion de los resultados
                Assert.IsFalse(Boton.Enabled);
                Assert.IsNull(Boton.BackgroundImage);
            }
        }

        [TestMethod]
        public void Test_Permisos_RestringirImpideRehabilitar()
        {
            //Arrange: inicializar las variables
            Type Tipo = ContabilidadFuncTipo("CapaVista_Contabilidad.ClsPermisosMenuContabilidad");
            using (Button Boton = new Button())
            {
                //Act: ejecucion del metodo a probar
                ContabilidadFuncInvocarEstatico(Tipo, "ContabilidadMetRestringir", Boton, false);
                Boton.Enabled = true;
                //Assert: comprobacion de los resultados
                Assert.IsFalse(Boton.Enabled);
            }
        }

        [TestMethod]
        public void Test_Permisos_RestringirPermitidoNoCambiaBoton()
        {
            //Arrange: inicializar las variables
            Type Tipo = ContabilidadFuncTipo("CapaVista_Contabilidad.ClsPermisosMenuContabilidad");
            using (Button Boton = new Button())
            {
                //Act: ejecucion del metodo a probar
                ContabilidadFuncInvocarEstatico(Tipo, "ContabilidadMetRestringir", Boton, true);
                //Assert: comprobacion de los resultados
                Assert.IsTrue(Boton.Enabled);
            }
        }

        // ===================== Capa Vista: ClsPresentacionContabilidad =====================

        [TestMethod]
        public void Test_Presentacion_PrepararVentanaPolizas()
        {
            //Arrange: inicializar las variables
            Type Tipo = ContabilidadFuncTipo("CapaVista_Contabilidad.ClsPresentacionContabilidad");
            using (FrmPolizas Formulario = new FrmPolizas())
            {
                //Act: ejecucion del metodo a probar
                ContabilidadFuncInvocarEstatico(Tipo, "ContabilidadMetPrepararVentana", Formulario, "Pólizas", new Size(900, 600));
                //Assert: comprobacion de los resultados
                Assert.AreEqual("10001 - Polizas", Formulario.Text);
                Assert.AreEqual(new Size(900, 600), Formulario.ClientSize);
                Assert.IsFalse(Formulario.MaximizeBox);
                Assert.IsTrue(Formulario.MinimizeBox);
                Assert.AreEqual(FormBorderStyle.FixedSingle, Formulario.FormBorderStyle);
                Assert.AreEqual(FormStartPosition.CenterParent, Formulario.StartPosition);
                Assert.AreEqual(Color.FromArgb(246, 247, 248).ToArgb(), Formulario.BackColor.ToArgb());
                Assert.IsNotNull(Formulario.Icon);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Test_Presentacion_VentanaSinCodigo()
        {
            //Arrange: inicializar las variables
            Type Tipo = ContabilidadFuncTipo("CapaVista_Contabilidad.ClsPresentacionContabilidad");
            using (FrmDesconocido Formulario = new FrmDesconocido())
            {
                //Act: ejecucion del metodo a probar
                ContabilidadFuncInvocarEstatico(Tipo, "ContabilidadMetPrepararVentana", Formulario, "Desconocido", new Size(400, 300));
                //Assert: comprobacion de los resultados
                Assert.Fail("Se esperaba KeyNotFoundException para una ventana sin código.");
            }
        }

        [TestMethod]
        public void Test_Presentacion_AplicarTipografia()
        {
            //Arrange: inicializar las variables
            Type Tipo = ContabilidadFuncTipo("CapaVista_Contabilidad.ClsPresentacionContabilidad");
            using (Panel Contenedor = new Panel())
            {
                Label Etiqueta = new Label { Font = new Font("Arial", 11F, FontStyle.Bold) };
                Label Titulo = new Label { Font = new Font("Lucida Fax", 16F) };
                TextBox Campo = new TextBox { Font = new Font("Arial", 9F, FontStyle.Italic) };
                Panel Interno = new Panel();
                Label EtiquetaInterna = new Label { Font = new Font("Arial", 10F) };
                Interno.Controls.Add(EtiquetaInterna);
                Contenedor.Controls.AddRange(new Control[] { Etiqueta, Titulo, Campo, Interno });
                //Act: ejecucion del metodo a probar
                ContabilidadFuncInvocarEstatico(Tipo, "ContabilidadMetAplicarTipografia", Contenedor);
                //Assert: comprobacion de los resultados
                Assert.AreEqual("Tahoma", Etiqueta.Font.FontFamily.Name);
                Assert.AreEqual(11F, Etiqueta.Font.Size);
                Assert.IsTrue(Etiqueta.Font.Bold);
                Assert.IsTrue(Titulo.Font.FontFamily.Name.StartsWith("Lucida"));
                Assert.AreEqual("Segoe UI", Campo.Font.FontFamily.Name);
                Assert.IsFalse(Campo.Font.Italic);
                Assert.AreEqual("Tahoma", EtiquetaInterna.Font.FontFamily.Name);
            }
        }
    }
}
