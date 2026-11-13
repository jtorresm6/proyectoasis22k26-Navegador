using CapaModelo_Liquidacion_Rrhh.Entidades;
using CapaModelo_Liquidacion_Rrhh.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_Liquidacion_Rrhh.Modelos_de_Controladores
{
    public class ClsModeloParametroLey
    {
        private int _IdParametroLey;
        private int _IdEmpresa;
        private string _CodigoParametroLey;
        private string _DescripcionParametroLey;
        private decimal _ValorParametroLey;
        private DateTime _VigenteDesdeParametroLey;
        private DateTime _VigenteHastaParametroLey;
        private bool _IsActive;

        public int IdParametroLey { get => _IdParametroLey; set => _IdParametroLey = value; }
        public int IdEmpresa { get => _IdEmpresa; set => _IdEmpresa = value; }
        public string CodigoParametroLey { get => _CodigoParametroLey; set => _CodigoParametroLey = value; }
        public string DescripcionParametroLey { get => _DescripcionParametroLey; set => _DescripcionParametroLey = value; }
        public decimal ValorParametroLey { get => _ValorParametroLey; set => _ValorParametroLey = value; }
        public DateTime VigenteDesdeParametroLey { get => _VigenteDesdeParametroLey; set => _VigenteDesdeParametroLey = value; }
        public DateTime VigenteHastaParametroLey { get => _VigenteHastaParametroLey; set => _VigenteHastaParametroLey = value; }
        public bool IsActive { get => _IsActive; set => _IsActive = value; }

        // Propiedades formateadas listas para presentar en el reporte visual
        public string EstadoTexto { get => _IsActive ? "Activo" : "Inactivo"; }
        public string FechaDesdeFormateada { get => _VigenteDesdeParametroLey.ToString("dd/MM/yyyy"); }
        public string FechaHastaFormateada { get => _VigenteHastaParametroLey.ToString("dd/MM/yyyy"); }

        private ClsRepositorioParametroLey _RepositorioParametroLey = new ClsRepositorioParametroLey();

        public List<ClsModeloParametroLey> ParametroLeyMetObtenerParaReporte()
        {
            var Resultado = _RepositorioParametroLey.RrhhMetObtenerTodos();
            var Lista = new List<ClsModeloParametroLey>();

            foreach (ClsParametroLey Item in Resultado)
            {
                Lista.Add(new ClsModeloParametroLey()
                {
                    _IdParametroLey = Item.IdParametroLey,
                    _IdEmpresa = Item.IdEmpresa,
                    _CodigoParametroLey = Item.CodigoParametroLey,
                    _DescripcionParametroLey = Item.DescripcionParametroLey,
                    _ValorParametroLey = Item.ValorParametroLey,
                    _VigenteDesdeParametroLey = Item.VigenteDesdeParametroLey,
                    _VigenteHastaParametroLey = Item.VigenteHastaParametroLey,
                    _IsActive = Item.IsActive
                });
            }

            return Lista;
        }
    }
}
