/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Modelo del controlador de tblPuesto. Arma la lista que usa el reporte de la aplicación 9004.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using CapaModelo_Empleado.Entidades;
using CapaModelo_Empleado.Repositorios;

namespace CapaControlador_Empleado.Modelos_de_Controladores
{
    public class ClsModeloPuesto
    {
        private int _IdPuesto;
        private int _IdTipoPuesto;
        private string _NombreTipoPuesto;
        private string _NombrePuesto;
        private string _DescripcionPuesto;
        private bool _EstadoPuesto;

        public int IdPuesto { get => _IdPuesto; set => _IdPuesto = value; }
        public int IdTipoPuesto { get => _IdTipoPuesto; set => _IdTipoPuesto = value; }
        public string NombreTipoPuesto { get => _NombreTipoPuesto; set => _NombreTipoPuesto = value; }
        public string NombrePuesto { get => _NombrePuesto; set => _NombrePuesto = value; }
        public string DescripcionPuesto { get => _DescripcionPuesto; set => _DescripcionPuesto = value; }
        public bool EstadoPuesto { get => _EstadoPuesto; set => _EstadoPuesto = value; }

        // Textos ya formateados para el reporte
        public string EstadoTexto { get => _EstadoPuesto ? "Activo" : "Inactivo"; }

        private ClsRepositorioPuesto _RepositorioPuesto = new ClsRepositorioPuesto();

        public List<ClsModeloPuesto> RrhhMetObtenerParaReporte()
        {
            var Lista = new List<ClsModeloPuesto>();

            foreach (ClsPuesto Item in _RepositorioPuesto.RrhhMetObtenerTodos())
            {
                Lista.Add(new ClsModeloPuesto
                {
                    _IdPuesto = Item.IdPuesto,
                    _IdTipoPuesto = Item.IdTipoPuesto,
                    _NombreTipoPuesto = Item.NombreTipoPuesto,
                    _NombrePuesto = Item.NombrePuesto,
                    _DescripcionPuesto = Item.DescripcionPuesto,
                    _EstadoPuesto = Item.EstadoPuesto
                });
            }

            return Lista;
        }
    }
}
