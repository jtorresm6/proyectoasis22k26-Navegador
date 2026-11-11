using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Proporciona el control visual base para los componentes reutilizables de Compras.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    public partial class ComprasUsrBase : UserControl
    {

        private Size _TamanoDiseno;
        private float _EscalaAltura = 1F;
        internal event EventHandler ComprasEvtTamanoNecesario;
        internal Size ComprasTamanoDiseno => _TamanoDiseno;

        internal void ComprasProcRegistrarDiseno()
        {
            _TamanoDiseno = ClientSize;
        }

        internal int ComprasFuncEscalarAltura(float Altura)
        {
            return (int)Math.Ceiling(Altura * _EscalaAltura);
        }

        internal void ComprasProcNotificarTamano()
        {
            ComprasEvtTamanoNecesario?.Invoke(this, EventArgs.Empty);
        }

        internal virtual Size ComprasFuncTamanoNecesario()
        {
            return _TamanoDiseno;
        }

        protected override void OnLoad(EventArgs Evento)
        {
            if (_TamanoDiseno.IsEmpty) ComprasProcRegistrarDiseno();
            base.OnLoad(Evento);
        }

        protected void ComprasProcAgregarColumna(ComprasUsrDataGridView Tabla, string Campo,
            string Encabezado, int Peso, bool EsMonto)
        {
            ClsPresentacionCompras.ComprasProcAgregarColumna(Tabla, Campo, Encabezado, Peso, EsMonto);
        }

        protected void ComprasProcMostrarMonto(ComprasUsrTextBox Entrada, decimal Monto)
        {
            ClsPresentacionCompras.ComprasProcMostrarMonto(Entrada, Monto);
        }

        protected void ComprasMetSalir(object Remitente, EventArgs Evento)
        {
            Hide();
        }

        protected override void ScaleControl(SizeF Factor, BoundsSpecified Especificado)
        {
            base.ScaleControl(Factor, Especificado);
            if (_TamanoDiseno.IsEmpty) return;
            _TamanoDiseno = new Size((int)Math.Ceiling(_TamanoDiseno.Width * Factor.Width), (int)Math.Ceiling(_TamanoDiseno.Height * Factor.Height));
            _EscalaAltura *= Factor.Height;
            ComprasProcNotificarTamano();
        }

        public ComprasUsrBase()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

    }
}
