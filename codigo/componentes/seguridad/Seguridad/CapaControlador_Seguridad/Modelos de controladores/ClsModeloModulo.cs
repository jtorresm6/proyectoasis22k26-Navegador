/*
 * ==================================================================
 * Área : Seguridad
 * Autor : Victor Samayoa y Oscar Morales
 * Carné : 9959-23-3424 y 9959-23-3070
 * Fecha : 24/09/2026
 * ==================================================================
 *  El ClsModeloModulo es el controlador que valida y prepara los
 *  datos de un módulo antes de enviarlos al repositorio, aplica
 *  reglas especificas: no permitir nombres de módulo duplicados,
 *  informar errores de forma clara cuando ocurre un problema de
 *  base de datos, y registrar cada operación (Agregar, Editar,
 *  Eliminar) en la bitácora del sistema.
 * ===================================================================
*/

using CapaModelo_Seguridad.Entidades;
using CapaModelo_Seguridad.Repositorios;
using System;
using System.Data;
using System.Data.Odbc;

namespace CapaControlador_Seguridad
{
    public class ClsModeloModulo
    {
        private int _IdModulo;
        private string _NombreModulo;
        private string _DescripcionModulo;
        private bool _IsActive;
        private ClsRepositorioModulo _RepositorioModulo;

        // Propiedad clave para saber si Guardamos, Editamos o Eliminamos
        public EstadoEntidad Estado { private get; set; }

        public int IdModulo { get => _IdModulo; set => _IdModulo = value; }
        public string NombreModulo { get => _NombreModulo; set => _NombreModulo = value; }
        public string DescripcionModulo { get => _DescripcionModulo; set => _DescripcionModulo = value; }
        public bool IsActive { get => _IsActive; set => _IsActive = value; }

        public ClsModeloModulo()
        {
            _RepositorioModulo = new ClsRepositorioModulo();
        }

        public string SeguridadMetGrabarCambios()
        {
            string Mensaje = null;
            try
            {
                var Modulo = new ClsModulo
                {
                    IdModulo = _IdModulo,
                    NombreModulo = _NombreModulo,
                    DescripcionModulo = _DescripcionModulo,
                    IsActive = _IsActive
                };

                switch (Estado)
                {
                    case EstadoEntidad.Added:
                        _RepositorioModulo.SeguridadMetAgregar(Modulo);
                        ClsModeloBitacora.SeguridadMetRegistrarAccion("INSERT", "tblModulo", Modulo.IdModulo, "Se agregó el módulo: " + _NombreModulo);
                        Mensaje = "Registro guardado exitosamente.";
                        break;
                    case EstadoEntidad.Modified:
                        _RepositorioModulo.SeguridadMetEditar(Modulo);
                        ClsModeloBitacora.SeguridadMetRegistrarAccion("UPDATE", "tblModulo", Modulo.IdModulo, "Se actualizó el módulo: " + _NombreModulo);
                        Mensaje = "Registro actualizado exitosamente.";
                        break;
                    case EstadoEntidad.Deleted:
                        _RepositorioModulo.SeguridadMetRemover(Modulo);
                        ClsModeloBitacora.SeguridadMetRegistrarAccion("DELETE", "tblModulo", Modulo.IdModulo, "Se eliminó el módulo ID: " + Modulo.IdModulo);
                        Mensaje = "Registro eliminado exitosamente.";
                        break;
                }
            }
            catch (OdbcException ex)
            {
                if (ex.Errors.Count > 0 && ex.Errors[0].NativeError == 1062)
                    Mensaje = "Ya existe un módulo con ese nombre. Use un nombre diferente.";
                else
                    Mensaje = "Ocurrió un problema al procesar la solicitud. Verifique los datos e intente nuevamente.";
            }
            catch (Exception)
            {
                Mensaje = "Ocurrió un error inesperado en el sistema. Intente nuevamente o contacte al administrador.";
            }
            return Mensaje;
        }

        public DataTable SeguridadMetObtenerModulosTabla()
        {
            return _RepositorioModulo.SeguridadMetObtenerModulosTabla();
        }
        public DataTable SeguridadMetObtenerModulosReporte()
        {
            return _RepositorioModulo.SeguridadMetObtenerModulosReporte();
        }
    }
}