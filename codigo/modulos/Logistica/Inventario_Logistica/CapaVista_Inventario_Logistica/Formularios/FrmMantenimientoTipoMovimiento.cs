
/*
 * Autor: José Xavier Bolaños Tenas
 * Carné: 0901-23-2128
 *
 * Descripción:
 * Formulario de mantenimiento para los tipos
 * de movimiento de inventario.
 *
 * Permite ingresar, guardar, modificar y eliminar
 * tipos de movimiento mediante el navegador,
 * incluyendo la búsqueda y consulta de registros.
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaControlador_Inventario_Logistica;
using CapaControlador_Inventario_Logistica.Modelos_de_Controladores;

namespace CapaVista_Inventario_Logistica.Formularios
{
    public partial class FrmMantenimientoTipoMovimiento : Form
    {
        ClsModelosTipoMovimiento _DatosTipoMovimiento = new ClsModelosTipoMovimiento();
        Dictionary<string, object> _FuenteReporteTipoMovimiento = new Dictionary<string, object>();

        // Constructor del formulario.
        public FrmMantenimientoTipoMovimiento()
        {
            InitializeComponent();
            _FuenteReporteTipoMovimiento.Add("DsMantenimientoTipoMovimiento", _DatosTipoMovimiento.LogisticaMetObtenerTodos());

            // Configuración del navegador para
            // tipos de movimiento de inventario.
            LogisticaNavTipoMovimiento.NavegadorMetConfigurar("tblTipoMovimiento", 4, 6, _FuenteReporteTipoMovimiento);
        }
    }
}
