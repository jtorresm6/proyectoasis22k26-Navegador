using CapaControlador_EvaluacionDesempeño.Controladores;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaModelo_EvaluacionDesempeño.Entidades;
using CapaVista_EvaluacionDesempeño.Utilidades;
using CapaVista_Seguridad.Ayudas;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 21/10/2026
 * Modificado por : Daniella Jurado 0901-23-3679 (02/11/2026)
 * ==================================================================
 * Propósito :
 *  Ventana 9404 - Ver Evaluaciones (consulta, solo lectura).
 *  El colaborador que inició sesión ve el resumen de su evaluación,
 *  la calificación por aspecto, el plan de mejora asignado y el
 *  historial de sus evaluaciones.
 * Reglas especificas:
 *  Solo se muestran las evaluaciones del empleado del usuario en
 *  sesión. El nivel sale del porcentaje: 80% o más Excelente, 50% o
 *  más Satisfactorio y menos de 50% Insuficiente.
 * ===================================================================
*/

namespace CapaVista_EvaluacionDesempeño.Formularios
{
    public partial class FrmVerEvaluaciones : Form
    {
        private const int ID_MODULO = 9000;
        private const int ID_APLICACION = 9404;

        private readonly ClsModeloVerEvaluaciones _VerEvaluaciones = new ClsModeloVerEvaluaciones();
        private List<ClsResultadoEvaluacion> _MisEvaluaciones = new List<ClsResultadoEvaluacion>();
        private bool _Sincronizando;

        // Filas del diseño para los aspectos: Criterio, Peso, Nota, Ponderado.
        private Tuple<Label, Label, Label, Label>[] _FilasAspectos;

        public FrmVerEvaluaciones()
        {
            InitializeComponent();

            _FilasAspectos = new[]
            {
                Tuple.Create(RrhhLblCriterioCalidadTrabajo, RrhhLblPesoCalidadTrabajo, RrhhLblNotaCalidadTrabajo, RrhhLblPonderadoCalidadTrabajo),
                Tuple.Create(RrhhLblCriterioProductividad, RrhhLblPesoProductividad, RrhhLblNotaProductividad, RrhhLblPonderadoProductividad),
                Tuple.Create(RrhhLblCriterioTrabajoEquipo, RrhhLblPesoTrabajoEquipo, RrhhLblNotaTrabajoEquipo, RrhhLblPonderadoTrabajoEquipo),
                Tuple.Create(RrhhLblCriterioPuntualidad, RrhhLblPesoPuntualidad, RrhhLblNotaPuntualidad, RrhhLblPonderadoPuntualidad),
                Tuple.Create(RrhhLblCriterioIniciativa, RrhhLblPesoIniciativa, RrhhLblNotaIniciativa, RrhhLblPonderadoIniciativa)
            };

            RrhhColHistorialEvaluador.DataPropertyName = "NombreEvaluador";
            RrhhColHistorialFecha.DataPropertyName = "FechaEvaluacionDesempeno";
            RrhhColHistorialFecha.DefaultCellStyle.Format = "dd/MM/yyyy";
            RrhhColHistorialPuntaje.DataPropertyName = "PuntajeEvaluacionDesempeno";
            RrhhColHistorialPuntaje.DefaultCellStyle.Format = "0.00";
            RrhhColHistorialNivel.DataPropertyName = "NombreNivelDesempeno";
            RrhhColHistorialPorcentaje.DataPropertyName = "PorcentajeTexto";
            RrhhDgvHistorial.AutoGenerateColumns = false;

            Load += FrmVerEvaluaciones_Load;
            RrhhCboResumenCiclo.SelectedIndexChanged += (Remitente, Evento) => RrhhMetMostrarSeleccionada(RrhhCboResumenCiclo.SelectedIndex);
            RrhhDgvHistorial.CellClick += (Remitente, Evento) => { if (Evento.RowIndex >= 0) RrhhMetMostrarSeleccionada(Evento.RowIndex); };
            RrhhBtnRefrescar.Click += (Remitente, Evento) => RrhhMetCargarEvaluaciones();
            RrhhBtnImprimir.Click += (Remitente, Evento) => ClsAccionesVista.RrhhMetImprimir(this);
            RrhhBtnInicio.Click += (Remitente, Evento) => RrhhMetMostrarSeleccionada(0);
            RrhhBtnAnterior.Click += (Remitente, Evento) => RrhhMetMostrarSeleccionada(RrhhCboResumenCiclo.SelectedIndex - 1);
            RrhhBtnSiguiente.Click += (Remitente, Evento) => RrhhMetMostrarSeleccionada(RrhhCboResumenCiclo.SelectedIndex + 1);
            RrhhBtnFin.Click += (Remitente, Evento) => RrhhMetMostrarSeleccionada(_MisEvaluaciones.Count - 1);
            RrhhBtnAyuda.Click += (Remitente, Evento) => ClsAccionesVista.RrhhMetMostrarAyuda(this);
        }

