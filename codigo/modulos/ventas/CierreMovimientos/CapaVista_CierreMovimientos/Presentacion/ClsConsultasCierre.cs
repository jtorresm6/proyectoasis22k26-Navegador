// Jose Javier Torres Martinez | 0901-23-1091 | 23/10/2026
using CapaControlador_CierreMovimientos;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System;
using VentasPresentacion;

namespace CapaVista_CierreMovimientos
{
    internal sealed class ClsConsultasCierre
    {
        private readonly ClsCierrePresentador _Estado;

        internal ClsConsultasCierre(ClsCierrePresentador Estado)
        {
            _Estado = Estado;
        }

        internal void VentasMetHistorial()
        {
            _Estado.Cargando = true;
            try
            {
                _Estado.IdCierre = 0;
                _Estado.NumeroCierre = "";
                _Estado.Vista.VentasDgvHistorialCierre.Rows.Clear();
                foreach (DataRow Fila in _Estado.Controlador.VentasFuncHistorial(_Estado.Moneda, ClsHerramientasVentas.VentasFuncObtenerId(_Estado.Vista.VentasCboClienteHistorial), _Estado.Vista.VentasTxtPeriodoHistorial.Text).Rows)
                {
                    int IndiceFila = _Estado.Vista.VentasDgvHistorialCierre.Rows.Add(Fila["Periodo"], Fila["Codigo"], Fila["Nombre"], Fila["Anterior"], Fila["Cargos"], Fila["Abonos"], Fila["CargosAcumulados"], Fila["AbonosAcumulados"], Fila["Actual"], Fila["Estado"]);
                    _Estado.Vista.VentasDgvHistorialCierre.Rows[IndiceFila].Tag = Fila;
                }
            }
            finally
            {
                _Estado.Cargando = false;
            }

            if (_Estado.Vista.VentasDgvHistorialCierre.CurrentRow != null)
            {
                var Fila2 = _Estado.Vista.VentasDgvHistorialCierre.CurrentRow.Tag as DataRow;
                if (Fila2 != null)
                {
                    _Estado.IdCierre = Convert.ToInt32(Fila2["idCierre"]);
                    _Estado.NumeroCierre = Convert.ToString(Fila2["numeroCierre"]);
                }
            }

            _Estado.Vista.VentasBtnConfirmar.Enabled = false;
            VentasMetMostrarPartida();
        }

        internal void VentasMetMostrarPartida()
        {
            _Estado.Vista.VentasDgvPoliza.Rows.Clear();
            _Estado.Vista.VentasBtnTrasladarContabilidad.Enabled = false;
            if (_Estado.IdCierre <= 0)
                return;
            _Estado.Partida = _Estado.Controlador.VentasFuncPartida(_Estado.IdCierre);
            foreach (DataRow Fila in _Estado.Partida.Rows)
                _Estado.Vista.VentasDgvPoliza.Rows.Add(Fila["Cuenta"], Fila["Descripcion"], Fila["Debe"], Fila["Haber"]);
            decimal Debe = _Estado.Partida.AsEnumerable().Sum(Registro => (decimal)Registro["Debe"]), Haber = _Estado.Partida.AsEnumerable().Sum(Registro => (decimal)Registro["Haber"]);
            _Estado.Vista.VentasLblPolizaTotales.Text = "Debe: " + Debe.ToString("N2", ClsHerramientasVentas.Cultura) + " / Haber: " + Haber.ToString("N2", ClsHerramientasVentas.Cultura);
            _Estado.Vista.VentasTxtReferenciaPoliza.Text = "CXC-" + _Estado.NumeroCierre;
            _Estado.Vista.VentasTxtEstadoTraslado.Text = "Preparada; pendiente de enlace";
            DataTable Estado = _Estado.Controlador.VentasFuncEstadoTraslado(_Estado.IdCierre);
            bool Confirmado = Estado.Rows.Count > 0 && (string)Estado.Rows[0]["estado"] == "CONFIRMADO";
            if (Confirmado)
            {
                _Estado.Vista.VentasTxtReferenciaPoliza.Text = Convert.ToString(Estado.Rows[0]["referenciaContable"]);
                _Estado.Vista.VentasTxtFechaTraslado.Text = Convert.ToDateTime(Estado.Rows[0]["fechaConfirmacion"]).ToString("dd/MM/yyyy HH:mm");
                _Estado.Vista.VentasTxtEstadoTraslado.Text = "Confirmado por Contabilidad";
            }

            _Estado.Vista.VentasBtnTrasladarContabilidad.Enabled = !Confirmado && _Estado.EnlaceContable != null && _Estado.Partida.Rows.Count > 0 && !_Estado.Partida.AsEnumerable().Any(Registro => ((string)Registro["Cuenta"]).StartsWith("PENDIENTE", StringComparison.Ordinal));
        }

        internal void VentasMetImprimirCierre()
        {
            DataTable Datos = _Estado.Revision;
            string Titulo = "Cierre " + _Estado.NumeroCierre;
            if (_Estado.Vista.VentasTbcCierre.SelectedTab == _Estado.Vista.VentasTbpHistorial)
            {
                Datos = _Estado.Controlador.VentasFuncHistorial(_Estado.Moneda, ClsHerramientasVentas.VentasFuncObtenerId(_Estado.Vista.VentasCboClienteHistorial), _Estado.Vista.VentasTxtPeriodoHistorial.Text);
                Titulo = "Historial de cierres";
            }
            else if (_Estado.Vista.VentasTbcCierre.SelectedTab == _Estado.Vista.VentasTbpContabilidad)
            {
                Datos = _Estado.Partida;
                Titulo = "Partida del cierre " + _Estado.NumeroCierre;
            }

            ClsHerramientasVentas.VentasMetImprimir(_Estado.Vista, Titulo, _Estado.Vista.VentasCboMonedaCierre.Text, Datos);
        }

        public void VentasMetConfigurarEnlaceContable(Func<string, int, DateTime, DataTable, string> Registrar)
        {
            _Estado.EnlaceContable = Registrar;
            if (_Estado.IdCierre > 0)
                VentasMetMostrarPartida();
        }
    }
}
