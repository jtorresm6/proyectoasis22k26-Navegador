using CapaModelo_GeneracionPagos.Entidades;
using CapaModelo_GeneracionPagos.Repositorio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 03/11/2026
 * ================================================================
 * Propósito :
 *  Controlador del proceso Generación de Pagos (8012). Recibe las
 *  acciones de la vista, valida los datos antes de pasarlos al
 *  repositorio y le devuelve a la vista tablas o un resultado con
 *  el mensaje a mostrar, sin que la vista toque la capa modelo.
 * ================================================================
 */

namespace CapaControlador_GeneracionPagos
{
    public class ClsControladorGeneracionPagos
    {
        public const int DiasAvisoVencimiento = 3;

        private const string TodosLosOrigenes = "Todos";
        private const string TodasLasMonedas = "Todas";
        private const string MonedaBase = "GTQ";
        private const string IpPredeterminada = "127.0.0.1";
        private const string TextoSinRegistrar = "Sin registrar";

        private static readonly Dictionary<string, string> _OrigenesPorNombre = new Dictionary<string, string>
        {
            { "Compras y CxP", ClsValoresPago.OrigenCompra },
            { "Recursos Humanos", ClsValoresPago.OrigenPlanilla },
            { "Logística", ClsValoresPago.OrigenLogistica }
        };

        private readonly ClsRepositorioGeneracionPagos _Repositorio = new ClsRepositorioGeneracionPagos();

        public DataTable BancosMetConsultarObligaciones(string Origen, string Moneda, string Texto)
        {
            string TipoOrigen;
            if (!_OrigenesPorNombre.TryGetValue(Origen ?? "", out TipoOrigen))
                TipoOrigen = "";

            string CodigoMoneda = string.IsNullOrEmpty(Moneda) || Moneda == TodasLasMonedas ? "" : Moneda;
            DataTable Obligaciones = _Repositorio.BancosMetConsultarObligaciones(TipoOrigen, CodigoMoneda, (Texto ?? "").Trim());

            foreach (DataRow Fila in Obligaciones.Rows)
                Fila["Origen"] = BancosFuncNombreOrigen(Convert.ToString(Fila["TipoOrigen"]));
            return Obligaciones;
        }

        public DataTable BancosMetConsultarMonedas()
        {
            return _Repositorio.BancosMetConsultarMonedas();
        }

        // Tipos de pago activos del catálogo que una orden puede usar; son los mismos para compras, planillas y transporte
        public DataTable BancosMetObtenerTiposPago(string TipoOrigen)
        {
            DataTable Catalogo = _Repositorio.BancosMetConsultarTiposPago();
            DataTable Disponibles = Catalogo.Clone();

            foreach (DataRow Tipo in Catalogo.Rows)
            {
                string Medio = ClsRepositorioGeneracionPagos.BancosFuncMedioDeTipoPago(Convert.ToString(Tipo["nombreMetodoPago"]));
                if (Medio != null)
                    Disponibles.ImportRow(Tipo);
            }
            return Disponibles;
        }

        public DataTable BancosMetConsultarCuentas(long IdMoneda)
        {
            return _Repositorio.BancosMetConsultarCuentas(IdMoneda);
        }

        public DataTable BancosMetConsultarOrdenes()
        {
            return _Repositorio.BancosMetConsultarOrdenes();
        }

        public DataTable BancosMetObtenerDatosOrden(long IdOrden)
        {
            return _Repositorio.BancosMetObtenerDatosOrden(IdOrden);
        }

        public DataTable BancosMetObtenerAutorizadores()
        {
            return _Repositorio.BancosMetObtenerAutorizadores();
        }

        public DataTable BancosMetConsultarAprobaciones(long IdOrden)
        {
            return _Repositorio.BancosMetConsultarAprobaciones(IdOrden);
        }

        public DataTable BancosMetObtenerChequera(long IdCuenta)
        {
            return _Repositorio.BancosMetObtenerChequeraDisponible(IdCuenta);
        }

        // Devuelve 0 cuando no hay una política vigente para el monto y la moneda
        public int BancosFuncObtenerAprobacionesRequeridas(decimal Importe, long IdMoneda)
        {
            DataTable Politica = _Repositorio.BancosMetObtenerPolitica(Importe, IdMoneda);
            return Politica.Rows.Count == 0 ? 0 : Convert.ToInt32(Politica.Rows[0]["cantidadAprobacionesPoliticaAprobacion"]);
        }

