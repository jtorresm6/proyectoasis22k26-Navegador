
/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: FrmMantenimientoBodegas.cs
 *
 * REALIZADO POR: VELVETH SARAÍ CHÁVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 *
 * DESCRIPCIÓN:
 * Formulario de mantenimiento de bodegas.
 * Permite gestionar los registros mediante el Navegador
 * y generar reportes con los datos registrados.
 * ============================================================================
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaControlador_Inventario_Logistica;
using CapaControlador_Inventario_Logistica.Modelos_de_Controladores;

namespace CapaVista_Inventario_Logistica.Formularios
{
    public partial class FrmMantenimientoBodegas : Form
    {
        private readonly ClsModeloBodega _DatosBodega =
            new ClsModeloBodega();

        private readonly Dictionary<string, object> _FuentesReporteBodega =
            new Dictionary<string, object>();

        public FrmMantenimientoBodegas()
        {
            InitializeComponent();

            
                // ================================================
                // OBTENER LOS REGISTROS DE BODEGAS
                // ================================================

                List<ClsModeloBodega> ListaBodegas =
                    _DatosBodega.LogisticaMetObtenerTodos();

                _FuentesReporteBodega.Add("DsReporteBodega", ListaBodegas);

                // ================================================
                // CONFIGURAR EL NAVEGADOR
                // ================================================

                LogisticaNavBodega.NavegadorMetConfigurar("tblBodega",4,4,_FuentesReporteBodega );
            }
           
        }
}

