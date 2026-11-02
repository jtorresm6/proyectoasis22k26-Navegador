/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Oskar Cermeño
 * Carné : 0901-23-15379
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Reglas de la póliza que no dependen de la pantalla: catálogos,
 *  período contable a partir de la fecha, totales del debe y del
 *  haber, cuadre, validación y grabación según el estado.
 * Reglas especificas:
 *  La validación vive en ClsModeloPolizaContabilidad (DataAnnotations).
 *  La Vista solo habla con esta clase, nunca con el Modelo.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Linq;
// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using CapaModelo_Contabilidad.Contratos;
using CapaModelo_Contabilidad.Entidades;
using CapaModelo_Contabilidad.Repositorios;
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaControlador_Contabilidad
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public class ClsControladorPolizasContabilidad
    {
        private readonly IRepositorioPolizasContabilidad _Repositorio;

        public ClsControladorPolizasContabilidad() : this(new ClsRepositorioPolizasContabilidad())
        {
        }

        public ClsControladorPolizasContabilidad(IRepositorioPolizasContabilidad Repositorio)
        {
            _Repositorio = Repositorio ?? throw new ArgumentNullException(nameof(Repositorio));
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public List<ClsModeloCuentaPolizaContabilidad> ContabilidadFuncObtenerCuentas(int IdModulo)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            return new ClsModeloCuentaContabilidad().ContabilidadFuncObtenerImputables(IdModulo)
                // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
                .Select(Cuenta => new ClsModeloCuentaPolizaContabilidad { IdCuenta = Cuenta.IdCuenta, Codigo = Cuenta.Codigo, Nombre = Cuenta.Nombre })
                // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
                .ToList();
        }

        public string ContabilidadFuncObtenerNombreModulo(int IdModulo)
        {
            string Nombre = _Repositorio.ContabilidadFuncObtenerNombreModulo(IdModulo);
            return string.IsNullOrWhiteSpace(Nombre) ? "Encabezado" : Nombre;
        }

        public string[] ContabilidadFuncObtenerEstados()
        {
            return _Repositorio.ContabilidadFuncObtenerEstados();
        }

        public string ContabilidadFuncObtenerPeriodo(DateTime Fecha)
        {
            return Fecha.ToString("yyyy-MM");
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public decimal ContabilidadFuncTotalDebe(IEnumerable<ClsModeloLineaPolizaContabilidad> Lineas)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            return Lineas.Sum(Linea => Linea.Debe);
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public decimal ContabilidadFuncTotalHaber(IEnumerable<ClsModeloLineaPolizaContabilidad> Lineas)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            return Lineas.Sum(Linea => Linea.Haber);
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public bool ContabilidadFuncEstaCuadrada(IEnumerable<ClsModeloLineaPolizaContabilidad> Lineas)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            decimal Debe = ContabilidadFuncTotalDebe(Lineas);
            return Debe > 0 && Debe == ContabilidadFuncTotalHaber(Lineas);
        }

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public string ContabilidadFuncValidar(string Descripcion, IList<ClsModeloLineaPolizaContabilidad> Lineas)
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            return ClsValidacionDatosContabilidad.ContabilidadFuncPrimerError(
                new ClsModeloPolizaContabilidad { Descripcion = Descripcion, Lineas = Lineas });
        }

        public List<ClsModeloPolizaContabilidad> ContabilidadFuncObtenerPolizas()
        {
            return _Repositorio.ContabilidadFuncObtenerTodos()
                .Select(Poliza => new ClsModeloPolizaContabilidad
                {
                    IdPoliza = Poliza.IdPoliza, CodigoPoliza = Poliza.CodigoPoliza, Fecha = Poliza.Fecha,
                    Periodo = Poliza.PeriodoContable, Estado = Poliza.Estado, IdModulo = Poliza.IdModulo, Modulo = Poliza.Modulo, Descripcion = Poliza.Descripcion,
                    // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
                    Lineas = Poliza.Detalle.Select(Detalle => new ClsModeloLineaPolizaContabilidad
                    // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
                        { IdCuenta = Detalle.IdCuenta, Cuenta = Detalle.Cuenta, Debe = Detalle.Debe, Haber = Detalle.Haber }).ToList()
                })
                .ToList();
        }

        public bool ContabilidadFuncGrabarCambios(EstadoEntidadContabilidad Estado, ClsModeloPolizaContabilidad Modelo, out string Mensaje)
        {
            if (Modelo == null)
            {
                Mensaje = "No se recibió la póliza.";
                return false;
            }

            Mensaje = ClsValidacionDatosContabilidad.ContabilidadFuncPrimerError(Modelo);
            if (Mensaje != string.Empty) return false;

            string EstadoPoliza = string.IsNullOrWhiteSpace(Modelo.Estado) ? "BORRADOR" : Modelo.Estado.Trim().ToUpperInvariant();
            if (!ContabilidadFuncObtenerEstados().Contains(EstadoPoliza))
            {
                Mensaje = "El estado '" + Modelo.Estado + "' no es válido.";
                return false;
            }
            if (Modelo.IdModulo <= 0)
            {
                Mensaje = "La póliza debe indicar el módulo que la origina.";
                return false;
            }

            if (Modelo.Lineas.GroupBy(Linea => Linea.IdCuenta).Any(Grupo => Grupo.Count() > 1))
            {
                Mensaje = "Una cuenta no puede repetirse en el detalle de la póliza.";
                return false;
            }

            ClsPolizaContabilidad Entidad = new ClsPolizaContabilidad
            {
                IdPoliza = Modelo.IdPoliza, Fecha = Modelo.Fecha.Date, Estado = EstadoPoliza, IdModulo = Modelo.IdModulo,
                PeriodoContable = ContabilidadFuncObtenerPeriodo(Modelo.Fecha), Descripcion = Modelo.Descripcion.Trim(),
                Detalle = Modelo.Lineas.Select(Linea => new ClsPolizaDetalleContabilidad
                    { IdCuenta = Linea.IdCuenta, Cuenta = Linea.Cuenta, Debe = Linea.Debe, Haber = Linea.Haber }).ToList()
            };
            try
            {
                switch (Estado)
                {
                    case EstadoEntidadContabilidad.Agregar:
                        if (_Repositorio.ContabilidadFuncAgregar(Entidad) <= 0)
                        {
                            Mensaje = "No se pudo registrar la póliza.";
                            return false;
                        }
                        Modelo.IdPoliza = Entidad.IdPoliza;
                        Modelo.CodigoPoliza = Entidad.CodigoPoliza;
                        Modelo.Periodo = Entidad.PeriodoContable;
                        Mensaje = "Póliza " + Entidad.CodigoPoliza + " registrada correctamente.";
                        break;
                    case EstadoEntidadContabilidad.Modificar:
                        if (_Repositorio.ContabilidadFuncEditar(Entidad) <= 0)
                        {
                            Mensaje = "Solo se puede modificar una póliza en borrador.";
                            return false;
                        }
                        Mensaje = "Póliza actualizada correctamente.";
                        break;
                    case EstadoEntidadContabilidad.Borrar:
                        if (_Repositorio.ContabilidadFuncEliminar(Entidad) <= 0)
                        {
                            Mensaje = "Una póliza aplicada no se puede anular.";
                            return false;
                        }
                        Mensaje = "Póliza eliminada correctamente.";
                        break;
                }
                return true;
            }
            catch (Exception Excepcion)
            {
                Mensaje = "Error al grabar la póliza: " + Excepcion.Message;
                return false;
            }
        }
    }
}
