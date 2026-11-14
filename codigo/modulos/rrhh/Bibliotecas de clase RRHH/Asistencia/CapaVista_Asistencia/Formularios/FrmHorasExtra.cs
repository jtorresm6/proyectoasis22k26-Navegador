/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * Última modificación : 10/11/2026
 * ==================================================================
 * Propósito :
 *  Formulario 9102 de Horas Extra. Se abre con el botón Horas extra de
 *  la ventana de Asistencia. Registra la hora extra de una asistencia
 *  en la que el empleado salió después de su jornada, calcula el pago
 *  y lleva su aprobación. Reporte muestra RpHorasExtra.rdlc con lo que
 *  la lista tiene filtrado, sin pasar por el Reporteador. Ayuda abre
 *  su página en RecursosHumanosAyudas.chm del repo.
 * Reglas especificas:
 *  Solo salen asistencias Presente o Tardanza con salida después del
 *  fin de la jornada y antes de la baja del empleado; los días de
 *  permiso o vacaciones quedan fuera porque su asistencia es Permiso.
 *  El valor de la hora es el salario / 30 / horas de la jornada
 *  (8 diurna, 7 mixta, 6 nocturna) y el recargo por defecto es 50 %.
 *  La duración no pasa de lo trabajado después de la jornada ni de lo
 *  que falta para llegar a 12 horas en el día. Lo que Nómina ya tomó queda solo para
 *  consulta. Los botones se habilitan según los permisos del usuario
 *  en sesión (módulo 9000, aplicación 9102). Es ventana secundaria,
 *  no se puede maximizar.
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
    public partial class FrmHorasExtra : Form
    {
        private readonly ClsModeloHoraExtra _HoraExtra = new ClsModeloHoraExtra();
        private ClsPermisoAplicacion _MisPermisos;

        private DataTable _Empleados;
        private DataTable _AsistenciasExtra;
        private DataTable _Registros;
        private DataView _Vista;
        private DataView _VistaAsistencias;

        private int? _IdSeleccionado;
        private int? _IdEmpleado;
        private int? _IdAsistenciaInicial;
        private bool _LlenandoEmpleado;
        private bool _CargandoRegistro;
        private bool _HayCambios;
        private bool _RegistroBloqueado;

        private readonly Dictionary<Button, Image[]> _ImagenesBotones = new Dictionary<Button, Image[]>();
        private readonly HashSet<Button> _BotonesSinPermiso = new HashSet<Button>();
        private readonly HashSet<Control> _CamposConError = new HashSet<Control>();
        private Dictionary<Control, Label> _EtiquetasError;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr Ventana, int Mensaje, IntPtr Parametro1, string Parametro2);

        public FrmHorasExtra() : this(null)
        {
        }

        // Si viene una asistencia desde la ventana de Asistencia, se abre con esa asistencia lista
        public FrmHorasExtra(int? IdAsistencia)
        {
            InitializeComponent();
            _IdAsistenciaInicial = IdAsistencia;
            this.DoubleBuffered = true;
            this.Load += FrmHorasExtra_Load;
            this.Shown += FrmHorasExtra_Shown;
            this.FormClosing += FrmHorasExtra_FormClosing;
        }

        private void FrmHorasExtra_Load(object sender, EventArgs e)
        {
            RrhhMetPrepararImagenes();
            RrhhMetAplicarPermisos();
            RrhhMetRedondear(RrhhPnlDetalle, 14);
            RrhhMetRedondear(RrhhPnlRegistros, 14);
            SendMessage(RrhhTxtBuscar.Handle, 0x1501, (IntPtr)1, "Buscar por nombre o código...");

            _EtiquetasError = new Dictionary<Control, Label>
            {
                { RrhhTxtEmpleado, RrhhLblErrorEmpleado },
                { RrhhCboAsistencia, RrhhLblErrorAsistencia },
                { RrhhCboConcepto, RrhhLblErrorConcepto },
                { RrhhNudDuracion, RrhhLblErrorDuracion },
                { RrhhCboEstado, RrhhLblErrorEstado },
                { RrhhTxtObservaciones, RrhhLblErrorObservaciones }
            };

            RrhhMetCrearTabla();
            RrhhDtpDesde.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            RrhhDtpHasta.Value = RrhhDtpDesde.Value.AddMonths(1).AddDays(-1);
            RrhhCboFiltroEstado.SelectedIndex = 0;

            _Vista = new DataView(_Registros);
            RrhhDgvHorasExtra.DataSource = _Vista;
            RrhhMetCargarDatos();
            RrhhMetAplicarFiltro();
        }

        private void FrmHorasExtra_Shown(object sender, EventArgs e)
        {
            if (!_MisPermisos.TieneAcceso)
            {
                MessageBox.Show("No tiene acceso a la ventana de Horas Extra. Consulte con el administrador del sistema.",
                                "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _HayCambios = false;
                this.Close();
                return;
            }
            RrhhMetNuevoRegistro();
            if (_IdAsistenciaInicial.HasValue) RrhhMetPrepararDesdeAsistencia(_IdAsistenciaInicial.Value);
        }

        private void FrmHorasExtra_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_HayCambios) return;
            DialogResult Respuesta = MessageBox.Show("Hay cambios sin guardar. ¿Desea salir de todas formas?", "Horas Extra",
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
                this, ClsModeloHoraExtra.IdModulo, ClsModeloHoraExtra.IdAplicacion, MapaBotones);

            // Los que el helper dejó apagados no los vuelve a encender el estado de la ventana
            foreach (Control Boton in MapaBotones.Keys)
            {
                if (!Boton.Enabled) RrhhMetDeshabilitarBoton((Button)Boton);
            }
        }

        // ---------- Datos ----------

        private void RrhhMetCrearTabla()
        {
            _Registros = new DataTable();
            _Registros.Columns.Add("idHoraExtra", typeof(int));
            _Registros.Columns.Add("idEmpleado", typeof(int));
            _Registros.Columns.Add("codigoEmpleado", typeof(string));
            _Registros.Columns.Add("nombreEmpleado", typeof(string));
            _Registros.Columns.Add("fechaAsistencia", typeof(DateTime));
            _Registros.Columns.Add("jornada", typeof(string));
            _Registros.Columns.Add("horaSalida", typeof(string));
            _Registros.Columns.Add("duracion", typeof(string));
            _Registros.Columns.Add("valorHora", typeof(decimal));
            _Registros.Columns.Add("recargo", typeof(decimal));
            _Registros.Columns.Add("totalPago", typeof(decimal));
            _Registros.Columns.Add("estado", typeof(string));
            _Registros.Columns.Add("aprobadoPor", typeof(string));
            _Registros.Columns.Add("nomina", typeof(string));
            _Registros.Columns.Add("idAsistencia", typeof(int));
            _Registros.Columns.Add("idPercepcion", typeof(int));
            _Registros.Columns.Add("duracionMinutos", typeof(int));
            _Registros.Columns.Add("fechaAprobacion", typeof(DateTime));
            _Registros.Columns.Add("observaciones", typeof(string));
            _Registros.Columns.Add("bloqueada", typeof(bool));
        }

        private void RrhhMetCargarDatos()
        {
            try
            {
                _Empleados = _HoraExtra.RrhhMetObtenerEmpleados();
                _AsistenciasExtra = _HoraExtra.RrhhMetObtenerAsistencias();
                _VistaAsistencias = new DataView(_AsistenciasExtra, "idEmpleado = -1", "fecha DESC", DataViewRowState.CurrentRows);

                RrhhCboConcepto.SelectedIndexChanged -= RrhhCampo_Cambiado;
                RrhhCboConcepto.DataSource = _HoraExtra.RrhhMetObtenerConceptos();
                RrhhCboConcepto.DisplayMember = "nombrePercepcionDeduccion";
                RrhhCboConcepto.ValueMember = "idPercepcionDeduccion";
                RrhhCboConcepto.SelectedIndex = -1;
                RrhhCboConcepto.SelectedIndexChanged += RrhhCampo_Cambiado;

                RrhhMetLlenarEmpleados();
                RrhhMetCargarRegistros();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("No se pudieron cargar las horas extra.\n" + ClsModeloAsistencia.RrhhMetMensajeError(Ex), "Horas Extra",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RrhhMetCargarRegistros()
        {
            _CargandoRegistro = true;
            _Registros.BeginLoadData();
            _Registros.Clear();
            foreach (ClsModeloHoraExtra Item in _HoraExtra.RrhhMetObtenerTodos())
            {
                string Jornada = Item.HoraInicioJornada.HasValue && Item.HoraFinJornada.HasValue
                    ? $"{Item.HoraInicioJornada.Value:hh\\:mm} - {Item.HoraFinJornada.Value:hh\\:mm}" : "-";
                _Registros.Rows.Add(Item.IdHoraExtra, Item.IdEmpleado, Item.CodigoEmpleado, Item.NombreEmpleado,
                                    Item.FechaAsistencia.Date, Jornada,
                                    Item.HoraSalida.HasValue ? Item.HoraSalida.Value.ToString(@"hh\:mm") : "-",
                                    RrhhMetTextoDuracion(Item.Duracion), Item.ValorHora, Item.PorcentajeRecargo * 100,
                                    Item.TotalPago, Item.EstadoHoraExtra,
                                    string.IsNullOrEmpty(Item.UsuarioAprueba) ? "-" : Item.UsuarioAprueba,
                                    Item.IdNominaEncabezado.HasValue ? "#" + Item.IdNominaEncabezado.Value : "-",
                                    Item.IdAsistenciaEmpleado, Item.IdPercepcionDeduccion, Item.Duracion,
                                    Item.FechaAprobacion.HasValue ? (object)Item.FechaAprobacion.Value : DBNull.Value,
                                    Item.Observaciones, Item.Bloqueada);
            }
            _Registros.EndLoadData();
            _CargandoRegistro = false;
        }

        private static string RrhhMetTextoDuracion(int Minutos)
        {
            return $"{Minutos / 60}:{Minutos % 60:00}";
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

        // ---------- Detalle ----------

        private void RrhhMetNuevoRegistro()
        {
            _CargandoRegistro = true;
            RrhhDgvHorasExtra.ClearSelection();
            RrhhDgvHorasExtra.CurrentCell = null;
            _IdSeleccionado = null;
            _RegistroBloqueado = false;

            RrhhMetPonerEmpleado(null, null);
            RrhhTxtCodigo.Clear();
            RrhhTxtEmpleado.Clear();
            RrhhMetFiltrarAsistencias(null);
            RrhhCboConcepto.SelectedIndex = RrhhCboConcepto.Items.Count == 1 ? 0 : -1;
            RrhhNudDuracion.Maximum = ClsModeloHoraExtra.DuracionMaxima;
            RrhhNudDuracion.Value = 0;
            RrhhNudRecargo.Value = ClsModeloHoraExtra.RecargoPorDefecto * 100;
            RrhhCboEstado.SelectedItem = "Pendiente";
            foreach (TextBox Campo in new[] { RrhhTxtObservaciones, RrhhTxtAprobadoPor, RrhhTxtFechaAprobacion })
                Campo.Clear();
            RrhhTxtAprobadoPor.Text = "-";
            RrhhTxtFechaAprobacion.Text = "-";
            RrhhTxtNomina.Text = "Sin asignar";
            _CargandoRegistro = false;

            RrhhMetBloquearDetalle(false);
            RrhhMetLimpiarErrores();
            RrhhMetMostrarAsistencia();
            _HayCambios = false;
            RrhhMetActualizarBotones();
        }

        private void RrhhMetPrepararDesdeAsistencia(int IdAsistencia)
        {
            DataRow[] Filas = _AsistenciasExtra?.Select("idAsistencia = " + IdAsistencia) ?? new DataRow[0];
            if (Filas.Length == 0)
            {
                MessageBox.Show("La asistencia seleccionada no genera hora extra: el empleado no salió después de su jornada " +
                                "o su estado no es Presente o Tardanza.", "Horas Extra", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Filas[0]["idHoraExtra"] != DBNull.Value)
            {
                RrhhMetSeleccionarRegistro((int)Filas[0]["idHoraExtra"]);
                return;
            }
            RrhhMetElegirEmpleado(RrhhMetBuscarEmpleado((int)Filas[0]["idEmpleado"]), null);
            RrhhCboAsistencia.SelectedValue = IdAsistencia;
        }

        private void RrhhMetSeleccionarRegistro(int IdHoraExtra)
        {
            RrhhCboFiltroEstado.SelectedIndex = 0;
            RrhhTxtFiltroEmpleado.Clear();
            RrhhTxtBuscar.Clear();
            DataRow[] Filas = _Registros.Select("idHoraExtra = " + IdHoraExtra);
            if (Filas.Length > 0)
            {
                DateTime Fecha = (DateTime)Filas[0]["fechaAsistencia"];
                if (Fecha < RrhhDtpDesde.Value.Date) RrhhDtpDesde.Value = Fecha;
                if (Fecha > RrhhDtpHasta.Value.Date) RrhhDtpHasta.Value = Fecha;
            }
            RrhhMetAplicarFiltro();
            foreach (DataGridViewRow Fila in RrhhDgvHorasExtra.Rows)
            {
                if (Fila.DataBoundItem is DataRowView Registro && (int)Registro["idHoraExtra"] == IdHoraExtra)
                {
                    RrhhDgvHorasExtra.CurrentCell = Fila.Cells[1];
                    Fila.Selected = true;
                    break;
                }
            }
        }

        private void RrhhDgvHorasExtra_SelectionChanged(object sender, EventArgs e)
        {
            if (_CargandoRegistro) return;
            DataGridViewRow Fila = RrhhDgvHorasExtra.CurrentRow;
            if (Fila == null || !Fila.Selected || !(Fila.DataBoundItem is DataRowView Registro)) return;
            RrhhMetMostrarDetalle(Registro.Row);
        }

        private void RrhhMetMostrarDetalle(DataRow Fila)
        {
            _CargandoRegistro = true;
            _IdSeleccionado = (int)Fila["idHoraExtra"];
            _RegistroBloqueado = (bool)Fila["bloqueada"];

            DataRow Empleado = RrhhMetBuscarEmpleado((int)Fila["idEmpleado"]);
            RrhhMetPonerEmpleado(Empleado, null);
            if (Empleado == null)
            {
                _IdEmpleado = (int)Fila["idEmpleado"];
                RrhhTxtCodigo.Text = Fila["codigoEmpleado"].ToString();
                RrhhTxtEmpleado.Text = Fila["nombreEmpleado"].ToString();
            }
            RrhhMetFiltrarAsistencias((int)Fila["idAsistencia"]);
            RrhhCboAsistencia.SelectedValue = (int)Fila["idAsistencia"];
            RrhhCboConcepto.SelectedValue = (int)Fila["idPercepcion"];

            int Minutos = (int)Fila["duracionMinutos"];
            RrhhNudDuracion.Maximum = Math.Max(Minutos, RrhhMetMinutosDisponibles());
            RrhhNudDuracion.Value = Minutos;
            RrhhNudRecargo.Value = Math.Min(RrhhNudRecargo.Maximum, (decimal)Fila["recargo"]);

            string Estado = Fila["estado"].ToString();
            if (!RrhhCboEstado.Items.Contains(Estado)) RrhhCboEstado.Items.Add(Estado);
            RrhhCboEstado.SelectedItem = Estado;
            RrhhTxtAprobadoPor.Text = Fila["aprobadoPor"].ToString();
            RrhhTxtFechaAprobacion.Text = Fila["fechaAprobacion"] == DBNull.Value
                ? "-" : ((DateTime)Fila["fechaAprobacion"]).ToString("dd/MM/yyyy HH:mm");
            RrhhTxtNomina.Text = Fila["nomina"].ToString() == "-" ? "Sin asignar" : "Nómina " + Fila["nomina"];
            RrhhTxtObservaciones.Text = Fila["observaciones"].ToString();
            _CargandoRegistro = false;

            RrhhMetMostrarAsistencia();
            RrhhTxtValorHora.Text = ((decimal)Fila["valorHora"]).ToString("Q #,##0.00");
            RrhhMetActualizarTotal();
            RrhhMetBloquearDetalle(_RegistroBloqueado);
            RrhhMetLimpiarErrores();
            _HayCambios = false;
            RrhhMetActualizarBotones();
        }

        // Una hora extra que Nómina ya tomó queda solo para consulta
        private void RrhhMetBloquearDetalle(bool Bloquear)
        {
            foreach (Control Campo in new Control[] { RrhhTxtCodigo, RrhhTxtEmpleado, RrhhCboAsistencia, RrhhCboConcepto,
                                                      RrhhNudDuracion, RrhhNudRecargo, RrhhCboEstado, RrhhTxtObservaciones })
                Campo.Enabled = !Bloquear;
            // Al modificar no se cambia de empleado
            if (!Bloquear && _IdSeleccionado != null)
            {
                RrhhTxtCodigo.Enabled = false;
                RrhhTxtEmpleado.Enabled = false;
            }
        }

        private DataRow RrhhMetBuscarEmpleado(int IdEmpleado)
        {
            if (_Empleados == null) return null;
            DataRow[] Filas = _Empleados.Select("idEmpleado = " + IdEmpleado);
            return Filas.Length > 0 ? Filas[0] : null;
        }

        private DataRow RrhhMetBuscarEmpleadoPor(string Columna, string Texto)
        {
            if (_Empleados == null) return null;
            DataRow[] Filas = _Empleados.Select($"{Columna} = '{Texto.Trim().Replace("'", "''")}'");
            return Filas.Length > 0 ? Filas[0] : null;
        }

        private void RrhhTxtCodigo_TextChanged(object sender, EventArgs e)
        {
            if (_LlenandoEmpleado || _CargandoRegistro || _Empleados == null) return;
            RrhhMetElegirEmpleado(RrhhMetBuscarEmpleadoPor("codigoEmpleado", RrhhTxtCodigo.Text), RrhhTxtCodigo);
        }

        private void RrhhTxtEmpleado_TextChanged(object sender, EventArgs e)
        {
            if (_LlenandoEmpleado || _CargandoRegistro || _Empleados == null) return;
            RrhhMetElegirEmpleado(RrhhMetBuscarEmpleadoPor("nombreEmpleado", RrhhTxtEmpleado.Text), RrhhTxtEmpleado);
        }

        private void RrhhMetElegirEmpleado(DataRow Empleado, TextBox Origen)
        {
            int? Id = Empleado == null ? (int?)null : (int)Empleado["idEmpleado"];
            if (Id == _IdEmpleado) return;
            RrhhMetPonerEmpleado(Empleado, Origen);
            RrhhMetFiltrarAsistencias(null);
            RrhhMetMostrarAsistencia();
            RrhhMetCampoCambiado(RrhhTxtEmpleado);
            if (Empleado != null && RrhhCboAsistencia.Items.Count == 0)
                RrhhMetMarcarError(RrhhCboAsistencia, "Este empleado no tiene asistencias pendientes con hora extra.");
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
            _LlenandoEmpleado = false;
        }

        // Asistencias del empleado que todavía no tienen hora extra (más la del registro que se está viendo)
        private void RrhhMetFiltrarAsistencias(int? IdAsistenciaActual)
        {
            if (_VistaAsistencias == null) return;
            bool Antes = _CargandoRegistro;
            _CargandoRegistro = true;
            string Filtro = $"idEmpleado = {(_IdEmpleado ?? -1)} AND (idHoraExtra IS NULL";
            Filtro += IdAsistenciaActual.HasValue ? $" OR idAsistencia = {IdAsistenciaActual.Value})" : ")";
            _VistaAsistencias.RowFilter = Filtro;
            RrhhCboAsistencia.DataSource = null;
            RrhhCboAsistencia.DisplayMember = "textoFecha";
            RrhhCboAsistencia.ValueMember = "idAsistencia";
            RrhhCboAsistencia.DataSource = _VistaAsistencias;
            RrhhCboAsistencia.SelectedIndex = _VistaAsistencias.Count == 1 && !IdAsistenciaActual.HasValue ? 0 : -1;
            _CargandoRegistro = Antes;
        }

        private DataRowView RrhhMetAsistenciaActual()
        {
            return RrhhCboAsistencia.SelectedItem as DataRowView;
        }

        private int RrhhMetMinutosDisponibles()
        {
            DataRowView Asistencia = RrhhMetAsistenciaActual();
            return Asistencia == null ? 0 : (int)Asistencia["minutosExtra"];
        }

        private void RrhhCboAsistencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_CargandoRegistro) return;
            RrhhMetMostrarAsistencia();
            int Disponibles = RrhhMetMinutosDisponibles();
            _CargandoRegistro = true;
            RrhhNudDuracion.Maximum = Math.Max(1, Disponibles);
            RrhhNudDuracion.Value = Disponibles;
            _CargandoRegistro = false;
            RrhhMetActualizarTotal();
            RrhhMetCampoCambiado(RrhhCboAsistencia);
            RrhhMetQuitarError(RrhhNudDuracion);
        }

        private void RrhhMetMostrarAsistencia()
        {
            DataRowView Asistencia = RrhhMetAsistenciaActual();
            if (Asistencia == null)
            {
                RrhhTxtJornada.Clear();
                RrhhTxtEntradaSalida.Clear();
                RrhhTxtValorHora.Clear();
                RrhhTxtTotal.Clear();
                return;
            }
            string Entrada = Asistencia["horaEntrada"] == DBNull.Value ? "--:--" : ((TimeSpan)Asistencia["horaEntrada"]).ToString(@"hh\:mm");
            RrhhTxtJornada.Text = Asistencia["jornada"].ToString();
            RrhhTxtEntradaSalida.Text = $"{Entrada} – {(TimeSpan)Asistencia["horaSalida"]:hh\\:mm}";
            RrhhTxtValorHora.Text = ((decimal)Asistencia["valorHora"]).ToString("Q #,##0.00");
            RrhhMetActualizarTotal();
        }

        private decimal RrhhMetValorHoraActual()
        {
            if (_IdSeleccionado != null)
            {
                // Al consultar o modificar se respeta el valor con el que se registró
                DataRow[] Filas = _Registros.Select("idHoraExtra = " + _IdSeleccionado);
                if (Filas.Length > 0) return (decimal)Filas[0]["valorHora"];
            }
            DataRowView Asistencia = RrhhMetAsistenciaActual();
            return Asistencia == null ? 0m : (decimal)Asistencia["valorHora"];
        }

        private void RrhhMetActualizarTotal()
        {
            if (RrhhMetAsistenciaActual() == null) { RrhhTxtTotal.Clear(); return; }
            decimal Total = ClsModeloHoraExtra.RrhhMetCalcularTotal((int)RrhhNudDuracion.Value, RrhhMetValorHoraActual(),
                                                                      RrhhNudRecargo.Value / 100);
            RrhhTxtTotal.Text = Total.ToString("Q #,##0.00");
        }

        private void RrhhCalculo_Cambiado(object sender, EventArgs e)
        {
            if (_CargandoRegistro) return;
            RrhhMetActualizarTotal();
            RrhhMetCampoCambiado((Control)sender);
        }

        private void RrhhCampo_Cambiado(object sender, EventArgs e)
        {
            if (_CargandoRegistro) return;
            RrhhMetCampoCambiado((Control)sender);
        }

        private void RrhhCboEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_CargandoRegistro) return;
            string Estado = RrhhCboEstado.SelectedItem?.ToString() ?? "";
            if (Estado == "Pendiente")
            {
                RrhhTxtAprobadoPor.Text = "-";
                RrhhTxtFechaAprobacion.Text = "-";
            }
            else
            {
                RrhhTxtAprobadoPor.Text = string.IsNullOrEmpty(ClsSesionSeguridad.NombreUsuario) ? "-" : ClsSesionSeguridad.NombreUsuario;
                RrhhTxtFechaAprobacion.Text = "Al guardar";
            }
            RrhhMetCampoCambiado(RrhhCboEstado);
        }

        private void RrhhMetCampoCambiado(Control Campo)
        {
            _HayCambios = true;
            RrhhMetQuitarError(Campo);
            RrhhMetActualizarBotones();
        }

        // ---------- Validación ----------

        private bool RrhhMetValidar()
        {
            RrhhMetLimpiarErrores();
            DataRowView Asistencia = RrhhMetAsistenciaActual();

            if (_IdEmpleado == null)
                RrhhMetMarcarError(RrhhTxtEmpleado, "Seleccione un empleado por código o nombre.");
            if (Asistencia == null)
                RrhhMetMarcarError(RrhhCboAsistencia, "Seleccione la fecha de asistencia.");
            if (RrhhCboConcepto.SelectedValue == null)
                RrhhMetMarcarError(RrhhCboConcepto, RrhhCboConcepto.Items.Count == 0
                    ? "No hay conceptos de percepción activos en Nómina." : "Seleccione el concepto de pago.");

            if (RrhhNudDuracion.Value <= 0)
                RrhhMetMarcarError(RrhhNudDuracion, "La duración debe ser mayor a 0 minutos.");
            else if (Asistencia != null && RrhhNudDuracion.Value > (int)Asistencia["minutosExtra"])
                RrhhMetMarcarError(RrhhNudDuracion, $"Máximo {(int)Asistencia["minutosExtra"]} min (lo trabajado después de la jornada, hasta 12 h en el día).");
            else if (Asistencia != null && RrhhMetValorHoraActual() <= 0)
                RrhhMetMarcarError(RrhhNudDuracion, "El empleado no tiene salario registrado.");

            if (RrhhCboEstado.SelectedItem == null || RrhhCboEstado.SelectedItem.ToString() == "Pagada")
                RrhhMetMarcarError(RrhhCboEstado, "Seleccione Pendiente, Aprobada o Rechazada.");
            if (RrhhCboEstado.SelectedItem?.ToString() == "Rechazada" && RrhhTxtObservaciones.Text.Trim().Length == 0)
                RrhhMetMarcarError(RrhhTxtObservaciones, "Escriba el motivo del rechazo.");

            return _CamposConError.Count == 0;
        }

        private void RrhhMetMarcarError(Control Campo, string Mensaje)
        {
            _CamposConError.Add(Campo);
            Label Etiqueta = _EtiquetasError[Campo];
            Etiqueta.Text = Mensaje;
            Etiqueta.Visible = true;
            Campo.Parent.Invalidate();
        }

        private void RrhhMetQuitarError(Control Campo)
        {
            if (!_CamposConError.Remove(Campo)) return;
            _EtiquetasError[Campo].Visible = false;
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

        // ---------- Botones ----------

        private void RrhhMetActualizarBotones()
        {
            RrhhMetHabilitarBoton(RrhhBtnGuardar, _HayCambios && _IdSeleccionado == null);
            RrhhMetHabilitarBoton(RrhhBtnModificar, _IdSeleccionado != null && !_RegistroBloqueado);
            RrhhMetHabilitarBoton(RrhhBtnEliminar, _IdSeleccionado != null && !_RegistroBloqueado);
        }

        private void RrhhMetHabilitarBoton(Button Boton, bool Habilitar)
        {
            Habilitar = Habilitar && !_BotonesSinPermiso.Contains(Boton);
            Boton.Enabled = Habilitar;
            if (_ImagenesBotones.TryGetValue(Boton, out Image[] Imagenes))
                Boton.BackgroundImage = Imagenes[Habilitar ? 0 : 2];
        }

        public void RrhhMetDeshabilitarBoton(Button Boton)
        {
            _BotonesSinPermiso.Add(Boton);
            RrhhMetHabilitarBoton(Boton, false);
        }

        private ClsModeloHoraExtra RrhhMetDetalleAModelo()
        {
            DataRowView Asistencia = RrhhMetAsistenciaActual();
            return new ClsModeloHoraExtra
            {
                IdEmpleado = _IdEmpleado.Value,
                IdAsistenciaEmpleado = (int)Asistencia["idAsistencia"],
                IdPercepcionDeduccion = Convert.ToInt32(RrhhCboConcepto.SelectedValue),
                Duracion = (int)RrhhNudDuracion.Value,
                ValorHora = RrhhMetValorHoraActual(),
                PorcentajeRecargo = RrhhNudRecargo.Value / 100,
                EstadoHoraExtra = RrhhCboEstado.SelectedItem.ToString(),
                Observaciones = RrhhTxtObservaciones.Text.Trim()
            };
        }

        private void RrhhBtnGuardar_Click(object sender, EventArgs e)
        {
            if (!RrhhMetValidar()) return;
            ClsModeloHoraExtra HoraExtra = RrhhMetDetalleAModelo();
            HoraExtra.Estado = EstadoEntidad.Added;
            RrhhMetGrabar(HoraExtra);
        }

        private void RrhhBtnModificar_Click(object sender, EventArgs e)
        {
            if (_IdSeleccionado == null) return;
            if (!_HayCambios)
            {
                MessageBox.Show("No hay cambios para modificar.", "Horas Extra", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!RrhhMetValidar()) return;
            ClsModeloHoraExtra HoraExtra = RrhhMetDetalleAModelo();
            HoraExtra.IdHoraExtra = _IdSeleccionado.Value;
            HoraExtra.Estado = EstadoEntidad.Modified;
            RrhhMetGrabar(HoraExtra);
        }

        private void RrhhBtnEliminar_Click(object sender, EventArgs e)
        {
            if (_IdSeleccionado == null) return;
            DataRow[] Filas = _Registros.Select("idHoraExtra = " + _IdSeleccionado);
            if (Filas.Length == 0) return;

            DialogResult Respuesta = MessageBox.Show(
                $"¿Seguro que desea eliminar la hora extra de {Filas[0]["nombreEmpleado"]} del " +
                $"{(DateTime)Filas[0]["fechaAsistencia"]:dd/MM/yyyy}? Esta acción no se puede deshacer.",
                "Eliminar hora extra", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Respuesta != DialogResult.Yes) return;

            RrhhMetGrabar(new ClsModeloHoraExtra { IdHoraExtra = _IdSeleccionado.Value, Estado = EstadoEntidad.Deleted });
        }

        // ---------- Reportes ----------

        // Datos del reporte: lo que la lista tiene filtrado en este momento
        private DataTable RrhhMetDatosReporte()
        {
            var Datos = new DataTable("DsHorasExtra");
            Datos.Columns.Add("Codigo", typeof(string));
            Datos.Columns.Add("Empleado", typeof(string));
            Datos.Columns.Add("Fecha", typeof(DateTime));
            Datos.Columns.Add("Jornada", typeof(string));
            Datos.Columns.Add("Salida", typeof(string));
            Datos.Columns.Add("Duracion", typeof(string));
            Datos.Columns.Add("ValorHora", typeof(decimal));
            Datos.Columns.Add("Recargo", typeof(string));
            Datos.Columns.Add("TotalPago", typeof(decimal));
            Datos.Columns.Add("Estado", typeof(string));
            Datos.Columns.Add("AprobadoPor", typeof(string));
            Datos.Columns.Add("Periodo", typeof(string));

            string Periodo = $"Del {RrhhDtpDesde.Value:dd/MM/yyyy} al {RrhhDtpHasta.Value:dd/MM/yyyy}";
            if (RrhhCboFiltroEstado.SelectedIndex > 0) Periodo += $"  ·  Estado: {RrhhCboFiltroEstado.SelectedItem}";
            foreach (DataRowView Fila in _Vista)
            {
                Datos.Rows.Add(Fila["codigoEmpleado"], Fila["nombreEmpleado"], Fila["fechaAsistencia"], Fila["jornada"],
                               Fila["horaSalida"], Fila["duracion"], Fila["valorHora"], $"{(decimal)Fila["recargo"]:0.##} %",
                               Fila["totalPago"], Fila["estado"], Fila["aprobadoPor"], Periodo);
            }
            return Datos;
        }

        // Reporte: igual que en Seguridad, abre la ventana del reporte .rdlc con lo que la lista tiene filtrado
        private void RrhhBtnReporte_Click(object sender, EventArgs e)
        {
            try
            {
                var Reporte = new FrmReporteHorasExtra(RrhhMetDatosReporte());
                Reporte.Show();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error al abrir el reporte: " + Ex.Message, "Reporte",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RrhhBtnAyuda_Click(object sender, EventArgs e)
        {
            FrmAsistencia.RrhhMetAbrirAyuda(this, "HorasExtra__RecursosHumanos.html");
        }

        private void RrhhMetGrabar(ClsModeloHoraExtra HoraExtra)
        {
            Cursor = Cursors.WaitCursor;
            string Mensaje = HoraExtra.RrhhMetGrabarCambios();
            Cursor = Cursors.Default;

            if (!HoraExtra.Grabado)
            {
                MessageBox.Show(Mensaje, "Horas Extra", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MessageBox.Show(Mensaje, "Horas Extra", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RrhhMetDespuesDeGuardar();
        }

        private void RrhhMetDespuesDeGuardar()
        {
            _HayCambios = false;
            RrhhMetCargarDatos();
            RrhhMetNuevoRegistro();
            RrhhMetAplicarFiltro();
        }

        private void RrhhBtnRefrescar_Click(object sender, EventArgs e)
        {
            RrhhMetCargarDatos();
            RrhhMetNuevoRegistro();
            RrhhMetAplicarFiltro();
        }

        private void RrhhBtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ---------- Lista ----------

        private void RrhhDgvHorasExtra_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null || e.ColumnIndex != RrhhColEstado.Index) return;
            e.CellStyle.BackColor = RrhhMetColorEstado(e.Value.ToString());
            e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
        }

        private static Color RrhhMetColorEstado(string Estado)
        {
            switch (Estado)
            {
                case "Pendiente": return Color.FromArgb(252, 235, 201);
                case "Aprobada": return Color.FromArgb(214, 242, 224);
                case "Rechazada": return Color.FromArgb(248, 215, 218);
                case "Pagada": return Color.FromArgb(221, 235, 247);
                default: return Color.White;
            }
        }

        private void RrhhBtnFiltrar_Click(object sender, EventArgs e)
        {
            if (RrhhDtpHasta.Value.Date < RrhhDtpDesde.Value.Date)
            {
                MessageBox.Show("La fecha Hasta no puede ser menor que la fecha Desde.", "Horas Extra",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
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
                Filtro += $" AND estado = '{RrhhCboFiltroEstado.SelectedItem}'";
            string Buscar = RrhhMetTextoFiltro(RrhhTxtBuscar.Text);
            if (Buscar.Length > 0)
                Filtro += $" AND (nombreEmpleado LIKE '%{Buscar}%' OR codigoEmpleado LIKE '%{Buscar}%')";

            _CargandoRegistro = true;
            _Vista.RowFilter = Filtro;
            _CargandoRegistro = false;
            if (_IdSeleccionado == null) RrhhDgvHorasExtra.ClearSelection();
            RrhhMetActualizarTotalRegistros();
        }

        private static string RrhhMetTextoFiltro(string Texto)
        {
            return Texto.Trim().Replace("'", "''").Replace("[", "").Replace("]", "").Replace("*", "").Replace("%", "");
        }

        private void RrhhMetActualizarTotalRegistros()
        {
            int Total = _Vista.Count;
            decimal Suma = 0m;
            foreach (DataRowView Fila in _Vista)
            {
                if (Fila["estado"].ToString() != "Rechazada") Suma += (decimal)Fila["totalPago"];
            }
            RrhhLblTotalRegistros.Text = Total == 0
                ? "No hay registros"
                : $"Mostrando 1 - {Total} de {Total} registros  ·  Total del período (sin rechazadas): Q {Suma:#,##0.00}";
        }

        private void RrhhBtnInicio_Click(object sender, EventArgs e)
        {
            RrhhMetIrAFila(0);
        }

        private void RrhhBtnAnterior_Click(object sender, EventArgs e)
        {
            RrhhMetIrAFila((RrhhDgvHorasExtra.CurrentRow?.Index ?? 1) - 1);
        }

        private void RrhhBtnSiguiente_Click(object sender, EventArgs e)
        {
            RrhhMetIrAFila((RrhhDgvHorasExtra.CurrentRow?.Index ?? -1) + 1);
        }

        private void RrhhBtnFin_Click(object sender, EventArgs e)
        {
            RrhhMetIrAFila(RrhhDgvHorasExtra.Rows.Count - 1);
        }

        private void RrhhMetIrAFila(int Indice)
        {
            int Total = RrhhDgvHorasExtra.Rows.Count;
            if (Total == 0) return;
            Indice = Math.Max(0, Math.Min(Total - 1, Indice));
            RrhhDgvHorasExtra.CurrentCell = RrhhDgvHorasExtra.Rows[Indice].Cells[1];
            RrhhDgvHorasExtra.Rows[Indice].Selected = true;
        }

        // ---------- Imágenes y bordes (igual que Asistencia) ----------

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
