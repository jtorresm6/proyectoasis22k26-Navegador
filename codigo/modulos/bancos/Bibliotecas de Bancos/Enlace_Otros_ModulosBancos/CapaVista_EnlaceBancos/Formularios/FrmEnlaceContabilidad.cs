// =====================================================================
// Autor:       Andre De Jesus Gonzalez Camey
// Carné:       9959-23-3117
// Módulo:      Tesorería y Bancos – Enlace con Contabilidad
// Descripción: Formulario principal del enlace: consulta pólizas bancarias del período,
//              genera la partida contable y la envía a Contabilidad. Incluye ayuda y
//              reporte.
// Fecha:       12/11/2026
// =====================================================================
using System;
using System.IO;
using System.Windows.Forms;
using CapaControlador_EnlaceB;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaModelo_EnlaceB.Entidades;
using CapaVista_EnlaceB.frmReportes;

namespace CapaVista_EnlaceB.Formularios
{
    public partial class FrmEnlaceContabilidad : Form
    {
        private readonly ClsControladorEnlaceB _Controlador;
        private readonly ClsPaginadorEnlace _PaginasMovimientos;
        private readonly ClsPaginadorEnlace _PaginasPartida;
        private long _Partida;
        private bool _Ocupado, _Rellenando, _PuedeEnviar;
        public FrmEnlaceContabilidad() : this(new ClsControladorEnlaceB())
        {
        }

        public FrmEnlaceContabilidad(ClsControladorEnlaceB Controlador)
        {
            _Controlador = Controlador ?? throw new ArgumentNullException(nameof(Controlador));
            InitializeComponent();
            _PaginasMovimientos = new ClsPaginadorEnlace(_BancarioDgvMovimientos, _BancarioBtnAnteriorMovimientos, _BancarioBtnSiguienteMovimientos, _BancarioLblPaginaMovimientos);
            _PaginasPartida = new ClsPaginadorEnlace(_BancarioDgvPartidaContable, _BancarioBtnAnteriorPartida, _BancarioBtnSiguientePartida, _BancarioLblPaginaPartida);
            _BancarioCboPeriodo.Items.Add("Mensual");
            _BancarioCboPeriodo.SelectedIndex = 0;
            _BancarioDtpFechaInicial.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            _BancarioDtpFechaFinal.Value = _BancarioDtpFechaInicial.Value.AddMonths(1).AddDays(-1);
            _BancarioBtnGenerarPartida.Enabled = false;
            _BancarioBtnEnviar.Enabled = false;
            _BancarioLblUsuario.Text = "Usuario: " + (ClsSesionSeguridad.HaySesionActiva ? ClsSesionSeguridad.NombreUsuario : "sin sesión");
            _BancarioDtpFechaInicial.ValueChanged += BancarioMetCambioFechas;
            _BancarioDtpFechaFinal.ValueChanged += BancarioMetCambioFechas;
            _BancarioCboEmpresa.SelectedIndexChanged += BancarioMetCambioSeleccion;
            _BancarioCboPais.SelectedIndexChanged += BancarioMetCambioSeleccion;
            _BancarioCboModulo.SelectedIndexChanged += BancarioMetCambioSeleccion;
            Shown += BancarioMetCatalogos;
        }

        private void BancarioMetCambioFechas(object Remitente, EventArgs Argumentos)
        {
            _PaginasMovimientos.BancarioMetCargar(null);
            BancarioMetCambioSeleccion(Remitente, Argumentos);
        }

        private void BancarioMetCambioSeleccion(object Remitente, EventArgs Argumentos)
        {
            ClsEstiloEnlace.BancarioMetValidacion(_BancarioPnlBordeFechaFinal, _BancarioLblErrorFechaFinal, "");
            if (Remitente == _BancarioCboEmpresa)
                ClsEstiloEnlace.BancarioMetValidacion(_BancarioPnlBordeEmpresa, _BancarioLblErrorEmpresa, "");
            if (Remitente == _BancarioCboPais)
                ClsEstiloEnlace.BancarioMetValidacion(_BancarioPnlBordePais, _BancarioLblErrorPais, "");
            if (Remitente == _BancarioCboModulo)
                ClsEstiloEnlace.BancarioMetValidacion(_BancarioPnlBordeModulo, _BancarioLblErrorModulo, "");
            BancarioMetInvalidar();
        }

