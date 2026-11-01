/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Define los códigos de las aplicaciones de Compras y permite consultar el identificador de su módulo.
 * ===================================================================
*/

namespace CapaControlador_Compras
{
    public static class ClsAplicacionesCompras
    {
        public const int MdiCompras = 6000;
        public const int MovimientosProveedores = 6001;
        public const int Polizas = 6002;
        public const int OrdenesCompra = 6003;
        public const int PagosProveedores = 6004;
        public const int AprobacionFinanciera = 6005;
        public const int CierreMovimientos = 6006;
        public const int AprobarOrden = 6007;
        public const int RechazarOrden = 6008;
        public const int EvaluarOrden = 6009;
        public const int MantenimientoProveedor = 6010;
        public const int MantenimientoMetodoPago = 6011;
        public const int EncabezadosComprasPorPagar = 6012;

        public static int ComprasFuncObtenerIdModulo()
        {
            return CapaModelo_Compras.ClsModuloCompras.ComprasFuncObtenerIdModulo();
        }
    }
}
