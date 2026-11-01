/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Modelo del controlador de tblDepartamento. Arma la lista que usa el reporte de la aplicación 9002.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using CapaModelo_Empleado.Entidades;
using CapaModelo_Empleado.Repositorios;

namespace CapaControlador_Empleado.Modelos_de_Controladores
{
    public class ClsModeloDepartamento
    {
        private int _IdDepartamento;
        private string _NombreDepartamento;
        private bool _EstadoDepartamento;

        public int IdDepartamento { get => _IdDepartamento; set => _IdDepartamento = value; }
        public string NombreDepartamento { get => _NombreDepartamento; set => _NombreDepartamento = value; }
        public bool EstadoDepartamento { get => _EstadoDepartamento; set => _EstadoDepartamento = value; }

        // Textos ya formateados para el reporte
        public string EstadoTexto { get => _EstadoDepartamento ? "Activo" : "Inactivo"; }

        private ClsRepositorioDepartamento _RepositorioDepartamento = new ClsRepositorioDepartamento();

        public List<ClsModeloDepartamento> RrhhMetObtenerParaReporte()
        {
            var Lista = new List<ClsModeloDepartamento>();

            foreach (ClsDepartamento Item in _RepositorioDepartamento.RrhhMetObtenerTodos())
            {
                Lista.Add(new ClsModeloDepartamento
                {
                    _IdDepartamento = Item.IdDepartamento,
                    _NombreDepartamento = Item.NombreDepartamento,
                    _EstadoDepartamento = Item.EstadoDepartamento
                });
            }

            return Lista;
        }
    }
}
