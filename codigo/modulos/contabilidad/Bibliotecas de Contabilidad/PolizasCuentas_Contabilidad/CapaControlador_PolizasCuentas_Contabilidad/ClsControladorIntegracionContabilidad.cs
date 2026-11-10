/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Oskar Cermeño
 * Carné : 0901-23-15379
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  Punto de entrada para que otros módulos (Bancos, Ventas, Compras,
 *  Recursos Humanos, Logística) envíen sus pólizas a Contabilidad y
 *  consulten las cuentas que pueden usar y el estado de lo enviado.
 * Reglas especificas:
 *  El módulo se identifica por su código (MOD-BANCO, MOD-VENTA...). Solo
 *  se aceptan cuentas de detalle asignadas al módulo, pólizas cuadradas
 *  y períodos abiertos. Un mismo documento de origen nunca se registra
 *  dos veces. La póliza entra VALIDADA y se aplica a saldos al instante
 *  si el módulo trabaja en LINEA.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Linq;
using CapaModelo_Contabilidad.Contratos;
using CapaModelo_Contabilidad.Entidades;
using CapaModelo_Contabilidad.Repositorios;

namespace CapaControlador_Contabilidad
{
    public class ClsControladorIntegracionContabilidad
    {
        private readonly ClsRepositorioIntegracionContabilidad _Integracion;
        private readonly IRepositorioPolizasContabilidad _Polizas;

        public ClsControladorIntegracionContabilidad() : this(new ClsRepositorioIntegracionContabilidad(), new ClsRepositorioPolizasContabilidad())
        {
        }

        public ClsControladorIntegracionContabilidad(ClsRepositorioIntegracionContabilidad Integracion, IRepositorioPolizasContabilidad Polizas)
        {
            _Integracion = Integracion;
            _Polizas = Polizas;
        }

        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
        public bool ContabilidadFuncRecibirPoliza(string CodigoModulo, string DocumentoOrigen, DateTime Fecha, string Descripcion,
            List<ClsModeloLineaIntegracionContabilidad> Lineas, out string CodigoPoliza, out string Estado, out string Mensaje)
        {
            return ContabilidadFuncRecibirPoliza(CodigoModulo, 0, DocumentoOrigen, Fecha, Descripcion, Lineas, out CodigoPoliza, out Estado, out Mensaje);
        }

        public bool ContabilidadFuncRecibirPoliza(string CodigoModulo, int IdPais, string DocumentoOrigen, DateTime Fecha, string Descripcion,
            List<ClsModeloLineaIntegracionContabilidad> Lineas, out string CodigoPoliza, out string Estado, out string Mensaje)
        {
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            CodigoPoliza = string.Empty;
            Estado = string.Empty;
            try
            {
                int IdModulo;
                string Metodo;
                if (!_Integracion.ContabilidadFuncObtenerModulo((CodigoModulo ?? string.Empty).Trim(), out IdModulo, out Metodo))
                {
                    Mensaje = "El módulo " + CodigoModulo + " no existe o está inactivo en Contabilidad.";
                    return false;
                }
                string Documento = (DocumentoOrigen ?? string.Empty).Trim();
                if (Documento.Length == 0 || Documento.Length > 30)
                {
                    Mensaje = "Indique el documento de origen (hasta 30 caracteres).";
                    return false;
                }
                if (ContabilidadFuncYaRecibida(IdModulo, Documento, out CodigoPoliza, out Estado, out Mensaje, out bool Aceptada)) return Aceptada;
                // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                if (IdPais != 0 && !_Polizas.ContabilidadFuncObtenerPaises().ContainsKey(IdPais))
                {
                    Mensaje = "El país " + IdPais + " no existe o está inactivo en Contabilidad.";
                    return false;
                }
                // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                if (Descripcion != null && Descripcion.Trim().Length > 255)
                {
                    Mensaje = "La descripción admite hasta 255 caracteres.";
                    return false;
                }

                // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                Mensaje = ContabilidadFuncValidarLineas(IdModulo, IdPais, CodigoModulo, Lineas, out List<ClsPolizaDetalleContabilidad> Detalle);
                // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                if (Mensaje != string.Empty) return false;

                ClsPolizaContabilidad Poliza = new ClsPolizaContabilidad
                {
                    // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                    Fecha = Fecha.Date, Estado = "VALIDADA", IdModulo = IdModulo, IdPais = IdPais, DocumentoOrigen = Documento,
                    // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                    Descripcion = string.IsNullOrWhiteSpace(Descripcion) ? null : Descripcion.Trim(), Detalle = Detalle
                };
                try
                {
                    _Polizas.ContabilidadFuncAgregar(Poliza);
                }
                catch (Exception Excepcion)
                {
                    if (ContabilidadFuncYaRecibida(IdModulo, Documento, out CodigoPoliza, out Estado, out Mensaje, out bool AceptadaAntes)) return AceptadaAntes;
                    Mensaje = Excepcion.Message;
                    return false;
                }

                CodigoPoliza = Poliza.CodigoPoliza;
                Estado = "VALIDADA";
                // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 02/11/2026
                if (Metodo != "LINEA")
                // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 02/11/2026
                {
                    Mensaje = "Póliza " + CodigoPoliza + " registrada como validada. Se aplicará a saldos en la próxima actualización batch.";
                    return true;
                }
                try
                {
                    _Polizas.ContabilidadFuncActualizarSaldos("LINEA", "RANGO", Poliza.Fecha, Poliza.Fecha, Poliza.IdPoliza,
                        ClsControladorSeguridadContabilidad.ContabilidadFuncUsuarioSesion());
                    Estado = "APLICADA";
                    Mensaje = "Póliza " + CodigoPoliza + " registrada y aplicada a saldos.";
                }
                catch (Exception Excepcion)
                {
                    Mensaje = "Póliza " + CodigoPoliza + " registrada como validada; los saldos se actualizarán en el próximo batch: " + Excepcion.Message;
                }
                return true;
            }
            catch (Exception Excepcion)
            {
                Mensaje = "No se pudo recibir la póliza: " + Excepcion.Message;
                return false;
            }
        }

        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
        public List<ClsModeloCuentaPolizaContabilidad> ContabilidadFuncCuentasPermitidas(string CodigoModulo)
        {
            return ContabilidadFuncCuentasPermitidas(CodigoModulo, 0);
        }

