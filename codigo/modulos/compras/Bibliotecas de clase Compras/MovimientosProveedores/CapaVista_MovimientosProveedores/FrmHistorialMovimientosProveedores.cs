
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

            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);

            this.BackColor = ClsTemaCompras.FondoGeneral;



            // Limpiar los datos de ejemplo del diseñador.

        }

    }
}
