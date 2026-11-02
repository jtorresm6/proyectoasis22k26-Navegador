
using CapaControlador_Componentes;
using CapaVista_Componentes.Componentes;
using System;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Carlos Andres Arriaza Lara
 * Carné : 0901-23-13862
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 * Presenta los datos generales del proveedor seleccionado
 * y permite consultar su historial de movimientos.
 * ==================================================================
*/

namespace CapaVista_MovimientosProveedores
{
    public partial class FrmHistorialMovimientosProveedores : Form
    {
        // Identificador del proveedor seleccionado.
        private int? _IdProveedorSeleccionado;

        public FrmHistorialMovimientosProveedores()
        {
            InitializeComponent();

            // Configuración inicial del formulario.
            ComprasBtnConsultar.Enabled = false;

            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);

            this.BackColor = ClsTemaCompras.FondoGeneral;

            // Conectar eventos.
            ComprasCboProveedor.SelectedIndexChanged +=
                ComprasMetProveedorSeleccionado;

            ComprasBtnConsultar.Click +=
                ComprasMetConsultarMovimientos;

            // Limpiar los datos de ejemplo del diseñador.
            ComprasProcLimpiarDatosProveedor();
        }

        // ============================================================
        // OBTENER DATOS DEL PROVEEDOR SELECCIONADO
        // ============================================================

        private void ComprasProcCargarDatosProveedor()
        {
            // Obtener el objeto del proveedor seleccionado.
            ClsProveedorCompras Proveedor =
                ComprasCboProveedor
                    .ComprasFuncObtenerProveedorSeleccionado();

            // Comprobar que exista un proveedor seleccionado.
            if (Proveedor == null)
            {
                ComprasProcLimpiarDatosProveedor();
                return;
            }

            // Guardar el identificador del proveedor.
            _IdProveedorSeleccionado = Proveedor.IdProveedor;

            // Mostrar los datos en las etiquetas.
            ComprasProcMostrarDatosProveedor(Proveedor);

            // Habilitar el botón de consulta.
            ComprasBtnConsultar.Enabled = true;
        }

        // ============================================================
        // MOSTRAR LOS DATOS DEL PROVEEDOR EN LAS ETIQUETAS
        // ============================================================

        private void ComprasProcMostrarDatosProveedor(
            ClsProveedorCompras Proveedor)
        {
            ComprasLblNombreProveedor.Text =
                Proveedor.NombreProveedor ?? string.Empty;

            ComprasLblNit.Text =
                Proveedor.NitProveedor ?? string.Empty;

            ComprasLblTelefono.Text =
                Proveedor.TelefonoProveedor ?? string.Empty;

            ComprasLblCorreo.Text =
                Proveedor.CorreoProveedor ?? string.Empty;
        }

        // ============================================================
        // EVENTO: CAMBIO DE PROVEEDOR SELECCIONADO
        // ============================================================

        private void ComprasMetProveedorSeleccionado(
            object Remitente, EventArgs Evento)
        {
            try
            {
                ComprasProcCargarDatosProveedor();
            }
            catch (Exception Error)
            {
                ComprasProcLimpiarDatosProveedor();

                MessageBox.Show(
                    "No se pudieron obtener los datos del proveedor.\n\n"
                    + Error.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // LIMPIAR LOS DATOS DEL PROVEEDOR
        // ============================================================

        private void ComprasProcLimpiarDatosProveedor()
        {
            _IdProveedorSeleccionado = null;

            ComprasLblNombreProveedor.Text = string.Empty;
            ComprasLblNit.Text = string.Empty;
            ComprasLblTelefono.Text = string.Empty;
            ComprasLblCorreo.Text = string.Empty;

            ComprasBtnConsultar.Enabled = false;
        }

        // ============================================================
        // EVENTO: BOTÓN CONSULTAR MOVIMIENTOS
        // ============================================================

        private void ComprasMetConsultarMovimientos(
            object Remitente, EventArgs Evento)
        {
            // Verificar que exista un proveedor seleccionado.
            if (!_IdProveedorSeleccionado.HasValue)
            {
                MessageBox.Show(
                    "Selecciona un proveedor antes de consultar.",
                    "Proveedor requerido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Obtener el ID del proveedor seleccionado.
            int IdProveedor = _IdProveedorSeleccionado.Value;

            // TODO:
            // Implementar la consulta de movimientos reales
            // utilizando el identificador IdProveedor.

            MessageBox.Show(
                "Proveedor seleccionado correctamente.\n"
                + "ID del proveedor: " + IdProveedor + "\n\n"
                + "La consulta de movimientos reales está pendiente.",
                "Consulta de movimientos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
