using CapaControlador_Seguridad;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaModelo_Asistencia.Entidades;
using CapaModelo_Asistencia.Repositorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * Última modificación : 10/11/2026
 * ==================================================================
 * Propósito :
 *  Controlador de la ventana 9102 de Horas Extra, que se abre desde
 *  Asistencia. Calcula el pago (valor de la hora ordinaria por la
 *  duración y el recargo). La hora ordinaria es el salario de
 *  tblEmpleado / 30 / horas de la jornada (8 diurna, 7 mixta y 6
 *  nocturna, arts. 116 y 117 del Código de Trabajo) y la hora extra no
 *  deja pasar de 12 horas en el día (art. 122). Valida contra la asistencia de donde sale
 *  la hora extra y graba en tblHoraExtra. Al aprobar o rechazar guarda
 *  quién lo hizo y cuándo. Lo que Nómina ya tomó no se puede cambiar.
 * ===================================================================
*/

namespace CapaControlador_Asistencia.Modelos_de_Controladores
{
    public class ClsModeloHoraExtra
    {
        public const int IdModulo = 9000;
        public const int IdAplicacion = 9102;
        // Tope absoluto (jornada nocturna de 6 h + 6 h extra = 12 h), igual que el CHECK de la tabla
        public const int DuracionMaxima = 360;
        public const decimal RecargoPorDefecto = 0.50m;
        // Código de Trabajo, art. 122: jornada ordinaria más extraordinaria no pasa de 12 horas al día
        public const int HorasMaximasDia = 12;
        private const int DiasMes = 30;

        private static readonly string[] EstadosEditables = { "PENDIENTE", "APROBADA", "RECHAZADA" };

        private int _IdHoraExtra;
        private int _IdPercepcionDeduccion;
        private string _NombrePercepcionDeduccion;
        private int _IdEmpleado;
        private string _CodigoEmpleado;
        private string _NombreEmpleado;
        private int _IdAsistenciaEmpleado;
        private DateTime _FechaAsistencia;
        private TimeSpan? _HoraInicioJornada;
        private TimeSpan? _HoraFinJornada;
        private TimeSpan? _HoraSalida;
        private int _Duracion;
        private decimal _ValorHora;
        private decimal _PorcentajeRecargo = RecargoPorDefecto;
        private decimal _TotalPago;
        private string _EstadoHoraExtra = "Pendiente";
        private int? _IdUsuarioAprueba;
        private string _UsuarioAprueba;
        private DateTime? _FechaAprobacion;
        private int? _IdNominaEncabezado;
        private string _Observaciones;

        private ClsRepositorioHoraExtra _RepositorioHoraExtra;
        private ClsPermisoAplicacion _PermisosUsuario;

        public EstadoEntidad Estado { private get; set; }
        public bool Grabado { get; private set; }

        public int IdHoraExtra { get => _IdHoraExtra; set => _IdHoraExtra = value; }
        public int IdPercepcionDeduccion { get => _IdPercepcionDeduccion; set => _IdPercepcionDeduccion = value; }
        public string NombrePercepcionDeduccion { get => _NombrePercepcionDeduccion; set => _NombrePercepcionDeduccion = value; }
        public int IdEmpleado { get => _IdEmpleado; set => _IdEmpleado = value; }
        public string CodigoEmpleado { get => _CodigoEmpleado; set => _CodigoEmpleado = value; }
        public string NombreEmpleado { get => _NombreEmpleado; set => _NombreEmpleado = value; }
        public int IdAsistenciaEmpleado { get => _IdAsistenciaEmpleado; set => _IdAsistenciaEmpleado = value; }
        public DateTime FechaAsistencia { get => _FechaAsistencia; set => _FechaAsistencia = value; }
        public TimeSpan? HoraInicioJornada { get => _HoraInicioJornada; set => _HoraInicioJornada = value; }
        public TimeSpan? HoraFinJornada { get => _HoraFinJornada; set => _HoraFinJornada = value; }
        public TimeSpan? HoraSalida { get => _HoraSalida; set => _HoraSalida = value; }
        // En minutos, como en la tabla
        public int Duracion { get => _Duracion; set => _Duracion = value; }
        public decimal ValorHora { get => _ValorHora; set => _ValorHora = value; }
        // 0.50 = 50 %
        public decimal PorcentajeRecargo { get => _PorcentajeRecargo; set => _PorcentajeRecargo = value; }
        public decimal TotalPago { get => _TotalPago; private set => _TotalPago = value; }
        // En la vista va como "Pendiente", en la BD como "PENDIENTE"
        public string EstadoHoraExtra { get => _EstadoHoraExtra; set => _EstadoHoraExtra = value; }
        public int? IdUsuarioAprueba { get => _IdUsuarioAprueba; private set => _IdUsuarioAprueba = value; }
        public string UsuarioAprueba { get => _UsuarioAprueba; private set => _UsuarioAprueba = value; }
        public DateTime? FechaAprobacion { get => _FechaAprobacion; private set => _FechaAprobacion = value; }
        public int? IdNominaEncabezado { get => _IdNominaEncabezado; private set => _IdNominaEncabezado = value; }
        public string Observaciones { get => _Observaciones; set => _Observaciones = value; }