        private void FrmVerEvaluaciones_Load(object sender, EventArgs e)
        {
            var MapaBotones = new Dictionary<Control, TipoPermiso>
            {
                { RrhhBtnImprimir, TipoPermiso.Imprimir }
            };

            var Permisos = ClsSeguridadFormHelper.SeguridadMetInicializarSeguridad(
                this, ID_MODULO, ID_APLICACION, MapaBotones);

            if (!Permisos.TieneAcceso)
            {
                MessageBox.Show("No tiene acceso a Ver Evaluaciones.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            RrhhMetCargarEvaluaciones();
        }

        private void RrhhMetCargarEvaluaciones()
        {
            try
            {
                _MisEvaluaciones = _VerEvaluaciones.RrhhMetObtenerMisEvaluaciones();
            }
            catch (Exception Ex)
            {
                _MisEvaluaciones = new List<ClsResultadoEvaluacion>();
                MessageBox.Show("No se pudieron cargar tus evaluaciones: " + Ex.Message, "Ver Evaluaciones",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _Sincronizando = true;
            RrhhCboResumenCiclo.DataSource = null;
            RrhhCboResumenCiclo.DataSource = _MisEvaluaciones;
            RrhhCboResumenCiclo.DisplayMember = "DescripcionEvaluacion";
            RrhhDgvHistorial.DataSource = null;
            RrhhDgvHistorial.DataSource = _MisEvaluaciones;
            _Sincronizando = false;

            if (_MisEvaluaciones.Count == 0)
            {
                RrhhMetLimpiar();
                MessageBox.Show("Todavía no tienes evaluaciones de desempeño para consultar.", "Ver Evaluaciones",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            RrhhMetMostrarSeleccionada(0);
        }

        // Muestra la evaluación del índice indicado y deja el combo y el historial en la misma fila.
        private void RrhhMetMostrarSeleccionada(int Indice)
        {
            if (_Sincronizando || _MisEvaluaciones.Count == 0) return;
            Indice = Math.Max(0, Math.Min(Indice, _MisEvaluaciones.Count - 1));

            _Sincronizando = true;
            RrhhCboResumenCiclo.SelectedIndex = Indice;
            RrhhDgvHistorial.ClearSelection();
            RrhhDgvHistorial.CurrentCell = RrhhDgvHistorial.Rows[Indice].Cells[RrhhColHistorialFecha.Index];
            RrhhDgvHistorial.Rows[Indice].Selected = true;
            _Sincronizando = false;

            ClsResultadoEvaluacion Evaluacion = _MisEvaluaciones[Indice];

            RrhhTxtResumenColaborador.Text = Evaluacion.NombreColaborador;
            RrhhTxtResumenPuesto.Text = Evaluacion.NombrePuesto;
            RrhhTxtResumenEvaluador.Text = Evaluacion.NombreEvaluador;
            RrhhTxtResumenFecha.Text = Evaluacion.FechaEvaluacionDesempeno.ToString("dd/MM/yyyy");
            RrhhLblPuntajeFinalValor.Text = Evaluacion.PorcentajeTexto;
            RrhhPnlPuntajeRelleno.Width = (int)(RrhhPnlPuntajePista.Width * Math.Min(1m, Evaluacion.PorcentajeEvaluacionDesempeno));
            RrhhLblNivelResumen.Text = Evaluacion.NombreNivelDesempeno;

            RrhhTxtPlanMejora.Text = Evaluacion.NombrePlanMejora == null
                ? "Sin plan de mejora asignado"
                : Evaluacion.NombrePlanMejora + ": " + Evaluacion.DescripcionPlanMejora;

            RrhhMetMostrarAspectos(Evaluacion);
        }

        private void RrhhMetMostrarAspectos(ClsResultadoEvaluacion Evaluacion)
        {
            for (int Indice = 0; Indice < _FilasAspectos.Length; Indice++)
            {
                var Fila = _FilasAspectos[Indice];
                if (Indice < Evaluacion.Aspectos.Count)
                {
                    ClsDetalleEvaluacion Aspecto = Evaluacion.Aspectos[Indice];
                    Fila.Item1.Text = Aspecto.NombreCriterioEvaluacion;
                    Fila.Item2.Text = (Aspecto.PesoDetalleEvaluacionDesempeno * 100).ToString("0") + "%";
                    Fila.Item3.Text = Aspecto.NotaDetalleEvaluacionDesempeno.ToString();
                    Fila.Item4.Text = Aspecto.PonderadoDetalleEvaluacionDesempeno.ToString("0.00");
                }
                else
                {
                    Fila.Item3.Text = "—";
                    Fila.Item4.Text = "0.00";
                }
            }

            RrhhLblTotalCriteriosValor.Text = Evaluacion.PuntajeEvaluacionDesempeno.ToString("0.00") + " / 5.00";
        }

        private void RrhhMetLimpiar()
        {
            RrhhTxtResumenColaborador.Clear();
            RrhhTxtResumenPuesto.Clear();
            RrhhTxtResumenEvaluador.Clear();
            RrhhTxtResumenFecha.Clear();
            RrhhLblPuntajeFinalValor.Text = "0%";
            RrhhPnlPuntajeRelleno.Width = 0;
            RrhhLblNivelResumen.Text = "—";
            RrhhTxtPlanMejora.Clear();
            RrhhMetMostrarAspectos(new ClsResultadoEvaluacion());
        }

        private void RrhhBtnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
