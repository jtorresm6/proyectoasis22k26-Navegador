/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander
 * Carné : 0901-23-3747
 * Fecha : 11/10/2026
 * ==================================================================
 * Propósito :
 *  Formulario 9101 de Asistencia. Muestra el detalle de la asistencia
 *  de un empleado (sus datos, el registro del día y un resumen) y la
 *  lista de asistencias registradas con sus filtros. El empleado se
 *  busca por código o por nombre y sus datos se llenan solos.
 * Reglas especificas:
 *  Por ahora es un prototipo no funcional, inicia sin datos y lo que
 *  se ingresa solo queda en memoria mientras la ventana está abierta.
 *  Si el empleado tiene un permiso aprobado para la fecha, el
 *  estado queda en Permiso y no se puede cambiar. Las horas trabajadas
 *  y las incidencias se calculan con la jornada del empleado. Es
 *  ventana secundaria, no se puede maximizar.
 * ===================================================================
*/

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
        private DataTable _Empleados;
        private DataTable _Permisos;
        private DataTable _Asistencias;
        private DataView _Vista;

        private int? _IdSeleccionado;
        private int? _IdEmpleado;
        private bool _LlenandoEmpleado;
        private bool _HayCambios;

        private bool _CargandoRegistro;

        private const int MinutosTolerancia = 5;

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
            this.Shown += (s, e) => RrhhMetNuevoRegistro();
        }

        private void FrmAsistencia_Load(object sender, EventArgs e)
        {
            RrhhMetPrepararImagenes();
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
            RrhhMetLlenarEmpleados();

            RrhhDtpDesde.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            RrhhDtpHasta.Value = RrhhDtpDesde.Value.AddMonths(1).AddDays(-1);
            RrhhCboFiltroEstado.SelectedIndex = 0;

            _Vista = new DataView(_Asistencias);
            RrhhDgvAsistencias.DataSource = _Vista;
            RrhhMetActualizarTotal();
        }

        private void RrhhMetCrearTablas()
        {
            _Empleados = new DataTable();
            _Empleados.Columns.Add("idEmpleado", typeof(int));
            _Empleados.Columns.Add("codigoEmpleado", typeof(string));
            _Empleados.Columns.Add("nombreEmpleado", typeof(string));
            _Empleados.Columns.Add("departamentoEmpleado", typeof(string));
            _Empleados.Columns.Add("puestoEmpleado", typeof(string));
            _Empleados.Columns.Add("tipoPuesto", typeof(string));
            _Empleados.Columns.Add("jornada", typeof(string));
            _Empleados.Columns.Add("horaInicioJornada", typeof(TimeSpan));
            _Empleados.Columns.Add("horaFinJornada", typeof(TimeSpan));

            _Permisos = new DataTable();
            _Permisos.Columns.Add("idPermiso", typeof(int));
            _Permisos.Columns.Add("idEmpleado", typeof(int));
            _Permisos.Columns.Add("tipoPermiso", typeof(string));
            _Permisos.Columns.Add("fechaInicioPermiso", typeof(DateTime));
            _Permisos.Columns.Add("fechaFinPermiso", typeof(DateTime));
            _Permisos.Columns.Add("motivoPermiso", typeof(string));
            _Permisos.Columns.Add("estadoPermiso", typeof(string));

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

        private void RrhhMetLlenarFila(DataRow Fila, int IdEmpleado, DateTime Fecha, object Entrada,
                                       object Salida, string Estado, string Observaciones)
        {
            DataRow Empleado = RrhhMetBuscarEmpleado(IdEmpleado);
            Fila["idEmpleado"] = IdEmpleado;
            Fila["codigoEmpleado"] = Empleado["codigoEmpleado"];
            Fila["nombreEmpleado"] = Empleado["nombreEmpleado"];
            Fila["departamentoEmpleado"] = Empleado["departamentoEmpleado"];
            Fila["puestoEmpleado"] = Empleado["puestoEmpleado"];
            Fila["fechaAsistencia"] = Fecha;
            Fila["horaEntrada"] = Entrada;
            Fila["horaSalida"] = Salida;
            Fila["estadoAsistencia"] = Estado;
            Fila["observacionesAsistencia"] = Observaciones;
            Fila["horasTrabajadas"] = RrhhMetHorasTrabajadas(Entrada, Salida);
        }

        private DataRow RrhhMetBuscarEmpleado(int IdEmpleado)
        {
            DataRow[] Filas = _Empleados.Select("idEmpleado = " + IdEmpleado);
            return Filas.Length > 0 ? Filas[0] : null;
        }

        private DataRow RrhhMetEmpleadoActual()
        {
            return _IdEmpleado == null ? null : RrhhMetBuscarEmpleado(_IdEmpleado.Value);
        }

        private DataRow RrhhMetBuscarPermiso(int IdEmpleado, DateTime Fecha)
        {
            string Dia = Fecha.Date.ToString("MM/dd/yyyy");
            DataRow[] Filas = _Permisos.Select($"idEmpleado = {IdEmpleado} AND estadoPermiso = 'APROBADO' " +
                                               $"AND fechaInicioPermiso <= #{Dia}# AND fechaFinPermiso >= #{Dia}#");
            return Filas.Length > 0 ? Filas[0] : null;
        }

        private DataRow RrhhMetPermisoActual()
        {
            return _IdEmpleado == null ? null : RrhhMetBuscarPermiso(_IdEmpleado.Value, RrhhDtpFecha.Value);
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
                RrhhLblPermiso.Text = $"{Permiso["tipoPermiso"]} aprobado: {Permiso["motivoPermiso"]}";
                RrhhLblPermiso.Visible = true;
                return;
            }

            RrhhCboEstado.Enabled = true;
            RrhhLblPermiso.Visible = false;
            if (!_CargandoRegistro && RrhhCboEstado.SelectedItem?.ToString() == "Permiso")
                RrhhCboEstado.SelectedItem = RrhhMetEstadoSugerido();
        }

        private string RrhhMetEstadoSugerido()
        {
            if (RrhhMetPermisoActual() != null) return "Permiso";
            object Entrada = RrhhMetLeerHora(RrhhDtpHoraEntrada);
            if (Entrada == DBNull.Value) return "Ausente";
            DataRow Empleado = RrhhMetEmpleadoActual();
            if (Empleado != null && ((TimeSpan)Entrada - (TimeSpan)Empleado["horaInicioJornada"]).TotalMinutes > MinutosTolerancia)
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
            if (Entrada == DBNull.Value) return "Sin marca de entrada";
            DataRow Empleado = RrhhMetEmpleadoActual();
            if (Empleado == null) return "-";

            var Incidencias = new List<string>();
            int Tarde = (int)((TimeSpan)Entrada - (TimeSpan)Empleado["horaInicioJornada"]).TotalMinutes;
            if (Tarde > MinutosTolerancia) Incidencias.Add($"Llegó {Tarde} min tarde");

            if (Salida == DBNull.Value)
                Incidencias.Add("Sin marca de salida");
            else
            {
                int Antes = (int)((TimeSpan)Empleado["horaFinJornada"] - (TimeSpan)Salida).TotalMinutes;
                if (Antes > 0) Incidencias.Add($"Salió {Antes} min antes");
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
            else if (Estado != "Ausente" && Estado != "Permiso" && Entrada == DBNull.Value)
                RrhhMetMarcarError(RrhhCboEstado, "Sin hora de entrada el estado debe ser Ausente.");

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
            if (Campo == RrhhCboEstado) RrhhLblPermiso.Visible = RrhhMetPermisoActual() != null;
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

            DataRow Fila = _Asistencias.NewRow();
            object UltimoId = _Asistencias.Compute("MAX(idAsistencia)", "");
            Fila["idAsistencia"] = UltimoId == DBNull.Value ? 1 : (int)UltimoId + 1;
            Fila["created_at"] = DateTime.Now;
            Fila["updated_at"] = DateTime.Now;
            RrhhMetPasarDetalleAFila(Fila);
            _Asistencias.Rows.Add(Fila);

            MessageBox.Show("La asistencia se guardó correctamente.", "Asistencia",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            RrhhMetDespuesDeGuardar();
        }

        private void RrhhBtnModificar_Click(object sender, EventArgs e)
        {
            if (_IdSeleccionado == null || !RrhhMetValidar()) return;

            DataRow Fila = _Asistencias.Select("idAsistencia = " + _IdSeleccionado)[0];
            Fila["updated_at"] = DateTime.Now;
            RrhhMetPasarDetalleAFila(Fila);

            MessageBox.Show("La asistencia se modificó correctamente.", "Asistencia",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            RrhhMetDespuesDeGuardar();
        }

        private void RrhhBtnEliminar_Click(object sender, EventArgs e)
        {
            if (_IdSeleccionado == null) return;

            DataRow Fila = _Asistencias.Select("idAsistencia = " + _IdSeleccionado)[0];
            DialogResult Respuesta = MessageBox.Show(
                $"¿Seguro que desea eliminar la asistencia de {Fila["nombreEmpleado"]} del " +
                $"{(DateTime)Fila["fechaAsistencia"]:dd/MM/yyyy}? Esta acción no se puede deshacer.",
                "Eliminar asistencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Respuesta != DialogResult.Yes) return;

            Fila.Delete();
            _Asistencias.AcceptChanges();
            RrhhMetDespuesDeGuardar();
        }

        private void RrhhBtnRefrescar_Click(object sender, EventArgs e)
        {
            RrhhMetNuevoRegistro();
            RrhhMetAplicarFiltro();
        }

        private void RrhhMetPasarDetalleAFila(DataRow Fila)
        {
            RrhhMetLlenarFila(Fila, _IdEmpleado.Value, RrhhDtpFecha.Value.Date,
                              RrhhMetLeerHora(RrhhDtpHoraEntrada), RrhhMetLeerHora(RrhhDtpHoraSalida),
                              RrhhCboEstado.SelectedItem.ToString(), RrhhTxtObservaciones.Text.Trim());
        }

        private void RrhhMetDespuesDeGuardar()
        {
            RrhhMetAplicarFiltro();
            RrhhMetNuevoRegistro();
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
