using System.Data;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Prepara las validaciones y el resumen de saldos del prototipo de cierre de movimientos, con sus totales por período.
 * ===================================================================
*/

namespace CapaControlador_CierreMovimientos
{
 
    
    
  
    public class ClsResumenCierre
    {
        public DataTable Validaciones { get; set; }
        public DataTable SaldosProveedor { get; set; }
        public decimal TotalCargos { get; set; }
        public decimal TotalAbonos { get; set; }
        public decimal SaldoFinal { get; set; }
        public bool PuedeCerrar { get; set; }
    }

    public class ClsCierreMovimientos
    {
        public DataTable ComprasFuncObtenerPeriodosCerrables()
        {
            DataTable Periodos = new DataTable();
            Periodos.Columns.Add("cmp_id_periodo", typeof(int));
            Periodos.Columns.Add("cmp_descripcion", typeof(string));
            Periodos.Rows.Add(202610, "Octubre 2026");
            return Periodos;
        }

        public ClsResumenCierre ComprasFuncPrepararCierre(int IdPeriodo)
        {
            DataTable Validaciones = ComprasFuncDatosValidaciones(IdPeriodo);
            DataTable Saldos = ComprasFuncDatosSaldos(IdPeriodo);

            ClsResumenCierre Resumen = new ClsResumenCierre
            {
                Validaciones = Validaciones,
                SaldosProveedor = Saldos,
                PuedeCerrar = true
            };

            foreach (DataRow Fila in Saldos.Rows)
            {
                decimal SaldoInicial = (decimal)Fila["cmp_saldo_inicial"];
                decimal Cargos = (decimal)Fila["cmp_total_cargos"];
                decimal Abonos = (decimal)Fila["cmp_total_abonos"];

                Fila["cmp_saldo_final"] = SaldoInicial + Cargos - Abonos;
                Resumen.TotalCargos += Cargos;
                Resumen.TotalAbonos += Abonos;
                Resumen.SaldoFinal += (decimal)Fila["cmp_saldo_final"];
            }

            foreach (DataRow Fila in Validaciones.Rows)
            {
                if ((string)Fila["cmp_estado"] == "Bloqueante")
                {
                    Resumen.PuedeCerrar = false;
                }
            }

            return Resumen;
        }

        private DataTable ComprasFuncDatosValidaciones(int IdPeriodo)
        {
            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("cmp_estado", typeof(string));
            Tabla.Columns.Add("cmp_validacion", typeof(string));
            Tabla.Columns.Add("cmp_origen", typeof(string));
            Tabla.Rows.Add("Correcta", "Período anterior cerrado", "3.8 Cierre");
            Tabla.Rows.Add("Correcta", "Sin movimientos pendientes", "3.1 Movimientos");
            Tabla.Rows.Add("Correcta", "Pagos cuadrados con caja", "3.5 Pagos");
            Tabla.Rows.Add("Correcta", "Pólizas enviadas a Contabilidad", "3.2 Enlace contable");
            Tabla.Rows.Add("Correcta", "Saldos cuadran con cuenta CxP", "Contabilidad");
            Tabla.Rows.Add("Advertencia", "1 compra recibida sin factura", "3.3 Órdenes de compra");
            Tabla.Rows.Add("Advertencia", "2 órdenes en autorización", "3.7 Aprobación");
            return Tabla;
        }

        private DataTable ComprasFuncDatosSaldos(int IdPeriodo)
        {
            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("cmp_proveedor", typeof(string));
            Tabla.Columns.Add("cmp_saldo_inicial", typeof(decimal));
            Tabla.Columns.Add("cmp_total_cargos", typeof(decimal));
            Tabla.Columns.Add("cmp_total_abonos", typeof(decimal));
            Tabla.Columns.Add("cmp_saldo_final", typeof(decimal));
            Tabla.Rows.Add("Cárnicos del Pacífico, S.A.", 85400m, 142750m, 120000m, 0m);
            Tabla.Rows.Add("Empaques Industriales GT", 22300m, 31200m, 40500m, 0m);
            Tabla.Rows.Add("Condimentos La Antigua", 9850m, 12400m, 9850m, 0m);
            Tabla.Rows.Add("Tripas Naturales de Occidente", 15600m, 18900m, 15600m, 0m);
            Tabla.Rows.Add("Frigoríficos Unidos", 41200m, 56300m, 50000m, 0m);
            return Tabla;
        }
    }
    
}
