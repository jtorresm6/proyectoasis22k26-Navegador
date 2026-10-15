/*
 * Área: Contabilidad. Autor: Gabriel André Guillén Pocón. Carné: 0901-23-1998.
 * Fecha: 14/10/2026. Propósito: datos sintéticos para las pantallas del prototipo.
 * No abre conexiones ni conserva cambios entre ventanas.
 */
using System;
using System.Data;
using System.Linq;

namespace CapaModelo_Contabilidad
{
    public class ClsRepositorioPrototiposContabilidad
    {
        public string[] ContabilidadFuncModulos()
        {
            return new[] { "Contabilidad", "Logística", "Compras", "Ventas", "Recursos Humanos", "Bancos" };
        }

        public DataTable ContabilidadFuncPresupuesto()
        {
            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("Nombre", typeof(string));
            Tabla.Columns.Add("Modulo", typeof(string));
            Tabla.Columns.Add("Cantidad", typeof(decimal));
            Tabla.Rows.Add("Insumos de producción", "Compras", 45000m);
            Tabla.Rows.Add("Distribución de producto", "Logística", 18000m);
            Tabla.Rows.Add("Promoción comercial", "Ventas", 12000m);
            return Tabla;
        }

        public DataTable ContabilidadFuncVidaUtil()
        {
            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("Tipo", typeof(string));
            Tabla.Columns.Add("Nombre", typeof(string));
            Tabla.Columns.Add("Meses", typeof(int));
            Tabla.Rows.Add("VEHICULO", "Vehículo de distribución", 60);
            Tabla.Rows.Add("MAQUINARIA", "Maquinaria de producción", 120);
            Tabla.Rows.Add("EQ_COMPUTO", "Equipo de cómputo", 36);
            Tabla.Rows.Add("MOBILIARIO", "Mobiliario y equipo", 60);
            return Tabla;
        }

        // La fecha de referencia es explícita para hacer reproducibles las muestras.
        public DataTable ContabilidadFuncMovimientos()
        {
            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("Poliza", typeof(int));
            Tabla.Columns.Add("Fecha", typeof(DateTime));
            Tabla.Columns.Add("Estado", typeof(string));
            Tabla.Columns.Add("Cuenta", typeof(string));
            // Clasificación SOLO de la muestra: cuenta aún no define un rubro en el SQL.
            Tabla.Columns.Add("Rubro", typeof(string));
            Tabla.Columns.Add("Debe", typeof(decimal));
            Tabla.Columns.Add("Haber", typeof(decimal));
            Tabla.Rows.Add(1, new DateTime(2026, 10, 2), "APLICADA", "Bancos", "ACTIVO", 120000m, 0m);
            Tabla.Rows.Add(1, new DateTime(2026, 10, 2), "APLICADA", "Ventas", "INGRESOS", 0m, 120000m);
            Tabla.Rows.Add(2, new DateTime(2026, 10, 5), "APLICADA", "Costo de ventas", "COSTOS", 65000m, 0m);
            Tabla.Rows.Add(2, new DateTime(2026, 10, 5), "APLICADA", "Inventario de producto terminado", "ACTIVO", 0m, 65000m);
            Tabla.Rows.Add(3, new DateTime(2026, 10, 8), "APLICADA", "Gastos de administración", "GASTOS", 18000m, 0m);
            Tabla.Rows.Add(3, new DateTime(2026, 10, 8), "APLICADA", "Bancos", "ACTIVO", 0m, 18000m);
            Tabla.Rows.Add(4, new DateTime(2026, 10, 10), "BORRADOR", "Gastos de ventas", "GASTOS", 2500m, 0m);
            Tabla.Rows.Add(4, new DateTime(2026, 10, 10), "BORRADOR", "Bancos", "ACTIVO", 0m, 2500m);
            Tabla.Rows.Add(5, new DateTime(2026, 10, 11), "ANULADA", "Bancos", "ACTIVO", 900m, 0m);
            Tabla.Rows.Add(5, new DateTime(2026, 10, 11), "ANULADA", "Ventas", "INGRESOS", 0m, 900m);
            Tabla.Rows.Add(6, new DateTime(2026, 9, 25), "APLICADA", "Bancos", "ACTIVO", 80000m, 0m);
            Tabla.Rows.Add(6, new DateTime(2026, 9, 25), "APLICADA", "Ventas", "INGRESOS", 0m, 80000m);
            Tabla.Columns.Add("Codigo", typeof(string));
            var Cuentas = new ClsRepositorioPolizasContabilidad().ContabilidadFuncObtenerCuentas();
            foreach (DataRow Fila in Tabla.Rows)
                Fila["Codigo"] = Cuentas.Single(Cuenta => Cuenta.Nombre == (string)Fila["Cuenta"]).Codigo;
            return Tabla;
        }
    }
}
