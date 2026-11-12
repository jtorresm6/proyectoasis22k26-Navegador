/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Mario Taracena
 * Carné : 0901-23-9335
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Reglas del presupuesto: módulos, partidas, totales, restante,
 *  validación y grabación según el estado. Responsable de la
 *  pantalla: Mario.
 * Reglas especificas:
 *  Las validaciones están en los modelos con DataAnnotations. La
 *  Vista recibe DataTable para poder paginar y ordenar la tabla.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CapaModelo_Contabilidad.Contratos;
using CapaModelo_Contabilidad.Repositorios;
// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using CapaModelo_Contabilidad.Entidades;
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaControlador_Contabilidad
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public class ClsControladorPresupuestosContabilidad
    {
        private readonly IRepositorioPresupuestosContabilidad _Repositorio = new ClsRepositorioPresupuestosContabilidad();

        public string[] ContabilidadFuncModulos()
        {
            return _Repositorio.ContabilidadFuncObtenerModulos();
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        public List<ClsModeloPresupuestoContabilidad> ContabilidadFuncPresupuestos()
        {
            return _Repositorio.ContabilidadFuncObtenerTodos().Select(Presupuesto => new ClsModeloPresupuestoContabilidad
            {
                IdPresupuesto = Presupuesto.IdPresupuesto, Nombre = Presupuesto.Nombre, FechaInicio = Presupuesto.FechaInicio,
                FechaCierre = Presupuesto.FechaCierre, Gastado = Presupuesto.Gastado,
                Partidas = Presupuesto.Detalle.Select(Partida => new ClsModeloPartidaPresupuestoContabilidad
                    { Nombre = Partida.Nombre, Modulo = Partida.Modulo, Cantidad = Partida.Cantidad }).ToList()
            }).ToList();
        }

        public DataTable ContabilidadFuncPresupuesto(ClsModeloPresupuestoContabilidad Presupuesto)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        {
            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("Nombre", typeof(string));
            Tabla.Columns.Add("Modulo", typeof(string));
            Tabla.Columns.Add("Cantidad", typeof(decimal));
            if (Presupuesto != null)
                // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
                foreach (ClsModeloPartidaPresupuestoContabilidad Partida in Presupuesto.Partidas)
                // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
                    Tabla.Rows.Add(Partida.Nombre, Partida.Modulo, Partida.Cantidad);
            return Tabla;
        }

        public Dictionary<string, string> ContabilidadFuncErroresPartida(string Nombre, string Modulo, decimal Cantidad, DataTable Detalle)
        {
            Dictionary<string, string> Errores = ClsValidacionDatosContabilidad.ContabilidadFuncErrores(
                new ClsModeloPartidaPresupuestoContabilidad { Nombre = Nombre, Modulo = Modulo, Cantidad = Cantidad });
            if (!Errores.ContainsKey("Modulo") && !ContabilidadFuncModulos().Contains(Modulo)) Errores["Modulo"] = "Seleccione un módulo válido.";
            if (!Errores.ContainsKey("Cantidad") && ContabilidadFuncTotalPresupuesto(Detalle) + Cantidad > ClsLimitesContabilidad.MaximoMonetario)
                Errores["Cantidad"] = "El total excede el límite permitido.";
            return Errores;
        }

        public string ContabilidadFuncErrorFechas(DateTime Inicio, DateTime Fin)
        {
            return ClsValidacionDatosContabilidad.ContabilidadFuncErrorRangoFechas(Inicio, Fin);
        }

        public decimal ContabilidadFuncTotalPresupuesto(DataTable Detalle)
        {
            decimal Total = 0m;
            foreach (DataRow Fila in Detalle.Rows) Total += Convert.ToDecimal(Fila["Cantidad"]);
            return Total;
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        public DataTable ContabilidadFuncEjecucion(DateTime Inicio, DateTime? Cierre, DataTable Detalle)
        {
            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("Modulo", typeof(string));
            Tabla.Columns.Add("Asignado", typeof(decimal));
            Tabla.Columns.Add("Ejecutado", typeof(decimal));
            Tabla.Columns.Add("Porcentaje", typeof(decimal));
            Tabla.Columns.Add("Restante", typeof(decimal));
            if (Detalle == null || Detalle.Rows.Count == 0) return Tabla;
            DateTime Hasta = Cierre.HasValue ? Cierre.Value : DateTime.Today;
            Dictionary<string, decimal> Ejecutado = Hasta.Date < Inicio.Date ? new Dictionary<string, decimal>()
                : _Repositorio.ContabilidadFuncObtenerEjecutado(Inicio, Hasta);
            foreach (var Modulo in Detalle.Rows.Cast<DataRow>()
                .GroupBy(Fila => Convert.ToString(Fila["Modulo"]), Fila => Convert.ToDecimal(Fila["Cantidad"])).OrderBy(Grupo => Grupo.Key))
            {
                decimal Asignado = Modulo.Sum();
                decimal Usado = Ejecutado.ContainsKey(Modulo.Key) ? Ejecutado[Modulo.Key] : 0m;
                Tabla.Rows.Add(Modulo.Key, Asignado, Usado, Asignado == 0m ? 0m : Usado / Asignado, Asignado - Usado);
            }
            return Tabla;
        }

        public decimal ContabilidadFuncTotalEjecutado(DataTable Ejecucion)
        {
            return Ejecucion == null ? 0m : Ejecucion.Rows.Cast<DataRow>().Sum(Fila => (decimal)Fila["Ejecutado"]);
        }
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        private ClsModeloPresupuestoContabilidad ContabilidadFuncCrearModelo(DateTime Inicio, DateTime? Cierre, decimal Gastado, DataTable Detalle,
            int IdPresupuesto = 0, string Nombre = "Presupuesto")
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        {
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
            ClsModeloPresupuestoContabilidad Modelo = new ClsModeloPresupuestoContabilidad
                { IdPresupuesto = IdPresupuesto, Nombre = Nombre, FechaInicio = Inicio, FechaCierre = Cierre, Gastado = Gastado };
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
            foreach (DataRow Fila in Detalle.Rows)
                Modelo.Partidas.Add(new ClsModeloPartidaPresupuestoContabilidad
                {
                    Nombre = Convert.ToString(Fila["Nombre"]), Modulo = Convert.ToString(Fila["Modulo"]), Cantidad = Convert.ToDecimal(Fila["Cantidad"])
                });
            return Modelo;
        }

        public string ContabilidadFuncValidarPresupuesto(DateTime Inicio, DateTime? Cierre, DataTable Detalle)
        {
            return ClsValidacionDatosContabilidad.ContabilidadFuncPrimerError(ContabilidadFuncCrearModelo(Inicio, Cierre, 0m, Detalle));
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        public bool ContabilidadFuncGrabarCambios(EstadoEntidadContabilidad Estado, ref int IdPresupuesto, string Nombre, DateTime Inicio,
            DateTime? Cierre, decimal Gastado, DataTable Detalle, out string Mensaje)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        {
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
            ClsModeloPresupuestoContabilidad Modelo = ContabilidadFuncCrearModelo(Inicio, Cierre, Gastado, Detalle, IdPresupuesto,
                string.IsNullOrWhiteSpace(Nombre) ? null : Nombre.Trim());
            Mensaje = Estado == EstadoEntidadContabilidad.Borrar ? string.Empty : ClsValidacionDatosContabilidad.ContabilidadFuncPrimerError(Modelo);
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
            if (Mensaje != string.Empty) return false;

            ClsPresupuestoContabilidad Entidad = new ClsPresupuestoContabilidad
            {
                // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
                IdPresupuesto = Modelo.IdPresupuesto, Nombre = Modelo.Nombre, Total = Modelo.Partidas.Sum(Partida => Partida.Cantidad),
                FechaInicio = Modelo.FechaInicio, FechaCierre = Modelo.FechaCierre, Gastado = Modelo.Gastado,
                // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
                Detalle = Modelo.Partidas.Select(Partida => new ClsPresupuestoDetalleContabilidad
                    { Nombre = Partida.Nombre.Trim(), Modulo = Partida.Modulo, Cantidad = Partida.Cantidad }).ToList()
            };
            try
            {
                switch (Estado)
                {
                    case EstadoEntidadContabilidad.Agregar:
                        _Repositorio.ContabilidadFuncAgregar(Entidad);
                        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
                        IdPresupuesto = Entidad.IdPresupuesto;
                        Mensaje = "Presupuesto guardado correctamente.";
                        // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
                        break;
                    case EstadoEntidadContabilidad.Modificar:
                        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
                        if (_Repositorio.ContabilidadFuncEditar(Entidad) == 0)
                        {
                            Mensaje = "El presupuesto ya no existe o fue eliminado.";
                            return false;
                        }
                        // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
                        Mensaje = "Presupuesto actualizado correctamente.";
                        break;
                    case EstadoEntidadContabilidad.Borrar:
                        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
                        if (_Repositorio.ContabilidadFuncEliminar(Entidad) == 0)
                        {
                            Mensaje = "El presupuesto ya no existe o fue eliminado.";
                            return false;
                        }
                        IdPresupuesto = 0;
                        // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
                        Mensaje = "Presupuesto eliminado correctamente.";
                        break;
                }
                return true;
            }
            catch (Exception Excepcion)
            {
                Mensaje = Excepcion.Message;
                return false;
            }
        }
    }
}
