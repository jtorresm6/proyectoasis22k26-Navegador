/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander
 * Carné : 0901-23-3747
 * Fecha : 11/10/2026
 * Última modificación : 10/11/2026
 * ==================================================================
 * Propósito :
 *  Formulario 9101 de Asistencia. Muestra el detalle de la asistencia
 *  de un empleado (sus datos, el registro del día y un resumen) y la
 *  lista de asistencias registradas con sus filtros. El empleado se
 *  busca por código o por nombre y sus datos se llenan solos.
 * Reglas especificas:
 *  Guarda, modifica y elimina en tblAsistenciaEmpleado por medio de
 *  CapaControlador_Asistencia. Un empleado solo tiene una asistencia
 *  por fecha. Si el empleado tiene un permiso aprobado por días o
 *  vacaciones para la fecha, el estado queda en Permiso y no se puede
 *  cambiar. Un permiso por horas quita la tardanza o la salida antes
 *  de tiempo que cubre. Los días de capacitación cuentan como día
 *  trabajado. No se registra asistencia desde la fecha de baja. Las
 *  horas trabajadas y las incidencias se calculan con la jornada y la
 *  tolerancia del empleado. Los botones Guardar, Modificar, Eliminar
 *  y Reporte se habilitan según los permisos del usuario en sesión
 *  (módulo 9000, aplicación 9101). El botón Horas extra abre la
 *  ventana 9102 con la asistencia seleccionada. Reporte muestra
 *  RpAsistencia.rdlc con lo que la lista tiene filtrado, sin pasar por
 *  el Reporteador. Ayuda abre su página en RrhhAyudas.chm.
 *  Es ventana secundaria, no se puede maximizar.
 * ===================================================================
*/