        private void BancarioMetHabilitarAcciones(bool Habilitar)
        {
            _BancarioCboEmpresa.Enabled = Habilitar;
            _BancarioCboPais.Enabled = Habilitar;
            _BancarioCboModulo.Enabled = Habilitar;
            _BancarioDtpFechaInicial.Enabled = Habilitar;
            _BancarioDtpFechaFinal.Enabled = Habilitar;
            _BancarioBtnConsultarMovimientos.Enabled = Habilitar;
            _BancarioBtnCatalogos.Enabled = Habilitar;
            _BancarioBtnHistorial.Enabled = Habilitar;
            _BancarioBtnLimpiar.Enabled = Habilitar;
            _BancarioBtnCerrar.Enabled = Habilitar;
            _BancarioBtnGenerarPartida.Enabled = Habilitar && BancarioMetCatalogosSeleccionados();
            _BancarioBtnEnviar.Enabled = Habilitar && _PuedeEnviar && _Partida > 0 && BancarioMetCatalogosSeleccionados();
        }

        private async void BancarioMetCatalogos(object Remitente, EventArgs Argumentos)
        {
            await BancarioMetCargarCatalogos();
        }

        private async void BancarioMetHistorialClick(object Remitente, EventArgs Argumentos)
        {
            await BancarioMetHistorial();
        }

        private void BancarioMetCerrar(object Remitente, EventArgs Argumentos)
        {
            Close();
        }

        private void BancarioMetReporte(object Remitente, EventArgs Argumentos)
        {
            using (FrmReporteEnlaceContabilidad Reporte = new FrmReporteEnlaceContabilidad())
            {
                Reporte.ShowDialog(this);
            }
        }

