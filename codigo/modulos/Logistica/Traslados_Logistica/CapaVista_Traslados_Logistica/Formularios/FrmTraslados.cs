using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_Traslados_Logistica;

/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 05/11/2026
 * ==================================================================
 * Propósito :
 * Formulario para registrar, consultar y actualizar traslados
 * entre bodegas y asignación del transporte correspondiente.
 * ===================================================================
 */

namespace CapaVista_Traslados_Logistica.Formularios
{
    public partial class FrmTraslados : Form
    {
        private ClsModeloTraslado _Traslado = new ClsModeloTraslado();
        private DataTable _Bodegas;
        private int _BodegaOrigenAnterior;
        private bool _ActualizandoLista;
        private bool _CargandoDatos;
        private bool _Editando;
        private string _CodigoTrasladoEnEdicion;

        public FrmTraslados()
        {
            InitializeComponent();
            LogisticaPnlDatosTraslados.Enabled = false;
            LogisticaBtnModificarTraslado.Enabled = false;
            LogisticaBtnGuardarTraslados.Enabled = false;
            LogisticaBtnCancelarTraslados.Enabled = false;
            LogisticaCboEstadoTraslado.Items.AddRange(new object[]
            {
                "SOLICITADO",
                "EN_TRANSITO",
                "RECIBIDO",
                "CANCELADO"
            });
            LogisticaCboEstadoTraslado.SelectedItem = "SOLICITADO";
            LogisticaCboEstadoTraslado.Enabled = false;
            LogisticaCboEstadoFiltro.DropDownStyle = ComboBoxStyle.DropDownList;
            LogisticaCboEstadoFiltro.Items.AddRange(new object[]
            {
                "Todos",
                "SOLICITADO",
                "EN_TRANSITO",
                "RECIBIDO",
                "CANCELADO"
            });
            LogisticaCboEstadoFiltro.SelectedIndex = 0;
            LogisticaDtpFechaDelFiltro.Checked = false;
            LogisticaNudCantidadTraslados.Minimum = 1;
            LogisticaNudCantidadTraslados.Value = 1;
            LogisticaNudCantidadTraslados.Maximum = int.MaxValue;

            Load += LogisticaProcInicializarDatos;
            LogisticaBtnNuevoTraslados.Click += LogisticaProcNuevoTraslado;
            LogisticaBtnCancelarTraslados.Click += LogisticaProcCancelarTraslado;
            LogisticaBtnGuardarTraslados.Click += LogisticaProcGuardarTraslado;
            LogisticaBtnModificarTraslado.Click += LogisticaProcModificarTraslado;
            LogisticaBtnBuscarTraslados.Click += LogisticaProcBuscarTraslados;
            LogisticaBtnImprimirTraslados.Click += LogisticaProcImprimirTraslados;
            LogisticaBtnAgregarTraslados.Click += LogisticaProcAgregarProducto;
            LogisticaBtnEliminarTraslados.Click += LogisticaProcEliminarProducto;
            LogisticaBtnInicioTraslados.Click += LogisticaProcIrAInicio;
            LogisticaBtnAnteriorTraslados.Click += LogisticaProcIrAlAnterior;
            LogisticaBtnSiguienteTraslados.Click += LogisticaProcIrAlSiguiente;
            LogisticaBtnFinTraslados.Click += LogisticaProcIrAlFin;
            LogisticaDgvListaTraslados.SelectionChanged += LogisticaProcSeleccionarTraslado;
            LogisticaDgvListaTraslados.CellFormatting += LogisticaProcColorearEstadoTraslado;
            LogisticaCboBodegaOrigen.SelectedValueChanged += LogisticaProcCambiarBodegaOrigen;
            LogisticaCboTipoTransporte.SelectedValueChanged += LogisticaProcCambiarTipoTransporte;
        }

