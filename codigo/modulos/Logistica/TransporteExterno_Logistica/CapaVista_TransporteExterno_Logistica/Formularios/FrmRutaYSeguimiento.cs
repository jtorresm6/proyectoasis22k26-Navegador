// ------------------------------------------------------
// Autor: Gabriela Pinto García
// Carné: 9959-23-1087
// Módulo: Logística - Transporte Externo (Vista)
// ------------------------------------------------------
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using CapaControlador_TransporteExterno_Logistica;

namespace CapaVista_TransporteExterno_Logistica.Formularios
{
    public partial class FrmRutaYSeguimiento : Form
    {
        private readonly ClsControladorRutaSeguimiento _Controlador;
        private readonly Timer _TimerGps;
        private WebView2 LogisticaWebMapa;
        private bool _cargandoFormulario = true;

        public FrmRutaYSeguimiento()
        {
            InitializeComponent();
            _Controlador = new ClsControladorRutaSeguimiento();

            _TimerGps = new Timer();
            _TimerGps.Interval = 3000;
            _TimerGps.Tick += TimerGps_Tick;

            this.Load += FrmRutaYSeguimiento_Load;
            this.FormClosing += FrmRutaYSeguimiento_FormClosing;
        }

        private void FrmRutaYSeguimiento_Load(object sender, EventArgs e)
        {
            try
            {
                _cargandoFormulario = true;

                LogisticaMetInicializarWebView2();
                LogisticaMetCargarGuias();
                LogisticaMetCargarBodegas();

                LogisticaMetEstablecerEstadoNeutro();

                this.LogisticaCboNumeroGuia.SelectionChangeCommitted += LogisticaCboNumeroGuia_SelectionChangeCommitted;
                this.LogisticaCboNumeroGuia.SelectedIndexChanged += LogisticaCboNumeroGuia_SelectedIndexChanged;

                this.LogisticaCboOrigen.SelectionChangeCommitted += LogisticaCboOrigen_SelectionChangeCommitted;
                this.LogisticaCboDestino.SelectionChangeCommitted += LogisticaCboDestino_SelectionChangeCommitted;

                this.LogisticaBtnActualizarUbicacion.Click += LogisticaBtnActualizarUbicacion_Click;
                this.LogisticaBtnGuardarRuta.Click += LogisticaBtnGuardarRuta_Click;
                this.LogisticaBtnModificarRuta.Click += LogisticaBtnModificarRuta_Click;
                this.LogisticaBtnEliminarRuta.Click += LogisticaBtnEliminarRuta_Click;
            }
            finally
            {
                _cargandoFormulario = false;
            }
        }

        private async void LogisticaMetInicializarWebView2()
        {
            try
            {
                string miApiKeyGoogle = "AIzaSyBt7Rp3vv35YhdfRw5CqmFhwQcgXN94FHg";

                LogisticaWebMapa = new WebView2 { Dock = DockStyle.Fill };
                LogisticaPnlMapa.Controls.Clear();
                LogisticaPnlMapa.Controls.Add(LogisticaWebMapa);

                await LogisticaWebMapa.EnsureCoreWebView2Async(null);

                LogisticaWebMapa.CoreWebView2.Settings.IsScriptEnabled = true;
                LogisticaWebMapa.CoreWebView2.Settings.IsWebMessageEnabled = true;

                string htmlGoogleMaps = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='utf-8'>
                        <style>html, body, #map {{ height: 100%; margin: 0; padding: 0; }}</style>
                    </head>
                    <body>
                        <div id='map'></div>
                        <script>
                            let map;
                            let marker = null;
                            let infoWindow = null;
                            let directionsService;
                            let directionsDisplay;
                            let mapaListo = false;

                            function initMap() {{
                                const centroInicial = {{ lat: 14.6349, lng: -90.5069 }};
                                map = new google.maps.Map(document.getElementById('map'), {{
                                    zoom: 13,
                                    center: centroInicial,
                                    mapTypeId: 'roadmap'
                                }});
                                
                                infoWindow = new google.maps.InfoWindow();
                                directionsService = new google.maps.DirectionsService();
                                directionsDisplay = new google.maps.DirectionsRenderer({{
                                    map: map,
                                    suppressMarkers: false
                                }});

                                mapaListo = true;
                            }}

                            function trazarRuta(origen, destino) {{
                                if (!mapaListo) {{
                                    setTimeout(() => trazarRuta(origen, destino), 500);
                                    return;
                                }}
                                if (!origen || !destino) return;

                                directionsService.route({{
                                    origin: origen,
                                    destination: destino,
                                    travelMode: google.maps.TravelMode.DRIVING
                                }}, function(response, status) {{
                                    if (status === 'OK') {{
                                        directionsDisplay.setDirections(response);
                                    }} else {{
                                        console.log('Directions error: ' + status);
                                    }}
                                }});
                            }}

                            function actualizarVida360(puntos, titulo, estado, hora) {{
                                if (!mapaListo || !puntos || puntos.length === 0) return;
                                const pos = {{ lat: parseFloat(puntos[0].lat), lng: parseFloat(puntos[0].lng) }};

                                if (!marker) {{
                                    marker = new google.maps.Marker({{
                                        position: pos,
                                        map: map,
                                        title: titulo,
                                        animation: google.maps.Animation.DROP,
                                        icon: 'http://maps.google.com/mapfiles/ms/icons/red-dot.png'
                                    }});
                                }} else {{
                                    marker.setPosition(pos);
                                }}

                                map.panTo(pos);

                                const contenido = `
                                    <div style='font-family: Arial; font-size: 12px; line-height: 1.4;'>
                                        <strong>${{titulo}}</strong><br>
                                        Ubicación exacta en vivo<br>
                                        Estado: ${{estado}}<br>
                                        Actualizado: ${{hora}}
                                    </div>`;
                                infoWindow.setContent(contenido);
                                infoWindow.open(map, marker);
                            }}

                            function ocultarMarcador() {{
                                if (marker) {{
                                    marker.setMap(null);
                                    marker = null;
                                }}
                                if (directionsDisplay) {{
                                    directionsDisplay.setDirections({{routes: []}});
                                }}
                            }}
                        </script>
                        <script async defer src='https://maps.googleapis.com/maps/api/js?key={miApiKeyGoogle}&callback=initMap'></script>
                    </body>
                    </html>";

                LogisticaWebMapa.CoreWebView2.SetVirtualHostNameToFolderMapping(
                    "appassets.example",
                    AppDomain.CurrentDomain.BaseDirectory,
                    Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow
                );

                LogisticaWebMapa.CoreWebView2.NavigateToString(htmlGoogleMaps);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ERROR MAPA GOOGLE]: " + ex.Message);
            }
        }

