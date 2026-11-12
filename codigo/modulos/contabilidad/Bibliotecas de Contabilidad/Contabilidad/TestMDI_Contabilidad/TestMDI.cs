/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 12/11/2026
 * ==================================================================
 * Propósito :
 *  Pruebas unitarias MSTest del MDI de Contabilidad: los seis
 *  indicadores, el registro de estados financieros emitidos y los
 *  submenús Gestionar y Reporte.
 * Reglas especificas:
 *  Cada prueba sigue Arrange, Act y Assert. Los indicadores se
 *  comparan con la base por el DSN EmbutidosS.A; el registro de
 *  emisiones borra al final lo que insertó. Los métodos privados se
 *  prueban por reflexión.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_Contabilidad;
using CapaVista_Contabilidad;

namespace TestMDI
{
    [TestClass]
    public class TestMDI
    {
        // tipo con el que se marcan las emisiones de prueba
        private const string _TipoPrueba = "Prueba MSTest";
        private const BindingFlags _Privado = BindingFlags.NonPublic | BindingFlags.Instance;
        private const BindingFlags _PrivadoEstatico = BindingFlags.NonPublic | BindingFlags.Static;

        // un conteo directo en la base
        private static int ContabilidadFuncContar(string Consulta)
        {
            using (OdbcConnection Conexion = new OdbcConnection("Dsn=EmbutidosS.A"))
            using (OdbcCommand Comando = new OdbcCommand(Consulta, Conexion))
            {
                Conexion.Open();
                return Convert.ToInt32(Comando.ExecuteScalar());
            }
        }

        // borra las emisiones de prueba
        private static void ContabilidadMetLimpiarEmisiones()
        {
            using (OdbcConnection Conexion = new OdbcConnection("Dsn=EmbutidosS.A"))
            using (OdbcCommand Comando = new OdbcCommand("DELETE FROM tblEstadoFinancieroEmitido WHERE tipoEstadoFinancieroEmitido = ?", Conexion))
            {
                Comando.Parameters.Add(new OdbcParameter("Tipo", _TipoPrueba));
                Conexion.Open();
                Comando.ExecuteNonQuery();
            }
        }

        // un campo privado del MDI
        private static T ContabilidadFuncCampo<T>(FrmMDIContabilidad Mdi, string Nombre)
        {
            return (T)typeof(FrmMDIContabilidad).GetField(Nombre, _Privado).GetValue(Mdi);
        }

        // abre o cierra un submenú del MDI
        private static void ContabilidadMetAlternar(FrmMDIContabilidad Mdi, Button Boton)
        {
            typeof(FrmMDIContabilidad).GetMethod("ContabilidadMetAlternarSubmenu", _Privado).Invoke(Mdi, new object[] { Boton, EventArgs.Empty });
        }

        [TestMethod]
        public void Test_IndicadoresIgualesALaBase()
        {
            // Arrange
            ClsControladorIndicadoresContabilidad Controlador = new ClsControladorIndicadoresContabilidad();
            // Act
            ClsModeloIndicadoresContabilidad Indicadores = Controlador.ContabilidadFuncObtener();
            // Assert
            Assert.AreEqual(ContabilidadFuncContar("SELECT COUNT(*) FROM tblPolizaEncabezado WHERE is_active = TRUE AND estadoPolizaEncabezado <> 'ANULADA'"), Indicadores.Polizas);
            Assert.AreEqual(ContabilidadFuncContar("SELECT COUNT(*) FROM tblActivoFijo WHERE is_active = TRUE"), Indicadores.ActivosFijos);
            Assert.AreEqual(ContabilidadFuncContar("SELECT COUNT(*) FROM tblCuenta WHERE is_active = TRUE"), Indicadores.Cuentas);
            Assert.AreEqual(ContabilidadFuncContar("SELECT COUNT(*) FROM tblEstadoFinancieroEmitido WHERE is_active = TRUE"), Indicadores.EstadosEmitidos);
        }

        [TestMethod]
        public void Test_IndicadoresSeLeenTodos()
        {
            // Arrange
            ClsControladorIndicadoresContabilidad Controlador = new ClsControladorIndicadoresContabilidad();
            // Act
            ClsModeloIndicadoresContabilidad Indicadores = Controlador.ContabilidadFuncObtener();
            // Assert
            Assert.IsTrue(Indicadores.Polizas.HasValue && Indicadores.PresupuestosVigentes.HasValue && Indicadores.ActivosFijos.HasValue);
            Assert.IsTrue(Indicadores.PeriodosCerrados.HasValue && Indicadores.EstadosEmitidos.HasValue && Indicadores.Cuentas.HasValue);
            Assert.IsTrue(Indicadores.PresupuestosVigentes >= 0 && Indicadores.PeriodosCerrados >= 0);
        }