        private void BancarioMetAyuda(object Remitente, EventArgs Argumentos)
        {
            DirectoryInfo Directorio = new DirectoryInfo(Application.StartupPath);
            while (Directorio != null)
            {
                string Ruta = Path.Combine(Directorio.FullName, "ayuda", "modulos", "bancos", "BancosAyudas", "BancosAyudas.chm");
                if (File.Exists(Ruta))
                {
                    Help.ShowHelp(this, Ruta, HelpNavigator.Topic, "EnlaceContabilidad_Bancos.htm");
                    return;
                }
                Directorio = Directorio.Parent;
            }
            MessageBox.Show("No se encontró el archivo de ayuda del módulo de Bancos.", "Ayuda de enlace con Contabilidad",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BancarioMetLimpiar(object Remitente, EventArgs Argumentos)
        {
            BancarioMetInvalidar();
            _PaginasMovimientos.BancarioMetCargar(null);
            _BancarioLblResultado.Text = "Sin partida seleccionada.";
        }

        private async void BancarioMetConsultar(object Remitente, EventArgs Argumentos)
        {
            await BancarioMetEjecutar(async () =>
            {
                if (!BancarioMetValidarFormulario(false))
                    return;
                DateTime Inicio = _BancarioDtpFechaInicial.Value.Date, Fin = _BancarioDtpFechaFinal.Value.Date;
                var Datos = await Task.Run(() => _Controlador.BancarioMetConsultarPolizas(Inicio, Fin));
                _PaginasMovimientos.BancarioMetCargar(Datos);
                _BancarioLblResultado.Text = Datos.Rows.Count + " pólizas consultadas. Revise la columna Resultado.";
            });
        }

        private async void BancarioMetGenerar(object Remitente, EventArgs Argumentos)
        {
            await BancarioMetEjecutar(async () =>
            {
                if (!BancarioMetValidarFormulario(true))
                    return;
                var Contexto = BancarioMetContexto();
                if (MessageBox.Show(this, "Se guardará la partida GTQ del mes con todas las pólizas validadas disponibles. ¿Continuar?", "Generar partida", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;
                BancarioMetMostrar(await Task.Run(() => _Controlador.BancarioMetGenerar(Contexto)));
                ClsEstiloEnlace.BancarioMetExito(this, "La partida se generó correctamente.");
            });
        }

        private async void BancarioMetEnviar(object Remitente, EventArgs Argumentos)
        {
            await BancarioMetEjecutar(async () =>
            {
                if (!BancarioMetValidarFormulario(true))
                    return;
                var Contexto = BancarioMetContexto();
                long Id = _Partida;
                if (MessageBox.Show(this, "¿Confirma el envío de la partida " + Id + " a Contabilidad? Según la configuración del módulo, puede aplicarse a saldos de inmediato.", "Confirmar traslado", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;
                var Resultado = await Task.Run(() => _Controlador.BancarioMetEnviarConResultado(Id, Contexto));
                BancarioMetMostrar(await Task.Run(() => _Controlador.BancarioMetObtener(Id)));
                _BancarioLblResultado.Text = "Póliza " + Resultado.Codigo + " · " + Resultado.Estado;
                ClsEstiloEnlace.BancarioMetExito(this, Resultado.Mensaje);
            });
        }

        private ClsContextoPartida BancarioMetContexto()
        {
            if (!ClsSesionSeguridad.HaySesionActiva)
                throw new InvalidOperationException("Inicie sesión en el sistema para generar o enviar partidas.");
            return new ClsContextoPartida
            {
                Empresa = _BancarioCboEmpresa.SelectedValue == null ? 0 : Convert.ToInt32(_BancarioCboEmpresa.SelectedValue),
                Pais = _BancarioCboPais.SelectedValue == null ? 0 : Convert.ToInt32(_BancarioCboPais.SelectedValue),
                ModuloContable = _BancarioCboModulo.SelectedValue == null ? 0 : Convert.ToInt32(_BancarioCboModulo.SelectedValue),
                Responsable = ClsSesionSeguridad.NombreUsuario,
                UsuarioId = ClsSesionSeguridad.IdUsuario,
                Ip = ClsSesionSeguridad.SeguridadMetObtenerIPLocal(),
                Inicio = _BancarioDtpFechaInicial.Value.Date,
                Fin = _BancarioDtpFechaFinal.Value.Date
            };
        }

        private bool BancarioMetValidarFormulario(bool Mensual)
        {
            DateTime Inicio = _BancarioDtpFechaInicial.Value.Date, Fin = _BancarioDtpFechaFinal.Value.Date;
            bool FechasValidas = Inicio <= Fin && Fin.Year < 9999 && (!Mensual || (Inicio.Day == 1 && Fin == Inicio.AddMonths(1).AddDays(-1)));
            ClsEstiloEnlace.BancarioMetValidacion(_BancarioPnlBordeFechaFinal, _BancarioLblErrorFechaFinal, FechasValidas ? "" : Mensual ? "Seleccione un mes completo." : "Revise el rango de fechas.");
            if (!FechasValidas)
            {
                _BancarioLblResultado.Text = "Corrija las fechas antes de continuar.";
                _BancarioDtpFechaFinal.Focus();
                return false;
            }

            if (!Mensual)
                return true;
            ClsEstiloEnlace.BancarioMetValidacion(_BancarioPnlBordeEmpresa, _BancarioLblErrorEmpresa, _BancarioCboEmpresa.SelectedIndex >= 0 ? "" : "Seleccione una opción.");
            ClsEstiloEnlace.BancarioMetValidacion(_BancarioPnlBordePais, _BancarioLblErrorPais, _BancarioCboPais.SelectedIndex >= 0 ? "" : "Seleccione una opción.");
            ClsEstiloEnlace.BancarioMetValidacion(_BancarioPnlBordeModulo, _BancarioLblErrorModulo, _BancarioCboModulo.SelectedIndex >= 0 ? "" : "Seleccione una opción.");
            return BancarioMetCatalogosSeleccionados();
        }

        private async Task BancarioMetCargarCatalogos()
        {
            await BancarioMetEjecutar(async () =>
            {
                var Datos = await Task.Run(() => new[] { _Controlador.BancarioMetCatalogo("empresa"), _Controlador.BancarioMetCatalogo("pais"), _Controlador.BancarioMetCatalogo("modulo") });
                ComboBox[] Controles =
                {
                    _BancarioCboEmpresa,
                    _BancarioCboPais,
                    _BancarioCboModulo
                };
                for (int Indice = 0; Indice < Controles.Length; Indice++)
                {
                    Controles[Indice].DisplayMember = "Nombre";
                    Controles[Indice].ValueMember = "Id";
                    Controles[Indice].DataSource = Datos[Indice];
                    Controles[Indice].SelectedIndex = -1;
                }

                _BancarioLblResultado.Text = Datos.Any(Catalogo => Catalogo.Rows.Count == 0) ? "Catálogos disponibles: empresas " + Datos[0].Rows.Count + ", países " + Datos[1].Rows.Count + ", módulos " + Datos[2].Rows.Count + ". Faltan datos activos en la base conectada." : "Seleccione empresa, país y módulo contable para habilitar Generar.";
            });
        }

        private void BancarioMetInvalidar()
        {
            if (_Rellenando)
                return;
            _BancarioBtnGenerarPartida.Enabled = BancarioMetCatalogosSeleccionados();
            _Partida = 0;
            _PuedeEnviar = false;
            _BancarioBtnEnviar.Enabled = false;
            _PaginasPartida.BancarioMetCargar(null);
            _BancarioLblTotalDebe.Text = "Total Debe";
            _BancarioLblTotalHaber.Text = "Total Haber";
            _BancarioLblDiferencia.Text = "Diferencia";
        }

        private void BancarioMetMostrar(ClsPartidaEnlace Parametro)
        {
            _Partida = Parametro.Id;
            _PaginasPartida.BancarioMetCargar(Parametro.Detalle);
            decimal Debe = Parametro.Detalle.Rows.Cast<DataRow>().Sum(Registro => Convert.ToDecimal(Registro["DebeDetallePartida"]));
            decimal Haber = Parametro.Detalle.Rows.Cast<DataRow>().Sum(Registro => Convert.ToDecimal(Registro["HaberDetallePartida"]));
            _BancarioLblTotalDebe.Text = "Debe: Q " + Debe.ToString("N2", System.Globalization.CultureInfo.GetCultureInfo("es-GT"));
            _BancarioLblTotalHaber.Text = "Haber: Q " + Haber.ToString("N2", System.Globalization.CultureInfo.GetCultureInfo("es-GT"));
            _BancarioLblDiferencia.Text = "Dif.: Q " + (Debe - Haber).ToString("N2", System.Globalization.CultureInfo.GetCultureInfo("es-GT"));
            _BancarioLblResultado.Text = "Partida " + Parametro.Id + " — " + Parametro.Estado + (string.IsNullOrEmpty(Parametro.Referencia) ? "" : "\nReferencia: " + Parametro.Referencia);
            _PuedeEnviar = Parametro.Estado == "VALIDADA" || Parametro.Estado == "ERROR";
            _BancarioBtnEnviar.Enabled = _PuedeEnviar;
        }

        private bool BancarioMetCatalogosSeleccionados()
        {
            return _BancarioCboEmpresa.SelectedIndex >= 0 && _BancarioCboPais.SelectedIndex >= 0 && _BancarioCboModulo.SelectedIndex >= 0;
        }

        private async Task BancarioMetHistorial()
        {
            await BancarioMetEjecutar(async () =>
            {
                DataTable Datos = await Task.Run(() => _Controlador.BancarioMetHistorico());
                using (var BancarioFrmDialogo = new FrmHistorialPartidas(Datos))
                {
                    if (BancarioFrmDialogo.ShowDialog(this) != DialogResult.OK)
                        return;
                    var Row = BancarioFrmDialogo.PartidaSeleccionada;
                    var Parametro = await Task.Run(() => _Controlador.BancarioMetObtener(Convert.ToInt64(Row["Id"])));
                    _Rellenando = true;
                    try
                    {
                        _BancarioDtpFechaInicial.Value = Convert.ToDateTime(Row["Inicio"]);
                        _BancarioDtpFechaFinal.Value = Convert.ToDateTime(Row["Fin"]);
                        if (_BancarioCboEmpresa.DataSource != null)
                            _BancarioCboEmpresa.SelectedValue = Row["EmpresaId"];
                    }
                    finally
                    {
                        _Rellenando = false;
                    }

                    BancarioMetMostrar(Parametro);
                }
            });
        }

        private async Task BancarioMetEjecutar(Func<Task> Accion)
        {
            if (_Ocupado)
                return;
            _Ocupado = true;
            UseWaitCursor = true;
            BancarioMetHabilitarAcciones(false);
            try
            {
                await Accion();
            }
            catch (Exception Ex)
            {
                _BancarioLblResultado.Text = "La operación no se completó.";
                ClsEstiloEnlace.BancarioMetError(this, Ex);
            }
            finally
            {
                BancarioMetHabilitarAcciones(true);
                _Ocupado = false;
                UseWaitCursor = false;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs Argumentos)
        {
            if (_Ocupado)
                Argumentos.Cancel = true;
            base.OnFormClosing(Argumentos);
        }
    }
}
