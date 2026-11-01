using System;
using System.Collections.Generic;
using CapaModelo_MovimientosBancarios.Contratos;
using System.Data.Odbc;
using CapaModelo_MovimientosBancarios.Entidades;
using CapaModelo_MovimientosBancarios.Repositorio;

namespace CapaControlador_MovimientosBancarios
{
    public class ClsModeloTipoTransaccion
    {
        private int _PK_TipoTransaccion;
        private string _CodigoTipoTransaccion;
        private string _NombreTipoTransaccion;
        private string _NaturalezaTipoTransaccion;
        private bool _IsActive;
        private DateTime _CreatedAt;
        private DateTime _UpdatedAt;
        private ClsRepositorioTipoTransaccion _RepositorioTipoTransaccion;

        public EstadoEntidad Estado { private get; set; }
        private List<ClsModeloTipoTransaccion> _ListaTipoTransaccion;

        public int PK_TipoTransaccion { get => _PK_TipoTransaccion; set => _PK_TipoTransaccion = value; }
        public string CodigoTipoTransaccion { get => _CodigoTipoTransaccion; set => _CodigoTipoTransaccion = value; }
        public string NombreTipoTransaccion { get => _NombreTipoTransaccion; set => _NombreTipoTransaccion = value; }
        public string NaturalezaTipoTransaccion { get => _NaturalezaTipoTransaccion; set => _NaturalezaTipoTransaccion = value; }
        public bool IsActive { get => _IsActive; set => _IsActive = value; }
        public DateTime CreatedAt { get => _CreatedAt; private set => _CreatedAt = value; }
        public DateTime UpdatedAt { get => _UpdatedAt; private set => _UpdatedAt = value; }

        public ClsModeloTipoTransaccion()
        {
            _RepositorioTipoTransaccion = new ClsRepositorioTipoTransaccion();
        }

        public string BancosMetGrabarCambios()
        {
            string Mensaje = null;

            var ModeloDatos = new ClsTipoTransaccion();
            ModeloDatos.PK_TipoTransaccion = _PK_TipoTransaccion;
            ModeloDatos.CodigoTipoTransaccion = _CodigoTipoTransaccion;
            ModeloDatos.NombreTipoTransaccion = _NombreTipoTransaccion;
            ModeloDatos.NaturalezaTipoTransaccion = _NaturalezaTipoTransaccion;

            switch (Estado)
            {
                case EstadoEntidad.Added:
                    _RepositorioTipoTransaccion.BancosMetAgregar(ModeloDatos);
                    Mensaje = "Grabacion exitosa";
                    break;
                case EstadoEntidad.Modified:
                    _RepositorioTipoTransaccion.BancosMetEditar(ModeloDatos);
                    Mensaje = "Actualizacion exitosa";
                    break;
                case EstadoEntidad.Deleted:
                    _RepositorioTipoTransaccion.BancosMetRemover(ModeloDatos);
                    Mensaje = "Eliminacion exitosa";
                    break;
            }
            return Mensaje;
        }

        public List<ClsModeloTipoTransaccion> BancosMetObtenerTodos()
        {
            var ResultadoConsulta = _RepositorioTipoTransaccion.BancosMetObtenerTodos();
            _ListaTipoTransaccion = new List<ClsModeloTipoTransaccion>();
            foreach (ClsTipoTransaccion Item in ResultadoConsulta)
            {
                _ListaTipoTransaccion.Add(new ClsModeloTipoTransaccion
                {
                    _PK_TipoTransaccion = Item.PK_TipoTransaccion,
                    _CodigoTipoTransaccion = Item.CodigoTipoTransaccion,
                    _NombreTipoTransaccion = Item.NombreTipoTransaccion,
                    _NaturalezaTipoTransaccion = Item.NaturalezaTipoTransaccion,
                    _IsActive = Item.IsActive,
                    _CreatedAt = Item.CreatedAt,
                    _UpdatedAt = Item.UpdatedAt
                });
            }
            return _ListaTipoTransaccion;
        }

        public IEnumerable<ClsModeloTipoTransaccion> BancosMetBuscarPorCodigo(string CodigoTipoTransaccion)
        {
            if (_ListaTipoTransaccion == null)
                BancosMetObtenerTodos();

            return _ListaTipoTransaccion.FindAll(e => e._CodigoTipoTransaccion == CodigoTipoTransaccion);
        }
    }
}