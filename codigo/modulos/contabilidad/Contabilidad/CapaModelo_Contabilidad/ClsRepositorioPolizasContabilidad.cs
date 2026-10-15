/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 14/10/2026
 * ==================================================================
 * Propósito :
 *  Origen de datos de las pólizas del prototipo. Entrega catálogos de
 *  EJEMPLO con la misma forma que el esquema presupuesto_db (tabla
 *  cuenta, estados y orígenes de poliza_encabezado). No consulta la
 *  base de datos: cuando el prototipo pase a datos reales, solo este
 *  repositorio cambia y el resto de las capas se conserva.
 * ===================================================================
*/

using System.Collections.Generic;

namespace CapaModelo_Contabilidad
{
    public class ClsRepositorioPolizasContabilidad
    {
        public List<ClsCuentaContabilidad> ContabilidadFuncObtenerCuentas()
        {
            string[] Nombres =
            {
                "Caja", "Bancos", "Clientes nacionales", "Inventario de materia prima",
                "Inventario de producto terminado", "Maquinaria y equipo", "Depreciación acumulada",
                "Proveedores", "IVA por pagar", "Préstamos bancarios", "Capital social", "Ventas",
                "Costo de ventas", "Gastos de administración", "Gastos de ventas", "Gastos financieros"
            };
            List<ClsCuentaContabilidad> Cuentas = new List<ClsCuentaContabilidad>();
            string[] Codigos =
            {
                "1.01.01.001", "1.01.02.001", "1.01.03.001", "1.01.04.001",
                "1.01.04.002", "1.02.01.001", "1.02.02.001", "2.01.01.001",
                "2.01.02.001", "2.02.01.001", "3.01.01.001", "4.01.01.001",
                "5.01.01.001", "6.01.01.001", "6.02.01.001", "6.03.01.001"
            };
            for (int Indice = 0; Indice < Nombres.Length; Indice++)
                Cuentas.Add(new ClsCuentaContabilidad { IdCuenta = Indice + 1, Codigo = Codigos[Indice], Nombre = Nombres[Indice] });
            return Cuentas;
        }

        // ENUM de poliza_encabezado.estado
        public string[] ContabilidadFuncObtenerEstados()
        {
            return new[] { "BORRADOR", "APLICADA", "ANULADA" };
        }

        // Valores documentados para poliza_encabezado.origen
        public string[] ContabilidadFuncObtenerOrigenes()
        {
            return new[] { "MANUAL", "ACTIVOS_FIJOS", "PRESUPUESTO" };
        }
    }
}
