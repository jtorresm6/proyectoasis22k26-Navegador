using System;
using System.Windows.Forms;
using CapaControlador_Compras;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaVista_Navegador;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Configura el navegador de mantenimiento con la tabla, el módulo y la aplicación utilizados para los permisos de la sesión.
 * ===================================================================
*/

namespace CapaVista_Compras
{
    internal static class ClsMantenimientoCompras
    {
        internal static void ComprasProcConfigurar(Form Formulario, Navegador Navegador, string Tabla, int CodigoAplicacion, int? CodigoModulo = null)
        {
            Navegador.Enabled = false;
            Formulario.Load += (Remitente, Evento) =>
            {
                string Mensaje;
                if (!ClsSesionSeguridad.HaySesionActiva)
                    Mensaje = "Inicie sesión desde Seguridad para habilitar el mantenimiento de proveedores.";
                else
                {
                    try
                    {
                        int Modulo = CodigoModulo ?? ClsAplicacionesCompras.ComprasFuncObtenerIdModulo();
                        Navegador.NavegadorMetConfigurar(Tabla, Modulo, CodigoAplicacion);
                        Navegador.Enabled = true;
                        return;
                    }
                    catch (InvalidOperationException Error)
                    {
                        Mensaje = Error.Message;
                    }
                }
                Formulario.Controls.Add(new Label
                {
                    Name = "ComprasLblEstadoSeguridad", Text = Mensaje, Dock = DockStyle.Top,
                    Height = 40, ForeColor = System.Drawing.Color.Red,
                    BackColor = CapaVista_Componentes.Componentes.ClsTemaCompras.FondoGeneral
                });
                Formulario.Controls[Formulario.Controls.Count - 1].BringToFront();
            };
        }
    }
}
