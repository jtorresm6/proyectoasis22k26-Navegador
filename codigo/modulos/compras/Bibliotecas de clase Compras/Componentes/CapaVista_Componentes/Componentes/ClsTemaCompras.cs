using System.Drawing;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Centraliza la paleta de colores utilizada por los controles y formularios de Compras.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    public static class ClsTemaCompras
    {
        public static readonly Color PrincipalNavegador = Color.FromArgb(30, 42, 90);
        public static readonly Color AcentoNavegador = Color.FromArgb(61, 86, 166);
        public static readonly Color FondoEtiquetasDestacadas = Color.FromArgb(217, 145, 62);
        public static readonly Color FondoGeneral = Color.FromArgb(246, 247, 250);
        public static readonly Color FondoBotones = Color.FromArgb(213, 220, 239);
        public static readonly Color FondoContenedor = Color.FromArgb(250, 247, 242);

        public static Color BotonHover => ComprasFuncMezclar(FondoBotones, AcentoNavegador, 20);
        public static Color BotonPresionado => ComprasFuncMezclar(FondoBotones, AcentoNavegador, 35);

        private static Color ComprasFuncMezclar(Color Fondo, Color Acento, int Porcentaje)
        {
            return Color.FromArgb(
                (Fondo.R * (100 - Porcentaje) + Acento.R * Porcentaje) / 100,
                (Fondo.G * (100 - Porcentaje) + Acento.G * Porcentaje) / 100,
                (Fondo.B * (100 - Porcentaje) + Acento.B * Porcentaje) / 100);
        }
    }
}
