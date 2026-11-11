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
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CapaControlador_CierreMovimientos
{
    public sealed class ClsCierreControlador
    {
        private const string ModuloContable = "MOD-VENTA";
        private const string DllContable = "CapaControlador_PolizasCuentas_Contabilidad.dll";

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
            if (Moneda <= 0)
                throw new InvalidOperationException("Seleccione la moneda del cierre antes de enviar la póliza.");
            if (Pais <= 0)
                throw new InvalidOperationException("Seleccione el país de Contabilidad antes de enviar la póliza.");
            if (Partida == null || Partida.Rows.Count == 0)
                throw new InvalidOperationException("La póliza no tiene líneas para enviar.");
            // Natali Montenegro: equivale a ContabilidadFuncCuentasPermitidas("MOD-VENTA", Pais)
            // y ContabilidadFuncRecibirPoliza("MOD-VENTA", Pais, Documento, Fecha, Descripcion, Lineas, out Codigo, out Estado, out Mensaje).
            // Se invocan desde la DLL instalada con Ventas para no exigir una referencia en el proyecto.
            object Contabilidad = VentasFuncCrearControladorContable("ClsControladorIntegracionContabilidad");
            MethodInfo ConsultarCuentas = Contabilidad.GetType().GetMethod("ContabilidadFuncCuentasPermitidas", new[] { typeof(string), typeof(int) });
            if (ConsultarCuentas == null)
                throw new InvalidOperationException("La DLL de Contabilidad no ofrece ContabilidadFuncCuentasPermitidas.");
            var Permitidas = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (object Cuenta in (IEnumerable)VentasFuncInvocar(ConsultarCuentas, Contabilidad, new object[] { ModuloContable, Pais }))
                Permitidas.Add(Convert.ToString(Cuenta.GetType().GetProperty("Codigo").GetValue(Cuenta, null)));
            if (Permitidas.Count == 0)
                throw new InvalidOperationException("Contabilidad no tiene cuentas permitidas para Ventas.");
            bool EsDemo = _Servicio.VentasFuncEsDocumentoDemo(Documento);
            Type TipoLinea = Contabilidad.GetType().Assembly.GetType("CapaControlador_Contabilidad.ClsModeloLineaIntegracionContabilidad", true);
            IList Lineas = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(TipoLinea));
            foreach (DataRow Fila in Partida.Rows)
            {
                string Cuenta = Convert.ToString(Fila["Cuenta"]).Trim();
                string Equivalente;
                if (EsDemo && CuentasDemo.TryGetValue(Cuenta, out Equivalente))
                    Cuenta = Equivalente;
                if (!Permitidas.Contains(Cuenta))
                    throw new InvalidOperationException("La cuenta " + Cuenta + " no está permitida para Ventas en Contabilidad. Revise la configuración de cuentas del movimiento.");
                object Linea = Activator.CreateInstance(TipoLinea);
                TipoLinea.GetProperty("CodigoCuenta").SetValue(Linea, Cuenta, null);
                TipoLinea.GetProperty("Debe").SetValue(Linea, Convert.ToDecimal(Fila["Debe"]), null);
                TipoLinea.GetProperty("Haber").SetValue(Linea, Convert.ToDecimal(Fila["Haber"]), null);
                TipoLinea.GetProperty("Descripcion").SetValue(Linea, Convert.ToString(Fila["Descripcion"]), null);
                Lineas.Add(Linea);
            }
            MethodInfo RecibirPoliza = Contabilidad.GetType().GetMethods().SingleOrDefault(Metodo =>
                Metodo.Name == "ContabilidadFuncRecibirPoliza" && Metodo.GetParameters().Length == 9 &&
                Metodo.GetParameters()[1].ParameterType == typeof(int));
            if (RecibirPoliza == null)
                throw new InvalidOperationException("La DLL de Contabilidad no ofrece ContabilidadFuncRecibirPoliza con país.");
            object[] Argumentos = { ModuloContable, Pais, Documento, Fecha,
                "Cierre de movimientos CxC " + Documento, Lineas, null, null, null };
            bool Aceptada = (bool)VentasFuncInvocar(RecibirPoliza, Contabilidad, Argumentos);
            string Codigo = Convert.ToString(Argumentos[6]);
            if (!Aceptada)
                throw new InvalidOperationException(Convert.ToString(Argumentos[8]));
            if (string.IsNullOrWhiteSpace(Codigo))
                throw new InvalidOperationException("Contabilidad aceptó la póliza sin devolver un código.");
            return Codigo;
        }

        private static object VentasFuncCrearControladorContable(string Nombre)
        {
            string Ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DllContable);
            if (!File.Exists(Ruta))
                throw new InvalidOperationException("Falta " + DllContable + " junto al ejecutable que inicia Ventas. Compile Contabilidad y copie su DLL y dependencias antes de activar el envío.");
            Assembly Biblioteca = Assembly.LoadFrom(Ruta);
            Type Controlador = Biblioteca.GetType("CapaControlador_Contabilidad." + Nombre, true);
            return Activator.CreateInstance(Controlador);
        }

        private static object VentasFuncInvocar(MethodInfo Metodo, object Controlador, object[] Argumentos)
        {
            try { return Metodo.Invoke(Controlador, Argumentos); }
            catch (TargetInvocationException Error) { throw Error.InnerException ?? Error; }
        }

        private readonly ClsCierreServicio _Servicio = new ClsCierreServicio();
        public string VentasFuncRutaAyuda(Assembly BibliotecaVista)
        {
            if (BibliotecaVista == null) throw new ArgumentNullException(nameof(BibliotecaVista));
            // Natali Montenegro: la ayuda acompaña al ejecutable o al repositorio, sin ruta absoluta fija.
            foreach (string Origen in new[] { AppDomain.CurrentDomain.BaseDirectory, Path.GetDirectoryName(BibliotecaVista.Location) })
                for (DirectoryInfo Carpeta = new DirectoryInfo(Origen); Carpeta != null; Carpeta = Carpeta.Parent)
                {
                    string Manual = Path.Combine(Carpeta.FullName, "ayuda", "modulos", "ventas", "CierreMovimientos.chm");
                    if (File.Exists(Manual)) return Manual;
                }
            throw new FileNotFoundException("No se encontró ayuda\\modulos\\ventas\\CierreMovimientos.chm desde la carpeta del programa.");
        }

        public DataTable VentasFuncPaisesContables()
        {
            using (var Repositorio = new CapaModelo_CierreMovimientos.ClsCierreRepositorio())
                return Repositorio.VentasFuncPaisesContables();
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