        // Ya la tomó Nómina o ya se pagó
        public bool Bloqueada => _IdNominaEncabezado.HasValue || string.Equals(_EstadoHoraExtra, "Pagada", StringComparison.OrdinalIgnoreCase);

        public ClsModeloHoraExtra()
        {
            _RepositorioHoraExtra = new ClsRepositorioHoraExtra();
        }

        // ---------- Cálculos ----------

        // Horas ordinarias de la jornada según el Código de Trabajo (arts. 116 y 117):
        // diurna 8 (entre 6:00 y 18:00), nocturna 6 (entre 18:00 y 6:00) y mixta 7.
        // Si la mixta tiene 4 horas o más de noche se toma como nocturna.
        public static int RrhhMetHorasJornadaLegal(TimeSpan Inicio, TimeSpan Fin)
        {
            int Desde = (int)Inicio.TotalMinutes;
            int Hasta = (int)Fin.TotalMinutes;
            if (Hasta <= Desde) Hasta += 1440;   // la jornada pasa de medianoche

            int Total = Hasta - Desde, Noche = 0;
            for (int Minuto = Desde; Minuto < Hasta; Minuto++)
            {
                int Reloj = Minuto % 1440;
                if (Reloj < 360 || Reloj >= 1080) Noche++;
            }
            if (Noche == 0) return 8;
            if (Noche == Total || Noche >= 240) return 6;
            return 7;
        }

        public static decimal RrhhMetCalcularValorHora(decimal SalarioMensual, int HorasJornada)
        {
            if (SalarioMensual <= 0 || HorasJornada <= 0) return 0m;
            return Math.Round(SalarioMensual / DiasMes / HorasJornada, 4);
        }

        // Lo que le falta a la jornada para llegar a 12 horas: diurna 240, mixta 300, nocturna 360
        public static int RrhhMetTopeExtra(int HorasJornada)
        {
            return Math.Min(DuracionMaxima, Math.Max(0, (HorasMaximasDia - HorasJornada) * 60));
        }

        public static decimal RrhhMetCalcularTotal(int DuracionMinutos, decimal ValorHora, decimal Recargo)
        {
            if (DuracionMinutos <= 0 || ValorHora <= 0) return 0m;
            return Math.Round(DuracionMinutos / 60m * ValorHora * (1 + Recargo), 2);
        }

        // Minutos trabajados después del fin de la jornada, sin pasar de las 12 horas del día
        public static int RrhhMetDuracionSugerida(TimeSpan HoraSalida, TimeSpan HoraInicioJornada, TimeSpan HoraFinJornada)
        {
            int Minutos = (int)(HoraSalida - HoraFinJornada).TotalMinutes;
            int Tope = RrhhMetTopeExtra(RrhhMetHorasJornadaLegal(HoraInicioJornada, HoraFinJornada));
            return Math.Max(0, Math.Min(Tope, Minutos));
        }

        // ---------- Grabar ----------

