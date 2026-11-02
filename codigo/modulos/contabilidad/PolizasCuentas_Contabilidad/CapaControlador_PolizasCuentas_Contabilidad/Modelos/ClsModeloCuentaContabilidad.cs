/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Oskar Cermeño
 * Carné : 0901-23-15379
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Modelo de una cuenta contable para la Vista: sus datos y las
 *  consultas por módulo ERP, de cuentas imputables, de ancestros y
 *  la búsqueda por texto.
 * Reglas especificas:
 *  Clase creada por Oskar Cermeño. Quedó de solo lectura porque el
 *  mantenimiento de cuentas lo hace el control Navegador; el filtro de
 *  cuentas por módulo se conserva para las pólizas.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Linq;
// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using CapaModelo_Contabilidad.Contratos;
using CapaModelo_Contabilidad.Entidades;
using CapaModelo_Contabilidad.Repositorios;
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaControlador_Contabilidad
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public class ClsModeloCuentaContabilidad
    {
        private readonly IRepositorioCuentasContabilidad _Repositorio;
        private List<ClsModeloCuentaContabilidad> _ListaCuentas;

        public int IdCuenta { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public int IdPadre { get; set; }
        public bool Activo { get; set; }
        public string Descripcion { get { return Codigo + " - " + Nombre; } }

        public ClsModeloCuentaContabilidad()
        {
            _Repositorio = new ClsRepositorioCuentasContabilidad();
        }

        public List<ClsModeloCuentaContabilidad> ContabilidadFuncObtenerTodos()
        {
            _ListaCuentas = ContabilidadFuncConvertir(_Repositorio.ContabilidadFuncObtenerTodos());
            return _ListaCuentas;
        }

        public List<ClsModeloCuentaContabilidad> ContabilidadFuncObtenerTodos(int IdModulo)
        {
            _ListaCuentas = ContabilidadFuncConvertir(_Repositorio.ContabilidadFuncObtenerTodos(IdModulo));
            return _ListaCuentas;
        }

        public List<ClsModeloCuentaContabilidad> ContabilidadFuncObtenerImputables(int IdModulo)
        {
            _ListaCuentas = ContabilidadFuncConvertir(_Repositorio.ContabilidadFuncObtenerImputables(IdModulo));
            return _ListaCuentas;
        }

        public List<ClsModeloCuentaContabilidad> ContabilidadFuncObtenerPadres()
        {
            return ContabilidadFuncConvertir(_Repositorio.ContabilidadFuncObtenerPadres(new ClsCuentaContabilidad { IdCuenta = IdCuenta }));
        }

        public IEnumerable<ClsModeloCuentaContabilidad> ContabilidadFuncBuscar(string Filtro)
        {
            if (_ListaCuentas == null) ContabilidadFuncObtenerTodos();
            return _ListaCuentas.FindAll(Cuenta =>
                Cuenta.Codigo.IndexOf(Filtro, StringComparison.OrdinalIgnoreCase) >= 0 ||
                Cuenta.Nombre.IndexOf(Filtro, StringComparison.OrdinalIgnoreCase) >= 0 ||
                Cuenta.Tipo.IndexOf(Filtro, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private static List<ClsModeloCuentaContabilidad> ContabilidadFuncConvertir(IEnumerable<ClsCuentaContabilidad> Entidades)
        {
            return Entidades.Select(Entidad => new ClsModeloCuentaContabilidad
            {
                IdCuenta = Entidad.IdCuenta, Codigo = Entidad.Codigo, Nombre = Entidad.Nombre,
                Tipo = Entidad.Tipo, IdPadre = Entidad.IdPadre, Activo = Entidad.Activo
            }).ToList();
        }
    }
}
