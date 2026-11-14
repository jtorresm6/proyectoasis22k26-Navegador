// =====================================================================
// Autor:       Andre De Jesus Gonzalez Camey
// Carné:       9959-23-3117
// Módulo:      Tesorería y Bancos – Enlace con otros módulos
// Descripción: Formulario de consulta de registros del módulo Recursos humanos dentro
//              del enlace con otros módulos.
// Fecha:       12/11/2026
// =====================================================================
using System;
using System.Windows.Forms;
using CapaControlador_EnlaceB;
using CapaModelo_EnlaceB.Entidades;

namespace CapaVista_EnlaceB.Formularios
{
    public partial class FrmEnlaceRecursosHumanos : Form
    {
        public FrmEnlaceRecursosHumanos() : this(new ClsControladorEnlaceB()) { }

        public FrmEnlaceRecursosHumanos(ClsControladorEnlaceB Controlador)
        {
            InitializeComponent();
            _BancarioConsulta.BancarioMetConfigurar(ModuloEnlace.RecursosHumanos, Controlador);
        }

        protected override void OnFormClosing(FormClosingEventArgs Argumentos)
        {
            if (_BancarioConsulta.Ocupado) Argumentos.Cancel = true;
            base.OnFormClosing(Argumentos);
        }
    }
}