        public string RrhhMetGrabarCambios()
        {
            string Mensaje = null;
            Grabado = false;
            try
            {
                // Primero los datos, luego el permiso y al final lo que se revisa en la BD
                if (Estado != EstadoEntidad.Deleted)
                {
                    string ErrorDatos = RrhhMetValidarDatos();
                    if (ErrorDatos != null) return ErrorDatos;
                }

                string SinPermiso = RrhhMetRevisarPermiso();
                if (SinPermiso != null) return SinPermiso;

                ClsHoraExtra Anterior = null;
                if (Estado != EstadoEntidad.Added)
                {
                    Anterior = _RepositorioHoraExtra.RrhhMetBuscarPorId(_IdHoraExtra);
                    if (Anterior == null)
                        return "La hora extra ya no existe, refresque la lista.";
                    if (Anterior.IdNominaEncabezado.HasValue || Anterior.EstadoHoraExtra == "PAGADA")
                        return "Esta hora extra ya fue tomada por Nómina, no se puede modificar ni eliminar.";
                }

                if (Estado == EstadoEntidad.Deleted)
                {
                    if (Anterior.EstadoHoraExtra == "APROBADA")
                        return "Una hora extra aprobada no se elimina; cámbiela a Rechazada si ya no aplica.";
                    if (_RepositorioHoraExtra.RrhhMetRemover(new ClsHoraExtra { IdHoraExtra = _IdHoraExtra }) == 0)
                        return "Esta hora extra ya fue tomada por Nómina, no se puede eliminar.";
                    Grabado = true;
                    return "La hora extra se eliminó correctamente.";
                }

                string Error = RrhhMetValidarAsistencia();
                if (Error != null) return Error;

                string EstadoBD = _EstadoHoraExtra.Trim().ToUpperInvariant();
                _TotalPago = RrhhMetCalcularTotal(_Duracion, _ValorHora, _PorcentajeRecargo);

                // Quién aprobó o rechazó: se guarda solo cuando cambia el estado
                bool CambioEstado = Anterior == null || Anterior.EstadoHoraExtra != EstadoBD;
                if (EstadoBD == "PENDIENTE")
                {
                    _IdUsuarioAprueba = null;
                    _FechaAprobacion = null;
                }
                else if (CambioEstado)
                {
                    _IdUsuarioAprueba = ClsSesionSeguridad.IdUsuario > 0 ? ClsSesionSeguridad.IdUsuario : (int?)null;
                    _FechaAprobacion = DateTime.Now;
                }
                else
                {
                    _IdUsuarioAprueba = Anterior.IdUsuarioAprueba;
                    _FechaAprobacion = Anterior.FechaAprobacionHoraExtra;
                }

                var ModeloDatos = new ClsHoraExtra
                {
                    IdHoraExtra = _IdHoraExtra,
                    IdPercepcionDeduccion = _IdPercepcionDeduccion,
                    IdEmpleado = _IdEmpleado,
                    IdAsistenciaEmpleado = _IdAsistenciaEmpleado,
                    DuracionHoraExtra = _Duracion,
                    ValorHoraOrdinariaHoraExtra = _ValorHora,
                    PorcentajeRecargoHoraExtra = _PorcentajeRecargo,
                    TotalPagoHoraExtra = _TotalPago,
                    EstadoHoraExtra = EstadoBD,
                    IdUsuarioAprueba = _IdUsuarioAprueba,
                    FechaAprobacionHoraExtra = _FechaAprobacion,
                    ObservacionHoraExtra = (_Observaciones ?? "").Trim()
                };

                if (Estado == EstadoEntidad.Added)
                {
                    _IdHoraExtra = _RepositorioHoraExtra.RrhhMetAgregar(ModeloDatos);
                    Mensaje = "La hora extra se guardó correctamente.";
                }
                else
                {
                    if (_RepositorioHoraExtra.RrhhMetEditar(ModeloDatos) == 0)
                        return "Esta hora extra ya fue tomada por Nómina, no se puede modificar.";
                    Mensaje = "La hora extra se modificó correctamente.";
                }
                Grabado = true;
            }
            catch (OdbcException OdbcEx)
            {
                int Codigo = OdbcEx.Errors.Count > 0 ? OdbcEx.Errors[0].NativeError : 0;
                if (Codigo == 1062)
                    Mensaje = "Esa asistencia ya tiene una hora extra registrada.";
                else if (Codigo == 1452)
                    Mensaje = "El concepto o la asistencia seleccionada ya no existe, refresque la ventana.";
                else if (Codigo == 3819 || Codigo == 4025)
                    Mensaje = $"La duración debe estar entre 1 y {DuracionMaxima} minutos y el recargo no puede ser negativo.";
                else
                    Mensaje = ClsModeloAsistencia.RrhhMetMensajeError(OdbcEx);
            }
            catch (Exception Ex)
            {
                Mensaje = ClsModeloAsistencia.RrhhMetMensajeError(Ex);
            }
            return Mensaje;
        }

