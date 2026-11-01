using System;
using System.Collections.Generic;
using CapaModelo_Empleado.Entidades;
using CapaModelo_Empleado.Repositorios;

namespace CapaControlador_Empleado.Modelos_de_Controladores
{
    public class ClsModeloCapacitacion
    {
        private int _IdCapacitacion;
        private int? _IdDepartamento;
        private string _NombreDepartamento;
        private string _NombreCapacitacion;
        private string _DescripcionCapacitacion;
        private DateTime _FechaInicioCapacitacion;
        private DateTime _FechaFinCapacitacion;
        private decimal _CostoCapacitacion;
        private int _CupoCapacitacion;
        private bool _EstadoCapacitacion;

        public int IdCapacitacion { get => _IdCapacitacion; set => _IdCapacitacion = value; }
        public int? IdDepartamento { get => _IdDepartamento; set => _IdDepartamento = value; }
        public string NombreDepartamento { get => _NombreDepartamento; set => _NombreDepartamento = value; }
        public string NombreCapacitacion { get => _NombreCapacitacion; set => _NombreCapacitacion = value; }
        public string DescripcionCapacitacion { get => _DescripcionCapacitacion; set => _DescripcionCapacitacion = value; }
        public DateTime FechaInicioCapacitacion { get => _FechaInicioCapacitacion; set => _FechaInicioCapacitacion = value; }
        public DateTime FechaFinCapacitacion { get => _FechaFinCapacitacion; set => _FechaFinCapacitacion = value; }
        public decimal CostoCapacitacion { get => _CostoCapacitacion; set => _CostoCapacitacion = value; }
        public int CupoCapacitacion { get => _CupoCapacitacion; set => _CupoCapacitacion = value; }
        public bool EstadoCapacitacion { get => _EstadoCapacitacion; set => _EstadoCapacitacion = value; }


        public string EstadoTexto { get => _EstadoCapacitacion ? "Activa" : "Inactiva"; }

        private ClsRepositorioCapacitacion _RepositorioCapacitacion = new ClsRepositorioCapacitacion();

        public List<ClsModeloCapacitacion> CapacitacionMetObtenerParaReporte()
        {
            var Resultado = _RepositorioCapacitacion.RrhhMetObtenerTodos();
            var Lista = new List<ClsModeloCapacitacion>();

            foreach (ClsCapacitacion Item in Resultado)
            {
                Lista.Add(new ClsModeloCapacitacion
                {
                    _IdCapacitacion = Item.IdCapacitacion,
                    _IdDepartamento = Item.IdDepartamento,
                    _NombreDepartamento = Item.NombreDepartamento,
                    _NombreCapacitacion = Item.NombreCapacitacion,
                    _DescripcionCapacitacion = Item.DescripcionCapacitacion,
                    _FechaInicioCapacitacion = Item.FechaInicioCapacitacion,
                    _FechaFinCapacitacion = Item.FechaFinCapacitacion,
                    _CostoCapacitacion = Item.CostoCapacitacion,
                    _CupoCapacitacion = Item.CupoCapacitacion,
                    _EstadoCapacitacion = Item.EstadoCapacitacion
                });
            }

            return Lista;
        }
    }
}