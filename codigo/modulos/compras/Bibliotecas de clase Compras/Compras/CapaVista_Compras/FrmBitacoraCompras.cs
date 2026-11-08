using CapaControlador_Seguridad;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaVista_Componentes.Componentes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CapaVista_Compras
{
    public partial class FrmBitacoraCompras : Form
    {
        private List<ClsModeloBitacora> _listaBitacoraCompleta = new List<ClsModeloBitacora>();
        private ClsPermisoAplicacion _Permisos;
        private Control _BotonReporteEncabezado;

        public FrmBitacoraCompras()
        {
            InitializeComponent();
            _BotonReporteEncabezado = ComprasEncabezado.Controls.Find("ComprasBtnReportes", true).First();
            _BotonReporteEncabezado.Enabled = false;
            ComprasMetColoresTabla();
            if (System.ComponentModel.LicenseManager.UsageMode ==
                System.ComponentModel.LicenseUsageMode.Designtime) return;
            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
            ComprasDgvBitacora.Tabla.ScrollBars = ScrollBars.Both;
        }

        internal static void ComprasMetEncabezadoSinImagen(ComprasUsrEncabezado encabezado, bool mostrarAcciones = false)
        {
            encabezado.BackColor = ClsTemaCompras.PrincipalNavegador;
            encabezado.ForeColor = ClsTemaCompras.FondoGeneral;
            ComprasMetQuitarImagenesEncabezado(encabezado, mostrarAcciones);
            // El componente selecciona un fondo en OnLoad; descartarlo también al cargarlo.
            encabezado.BackgroundImageChanged += (sender, e) =>
            {
                if (encabezado.BackgroundImage != null) encabezado.BackgroundImage = null;
            };
            encabezado.Load += (sender, e) => ComprasMetQuitarImagenesEncabezado(encabezado, mostrarAcciones);
        }

        private static void ComprasMetQuitarImagenesEncabezado(Control control, bool mostrarAcciones)
        {
            if (!(control is PictureBox) && !(control is Button))
                control.BackgroundImage = null;
            var imagen = control as PictureBox;
            if (imagen != null)
            {
                imagen.Visible = true;
            }
            var boton = control as Button;
            if (boton != null)
            {
                boton.Visible = mostrarAcciones;
            }
            var etiqueta = control as ComprasUsrLabel;
            if (etiqueta != null) etiqueta.ColorTexto = ClsTemaCompras.FondoGeneral;
            var panel = control as TableLayoutPanel;
            if (panel != null && panel.Name == "ComprasTlpPrincipal" && panel.ColumnStyles.Count >= 3)
            {
                if (!mostrarAcciones)
                {
                    panel.ColumnStyles[2].SizeType = SizeType.Absolute;
                    panel.ColumnStyles[2].Width = 0;
                }
            }
            foreach (Control hijo in control.Controls) ComprasMetQuitarImagenesEncabezado(hijo, mostrarAcciones);
        }

        private void ComprasMetColoresTabla()
        {
            var tabla = ComprasDgvBitacora.Tabla;
            tabla.EnableHeadersVisualStyles = false;
            tabla.BackgroundColor = ClsTemaCompras.FondoGeneral;
            tabla.GridColor = ClsTemaCompras.FondoBotones;
            tabla.ColumnHeadersDefaultCellStyle.BackColor = ClsTemaCompras.PrincipalNavegador;
            tabla.ColumnHeadersDefaultCellStyle.ForeColor = ClsTemaCompras.FondoGeneral;
            tabla.ColumnHeadersDefaultCellStyle.SelectionBackColor = ClsTemaCompras.PrincipalNavegador;
            tabla.ColumnHeadersDefaultCellStyle.SelectionForeColor = ClsTemaCompras.FondoGeneral;
            tabla.DefaultCellStyle.BackColor = ClsTemaCompras.FondoGeneral;
            tabla.DefaultCellStyle.ForeColor = ClsTemaCompras.PrincipalNavegador;
            tabla.DefaultCellStyle.SelectionBackColor = ClsTemaCompras.AcentoNavegador;
            tabla.DefaultCellStyle.SelectionForeColor = ClsTemaCompras.FondoGeneral;
            tabla.RowsDefaultCellStyle.BackColor = ClsTemaCompras.FondoGeneral;
            tabla.RowsDefaultCellStyle.ForeColor = ClsTemaCompras.PrincipalNavegador;
            tabla.RowsDefaultCellStyle.SelectionBackColor = ClsTemaCompras.AcentoNavegador;
            tabla.RowsDefaultCellStyle.SelectionForeColor = ClsTemaCompras.FondoGeneral;
            tabla.AlternatingRowsDefaultCellStyle.BackColor = ClsTemaCompras.FondoBotones;
            tabla.AlternatingRowsDefaultCellStyle.ForeColor = ClsTemaCompras.PrincipalNavegador;
            tabla.AlternatingRowsDefaultCellStyle.SelectionBackColor = ClsTemaCompras.AcentoNavegador;
            tabla.AlternatingRowsDefaultCellStyle.SelectionForeColor = ClsTemaCompras.FondoGeneral;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;
            try
            {
                // Los mismos identificadores de acceso que utiliza la bitácora de Seguridad.
                var permisos = new ClsModeloAsigAppPerf();
                permisos.SeguridadMetObtenerTodos();
                _Permisos = permisos.SeguridadMetObtenerPermisosSesion(4, 12);
                bool acceso = _Permisos != null && _Permisos.TieneAcceso;
                ComprasTxtBuscar.Enabled = ComprasBtnBuscar.Enabled = ComprasDgvBitacora.Enabled = acceso;
                _BotonReporteEncabezado.Enabled = acceso && _Permisos.PuedeImprimir;
                if (!acceso)
                {
                    ComprasLblEstado.Texto = "No tiene acceso a la bitácora.";
                    return;
                }
                _listaBitacoraCompleta = new ClsModeloBitacora().SeguridadMetObtenerTodas()
                    ?? new List<ClsModeloBitacora>();
                AplicarFiltro();
            }
            catch (Exception ex)
            {
                ComprasTxtBuscar.Enabled = ComprasBtnBuscar.Enabled = _BotonReporteEncabezado.Enabled = false;
                ComprasLblEstado.Texto = "No se pudo cargar la bitácora.";
                MessageBox.Show(this, "Error al cargar la bitácora: " + ex.Message,
                    "Bitácora", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComprasMetConfigurarColumnas()
        {
            ComprasLblEstado.Texto = "Registros encontrados: " + ((DataTable)ComprasDgvBitacora.DataSource).Rows.Count;
        }

        private void ComprasMetBuscar(object sender, EventArgs e)
        {
            if (_Permisos != null && _Permisos.TieneAcceso) AplicarFiltro();
        }

        private void ComprasMetBuscarTecla(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            ComprasMetBuscar(sender, e);
        }

        private void ComprasMetPrepararReporte(object sender, ComprasReporteEventArgs e)
        {
            if (_Permisos == null || !_Permisos.TieneAcceso || !_Permisos.PuedeImprimir)
                throw new InvalidOperationException("No tiene permiso para imprimir la bitácora.");
            e.Reporte.DataSources.Clear();
            e.Reporte.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource(
                "RpReporteBitacora", _listaBitacoraCompleta));
        }

        private void ComprasMetAyuda(object sender, EventArgs e)
        {
            const string archivo = "C:/SeguridadAyudas/SeguridadAyudas.chm";
            if (System.IO.File.Exists(archivo))
                Help.ShowHelp(this, archivo, "Bitacora_Seguridad.html");
            else
                MessageBox.Show(this, "Busque por usuario, acción, tabla, registro, detalles, IP o fecha. " +
                    "Pulse Buscar o Enter; borre el texto para mostrar todo. Reporte abre el historial completo. " +
                    "La bitácora es de solo lectura.", "Ayuda de bitácora", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ComprasMetSalir(object sender, EventArgs e) { Close(); }
        private void AplicarFiltro()
        {
            if (_listaBitacoraCompleta == null)
                return;

            string criterio = ComprasTxtBuscar != null ? ComprasTxtBuscar.Text.Trim() : string.Empty;

            List<ClsModeloBitacora> listaFiltrada;
            if (string.IsNullOrEmpty(criterio))
            {
                listaFiltrada = _listaBitacoraCompleta;
            }
            else
            {
                string criterioMin = criterio.ToLower();
                listaFiltrada = _listaBitacoraCompleta.Where(b =>
                    b.IdBitacora.ToString().Contains(criterioMin) ||
                    (b.IdUsuario.HasValue && b.IdUsuario.Value.ToString().Contains(criterioMin)) ||
                    (!string.IsNullOrEmpty(b.NombreUsuario) && b.NombreUsuario.ToLower().Contains(criterioMin)) ||
                    (!string.IsNullOrEmpty(b.AccionBitacora) && b.AccionBitacora.ToLower().Contains(criterioMin)) ||
                    (!string.IsNullOrEmpty(b.TablaBitacora) && b.TablaBitacora.ToLower().Contains(criterioMin)) ||
                    b.IdRegistroBitacora.ToString().Contains(criterioMin) ||
                    (!string.IsNullOrEmpty(b.DetallesBitacora) && b.DetallesBitacora.ToLower().Contains(criterioMin)) ||
                    (!string.IsNullOrEmpty(b.IpBitacora) && b.IpBitacora.ToLower().Contains(criterioMin)) ||
                    b.FechaHoraBitacora.ToString("yyyy-MM-dd HH:mm:ss").ToLower().Contains(criterioMin) ||
                    b.FechaHoraBitacora.ToString("dd/MM/yyyy HH:mm:ss").ToLower().Contains(criterioMin) ||
                    b.FechaHoraBitacora.ToString("yyyy-MM-dd").ToLower().Contains(criterioMin) ||
                    b.FechaHoraBitacora.ToString("dd/MM/yyyy").ToLower().Contains(criterioMin)
                ).ToList();
            }

            var datos = new DataTable();
            var propiedades = System.ComponentModel.TypeDescriptor.GetProperties(typeof(ClsModeloBitacora));
            foreach (System.ComponentModel.PropertyDescriptor propiedad in propiedades)
                datos.Columns.Add(propiedad.Name, Nullable.GetUnderlyingType(propiedad.PropertyType) ?? propiedad.PropertyType);
            foreach (var registro in listaFiltrada)
            {
                var fila = datos.NewRow();
                foreach (System.ComponentModel.PropertyDescriptor propiedad in propiedades)
                    fila[propiedad.Name] = propiedad.GetValue(registro) ?? DBNull.Value;
                datos.Rows.Add(fila);
            }
            var anterior = ComprasDgvBitacora.DataSource as DataTable;
            ComprasDgvBitacora.DataSource = datos;
            anterior?.Dispose();
            ComprasMetConfigurarColumnas();
        }

    }
}
