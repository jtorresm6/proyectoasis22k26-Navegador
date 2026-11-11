using System;
using System.Collections.Generic;
using System.Drawing;
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
 *  Agrupa las herramientas de moneda de Compras: tipo de cambio vigente,
 *  conversor de montos, moneda de visualización e historial del tipo de
 *  cambio. Las monedas y tipos de cambio se leen de las tablas de Bancos.
 * ===================================================================
*/

namespace CapaVista_Herramientas
{
    public partial class FrmHerramientasMoneda : Form
    {
        private readonly ClsControladorMoneda _Controlador = new ClsControladorMoneda();
        private readonly Dictionary<ClsButtonCompras, Control> _Secciones = new Dictionary<ClsButtonCompras, Control>();

        public FrmHerramientasMoneda()
        {
            InitializeComponent();
            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);

            _Secciones.Add(ComprasBtnTipoCambio, ComprasUsrTipoCambio);
            _Secciones.Add(ComprasBtnConversor, ComprasUsrConversor);
            _Secciones.Add(ComprasBtnVisualizacion, ComprasUsrVisualizacion);
            _Secciones.Add(ComprasBtnHistorial, ComprasUsrHistorial);
            foreach (ClsButtonCompras Pestana in _Secciones.Keys) Pestana.Click += ComprasMetSeleccionarSeccion;

            ComprasBtnSalir.Click += (Remitente, Evento) => Close();
            Load += ComprasMetCargar;
        }

        private void ComprasMetCargar(object Remitente, EventArgs Evento)
        {
            ComprasProcMostrarSeccion(ComprasBtnTipoCambio);

            List<ClsMonedaCompras> Monedas;
            try
            {
                Monedas = _Controlador.ComprasFuncObtenerMonedas();
            }
            catch (InvalidOperationException Error)
            {
                MessageBox.Show(Error.Message, "Herramientas de moneda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Monedas.Count == 0)
            {
                MessageBox.Show("Bancos todavía no tiene monedas activas registradas.", "Herramientas de moneda",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ClsMonedaCompras MonedaLocal = _Controlador.ComprasFuncObtenerMonedaLocal(Monedas);
            ComprasUsrTipoCambio.ComprasProcInicializar(_Controlador);
            ComprasUsrConversor.ComprasProcInicializar(_Controlador, Monedas, MonedaLocal);
            ComprasUsrVisualizacion.ComprasProcInicializar(_Controlador, Monedas, MonedaLocal);
            ComprasUsrHistorial.ComprasProcInicializar(_Controlador, Monedas, MonedaLocal);
        }

        private void ComprasMetSeleccionarSeccion(object Remitente, EventArgs Evento)
        {
            ComprasProcMostrarSeccion((ClsButtonCompras)Remitente);
        }

        private void ComprasProcMostrarSeccion(ClsButtonCompras Pestana)
        {
            foreach (KeyValuePair<ClsButtonCompras, Control> Seccion in _Secciones)
            {
                bool Activa = Seccion.Key == Pestana;
                Seccion.Value.Visible = Activa;
                Seccion.Key.BackColor = Activa ? ClsTemaCompras.AcentoNavegador : ClsTemaCompras.FondoBotones;
                Seccion.Key.ForeColor = Activa ? Color.White : ClsTemaCompras.PrincipalNavegador;
            }
        }
    }
}