        private void LogisticaMetEstablecerEstadoNeutro()
        {
            LogisticaCboOrigen.SelectedIndex = -1;
            LogisticaCboDestino.SelectedIndex = -1;
            LogisticaTxtDireccionDestino.Clear();
            LogisticaTxtDireccionOrigen.Clear();

            LogisticaLblValorUbicacion.Text = "Esperando selección...";
            LogisticaLblValorEstadoUbicacion.Text = "Inactivo";
            LogisticaLblValorUltimaActualizacion.Text = "--:--:--";

            if (LogisticaWebMapa != null && LogisticaWebMapa.CoreWebView2 != null)
            {
                LogisticaWebMapa.ExecuteScriptAsync("ocultarMarcador();");
            }
        }

        private void LogisticaMetCargarGuias()
        {
            try
            {
                DataTable dtGuias = _Controlador.LogisticaFuncObtenerGuias();
                if (dtGuias != null && dtGuias.Rows.Count > 0)
                {
                    LogisticaCboNumeroGuia.DataSource = dtGuias;
                    LogisticaCboNumeroGuia.DisplayMember = "Guia";
                    LogisticaCboNumeroGuia.ValueMember = "Id";
                    LogisticaCboNumeroGuia.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ERROR GUIAS]: " + ex.Message);
            }
        }

        private void LogisticaMetCargarBodegas()
        {
            try
            {
                DataTable dtOrigen = _Controlador.LogisticaFuncObtenerBodegas();
                if (dtOrigen != null && dtOrigen.Rows.Count > 0)
                {
                    DataTable dtDestino = dtOrigen.Copy();

                    LogisticaCboOrigen.DataSource = dtOrigen;
                    LogisticaCboOrigen.DisplayMember = "Nombre";
                    LogisticaCboOrigen.ValueMember = "Id";

                    LogisticaCboDestino.DataSource = dtDestino;
                    LogisticaCboDestino.DisplayMember = "Nombre";
                    LogisticaCboDestino.ValueMember = "Id";

                    LogisticaCboOrigen.SelectedIndex = -1;
                    LogisticaCboDestino.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ERROR BODEGAS]: " + ex.Message);
            }
        }

        private int LogisticaFuncObtenerIdServicio()
        {
            if (LogisticaCboNumeroGuia.SelectedValue == null) return 0;
            if (LogisticaCboNumeroGuia.SelectedValue is DataRowView drv)
            {
                return Convert.ToInt32(drv["Id"]);
            }
            if (LogisticaCboNumeroGuia.SelectedValue is int idInt)
            {
                return idInt;
            }
            int id;
            return int.TryParse(LogisticaCboNumeroGuia.SelectedValue.ToString(), out id) ? id : 0;
        }

