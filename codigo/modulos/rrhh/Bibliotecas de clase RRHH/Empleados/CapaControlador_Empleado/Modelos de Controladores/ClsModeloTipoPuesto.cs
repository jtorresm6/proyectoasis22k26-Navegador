/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Modelo del controlador de tblTipoPuesto. Arma la lista que usa el reporte de la aplicación 9003.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using CapaModelo_Empleado.Entidades;
using CapaModelo_Empleado.Repositorios;

namespace CapaControlador_Empleado.Modelos_de_Controladores
{
    public class ClsModeloTipoPuesto
    {
        private int _IdTipoPuesto;
        private string _NombreTipoPuesto;
        private bool _EstadoTipoPuesto;

        public int IdTipoPuesto { get => _IdTipoPuesto; set => _IdTipoPuesto = value; }
        public string NombreTipoPuesto { get => _NombreTipoPuesto; set => _NombreTipoPuesto = value; }
        public bool EstadoTipoPuesto { get => _EstadoTipoPuesto; set => _EstadoTipoPuesto = value; }

        // Textos ya formateados para el reporte
        public string EstadoTexto { get => _EstadoTipoPuesto ? "Activo" : "Inactivo"; }

        private ClsRepositorioTipoPuesto _RepositorioTipoPuesto = new ClsRepositorioTipoPuesto();

        public List<ClsModeloTipoPuesto> RrhhMetObtenerParaReporte()
        {
            var Lista = new List<ClsModeloTipoPuesto>();

            foreach (ClsTipoPuesto Item in _RepositorioTipoPuesto.RrhhMetObtenerTodos())
            {
                Lista.Add(new ClsModeloTipoPuesto
                {
                    _IdTipoPuesto = Item.IdTipoPuesto,
                    _NombreTipoPuesto = Item.NombreTipoPuesto,
                    _EstadoTipoPuesto = Item.EstadoTipoPuesto
                });
            }

            return Lista;
        }
    }
}
