using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaModelo_Empleado.Entidades;
using CapaModelo_Empleado.Repositorios;

namespace CapaControlador_Empleado.Modelos_de_Controladores
{
    public class ClsModeloInstructor
    {
        private int _IdInstructor;
        private string _NombreInstructor;
        private string _EspecialidadInstructor;
        private string _CorreoInstructor;
        private bool _EstadoInstructor;

        public int IdInstructor { get => _IdInstructor; set => _IdInstructor = value; }
        public string NombreInstructor { get => _NombreInstructor; set => _NombreInstructor = value; }
        public string EspecialidadInstructor { get => _EspecialidadInstructor; set => _EspecialidadInstructor = value; }
        public string CorreoInstructor { get => _CorreoInstructor; set => _CorreoInstructor = value; }
        public bool EstadoInstructor { get => _EstadoInstructor; set => _EstadoInstructor = value; }

        public string EstadoTexto { get => _EstadoInstructor ? "Activo" : "Inactivo"; }

        private ClsRepositorioInstructor _RepositorioInstructor = new ClsRepositorioInstructor();

        public List<ClsModeloInstructor> InstructorMetObtenerParaReporte()
        {
            var Resultado = _RepositorioInstructor.RrhhMetObtenerTodos();
            var Lista = new List<ClsModeloInstructor>();

            foreach (ClsInstructor Item in Resultado)
            {
                Lista.Add(new ClsModeloInstructor
                {
                    _IdInstructor = Item.IdInstructor,
                    _NombreInstructor = Item.NombreInstructor,
                    _EspecialidadInstructor = Item.EspecialidadInstructor,
                    _CorreoInstructor = Item.CorreoInstructor,
                    _EstadoInstructor = Item.EstadoInstructor
                });
            }

            return Lista;
        }
    }
}
