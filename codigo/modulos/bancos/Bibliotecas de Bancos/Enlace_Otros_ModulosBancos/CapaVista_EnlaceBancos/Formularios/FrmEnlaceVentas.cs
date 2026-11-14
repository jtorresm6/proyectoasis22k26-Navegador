// =====================================================================
// Autor:       Andre De Jesus Gonzalez Camey
// Carné:       9959-23-3117
// Módulo:      Tesorería y Bancos – Enlace con otros módulos
// Descripción: Formulario de consulta de registros del módulo Ventas dentro del
//              enlace con otros módulos.
// Fecha:       12/11/2026
// =====================================================================
using System;
using System.Windows.Forms;
using CapaControlador_EnlaceB;
using CapaModelo_EnlaceB.Entidades;

namespace CapaVista_EnlaceB.Formularios
{
    public partial class FrmEnlaceVentas : Form
    {
        public FrmEnlaceVentas() : this(new ClsControladorEnlaceB()) { }

        public FrmEnlaceVentas(ClsControladorEnlaceB Controlador)
        {
            InitializeComponent();
            _BancarioConsulta.BancarioMetConfigurar(ModuloEnlace.Ventas, Controlador);
        }

        protected override void OnFormClosing(FormClosingEventArgs Argumentos)
        {
            if (_BancarioConsulta.Ocupado) Argumentos.Cancel = true;
            base.OnFormClosing(Argumentos);
        }
    }
}