        private void LogisticaProcInicializarDatos(object Sender, EventArgs E)
        {
            try
            {
                _CargandoDatos = true;
                _Bodegas = _Traslado.LogisticaMetObtenerBodegas();
                LogisticaMetEnlazarCombo(
                    LogisticaCboBodegaOrigen, _Bodegas, "NombreBodega", "CodigoBodega");
                LogisticaMetEnlazarCombo(
                    LogisticaCboBodegaDestinoTraslados, _Bodegas.Copy(),
                    "NombreBodega", "CodigoBodega");
                LogisticaMetEnlazarCombo(
                    LogisticaCboTipoTransporte,
                    _Traslado.LogisticaMetObtenerTiposTransporte(),
                    "NombreTipoTransporte", "CodigoTipoTransporte");
                LogisticaMetEnlazarCombo(
                    LogisticaCboConductorTraslado,
                    _Traslado.LogisticaMetObtenerConductoresDisponibles(null),
                    "NombreConductor", "CodigoConductor");
                LogisticaMetEnlazarCombo(
                    LogisticaCboPlacaTransporte,
                    LogisticaMetCrearTablaVacia("CodigoTransporte", "PlacaTransporte"),
                    "PlacaTransporte", "CodigoTransporte");
                LogisticaMetEnlazarCombo(
                    LogisticaCboProductoTraslados,
                    LogisticaMetCrearTablaVacia("CodigoProducto", "ProductoMostrar"),
                    "ProductoMostrar", "CodigoProducto");
            }
            catch (DataException Error)
            {
                LogisticaProcMostrarError("No fue posible cargar los catálogos. " + Error.Message);
                return;
            }
            finally
            {
                _CargandoDatos = false;
            }

            LogisticaProcCargarTraslados(Sender, E);
        }

        private void LogisticaProcCargarTraslados(object Sender, EventArgs E)
        {
            try
            {
                _ActualizandoLista = true;
                LogisticaDgvListaTraslados.DataSource =
                    _Traslado.LogisticaMetCargarTraslados();

                if (LogisticaDgvListaTraslados.Rows.Count > 0)
                {
                    LogisticaMetMostrarTraslado(_Traslado.LogisticaMetIrAInicio());
                }
                else
                {
                    LogisticaMetRestablecerCampos();
                }
            }
            catch (DataException Error)
            {
                LogisticaProcMostrarError("No fue posible cargar los traslados. " + Error.Message);
            }
            finally
            {
                _ActualizandoLista = false;
            }
        }

        private void LogisticaProcBuscarTraslados(object Sender, EventArgs E)
        {
            try
            {
                DateTime? FechaSolicitud = LogisticaDtpFechaDelFiltro.Checked
                    ? (DateTime?)LogisticaDtpFechaDelFiltro.Value.Date
                    : null;
                string Estado = LogisticaCboEstadoFiltro.SelectedItem == null
                    ? "Todos"
                    : Convert.ToString(LogisticaCboEstadoFiltro.SelectedItem);

                _ActualizandoLista = true;
                LogisticaDgvListaTraslados.DataSource = _Traslado.LogisticaMetFiltrarTraslados(
                    LogisticaTxtCodigoTrasladoFiltro.Text, FechaSolicitud, Estado);

                if (LogisticaDgvListaTraslados.Rows.Count > 0)
                {
                    LogisticaMetMostrarTraslado(_Traslado.LogisticaMetIrAInicio());
                }
                else
                {
                    _Traslado.LogisticaMetLimpiarDetalle();
                    LogisticaDgvProductoTraslados.Rows.Clear();
                    LogisticaMetRestablecerCampos();
                }
            }
            catch (DataException Error)
            {
                LogisticaProcMostrarError("No fue posible filtrar los traslados. " + Error.Message);
            }
            finally
            {
                _ActualizandoLista = false;
            }
        }

        private void LogisticaProcNuevoTraslado(object Sender, EventArgs E)
        {
            _Editando = false;
            _CodigoTrasladoEnEdicion = null;
            LogisticaMetRestablecerCampos();
            LogisticaPnlDatosTraslados.Enabled = true;
            LogisticaTxtCodigoTraslado.Enabled = true;
            LogisticaCboEstadoTraslado.Enabled = false;
            LogisticaCboConductorTraslado.Enabled = false;
            LogisticaBtnGuardarTraslados.Enabled = true;
            LogisticaBtnCancelarTraslados.Enabled = true;
            LogisticaTxtCodigoTraslado.Focus();
        }

