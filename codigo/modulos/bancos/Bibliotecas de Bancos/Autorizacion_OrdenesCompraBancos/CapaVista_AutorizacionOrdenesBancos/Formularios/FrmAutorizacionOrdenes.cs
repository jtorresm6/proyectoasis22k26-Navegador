using CapaControlador_AutorizacionOrdenes;
using CapaControlador_AutorizacionOrdenes.Modelos_de_controladores;
using CapaVista_AutorizacionOrdenes.frmReportes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaVista_Navegador;

/*
 * ==================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 4/11/2026
 * ==================================================================
 * Propósito :
 *  El formulario Autorización de Órdenes de Compra (proceso 8011) permite al
 *  área de Tesorería y Bancos revisar las órdenes pendientes, asignar una o
 *  varias cuentas bancarias con su disponibilidad y autorizar o rechazar la
 *  orden, comunicándose con la capa controlador.
 * ===================================================================
*/

namespace CapaVista_AutorizacionOrdenes.Formularios
{
    public partial class FrmAutorizacionOrdenes : Form
    {
        private const int ID_EMPRESA = 1;

        private ClsModeloOrdenCompra _ModeloOrdenCompra = new ClsModeloOrdenCompra();
        private ClsModeloCuentaDisponible _ModeloCuentaDisponible = new ClsModeloCuentaDisponible();
        private ClsModeloAutorizacionOrdenCompra _ModeloAutorizacionOrdenCompra = new ClsModeloAutorizacionOrdenCompra();

        public FrmAutorizacionOrdenes()
        {
            InitializeComponent();
            this.Load += FrmAutorizacionOrdenes_Load;
            BancosDgvOrdenesPendientes.CellClick += BancosDgvOrdenesPendientes_CellClick;
            BancosCboBanco.SelectionChangeCommitted += BancosCboBanco_CambioSeleccion;
            BancosMetReinicio();
            BancosTxtMotivoRechazo.MaxLength = 100;
        }

        private void FrmAutorizacionOrdenes_Load(object sender, EventArgs e)
        {
            BancosMetColumnasAsignacion();
            BancosMetCargarCombos();
            BancosMetListaOrdenesPendientes();
            BancosMetAplicarColorSeleccion(BancosDgvOrdenesPendientes);
            BancosMetAplicarColorSeleccion(BancosDgvAsignacionCuenta);
            BancosMetEstablecerFuente(BancosDgvOrdenesPendientes);
            BancosMetEstablecerFuente(BancosDgvAsignacionCuenta);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(290, 0);
        }