        [TestMethod]
        public void Test_IndicadorQueFallaQuedaNulo()
        {
            // Arrange
            MethodInfo Leer = typeof(ClsControladorIndicadoresContabilidad).GetMethod("ContabilidadFuncLeer", _PrivadoEstatico);
            Func<int> Falla = () => { throw new InvalidOperationException("sin base"); };
            Func<int> Bien = () => 7;
            // Act
            object ConFalla = Leer.Invoke(null, new object[] { Falla });
            object SinFalla = Leer.Invoke(null, new object[] { Bien });
            // Assert
            Assert.IsNull(ConFalla);
            Assert.AreEqual(7, SinFalla);
        }

        [TestMethod]
        public void Test_RegistrarEmisionSumaUnEstado()
        {
            // Arrange
            ClsControladorIndicadoresContabilidad Indicadores = new ClsControladorIndicadoresContabilidad();
            ClsControladorEstadosFinancierosContabilidad EstadosFinancieros = new ClsControladorEstadosFinancierosContabilidad();
            int Antes = Indicadores.ContabilidadFuncObtener().EstadosEmitidos.Value;
            try
            {
                // Act
                bool Registrado = EstadosFinancieros.ContabilidadFuncRegistrarEmision(_TipoPrueba, new DateTime(2026, 10, 1), new DateTime(2026, 10, 31), 1, "GTQ");
                int Despues = Indicadores.ContabilidadFuncObtener().EstadosEmitidos.Value;
                // Assert
                Assert.IsTrue(Registrado);
                Assert.AreEqual(Antes + 1, Despues);
            }
            finally
            {
                ContabilidadMetLimpiarEmisiones();
            }
        }

        [TestMethod]
        public void Test_EmisionDeTodosLosPaisesSinPais()
        {
            // Arrange
            ClsControladorEstadosFinancierosContabilidad EstadosFinancieros = new ClsControladorEstadosFinancierosContabilidad();
            try
            {
                // Act
                EstadosFinancieros.ContabilidadFuncRegistrarEmision(_TipoPrueba, new DateTime(2026, 1, 1), new DateTime(2026, 10, 31), 0, "USD");
                int SinPais = ContabilidadFuncContar("SELECT COUNT(*) FROM tblEstadoFinancieroEmitido WHERE tipoEstadoFinancieroEmitido = '" + _TipoPrueba + "' AND idPais IS NULL");
                // Assert
                Assert.AreEqual(1, SinPais);
            }
            finally
            {
                ContabilidadMetLimpiarEmisiones();
            }
        }

        [TestMethod]
        public void Test_EmisionConFechasInvertidasNoSeRegistra()
        {
            // Arrange
            ClsControladorEstadosFinancierosContabilidad EstadosFinancieros = new ClsControladorEstadosFinancierosContabilidad();
            try
            {
                // Act
                bool Registrado = EstadosFinancieros.ContabilidadFuncRegistrarEmision(_TipoPrueba, new DateTime(2026, 10, 31), new DateTime(2026, 10, 1), 1, "GTQ");
                // Assert
                Assert.IsFalse(Registrado);
            }
            finally
            {
                ContabilidadMetLimpiarEmisiones();
            }
        }

        [TestMethod]
        public void Test_TextoDelIndicador()
        {
            // Arrange
            MethodInfo Texto = typeof(FrmMDIContabilidad).GetMethod("ContabilidadFuncTextoIndicador", _PrivadoEstatico);
            // Act
            string ConValor = (string)Texto.Invoke(null, new object[] { (int?)1234 });
            string SinValor = (string)Texto.Invoke(null, new object[] { null });
            // Assert
            Assert.AreEqual(1234.ToString("N0"), ConValor);
            Assert.AreEqual("—", SinValor);
        }

