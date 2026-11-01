using System;

namespace CapaModelo_Empleado.Entidades
{
    public class ClsCapacitacion
    {
        public int IdCapacitacion { get; set; }
        public int? IdDepartamento { get; set; }
        public string NombreDepartamento { get; set; }
        public string NombreCapacitacion { get; set; }
        public string DescripcionCapacitacion { get; set; }
        public DateTime FechaInicioCapacitacion { get; set; }
        public DateTime FechaFinCapacitacion { get; set; }
        public decimal CostoCapacitacion { get; set; }
        public int CupoCapacitacion { get; set; }
        public bool EstadoCapacitacion { get; set; }
    }
}