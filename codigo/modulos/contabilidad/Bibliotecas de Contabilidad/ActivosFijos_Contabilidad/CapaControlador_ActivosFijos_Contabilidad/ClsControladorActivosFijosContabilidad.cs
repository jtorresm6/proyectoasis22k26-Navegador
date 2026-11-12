/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Reglas de activos fijos: catálogos, validación, grabación según el
 *  estado y envío a Logística. Responsable de la pantalla: Dylan.
 * Reglas especificas:
 *  La validación vive en ClsModeloActivoFijoContabilidad. Solo los
 *  activos del módulo de Logística se le envían, al crearlos. La
 *  depreciación es de línea recta y genera una póliza por mes.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CapaModelo_Contabilidad.Contratos;
// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using CapaModelo_Contabilidad.Entidades;
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using CapaModelo_Contabilidad.Repositorios;

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaControlador_Contabilidad
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public class ClsControladorActivosFijosContabilidad
    {
        // Inicio de cambio: Gabriel Guillén - 0901-23-1998 - 09/11/2026
        private const string _ModuloLogistica = "Logística";
        // Fin de cambio: Gabriel Guillén - 0901-23-1998 - 09/11/2026

        private readonly IRepositorioActivosFijosContabilidad _Repositorio = new ClsRepositorioActivosFijosContabilidad();

        // Inicio de cambio: Gabriel Guillén - 0901-23-1998 - 10/11/2026
        // Cuentas de gasto y de depreciación acumulada por tipo de activo
        private static readonly Dictionary<string, string[]> _CuentasDepreciacion = new Dictionary<string, string[]>
        {
            { "EDIFICIO", new[] { "5.04.04.001", "1.02.07.001" } },
            { "MAQUINARIA", new[] { "5.02.03.001", "1.02.07.002" } },
            { "VEHICULO", new[] { "5.04.04.003", "1.02.07.003" } },
            { "MOBILIARIO", new[] { "5.04.04.004", "1.02.07.004" } },
            { "EQ_COMPUTO", new[] { "5.04.04.005", "1.02.07.005" } }
        };
        // Fin de cambio: Gabriel Guillén - 0901-23-1998 - 10/11/2026

        public string[] ContabilidadFuncModulos()
        {
            return _Repositorio.ContabilidadFuncObtenerModulos();
        }

        public DataTable ContabilidadFuncVidaUtil()
        {
            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("Tipo", typeof(string));
            Tabla.Columns.Add("Nombre", typeof(string));
            Tabla.Columns.Add("Meses", typeof(int));
            foreach (ClsVidaUtilContabilidad Vida in _Repositorio.ContabilidadFuncObtenerVidaUtil())
                Tabla.Rows.Add(Vida.Tipo, Vida.Nombre, Vida.Meses);
            return Tabla;
        }

        // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        public DataTable ContabilidadFuncActivos()
        {
            Dictionary<string, string> Tipos = _Repositorio.ContabilidadFuncObtenerVidaUtil().ToDictionary(Vida => Vida.Tipo, Vida => Vida.Nombre);
            DataTable Tabla = new DataTable();
            Tabla.Columns.Add("IdActivo", typeof(int));
            Tabla.Columns.Add("Codigo", typeof(string));
            Tabla.Columns.Add("Descripcion", typeof(string));
            Tabla.Columns.Add("Modulo", typeof(string));
            Tabla.Columns.Add("TipoCodigo", typeof(string));
            Tabla.Columns.Add("Tipo", typeof(string));
            Tabla.Columns.Add("Adquisicion", typeof(DateTime));
            Tabla.Columns.Add("Inicio", typeof(DateTime));
            Tabla.Columns.Add("Compra", typeof(decimal));
            Tabla.Columns.Add("Residual", typeof(decimal));
            Tabla.Columns.Add("Libros", typeof(decimal));
            // Inicio de cambio: Gabriel Guillén - 0901-23-1998 - 10/11/2026
            Tabla.Columns.Add("Acumulada", typeof(decimal));
            // Fin de cambio: Gabriel Guillén - 0901-23-1998 - 10/11/2026
            Tabla.Columns.Add("Meses", typeof(int));
            foreach (ClsActivoFijoContabilidad Activo in _Repositorio.ContabilidadFuncObtenerTodos())
                Tabla.Rows.Add(Activo.IdActivo, Activo.Codigo, Activo.Descripcion, Activo.Modulo, Activo.Tipo,
                    Tipos.ContainsKey(Activo.Tipo) ? Tipos[Activo.Tipo] : Activo.Tipo, Activo.Adquisicion, Activo.Inicio,
                    Activo.Compra, Activo.Residual, Activo.ValorEnLibros, Activo.Compra - Activo.ValorEnLibros, Activo.Meses);
            return Tabla;
        }
        // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026

        // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        private ClsModeloActivoFijoContabilidad ContabilidadFuncCrearModelo(string Descripcion, DateTime Adquisicion,
        // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            DateTime Inicio, decimal Compra, decimal Residual, int Meses)
        {
            return new ClsModeloActivoFijoContabilidad
            {
                // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                Descripcion = Descripcion, Adquisicion = Adquisicion, Inicio = Inicio,
                // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                Compra = Compra, Residual = Residual, Meses = Meses
            };
        }

        // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        public Dictionary<string, string> ContabilidadFuncErroresActivo(string Descripcion, DateTime Adquisicion,
        // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            DateTime Inicio, decimal Compra, decimal Residual, int Meses)
        {
            return ClsValidacionDatosContabilidad.ContabilidadFuncErrores(
                // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                ContabilidadFuncCrearModelo(Descripcion, Adquisicion, Inicio, Compra, Residual, Meses));
                // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        }

        // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        public string ContabilidadFuncValidarActivo(string Descripcion, DateTime Adquisicion,
        // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            DateTime Inicio, decimal Compra, decimal Residual, int Meses)
        {
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            return ContabilidadFuncErroresActivo(Descripcion, Adquisicion, Inicio, Compra, Residual, Meses).Values.FirstOrDefault() ?? string.Empty;
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        }

        // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
        public bool ContabilidadFuncGrabarCambios(EstadoEntidadContabilidad Estado, ref int IdActivo, string Descripcion, string Modulo,
        // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            string Tipo, DateTime Adquisicion, DateTime Inicio, decimal Compra, decimal Residual, int Meses, out string Mensaje)
        {
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            ClsModeloActivoFijoContabilidad Modelo = ContabilidadFuncCrearModelo(Descripcion, Adquisicion, Inicio, Compra, Residual, Meses);
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            Modelo.IdActivo = IdActivo;
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            Modelo.Modulo = Modulo;
            Modelo.Tipo = Tipo;
            // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            Mensaje = Estado == EstadoEntidadContabilidad.Borrar ? string.Empty : ClsValidacionDatosContabilidad.ContabilidadFuncPrimerError(Modelo);
            if (Mensaje == string.Empty && Estado != EstadoEntidadContabilidad.Borrar && (string.IsNullOrWhiteSpace(Modulo) || string.IsNullOrWhiteSpace(Tipo)))
                Mensaje = "Seleccione el módulo y el tipo de activo.";
            // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            if (Mensaje != string.Empty) return false;

            ClsActivoFijoContabilidad Entidad = new ClsActivoFijoContabilidad
            {
                // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                IdActivo = Modelo.IdActivo, Descripcion = (Modelo.Descripcion ?? string.Empty).Trim(),
                Modulo = Modelo.Modulo, Tipo = Modelo.Tipo, Adquisicion = Modelo.Adquisicion, Inicio = Modelo.Inicio, Compra = Modelo.Compra,
                Residual = Modelo.Residual, ValorEnLibros = Modelo.Compra, Meses = Modelo.Meses
                // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
            };
            try
            {
                switch (Estado)
                {
                    case EstadoEntidadContabilidad.Agregar:
                        _Repositorio.ContabilidadFuncAgregar(Entidad);
                        // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                        IdActivo = Entidad.IdActivo;
                        Mensaje = "Activo " + Entidad.Codigo + " guardado correctamente.";
                        // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                        // Inicio de cambio: Dylan Hernández - 0901-23-519 - 29/10/2026
                        if (Entidad.Modulo == _ModuloLogistica) Mensaje += " " + ContabilidadFuncEnviarLogistica(Entidad);
                        // Fin de cambio: Dylan Hernández - 0901-23-519 - 29/10/2026
                        break;
                    case EstadoEntidadContabilidad.Modificar:
                        // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                        if (_Repositorio.ContabilidadFuncEditar(Entidad) == 0)
                        {
                            Mensaje = "El activo ya no existe o fue eliminado.";
                            return false;
                        }
                        // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                        Mensaje = "Activo actualizado correctamente.";
                        break;
                    case EstadoEntidadContabilidad.Borrar:
                        // Inicio de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                        if (_Repositorio.ContabilidadFuncEliminar(Entidad) == 0)
                        {
                            Mensaje = "El activo ya no existe o fue eliminado.";
                            return false;
                        }
                        IdActivo = 0;
                        // Fin de cambio: Dylan Hernández - 0901-23-519 - 28/10/2026
                        Mensaje = "Activo eliminado correctamente.";
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

        // Inicio de cambio: Dylan Hernández - 0901-23-519 - 29/10/2026
        private string ContabilidadFuncEnviarLogistica(ClsActivoFijoContabilidad Activo)
        {
            try
            {
                IReceptorActivoFijoContabilidad Receptor = ClsEnlaceLogisticaContabilidad.ContabilidadFuncBuscarReceptor();
                if (Receptor == null) return "Logística todavía no publica la recepción de activos; no se le envió.";
                Dictionary<string, string> Tipos = _Repositorio.ContabilidadFuncObtenerVidaUtil().ToDictionary(Vida => Vida.Tipo, Vida => Vida.Nombre);
                ClsModeloActivoLogisticaContabilidad Datos = new ClsModeloActivoLogisticaContabilidad
                {
                    Codigo = Activo.Codigo, Descripcion = Activo.Descripcion, Modulo = Activo.Modulo, CodigoTipo = Activo.Tipo,
                    NombreTipo = Tipos.ContainsKey(Activo.Tipo) ? Tipos[Activo.Tipo] : Activo.Tipo, FechaAdquisicion = Activo.Adquisicion,
                    FechaInicioUso = Activo.Inicio, ValorCompra = Activo.Compra, ValorResidual = Activo.Residual, VidaUtilMeses = Activo.Meses
                };
                string Referencia, Respuesta;
                return Receptor.ContabilidadFuncRecibirActivoFijo(Datos, out Referencia, out Respuesta)
                    ? "Enviado a Logística" + (string.IsNullOrWhiteSpace(Referencia) ? "." : " con referencia " + Referencia + ".")
                    : "Logística no lo recibió: " + Respuesta;
            }
            catch (Exception Excepcion)
            {
                return "No se pudo enviar a Logística: " + Excepcion.Message;
            }
        }
        // Fin de cambio: Dylan Hernández - 0901-23-519 - 29/10/2026

        // Inicio de cambio: Gabriel Guillén - 0901-23-1998 - 10/11/2026
        // Cuota de línea recta del mes
        public static decimal ContabilidadFuncCuotaMensual(decimal Compra, decimal Residual, int Meses, decimal Libros,
            DateTime Inicio, DateTime Mes)
        {
            DateTime FinMes = new DateTime(Mes.Year, Mes.Month, 1).AddMonths(1).AddDays(-1);
            decimal Pendiente = Libros - Residual;
            if (Meses <= 0 || Inicio.Date > FinMes || Pendiente <= 0m) return 0m;
            decimal Cuota = Math.Round((Compra - Residual) / Meses, 2, MidpointRounding.AwayFromZero);
            return Math.Min(Cuota, Pendiente);
        }

        // Revisa que el mes no sea futuro
        private static string ContabilidadFuncValidarMes(DateTime Mes)
        {
            DateTime Actual = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            return new DateTime(Mes.Year, Mes.Month, 1) > Actual ? "No se puede depreciar un mes futuro." : string.Empty;
        }

        // Cuotas del mes de los activos vigentes
        private static List<ClsCuotaDepreciacionContabilidad> ContabilidadFuncCuotas(IEnumerable<ClsActivoFijoContabilidad> Activos, DateTime Mes)
        {
            List<ClsCuotaDepreciacionContabilidad> Cuotas = new List<ClsCuotaDepreciacionContabilidad>();
            foreach (ClsActivoFijoContabilidad Activo in Activos)
            {
                decimal Monto = ContabilidadFuncCuotaMensual(Activo.Compra, Activo.Residual, Activo.Meses, Activo.ValorEnLibros, Activo.Inicio, Mes);
                if (Monto > 0m) Cuotas.Add(new ClsCuotaDepreciacionContabilidad { IdActivo = Activo.IdActivo, Monto = Monto });
            }
            return Cuotas;
        }

        // Resumen antes de depreciar el mes
        public string ContabilidadFuncResumenDepreciacion(DateTime Mes, out int Cantidad, out decimal Total)
        {
            Cantidad = 0;
            Total = 0m;
            string Mensaje = ContabilidadFuncValidarMes(Mes);
            if (Mensaje != string.Empty) return Mensaje;
            string Periodo = Mes.ToString("yyyy-MM");
            try
            {
                if (_Repositorio.ContabilidadFuncPeriodoDepreciado(Periodo)) return "El mes " + Periodo + " ya tiene póliza de depreciación.";
                List<ClsCuotaDepreciacionContabilidad> Cuotas = ContabilidadFuncCuotas(_Repositorio.ContabilidadFuncObtenerTodos(), Mes);
                Cantidad = Cuotas.Count;
                Total = Cuotas.Sum(Cuota => Cuota.Monto);
                return Cantidad == 0 ? "No hay activos por depreciar en " + Periodo + "." : string.Empty;
            }
            catch (Exception Excepcion)
            {
                return Excepcion.Message;
            }
        }

        // Genera la póliza de depreciación del mes
        public bool ContabilidadFuncDepreciarMes(DateTime Mes, out string Mensaje)
        {
            Mensaje = ContabilidadFuncValidarMes(Mes);
            if (Mensaje != string.Empty) return false;
            string Periodo = Mes.ToString("yyyy-MM");
            try
            {
                List<ClsActivoFijoContabilidad> Activos = _Repositorio.ContabilidadFuncObtenerTodos().ToList();
                List<ClsCuotaDepreciacionContabilidad> Cuotas = ContabilidadFuncCuotas(Activos, Mes);
                if (Cuotas.Count == 0)
                {
                    Mensaje = "No hay activos por depreciar en " + Periodo + ".";
                    return false;
                }
                Dictionary<int, string> TipoActivo = Activos.ToDictionary(Activo => Activo.IdActivo, Activo => Activo.Tipo);
                List<string> SinCuenta = Cuotas.Select(Cuota => TipoActivo[Cuota.IdActivo]).Distinct()
                    .Where(Tipo => !_CuentasDepreciacion.ContainsKey(Tipo)).ToList();
                if (SinCuenta.Count > 0)
                {
                    Mensaje = "Estos tipos de activo no tienen cuentas de depreciación: " + string.Join(", ", SinCuenta) + ".";
                    return false;
                }
                Dictionary<string, string> Nombres = _Repositorio.ContabilidadFuncObtenerVidaUtil().ToDictionary(Vida => Vida.Tipo, Vida => Vida.Nombre);
                List<ClsLineaDepreciacionContabilidad> Lineas = new List<ClsLineaDepreciacionContabilidad>();
                foreach (IGrouping<string, ClsCuotaDepreciacionContabilidad> Grupo in Cuotas.GroupBy(Cuota => TipoActivo[Cuota.IdActivo]).OrderBy(Grupo => Grupo.Key))
                {
                    decimal Monto = Grupo.Sum(Cuota => Cuota.Monto);
                    string Nombre = Nombres.ContainsKey(Grupo.Key) ? Nombres[Grupo.Key] : Grupo.Key;
                    Lineas.Add(new ClsLineaDepreciacionContabilidad { CodigoCuenta = _CuentasDepreciacion[Grupo.Key][0], Debe = Monto,
                        Descripcion = "Depreciación " + Periodo + " - " + Nombre });
                    Lineas.Add(new ClsLineaDepreciacionContabilidad { CodigoCuenta = _CuentasDepreciacion[Grupo.Key][1], Haber = Monto,
                        Descripcion = "Depreciación acumulada " + Periodo + " - " + Nombre });
                }
                DateTime FinMes = new DateTime(Mes.Year, Mes.Month, 1).AddMonths(1).AddDays(-1);
                string Codigo = _Repositorio.ContabilidadFuncRegistrarDepreciacion(Periodo, FinMes,
                    "Depreciación mensual de activos fijos " + Periodo, Cuotas, Lineas);
                Mensaje = "Póliza " + Codigo + " generada: " + Cuotas.Count + " activo(s) por Q " + Cuotas.Sum(Cuota => Cuota.Monto).ToString("#,##0.00") +
                    ". Queda VALIDADA para actualizar saldos en Pólizas.";
                return true;
            }
            catch (Exception Excepcion)
            {
                Mensaje = Excepcion.Message;
                return false;
            }
        }
        // Fin de cambio: Gabriel Guillén - 0901-23-1998 - 10/11/2026
    }
}