        [TestMethod]
        public void Test_CuatroModulosConGestionarYReporte()
        {
            // Arrange
            using (FrmMDIContabilidad Mdi = new FrmMDIContabilidad())
            {
                Dictionary<Button, Button[]> Submenus = ContabilidadFuncCampo<Dictionary<Button, Button[]>>(Mdi, "_Submenus");
                string[] Modulos = { "ContabilidadBtnPolizasLocales", "ContabilidadBtnPresupuestos", "ContabilidadBtnActivosFijos", "ContabilidadBtnCierreContable" };
                // Act
                Button EstadosFinancieros = ContabilidadFuncCampo<Button>(Mdi, "ContabilidadBtnEstadosFinancieros");
                // Assert
                foreach (string Nombre in Modulos)
                {
                    Button[] Hijos = Submenus[ContabilidadFuncCampo<Button>(Mdi, Nombre)];
                    Assert.AreEqual(2, Hijos.Length, Nombre);
                    Assert.AreEqual("Gestionar", Hijos[0].AccessibleName, Nombre);
                    Assert.AreEqual("Reporte", Hijos[1].AccessibleName, Nombre);
                }
                Assert.IsFalse(Submenus.ContainsKey(EstadosFinancieros));
            }
        }

        [TestMethod]
        public void Test_SoloUnSubmenuAbierto()
        {
            // Arrange
            using (FrmMDIContabilidad Mdi = new FrmMDIContabilidad())
            {
                Button Polizas = ContabilidadFuncCampo<Button>(Mdi, "ContabilidadBtnPolizasLocales");
                Button Cierre = ContabilidadFuncCampo<Button>(Mdi, "ContabilidadBtnCierreContable");
                // Act
                ContabilidadMetAlternar(Mdi, Polizas);
                Button TrasPolizas = ContabilidadFuncCampo<Button>(Mdi, "_SubmenuAbierto");
                ContabilidadMetAlternar(Mdi, Cierre);
                Button TrasCierre = ContabilidadFuncCampo<Button>(Mdi, "_SubmenuAbierto");
                ContabilidadMetAlternar(Mdi, Cierre);
                Button TrasCerrar = ContabilidadFuncCampo<Button>(Mdi, "_SubmenuAbierto");
                // Assert
                Assert.AreSame(Polizas, TrasPolizas);
                Assert.AreSame(Cierre, TrasCierre);
                Assert.IsNull(TrasCerrar);
            }
        }

        [TestMethod]
        public void Test_SubmenuAbiertoEmpujaLosDeAbajo()
        {
            // Arrange
            using (FrmMDIContabilidad Mdi = new FrmMDIContabilidad())
            {
                Button Polizas = ContabilidadFuncCampo<Button>(Mdi, "ContabilidadBtnPolizasLocales");
                Button Presupuestos = ContabilidadFuncCampo<Button>(Mdi, "ContabilidadBtnPresupuestos");
                Button Gestionar = ContabilidadFuncCampo<Button>(Mdi, "ContabilidadBtnPolizasLocalesGestion");
                Button Reporte = ContabilidadFuncCampo<Button>(Mdi, "ContabilidadBtnPolizasLocalesReporte");
                int Cerrado = Presupuestos.Top;
                // Act
                ContabilidadMetAlternar(Mdi, Polizas);
                int Abierto = Presupuestos.Top;
                // Assert
                Assert.IsTrue(Gestionar.Top > Polizas.Top && Reporte.Top > Gestionar.Top);
                Assert.IsTrue(Abierto > Reporte.Top);
                Assert.AreEqual(Cerrado + Gestionar.Height + Reporte.Height + 14, Abierto);
            }
        }

        [TestMethod]
        public void Test_PermisoDelModuloAplicaASusOpciones()
        {
            // Arrange
            using (FrmMDIContabilidad Mdi = new FrmMDIContabilidad())
            {
                Button Presupuestos = ContabilidadFuncCampo<Button>(Mdi, "ContabilidadBtnPresupuestos");
                Button[] Hijos = ContabilidadFuncCampo<Dictionary<Button, Button[]>>(Mdi, "_Submenus")[Presupuestos];
                MethodInfo Aplicar = typeof(FrmMDIContabilidad).GetMethod("ContabilidadMetAplicarPermiso", _Privado);
                // Act
                Aplicar.Invoke(Mdi, new object[] { Presupuestos, true });
                bool ConPermiso = Hijos[0].Enabled && Hijos[1].Enabled;
                Aplicar.Invoke(Mdi, new object[] { Presupuestos, false });
                bool SinPermiso = Hijos[0].Enabled || Hijos[1].Enabled;
                // Assert
                Assert.IsTrue(ConPermiso);
                Assert.IsFalse(SinPermiso);
            }
        }
    }
}
