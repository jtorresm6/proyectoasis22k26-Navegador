// =====================================================================
// Autor:       Andre De Jesus Gonzalez Camey
// Carné:       9959-23-3117
// Módulo:      Tesorería y Bancos – Enlace con Contabilidad
// Descripción: Formulario que muestra el historial de partidas enviadas a
//              Contabilidad y su estado.
// Fecha:       12/11/2026
// =====================================================================
using System;
using System.Windows.Forms;
using CapaControlador_EnlaceB;
using System.Data;

namespace CapaVista_EnlaceB.Formularios
{
    public partial class FrmHistorialPartidas : Form
    {
        private readonly ClsPaginadorEnlace _Paginador;
        internal DataRow PartidaSeleccionada { get; private set; }

        public FrmHistorialPartidas()
        {
            InitializeComponent();
            _Paginador = new ClsPaginadorEnlace(_BancarioDgvPartidas, _BancarioBtnAnterior, _BancarioBtnSiguiente, _BancarioLblPagina);
        }

        internal FrmHistorialPartidas(DataTable Partidas) : this()
        {
            _Paginador.BancarioMetCargar(Partidas);
        }

        private void BancarioMetSeleccionar(object Remitente, EventArgs Argumentos)
        {
            var Seleccion = _BancarioDgvPartidas.CurrentRow?.DataBoundItem as DataRowView;
            if (Seleccion == null) return;
            PartidaSeleccionada = Seleccion.Row;
            DialogResult = DialogResult.OK;
        }
    }
}
