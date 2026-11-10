using CapaControlador_Contabilidad;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CapaControlador_Polizas.CapaControlador_Polizas
{
    // Capa controlador de pólizas: recibe la póliza de la Vista y la reenvía al controlador de
    // integración de Contabilidad (ClsControladorIntegracionContabilidad, en la DLL).
    // Este proyecto es el único que referencia CapaControlador_Contabilidad.dll.
    // Ajuste el namespace al de su proyecto de controlador de pólizas.
    namespace CapaControlador_Polizas
    {
        public class ClsControladorPolizasCompras
        {
            // Código del módulo registrado en Contabilidad para Compras.
            private const string CodigoModulo = "MOD-COMPR";

            private ClsControladorIntegracionContabilidad _Integracion;

            public ClsControladorPolizasCompras()
            {
            }

            // Para pruebas. Es internal para que la Vista no necesite la referencia a la DLL.
            internal ClsControladorPolizasCompras(ClsControladorIntegracionContabilidad Integracion)
            {
                _Integracion = Integracion;
            }

            // Se crea al primer uso: si falta la DLL o la configuración de base de datos,
            // el error se captura y llega a la Vista como mensaje.
            private ClsControladorIntegracionContabilidad Integracion
            {
                get { return _Integracion ?? (_Integracion = new ClsControladorIntegracionContabilidad()); }
            }

            /// <summary>Cuentas que Contabilidad permite usar en pólizas de Compras.</summary>
            public List<ClsModeloCuentaPolizaCompras> ContabilidadFuncCuentasCompras()
            {
                try
                {
                    return Integracion.ContabilidadFuncCuentasPermitidas(CodigoModulo)
                        .Select(Cuenta => new ClsModeloCuentaPolizaCompras { Codigo = Cuenta.Codigo, Nombre = Cuenta.Nombre })
                        .ToList();
                }
                catch (Exception)
                {
                    return new List<ClsModeloCuentaPolizaCompras>();
                }
            }

            /// <summary>
            /// Documento de origen por defecto (hasta 30 caracteres). Si Compras tiene un número propio
            /// de compra u orden, la Vista puede enviar ese en su lugar.
            /// </summary>
            public string ContabilidadFuncGenerarDocumentoOrigen()
            {
                return "PC-" + DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
            }

            /// <summary>
            /// Envía la póliza a Contabilidad. Estado devuelto: "VALIDADA" (pendiente de batch) o "APLICADA" (ya en saldos).
            /// Reenviar el mismo DocumentoOrigen no duplica la póliza: Contabilidad devuelve la ya recibida.
            /// </summary>
            public bool ContabilidadFuncEnviarPoliza(ClsModeloPolizaCompras Poliza, out string CodigoPoliza, out string Estado, out string Mensaje)
            {
                CodigoPoliza = string.Empty;
                Estado = string.Empty;

                Mensaje = ContabilidadFuncValidarPoliza(Poliza);
                if (Mensaje != string.Empty) return false;

                try
                {
                    List<ClsModeloLineaIntegracionContabilidad> Lineas = Poliza.Lineas
                        .Select(Linea => new ClsModeloLineaIntegracionContabilidad
                        {
                            CodigoCuenta = (Linea.CodigoCuenta ?? string.Empty).Trim(),
                            Debe = Linea.Debe,
                            Haber = Linea.Haber,
                            Descripcion = Linea.Descripcion
                        })
                        .ToList();

                    return Integracion.ContabilidadFuncRecibirPoliza(CodigoModulo, Poliza.DocumentoOrigen.Trim(), Poliza.Fecha,
                        Poliza.Descripcion, Lineas, out CodigoPoliza, out Estado, out Mensaje);
                }
                catch (Exception Excepcion)
                {
                    Mensaje = "No se pudo comunicar con Contabilidad: " + Excepcion.Message;
                    return false;
                }
            }

            // Validaciones rápidas para avisar antes de llamar a Contabilidad, que vuelve a validar todo.
            private static string ContabilidadFuncValidarPoliza(ClsModeloPolizaCompras Poliza)
            {
                if (Poliza == null) return "No hay póliza para enviar.";

                string Documento = (Poliza.DocumentoOrigen ?? string.Empty).Trim();
                if (Documento.Length == 0 || Documento.Length > 30)
                    return "Indique el documento de origen (hasta 30 caracteres).";

                if (Poliza.Descripcion != null && Poliza.Descripcion.Trim().Length > 255)
                    return "La descripción admite hasta 255 caracteres.";

                List<ClsModeloLineaPolizaCompras> Lineas = Poliza.Lineas ?? new List<ClsModeloLineaPolizaCompras>();
                if (Lineas.Count < 2)
                    return "La póliza necesita al menos dos líneas.";

                foreach (ClsModeloLineaPolizaCompras Linea in Lineas)
                {
                    if (string.IsNullOrWhiteSpace(Linea.CodigoCuenta))
                        return "Todas las líneas deben tener una cuenta.";
                    if (Linea.Debe < 0 || Linea.Haber < 0)
                        return "Los importes no pueden ser negativos.";
                    if ((Linea.Debe > 0 && Linea.Haber > 0) || (Linea.Debe == 0 && Linea.Haber == 0))
                        return "La cuenta " + Linea.CodigoCuenta + " debe tener importe en Debe o en Haber, no en ambos ni en ninguno.";
                }

                decimal TotalDebe = Lineas.Sum(Linea => Linea.Debe);
                decimal TotalHaber = Lineas.Sum(Linea => Linea.Haber);
                if (TotalDebe != TotalHaber)
                    return "La póliza no cuadra: Debe " + TotalDebe.ToString("N2") + " y Haber " + TotalHaber.ToString("N2") + ".";

                return string.Empty;
            }
        }
    }
}