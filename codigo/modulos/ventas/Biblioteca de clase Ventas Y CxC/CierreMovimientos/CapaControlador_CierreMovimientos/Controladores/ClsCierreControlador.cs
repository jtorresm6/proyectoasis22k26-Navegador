/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 09/11/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Coordinar el cierre de movimientos y el enlace contable.
 */
// Integración contable: Natali Sofía Montenegro Portillo | 0901-23-10017 | 09/11/2026
// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
#if CONTABILIDAD
using CapaControlador_Contabilidad;
#endif

namespace CapaControlador_CierreMovimientos
{
    public sealed class ClsCierreControlador
    {
#if CONTABILIDAD
        private const string ModuloContable = "MOD-VENTA";
#endif

        // Natali Sofía Montenegro Portillo | 0901-23-10017 | Equivalencias limitadas a cierres V16-DEMO.
        private static readonly Dictionary<string, string> CuentasDemo = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "1103", "1.01.04.001" }, // Clientes Nacionales
            { "1101", "1.01.01.001" }, // Caja General
            { "4101", "4.01.01.001" }, // Ventas nacionales
            { "4102", "4.01.01.001" }  // Nota de crédito: reversión de ventas nacionales
        };

        public string VentasFuncRegistrarContabilidad(string Documento, int Moneda, DateTime Fecha, DataTable Partida)
        {
            return VentasFuncRegistrarContabilidad(Documento, Moneda, Fecha, Partida, 0);
        }

        // Natali Sofía Montenegro Portillo | 0901-23-10017 | El país elegido determina las cuentas permitidas y la póliza recibida.
        public string VentasFuncRegistrarContabilidad(string Documento, int Moneda, DateTime Fecha, DataTable Partida, int Pais)
        {
#if CONTABILIDAD
            if (Moneda <= 0)
                throw new InvalidOperationException("Seleccione la moneda del cierre antes de enviar la póliza.");
            if (Pais <= 0)
                throw new InvalidOperationException("Seleccione el país de Contabilidad antes de enviar la póliza.");
            if (Partida == null || Partida.Rows.Count == 0)
                throw new InvalidOperationException("La póliza no tiene líneas para enviar.");
            var Contabilidad = new ClsControladorIntegracionContabilidad();
            var Permitidas = new HashSet<string>(Contabilidad.ContabilidadFuncCuentasPermitidas(ModuloContable, Pais)
                .Select(Cuenta => Cuenta.Codigo), StringComparer.OrdinalIgnoreCase);
            if (Permitidas.Count == 0)
                throw new InvalidOperationException("Contabilidad no tiene cuentas permitidas para Ventas.");
            bool EsDemo = _Servicio.VentasFuncEsDocumentoDemo(Documento);
            var Lineas = new List<ClsModeloLineaIntegracionContabilidad>();
            foreach (DataRow Fila in Partida.Rows)
            {
                string Cuenta = Convert.ToString(Fila["Cuenta"]).Trim();
                string Equivalente;
                if (EsDemo && CuentasDemo.TryGetValue(Cuenta, out Equivalente))
                    Cuenta = Equivalente;
                if (!Permitidas.Contains(Cuenta))
                    throw new InvalidOperationException("La cuenta " + Cuenta + " no está permitida para Ventas en Contabilidad. Revise la configuración de cuentas del movimiento.");
                Lineas.Add(new ClsModeloLineaIntegracionContabilidad
                {
                    CodigoCuenta = Cuenta,
                    Debe = Convert.ToDecimal(Fila["Debe"]),
                    Haber = Convert.ToDecimal(Fila["Haber"]),
                    Descripcion = Convert.ToString(Fila["Descripcion"])
                });
            }
            string Codigo, Estado, Mensaje;
            if (!Contabilidad.ContabilidadFuncRecibirPoliza(ModuloContable, Pais, Documento, Fecha,
                "Cierre de movimientos CxC " + Documento, Lineas, out Codigo, out Estado, out Mensaje))
                throw new InvalidOperationException(Mensaje);
            if (string.IsNullOrWhiteSpace(Codigo))
                throw new InvalidOperationException("Contabilidad aceptó la póliza sin devolver un código.");
            return Codigo;
#else
            // Natali Montenegro: el cierre se puede revisar y confirmar sin la DLL del equipo de Contabilidad.
            throw new InvalidOperationException("El enlace con Contabilidad está desactivado en esta compilación de prueba.");
#endif
        }

        private readonly ClsCierreServicio _Servicio = new ClsCierreServicio();
        public DataTable VentasFuncPaisesContables()
        {
            DataTable Paises = new DataTable();
            Paises.Columns.Add("Id", typeof(int));
            Paises.Columns.Add("Nombre", typeof(string));
#if CONTABILIDAD
            foreach (var Pais in new ClsControladorPolizasContabilidad().ContabilidadFuncObtenerPaises())
                Paises.Rows.Add(Pais.Key, Pais.Value);
#endif
            return Paises;
        }
        public bool VentasFuncEsDocumentoDemo(string Documento)
        {
            return _Servicio.VentasFuncEsDocumentoDemo(Documento);
        }

        public string VentasFuncEstadoCierre(int IdCierre)
        {
            return _Servicio.VentasFuncEstadoCierre(IdCierre);
        }
        public DateTime VentasFuncSiguientePeriodo(int Moneda, DateTime PeriodoElegido)
        {
            return _Servicio.VentasFuncSiguientePeriodo(Moneda, PeriodoElegido);
        }
        public DataTable VentasFuncListar(int Moneda)
        {
            return _Servicio.VentasFuncListar(Moneda);
        }

        public string VentasFuncGuardarBorrador(int Id, int Moneda, int Usuario, string Tipo, DateTime Desde, DateTime Hasta, string Observaciones)
        {
            return _Servicio.VentasFuncGuardarBorrador(Id, Moneda, Usuario, Tipo, Desde, Hasta, Observaciones);
        }

        public void VentasMetEliminarBorrador(int Id, int Usuario, string Motivo)
        {
            _Servicio.VentasMetEliminarBorrador(Id, Usuario, Motivo);
        }

        public DataTable VentasFuncCatalogo(string Nombre)
        {
            return _Servicio.VentasFuncCatalogo(Nombre);
        }

        public DataTable VentasFuncRevisar(int Moneda, string Tipo, DateTime Desde, DateTime Hasta)
        {
            return _Servicio.VentasFuncRevisar(Moneda, Tipo, Desde, Hasta);
        }

        public DataTable VentasFuncEstadoPeriodo(int Moneda, string Tipo, DateTime Desde, DateTime Hasta)
        {
            return _Servicio.VentasFuncEstadoPeriodo(Moneda, Tipo, Desde, Hasta);
        }

        public string VentasFuncConfirmar(int Moneda, int Usuario, string Tipo, DateTime Desde, DateTime Hasta, bool Revisado)
        {
            return _Servicio.VentasFuncConfirmar(Moneda, Usuario, Tipo, Desde, Hasta, Revisado);
        }

        public DataTable VentasFuncHistorial(int Moneda, int Cliente, string Periodo)
        {
            return _Servicio.VentasFuncHistorial(Moneda, Cliente, Periodo);
        }

        public DataTable VentasFuncPartida(int IdCierre)
        {
            DataTable Partida = _Servicio.VentasFuncPartida(IdCierre);
            if (_Servicio.VentasFuncEsCierreDemo(IdCierre))
                foreach (DataRow Fila in Partida.Rows)
                {
                    string Equivalente;
                    if (CuentasDemo.TryGetValue(Convert.ToString(Fila["Cuenta"]).Trim(), out Equivalente))
                        Fila["Cuenta"] = Equivalente;
                }
            return Partida;
        }

        public DataTable VentasFuncEstadoTraslado(int IdCierre)
        {
            return _Servicio.VentasFuncEstadoTraslado(IdCierre);
        }

        public string VentasFuncTrasladar(int IdCierre, int Usuario, Func<string, int, DateTime, DataTable, string> Registrar)
        {
            if (Registrar == null)
                throw new InvalidOperationException("Configure el enlace de Contabilidad.");
            DataSet Datos = _Servicio.VentasFuncPrepararTraslado(IdCierre, Usuario);
            DataRow Encabezado = Datos.Tables[0].Rows[0];
            if ((string)Encabezado["estado"] == "CONFIRMADO")
                return (string)Encabezado["referenciaContable"];
            // Sin transaccion abierta durante la llamada externa. El receptor debe deduplicar por clave.
            string Referencia = Registrar((string)Encabezado["claveIntegracion"], Convert.ToInt32(Encabezado["idMoneda"]), Convert.ToDateTime(Encabezado["fechaHasta"]), Datos.Tables[1]);
            if (string.IsNullOrWhiteSpace(Referencia))
                throw new InvalidOperationException("Contabilidad no devolvio una confirmacion. La partida sigue pendiente con la misma clave para reintentar.");
            _Servicio.VentasMetConfirmarTraslado(IdCierre, Usuario, Referencia);
            return Referencia;
        }

        public void VentasMetValidarCaptura(bool Condicion, string Mensaje)
        {
            VentasValidaciones.ClsValidacionesVentas.VentasMetExigir(Condicion, Mensaje);
        }

        public decimal VentasFuncImporteCelda(object Valor)
        {
            return VentasValidaciones.ClsValidacionesVentas.VentasFuncImporteCelda(Valor);
        }

        public DateTime[] VentasFuncPeriodo(int Anio, int Mes, bool Anual)
        {
            return Validaciones.ClsReglasCierre.VentasFuncPeriodo(Anio, Mes, Anual);
        }
    }
}
