/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Modelo del controlador de tblJornada. Arma la lista que usa el reporte de la aplicación 9005.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using CapaModelo_Empleado.Entidades;
using CapaModelo_Empleado.Repositorios;

namespace CapaControlador_Empleado.Modelos_de_Controladores
{
    public class ClsModeloJornada
    {
        private int _IdJornada;
        private string _NombreJornada;
        private TimeSpan _HoraInicioJornada;
        private TimeSpan _HoraFinJornada;
        private int _MinutosToleranciaJornada;
        private bool _EstadoJornada;

        public int IdJornada { get => _IdJornada; set => _IdJornada = value; }
        public string NombreJornada { get => _NombreJornada; set => _NombreJornada = value; }
        public TimeSpan HoraInicioJornada { get => _HoraInicioJornada; set => _HoraInicioJornada = value; }
        public TimeSpan HoraFinJornada { get => _HoraFinJornada; set => _HoraFinJornada = value; }
        public int MinutosToleranciaJornada { get => _MinutosToleranciaJornada; set => _MinutosToleranciaJornada = value; }
        public bool EstadoJornada { get => _EstadoJornada; set => _EstadoJornada = value; }

        // Textos ya formateados para el reporte
        public string HoraInicioTexto { get => _HoraInicioJornada.ToString(@"hh\:mm"); }
        public string HoraFinTexto { get => _HoraFinJornada.ToString(@"hh\:mm"); }
        public string EstadoTexto { get => _EstadoJornada ? "Activo" : "Inactivo"; }

        private ClsRepositorioJornada _RepositorioJornada = new ClsRepositorioJornada();

        public List<ClsModeloJornada> RrhhMetObtenerParaReporte()
        {
            var Lista = new List<ClsModeloJornada>();

            foreach (ClsJornada Item in _RepositorioJornada.RrhhMetObtenerTodos())
            {
                Lista.Add(new ClsModeloJornada
                {
                    _IdJornada = Item.IdJornada,
                    _NombreJornada = Item.NombreJornada,
                    _HoraInicioJornada = Item.HoraInicioJornada,
                    _HoraFinJornada = Item.HoraFinJornada,
                    _MinutosToleranciaJornada = Item.MinutosToleranciaJornada,
                    _EstadoJornada = Item.EstadoJornada
                });
            }

            return Lista;
        }
    }
}