        public List<ClsModeloCuentaPolizaContabilidad> ContabilidadFuncCuentasPermitidas(string CodigoModulo, int IdPais)
        {
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            int IdModulo;
            string Metodo;
            if (!_Integracion.ContabilidadFuncObtenerModulo((CodigoModulo ?? string.Empty).Trim(), out IdModulo, out Metodo))
                return new List<ClsModeloCuentaPolizaContabilidad>();
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            return _Integracion.ContabilidadFuncObtenerCuentasPermitidas(IdModulo, IdPais)
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                .Select(Cuenta => new ClsModeloCuentaPolizaContabilidad { IdCuenta = Cuenta.IdCuenta, Codigo = Cuenta.Codigo, Nombre = Cuenta.Nombre })
                .ToList();
        }

        public bool ContabilidadFuncEstadoDocumento(string CodigoModulo, string DocumentoOrigen, out string CodigoPoliza, out string Estado)
        {
            CodigoPoliza = string.Empty;
            Estado = string.Empty;
            int IdModulo;
            string Metodo;
            if (!_Integracion.ContabilidadFuncObtenerModulo((CodigoModulo ?? string.Empty).Trim(), out IdModulo, out Metodo)) return false;
            ClsPolizaContabilidad Poliza = _Integracion.ContabilidadFuncObtenerPorDocumento(IdModulo, (DocumentoOrigen ?? string.Empty).Trim());
            if (Poliza == null) return false;
            CodigoPoliza = Poliza.CodigoPoliza;
            Estado = Poliza.Estado;
            return true;
        }

        private bool ContabilidadFuncYaRecibida(int IdModulo, string Documento, out string CodigoPoliza, out string Estado, out string Mensaje, out bool Aceptada)
        {
            CodigoPoliza = string.Empty;
            Estado = string.Empty;
            Mensaje = string.Empty;
            Aceptada = false;
            ClsPolizaContabilidad Existente = _Integracion.ContabilidadFuncObtenerPorDocumento(IdModulo, Documento);
            if (Existente == null) return false;
            CodigoPoliza = Existente.CodigoPoliza;
            Estado = Existente.Estado;
            Aceptada = Existente.Estado != "ANULADA";
            Mensaje = Aceptada
                ? "El documento " + Documento + " ya había sido recibido en la póliza " + CodigoPoliza + "; no se registró de nuevo."
                : "El documento " + Documento + " ya fue recibido en la póliza " + CodigoPoliza + ", que está anulada.";
            return true;
        }

        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
        private string ContabilidadFuncValidarLineas(int IdModulo, int IdPais, string CodigoModulo, List<ClsModeloLineaIntegracionContabilidad> Lineas,
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            out List<ClsPolizaDetalleContabilidad> Detalle)
        {
            Detalle = new List<ClsPolizaDetalleContabilidad>();
            if (Lineas == null || Lineas.Count < 2) return "La póliza necesita al menos dos líneas.";
            // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            Dictionary<string, ClsCuentaContabilidad> Permitidas = _Integracion.ContabilidadFuncObtenerCuentasPermitidas(IdModulo, IdPais)
                .ToDictionary(Cuenta => Cuenta.Codigo);
            // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
            foreach (ClsModeloLineaIntegracionContabilidad Linea in Lineas)
            {
                string Codigo = (Linea.CodigoCuenta ?? string.Empty).Trim();
                if (!Permitidas.ContainsKey(Codigo))
                    // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                    return "La cuenta " + Codigo + " no está asignada al módulo " + CodigoModulo + (IdPais == 0 ? string.Empty : " en ese país")
                        + " o no es una cuenta de detalle.";
                    // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
                if (Linea.Debe < 0m || Linea.Haber < 0m || (Linea.Debe > 0m) == (Linea.Haber > 0m))
                    return "La línea de la cuenta " + Codigo + " debe llevar un monto en el debe o en el haber, no en ambos.";
                if (Linea.Debe > ClsLimitesContabilidad.MaximoMonetario || Linea.Haber > ClsLimitesContabilidad.MaximoMonetario)
                    return "La línea de la cuenta " + Codigo + " excede el monto permitido.";
                Detalle.Add(new ClsPolizaDetalleContabilidad
                {
                    IdCuenta = Permitidas[Codigo].IdCuenta, Debe = Linea.Debe, Haber = Linea.Haber,
                    DescripcionDetalle = string.IsNullOrWhiteSpace(Linea.Descripcion) ? null : Linea.Descripcion.Trim()
                });
            }
            decimal Debe = Detalle.Sum(Linea => Linea.Debe);
            decimal Haber = Detalle.Sum(Linea => Linea.Haber);
            if (Debe != Haber) return "La póliza no cuadra: el debe suma " + Debe.ToString("N2") + " y el haber " + Haber.ToString("N2") + ".";
            return string.Empty;
        }
    }
}