        // Devuelve 0 cuando no hay tipo de cambio vigente
        public decimal BancosFuncObtenerTipoCambio(string CodigoMoneda)
        {
            if (CodigoMoneda == MonedaBase)
                return 1m;

            DataTable Cambio = _Repositorio.BancosFuncObtenerTipoCambio(CodigoMoneda);
            return Cambio.Rows.Count == 0 ? 0m : Convert.ToDecimal(Cambio.Rows[0]["valorTipoCambio"]);
        }

        public decimal BancosFuncCalcularEquivalente(decimal Monto, decimal TipoCambio)
        {
            return ClsRepositorioGeneracionPagos.BancosFuncCalcularEquivalente(Monto, TipoCambio);
        }

        public string BancosFuncFormatearImporte(decimal Importe, string CodigoMoneda)
        {
            string Simbolo = CodigoMoneda == MonedaBase ? "Q " : CodigoMoneda == "USD" ? "US$ " : CodigoMoneda + " ";
            return Simbolo + Importe.ToString("N2", CultureInfo.InvariantCulture);
        }

        // Obligaciones pendientes que ya vencieron, vencen hoy o vencen dentro de los días de aviso,
        // sin importar los filtros que tenga la vista
        public ClsResumenVencimientos BancosMetConsultarVencimientos()
        {
            DataTable Obligaciones = BancosMetConsultarObligaciones(TodosLosOrigenes, TodasLasMonedas, "");
            return BancosFuncResumirVencimientos(Obligaciones, DateTime.Today);
        }

        // Las obligaciones sin fecha de vencimiento no se avisan. El detalle sale ordenado de la más atrasada a la más próxima.
        public ClsResumenVencimientos BancosFuncResumirVencimientos(DataTable Obligaciones, DateTime Hoy)
        {
            var Avisos = new List<ClsAvisoVencimiento>();
            foreach (DataRow Fila in Obligaciones.Rows)
            {
                if (Fila["Vencimiento"] == DBNull.Value)
                    continue;

                DateTime Vencimiento = Convert.ToDateTime(Fila["Vencimiento"]).Date;
                int Dias = (Vencimiento - Hoy.Date).Days;
                if (Dias > DiasAvisoVencimiento)
                    continue;

                Avisos.Add(new ClsAvisoVencimiento(
                    Convert.ToString(Fila["Origen"]),
                    Convert.ToString(Fila["Documento"]),
                    Convert.ToString(Fila["Beneficiario"]),
                    BancosFuncFormatearImporte(Convert.ToDecimal(Fila["Pendiente"]), Convert.ToString(Fila["Moneda"])),
                    Vencimiento,
                    Dias));
            }

            Avisos.Sort((A, B) =>
            {
                int Orden = A.Dias.CompareTo(B.Dias);
                return Orden != 0 ? Orden : string.Compare(A.Documento, B.Documento, StringComparison.Ordinal);
            });
            return new ClsResumenVencimientos(Avisos);
        }

        public ClsResultadoOperacion BancosMetPrepararOrden(string TipoOrigen, long IdOrigen, long IdCuenta, long IdMoneda,
            decimal Monto, DateTime FechaPago, long IdTipoPago, string Usuario, int? IdUsuario)
        {
            return BancosFuncEjecutarOperacion(() =>
            {
                if (TipoOrigen != ClsValoresPago.OrigenCompra && TipoOrigen != ClsValoresPago.OrigenPlanilla && TipoOrigen != ClsValoresPago.OrigenLogistica)
                    return new ClsResultadoOperacion(false, "Seleccione una obligación de la lista.");

                string Medio = BancosFuncObtenerMedio(IdTipoPago);
                if (Medio == null)
                    return new ClsResultadoOperacion(false, "Seleccione el tipo de pago.");

                var Orden = new ClsOrdenPago
                {
                    ImporteOrdenPago = Monto,
                    MedioOrdenPago = Medio,
                    FechaProgramadaOrdenPago = FechaPago,
                    IdentificadorSolicitanteOrdenPago = Usuario,
                    IdCuentaBancaria = IdCuenta,
                    IdMoneda = IdMoneda,
                    IdOrdenCompraRecibida = TipoOrigen == ClsValoresPago.OrigenCompra ? (long?)IdOrigen : null,
                    IdDetallePlanillaRecibida = TipoOrigen == ClsValoresPago.OrigenPlanilla ? (long?)IdOrigen : null,
                    IdSolicitudPagoTransporte = TipoOrigen == ClsValoresPago.OrigenLogistica ? (long?)IdOrigen : null,
                    IdUsuario = IdUsuario,
                    Ip = BancosFuncObtenerIp()
                };

                long IdOrden = _Repositorio.BancosMetPrepararOrden(Orden);
                return new ClsResultadoOperacion(true, "La orden de pago se preparó correctamente.", IdOrden.ToString());
            });
        }

