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
        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
        private Dictionary<int, string> _Monedas;
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026

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

        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
        public List<KeyValuePair<int, string>> ContabilidadFuncObtenerPaises()
        {
            return _Repositorio.ContabilidadFuncObtenerPaises().ToList();
        }

        public string ContabilidadFuncMonedaPais(int IdPais)
        {
            if (_Monedas == null) _Monedas = _Repositorio.ContabilidadFuncObtenerMonedasPais();
            return _Monedas.ContainsKey(IdPais) ? _Monedas[IdPais] : "GTQ";
        }
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026

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

        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026
        // El sistema empezó a operar en octubre de 2026
        public static readonly DateTime InicioSistema = new DateTime(2026, 10, 1);
        private const string _ErrorInicioSistema = "La fecha no es correcta: el sistema inició en octubre de 2026.";

        // Último día que se puede elegir
        public DateTime ContabilidadFuncFechaMaxima()
        {
            DateTime Mes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            return Mes.AddMonths(1).AddDays(-1);
        }

        // Error de una fecha suelta
        public string ContabilidadFuncErrorFecha(DateTime Fecha)
        {
            return Fecha.Date < InicioSistema ? _ErrorInicioSistema : string.Empty;
        }

        // Error de un rango de fechas
        public string ContabilidadFuncErrorFechas(DateTime Inicio, DateTime Fin)
        {
            if (Inicio.Date < InicioSistema || Fin.Date < InicioSistema) return _ErrorInicioSistema;
            return ClsValidacionDatosContabilidad.ContabilidadFuncErrorRangoFechas(Inicio, Fin);
        }
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026

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
                    // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                    IdPais = Poliza.IdPais,
                    // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                    // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
                    Lineas = Poliza.Detalle.Select(Detalle => new ClsModeloLineaPolizaContabilidad
                    // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
                        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
                        { IdCuenta = Detalle.IdCuenta, Cuenta = Detalle.Codigo + " - " + Detalle.Cuenta, Debe = Detalle.Debe, Haber = Detalle.Haber }).ToList()
                        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
                })
                .ToList();
        }

        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
        public ClsModeloPolizaContabilidad ContabilidadFuncObtenerPoliza(int IdPoliza)
        {
            return ContabilidadFuncObtenerPolizas().FirstOrDefault(Poliza => Poliza.IdPoliza == IdPoliza);
        }

        public bool ContabilidadFuncPuedeModificar(string Estado)
        {
            return string.IsNullOrWhiteSpace(Estado) || Estado == "BORRADOR";
        }

        public int ContabilidadFuncNumeroPoliza(string Codigo)
        {
            int Numero;
            return !string.IsNullOrWhiteSpace(Codigo) && int.TryParse(Codigo.Substring(Codigo.LastIndexOf('-') + 1), out Numero) ? Numero : 0;
        }

        public string[] ContabilidadFuncObtenerTiposPeriodo()
        {
            return new[] { "MENSUAL", "ANUAL", "SEMANAL", "RANGO" };
        }

        public string ContabilidadFuncModoActualizacion(int IdModulo)
        {
            try
            {
                // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
                // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 02/11/2026
                return _Repositorio.ContabilidadFuncObtenerMetodoActualizacion(IdModulo) == "LINEA" ? "LINEA" : "BATCH";
                // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 02/11/2026
                // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            }
            catch
            {
                return "BATCH";
            }
        }

        public void ContabilidadMetCalcularRango(string TipoPeriodo, DateTime Referencia, out DateTime Desde, out DateTime Hasta)
        {
            DateTime Dia = Referencia.Date;
            switch (TipoPeriodo)
            {
                case "ANUAL":
                    Desde = new DateTime(Dia.Year, 1, 1);
                    Hasta = new DateTime(Dia.Year, 12, 31);
                    break;
                case "SEMANAL":
                    Desde = Dia.AddDays(-(((int)Dia.DayOfWeek + 6) % 7));
                    Hasta = Desde.AddDays(6);
                    break;
                default:
                    Desde = new DateTime(Dia.Year, Dia.Month, 1);
                    Hasta = Desde.AddMonths(1).AddDays(-1);
                    break;
            }
        }

        public int ContabilidadFuncActualizarSaldos(string TipoPeriodo, DateTime Desde, DateTime Hasta, out string Mensaje)
        {
            if (!ContabilidadFuncObtenerTiposPeriodo().Contains(TipoPeriodo))
            {
                Mensaje = "El tipo de período '" + TipoPeriodo + "' no es válido.";
                return -1;
            }
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026
            string ErrorFechas = ContabilidadFuncErrorFechas(Desde, Hasta);
            if (ErrorFechas != string.Empty)
            {
                Mensaje = ErrorFechas;
                return -1;
            }
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026
            try
            {
                int Procesadas = _Repositorio.ContabilidadFuncActualizarSaldos("BATCH", TipoPeriodo, Desde.Date, Hasta.Date, 0,
                    ClsControladorSeguridadContabilidad.ContabilidadFuncUsuarioSesion());
                Mensaje = Procesadas == 0
                    ? "No hay pólizas validadas pendientes de actualizar entre " + Desde.ToString("dd/MM/yyyy") + " y " + Hasta.ToString("dd/MM/yyyy") + "."
                    : "Saldos actualizados: " + Procesadas + (Procesadas == 1 ? " póliza aplicada." : " pólizas aplicadas.");
                return Procesadas;
            }
            catch (Exception Excepcion)
            {
                Mensaje = "No se pudieron actualizar los saldos: " + Excepcion.Message;
                return -1;
            }
        }
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026

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
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026
            if (Estado != EstadoEntidadContabilidad.Borrar)
            {
                Mensaje = ContabilidadFuncErrorFecha(Modelo.Fecha);
                if (Mensaje != string.Empty) return false;
            }
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 10/11/2026

            if (Modelo.Lineas.GroupBy(Linea => Linea.IdCuenta).Any(Grupo => Grupo.Count() > 1))
            {
                Mensaje = "Una cuenta no puede repetirse en el detalle de la póliza.";
                return false;
            }
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
            if (Estado == EstadoEntidadContabilidad.Modificar && !string.IsNullOrWhiteSpace(Modelo.Periodo) &&
                Modelo.Periodo != ContabilidadFuncObtenerPeriodo(Modelo.Fecha))
            {
                Mensaje = "La fecha de la póliza debe quedar dentro del período " + Modelo.Periodo + ".";
                return false;
            }
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026

            ClsPolizaContabilidad Entidad = new ClsPolizaContabilidad
            {
                IdPoliza = Modelo.IdPoliza, Fecha = Modelo.Fecha.Date, Estado = EstadoPoliza, IdModulo = Modelo.IdModulo,
                // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                IdPais = Modelo.IdPais,
                // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
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
                        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
                        Mensaje = "Póliza anulada correctamente.";
                        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
                        break;
                }
                // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 02/11/2026
                if (Estado != EstadoEntidadContabilidad.Borrar && EstadoPoliza == "VALIDADA" && Modelo.ModoActualizacion == "LINEA")
                // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 02/11/2026
                // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
                {
                    try
                    {
                        _Repositorio.ContabilidadFuncActualizarSaldos("LINEA", "RANGO", Entidad.Fecha, Entidad.Fecha, Modelo.IdPoliza,
                            ClsControladorSeguridadContabilidad.ContabilidadFuncUsuarioSesion());
                        Modelo.Estado = "APLICADA";
                        Mensaje += " Saldos actualizados en línea: la póliza quedó aplicada.";
                    }
                    catch (Exception Excepcion)
                    {
                        Mensaje += " La póliza quedó validada, pero no se pudieron actualizar los saldos: " + Excepcion.Message;
                    }
                }
                // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
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