        private void LogisticaProcCancelarTraslado(object Sender, EventArgs E)
        {
            LogisticaMetRestablecerCampos();
        }

        private void LogisticaProcModificarTraslado(object Sender, EventArgs E)
        {
            DataRowView FilaSeleccionada =
                LogisticaDgvListaTraslados.CurrentRow == null
                    ? null
                    : LogisticaDgvListaTraslados.CurrentRow.DataBoundItem as DataRowView;
            if (FilaSeleccionada == null)
            {
                LogisticaProcMostrarError("Seleccione el traslado que desea modificar.");
                return;
            }

            string Estado = Convert.ToString(FilaSeleccionada.Row["Estado"]);
            if (Estado == "RECIBIDO" || Estado == "CANCELADO")
            {
                LogisticaProcMostrarError(
                    "Un traslado recibido o cancelado es histórico y no puede modificarse.");
                return;
            }

            _CodigoTrasladoEnEdicion =
                Convert.ToString(FilaSeleccionada.Row["CodigoTraslado"]);
            try
            {
                _Traslado.LogisticaMetCargarDetalleTraslado(
                    _CodigoTrasladoEnEdicion);
                LogisticaMetActualizarDetalle();
            }
            catch (DataException Error)
            {
                LogisticaProcMostrarError(
                    "No fue posible cargar el detalle del traslado. " + Error.Message);
                return;
            }

            _Editando = true;
            LogisticaPnlDatosTraslados.Enabled = true;
            LogisticaBtnModificarTraslado.Enabled = false;
            LogisticaBtnGuardarTraslados.Enabled = true;
            LogisticaBtnCancelarTraslados.Enabled = true;
            LogisticaTxtCodigoTraslado.Enabled = false;
            LogisticaCboEstadoTraslado.Items.Clear();
            if (Estado == "SOLICITADO")
            {
                LogisticaCboEstadoTraslado.Items.AddRange(new object[]
                {
                    "SOLICITADO",
                    "EN_TRANSITO",
                    "CANCELADO"
                });
            }
            else
            {
                LogisticaCboEstadoTraslado.Items.AddRange(new object[]
                {
                    "EN_TRANSITO",
                    "RECIBIDO",
                    "CANCELADO"
                });
            }

            LogisticaCboEstadoTraslado.SelectedItem = Estado;
            LogisticaCboEstadoTraslado.Enabled = true;
            bool SolicitudEditable = Estado == "SOLICITADO";
            LogisticaCboBodegaOrigen.Enabled = SolicitudEditable;
            LogisticaCboBodegaDestinoTraslados.Enabled = SolicitudEditable;
            LogisticaCboTipoTransporte.Enabled = SolicitudEditable;
            LogisticaCboPlacaTransporte.Enabled = SolicitudEditable;
            LogisticaCboConductorTraslado.Enabled = SolicitudEditable;
            LogisticaCboProductoTraslados.Enabled = SolicitudEditable;
            LogisticaNudCantidadTraslados.Enabled = SolicitudEditable;
            LogisticaBtnAgregarTraslados.Enabled = SolicitudEditable;
            LogisticaBtnEliminarTraslados.Enabled = SolicitudEditable;
            LogisticaTxtObservacionesTraslados.Enabled = true;
            if (SolicitudEditable)
            {
                LogisticaMetCargarConductoresDisponibles();
            }
        }

        private void LogisticaProcAgregarProducto(object Sender, EventArgs E)
        {
            DataRowView ProductoSeleccionado =
                LogisticaCboProductoTraslados.SelectedItem as DataRowView;
            if (ProductoSeleccionado == null)
            {
                LogisticaProcMostrarError("Seleccione un producto con existencias en la bodega de origen.");
                return;
            }

            try
            {
                _Traslado.LogisticaMetAgregarProducto(
                    ProductoSeleccionado.Row, Convert.ToInt32(LogisticaNudCantidadTraslados.Value));
                LogisticaMetActualizarDetalle();
            }
            catch (ArgumentException Error)
            {
                LogisticaProcMostrarError(Error.Message);
            }
            catch (InvalidOperationException Error)
            {
                LogisticaProcMostrarError(Error.Message);
            }
        }

