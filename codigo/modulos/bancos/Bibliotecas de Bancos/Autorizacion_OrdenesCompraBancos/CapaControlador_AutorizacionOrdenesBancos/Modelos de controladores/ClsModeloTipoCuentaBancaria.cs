using CapaModelo_AutorizacionOrdenes.Entidades;
using CapaModelo_Bancos.Repositorios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ==================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 07/11/2026
 * ==================================================================
 * Propósito :
 *  La clase Modelo de Tipo de Cuenta Bancaria pertenece a la capa
 *  controlador. Actúa como intermediario entre la vista y el repositorio
 *  de datos (_RepositorioTipoCuentaBancaria) gestionando el estado CRUD
 *  de cada registro: agregar, modificar y eliminar, mediante el
 *  EstadoEntidad, aplicando validaciones con Data Annotations y
 *  retornando un mensaje con el resultado de la operación.
 * ===================================================================
*/

namespace CapaControlador_AutorizacionOrdenes.Modelos_de_controladores
{
    public class ClsModeloTipoCuentaBancaria
    {
        private int _IdTipoCuentaBancaria;
        private string _CodigoTipoCuentaBancaria;
        private string _NombreTipoCuentaBancaria;
        private bool _IsActive;
        private DateTime _CreatedAt;
        private DateTime _UpdatedAt;
        private ClsRepositorioTipoCuentaBancaria _RepositorioTipoCuentaBancaria;

        public EstadoEntidad Estado { private get; set; }
        private List<ClsModeloTipoCuentaBancaria> _ListaTipoCuentaBancaria;

        public int IdTipoCuentaBancaria { get => _IdTipoCuentaBancaria; set => _IdTipoCuentaBancaria = value; }

        [Required(ErrorMessage = "El campo Código Tipo Cuenta Bancaria es requerido")]
        public string CodigoTipoCuentaBancaria { get => _CodigoTipoCuentaBancaria; set => _CodigoTipoCuentaBancaria = value; }

        [Required(ErrorMessage = "El campo Nombre Tipo Cuenta Bancaria es requerido")]
        public string NombreTipoCuentaBancaria { get => _NombreTipoCuentaBancaria; set => _NombreTipoCuentaBancaria = value; }

        [Required(ErrorMessage = "El campo Estado es requerido")]
        public bool IsActive { get => _IsActive; set => _IsActive = value; }

        public DateTime CreatedAt { get => _CreatedAt; private set => _CreatedAt = value; }

        public DateTime UpdatedAt { get => _UpdatedAt; private set => _UpdatedAt = value; }

        public string BancosMetGrabarCambios()
        {
            string Mensaje = null;
            try
            {
                var DatosTipoCuentaBancaria = new ClsTipoCuentaBancaria();
                DatosTipoCuentaBancaria.IdTipoCuentaBancaria = _IdTipoCuentaBancaria;
                DatosTipoCuentaBancaria.CodigoTipoCuentaBancaria = _CodigoTipoCuentaBancaria;
                DatosTipoCuentaBancaria.NombreTipoCuentaBancaria = _NombreTipoCuentaBancaria;
                DatosTipoCuentaBancaria.IsActive = _IsActive;

                switch (Estado)
                {
                    case EstadoEntidad.Added:
                        _RepositorioTipoCuentaBancaria.BancosMetAgregar(DatosTipoCuentaBancaria);
                        Mensaje = "Grabacion exitosa";
                        break;

                    case EstadoEntidad.Modified:
                        _RepositorioTipoCuentaBancaria.BancosMetEditar(DatosTipoCuentaBancaria);
                        Mensaje = "Actualizacion exitosa";
                        break;

                    case EstadoEntidad.Deleted:
                        _RepositorioTipoCuentaBancaria.BancosMetRemover(DatosTipoCuentaBancaria);
                        Mensaje = "Eliminacion exitosa";
                        break;
                }
            }
            catch (Exception Ex)
            {
                Mensaje = "No se pudo grabar: " + Ex.Message;
            }
            return Mensaje;
        }

        public List<ClsModeloTipoCuentaBancaria> BancosMetObtenerTodos()
        {
            var DatosTipoCuentaBancaria = _RepositorioTipoCuentaBancaria.BancosMetObtenerTodos();
            _ListaTipoCuentaBancaria = new List<ClsModeloTipoCuentaBancaria>();
            foreach (ClsTipoCuentaBancaria Item in DatosTipoCuentaBancaria)
            {
                _ListaTipoCuentaBancaria.Add(new ClsModeloTipoCuentaBancaria
                {
                    _IdTipoCuentaBancaria = Item.IdTipoCuentaBancaria,
                    _CodigoTipoCuentaBancaria = Item.CodigoTipoCuentaBancaria,
                    _NombreTipoCuentaBancaria = Item.NombreTipoCuentaBancaria,
                    _IsActive = Item.IsActive,
                    _CreatedAt = Item.CreatedAt,
                    _UpdatedAt = Item.UpdatedAt
                });
            }
            return _ListaTipoCuentaBancaria;
        }

        public ClsModeloTipoCuentaBancaria BancosMetBuscarPorId(int IdTipoCuentaBancaria)
        {
            return _ListaTipoCuentaBancaria.Find(e => e._IdTipoCuentaBancaria == IdTipoCuentaBancaria);
        }

        public ClsModeloTipoCuentaBancaria()
        {
            _RepositorioTipoCuentaBancaria = new ClsRepositorioTipoCuentaBancaria();
        }
    }
}