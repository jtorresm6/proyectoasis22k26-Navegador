/*
 * Autor: José Xavier Bolaños Tenas
 * Carné: 0901-23-2128
 * Descripción: Obtiene tipos de movimiento del repositorio y adapta sus datos al modelo del controlador.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaModelo_Inventario_Logistica.Entidades;
using CapaModelo_Inventario_Logistica.Repositorios;

namespace CapaControlador_Inventario_Logistica.Modelos_de_Controladores
{
    public class ClsModelosTipoMovimiento
    {
        private readonly ClsRepositorioTipoMovimiento _RepositorioTipoMovimiento;

        public int _PkTipoMovimiento { get; set; }
        public string _NombreTipoMovimiento { get; set; }
        public string _DescripcionTipoMovimiento { get; set; }
        public string _SignoTipoMovimiento { get; set; }
        public DateTime _CreatedAt { get; set; }
        public DateTime _UpdatedAt { get; set; }

        public ClsModelosTipoMovimiento()
        {
            _RepositorioTipoMovimiento = new ClsRepositorioTipoMovimiento();
        }

        public List<ClsModelosTipoMovimiento> LogisticaMetObtenerTodos()
        {
            var ResultadoConsulta = _RepositorioTipoMovimiento.LogisticaMetObtenerTodos();
            var ListaTipoMovimiento = new List<ClsModelosTipoMovimiento>();
            foreach (ClsTipoMovimiento Item in ResultadoConsulta)
            {
                ListaTipoMovimiento.Add(new ClsModelosTipoMovimiento
                {
                    _PkTipoMovimiento = Item.Pk_TipoMovimiento,
                    _NombreTipoMovimiento = Item.NombreTipoMovimiento,
                    _DescripcionTipoMovimiento = Item.DescripcionTipoMovimiento,
                    _SignoTipoMovimiento = Item.SignoTipoMovimiento,
                    _CreatedAt = Item.CreatedAt,
                    _UpdatedAt = Item.UpdatedAt
                });
            }
            return ListaTipoMovimiento;
        }
    }
}
