/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 *  Punto de entrada del botón Reporte de Presupuestos en el MDI.
 * Reglas especificas:
 *  Este archivo no se modifica. El módulo entrega su ventana en
 *  ClsReportePresupuestosContabilidad.Reporte.cs implementando
 *  ContabilidadMetCrearReporte. Mientras no exista, el MDI avisa que
 *  el reporte todavía no está disponible.
 * ===================================================================
*/

using System.Windows.Forms;
using CapaControlador_Contabilidad;

namespace CapaVista_Contabilidad
{
    public static partial class ClsReportePresupuestosContabilidad
    {
        // ventana del reporte o null
        public static Form ContabilidadFuncCrearReporte(ClsModeloPermisosPantallaContabilidad Permisos)
        {
            Form Formulario = null;
            ContabilidadMetCrearReporte(Permisos, ref Formulario);
            return Formulario;
        }

        // la implementa el módulo
        static partial void ContabilidadMetCrearReporte(ClsModeloPermisosPantallaContabilidad Permisos, ref Form Formulario);
    }
}