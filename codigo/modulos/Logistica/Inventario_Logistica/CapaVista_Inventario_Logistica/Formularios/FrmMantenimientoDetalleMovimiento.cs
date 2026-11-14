
/*
 * Autor: José Xavier Bolaños Tenas
 * Carné: 0901-23-2128
 *
 * Descripción:
 * Formulario de mantenimiento de detalles de movimientos
 * de inventario. Permite gestionar los registros mediante
 * el navegador, incluyendo las operaciones de ingreso,
 * modificación, eliminación, búsqueda y consulta.
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaControlador_Inventario_Logistica;
using CapaControlador_Inventario_Logistica.Modelos_de_Controladores;

namespace CapaVista_Inventario_Logistica.Formularios
{
    public partial class FrmMantenimientoDetalleMovimiento : Form
    {
        ClsModelosDetalleMovimiento _DatosDetalleMovimiento = new ClsModelosDetalleMovimiento();
        Dictionary<string, object> _FuentesReporteDetalleTipoMovimiento = new Dictionary<string, object>();
        // Constructor del formulario.
        public FrmMantenimientoDetalleMovimiento()
        {
            InitializeComponent();
            _FuentesReporteDetalleTipoMovimiento.Add("DsMantenimientoDetalleMovimiento", _DatosDetalleMovimiento.LogisticaMetObtenerTodos());

            // Configuración del navegador para detalles
            // de movimientos de inventario.
            LogisticaNavDetalleMovimiento.NavegadorMetConfigurar("tblDetalleMovimiento", 4, 6, _FuentesReporteDetalleTipoMovimiento);
        }
    }
}
