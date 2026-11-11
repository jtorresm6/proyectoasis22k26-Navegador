/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Oskar Cermeño
 * Carné : 0901-23-15379
 * Fecha : 29/10/2026
 * ==================================================================
 * Propósito :
 *  Ejemplo de cómo Compras se conecta con Contabilidad: consulta las
 *  cuentas que tiene asignadas y envía la póliza de una compra.
 * Reglas especificas:
 *  Los datos van fijos para el ejemplo. La compra de materia prima al
 *  crédito carga Inventario de Materias Primas e IVA Acreditable y
 *  abona Proveedores Nacionales. El 1 es el país (Guatemala).
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using CapaControlador_Contabilidad;

namespace EjemploIntegracion
{
    public class ClsEjemploIntegracionContabilidad
    {
        private readonly ClsControladorIntegracionContabilidad _Contabilidad = new ClsControladorIntegracionContabilidad();

        public List<ClsModeloCuentaPolizaContabilidad> ContabilidadFuncCuentasCompras()
        {
            return _Contabilidad.ContabilidadFuncCuentasPermitidas("MOD-COMPR", 1);
        }

        public bool ContabilidadFuncEnviarPolizaCompra(out string CodigoPoliza, out string Estado, out string Mensaje)
        {
            List<ClsModeloLineaIntegracionContabilidad> Lineas = new List<ClsModeloLineaIntegracionContabilidad>
            {
                new ClsModeloLineaIntegracionContabilidad { CodigoCuenta = "1.01.06.001", Debe = 10000.00m, Descripcion = "Materia prima" },
                new ClsModeloLineaIntegracionContabilidad { CodigoCuenta = "1.01.07.001", Debe = 1200.00m, Descripcion = "IVA de la compra" },
                new ClsModeloLineaIntegracionContabilidad { CodigoCuenta = "2.01.01.001", Haber = 11200.00m, Descripcion = "Por pagar a Carnes del Valle" }
            };
            return _Contabilidad.ContabilidadFuncRecibirPoliza("MOD-COMPR", 1, "OC-2026-0045", new DateTime(2026, 10, 27),
                "Compra de materia prima al crédito, orden OC-2026-0045", Lineas, out CodigoPoliza, out Estado, out Mensaje);
        }
    }
}
