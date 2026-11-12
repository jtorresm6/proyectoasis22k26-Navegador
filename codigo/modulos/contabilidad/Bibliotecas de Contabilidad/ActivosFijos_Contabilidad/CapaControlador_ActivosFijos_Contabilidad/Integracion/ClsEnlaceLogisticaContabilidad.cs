/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  Busca la implementación de IReceptorActivoFijoContabilidad que
 *  publica Logística y le entrega los activos fijos pendientes.
 * Reglas especificas:
 *  Revisa los ensamblados ya cargados y los DLL de la carpeta de la
 *  aplicación cuyo nombre contenga "Logistica". Si no la encuentra o
 *  Logística rechaza el activo, este sigue PENDIENTE.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CapaControlador_Contabilidad
{
    public static class ClsEnlaceLogisticaContabilidad
    {
        public static IReceptorActivoFijoContabilidad ContabilidadFuncBuscarReceptor()
        {
            Type Contrato = typeof(IReceptorActivoFijoContabilidad);
            List<Assembly> Ensamblados = AppDomain.CurrentDomain.GetAssemblies().ToList();
            foreach (string Archivo in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*Logistica*.dll"))
            {
                try
                {
                    Ensamblados.Add(Assembly.LoadFrom(Archivo));
                }
                catch
                {
                }
            }
            foreach (Assembly Ensamblado in Ensamblados.Distinct())
            {
                Type[] Tipos;
                try
                {
                    Tipos = Ensamblado.GetTypes();
                }
                catch (ReflectionTypeLoadException Excepcion)
                {
                    Tipos = Excepcion.Types.Where(Tipo => Tipo != null).ToArray();
                }
                catch
                {
                    continue;
                }
                Type Receptor = Tipos.FirstOrDefault(Tipo => Contrato.IsAssignableFrom(Tipo) && Tipo.IsClass && !Tipo.IsAbstract
                    && Tipo.GetConstructor(Type.EmptyTypes) != null);
                if (Receptor != null) return (IReceptorActivoFijoContabilidad)Activator.CreateInstance(Receptor);
            }
            return null;
        }
    }
}