        private void LogisticaProcEliminarProducto(object Sender, EventArgs E)
        {
            if (LogisticaDgvProductoTraslados.CurrentRow == null ||
                LogisticaDgvProductoTraslados.CurrentRow.Tag == null)
            {
                LogisticaProcMostrarError("Seleccione el producto que desea eliminar.");
                return;
            }

            int CodigoProducto =
                Convert.ToInt32(LogisticaDgvProductoTraslados.CurrentRow.Tag);
            _Traslado.LogisticaMetEliminarProducto(CodigoProducto);
            LogisticaMetActualizarDetalle();
        }

        private void LogisticaProcGuardarTraslado(object Sender, EventArgs E)
        {
            try
            {
                int CodigoConductor = _Editando
                    ? LogisticaMetObtenerCodigoSeleccionado(LogisticaCboConductorTraslado)
                    : 0;
                if (_Editando)
                {
                    _Traslado.LogisticaMetActualizarTraslado(
                        _CodigoTrasladoEnEdicion,
                        LogisticaMetObtenerCodigoSeleccionado(LogisticaCboBodegaOrigen),
                        LogisticaMetObtenerCodigoSeleccionado(LogisticaCboBodegaDestinoTraslados),
                        LogisticaMetObtenerCodigoSeleccionado(LogisticaCboTipoTransporte),
                        LogisticaMetObtenerCodigoSeleccionado(LogisticaCboPlacaTransporte),
                        CodigoConductor,
                        LogisticaTxtObservacionesTraslados.Text,
                        Convert.ToString(LogisticaCboEstadoTraslado.SelectedItem));
                }
                else
                {
                    _Traslado.LogisticaMetGuardarTraslado(
                        LogisticaTxtCodigoTraslado.Text,
                        LogisticaMetObtenerCodigoSeleccionado(LogisticaCboBodegaOrigen),
                        LogisticaMetObtenerCodigoSeleccionado(LogisticaCboBodegaDestinoTraslados),
                        LogisticaMetObtenerCodigoSeleccionado(LogisticaCboTipoTransporte),
                        LogisticaMetObtenerCodigoSeleccionado(LogisticaCboPlacaTransporte),
                        CodigoConductor,
                        LogisticaTxtObservacionesTraslados.Text);
                }

                MessageBox.Show(
                    _Editando
                        ? "El traslado se actualizó correctamente."
                        : "El traslado se guardó correctamente.",
                    _Editando ? "Traslado actualizado" : "Traslado guardado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                LogisticaMetRestablecerCampos();
                LogisticaProcCargarTraslados(Sender, E);
                if (LogisticaDgvListaTraslados.Rows.Count > 0)
                {
                    LogisticaMetMostrarTraslado(_Traslado.LogisticaMetIrAlFin());
                }
            }
            catch (InvalidOperationException Error)
            {
                LogisticaProcMostrarError(Error.Message);
            }
            catch (DataException Error)
            {
                LogisticaProcMostrarError(Error.Message);
            }
        }

        private void LogisticaProcCambiarBodegaOrigen(object Sender, EventArgs E)
        {
            if (_CargandoDatos)
            {
                return;
            }

            int CodigoBodega = LogisticaMetObtenerCodigoSeleccionado(LogisticaCboBodegaOrigen);
            if (CodigoBodega > 0 && _BodegaOrigenAnterior > 0 &&
                CodigoBodega != _BodegaOrigenAnterior &&
                _Traslado.LogisticaMetObtenerDetalle().Rows.Count > 0)
            {
                DialogResult Confirmacion = MessageBox.Show(
                    "Cambiar la bodega de origen quitará los productos agregados. ¿Desea continuar?",
                    "Cambiar bodega de origen",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (Confirmacion != DialogResult.Yes)
                {
                    _CargandoDatos = true;
                    LogisticaCboBodegaOrigen.SelectedValue = _BodegaOrigenAnterior;
                    _CargandoDatos = false;
                    return;
                }

                _Traslado.LogisticaMetLimpiarDetalle();
                LogisticaMetActualizarDetalle();
            }

            _BodegaOrigenAnterior = CodigoBodega;
            try
            {
                LogisticaMetActualizarBodegasDestino(CodigoBodega);
                LogisticaMetEnlazarCombo(
                    LogisticaCboProductoTraslados,
                    _Traslado.LogisticaMetObtenerProductosDisponibles(
                        CodigoBodega, _Editando ? _CodigoTrasladoEnEdicion : null),
                    "ProductoMostrar", "CodigoProducto");
            }
            catch (DataException Error)
            {
                LogisticaProcMostrarError("No fue posible cargar los productos de la bodega. " + Error.Message);
            }
        }

        private void LogisticaProcCambiarTipoTransporte(object Sender, EventArgs E)
        {
            if (_CargandoDatos)
            {
                return;
            }

            try
            {
                LogisticaMetEnlazarCombo(
                    LogisticaCboPlacaTransporte,
                    _Traslado.LogisticaMetObtenerTransportesDisponibles(
                        LogisticaMetObtenerCodigoSeleccionado(LogisticaCboTipoTransporte),
                        _Editando ? _CodigoTrasladoEnEdicion : null),
                    "PlacaTransporte", "CodigoTransporte");
            }
            catch (DataException Error)
            {
                LogisticaProcMostrarError("No fue posible cargar las placas disponibles. " + Error.Message);
            }
        }

        private void LogisticaProcIrAInicio(object Sender, EventArgs E)
        {
            LogisticaMetMostrarTraslado(_Traslado.LogisticaMetIrAInicio());
        }

        private void LogisticaProcIrAlAnterior(object Sender, EventArgs E)
        {
            LogisticaMetMostrarTraslado(_Traslado.LogisticaMetIrAlAnterior());
        }

        private void LogisticaProcIrAlSiguiente(object Sender, EventArgs E)
        {
            LogisticaMetMostrarTraslado(_Traslado.LogisticaMetIrAlSiguiente());
        }

        private void LogisticaProcIrAlFin(object Sender, EventArgs E)
        {
            LogisticaMetMostrarTraslado(_Traslado.LogisticaMetIrAlFin());
        }

        private void LogisticaProcSeleccionarTraslado(object Sender, EventArgs E)
        {
            if (_ActualizandoLista || LogisticaDgvListaTraslados.CurrentRow == null)
            {
                return;
            }

            _Editando = false;
            _CodigoTrasladoEnEdicion = null;
            _Traslado.LogisticaMetLimpiarDetalle();
            LogisticaDgvProductoTraslados.Rows.Clear();
            DataRowView FilaActual =
                LogisticaDgvListaTraslados.CurrentRow.DataBoundItem as DataRowView;
            if (FilaActual != null)
            {
                LogisticaMetMostrarTraslado(
                    _Traslado.LogisticaMetSeleccionarTraslado(FilaActual.Row));
            }
        }

        private void LogisticaProcColorearEstadoTraslado(
            object Sender, DataGridViewCellFormattingEventArgs E)
        {
            if (E.RowIndex < 0 ||
                LogisticaDgvListaTraslados.Columns[E.ColumnIndex].DataPropertyName != "Estado")
            {
                return;
            }

            switch (Convert.ToString(E.Value).Trim().ToUpperInvariant())
            {
                case "SOLICITADO":
                    E.CellStyle.ForeColor = System.Drawing.Color.RoyalBlue;
                    break;
                case "EN_TRANSITO":
                    E.CellStyle.ForeColor = System.Drawing.Color.DarkOrange;
                    break;
                case "RECIBIDO":
                    E.CellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    break;
                case "CANCELADO":
                    E.CellStyle.ForeColor = System.Drawing.Color.Firebrick;
                    break;
            }
        }

        private void LogisticaMetMostrarTraslado(DataRow Traslado)
        {
            if (Traslado == null)
            {
                LogisticaMetRestablecerCampos();
                return;
            }

            LogisticaPnlDatosTraslados.Enabled = false;
            LogisticaBtnGuardarTraslados.Enabled = false;
            LogisticaBtnCancelarTraslados.Enabled = false;
            _ActualizandoLista = true;
            _CargandoDatos = true;
            try
            {
                LogisticaDgvListaTraslados.ClearSelection();
                foreach (DataGridViewRow Fila in LogisticaDgvListaTraslados.Rows)
                {
                    DataRowView FilaEnlazada = Fila.DataBoundItem as DataRowView;
                    if (FilaEnlazada != null && FilaEnlazada.Row == Traslado)
                    {
                        Fila.Selected = true;
                        LogisticaDgvListaTraslados.CurrentCell = Fila.Cells[0];
                        break;
                    }
                }

                int CodigoBodegaOrigen = Convert.ToInt32(Traslado["CodigoBodegaOrigen"]);
                LogisticaCboBodegaOrigen.SelectedValue = CodigoBodegaOrigen;
                LogisticaMetActualizarBodegasDestino(CodigoBodegaOrigen);
                LogisticaCboBodegaDestinoTraslados.SelectedValue =
                    Convert.ToInt32(Traslado["CodigoBodegaDestino"]);

                int CodigoTipoTransporte = Traslado.IsNull("CodigoTipoTransporte")
                    ? 0
                    : Convert.ToInt32(Traslado["CodigoTipoTransporte"]);
                LogisticaCboTipoTransporte.SelectedValue = CodigoTipoTransporte;

                int CodigoTransporte = Traslado.IsNull("CodigoTransporte")
                    ? 0
                    : Convert.ToInt32(Traslado["CodigoTransporte"]);
                DataTable TransporteActual =
                    LogisticaMetCrearTablaVacia("CodigoTransporte", "PlacaTransporte");
                if (CodigoTransporte > 0)
                {
                    TransporteActual.Rows.Add(CodigoTransporte, Convert.ToString(Traslado["Transporte"]));
                }
                LogisticaMetEnlazarCombo(
                    LogisticaCboPlacaTransporte,
                    TransporteActual,
                    "PlacaTransporte",
                    "CodigoTransporte");
                LogisticaCboPlacaTransporte.SelectedValue = CodigoTransporte;

                int CodigoConductor = Traslado.Table.Columns.Contains("CodigoConductor") &&
                    !Traslado.IsNull("CodigoConductor")
                    ? Convert.ToInt32(Traslado["CodigoConductor"])
                    : 0;
                DataTable ConductorActual =
                    LogisticaMetCrearTablaVacia("CodigoConductor", "NombreConductor");
                if (CodigoConductor > 0)
                {
                    ConductorActual.Rows.Add(
                        CodigoConductor, Convert.ToString(Traslado["Conductor"]));
                }
                LogisticaMetEnlazarCombo(
                    LogisticaCboConductorTraslado,
                    ConductorActual,
                    "NombreConductor",
                    "CodigoConductor");
                LogisticaCboConductorTraslado.SelectedValue = CodigoConductor;

                LogisticaTxtCodigoTraslado.Text =
                    Convert.ToString(Traslado["CodigoTraslado"]);
                LogisticaCboEstadoTraslado.SelectedItem =
                    Convert.ToString(Traslado["Estado"]);
                LogisticaBtnModificarTraslado.Enabled =
                    Convert.ToString(Traslado["Estado"]) != "RECIBIDO" &&
                    Convert.ToString(Traslado["Estado"]) != "CANCELADO";
                LogisticaTxtObservacionesTraslados.Text =
                    Convert.ToString(Traslado["Observaciones"]);
            }
            finally
            {
                _CargandoDatos = false;
                _ActualizandoLista = false;
            }
        }

        private void LogisticaMetRestablecerCampos()
        {
            _CargandoDatos = true;
            try
            {
                LogisticaPnlDatosTraslados.Enabled = false;
                LogisticaTxtCodigoTraslado.Clear();
                LogisticaTxtCodigoTraslado.Enabled = true;
                LogisticaCboBodegaOrigen.SelectedIndex = -1;
                LogisticaCboTipoTransporte.SelectedIndex = -1;
                LogisticaCboEstadoTraslado.Items.Clear();
                LogisticaCboEstadoTraslado.Items.AddRange(new object[]
                {
                    "SOLICITADO",
                    "EN_TRANSITO",
                    "RECIBIDO",
                    "CANCELADO"
                });
                LogisticaCboEstadoTraslado.SelectedItem = "SOLICITADO";
                LogisticaCboEstadoTraslado.Enabled = false;
                LogisticaTxtObservacionesTraslados.Clear();
                LogisticaTxtObservacionesTraslados.Enabled = true;
                LogisticaCboBodegaOrigen.Enabled = true;
                LogisticaCboBodegaDestinoTraslados.Enabled = true;
                LogisticaCboTipoTransporte.Enabled = true;
                LogisticaCboPlacaTransporte.Enabled = true;
                LogisticaCboConductorTraslado.Enabled = true;
                LogisticaCboProductoTraslados.Enabled = true;
                LogisticaNudCantidadTraslados.Value = LogisticaNudCantidadTraslados.Minimum;
                LogisticaNudCantidadTraslados.Enabled = true;
                LogisticaBtnAgregarTraslados.Enabled = true;
                LogisticaBtnEliminarTraslados.Enabled = true;
                LogisticaDgvProductoTraslados.Rows.Clear();
                LogisticaMetEnlazarCombo(
                    LogisticaCboBodegaDestinoTraslados,
                    _Bodegas == null ? LogisticaMetCrearTablaVacia("CodigoBodega", "NombreBodega") : _Bodegas.Copy(),
                    "NombreBodega", "CodigoBodega");
                LogisticaMetEnlazarCombo(
                    LogisticaCboPlacaTransporte,
                    LogisticaMetCrearTablaVacia("CodigoTransporte", "PlacaTransporte"),
                    "PlacaTransporte", "CodigoTransporte");
                LogisticaMetEnlazarCombo(
                    LogisticaCboConductorTraslado,
                    LogisticaMetCrearTablaVacia("CodigoConductor", "NombreConductor"),
                    "NombreConductor", "CodigoConductor");
                LogisticaMetEnlazarCombo(
                    LogisticaCboProductoTraslados,
                    LogisticaMetCrearTablaVacia("CodigoProducto", "ProductoMostrar"),
                    "ProductoMostrar", "CodigoProducto");
                _BodegaOrigenAnterior = 0;
                _Traslado.LogisticaMetLimpiarDetalle();
                _Editando = false;
                _CodigoTrasladoEnEdicion = null;
                DataRowView FilaActual = LogisticaDgvListaTraslados.CurrentRow == null
                    ? null
                    : LogisticaDgvListaTraslados.CurrentRow.DataBoundItem as DataRowView;
                LogisticaBtnModificarTraslado.Enabled = FilaActual != null &&
                    Convert.ToString(FilaActual.Row["Estado"]) != "RECIBIDO" &&
                    Convert.ToString(FilaActual.Row["Estado"]) != "CANCELADO";
            }
            finally
            {
                _CargandoDatos = false;
            }
        }

        private void LogisticaMetCargarConductoresDisponibles()
        {
            try
            {
                int CodigoActual = LogisticaMetObtenerCodigoSeleccionado(LogisticaCboConductorTraslado);
                LogisticaMetEnlazarCombo(
                    LogisticaCboConductorTraslado,
                    _Traslado.LogisticaMetObtenerConductoresDisponibles(
                        _Editando ? _CodigoTrasladoEnEdicion : null),
                    "NombreConductor",
                    "CodigoConductor");
                if (CodigoActual > 0)
                {
                    LogisticaCboConductorTraslado.SelectedValue = CodigoActual;
                }
            }
            catch (DataException Error)
            {
                LogisticaProcMostrarError(
                    "No fue posible cargar los conductores disponibles. " + Error.Message);
            }
        }

        private void LogisticaMetActualizarBodegasDestino(int CodigoBodegaOrigen)
        {
            DataTable BodegasDestino = _Bodegas.Clone();
            foreach (DataRow Bodega in _Bodegas.Rows)
            {
                if (Convert.ToInt32(Bodega["CodigoBodega"]) != CodigoBodegaOrigen)
                {
                    BodegasDestino.ImportRow(Bodega);
                }
            }

            LogisticaMetEnlazarCombo(
                LogisticaCboBodegaDestinoTraslados,
                BodegasDestino,
                "NombreBodega",
                "CodigoBodega");
        }

        private void LogisticaMetActualizarDetalle()
        {
            LogisticaDgvProductoTraslados.Rows.Clear();
            foreach (DataRow Detalle in _Traslado.LogisticaMetObtenerDetalle().Rows)
            {
                int Indice = LogisticaDgvProductoTraslados.Rows.Add(
                    Detalle["SKU"],
                    Detalle["Nombre"],
                    Detalle["Descripcion"],
                    Detalle["Cantidad"],
                    Detalle["PesoTotal"]);
                LogisticaDgvProductoTraslados.Rows[Indice].Tag =
                    Convert.ToInt32(Detalle["CodigoProducto"]);
            }
        }

        private void LogisticaMetEnlazarCombo(ComboBox Combo, DataTable Datos, string CampoMostrar, string CampoValor)
        {
            Combo.DataSource = null;
            Combo.DisplayMember = CampoMostrar;
            Combo.ValueMember = CampoValor;
            Combo.DataSource = Datos;
            Combo.SelectedIndex = -1;
        }

        private DataTable LogisticaMetCrearTablaVacia(string CampoCodigo, string CampoNombre)
        {
            var Tabla = new DataTable();
            Tabla.Columns.Add(CampoCodigo, typeof(int));
            Tabla.Columns.Add(CampoNombre, typeof(string));
            return Tabla;
        }

        private int LogisticaMetObtenerCodigoSeleccionado(ComboBox Combo)
        {
            if (Combo.SelectedIndex < 0 || Combo.SelectedValue == null ||
                Combo.SelectedValue == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(Combo.SelectedValue);
        }

        private void LogisticaProcMostrarError(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Traslados", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void LogisticaProcImprimirTraslados(object Sender, EventArgs E)
        {
            try
            {
                DataTable Traslados = _Traslado.LogisticaMetObtenerTrasladosParaReporte();
                using (var Reporte = new FrmReporteTraslados(Traslados))
                {
                    Reporte.ShowDialog(this);
                }
            }
            catch (DataException Error)
            {
                LogisticaProcMostrarError(
                    "No fue posible generar el reporte de traslados. " + Error.Message);
            }
        }

        private void LogisticaProcMostrarDetalle(object Sender, EventArgs E)
        {
            DataRowView FilaSeleccionada = LogisticaDgvListaTraslados.CurrentRow == null
                ? null
                : LogisticaDgvListaTraslados.CurrentRow.DataBoundItem as DataRowView;
            if (FilaSeleccionada == null)
            {
                LogisticaProcMostrarError("Seleccione un traslado para consultar su detalle.");
                return;
            }

            try
            {
                DataRow Traslado = FilaSeleccionada.Row;
                DataTable Productos = _Traslado.LogisticaMetObtenerDetalleTraslado(
                    Convert.ToString(Traslado["CodigoTraslado"]));
                using (var FormularioDetalle = new FrmDetalleTraslados(Traslado, Productos))
                {
                    FormularioDetalle.ShowDialog(this);
                }
            }
            catch (DataException Error)
            {
                LogisticaProcMostrarError(
                    "No fue posible cargar el detalle del traslado. " + Error.Message);
            }
        }
    }
}