        public ClsResultadoOperacion BancosMetAutorizarOrden(long IdOrden, string Autorizador, int? IdUsuario)
        {
            return BancosFuncRegistrarDecision(IdOrden, Autorizador, IdUsuario, ClsValoresPago.DecisionAprobada);
        }

        public ClsResultadoOperacion BancosMetRechazarOrden(long IdOrden, string Autorizador, int? IdUsuario)
        {
            return BancosFuncRegistrarDecision(IdOrden, Autorizador, IdUsuario, ClsValoresPago.DecisionRechazada);
        }

        public ClsResultadoOperacion BancosMetEmitirCheque(long IdOrden, string Usuario, int? IdUsuario)
        {
            return BancosFuncEjecutarOperacion(() =>
            {
                DateTime Fecha = DateTime.Now;
                string Numero = _Repositorio.BancosMetEmitirCheque(IdOrden, Usuario, BancosFuncGenerarReferencia(Usuario, Fecha),
                    Fecha, IdUsuario, BancosFuncObtenerIp());
                return new ClsResultadoOperacion(true, "Se generó el cheque " + Numero + ".", Numero);
            });
        }

        public ClsResultadoOperacion BancosMetEnviarTransferencia(long IdOrden, string Usuario, int? IdUsuario)
        {
            return BancosFuncEjecutarOperacion(() =>
            {
                _Repositorio.BancosMetEnviarTransferencia(IdOrden, BancosFuncGenerarReferencia(Usuario, DateTime.Now),
                    IdUsuario, BancosFuncObtenerIp());
                return new ClsResultadoOperacion(true, "La instrucción de pago quedó registrada. El pago se confirma en la pestaña Registrar.");
            });
        }

        public ClsResultadoOperacion BancosMetRegistrarMovimiento(long IdOrden, int? IdUsuario)
        {
            return BancosFuncEjecutarOperacion(() =>
            {
                long IdMovimiento = _Repositorio.BancosMetRegistrarMovimiento(IdOrden, IdUsuario, BancosFuncObtenerIp());
                return new ClsResultadoOperacion(true, "El movimiento bancario y la póliza en borrador quedaron registrados.", IdMovimiento.ToString());
            });
        }

        public ClsDatosRegistro BancosMetObtenerRegistro(long IdOrden)
        {
            var Datos = new ClsDatosRegistro { Poliza = BancosFuncCrearTablaPoliza() };

            DataTable Movimiento = _Repositorio.BancosMetObtenerMovimiento(IdOrden);
            if (Movimiento.Rows.Count > 0)
            {
                BancosFuncCargarMovimientoRegistrado(Datos, Movimiento.Rows[0]);
                return Datos;
            }

            BancosFuncCargarVistaPrevia(Datos, IdOrden);
            return Datos;
        }

        private void BancosFuncCargarMovimientoRegistrado(ClsDatosRegistro Datos, DataRow Movimiento)
        {
            Datos.Registrado = true;
            Datos.Referencia = Convert.ToString(Movimiento["referenciaMovimientoBancario"]);
            Datos.Importe = Convert.ToDecimal(Movimiento["importeMovimientoBancario"]);
            Datos.TipoCambio = Convert.ToDecimal(Movimiento["tipoCambioAplicadoMovimientoBancario"]);
            Datos.ImporteGTQ = Convert.ToDecimal(Movimiento["importeGTQMovimientoBancario"]);

            if (Movimiento["idPolizaBancaria"] == DBNull.Value)
                return;

            Datos.EstadoPoliza = Convert.ToString(Movimiento["estadoPolizaBancaria"]);
            DataTable Lineas = _Repositorio.BancosMetConsultarPoliza(Convert.ToInt64(Movimiento["idPolizaBancaria"]));
            foreach (DataRow Linea in Lineas.Rows)
            {
                Datos.Poliza.Rows.Add(Convert.ToString(Linea["codigoCuentaContableDetallePolizaBancaria"]), Convert.ToString(Linea["nombreCuenta"]),
                    Convert.ToDecimal(Linea["debeDetallePolizaBancaria"]), Convert.ToDecimal(Linea["haberDetallePolizaBancaria"]));
            }
        }

