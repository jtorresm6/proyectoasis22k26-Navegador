// =====================================================================
// Autor:       Andre De Jesus Gonzalez Camey
// Carné:       9959-23-3117
// Módulo:      Tesorería y Bancos – Enlace con Contabilidad
// Descripción: Ventana de confirmación que informa que la operación (envío de
//              partida) se completó correctamente.
// Fecha:       12/11/2026
// =====================================================================
using System;
using System.Windows.Forms;
using CapaControlador_EnlaceB;

namespace CapaVista_EnlaceB.Formularios
{
    public partial class FrmOperacionCompletada : Form
    {
        public FrmOperacionCompletada()
        {
            InitializeComponent();
        }

        internal FrmOperacionCompletada(string Mensaje) : this()
        {
            _BancarioLblMensaje.Text = Mensaje;
        }
    }
}

