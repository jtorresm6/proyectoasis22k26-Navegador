using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_Herramientas;
using CapaVista_Componentes.Componentes;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 29/10/2026
 * ==================================================================
 * Propósito :
 *  Muestra, en modo de solo lectura, el último tipo de cambio que Bancos tiene
 *  registrado para cada par de monedas a la fecha seleccionada.
 * ===================================================================
*/

namespace CapaVista_Herramientas.Controles
{
    public partial class UsrTipoCambioVigente : UserControl
    {
        private const string _FormatoTipoCambio = "N4";
        private ClsControladorMoneda _Controlador;

        public UsrTipoCambioVigente()
        {
            InitializeComponent();
            ComprasProcConfigurarTabla();
            ComprasBtnConsultar.Click += ComprasMetConsultar;
        }

        public void ComprasProcInicializar(ClsControladorMoneda Controlador)
        {
            _Controlador = Controlador;
            ComprasDtpFecha.Value = DateTime.Today;
            ComprasProcConsultar();
        }

        private void ComprasProcConfigurarTabla()
        {
            ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvTiposCambio, "codigoOrigen", "Moneda origen", 14, false);
            ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvTiposCambio, "codigoDestino", "Moneda destino", 14, false);
            ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvTiposCambio, "valorTipoCambio", "Tipo de cambio", 16, false);
            ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvTiposCambio, "fechaTipoCambio", "Fecha", 14, false);
            ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvTiposCambio, "fuenteTipoCambio", "Fuente", 20, false);
            ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvTiposCambio, "metodoTipoCambio", "Método", 12, false);
            ClsPresentacionCompras.ComprasProcAgregarColumna(ComprasDgvTiposCambio, "versionTipoCambio", "Versión", 10, false);

            DataGridViewColumn Valor = ComprasDgvTiposCambio.Columns["ComprasColvalorTipoCambio"];
            Valor.DefaultCellStyle.Format = _FormatoTipoCambio;
            Valor.DefaultCellStyle.FormatProvider = System.Globalization.CultureInfo.InvariantCulture;
            Valor.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ComprasDgvTiposCambio.Columns["ComprasColfechaTipoCambio"].DefaultCellStyle.Format = "dd/MM/yyyy";
            ComprasDgvTiposCambio.Columns["ComprasColversionTipoCambio"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
        }

        private void ComprasMetConsultar(object Remitente, EventArgs Evento)
        {
            ComprasProcConsultar();
        }

        private void ComprasProcConsultar()
        {
            if (_Controlador == null) return;

            try
            {
                DataTable Datos = _Controlador.ComprasFuncObtenerTiposCambioVigentes(ComprasDtpFecha.Value);
                ComprasDgvTiposCambio.DataSource = Datos;
                ComprasLblNota.Texto = Datos.Rows.Count == 0
                    ? "Bancos no tiene tipos de cambio registrados hasta la fecha seleccionada."
                    : "Tipos de cambio registrados por el módulo de Bancos. Consulta de solo lectura.";
            }
            catch (InvalidOperationException Error)
            {
                MessageBox.Show(Error.Message, "Tipo de cambio vigente", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