        private DataRow LogisticaFuncObtenerFilaSeleccionada(ComboBox cbo)
        {
            return cbo.SelectedItem is DataRowView drv ? drv.Row : null;
        }

        private void LogisticaCboNumeroGuia_SelectionChangeCommitted(object sender, EventArgs e)
        {
            EjecutarCambioGuia();
        }

        private void LogisticaCboNumeroGuia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_cargandoFormulario)
            {
                EjecutarCambioGuia();
            }
        }

        private void EjecutarCambioGuia()
        {
            if (LogisticaCboNumeroGuia.SelectedIndex == -1)
            {
                _TimerGps.Stop();
                LogisticaMetEstablecerEstadoNeutro();
                return;
            }

            int idServicio = LogisticaFuncObtenerIdServicio();
            if (idServicio > 0)
            {
                LogisticaMetCargarDetallesRuta(idServicio);
                LogisticaMetActualizarUbicacionGps(true);
                _TimerGps.Start();
            }
            else
            {
                _TimerGps.Stop();
                LogisticaMetEstablecerEstadoNeutro();
            }
        }

        private void LogisticaCboOrigen_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DataRow row = LogisticaFuncObtenerFilaSeleccionada(LogisticaCboOrigen);
            if (row != null && row.Table.Columns.Contains("Direccion"))
            {
                LogisticaTxtDireccionDestino.Text = Convert.ToString(row["Direccion"]);
            }
        }

        private void LogisticaCboDestino_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DataRow row = LogisticaFuncObtenerFilaSeleccionada(LogisticaCboDestino);
            if (row != null && row.Table.Columns.Contains("Direccion"))
            {
                LogisticaTxtDireccionOrigen.Text = Convert.ToString(row["Direccion"]);
            }
        }

        private async void LogisticaMetCargarDetallesRuta(int idServicio)
        {
            try
            {
                DataTable dtRuta = _Controlador.LogisticaFuncObtenerRuta(idServicio);
                if (dtRuta != null && dtRuta.Rows.Count > 0)
                {
                    DataRow dr = dtRuta.Rows[0];
                    string nombreOrigen = Convert.ToString(dr["Origen"]).Trim();
                    string nombreDestino = Convert.ToString(dr["Destino"]).Trim();
                    string dirOrigen = Convert.ToString(dr["DireccionOrigen"]).Trim();
                    string dirDestino = Convert.ToString(dr["DireccionDestino"]).Trim();

                    if (!string.IsNullOrEmpty(nombreOrigen))
                    {
                        int idxOrigen = LogisticaCboOrigen.FindStringExact(nombreOrigen);
                        LogisticaCboOrigen.SelectedIndex = idxOrigen >= 0 ? idxOrigen : -1;
                        if (idxOrigen < 0) LogisticaCboOrigen.Text = nombreOrigen;
                    }
                    else
                    {
                        LogisticaCboOrigen.SelectedIndex = -1;
                    }

                    if (!string.IsNullOrEmpty(nombreDestino))
                    {
                        int idxDestino = LogisticaCboDestino.FindStringExact(nombreDestino);
                        LogisticaCboDestino.SelectedIndex = idxDestino >= 0 ? idxDestino : -1;
                        if (idxDestino < 0) LogisticaCboDestino.Text = nombreDestino;
                    }
                    else
                    {
                        LogisticaCboDestino.SelectedIndex = -1;
                    }

                    LogisticaTxtDireccionDestino.Text = dirOrigen;
                    LogisticaTxtDireccionOrigen.Text = dirDestino;

                    if (LogisticaWebMapa != null && LogisticaWebMapa.CoreWebView2 != null &&
                        !string.IsNullOrWhiteSpace(dirOrigen) && !string.IsNullOrWhiteSpace(dirDestino))
                    {
                        string origEscapado = dirOrigen.Replace("'", "\\'");
                        string destEscapado = dirDestino.Replace("'", "\\'");

                        string scriptJs = $"trazarRuta('{origEscapado}', '{destEscapado}');";
                        await LogisticaWebMapa.ExecuteScriptAsync(scriptJs);
                    }
                }
                else
                {
                    LogisticaMetEstablecerEstadoNeutro();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ERROR RUTA]: " + ex.Message);
            }
        }

        private void TimerGps_Tick(object sender, EventArgs e)
        {
            LogisticaMetActualizarUbicacionGps(true);
        }

        private async void LogisticaMetActualizarUbicacionGps(bool silencioso = false)
        {
            int idServicio = LogisticaFuncObtenerIdServicio();

            if (idServicio <= 0)
            {
                _TimerGps.Stop();
                if (LogisticaWebMapa != null && LogisticaWebMapa.CoreWebView2 != null)
                {
                    await LogisticaWebMapa.ExecuteScriptAsync("ocultarMarcador();");
                }
                return;
            }

            try
            {
                DataTable dtUbicacion = _Controlador.LogisticaFuncObtenerUbicacion(idServicio);
                string horaAct = DateTime.Now.ToString("HH:mm:ss");

                if (dtUbicacion != null && dtUbicacion.Rows.Count > 0)
                {
                    string textoUbicacion = Convert.ToString(dtUbicacion.Rows[0]["Ubicacion"]).Trim();

                    if (!string.IsNullOrWhiteSpace(textoUbicacion) && textoUbicacion.Contains(","))
                    {
                        string[] partes = textoUbicacion.Split(',');
                        if (partes.Length >= 2)
                        {
                            string latStr = partes[0].Trim();
                            string lngStr = partes[1].Trim();

                            if (!latStr.Contains(".") && latStr.Length > 3)
                            {
                                latStr = latStr.Substring(0, 2) + "." + latStr.Substring(2);
                            }
                            if (!lngStr.Contains(".") && lngStr.Length > 4)
                            {
                                if (lngStr.StartsWith("-"))
                                    lngStr = lngStr.Substring(0, 3) + "." + lngStr.Substring(3);
                                else
                                    lngStr = lngStr.Substring(0, 2) + "." + lngStr.Substring(2);
                            }

                            double lat, lng;
                            bool latOk = double.TryParse(latStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out lat);
                            bool lngOk = double.TryParse(lngStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out lng);

                            if (latOk && lngOk)
                            {
                                string latFormat = lat.ToString("F6", System.Globalization.CultureInfo.InvariantCulture);
                                string lngFormat = lng.ToString("F6", System.Globalization.CultureInfo.InvariantCulture);

                                LogisticaLblValorUbicacion.Text = $"{latFormat}, {lngFormat}";
                                LogisticaLblValorEstadoUbicacion.Text = "Activo / Transmitiendo";
                                LogisticaLblValorUltimaActualizacion.Text = horaAct;

                                if (LogisticaWebMapa != null && LogisticaWebMapa.CoreWebView2 != null)
                                {
                                    string scriptJs = $"actualizarVida360([{{lat: {latFormat}, lng: {lngFormat}}}], 'Guía: GUIA-{idServicio:D4}', 'En Ruta', '{horaAct}');";
                                    await LogisticaWebMapa.ExecuteScriptAsync(scriptJs);
                                }
                                return;
                            }
                        }
                    }
                }

                LogisticaLblValorUbicacion.Text = "Dispositivo Inactivo";
                LogisticaLblValorEstadoUbicacion.Text = "Sin transmisión GPS";
                LogisticaLblValorUltimaActualizacion.Text = horaAct;

                if (LogisticaWebMapa != null && LogisticaWebMapa.CoreWebView2 != null)
                {
                    await LogisticaWebMapa.ExecuteScriptAsync("ocultarMarcador();");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ERROR GPS]: " + ex.Message);
                if (!silencioso)
                {
                    MessageBox.Show("Error al consultar GPS: " + ex.Message, "Error GPS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LogisticaBtnGuardarRuta_Click(object sender, EventArgs e)
        {
            try
            {
                int idServicio = LogisticaFuncObtenerIdServicio();
                DataRow origen = LogisticaFuncObtenerFilaSeleccionada(LogisticaCboOrigen);
                DataRow destino = LogisticaFuncObtenerFilaSeleccionada(LogisticaCboDestino);

                _Controlador.LogisticaMetAgregarRuta(idServicio, origen, destino);
                MessageBox.Show("Ruta guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogisticaMetCargarDetallesRuta(idServicio);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validación de Ruta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LogisticaBtnModificarRuta_Click(object sender, EventArgs e)
        {
            try
            {
                int idServicio = LogisticaFuncObtenerIdServicio();
                DataRow origen = LogisticaFuncObtenerFilaSeleccionada(LogisticaCboOrigen);
                DataRow destino = LogisticaFuncObtenerFilaSeleccionada(LogisticaCboDestino);

                _Controlador.LogisticaMetModificarRuta(idServicio, origen, destino);
                MessageBox.Show("Ruta modificada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogisticaMetCargarDetallesRuta(idServicio);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validación de Ruta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LogisticaBtnEliminarRuta_Click(object sender, EventArgs e)
        {
            try
            {
                int idServicio = LogisticaFuncObtenerIdServicio();
                _Controlador.LogisticaMetEliminarRuta(idServicio);
                LogisticaMetEstablecerEstadoNeutro();
                MessageBox.Show("Ruta eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LogisticaBtnActualizarUbicacion_Click(object sender, EventArgs e)
        {
            LogisticaMetActualizarUbicacionGps(false);
        }

        private void FrmRutaYSeguimiento_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_TimerGps != null)
            {
                _TimerGps.Stop();
                _TimerGps.Dispose();
            }
        }
    }
}

