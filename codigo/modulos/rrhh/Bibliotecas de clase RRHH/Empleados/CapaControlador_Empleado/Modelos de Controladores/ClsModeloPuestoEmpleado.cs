/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Modelo del controlador de tblPuestoEmpleado. Arma la lista que usa el reporte de la aplicación 9006.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using CapaModelo_Empleado.Entidades;
using CapaModelo_Empleado.Repositorios;

namespace CapaControlador_Empleado.Modelos_de_Controladores
{
    public class ClsModeloPuestoEmpleado
    {
        private int _IdPuestoEmpleado;
        private int _IdEmpleado;
        private string _CodigoEmpleado;
        private string _NombreEmpleado;
        private int _IdPuesto;
        private string _NombrePuesto;
        private DateTime _FechaAsignacionPuestoEmpleado;
        private DateTime? _FechaFinPuestoEmpleado;
        private bool _EstadoPuestoEmpleado;

        public int IdPuestoEmpleado { get => _IdPuestoEmpleado; set => _IdPuestoEmpleado = value; }
        public int IdEmpleado { get => _IdEmpleado; set => _IdEmpleado = value; }
        public string CodigoEmpleado { get => _CodigoEmpleado; set => _CodigoEmpleado = value; }
        public string NombreEmpleado { get => _NombreEmpleado; set => _NombreEmpleado = value; }
        public int IdPuesto { get => _IdPuesto; set => _IdPuesto = value; }
        public string NombrePuesto { get => _NombrePuesto; set => _NombrePuesto = value; }
        public DateTime FechaAsignacionPuestoEmpleado { get => _FechaAsignacionPuestoEmpleado; set => _FechaAsignacionPuestoEmpleado = value; }
        public DateTime? FechaFinPuestoEmpleado { get => _FechaFinPuestoEmpleado; set => _FechaFinPuestoEmpleado = value; }
        public bool EstadoPuestoEmpleado { get => _EstadoPuestoEmpleado; set => _EstadoPuestoEmpleado = value; }

        // Textos ya formateados para el reporte
        public string FechaAsignacionTexto { get => _FechaAsignacionPuestoEmpleado.ToString("dd/MM/yyyy"); }
        public string FechaFinTexto { get => _FechaFinPuestoEmpleado.HasValue ? _FechaFinPuestoEmpleado.Value.ToString("dd/MM/yyyy") : "Vigente"; }
        public string EstadoTexto { get => _EstadoPuestoEmpleado ? "Activo" : "Inactivo"; }

        private ClsRepositorioPuestoEmpleado _RepositorioPuestoEmpleado = new ClsRepositorioPuestoEmpleado();

        public List<ClsModeloPuestoEmpleado> RrhhMetObtenerParaReporte()
        {
            var Lista = new List<ClsModeloPuestoEmpleado>();

            foreach (ClsPuestoEmpleado Item in _RepositorioPuestoEmpleado.RrhhMetObtenerTodos())
            {
                Lista.Add(new ClsModeloPuestoEmpleado
                {
                    _IdPuestoEmpleado = Item.IdPuestoEmpleado,
                    _IdEmpleado = Item.IdEmpleado,
                    _CodigoEmpleado = Item.CodigoEmpleado,
                    _NombreEmpleado = Item.NombreEmpleado,
                    _IdPuesto = Item.IdPuesto,
                    _NombrePuesto = Item.NombrePuesto,
                    _FechaAsignacionPuestoEmpleado = Item.FechaAsignacionPuestoEmpleado,
                    _FechaFinPuestoEmpleado = Item.FechaFinPuestoEmpleado,
                    _EstadoPuestoEmpleado = Item.EstadoPuestoEmpleado
                });
            }

            return Lista;
        }
    }
}
