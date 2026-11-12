using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 05/11/2026
 * ==================================================================
 * Propósito :
 * Formulario de consulta del detalle de un traslado logístico.
 * ===================================================================
 */

namespace CapaVista_Traslados_Logistica.Formularios
{
    public partial class FrmDetalleTraslados : Form
    {
        // Inicializa los controles del formulario de detalle.
        public FrmDetalleTraslados()
        {
            InitializeComponent();
            LogisticaBtnAyudaTraslados.Click += LogisticaProcMostrarAyuda;
            LogisticaTxtCodigoTraslado.ReadOnly = true;
            LogisticaTxtEstadoTraslado.ReadOnly = true;
            LogisticaTxtBodegaOrigen.ReadOnly = true;
            LogisticaTxtBodegaDestino.ReadOnly = true;
            LogisticaTxtFechaSolicitud.ReadOnly = true;
            LogisticaTxtFechaSalida.ReadOnly = true;
            LogisticaTxtFechaRecibido.ReadOnly = true;
            LogisticaTxtCodigoTransporte.ReadOnly = true;
            LogisticaTxtConductor.ReadOnly = true;
            LogisticaTxtTipoTransporte.ReadOnly = true;
            LogisticaTxtObservaciones.ReadOnly = true;
            LogisticaDgvDetalleTraslados.ReadOnly = true;
        }

        public FrmDetalleTraslados(DataRow Traslado, DataTable Productos)
            : this()
        {
            if (Traslado == null)
            {
                throw new ArgumentNullException("Traslado");
            }

            if (Productos == null)
            {
                throw new ArgumentNullException("Productos");
            }

            LogisticaTxtCodigoTraslado.Text = LogisticaMetObtenerTexto(
                Traslado, "CodigoTraslado", "");
            LogisticaTxtEstadoTraslado.Text = LogisticaMetObtenerTexto(
                Traslado, "Estado", "");
            LogisticaTxtBodegaOrigen.Text = LogisticaMetObtenerTexto(
                Traslado, "BodegaOrigen", "");
            LogisticaTxtBodegaDestino.Text = LogisticaMetObtenerTexto(
                Traslado, "BodegaDestino", "");
            LogisticaTxtFechaSolicitud.Text = LogisticaMetObtenerFecha(
                Traslado, "FechaSolicitud");
            LogisticaTxtFechaSalida.Text = LogisticaMetObtenerFecha(
                Traslado, "FechaSalida");
            LogisticaTxtFechaRecibido.Text = LogisticaMetObtenerFecha(
                Traslado, "FechaLlegada");

            int CodigoTransporte = Traslado.IsNull("CodigoTransporte")
                ? 0
                : Convert.ToInt32(Traslado["CodigoTransporte"]);
            string Transporte = LogisticaMetObtenerTexto(Traslado, "Transporte", "");
            LogisticaTxtCodigoTransporte.Text = CodigoTransporte > 0
                ? CodigoTransporte + " - " + Transporte
                : "Sin asignar";
            LogisticaTxtConductor.Text = LogisticaMetObtenerTexto(
                Traslado, "Conductor", "Sin asignar");
            LogisticaTxtTipoTransporte.Text = LogisticaMetObtenerTexto(
                Traslado, "TipoTransporte", "Sin asignar");
            LogisticaTxtObservaciones.Text = LogisticaMetObtenerTexto(
                Traslado, "Observaciones", "Sin observaciones");

            int TotalUnidades = 0;
            double PesoTotal = 0;
            decimal CostoTotal = 0;
            foreach (DataRow Producto in Productos.Rows)
            {
                int Cantidad = Convert.ToInt32(Producto["Cantidad"]);
                double PesoProducto = Convert.ToDouble(Producto["PesoTotal"]);
                decimal CostoProducto = Convert.ToDecimal(Producto["PrecioTotal"]);
                TotalUnidades += Cantidad;
                PesoTotal += PesoProducto;
                CostoTotal += CostoProducto;
                LogisticaDgvDetalleTraslados.Rows.Add(
                    Producto["CodigoProducto"],
                    Producto["Nombre"],
                    Producto["Descripcion"],
                    Producto["Cantidad"],
                    Producto["PesoUnitario"],
                    Producto["PesoTotal"],
                    Producto["PrecioUnitario"],
                    Producto["PrecioTotal"]);
            }

                    LogisticaLblResumenProductos.Text = "Productos: " + Productos.Rows.Count;
                    LogisticaLblResumenUnidades.Text = "Unidades: " + TotalUnidades;
                    LogisticaLblResumenPeso.Text = "Peso total: " + PesoTotal.ToString("N2") + " kg";
                    LogisticaLblResumenCosto.Text = "Costo total: " + CostoTotal.ToString("N2");
        }

        private static string LogisticaMetObtenerTexto(
            DataRow Fila, string Columna, string ValorPredeterminado)
        {
            return Fila.Table.Columns.Contains(Columna) && !Fila.IsNull(Columna)
                ? Convert.ToString(Fila[Columna])
                : ValorPredeterminado;
        }

        private static string LogisticaMetObtenerFecha(DataRow Fila, string Columna)
        {
            if (!Fila.Table.Columns.Contains(Columna) || Fila.IsNull(Columna))
            {
                return "Pendiente";
            }

            return Convert.ToDateTime(Fila[Columna]).ToString("g");
        }

        private void LogisticaProcMostrarAyuda(object sender, EventArgs e)
        {
            DirectoryInfo DirectorioActual =
                new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            while (DirectorioActual != null)
            {
                string RutaAyuda = Path.Combine(
                    DirectorioActual.FullName,
                    "ayuda",
                    "modulos",
                    "logistica",
                    "LogisticaAyudas",
                    "LogisticaAyudas.chm");

                if (File.Exists(RutaAyuda))
                {
                    Help.ShowHelp(this, RutaAyuda, "DetalleTraslados_Logistica.html");
                    return;
                }

                DirectorioActual = DirectorioActual.Parent;
            }

            MessageBox.Show(
                this,
                "No se encontró el archivo de ayuda del detalle de traslados.",
                "Detalle de traslados",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