        private void BancosMetListaOrdenesPendientes()
        {
            try
            {
                BancosDgvOrdenesPendientes.DataSource = _ModeloOrdenCompra.BancosMetObtenerPendientes();
                BancosMetColumnasOrdenesPendientes();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void BancosMetColumnasOrdenesPendientes()
        {
            BancosDgvOrdenesPendientes.Columns[0].Visible = false;
            BancosDgvOrdenesPendientes.Columns[1].HeaderText = "No. de orden";
            BancosDgvOrdenesPendientes.Columns[2].HeaderText = "Proveedor";
            BancosDgvOrdenesPendientes.Columns[3].HeaderText = "Fecha";
            BancosDgvOrdenesPendientes.Columns[4].HeaderText = "Vence";
            BancosDgvOrdenesPendientes.Columns[5].HeaderText = "Monto";
            BancosDgvOrdenesPendientes.Columns[6].HeaderText = "Estado";
            BancosDgvOrdenesPendientes.Columns[7].Visible = false;
            BancosDgvOrdenesPendientes.Columns[8].HeaderText = "Moneda";

            BancosDgvOrdenesPendientes.Columns[1].Width = 90;
            BancosDgvOrdenesPendientes.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            BancosDgvOrdenesPendientes.Columns[3].Width = 90;
            BancosDgvOrdenesPendientes.Columns[4].Width = 90;
            BancosDgvOrdenesPendientes.Columns[5].Width = 100;
            BancosDgvOrdenesPendientes.Columns[6].Width = 80;
            BancosDgvOrdenesPendientes.Columns[8].Width = 70;

            BancosDgvOrdenesPendientes.Columns[3].DefaultCellStyle.Format = "dd/MM/yyyy";
            BancosDgvOrdenesPendientes.Columns[4].DefaultCellStyle.Format = "dd/MM/yyyy";
            BancosDgvOrdenesPendientes.Columns[5].DefaultCellStyle.Format = "N2";
            BancosDgvOrdenesPendientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void BancosMetCargarCombos()
        {
            try
            {
                BancosCboBanco.DataSource = _ModeloCuentaDisponible.BancosMetObtenerTodos();
                BancosCboBanco.DisplayMember = "NombreCuentaCompleto";
                BancosCboBanco.ValueMember = "IdCuenta";
                BancosCboBanco.SelectedIndex = -1;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void BancosMetReinicio()
        {
            BancosLblValorTotalAsignado.Text = "Q 0.00";
            BancosLblMontoReservadoValor.Text = "Q 0.00";
            BancosLblValorDiferencia.Text = "Q 0.00";
            BancosLblSaldoValor.Text = "Q 0.00";
            BancosLblMonedaValor.Text = "GTQ";
            BancosLblTipoCambioValor.Text = "1.00";
            BancosLblResultadoFondos.Text = "Resultado: ";
            BancosTxtMontoAsignado.Text = string.Empty;
            BancosTxtMotivoRechazo.Text = string.Empty;
            BancosCboBanco.SelectedIndex = -1;
            BancosDgvAsignacionCuenta.Rows.Clear();
            BancosDgvOrdenesPendientes.ClearSelection();
            BancosPnlDisponibilidadAsignacion.Enabled = false;
        }

        private void BancosCboBanco_CambioSeleccion(object sender, EventArgs e)
        {
            try
            {
                if (!BancosMetValidarOrdenSeleccionada())
                {
                    BancosCboBanco.SelectedIndex = -1;
                    return;
                }
                long IdCuenta = Convert.ToInt64(BancosCboBanco.SelectedValue);
                var Cuenta = _ModeloCuentaDisponible.BancosMetBuscarPorId(IdCuenta);


                if (Cuenta != null)
                {
                    long IdMonedaOrden = Convert.ToInt64(BancosDgvOrdenesPendientes.CurrentRow.Cells[7].Value);
                    BancosLblSaldoValor.Text = Cuenta.SaldoDisponible.ToString("N2");
                    BancosLblMontoReservadoValor.Text = Cuenta.MontoComprometido.ToString("N2");
                    BancosLblMonedaValor.Text = Cuenta.CodigoMoneda;

                    if (Cuenta.IdMoneda == IdMonedaOrden)
                    {
                        BancosLblTipoCambioValor.Text = "1.00";
                    }
                    else
                    {
                        BancosLblTipoCambioValor.Text = "No disponible";
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void BancosMetAgregarAsignacion()
        {
            try
            {
                if (!BancosMetValidarOrdenSeleccionada())
                {
                    return;
                }

                if (BancosCboBanco.SelectedIndex < 0)
                {
                    MessageBox.Show("Seleccione un banco de pago");
                    return;
                }

                decimal Monto;

                if (!decimal.TryParse(BancosTxtMontoAsignado.Text, out Monto) || Monto <= 0)
                {
                    MessageBox.Show("Ingrese un monto asignado válido");
                    return;
                }

                if (Monto != Math.Round(Monto, 2))
                {
                    MessageBox.Show("El monto solo puede tener 2 decimales");
                    return;
                }

                long IdCuenta = Convert.ToInt64(BancosCboBanco.SelectedValue);

                if (BancosDgvAsignacionCuenta.Rows.Cast<DataGridViewRow>().Any(Fila => Convert.ToInt64(Fila.Tag) == IdCuenta))
                {
                    MessageBox.Show("Esta cuenta ya fue asignada");
                    return;
                }

                decimal Asignado = BancosMetTotalAsignado();

                var Cuenta = _ModeloCuentaDisponible.BancosMetBuscarPorId(IdCuenta);
                
                if (Cuenta == null)
                {
                    MessageBox.Show("No se encontró la cuenta seleccionada");
                    return;
                }

                decimal Importe = BancosMetImporteOrdenSeleccionada();
                long IdMonedaOrden = Convert.ToInt64(BancosDgvOrdenesPendientes.CurrentRow.Cells[7].Value);

                if (Cuenta.IdMoneda != IdMonedaOrden)
                {
                    MessageBox.Show("En esta entrega solo se permiten cuentas de la misma moneda que la orden");
                    return;
                }
                decimal TipoCambio = 1m;

                if (Asignado + Monto > Importe)
                {
                    MessageBox.Show("El monto supera lo que falta por asignar de la orden");
                    return;
                }

                if (Monto > Cuenta.SaldoDisponible * TipoCambio)
                {
                    MessageBox.Show("El monto supera el saldo disponible de la cuenta");
                    return;
                }

                int Indice = BancosDgvAsignacionCuenta.Rows.Add(Cuenta.NombreBanco, Cuenta.NumeroCuenta, Cuenta.CodigoMoneda, Cuenta.SaldoDisponible, TipoCambio, Monto);
                BancosDgvAsignacionCuenta.Rows[Indice].Tag = IdCuenta;

                BancosTxtMontoAsignado.Text = string.Empty;
                BancosCboBanco.SelectedIndex = -1;
                BancosMetActualizarTotales();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void BancosMetActualizarTotales()
        {
            decimal Total = BancosMetTotalAsignado();

            decimal Importe = 0;
            if (BancosDgvOrdenesPendientes.SelectedRows.Count > 0)
            {
                Importe = BancosMetImporteOrdenSeleccionada();
            }

            BancosLblValorTotalAsignado.Text = Total.ToString("N2");
            BancosLblValorDiferencia.Text = (Importe - Total).ToString("N2");

            if (Importe > 0 && Total == Importe)
            {
                BancosLblResultadoFondos.Text = "Resultado: Fondos suficientes";
            }
            else
            {
                BancosLblResultadoFondos.Text = "Resultado: Asignación incompleta";
            }
        }

        private bool BancosMetValidarDesbloqueo()
        {
            if (!BancosPnlDisponibilidadAsignacion.Enabled)
            {
                MessageBox.Show("Primero se debe presionar Ingresar para desbloquear el formulario");
                return false;
            }
            return true;
        }

        private bool BancosMetValidarOrdenSeleccionada()
        {
            if (BancosDgvOrdenesPendientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una orden del listado");
                return false;
            }
            return true;
        }

        private decimal BancosMetTotalAsignado()
        {
            decimal Total = 0;
            foreach (DataGridViewRow Item in BancosDgvAsignacionCuenta.Rows)
            {
                Total += Convert.ToDecimal(Item.Cells[5].Value);
            }
            return Total;
        }

        private decimal BancosMetImporteOrdenSeleccionada()
        {
            return Convert.ToDecimal(BancosDgvOrdenesPendientes.CurrentRow.Cells[5].Value);
        }

        private void BancosMetColumnasAsignacion()
        {
            BancosDgvAsignacionCuenta.Columns.Clear();
            BancosDgvAsignacionCuenta.Columns.Add("BancosColBanco", "Banco");
            BancosDgvAsignacionCuenta.Columns.Add("BancosColCuenta", "Cuenta");
            BancosDgvAsignacionCuenta.Columns.Add("BancosColMonedaAsig", "Moneda");
            BancosDgvAsignacionCuenta.Columns.Add("BancosColSaldoDisponible", "Saldo Disponible");
            BancosDgvAsignacionCuenta.Columns.Add("BancosColTipoCambio", "Tipo Cambio");
            BancosDgvAsignacionCuenta.Columns.Add("BancosColMontoAsignado", "Monto Asignado");

            BancosDgvAsignacionCuenta.Columns[3].DefaultCellStyle.Format = "N2";
            BancosDgvAsignacionCuenta.Columns[4].DefaultCellStyle.Format = "N4";
            BancosDgvAsignacionCuenta.Columns[5].DefaultCellStyle.Format = "N2";
            BancosDgvAsignacionCuenta.ReadOnly = true;
            BancosDgvAsignacionCuenta.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void BancosMetGrabarDecision(string Estado)
        {
            try
            {
                long IdOrdenCompra = Convert.ToInt64(BancosDgvOrdenesPendientes.CurrentRow.Cells[0].Value);
                long IdMonedaOrden = Convert.ToInt64(BancosDgvOrdenesPendientes.CurrentRow.Cells[7].Value);

                _ModeloAutorizacionOrdenCompra = new ClsModeloAutorizacionOrdenCompra();
                _ModeloAutorizacionOrdenCompra.IdOrdenCompra = IdOrdenCompra;
                if (!ClsNavegadorSesion.NavegadorFuncHaySesion())
                {
                    MessageBox.Show("Debe iniciar sesión para autorizar o rechazar una orden.");
                    return;
                }

                _ModeloAutorizacionOrdenCompra.IdEmpresaAutorizacionOrden = ID_EMPRESA;
                _ModeloAutorizacionOrdenCompra.EstadoAutorizacionOrden = Estado;
                _ModeloAutorizacionOrdenCompra.IdUsuarioAutoriza = ClsNavegadorSesion.NavegadorFuncIdUsuario();
                _ModeloAutorizacionOrdenCompra.FechaDecisionAutorizacionOrden = DateTime.Now;
                _ModeloAutorizacionOrdenCompra.Ip = "127.0.0.1";

                if (Estado == "AUTORIZADA")
                {
                    _ModeloAutorizacionOrdenCompra.BancosMetLimpiarCuentas();
                    foreach (DataGridViewRow Item in BancosDgvAsignacionCuenta.Rows)
                    {
                        var Cuenta = _ModeloCuentaDisponible.BancosMetBuscarPorId(Convert.ToInt64(Item.Tag));

                        if (Cuenta == null)
                        {
                            MessageBox.Show("No se encontró una de las cuentas asignadas");
                            return;
                        }

                        decimal Monto = Convert.ToDecimal(Item.Cells[5].Value);
                        _ModeloAutorizacionOrdenCompra.BancosMetAgregarCuenta(Cuenta, IdMonedaOrden, Monto);
                    }
                }
                else
                {
                    _ModeloAutorizacionOrdenCompra.MotivoRechazoAutorizacionOrden = BancosTxtMotivoRechazo.Text.Trim();
                }

                string Mensaje = _ModeloAutorizacionOrdenCompra.BancosMetGrabarCambios();
                MessageBox.Show(Mensaje);

                if (Mensaje == "Autorizacion Exitosa" || Mensaje == "Rechazo Exitoso")
                {
                    BancosMetCargarCombos();
                    BancosMetReinicio();
                    BancosMetListaOrdenesPendientes();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
        private void BancosBtnAutorizar_Click(object sender, EventArgs e)
        {

            if (!BancosMetValidarDesbloqueo())
            {
                return;
            }

            if (!BancosMetValidarOrdenSeleccionada())
            {
                return;
            }

            if (BancosDgvAsignacionCuenta.Rows.Count == 0)
            {
                MessageBox.Show("Debe asignar al menos una cuenta");
                return;
            }

            decimal Importe = BancosMetImporteOrdenSeleccionada();
            decimal Total = BancosMetTotalAsignado();

            if (Total != Importe)
            {
                MessageBox.Show("El total asignado debe ser igual al monto de la orden");
                return;
            }

            if (MessageBox.Show("¿Estas seguro que deseas autorizar esta orden?", "Autorizar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                BancosMetGrabarDecision("AUTORIZADA");
            }
        }

        private void BancosBtnRechazar_Click(object sender, EventArgs e)
        {
            if (!BancosMetValidarDesbloqueo())
            {
                return;
            }

            if (!BancosMetValidarOrdenSeleccionada())
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(BancosTxtMotivoRechazo.Text))
            {
                MessageBox.Show("Debe ingresar el motivo de rechazo");
                return;
            }

            if (MessageBox.Show("¿Estas seguro que deseas rechazar esta orden?", "Rechazar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                BancosMetGrabarDecision("RECHAZADA");
            }
        }

        private void BancosBtnIngresar_Click(object sender, EventArgs e)
        {
            if (!BancosPnlDisponibilidadAsignacion.Enabled)
            {
                BancosMetReinicio();
                BancosPnlDisponibilidadAsignacion.Enabled = true;
                return;
            }
            BancosMetAgregarAsignacion();
        }

        private void BancosBtnEliminar_Click(object sender, EventArgs e)
        {
            if (!BancosMetValidarDesbloqueo())
            {
                return;
            }

            if (BancosDgvAsignacionCuenta.CurrentRow == null)
            {
                MessageBox.Show("Seleccione la cuenta que desea eliminar");
                return;
            }

            if (MessageBox.Show("¿Estas seguro que deseas eliminar este registro?", "Eliminar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                BancosDgvAsignacionCuenta.Rows.Remove(BancosDgvAsignacionCuenta.CurrentRow);
                BancosMetActualizarTotales();
            }
        }

        private void BancosBtnLimpiar_Click(object sender, EventArgs e)
        {
            BancosMetReinicio();
        }

        private void BancosBtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BancosDgvOrdenesPendientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (!BancosMetValidarDesbloqueo())
            {
                return;
            }

            if (BancosDgvOrdenesPendientes.SelectedRows.Count > 0)
            {
                decimal Importe = BancosMetImporteOrdenSeleccionada();
                BancosLblValorTotalAsignado.Text = "0.00";
                BancosLblValorDiferencia.Text = Importe.ToString("N2");
                BancosLblResultadoFondos.Text = "Resultado: ";
                BancosTxtMontoAsignado.Text = string.Empty;
                BancosDgvAsignacionCuenta.Rows.Clear();
                BancosCboBanco.SelectedIndex = -1;
            }
        }

        private void BancosMetAplicarColorSeleccion(DataGridView Grid)
        {
            Grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(198, 224, 180);
            Grid.DefaultCellStyle.SelectionForeColor = Color.Black;

            Grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Grid.ColumnHeadersDefaultCellStyle.BackColor;
            Grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Grid.ColumnHeadersDefaultCellStyle.ForeColor;
            Grid.RowHeadersDefaultCellStyle.SelectionBackColor = Grid.RowHeadersDefaultCellStyle.BackColor;
        }

        private void BancosMetEstablecerFuente(DataGridView Grid)
        {
            Grid.AlternatingRowsDefaultCellStyle.Font = null;
        }

        private void BancosBtnRefrescar_Click(object sender, EventArgs e)
        {
            BancosMetCargarCombos();
            BancosMetListaOrdenesPendientes();
            BancosMetReinicio();
        }

        private void BancosBtnReporte_Click(object sender, EventArgs e)
        {
            FrmReporteAutorizacionOrdenCompra reporte = new FrmReporteAutorizacionOrdenCompra();
            reporte.Show();
        }
    }
}
