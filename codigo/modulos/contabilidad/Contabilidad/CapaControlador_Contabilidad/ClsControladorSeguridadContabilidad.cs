/*
 * Área: Contabilidad
 * Autor: Gabriel André Guillén Pocón
 * Carné: 0901-23-1998
 * Fecha: 10/10/2026
 * Propósito: leer la sesión de Contabilidad y resolver los permisos de sus roles.
 * Los códigos de ventana se relacionan con los IDs reales del catálogo;
 * no se crean usuarios, sesiones, aplicaciones ni permisos.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using CapaModelo_Contabilidad;

namespace CapaControlador_Contabilidad
{
    public class ClsControladorSeguridadContabilidad
    {
        private readonly Dictionary<int, string> _Aplicaciones = new Dictionary<int, string>
        {
            { 10001, "Pólizas locales" }, { 10002, "Presupuestos" },
            { 10003, "Activos fijos" }, { 10004, "Cierre contable" },
            { 10005, "Estados financieros" }
        };

        public ClsEstadoSeguridadContabilidad ContabilidadFuncObtenerEstado()
        {
            ClsEstadoSeguridadContabilidad Estado = new ClsEstadoSeguridadContabilidad
            {
                HaySesion = ClsSesionContabilidad.HaySesionActiva,
                NombreUsuario = "Sin sesión",
                Roles = "Sin sesión",
                Mensaje = ""
            };
            if (!Estado.HaySesion) return Estado;

            Estado.NombreUsuario = string.IsNullOrWhiteSpace(ClsSesionContabilidad.NombreEmpleado)
                ? ClsSesionContabilidad.NombreUsuario : ClsSesionContabilidad.NombreEmpleado;
            Estado.Roles = ClsSesionContabilidad.ContabilidadFuncRolesComoTexto();
            if (string.IsNullOrWhiteSpace(Estado.Roles)) Estado.Roles = "Sin roles";

            try
            {
                ClsRepositorioSeguridadContabilidad Repositorio = new ClsRepositorioSeguridadContabilidad();
                DataTable Aplicaciones = Repositorio.ContabilidadFuncObtenerAplicaciones();
                // Resolver el registro antes de leer permisos evita usar un ID inventado.
                Dictionary<int, DataRow> Registros = new Dictionary<int, DataRow>();
                foreach (KeyValuePair<int, string> Aplicacion in _Aplicaciones)
                {
                    foreach (DataRow Fila in Aplicaciones.Rows)
                    {
                        if (ContabilidadFuncNormalizarNombre(Convert.ToString(Fila["nombreAplicacion"])) !=
                            ContabilidadFuncNormalizarNombre(Aplicacion.Value)) continue;
                        if (Registros.ContainsKey(Aplicacion.Key))
                            throw new InvalidOperationException("Existe más de un registro para " + Aplicacion.Value + ".");
                        Registros.Add(Aplicacion.Key, Fila);
                    }
                }
                if (Registros.Count == 0)
                {
                    Estado.Mensaje = "Las aplicaciones de Contabilidad aún no están registradas en Seguridad.";
                    return Estado;
                }
                foreach (KeyValuePair<int, DataRow> Registro in Registros)
                {
                    Estado.Modulos[Registro.Key] = Convert.ToInt32(Registro.Value["idModulo"]);
                    Estado.Aplicaciones[Registro.Key] = Convert.ToInt32(Registro.Value["idAplicacion"]);
                    Estado.Accesos[Registro.Key] = Repositorio.ContabilidadFuncTieneAcceso(
                        ClsSesionContabilidad.IdsRoles,
                        Convert.ToInt32(Registro.Value["idModulo"]),
                        Convert.ToInt32(Registro.Value["idAplicacion"]));
                }
                if (Registros.Count != _Aplicaciones.Count)
                    Estado.Mensaje = "Algunas aplicaciones de Contabilidad aún no están registradas en Seguridad.";
            }
            catch (Exception Excepcion)
            {
                // Un fallo de consulta nunca conserva permisos parciales o anteriores.
                foreach (int Codigo in _Aplicaciones.Keys) Estado.Accesos[Codigo] = false;
                Estado.Modulos.Clear();
                Estado.Aplicaciones.Clear();
                Estado.Mensaje = "No se pudieron consultar los permisos de Contabilidad. " + Excepcion.Message;
            }
            return Estado;
        }

        private static string ContabilidadFuncNormalizarNombre(string Nombre)
        {
            StringBuilder Resultado = new StringBuilder();
            foreach (char Caracter in Nombre.Normalize(NormalizationForm.FormD))
            {
                if (CharUnicodeInfo.GetUnicodeCategory(Caracter) != UnicodeCategory.NonSpacingMark &&
                    !char.IsWhiteSpace(Caracter)) Resultado.Append(char.ToUpperInvariant(Caracter));
            }
            return Resultado.ToString();
        }
    }
}