using CapaControlador_Asistencia.Modelos_de_Controladores;
using CapaControlador_Seguridad;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaVista_Asistencia.frmReportes;
using CapaVista_Seguridad.Ayudas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CapaVista_Asistencia.Formularios
{
    public partial class FrmAsistencia : Form
    {
        private readonly ClsModeloAsistencia _Asistencia = new ClsModeloAsistencia();
        private ClsPermisoAplicacion _MisPermisos;

        private DataTable _Empleados;
        private DataTable _Permisos;
        private DataTable _Capacitaciones;
        private DataTable _Asistencias;
        private DataView _Vista;

        private int? _IdSeleccionado;
        private int? _IdEmpleado;
        private bool _LlenandoEmpleado;
        private bool _HayCambios;

        private bool _CargandoRegistro;

        private readonly Dictionary<Button, Image[]> _ImagenesBotones = new Dictionary<Button, Image[]>();
        private readonly HashSet<Button> _BotonesSinPermiso = new HashSet<Button>();

        private readonly HashSet<Control> _CamposConError = new HashSet<Control>();
        private Dictionary<Control, Label> _EtiquetasError;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr Ventana, int Mensaje, IntPtr Parametro1, string Parametro2);

        public FrmAsistencia()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Load += FrmAsistencia_Load;
            this.Shown += FrmAsistencia_Shown;
            this.FormClosing += FrmAsistencia_FormClosing;
        }

        private void FrmAsistencia_Load(object sender, EventArgs e)
        {
            RrhhMetPrepararImagenes();
            RrhhMetAplicarPermisos();
            RrhhMetRedondear(RrhhPnlDetalle, 14);
            RrhhMetRedondear(RrhhPnlRegistros, 14);

            SendMessage(RrhhTxtBuscar.Handle, 0x1501, (IntPtr)1, "Buscar por nombre o código...");

            _EtiquetasError = new Dictionary<Control, Label>
            {
                { RrhhTxtEmpleado, RrhhLblErrorEmpleado },
                { RrhhDtpFecha, RrhhLblErrorFecha },
                { RrhhDtpHoraSalida, RrhhLblErrorHoraSalida },
                { RrhhCboEstado, RrhhLblErrorEstado }
            };

            RrhhMetCrearTablas();

            RrhhDtpDesde.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            RrhhDtpHasta.Value = RrhhDtpDesde.Value.AddMonths(1).AddDays(-1);
            RrhhCboFiltroEstado.SelectedIndex = 0;

            _Vista = new DataView(_Asistencias);
            RrhhDgvAsistencias.DataSource = _Vista;
            RrhhMetCargarDatos();
            RrhhMetAplicarFiltro();
        }

        private void FrmAsistencia_Shown(object sender, EventArgs e)
        {
            if (!_MisPermisos.TieneAcceso)
            {
                MessageBox.Show("No tiene acceso a la ventana de Asistencia. Consulte con el administrador del sistema.",
                                "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _HayCambios = false;
                this.Close();
                return;
            }
            RrhhMetNuevoRegistro();
        }

        private void FrmAsistencia_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_HayCambios) return;
            DialogResult Respuesta = MessageBox.Show("Hay cambios sin guardar. ¿Desea salir de todas formas?", "Asistencia",
                                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (Respuesta != DialogResult.Yes) e.Cancel = true;
        }

        // Permisos por botón con el helper de Seguridad, como las demás ventanas de RRHH
        private void RrhhMetAplicarPermisos()
        {
            var MapaBotones = new Dictionary<Control, TipoPermiso>
            {
                { RrhhBtnGuardar, TipoPermiso.Insertar },
                { RrhhBtnModificar, TipoPermiso.Editar },
                { RrhhBtnEliminar, TipoPermiso.Eliminar },
                { RrhhBtnReporte, TipoPermiso.Imprimir }
            };
            _MisPermisos = ClsSeguridadFormHelper.SeguridadMetInicializarSeguridad(
                this, ClsModeloAsistencia.IdModulo, ClsModeloAsistencia.IdAplicacion, MapaBotones);

            // Los que el helper dejó apagados no los vuelve a encender el estado de la ventana
            foreach (Control Boton in MapaBotones.Keys)
            {
                if (!Boton.Enabled) RrhhMetDeshabilitarBoton((Button)Boton);
            }

            // Horas Extra tiene sus propios permisos (aplicación 9102)
            if (!ClsSeguridadFormHelper.SeguridadMetTieneAcceso(ClsModeloHoraExtra.IdModulo, ClsModeloHoraExtra.IdAplicacion))
                RrhhMetDeshabilitarBoton(RrhhBtnHorasExtra);
        }

        // ---------- Reportes ----------

        // Datos del reporte: lo que la lista tiene filtrado en este momento
        private DataTable RrhhMetDatosReporte()
        {
            var Datos = new DataTable("DsAsistencia");
            Datos.Columns.Add("Codigo", typeof(string));
            Datos.Columns.Add("Empleado", typeof(string));
            Datos.Columns.Add("Departamento", typeof(string));
            Datos.Columns.Add("Fecha", typeof(DateTime));
            Datos.Columns.Add("Entrada", typeof(string));
            Datos.Columns.Add("Salida", typeof(string));
            Datos.Columns.Add("HorasTrabajadas", typeof(string));
            Datos.Columns.Add("Estado", typeof(string));
            Datos.Columns.Add("Observaciones", typeof(string));
            Datos.Columns.Add("Periodo", typeof(string));

            string Periodo = $"Del {RrhhDtpDesde.Value:dd/MM/yyyy} al {RrhhDtpHasta.Value:dd/MM/yyyy}";
            if (RrhhCboFiltroEstado.SelectedIndex > 0) Periodo += $"  ·  Estado: {RrhhCboFiltroEstado.SelectedItem}";
            foreach (DataRowView Fila in _Vista)
            {
                Datos.Rows.Add(Fila["codigoEmpleado"], Fila["nombreEmpleado"], Fila["departamentoEmpleado"],
                               Fila["fechaAsistencia"], RrhhMetTextoHora(Fila["horaEntrada"]), RrhhMetTextoHora(Fila["horaSalida"]),
                               Fila["horasTrabajadas"], Fila["estadoAsistencia"],
                               Fila["observacionesAsistencia"].ToString() == "" ? "-" : Fila["observacionesAsistencia"], Periodo);
            }
            return Datos;
        }

        private static string RrhhMetTextoHora(object Hora)
        {
            return Hora is TimeSpan Valor ? Valor.ToString(@"hh\:mm") : "-";
        }

        // Reporte: igual que en Seguridad, abre la ventana del reporte .rdlc con lo que la lista tiene filtrado
        private void RrhhBtnReporte_Click(object sender, EventArgs e)
        {
            try
            {
                var Reporte = new FrmReporteAsistencia(RrhhMetDatosReporte());
                Reporte.Show();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error al abrir el reporte: " + Ex.Message, "Reporte",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Abre la ayuda de esta ventana dentro del .chm de RRHH, igual que en Seguridad
        private void RrhhBtnAyuda_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "C:/RrhhAyudas/RrhhAyudas.chm", "Ayudas_Asistencia.html");
        }

        private void RrhhBtnHorasExtra_Click(object sender, EventArgs e)
        {
            using (var HorasExtra = new FrmHorasExtra(_IdSeleccionado))
            {
                HorasExtra.ShowDialog(this);
            }
        }

        private void RrhhMetCargarDatos()
        {
            try
            {
                _Empleados = _Asistencia.RrhhMetObtenerEmpleados();
                _Permisos = _Asistencia.RrhhMetObtenerPermisos();
                _Capacitaciones = _Asistencia.RrhhMetObtenerCapacitaciones();
                RrhhMetLlenarEmpleados();
                RrhhMetCargarAsistencias();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("No se pudieron cargar los datos de asistencia.\n" + ClsModeloAsistencia.RrhhMetMensajeError(Ex), "Asistencia",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RrhhMetCargarAsistencias()
        {
            _CargandoRegistro = true;
            _Asistencias.BeginLoadData();
            _Asistencias.Clear();
            foreach (ClsModeloAsistencia Item in _Asistencia.RrhhMetObtenerTodos())
            {
                object Entrada = Item.HoraEntrada.HasValue ? (object)Item.HoraEntrada.Value : DBNull.Value;
                object Salida = Item.HoraSalida.HasValue ? (object)Item.HoraSalida.Value : DBNull.Value;
                _Asistencias.Rows.Add(Item.IdAsistenciaEmpleado, Item.IdEmpleado, Item.CodigoEmpleado, Item.NombreEmpleado,
                                      Item.DepartamentoEmpleado, Item.PuestoEmpleado, Item.FechaAsistencia.Date,
                                      Entrada, Salida, Item.EstadoAsistencia, Item.Observaciones,
                                      Item.CreatedAt, Item.UpdatedAt, RrhhMetHorasTrabajadas(Entrada, Salida));
            }
            _Asistencias.EndLoadData();
            _CargandoRegistro = false;
        }

        private void RrhhMetCrearTablas()
        {
            _Asistencias = new DataTable();
            _Asistencias.Columns.Add("idAsistencia", typeof(int));
            _Asistencias.Columns.Add("idEmpleado", typeof(int));
            _Asistencias.Columns.Add("codigoEmpleado", typeof(string));
            _Asistencias.Columns.Add("nombreEmpleado", typeof(string));
            _Asistencias.Columns.Add("departamentoEmpleado", typeof(string));
            _Asistencias.Columns.Add("puestoEmpleado", typeof(string));
            _Asistencias.Columns.Add("fechaAsistencia", typeof(DateTime));
            _Asistencias.Columns.Add("horaEntrada", typeof(TimeSpan));
            _Asistencias.Columns.Add("horaSalida", typeof(TimeSpan));
            _Asistencias.Columns.Add("estadoAsistencia", typeof(string));
            _Asistencias.Columns.Add("observacionesAsistencia", typeof(string));
            _Asistencias.Columns.Add("created_at", typeof(DateTime));
            _Asistencias.Columns.Add("updated_at", typeof(DateTime));
            _Asistencias.Columns.Add("horasTrabajadas", typeof(string));
        }

        private DataRow RrhhMetBuscarEmpleado(int IdEmpleado)
        {
            if (_Empleados == null) return null;
            DataRow[] Filas = _Empleados.Select("idEmpleado = " + IdEmpleado);
            return Filas.Length > 0 ? Filas[0] : null;
        }

        private static int RrhhMetTolerancia(DataRow Empleado)
        {
            return Empleado["minutosTolerancia"] == DBNull.Value ? 0 : (int)Empleado["minutosTolerancia"];
        }

        private DataRow RrhhMetEmpleadoActual()
        {
            return _IdEmpleado == null ? null : RrhhMetBuscarEmpleado(_IdEmpleado.Value);
        }

        // Unidad DIAS: permisos por días y vacaciones; HORAS: permisos de unas horas
        private DataRow[] RrhhMetBuscarPermisos(int IdEmpleado, DateTime Fecha, string Unidad)
        {
            if (_Permisos == null) return new DataRow[0];
            string Dia = Fecha.Date.ToString("MM/dd/yyyy");
            return _Permisos.Select($"idEmpleado = {IdEmpleado} AND unidadPermiso = '{Unidad}' " +
                                    $"AND fechaInicioPermiso <= #{Dia}# AND fechaFinPermiso >= #{Dia}#");
        }

        private DataRow RrhhMetPermisoActual()
        {
            if (_IdEmpleado == null) return null;
            DataRow[] Filas = RrhhMetBuscarPermisos(_IdEmpleado.Value, RrhhDtpFecha.Value, "DIAS");
            return Filas.Length > 0 ? Filas[0] : null;
        }

        private DataRow[] RrhhMetPermisosHorasActuales()
        {
            if (_IdEmpleado == null) return new DataRow[0];
            return RrhhMetBuscarPermisos(_IdEmpleado.Value, RrhhDtpFecha.Value, "HORAS");
        }

        private DataRow RrhhMetCapacitacionActual()
        {
            if (_IdEmpleado == null || _Capacitaciones == null) return null;
            string Dia = RrhhDtpFecha.Value.Date.ToString("MM/dd/yyyy");
            DataRow[] Filas = _Capacitaciones.Select($"idEmpleado = {_IdEmpleado.Value} " +
                                                     $"AND fechaInicio <= #{Dia}# AND fechaFin >= #{Dia}#");
            return Filas.Length > 0 ? Filas[0] : null;
        }

        // Revisa si algún permiso por horas cubre el rango [Desde, Hasta] con la tolerancia de la jornada
        private bool RrhhMetPermisoHorasCubre(TimeSpan Desde, TimeSpan Hasta, int Tolerancia)
        {
            var Margen = TimeSpan.FromMinutes(Tolerancia);
            foreach (DataRow Permiso in RrhhMetPermisosHorasActuales())
            {
                if (Permiso["horaInicioPermiso"] == DBNull.Value || Permiso["horaFinPermiso"] == DBNull.Value) continue;
                if ((TimeSpan)Permiso["horaInicioPermiso"] <= Desde + Margen && (TimeSpan)Permiso["horaFinPermiso"] >= Hasta - Margen)
                    return true;
            }
            return false;
        }

        private bool RrhhMetTardanzaCubierta(DataRow Empleado, TimeSpan Entrada)
        {
            return RrhhMetPermisoHorasCubre((TimeSpan)Empleado["horaInicioJornada"], Entrada, RrhhMetTolerancia(Empleado));
        }

        private bool RrhhMetSalidaCubierta(DataRow Empleado, TimeSpan Salida)
        {
            return RrhhMetPermisoHorasCubre(Salida, (TimeSpan)Empleado["horaFinJornada"], RrhhMetTolerancia(Empleado));
        }

        private DateTime? RrhhMetFechaBaja(DataRow Empleado)
        {
            return Empleado == null || Empleado["fechaBaja"] == DBNull.Value ? (DateTime?)null : (DateTime)Empleado["fechaBaja"];
        }

        private static string RrhhMetHorasTrabajadas(object Entrada, object Salida)
        {
            if (Entrada == DBNull.Value) return "0:00";
            if (Salida == DBNull.Value) return "-";
            TimeSpan Horas = (TimeSpan)Salida - (TimeSpan)Entrada;
            if (Horas <= TimeSpan.Zero) return "-";
            return $"{(int)Horas.TotalHours}:{Horas.Minutes:00}";
        }

        private void RrhhMetLlenarEmpleados()
        {
            var Nombres = new AutoCompleteStringCollection();
            var Codigos = new AutoCompleteStringCollection();
            foreach (DataRow Fila in _Empleados.Rows)
            {
                Nombres.Add(Fila["nombreEmpleado"].ToString());
                Codigos.Add(Fila["codigoEmpleado"].ToString());
            }
            RrhhTxtEmpleado.AutoCompleteCustomSource = Nombres;
            RrhhTxtFiltroEmpleado.AutoCompleteCustomSource = Nombres;
            RrhhTxtCodigo.AutoCompleteCustomSource = Codigos;
        }

        private void RrhhMetNuevoRegistro()
        {
            _CargandoRegistro = true;
            RrhhDgvAsistencias.ClearSelection();
            RrhhDgvAsistencias.CurrentCell = null;
            _IdSeleccionado = null;

            RrhhMetPonerEmpleado(null, null);
            foreach (TextBox Campo in new[] { RrhhTxtCodigo, RrhhTxtEmpleado, RrhhTxtDepartamento, RrhhTxtPuesto, RrhhTxtJornada,
                                              RrhhTxtTipoPuesto, RrhhTxtObservaciones, RrhhTxtFechaRegistro,
                                              RrhhTxtUltimaModificacion })
                Campo.Clear();
            RrhhDtpFecha.Value = DateTime.Today;
            RrhhMetPonerHora(RrhhDtpHoraEntrada, DBNull.Value);
            RrhhMetPonerHora(RrhhDtpHoraSalida, DBNull.Value);
            RrhhCboEstado.SelectedIndex = -1;
            _CargandoRegistro = false;

            RrhhMetLimpiarErrores();
            RrhhMetRevisarPermiso();
            RrhhMetActualizarResumen();
            _HayCambios = false;
            RrhhMetActualizarBotones();
        }

        private void RrhhDgvAsistencias_SelectionChanged(object sender, EventArgs e)
        {
            if (_CargandoRegistro) return;
            DataGridViewRow Fila = RrhhDgvAsistencias.CurrentRow;
            if (Fila == null || !Fila.Selected || !(Fila.DataBoundItem is DataRowView Registro)) return;
            RrhhMetMostrarDetalle(Registro.Row);
        }

        private void RrhhMetMostrarDetalle(DataRow Fila)
        {
            _CargandoRegistro = true;
            _IdSeleccionado = (int)Fila["idAsistencia"];
            RrhhMetPonerEmpleado(RrhhMetBuscarEmpleado((int)Fila["idEmpleado"]), null);
            if (_IdEmpleado == null)
            {
                // El empleado ya no está activo, solo se muestra quién era
                RrhhTxtCodigo.Text = Fila["codigoEmpleado"].ToString();
                RrhhTxtEmpleado.Text = Fila["nombreEmpleado"].ToString();
                RrhhTxtDepartamento.Text = Fila["departamentoEmpleado"].ToString();
                RrhhTxtPuesto.Text = Fila["puestoEmpleado"].ToString();
            }
            RrhhDtpFecha.Value = (DateTime)Fila["fechaAsistencia"];
            RrhhMetPonerHora(RrhhDtpHoraEntrada, Fila["horaEntrada"]);
            RrhhMetPonerHora(RrhhDtpHoraSalida, Fila["horaSalida"]);
            RrhhCboEstado.SelectedItem = Fila["estadoAsistencia"].ToString();
            RrhhTxtObservaciones.Text = Fila["observacionesAsistencia"].ToString();
            RrhhTxtFechaRegistro.Text = ((DateTime)Fila["created_at"]).ToString("dd/MM/yyyy HH:mm");
            RrhhTxtUltimaModificacion.Text = ((DateTime)Fila["updated_at"]).ToString("dd/MM/yyyy HH:mm");
            _CargandoRegistro = false;

            RrhhMetLimpiarErrores();
            RrhhMetRevisarPermiso();
            RrhhMetActualizarResumen();
            _HayCambios = false;
            RrhhMetActualizarBotones();
        }

        private static void RrhhMetPonerHora(DateTimePicker Control, object Hora)
        {
            Control.Value = DateTime.Today.Add(Hora == DBNull.Value ? new TimeSpan(8, 0, 0) : (TimeSpan)Hora);
            Control.Checked = Hora != DBNull.Value;
        }

        private static object RrhhMetLeerHora(DateTimePicker Control)
        {
            return Control.Checked ? (object)new TimeSpan(Control.Value.Hour, Control.Value.Minute, 0) : DBNull.Value;
        }

        private void RrhhTxtCodigo_TextChanged(object sender, EventArgs e)
        {
            if (_LlenandoEmpleado || _Empleados == null) return;
            RrhhMetElegirEmpleado(RrhhMetBuscarEmpleadoPor("codigoEmpleado", RrhhTxtCodigo.Text), RrhhTxtCodigo);
        }

        private void RrhhTxtEmpleado_TextChanged(object sender, EventArgs e)
        {
            if (_LlenandoEmpleado || _Empleados == null) return;
            RrhhMetElegirEmpleado(RrhhMetBuscarEmpleadoPor("nombreEmpleado", RrhhTxtEmpleado.Text), RrhhTxtEmpleado);
        }

        private DataRow RrhhMetBuscarEmpleadoPor(string Columna, string Texto)
        {
            DataRow[] Filas = _Empleados.Select($"{Columna} = '{Texto.Trim().Replace("'", "''")}'");
            return Filas.Length > 0 ? Filas[0] : null;
        }

        private void RrhhMetElegirEmpleado(DataRow Empleado, TextBox Origen)
        {
            int? Id = Empleado == null ? (int?)null : (int)Empleado["idEmpleado"];
            if (Id == _IdEmpleado) return;

            RrhhMetPonerEmpleado(Empleado, Origen);
            if (_CargandoRegistro) return;
            RrhhMetRevisarPermiso();
            RrhhMetCampoCambiado(RrhhTxtEmpleado);
        }

        private void RrhhMetPonerEmpleado(DataRow Empleado, TextBox Origen)
        {
            _LlenandoEmpleado = true;
            _IdEmpleado = Empleado == null ? (int?)null : (int)Empleado["idEmpleado"];
            if (Empleado != null)
            {
                if (Origen != RrhhTxtCodigo) RrhhTxtCodigo.Text = Empleado["codigoEmpleado"].ToString();
                if (Origen != RrhhTxtEmpleado) RrhhTxtEmpleado.Text = Empleado["nombreEmpleado"].ToString();
            }
            RrhhTxtDepartamento.Text = Empleado?["departamentoEmpleado"].ToString() ?? "";
            RrhhTxtPuesto.Text = Empleado?["puestoEmpleado"].ToString() ?? "";
            RrhhTxtJornada.Text = Empleado?["jornada"].ToString() ?? "";
            RrhhTxtTipoPuesto.Text = Empleado?["tipoPuesto"].ToString() ?? "";
            _LlenandoEmpleado = false;
        }

        private void RrhhDtpFecha_ValueChanged(object sender, EventArgs e)
        {
            if (_CargandoRegistro) return;
            RrhhMetRevisarPermiso();
            RrhhMetCampoCambiado(RrhhDtpFecha);
        }

        private void RrhhDtpHora_ValueChanged(object sender, EventArgs e)
        {
            if (_CargandoRegistro) return;
            if (RrhhCboEstado.Enabled) RrhhCboEstado.SelectedItem = RrhhMetEstadoSugerido();
            RrhhMetCampoCambiado((Control)sender);
            RrhhMetQuitarError(RrhhDtpHoraSalida);
        }

        private void RrhhCboEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_CargandoRegistro) return;
            RrhhMetCampoCambiado(RrhhCboEstado);
        }

        private void RrhhTxtObservaciones_TextChanged(object sender, EventArgs e)
        {
            if (_CargandoRegistro) return;
            RrhhMetCampoCambiado(RrhhTxtObservaciones);
        }

        private void RrhhMetCampoCambiado(Control Campo)
        {
            _HayCambios = true;
            RrhhMetQuitarError(Campo);
            RrhhMetActualizarResumen();
            RrhhMetActualizarBotones();
        }

        private void RrhhMetRevisarPermiso()
        {
            DataRow Permiso = RrhhMetPermisoActual();
            if (Permiso != null)
            {
                RrhhCboEstado.SelectedItem = "Permiso";
                RrhhCboEstado.Enabled = false;
                RrhhMetQuitarError(RrhhCboEstado);
                RrhhLblPermiso.Text = Permiso["tipoPermiso"].ToString() == "Vacaciones"
                    ? $"De vacaciones del {(DateTime)Permiso["fechaInicioPermiso"]:dd/MM} al {(DateTime)Permiso["fechaFinPermiso"]:dd/MM}"
                    : $"{Permiso["tipoPermiso"]} aprobado: {Permiso["motivoPermiso"]}";
                RrhhLblPermiso.Visible = true;
                return;
            }

            RrhhCboEstado.Enabled = true;
            RrhhLblPermiso.Text = RrhhMetAvisoDelDia();
            RrhhLblPermiso.Visible = RrhhLblPermiso.Text.Length > 0;
            if (!_CargandoRegistro && RrhhCboEstado.SelectedItem?.ToString() == "Permiso")
                RrhhCboEstado.SelectedItem = RrhhMetEstadoSugerido();
        }

        // Aviso de capacitación o permiso por horas que no bloquea el estado
        private string RrhhMetAvisoDelDia()
        {
            DataRow Capacitacion = RrhhMetCapacitacionActual();
            if (Capacitacion != null) return $"En capacitación: {Capacitacion["nombreCapacitacion"]}";

            DataRow[] PermisosHoras = RrhhMetPermisosHorasActuales();
            if (PermisosHoras.Length > 0 && PermisosHoras[0]["horaInicioPermiso"] != DBNull.Value)
                return $"Permiso por horas de {(TimeSpan)PermisosHoras[0]["horaInicioPermiso"]:hh\\:mm} " +
                       $"a {(TimeSpan)PermisosHoras[0]["horaFinPermiso"]:hh\\:mm}: {PermisosHoras[0]["motivoPermiso"]}";
            return "";
        }

        private string RrhhMetEstadoSugerido()
        {
            if (RrhhMetPermisoActual() != null) return "Permiso";
            object Entrada = RrhhMetLeerHora(RrhhDtpHoraEntrada);
            if (Entrada == DBNull.Value) return RrhhMetCapacitacionActual() != null ? "Presente" : "Ausente";
            DataRow Empleado = RrhhMetEmpleadoActual();
            if (Empleado != null && ((TimeSpan)Entrada - (TimeSpan)Empleado["horaInicioJornada"]).TotalMinutes > RrhhMetTolerancia(Empleado)
                && !RrhhMetTardanzaCubierta(Empleado, (TimeSpan)Entrada))
                return "Tardanza";
            return "Presente";
        }

        private void RrhhMetActualizarResumen()
        {
            object Entrada = RrhhMetLeerHora(RrhhDtpHoraEntrada);
            object Salida = RrhhMetLeerHora(RrhhDtpHoraSalida);
            string Estado = RrhhCboEstado.SelectedItem?.ToString() ?? "";
            bool HayEmpleado = RrhhMetEmpleadoActual() != null;

            RrhhTxtHorasTrabajadas.Text = HayEmpleado ? RrhhMetHorasTrabajadas(Entrada, Salida) : "";
            RrhhTxtEstadoResumen.Text = Estado;
            RrhhTxtEstadoResumen.BackColor = RrhhMetColorEstado(Estado);
            RrhhTxtIncidencias.Text = HayEmpleado ? RrhhMetIncidencias(Entrada, Salida) : "";
        }

        private string RrhhMetIncidencias(object Entrada, object Salida)
        {
            if (RrhhMetPermisoActual() != null) return "Cubierto por permiso";
            bool EnCapacitacion = RrhhMetCapacitacionActual() != null;
            if (Entrada == DBNull.Value) return EnCapacitacion ? "En capacitación" : "Sin marca de entrada";
            DataRow Empleado = RrhhMetEmpleadoActual();
            if (Empleado == null) return "-";

            var Incidencias = new List<string>();
            int Tarde = (int)((TimeSpan)Entrada - (TimeSpan)Empleado["horaInicioJornada"]).TotalMinutes;
            if (Tarde > RrhhMetTolerancia(Empleado))
                Incidencias.Add(RrhhMetTardanzaCubierta(Empleado, (TimeSpan)Entrada)
                                ? "Entrada cubierta por permiso" : $"Llegó {Tarde} min tarde");

            if (Salida == DBNull.Value)
                Incidencias.Add("Sin marca de salida");
            else
            {
                int Antes = (int)((TimeSpan)Empleado["horaFinJornada"] - (TimeSpan)Salida).TotalMinutes;
                if (Antes > 0)
                    Incidencias.Add(RrhhMetSalidaCubierta(Empleado, (TimeSpan)Salida)
                                    ? "Salida cubierta por permiso" : $"Salió {Antes} min antes");
                else if (Antes < 0)
                    Incidencias.Add($"{-Antes} min después de la jornada");
            }
            return Incidencias.Count == 0 ? "-" : string.Join(", ", Incidencias);
        }

        private static Color RrhhMetColorEstado(string Estado)
        {
            switch (Estado)
            {
                case "Presente": return Color.FromArgb(214, 242, 224);
                case "Tardanza": return Color.FromArgb(252, 235, 201);
                case "Ausente": return Color.FromArgb(248, 215, 218);
                case "Permiso": return Color.FromArgb(221, 235, 247);
                default: return Color.FromArgb(245, 248, 247);
            }
        }

        private bool RrhhMetValidar()
        {
            RrhhMetLimpiarErrores();
            DataRow Empleado = RrhhMetEmpleadoActual();
            object Entrada = RrhhMetLeerHora(RrhhDtpHoraEntrada);
            object Salida = RrhhMetLeerHora(RrhhDtpHoraSalida);
            string Estado = RrhhCboEstado.SelectedItem?.ToString();

            if (Empleado == null)
                RrhhMetMarcarError(RrhhTxtEmpleado, "Seleccione un empleado por código o nombre.");

            if (RrhhDtpFecha.Value.Date > DateTime.Today)
                RrhhMetMarcarError(RrhhDtpFecha, "La fecha no puede ser futura.");
            else if (Empleado != null && RrhhDtpFecha.Value.Date < (DateTime)Empleado["fechaContratacion"])
                RrhhMetMarcarError(RrhhDtpFecha, $"El empleado fue contratado el {(DateTime)Empleado["fechaContratacion"]:dd/MM/yyyy}.");
            else if (RrhhMetFechaBaja(Empleado) is DateTime FechaBaja && RrhhDtpFecha.Value.Date >= FechaBaja)
                RrhhMetMarcarError(RrhhDtpFecha, $"El empleado fue dado de baja el {FechaBaja:dd/MM/yyyy}.");
            else if (Empleado != null && RrhhMetExisteAsistencia((int)Empleado["idEmpleado"], RrhhDtpFecha.Value))
                RrhhMetMarcarError(RrhhDtpFecha, "Este empleado ya tiene asistencia en esta fecha.");

            if (Salida != DBNull.Value && Entrada == DBNull.Value)
                RrhhMetMarcarError(RrhhDtpHoraSalida, "Marque primero la hora de entrada.");
            else if (Salida != DBNull.Value && (TimeSpan)Salida <= (TimeSpan)Entrada)
                RrhhMetMarcarError(RrhhDtpHoraSalida, "La salida debe ser después de la entrada.");

            if (string.IsNullOrEmpty(Estado))
                RrhhMetMarcarError(RrhhCboEstado, "Seleccione el estado.");
            else if (Estado == "Permiso" && RrhhMetPermisoActual() == null)
                RrhhMetMarcarError(RrhhCboEstado, "No tiene un permiso aprobado para esta fecha.");
            else if (Estado == "Tardanza" && Entrada == DBNull.Value)
                RrhhMetMarcarError(RrhhCboEstado, "Sin hora de entrada no puede ser tardanza.");
            else if (Estado == "Presente" && Entrada == DBNull.Value && RrhhMetCapacitacionActual() == null)
                RrhhMetMarcarError(RrhhCboEstado, "Sin hora de entrada el estado debe ser Ausente.");
            else if (Estado == "Ausente" && Entrada != DBNull.Value)
                RrhhMetMarcarError(RrhhCboEstado, "Si está ausente quite la hora de entrada.");

            return _CamposConError.Count == 0;
        }

        private bool RrhhMetExisteAsistencia(int IdEmpleado, DateTime Fecha)
        {
            string Filtro = $"idEmpleado = {IdEmpleado} AND fechaAsistencia = #{Fecha.Date:MM/dd/yyyy}#";
            if (_IdSeleccionado != null) Filtro += " AND idAsistencia <> " + _IdSeleccionado;
            return _Asistencias.Select(Filtro).Length > 0;
        }

        private void RrhhMetMarcarError(Control Campo, string Mensaje)
        {
            _CamposConError.Add(Campo);
            Label Etiqueta = _EtiquetasError[Campo];
            Etiqueta.Text = Mensaje;
            Etiqueta.Visible = true;
            if (Campo == RrhhCboEstado) RrhhLblPermiso.Visible = false;
            Campo.Parent.Invalidate();
        }

        private void RrhhMetQuitarError(Control Campo)
        {
            if (!_CamposConError.Remove(Campo)) return;
            _EtiquetasError[Campo].Visible = false;
            if (Campo == RrhhCboEstado) RrhhLblPermiso.Visible = RrhhLblPermiso.Text.Length > 0;
            Campo.Parent.Invalidate();
        }

        private void RrhhMetLimpiarErrores()
        {
            foreach (Control Campo in _CamposConError.ToList()) RrhhMetQuitarError(Campo);
        }

        private void RrhhGbx_Paint(object sender, PaintEventArgs e)
        {
            using (Pen Lapiz = new Pen(Color.FromArgb(211, 47, 47), 2))
            {
                foreach (Control Campo in _CamposConError)
                {
                    if (Campo.Parent != sender) continue;
                    Rectangle Borde = Campo.Bounds;
                    Borde.Inflate(1, 1);
                    e.Graphics.DrawRectangle(Lapiz, Borde);
                }
            }
        }

        private void RrhhMetActualizarBotones()
        {
            RrhhMetHabilitarBoton(RrhhBtnGuardar, _HayCambios && _IdSeleccionado == null);
            RrhhMetHabilitarBoton(RrhhBtnModificar, _IdSeleccionado != null);
            RrhhMetHabilitarBoton(RrhhBtnEliminar, _IdSeleccionado != null);
        }

        private void RrhhMetHabilitarBoton(Button Boton, bool Habilitar)
        {
            Habilitar = Habilitar && !_BotonesSinPermiso.Contains(Boton);
            Boton.Enabled = Habilitar;
            if (_ImagenesBotones.TryGetValue(Boton, out Image[] Imagenes))
                Boton.BackgroundImage = Imagenes[Habilitar ? 0 : 2];
        }

        private void RrhhBtnGuardar_Click(object sender, EventArgs e)
        {
            if (!RrhhMetValidar()) return;

            ClsModeloAsistencia Asistencia = RrhhMetDetalleAModelo();
            Asistencia.Estado = EstadoEntidad.Added;
            RrhhMetGrabar(Asistencia);
        }

        private void RrhhBtnModificar_Click(object sender, EventArgs e)
        {
            if (_IdSeleccionado == null) return;
            if (!_HayCambios)
            {
                MessageBox.Show("No hay cambios para modificar.", "Asistencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!RrhhMetValidar()) return;

            ClsModeloAsistencia Asistencia = RrhhMetDetalleAModelo();
            Asistencia.IdAsistenciaEmpleado = _IdSeleccionado.Value;
            Asistencia.Estado = EstadoEntidad.Modified;
            RrhhMetGrabar(Asistencia);
        }

        private void RrhhBtnEliminar_Click(object sender, EventArgs e)
        {
            if (_IdSeleccionado == null) return;

            DataRow[] Filas = _Asistencias.Select("idAsistencia = " + _IdSeleccionado);
            if (Filas.Length == 0) return;
            DataRow Fila = Filas[0];

            DialogResult Respuesta = MessageBox.Show(
                $"¿Seguro que desea eliminar la asistencia de {Fila["nombreEmpleado"]} del " +
                $"{(DateTime)Fila["fechaAsistencia"]:dd/MM/yyyy}? Esta acción no se puede deshacer.",
                "Eliminar asistencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Respuesta != DialogResult.Yes) return;

            var Asistencia = new ClsModeloAsistencia
            {
                IdAsistenciaEmpleado = _IdSeleccionado.Value,
                IdEmpleado = (int)Fila["idEmpleado"],
                FechaAsistencia = (DateTime)Fila["fechaAsistencia"],
                Estado = EstadoEntidad.Deleted
            };
            RrhhMetGrabar(Asistencia);
        }

        private void RrhhBtnRefrescar_Click(object sender, EventArgs e)
        {
            RrhhMetCargarDatos();
            RrhhMetNuevoRegistro();
            RrhhMetAplicarFiltro();
        }

        private ClsModeloAsistencia RrhhMetDetalleAModelo()
        {
            object Entrada = RrhhMetLeerHora(RrhhDtpHoraEntrada);
            object Salida = RrhhMetLeerHora(RrhhDtpHoraSalida);
            return new ClsModeloAsistencia
            {
                IdEmpleado = _IdEmpleado.Value,
                FechaAsistencia = RrhhDtpFecha.Value.Date,
                HoraEntrada = Entrada == DBNull.Value ? (TimeSpan?)null : (TimeSpan)Entrada,
                HoraSalida = Salida == DBNull.Value ? (TimeSpan?)null : (TimeSpan)Salida,
                EstadoAsistencia = RrhhCboEstado.SelectedItem.ToString(),
                Observaciones = RrhhTxtObservaciones.Text.Trim()
            };
        }

        private void RrhhMetGrabar(ClsModeloAsistencia Asistencia)
        {
            Cursor = Cursors.WaitCursor;
            string Mensaje = Asistencia.RrhhMetGrabarCambios();
            Cursor = Cursors.Default;

            if (!Asistencia.Grabado)
            {
                MessageBox.Show(Mensaje, "Asistencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(Mensaje, "Asistencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RrhhMetDespuesDeGuardar();
        }

        private void RrhhMetDespuesDeGuardar()
        {
            RrhhMetCargarDatos();
            RrhhMetNuevoRegistro();
            RrhhMetAplicarFiltro();
        }

        private void RrhhDgvAsistencias_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            if (e.ColumnIndex == RrhhColEstado.Index)
            {
                e.CellStyle.BackColor = RrhhMetColorEstado(e.Value.ToString());
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
            }
            else if (e.ColumnIndex == RrhhColHoraEntrada.Index || e.ColumnIndex == RrhhColHoraSalida.Index)
            {
                e.Value = e.Value is TimeSpan Hora ? Hora.ToString(@"hh\:mm") : "-";
                e.FormattingApplied = true;
            }
            else if (e.ColumnIndex == RrhhColObservaciones.Index && e.Value.ToString() == "")
            {
                e.Value = "-";
                e.FormattingApplied = true;
            }
        }

        private void RrhhBtnFiltrar_Click(object sender, EventArgs e)
        {
            RrhhMetAplicarFiltro();
        }

        private void RrhhTxtBuscar_TextChanged(object sender, EventArgs e)
        {
            RrhhMetAplicarFiltro();
        }

        private void RrhhMetAplicarFiltro()
        {
            if (_Vista == null) return;

            string Filtro = string.Format("fechaAsistencia >= #{0:MM/dd/yyyy}# AND fechaAsistencia <= #{1:MM/dd/yyyy}#",
                                          RrhhDtpDesde.Value.Date, RrhhDtpHasta.Value.Date);
            string Empleado = RrhhMetTextoFiltro(RrhhTxtFiltroEmpleado.Text);
            if (Empleado.Length > 0)
                Filtro += $" AND (nombreEmpleado LIKE '%{Empleado}%' OR codigoEmpleado LIKE '%{Empleado}%')";
            if (RrhhCboFiltroEstado.SelectedIndex > 0)
                Filtro += $" AND estadoAsistencia = '{RrhhCboFiltroEstado.SelectedItem}'";

            string Buscar = RrhhMetTextoFiltro(RrhhTxtBuscar.Text);
            if (Buscar.Length > 0)
                Filtro += $" AND (nombreEmpleado LIKE '%{Buscar}%' OR codigoEmpleado LIKE '%{Buscar}%')";

            _CargandoRegistro = true;
            _Vista.RowFilter = Filtro;
            _CargandoRegistro = false;
            if (_IdSeleccionado == null) RrhhDgvAsistencias.ClearSelection();
            RrhhMetActualizarTotal();
        }

        private static string RrhhMetTextoFiltro(string Texto)
        {
            return Texto.Trim().Replace("'", "''").Replace("[", "").Replace("]", "").Replace("*", "").Replace("%", "");
        }

        private void RrhhMetActualizarTotal()
        {
            int Total = _Vista.Count;
            RrhhLblTotalRegistros.Text = Total == 0 ? "No hay registros" : $"Mostrando 1 - {Total} de {Total} registros";
        }

        private void RrhhBtnInicio_Click(object sender, EventArgs e)
        {
            RrhhMetIrAFila(0);
        }

        private void RrhhBtnAnterior_Click(object sender, EventArgs e)
        {
            RrhhMetIrAFila((RrhhDgvAsistencias.CurrentRow?.Index ?? 1) - 1);
        }

        private void RrhhBtnSiguiente_Click(object sender, EventArgs e)
        {
            RrhhMetIrAFila((RrhhDgvAsistencias.CurrentRow?.Index ?? -1) + 1);
        }

        private void RrhhBtnFin_Click(object sender, EventArgs e)
        {
            RrhhMetIrAFila(RrhhDgvAsistencias.Rows.Count - 1);
        }

        private void RrhhMetIrAFila(int Indice)
        {
            int Total = RrhhDgvAsistencias.Rows.Count;
            if (Total == 0) return;
            Indice = Math.Max(0, Math.Min(Total - 1, Indice));
            RrhhDgvAsistencias.CurrentCell = RrhhDgvAsistencias.Rows[Indice].Cells[0];
            RrhhDgvAsistencias.Rows[Indice].Selected = true;
        }

        private void RrhhBtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void RrhhMetDeshabilitarBoton(Button Boton)
        {
            _BotonesSinPermiso.Add(Boton);
            RrhhMetHabilitarBoton(Boton, false);
        }

        private void RrhhMetPrepararImagenes()
        {
            foreach (Control Control in RrhhPnlBotones.Controls)
            {
                if (!(Control is Button Boton) || Boton.BackgroundImage == null) continue;

                Image Original = Boton.BackgroundImage;
                int Lado = Math.Min(Boton.Width, Boton.Height);
                Bitmap Normal = RrhhMetEscalarImagen(Original, Lado, Lado);
                _ImagenesBotones[Boton] = new Image[] { Normal, RrhhMetAclarar(Normal), RrhhMetGris(Normal) };

                Boton.BackgroundImage = Normal;
                Boton.BackgroundImageLayout = ImageLayout.Center;
                Boton.MouseEnter += RrhhBtnBarra_MouseEnter;
                Boton.MouseLeave += RrhhBtnBarra_MouseLeave;
                Original.Dispose();
            }

            Image Fondo = RrhhPnlEncabezado.BackgroundImage;
            RrhhPnlEncabezado.BackgroundImage = RrhhMetEscalarImagen(Fondo, RrhhPnlEncabezado.Width, RrhhPnlEncabezado.Height);
            RrhhPnlEncabezado.BackgroundImageLayout = ImageLayout.None;
            Fondo.Dispose();

            RrhhMetAjustarImagen(RrhhPbIcono);
            RrhhMetAjustarImagen(RrhhPbMascota);
        }

        private void RrhhBtnBarra_MouseEnter(object sender, EventArgs e)
        {
            Button Boton = (Button)sender;
            if (Boton.Enabled) Boton.BackgroundImage = _ImagenesBotones[Boton][1];
        }

        private void RrhhBtnBarra_MouseLeave(object sender, EventArgs e)
        {
            Button Boton = (Button)sender;
            if (Boton.Enabled) Boton.BackgroundImage = _ImagenesBotones[Boton][0];
        }

        private void RrhhMetAjustarImagen(PictureBox Caja)
        {
            Image Original = Caja.Image;
            if (Original == null) return;

            float Escala = Math.Min((float)Caja.Width / Original.Width, (float)Caja.Height / Original.Height);
            Caja.Image = RrhhMetEscalarImagen(Original, Math.Max(1, (int)(Original.Width * Escala)),
                                              Math.Max(1, (int)(Original.Height * Escala)));
            Caja.SizeMode = PictureBoxSizeMode.CenterImage;
            Original.Dispose();
        }

        private static Bitmap RrhhMetEscalarImagen(Image Original, int Ancho, int Alto)
        {
            var Resultado = new Bitmap(Ancho, Alto, PixelFormat.Format32bppPArgb);
            using (Graphics G = Graphics.FromImage(Resultado))
            {
                G.InterpolationMode = InterpolationMode.HighQualityBicubic;
                G.PixelOffsetMode = PixelOffsetMode.HighQuality;
                G.SmoothingMode = SmoothingMode.AntiAlias;
                G.DrawImage(Original, 0, 0, Ancho, Alto);
            }
            return Resultado;
        }

        private static Bitmap RrhhMetAclarar(Image Original)
        {
            return RrhhMetAplicarColor(Original, new ColorMatrix(new[]
            {
                new float[] { 1, 0, 0, 0, 0 },
                new float[] { 0, 1, 0, 0, 0 },
                new float[] { 0, 0, 1, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { 0.12f, 0.12f, 0.12f, 0, 1 }
            }));
        }

        private static Bitmap RrhhMetGris(Image Original)
        {
            return RrhhMetAplicarColor(Original, new ColorMatrix(new[]
            {
                new float[] { 0.2f, 0.2f, 0.2f, 0, 0 },
                new float[] { 0.4f, 0.4f, 0.4f, 0, 0 },
                new float[] { 0.1f, 0.1f, 0.1f, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { 0.35f, 0.35f, 0.35f, 0, 1 }
            }));
        }

        private static Bitmap RrhhMetAplicarColor(Image Original, ColorMatrix Matriz)
        {
            var Resultado = new Bitmap(Original.Width, Original.Height, PixelFormat.Format32bppPArgb);
            using (Graphics G = Graphics.FromImage(Resultado))
            using (var Atributos = new ImageAttributes())
            {
                Atributos.SetColorMatrix(Matriz);
                G.DrawImage(Original, new Rectangle(0, 0, Original.Width, Original.Height),
                            0, 0, Original.Width, Original.Height, GraphicsUnit.Pixel, Atributos);
            }
            return Resultado;
        }

        private void RrhhPnlTarjeta_Paint(object sender, PaintEventArgs e)
        {
            Control Tarjeta = (Control)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath Ruta = RrhhMetRedondearEsquinas(new Rectangle(0, 0, Tarjeta.Width - 1, Tarjeta.Height - 1), 14))
            using (Pen Lapiz = new Pen(Color.FromArgb(160, 31, 138, 143), 1.5f))
            {
                e.Graphics.DrawPath(Lapiz, Ruta);
            }
        }

        private void RrhhMetRedondear(Control Control, int Radio)
        {
            using (GraphicsPath Ruta = RrhhMetRedondearEsquinas(Control.ClientRectangle, Radio))
                Control.Region = new Region(Ruta);
        }

        private static GraphicsPath RrhhMetRedondearEsquinas(Rectangle Rectangulo, int Radio)
        {
            GraphicsPath Ruta = new GraphicsPath();
            int Diametro = Radio * 2;
            Ruta.AddArc(Rectangulo.X, Rectangulo.Y, Diametro, Diametro, 180, 90);
            Ruta.AddArc(Rectangulo.Right - Diametro, Rectangulo.Y, Diametro, Diametro, 270, 90);
            Ruta.AddArc(Rectangulo.Right - Diametro, Rectangulo.Bottom - Diametro, Diametro, Diametro, 0, 90);
            Ruta.AddArc(Rectangulo.X, Rectangulo.Bottom - Diametro, Diametro, Diametro, 90, 90);
            Ruta.CloseFigure();
            return Ruta;
        }
    }
}