        // Reglas que no necesitan la BD; devuelve null si todo está bien
        public string RrhhMetValidarDatos()
        {
            if (_IdEmpleado <= 0)
                return "Debe seleccionar un empleado.";
            if (_IdAsistenciaEmpleado <= 0)
                return "Seleccione la asistencia de donde sale la hora extra.";
            if (_IdPercepcionDeduccion <= 0)
                return "Seleccione el concepto de pago.";
            if (_Duracion <= 0)
                return "La duración debe ser mayor a 0 minutos.";
            if (_Duracion > DuracionMaxima)
                return $"La duración no puede pasar de {DuracionMaxima} minutos.";
            if (_ValorHora <= 0)
                return "El empleado no tiene salario registrado, no se puede calcular el valor de la hora.";
            if (_PorcentajeRecargo < 0 || _PorcentajeRecargo > 9.99m)
                return "El recargo debe estar entre 0 % y 999 %.";
            string EstadoBD = (_EstadoHoraExtra ?? "").Trim().ToUpperInvariant();
            if (Array.IndexOf(EstadosEditables, EstadoBD) < 0)
                return "Seleccione un estado válido.";
            if (EstadoBD == "RECHAZADA" && string.IsNullOrWhiteSpace(_Observaciones))
                return "Escriba en observaciones el motivo del rechazo.";
            if ((_Observaciones ?? "").Trim().Length > 255)
                return "Las observaciones no pueden pasar de 255 caracteres.";
            return null;
        }

        // La asistencia tiene que seguir siendo válida: del mismo empleado y con salida después de la jornada
        private string RrhhMetValidarAsistencia()
        {
            ClsAsistenciaHoraExtra Asistencia = null;
            foreach (ClsAsistenciaHoraExtra Item in _RepositorioHoraExtra.RrhhMetObtenerAsistenciasConExtra())
            {
                if (Item.IdAsistenciaEmpleado == _IdAsistenciaEmpleado) { Asistencia = Item; break; }
            }
            if (Asistencia == null || Asistencia.IdEmpleado != _IdEmpleado)
                return "La asistencia seleccionada ya no genera hora extra (revise su estado u hora de salida).";
            if (Asistencia.IdHoraExtra.HasValue && Asistencia.IdHoraExtra.Value != _IdHoraExtra)
                return "Esa asistencia ya tiene una hora extra registrada.";

            int Disponible = RrhhMetDuracionSugerida(Asistencia.HoraSalidaAsistenciaEmpleado,
                                                     Asistencia.HoraInicioJornada, Asistencia.HoraFinJornada);
            if (_Duracion > Disponible)
                return $"La duración no puede pasar de {Disponible} minutos (lo trabajado después de su jornada, sin pasar de 12 horas en el día).";
            return null;
        }

        // ---------- Permisos ----------

        public ClsPermisoAplicacion RrhhMetPermisosUsuario()
        {
            if (_PermisosUsuario != null) return _PermisosUsuario;
            try
            {
                var ModeloPermisos = new ClsModeloAsigAppPerf();
                ModeloPermisos.SeguridadMetObtenerTodos();
                _PermisosUsuario = ModeloPermisos.SeguridadMetObtenerPermisosSesion(IdModulo, IdAplicacion);
            }
            catch (Exception)
            {
                _PermisosUsuario = new ClsPermisoAplicacion();
            }
            return _PermisosUsuario;
        }

        private string RrhhMetRevisarPermiso()
        {
            ClsPermisoAplicacion Permisos = RrhhMetPermisosUsuario();
            if (!Permisos.TieneAcceso)
                return "No tiene acceso a la ventana de Horas Extra.";
            if (Estado == EstadoEntidad.Added && !Permisos.PuedeInsertar)
                return "No tiene permiso para registrar horas extra.";
            if (Estado == EstadoEntidad.Modified && !Permisos.PuedeEditar)
                return "No tiene permiso para modificar horas extra.";
            if (Estado == EstadoEntidad.Deleted && !Permisos.PuedeEliminar)
                return "No tiene permiso para eliminar horas extra.";
            return null;
        }

        // ---------- Consultas para la vista ----------

