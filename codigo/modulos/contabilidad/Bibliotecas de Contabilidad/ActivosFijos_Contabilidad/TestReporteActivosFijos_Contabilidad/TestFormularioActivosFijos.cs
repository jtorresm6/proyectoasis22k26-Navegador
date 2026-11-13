/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 12/11/2026
 * ==================================================================
 * Propósito :
 *  Pruebas de la pantalla de activos fijos: un valor numérico borrado
 *  no debe dejar guardar.
 * Reglas especificas:
 *  Cada prueba sigue Arrange, Act y Assert. La pantalla lee los
 *  catálogos de la base local pero no graba: el módulo queda sin
 *  elegir, así que aunque la validación fallara no se insertaría nada.
 * ===================================================================
*/

using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaVista_Contabilidad;

namespace TestReporteActivosFijos
{
    // Pruebas de la pantalla de activos fijos
    [TestClass]
    public class TestFormularioActivosFijos
    {
        // Ejecuta en un hilo STA con tiempo límite
        private static void ContabilidadMetEnSta(Action Accion)
        {
            Exception? Error = null;
            Thread Hilo = new Thread(() =>
            {
                try { Accion(); }
                catch (Exception Excepcion) { Error = Excepcion; }
            });
            Hilo.IsBackground = true;
            Hilo.SetApartmentState(ApartmentState.STA);
            Hilo.Start();
            if (!Hilo.Join(30000)) throw new AssertFailedException("La pantalla abrió un diálogo: intentó guardar.");
            if (Error != null) throw new AssertFailedException(Error.ToString());
        }

        // Busca un control por nombre
        private static T ContabilidadFuncControl<T>(Form Formulario, string Nombre) where T : Control
        {
            return (T)Formulario.Controls.Find(Nombre, true)[0];
        }

        // Borra el texto como lo haría el usuario
        private static void ContabilidadMetBorrar(NumericUpDown Campo)
        {
            typeof(UpDownBase).GetProperty("UserEdit", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(Campo, true);
            Campo.Controls[1].Text = string.Empty;
        }

        // Prepara la ficha con un campo borrado y pulsa guardar
        private static string ContabilidadFuncGuardarConCampoBorrado(string NombreCampo, decimal ValorAnterior)
        {
            string Mensaje = null!;
            ContabilidadMetEnSta(() =>
            {
                using (FrmActivosFijos Formulario = new FrmActivosFijos())
                {
                    Formulario.StartPosition = FormStartPosition.Manual;
                    Formulario.Location = new System.Drawing.Point(-4000, -4000);
                    Formulario.ShowInTaskbar = false;
                    Formulario.Show();
                    ContabilidadFuncControl<TextBox>(Formulario, "ContabilidadTxtDescripcion").Text = "Prueba de campo vacío";
                    ContabilidadFuncControl<ComboBox>(Formulario, "ContabilidadCboModulo").SelectedIndex = -1;
                    ContabilidadFuncControl<NumericUpDown>(Formulario, "ContabilidadNudCompra").Value = 5000m;
                    NumericUpDown Campo = ContabilidadFuncControl<NumericUpDown>(Formulario, NombreCampo);
                    Campo.Value = ValorAnterior;
                    ContabilidadMetBorrar(Campo);
                    typeof(FrmActivosFijos).GetMethod("ContabilidadMetValidar", BindingFlags.Instance | BindingFlags.NonPublic)!
                        .Invoke(Formulario, new object[] { Formulario, EventArgs.Empty });
                    Mensaje = ContabilidadFuncControl<Label>(Formulario, "ContabilidadLblError" + NombreCampo).Text;
                    Formulario.Close();
                }
            });
            return Mensaje;
        }

        // Compra borrada
        [TestMethod]
        public void Test_Guardar_ValorDeCompraBorrado_NoGuarda()
        {
            // Arrange y Act
            string Mensaje = ContabilidadFuncGuardarConCampoBorrado("ContabilidadNudCompra", 12000m);

            // Assert
            Assert.AreEqual("Ingrese el valor de compra.", Mensaje);
        }

        // Residual borrado
        [TestMethod]
        public void Test_Guardar_ValorResidualBorrado_NoGuarda()
        {
            // Arrange y Act
            string Mensaje = ContabilidadFuncGuardarConCampoBorrado("ContabilidadNudResidual", 500m);

            // Assert
            Assert.AreEqual("Ingrese el valor residual (0 si no tiene).", Mensaje);
        }

        // Vida útil borrada
        [TestMethod]
        public void Test_Guardar_VidaUtilBorrada_NoGuarda()
        {
            // Arrange y Act
            string Mensaje = ContabilidadFuncGuardarConCampoBorrado("ContabilidadNudMeses", 60m);

            // Assert
            Assert.AreEqual("Ingrese la vida útil en meses.", Mensaje);
        }
    }
}
