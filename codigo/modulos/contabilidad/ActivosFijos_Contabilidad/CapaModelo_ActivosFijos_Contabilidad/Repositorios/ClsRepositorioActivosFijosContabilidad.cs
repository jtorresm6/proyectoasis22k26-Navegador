/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Repositorio de activos fijos y del maestro de vida útil. Por ahora
 *  trabaja en memoria; no consulta la base de datos. Al pasar a MySQL
 *  solo cambia esta clase.
 * ===================================================================
*/

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
    public class ClsRepositorioActivosFijosContabilidad : IRepositorioActivosFijosContabilidad
    {
        private readonly List<ClsActivoFijoContabilidad> _Activos = new List<ClsActivoFijoContabilidad>();

        public int ContabilidadFuncAgregar(ClsActivoFijoContabilidad Entidad)
        {
            Entidad.IdActivo = _Activos.Count == 0 ? 1 : _Activos.Max(Activo => Activo.IdActivo) + 1;
            _Activos.Add(Entidad);
            return 1;
        }

        public int ContabilidadFuncEditar(ClsActivoFijoContabilidad Entidad)
        {
            int Indice = _Activos.FindIndex(Activo => Activo.IdActivo == Entidad.IdActivo);
            if (Indice < 0) return 0;
            _Activos[Indice] = Entidad;
            return 1;
        }

        public int ContabilidadFuncEliminar(ClsActivoFijoContabilidad Entidad)
        {
            return _Activos.RemoveAll(Activo => Activo.IdActivo == Entidad.IdActivo);
        }

        public IEnumerable<ClsActivoFijoContabilidad> ContabilidadFuncObtenerTodos()
        {
            return _Activos.ToList();
        }

        public string[] ContabilidadFuncObtenerModulos()
        {
            return new[] { "Contabilidad", "Logística", "Compras", "Ventas", "Recursos Humanos", "Bancos" };
        }

        public List<ClsVidaUtilContabilidad> ContabilidadFuncObtenerVidaUtil()
        {
            return new List<ClsVidaUtilContabilidad>
            {
                new ClsVidaUtilContabilidad { Tipo = "VEHICULO", Nombre = "Vehículo de distribución", Meses = 60 },
                new ClsVidaUtilContabilidad { Tipo = "MAQUINARIA", Nombre = "Maquinaria de producción", Meses = 120 },
                new ClsVidaUtilContabilidad { Tipo = "EQ_COMPUTO", Nombre = "Equipo de cómputo", Meses = 36 },
                new ClsVidaUtilContabilidad { Tipo = "MOBILIARIO", Nombre = "Mobiliario y equipo", Meses = 60 }
            };
        }
    }
}