        public List<ClsModeloHoraExtra> RrhhMetObtenerTodos()
        {
            var Lista = new List<ClsModeloHoraExtra>();
            foreach (ClsHoraExtra Item in _RepositorioHoraExtra.RrhhMetObtenerTodos())
            {
                Lista.Add(new ClsModeloHoraExtra
                {
                    _IdHoraExtra = Item.IdHoraExtra,
                    _IdPercepcionDeduccion = Item.IdPercepcionDeduccion,
                    _NombrePercepcionDeduccion = Item.NombrePercepcionDeduccion,
                    _IdEmpleado = Item.IdEmpleado,
                    _CodigoEmpleado = Item.CodigoEmpleado,
                    _NombreEmpleado = Item.NombreEmpleado,
                    _IdAsistenciaEmpleado = Item.IdAsistenciaEmpleado,
                    _FechaAsistencia = Item.FechaAsistenciaEmpleado,
                    _HoraInicioJornada = Item.HoraInicioJornada,
                    _HoraFinJornada = Item.HoraFinJornada,
                    _HoraSalida = Item.HoraSalidaAsistenciaEmpleado,
                    _Duracion = Item.DuracionHoraExtra,
                    _ValorHora = Item.ValorHoraOrdinariaHoraExtra,
                    _PorcentajeRecargo = Item.PorcentajeRecargoHoraExtra,
                    _TotalPago = Item.TotalPagoHoraExtra,
                    _EstadoHoraExtra = RrhhMetEstadoVista(Item.EstadoHoraExtra),
                    _IdUsuarioAprueba = Item.IdUsuarioAprueba,
                    _UsuarioAprueba = Item.NombreUsuarioAprueba,
                    _FechaAprobacion = Item.FechaAprobacionHoraExtra,
                    _IdNominaEncabezado = Item.IdNominaEncabezado,
                    _Observaciones = Item.ObservacionHoraExtra
                });
            }
            return Lista;
        }

        // Las columnas van con los mismos nombres que usa la vista
        public DataTable RrhhMetObtenerAsistencias()
        {
            var Tabla = new DataTable();
            Tabla.Columns.Add("idAsistencia", typeof(int));
            Tabla.Columns.Add("idEmpleado", typeof(int));
            Tabla.Columns.Add("fecha", typeof(DateTime));
            Tabla.Columns.Add("textoFecha", typeof(string));
            Tabla.Columns.Add("horaEntrada", typeof(TimeSpan));
            Tabla.Columns.Add("horaSalida", typeof(TimeSpan));
            Tabla.Columns.Add("jornada", typeof(string));
            Tabla.Columns.Add("horaInicioJornada", typeof(TimeSpan));
            Tabla.Columns.Add("horaFinJornada", typeof(TimeSpan));
            Tabla.Columns.Add("valorHora", typeof(decimal));
            Tabla.Columns.Add("minutosExtra", typeof(int));
            Tabla.Columns.Add("idHoraExtra", typeof(int));

            foreach (ClsAsistenciaHoraExtra Item in _RepositorioHoraExtra.RrhhMetObtenerAsistenciasConExtra())
            {
                int HorasJornada = RrhhMetHorasJornadaLegal(Item.HoraInicioJornada, Item.HoraFinJornada);
                int Minutos = RrhhMetDuracionSugerida(Item.HoraSalidaAsistenciaEmpleado, Item.HoraInicioJornada, Item.HoraFinJornada);
                Tabla.Rows.Add(Item.IdAsistenciaEmpleado, Item.IdEmpleado, Item.FechaAsistenciaEmpleado.Date,
                               $"{Item.FechaAsistenciaEmpleado:dd/MM/yyyy}  ({Minutos} min extra)",
                               Item.HoraEntradaAsistenciaEmpleado.HasValue ? (object)Item.HoraEntradaAsistenciaEmpleado.Value : DBNull.Value,
                               Item.HoraSalidaAsistenciaEmpleado,
                               $"{Item.NombreJornada} ({Item.HoraInicioJornada:hh\\:mm} - {Item.HoraFinJornada:hh\\:mm})",
                               Item.HoraInicioJornada, Item.HoraFinJornada,
                               RrhhMetCalcularValorHora(Item.SalarioEmpleado, HorasJornada), Minutos,
                               Item.IdHoraExtra.HasValue ? (object)Item.IdHoraExtra.Value : DBNull.Value);
            }
            return Tabla;
        }

        public DataTable RrhhMetObtenerConceptos()
        {
            return _RepositorioHoraExtra.RrhhMetObtenerConceptos();
        }

        public DataTable RrhhMetObtenerEmpleados()
        {
            return new ClsModeloAsistencia().RrhhMetObtenerEmpleados();
        }

        private static string RrhhMetEstadoVista(string EstadoBD)
        {
            if (string.IsNullOrEmpty(EstadoBD)) return "";
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(EstadoBD.ToLowerInvariant());
        }
    }
}