        private void BancosFuncCargarVistaPrevia(ClsDatosRegistro Datos, long IdOrden)
        {
            DataTable Orden = _Repositorio.BancosMetObtenerDatosOrden(IdOrden);
            if (Orden.Rows.Count == 0)
                return;

            DataRow DatosOrden = Orden.Rows[0];
            Datos.Importe = Convert.ToDecimal(DatosOrden["importeOrdenPago"]);
            Datos.TipoCambio = BancosFuncObtenerTipoCambio(Convert.ToString(DatosOrden["codigoMoneda"]));
            Datos.ImporteGTQ = BancosFuncCalcularEquivalente(Datos.Importe, Datos.TipoCambio);
            Datos.EstadoPoliza = TextoSinRegistrar;

            if (Datos.TipoCambio == 0m)
            {
                Datos.Advertencia = "No hay tipo de cambio vigente para la moneda de la orden.";
                return;
            }

            DataTable Configuracion = _Repositorio.BancosMetObtenerConfiguracionPoliza(Convert.ToInt64(DatosOrden["Fk_CuentaBancaria_Codigo"]));
            if (Configuracion.Rows.Count == 0)
            {
                Datos.Advertencia = "Falta la configuración de póliza de la cuenta para un tipo de transacción de egreso.";
                return;
            }

            DataRow Cuentas = Configuracion.Rows[0];
            Datos.Poliza.Rows.Add(Convert.ToString(Cuentas["codigoCuentaContableContrapartidaConfiguracionPoliza"]),
                Convert.ToString(Cuentas["nombreCuentaContrapartida"]), Datos.ImporteGTQ, 0m);
            Datos.Poliza.Rows.Add(Convert.ToString(Cuentas["codigoCuentaContableBancoConfiguracionPoliza"]),
                Convert.ToString(Cuentas["nombreCuentaBanco"]), 0m, Datos.ImporteGTQ);
        }

        private string BancosFuncObtenerMedio(long IdTipoPago)
        {
            foreach (DataRow Tipo in _Repositorio.BancosMetConsultarTiposPago().Rows)
            {
                if (Convert.ToInt64(Tipo["idMetodoPago"]) == IdTipoPago)
                    return ClsRepositorioGeneracionPagos.BancosFuncMedioDeTipoPago(Convert.ToString(Tipo["nombreMetodoPago"]));
            }
            return null;
        }

        private ClsResultadoOperacion BancosFuncRegistrarDecision(long IdOrden, string Autorizador, int? IdUsuario, string Decision)
        {
            return BancosFuncEjecutarOperacion(() =>
            {
                if (string.IsNullOrWhiteSpace(Autorizador))
                    return new ClsResultadoOperacion(false, "Seleccione el autorizador.");

                DateTime Fecha = DateTime.Now;
                string Estado = _Repositorio.BancosMetRegistrarAprobacion(IdOrden, Autorizador, Decision, null,
                    BancosFuncGenerarReferencia(Autorizador, Fecha), Fecha, IdUsuario, BancosFuncObtenerIp());
                return new ClsResultadoOperacion(true, BancosFuncMensajeDecision(Estado), Estado);
            });
        }

        private ClsResultadoOperacion BancosFuncEjecutarOperacion(Func<ClsResultadoOperacion> Operacion)
        {
            try
            {
                return Operacion();
            }
            catch (InvalidOperationException Ex)
            {
                return new ClsResultadoOperacion(false, Ex.Message);
            }
            catch (Exception Ex)
            {
                return new ClsResultadoOperacion(false, "No se pudo completar la operación: " + Ex.Message);
            }
        }

        private string BancosFuncMensajeDecision(string Estado)
        {
            switch (Estado)
            {
                case ClsValoresPago.EstadoAutorizada:
                    return "La orden quedó autorizada.";
                case ClsValoresPago.EstadoRechazada:
                    return "La orden fue rechazada.";
                default:
                    return "Aprobación registrada. La orden aún necesita más aprobaciones.";
            }
        }

        private string BancosFuncNombreOrigen(string TipoOrigen)
        {
            foreach (KeyValuePair<string, string> Origen in _OrigenesPorNombre)
            {
                if (Origen.Value == TipoOrigen)
                    return Origen.Key;
            }
            return TipoOrigen;
        }

        private DataTable BancosFuncCrearTablaPoliza()
        {
            var Tabla = new DataTable();
            Tabla.Columns.Add("Codigo", typeof(string));
            Tabla.Columns.Add("Nombre", typeof(string));
            Tabla.Columns.Add("Debe", typeof(decimal));
            Tabla.Columns.Add("Haber", typeof(decimal));
            return Tabla;
        }

        private string BancosFuncGenerarReferencia(string Usuario, DateTime Fecha)
        {
            return "SESION-" + Usuario + "-" + Fecha.ToString("yyyyMMddHHmmss");
        }

        private string BancosFuncObtenerIp()
        {
            try
            {
                foreach (IPAddress Direccion in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    if (Direccion.AddressFamily == AddressFamily.InterNetwork)
                        return Direccion.ToString();
                }
            }
            catch (SocketException)
            {
            }
            return IpPredeterminada;
        }
    }
}
