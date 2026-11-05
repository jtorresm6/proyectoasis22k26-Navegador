/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Mario Taracena
 * Carné : 0901-23-9335
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Repositorio de presupuestos. Por ahora trabaja en memoria con una
 *  muestra; no consulta la base de datos. Al pasar a MySQL solo
 *  cambia esta clase.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Linq;
using CapaModelo_Contabilidad.Contratos;
// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using CapaModelo_Contabilidad.Entidades;
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaModelo_Contabilidad.Repositorios
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public class ClsRepositorioPresupuestosContabilidad : IRepositorioPresupuestosContabilidad
    {
        private readonly List<ClsPresupuestoContabilidad> _Presupuestos = new List<ClsPresupuestoContabilidad>();

        public ClsRepositorioPresupuestosContabilidad()
        {
            ClsPresupuestoContabilidad Muestra = new ClsPresupuestoContabilidad
            {
                IdPresupuesto = 1, FechaInicio = new DateTime(2026, 10, 1), FechaCierre = new DateTime(2026, 10, 31), Gastado = 21500m
            };
            Muestra.Detalle.Add(new ClsPresupuestoDetalleContabilidad { Nombre = "Insumos de producción", Modulo = "Compras", Cantidad = 45000m });
            Muestra.Detalle.Add(new ClsPresupuestoDetalleContabilidad { Nombre = "Distribución de producto", Modulo = "Logística", Cantidad = 18000m });
            Muestra.Detalle.Add(new ClsPresupuestoDetalleContabilidad { Nombre = "Promoción comercial", Modulo = "Ventas", Cantidad = 12000m });
            _Presupuestos.Add(Muestra);
        }

        public int ContabilidadFuncAgregar(ClsPresupuestoContabilidad Entidad)
        {
            Entidad.IdPresupuesto = _Presupuestos.Count == 0 ? 1 : _Presupuestos.Max(Presupuesto => Presupuesto.IdPresupuesto) + 1;
            _Presupuestos.Add(Entidad);
            return 1;
        }

        public int ContabilidadFuncEditar(ClsPresupuestoContabilidad Entidad)
        {
            int Indice = _Presupuestos.FindIndex(Presupuesto => Presupuesto.IdPresupuesto == Entidad.IdPresupuesto);
            if (Indice < 0) return 0;
            _Presupuestos[Indice] = Entidad;
            return 1;
        }

        public int ContabilidadFuncEliminar(ClsPresupuestoContabilidad Entidad)
        {
            return _Presupuestos.RemoveAll(Presupuesto => Presupuesto.IdPresupuesto == Entidad.IdPresupuesto);
        }

        public IEnumerable<ClsPresupuestoContabilidad> ContabilidadFuncObtenerTodos()
        {
            return _Presupuestos.ToList();
        }

        public string[] ContabilidadFuncObtenerModulos()
        {
            return new[] { "Contabilidad", "Logística", "Compras", "Ventas", "Recursos Humanos", "Bancos" };
        }
    }
}
