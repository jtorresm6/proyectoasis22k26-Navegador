// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using CapaControlador_MovimientosClientes.Validaciones;
using CapaModelo_MovimientosClientes;
using System.Data;
using System.Linq;
using System;
using VentasValidaciones;

namespace CapaControlador_MovimientosClientes.Servicios
{
    internal sealed class ClsMovimientosServicio
    {
        // RecibosMovimientos.
        public string VentasFuncRegistrarRecibo(int Cliente, int Moneda, int Usuario, string Numero, DateTime Fecha, decimal Monto, decimal TipoCambio, string Forma, string Referencia, string Observaciones, DataTable Aplicaciones)
        {
            ClsValidacionesVentas.VentasMetExigir(Monto > 0 && Monto <= ClsValidacionesVentas.ImporteMaximo && Monto == ClsValidacionesVentas.VentasFuncRedondear(Monto) && TipoCambio > 0 && TipoCambio <= 999999999999.999999m && TipoCambio == Math.Round(TipoCambio, 6), "Monto o tipo de cambio invalido.");
            ClsValidacionesVentas.VentasMetExigir(new[] { "EFECTIVO", "TRANSFERENCIA", "DEPOSITO", "CHEQUE", "TARJETA" }.Contains(Forma), "Seleccione la forma de pago.");
            Referencia = ClsValidacionesVentas.VentasFuncTexto(Referencia, 100, "referencia", Forma != "EFECTIVO");
            Numero = ClsValidacionesVentas.VentasFuncTexto(Numero, 30, "numero de recibo", false);
            Observaciones = ClsValidacionesVentas.VentasFuncTexto(Observaciones, 500, "observaciones", false);
            ClsValidacionesVentas.VentasMetExigir(Aplicaciones != null && Aplicaciones.Rows.Count > 0, "Seleccione las facturas a pagar.");
            var Filas = Aplicaciones.AsEnumerable().OrderBy(Registro => Convert.ToInt32(Registro["idFactura"])).ToList();
            ClsValidacionesVentas.VentasMetExigir(Filas.Select(Registro => Convert.ToInt32(Registro["idFactura"])).Distinct().Count() == Filas.Count, "Una factura se repite en las aplicaciones.");
            ClsValidacionesVentas.VentasMetExigir(Filas.Sum(Registro => Convert.ToDecimal(Registro["Importe"])) == Monto, "Aplique el monto completo del recibo a las facturas; no se admiten anticipos en esta etapa.");
            using (var Repositorio = new ClsMovimientosRepositorio())
            {
                Repositorio.VentasMetIniciar();
                Repositorio.VentasMetUsuario(Usuario);
                Repositorio.VentasMetPeriodoAbierto(Moneda, Fecha);
                ClsValidacionesVentas.VentasMetExigir(Convert.ToInt32(Repositorio.Recibos.VentasFuncRegistrarReciboConsultarCliente(Cliente)) == 1, "Seleccione un cliente activo.");
                if (Numero.Length == 0)
                    Numero = Repositorio.VentasFuncNumero("REC");
                // Validar y bloquear todas las facturas antes de registrar el encabezado.
                foreach (DataRow Fila in Filas)
                {
                    int Factura = Convert.ToInt32(Fila["idFactura"]);
                    decimal Importe = Convert.ToDecimal(Fila["Importe"]);
                    DataTable Datos = Repositorio.Recibos.VentasFuncRegistrarReciboConsultarFactura(Factura, Cliente, Moneda);
                    ClsValidacionesVentas.VentasMetExigir(Datos.Rows.Count == 1, "Una factura no pertenece al cliente/moneda o no esta emitida.");
                    ClsValidacionesVentas.VentasMetExigir(Fecha.Date >= Convert.ToDateTime(Datos.Rows[0][0]).Date, "El pago es anterior a una factura.");
                    ClsValidacionesVentas.VentasMetExigir(Importe > 0 && Importe == ClsValidacionesVentas.VentasFuncRedondear(Importe), "Cada aplicacion debe ser positiva y tener hasta dos decimales.");
                    ClsValidacionesVentas.VentasMetExigir(Importe <= VentasFuncDisponible(Repositorio, Factura, Fecha), "El saldo de una factura cambio o el importe lo supera. Consulte nuevamente.");
                }

                ClsValidacionesVentas.VentasMetExigir(Repositorio.Recibos.VentasFuncRegistrarReciboInsertarPago(Numero, Cliente, Moneda, Fecha.Date, Monto, Referencia, Observaciones, Usuario, Forma) == 1,
                    "La forma de pago no está configurada en el catálogo de Ventas.");
                int Pago = Repositorio.VentasFuncObtenerId();
                foreach (DataRow Fila in Filas)
                {
                    int Factura = Convert.ToInt32(Fila["idFactura"]);
                    decimal Importe = Convert.ToDecimal(Fila["Importe"]);
                    Repositorio.Recibos.VentasFuncRegistrarReciboInsertarAplicacionpago(Pago, Factura, Fecha.Date, Importe, Usuario);
                    VentasMetMovimiento(Repositorio, Factura, Fecha, "PAGO", "Recibo " + Numero, 0, Importe, Usuario, Repositorio.VentasFuncObtenerId());
                }

                Repositorio.VentasMetConfirmar();
                return Numero;
            }
        }

        public DataTable VentasFuncListarRecibos(int Cliente, int Moneda)
        {
            using (var Repositorio = new ClsMovimientosRepositorio())
                return Repositorio.Consultas.VentasFuncListarRecibosConsultarPago(Cliente, Cliente, Moneda, Moneda);
        }

        public DataTable VentasFuncConsultarRecibos(string Recibo, string Deposito)
        {
            using (var Repositorio = new ClsMovimientosRepositorio())
                return Repositorio.Consultas.VentasFuncGeneralesConsultarRecibosConsultarPago(Recibo ?? "", Recibo ?? "", Deposito ?? "", Deposito ?? "");
        }

        public DataTable VentasFuncAplicaciones(int Pago)
        {
            using (var Repositorio = new ClsMovimientosRepositorio())
                return Repositorio.Consultas.VentasFuncAplicacionesConsultarAplicacionpago(Pago);
        }

        public void VentasMetAnularRecibo(int Pago, DateTime Fecha, int Usuario, string Motivo)
        {
            Motivo = ClsValidacionesVentas.VentasFuncTexto(Motivo, 180, "motivo de anulacion");
            using (var Repositorio = new ClsMovimientosRepositorio())
            {
                Repositorio.VentasMetIniciar();
                Repositorio.VentasMetUsuario(Usuario);
                object Moneda = Repositorio.Recibos.VentasFuncAnularReciboConsultarPago(Pago);
                ClsValidacionesVentas.VentasMetExigir(Moneda != null, "No existe el recibo.");
                Repositorio.VentasMetPeriodoAbierto(Convert.ToInt32(Moneda), Fecha);
                DataTable RecibosConsultados = Repositorio.Recibos.VentasFuncAnularReciboConsultarPago2(Pago);
                ClsValidacionesVentas.VentasMetExigir((string)RecibosConsultados.Rows[0]["estadoPago"] == "CONFIRMADO" && Fecha.Date >= Convert.ToDateTime(RecibosConsultados.Rows[0]["fechaPago"]).Date, "El recibo ya esta anulado o la fecha es anterior al pago.");
                ClsValidacionesVentas.VentasMetExigir(Convert.ToInt32(Repositorio.Recibos.VentasFuncAnularReciboConsultarDepositoventasdetalle(Pago)) == 0, "El recibo pertenece a un deposito; coordine su reverso con Bancos.");
                DataTable Movimientos = Repositorio.Recibos.VentasFuncAnularReciboConsultarMovimientocxc(Pago);
                foreach (DataRow Movimiento in Movimientos.Rows)
                    VentasMetMovimiento(Repositorio, Convert.ToInt32(Movimiento["idFactura"]), Fecha, "REVERSO", "Anulacion: " + Motivo, (decimal)Movimiento["abono"], (decimal)Movimiento["cargo"], Usuario, null, null, Convert.ToInt32(Movimiento["idMovimiento"]));
                Repositorio.Recibos.VentasFuncAnularReciboActualizarAplicacionpago(Pago);
                Repositorio.Recibos.VentasFuncAnularReciboActualizarPago(Pago);
                Repositorio.VentasMetAuditar("RECIBO", Pago, "ANULAR", Motivo, Usuario);
                Repositorio.VentasMetConfirmar();
            }
        }

        public void VentasMetModificarRecibo(int Pago, int Usuario, string Observaciones)
        {
            Observaciones = ClsValidacionesVentas.VentasFuncTexto(Observaciones, 500, "observaciones", false);
            using (var Repositorio = new ClsMovimientosRepositorio())
            {
                Repositorio.VentasMetIniciar();
                Repositorio.VentasMetUsuario(Usuario);
                DataTable Origen = Repositorio.Recibos.VentasFuncModificarReciboConsultarPago(Pago);
                ClsValidacionesVentas.VentasMetExigir(Origen.Rows.Count == 1, "No existe el recibo.");
                Repositorio.VentasMetPeriodoAbierto(Convert.ToInt32(Origen.Rows[0]["idMoneda"]), Convert.ToDateTime(Origen.Rows[0]["fechaPago"]));
                DataTable Actual = Repositorio.Recibos.VentasFuncModificarReciboConsultarPago2(Pago);
                ClsValidacionesVentas.VentasMetExigir((string)Actual.Rows[0][0] == "CONFIRMADO", "El recibo fue anulado.");
                ClsValidacionesVentas.VentasMetExigir(Convert.ToInt32(Repositorio.Recibos.VentasFuncAnularReciboConsultarDepositoventasdetalle(Pago)) == 0, "El recibo ya forma parte de un deposito; coordine el cambio con Bancos.");
                Repositorio.Recibos.VentasFuncModificarReciboActualizarPago(Observaciones, Pago);
                Repositorio.VentasMetAuditar("RECIBO", Pago, "MODIFICAR", Observaciones, Usuario);
                Repositorio.VentasMetConfirmar();
            }
        }

        public DataSet VentasFuncLeerRecibo(int Id)
        {
            using (var Repositorio = new ClsMovimientosRepositorio())
            {
                var Datos = new DataSet();
                Datos.Tables.Add(Repositorio.Consultas.VentasFuncLecturaConsultarRecibo(Id));
                ClsValidacionesVentas.VentasMetExigir(Datos.Tables[0].Rows.Count == 1, "No se encontro el recibo.");
                Datos.Tables.Add(Repositorio.Consultas.VentasFuncLecturaConsultarAplicacionesRecibo(Id));
                return Datos;
            }
        }

        // OperacionMovimientos.
        private static void VentasMetMovimiento(ClsMovimientosRepositorio Repositorio, int Factura, DateTime Fecha, string Tipo, string Descripcion, decimal Cargo, decimal Abono, int Usuario, int? Aplicacion = null, int? Nota = null, int? Origen = null)
        {
            Repositorio.VentasFuncMovimientoInsertarMovimientocxc(Factura, Fecha.Date, Tipo, Descripcion, Cargo, Abono, Usuario, Aplicacion, Nota, Origen);
        }

        private static decimal VentasFuncSaldo(ClsMovimientosRepositorio Repositorio, int Factura, DateTime? Fecha)
        {
            return ClsValidacionesVentas.VentasFuncDecimal(Fecha.HasValue ? Repositorio.VentasFuncSaldoConsultarMovimientocxc(Factura, Fecha.Value.Date) : Repositorio.VentasFuncSaldoConsultarMovimientocxc2(Factura));
        }

        private static decimal VentasFuncDisponible(ClsMovimientosRepositorio Repositorio, int Factura, DateTime Fecha)
        {
            decimal AlDia = VentasFuncSaldo(Repositorio, Factura, Fecha);
            // Un pago retroactivo tampoco puede dejar negativos los saldos de fechas posteriores.
            object Minimo = Repositorio.VentasFuncDisponibleConsultarMovimientocxc(Factura, Fecha.Date);
            return Minimo == null || Minimo == DBNull.Value ? AlDia : Math.Min(AlDia, Convert.ToDecimal(Minimo));
        }

        // DocumentosMovimientos.
        public string VentasFuncRegistrarDocumento(int Cliente, int Moneda, int Usuario, int Establecimiento, string Numero, int Tipo, DateTime Fecha, DateTime Vencimiento, decimal Importe, string Referencia, string Motivo)
        {
            ClsValidacionesVentas.VentasMetExigir(Tipo >= 0 && Tipo <= 4, "Seleccione el tipo de documento.");
            ClsValidacionesVentas.VentasMetExigir(Importe > 0 && Importe <= ClsValidacionesVentas.ImporteMaximo && Importe == ClsValidacionesVentas.VentasFuncRedondear(Importe), "El importe debe ser positivo, estar dentro del rango y tener hasta dos decimales.");
            Numero = ClsValidacionesVentas.VentasFuncTexto(Numero, 30, "numero de documento", false);
            Motivo = ClsValidacionesVentas.VentasFuncTexto(Motivo, 255, "descripcion / motivo");
            using (var Repositorio = new ClsMovimientosRepositorio())
            {
                Repositorio.VentasMetIniciar();
                Repositorio.VentasMetUsuario(Usuario);
                Repositorio.VentasMetPeriodoAbierto(Moneda, Fecha);
                DataTable Clientes = Repositorio.VentasFuncRegistrarDocumentoConsultarCliente(Cliente);
                ClsValidacionesVentas.VentasMetExigir(Clientes.Rows.Count == 1, "Seleccione un cliente activo.");
                if (Numero.Length == 0)
                    Numero = Repositorio.VentasFuncNumero(Tipo == 0 ? "CXC" : "NOT");
                int Factura;
                if (Tipo == 0)
                {
                    ClsValidacionesVentas.VentasMetExigir(Vencimiento.Date >= Fecha.Date, "El vencimiento es anterior a la emision.");
                    ClsValidacionesVentas.VentasMetExigir(Convert.ToInt32(Repositorio.VentasFuncRegistrarDocumentoConsultarEstablecimientoventas(Establecimiento)) == 1, "Seleccione un establecimiento emisor activo.");
                    int VersionCliente = Repositorio.VentasFuncCrearVersionCliente(Cliente);
                    Repositorio.VentasFuncRegistrarDocumentoInsertarFactura(Numero, Establecimiento, Moneda, Fecha.Date, Vencimiento.Date,
                        Vencimiento.Date == Fecha.Date ? "CONTADO" : "CREDITO", Motivo, Usuario, VersionCliente);
                    Factura = Repositorio.VentasFuncObtenerId();
                    VentasMetMovimiento(Repositorio, Factura, Fecha, "FACTURA", Motivo, Importe, 0, Usuario);
                }
                else
                {
                    Referencia = ClsValidacionesVentas.VentasFuncTexto(Referencia, 80, "factura relacionada (serie-numero)");
                    DataTable Facturas = Repositorio.VentasFuncRegistrarDocumentoConsultarFactura(Cliente, Moneda, Referencia);
                    ClsValidacionesVentas.VentasMetExigir(Facturas.Rows.Count == 1, "La referencia debe identificar una sola factura emitida del cliente y moneda seleccionados.");
                    Factura = Convert.ToInt32(Facturas.Rows[0]["idFactura"]);
                    ClsValidacionesVentas.VentasMetExigir(Fecha.Date >= Convert.ToDateTime(Facturas.Rows[0]["fechaEmision"]).Date, "El ajuste es anterior a la factura.");
                    bool Abono = Tipo == 1 || Tipo == 4;
                    if (Abono)
                        ClsValidacionesVentas.VentasMetExigir(Importe <= VentasFuncDisponible(Repositorio, Factura, Fecha), "El abono supera el saldo disponible de la factura.");
                    Repositorio.VentasFuncRegistrarDocumentoInsertarNotacxc(Numero, Factura, Abono ? "CREDITO" : "DEBITO", Fecha.Date, Importe, Motivo, Usuario);
                    int Nota = Repositorio.VentasFuncObtenerId();
                    string[] Tipos =
                    {
                        "FACTURA",
                        "NOTA_CREDITO",
                        "NOTA_DEBITO",
                        "AJUSTE_POSITIVO",
                        "AJUSTE_NEGATIVO"
                    };
                    VentasMetMovimiento(Repositorio, Factura, Fecha, Tipos[Tipo], Motivo, Abono ? 0 : Importe, Abono ? Importe : 0, Usuario, null, Nota);
                }

                Repositorio.VentasMetConfirmar();
                return Numero;
            }
        }

        public DataTable VentasFuncListarDocumentos(int Cliente, int Moneda)
        {
            using (var Repositorio = new ClsMovimientosRepositorio())
                return Repositorio.Consultas.VentasFuncListarDocumentosConsultarFactura(Cliente, Cliente, Moneda, Moneda, Cliente, Cliente, Moneda, Moneda);
        }

        public DataTable VentasFuncDocumento(int Cliente, int Moneda, int Establecimiento, string Numero, bool Factura)
        {
            using (var Repositorio = new ClsMovimientosRepositorio())
                return Factura ? Repositorio.Consultas.VentasFuncDocumentoConsultarFactura(Cliente, Moneda, Establecimiento, Numero) : Repositorio.Consultas.VentasFuncDocumentoConsultarNotacxc(Cliente, Moneda, Numero);
        }

        private static DataRow VentasFuncBloquearDocumento(ClsMovimientosRepositorio Repositorio, int Id, string Clase)
        {
            ClsValidacionesVentas.VentasMetExigir(Clase == "FACTURA" || Clase == "NOTA", "Seleccione un documento interno de CxC.");
            DataTable Origen = Clase == "FACTURA" ? Repositorio.VentasFuncBloquearDocumentoConsultarFactura(Id) : Repositorio.VentasFuncBloquearDocumentoConsultarNotacxc(Id);
            ClsValidacionesVentas.VentasMetExigir(Origen.Rows.Count == 1, "El documento no existe.");
            Repositorio.VentasMetPeriodoAbierto(Convert.ToInt32(Origen.Rows[0]["idMoneda"]), Convert.ToDateTime(Origen.Rows[0]["Fecha"]));
            DataTable Factura = Repositorio.VentasFuncBloquearDocumentoConsultarFactura2(Origen.Rows[0]["idFactura"]);
            ClsValidacionesVentas.VentasMetExigir((string)Factura.Rows[0]["estadoFactura"] == "EMITIDA", "La factura no esta emitida o ya fue anulada.");
            if (Clase == "FACTURA")
                return Factura.Rows[0];
            DataTable Nota = Repositorio.VentasFuncBloquearDocumentoConsultarNotacxc2(Id);
            ClsValidacionesVentas.VentasMetExigir((string)Nota.Rows[0]["estadoNota"] == "APLICADA", "La nota ya fue anulada.");
            return Nota.Rows[0];
        }

        public void VentasMetModificarDocumento(int Id, string Clase, int Usuario, string Descripcion, DateTime Vencimiento)
        {
            Descripcion = ClsValidacionesVentas.VentasFuncTexto(Descripcion, 255, "descripcion");
            using (var Repositorio = new ClsMovimientosRepositorio())
            {
                Repositorio.VentasMetIniciar();
                Repositorio.VentasMetUsuario(Usuario);
                DataRow Fila = VentasFuncBloquearDocumento(Repositorio, Id, Clase);
                if (Clase == "FACTURA")
                {
                    ClsValidacionesVentas.VentasMetExigir(Vencimiento.Date >= Convert.ToDateTime(Fila["fechaEmision"]).Date, "El vencimiento no puede ser anterior a la emision.");
                    Repositorio.VentasFuncModificarDocumentoActualizarFactura(Descripcion, Vencimiento.Date, Id);
                }
                else
                    Repositorio.VentasFuncModificarDocumentoActualizarNotacxc(Descripcion, Id);
                Repositorio.VentasMetAuditar(Clase + "_CXC", Id, "MODIFICAR", "Descripcion: " + Descripcion + "; vencimiento: " + Vencimiento.ToString("yyyy-MM-dd"), Usuario);
                Repositorio.VentasMetConfirmar();
            }
        }

        public void VentasMetAnularDocumento(int Id, string Clase, int Usuario, string Motivo)
        {
            Motivo = ClsValidacionesVentas.VentasFuncTexto(Motivo, 180, "motivo de anulacion");
            using (var Repositorio = new ClsMovimientosRepositorio())
            {
                Repositorio.VentasMetIniciar();
                Repositorio.VentasMetUsuario(Usuario);
                DataRow Fila = VentasFuncBloquearDocumento(Repositorio, Id, Clase);
                int Factura = Convert.ToInt32(Fila["idFactura"]);
                DataRow Fila2 = Repositorio.VentasFuncAnularDocumentoConsultarFactura(Factura).Rows[0];
                Repositorio.VentasMetPeriodoAbierto(Convert.ToInt32(Fila2["idMoneda"]), DateTime.Today);
                if (Clase == "FACTURA")
                {
                    ClsValidacionesVentas.VentasMetExigir(Convert.ToInt32(Repositorio.VentasFuncAnularDocumentoConsultarAplicacionpago(Factura)) == 0 && Convert.ToInt32(Repositorio.VentasFuncAnularDocumentoConsultarNotacxc(Factura)) == 0, "Anule primero los recibos y las notas aplicados a esta factura.");
                }

                DataTable Libro = Clase == "FACTURA" ? Repositorio.VentasFuncAnularDocumentoConsultarMovimientocxc(Factura) : Repositorio.VentasFuncAnularDocumentoConsultarMovimientocxc2(Id);
                ClsValidacionesVentas.VentasMetExigir(Libro.Rows.Count == 1, "El movimiento original no se puede identificar de forma unica.");
                DataRow Movimiento = Libro.Rows[0];
                ClsValidacionesVentas.VentasMetExigir(Convert.ToInt32(Repositorio.VentasFuncAnularDocumentoConsultarMovimientocxc3(Movimiento["idMovimiento"])) == 0, "El documento ya tiene un reverso.");
                if ((decimal)Movimiento["cargo"] > 0)
                    ClsValidacionesVentas.VentasMetExigir(VentasFuncDisponible(Repositorio, Factura, DateTime.Today) >= (decimal)Movimiento["cargo"], "La anulacion dejaria un saldo negativo. Revise pagos y ajustes posteriores.");
                VentasMetMovimiento(Repositorio, Factura, DateTime.Today, "REVERSO", "Anulacion: " + Motivo, (decimal)Movimiento["abono"], (decimal)Movimiento["cargo"], Usuario, null, null, Convert.ToInt32(Movimiento["idMovimiento"]));
                if (Clase == "FACTURA")
                    Repositorio.VentasFuncAnularDocumentoActualizarFactura(Id);
                else
                    Repositorio.VentasFuncAnularDocumentoActualizarNotacxc(Id);
                Repositorio.VentasMetAuditar(Clase + "_CXC", Id, "ANULAR", Motivo, Usuario);
                Repositorio.VentasMetConfirmar();
            }
        }

        public DataRow VentasFuncLeerDocumento(int Id, string Clase)
        {
            ClsValidacionesVentas.VentasMetExigir(Id > 0 && (Clase == "FACTURA" || Clase == "NOTA"), "Seleccione un documento de CxC.");
            using (var Repositorio = new ClsMovimientosRepositorio())
            {
                DataTable Datos = (Clase == "FACTURA" ? Repositorio.Consultas.VentasFuncLecturaConsultarFactura(Id) : Repositorio.Consultas.VentasFuncLecturaConsultarNota(Id));
                ClsValidacionesVentas.VentasMetExigir(Datos.Rows.Count == 1, "No se encontro el documento interno.");
                return Datos.Rows[0];
            }
        }

        // DepositosMovimientos.
        public DataSet VentasFuncDeposito(string Numero)
        {
            using (var Repositorio = new ClsMovimientosRepositorio())
            {
                var ConjuntoDatos = new DataSet();
                ConjuntoDatos.Tables.Add(Repositorio.Consultas.VentasFuncDepositoConsultarDepositoventas(Numero));
                ClsValidacionesVentas.VentasMetExigir(ConjuntoDatos.Tables[0].Rows.Count == 1, "Consulte el codigo de un deposito preparado.");
                ConjuntoDatos.Tables.Add(Repositorio.Consultas.VentasFuncDepositoConsultarDepositoventasdetalle(ConjuntoDatos.Tables[0].Rows[0]["idDeposito"]));
                return ConjuntoDatos;
            }
        }

        public DataTable VentasFuncRecibosDisponibles(int Moneda)
        {
            using (var Repositorio = new ClsMovimientosRepositorio())
                return Repositorio.Consultas.VentasFuncRecibosDisponiblesConsultarPago(Moneda);
        }

        public string VentasFuncPrepararDeposito(int Moneda, int Usuario, DateTime Fecha, string Numero, string Empresa, string Banco, string Cuenta, string Concepto, string Referencia, DataTable Recibos)
        {
            Empresa = ClsValidacionesVentas.VentasFuncTexto(Empresa, 100, "empresa suministrada por Bancos");
            Banco = ClsValidacionesVentas.VentasFuncTexto(Banco, 100, "banco destino");
            Cuenta = ClsValidacionesVentas.VentasFuncTexto(Cuenta, 100, "cuenta de la empresa");
            Concepto = ClsValidacionesVentas.VentasFuncTexto(Concepto, 100, "concepto bancario");
            Referencia = ClsValidacionesVentas.VentasFuncTexto(Referencia, 100, "referencia", false);
            Numero = ClsValidacionesVentas.VentasFuncTexto(Numero, 30, "codigo de deposito", false);
            ClsValidacionesVentas.VentasMetExigir(Recibos != null && Recibos.Rows.Count > 0, "Seleccione recibos para preparar el deposito.");
            var Filas = Recibos.AsEnumerable().OrderBy(Registro => Convert.ToInt32(Registro["idPago"])).ToList();
            ClsValidacionesVentas.VentasMetExigir(Filas.Select(Registro => Convert.ToInt32(Registro["idPago"])).Distinct().Count() == Filas.Count, "No repita un recibo.");
            using (var Repositorio = new ClsMovimientosRepositorio())
            {
                Repositorio.VentasMetIniciar();
                Repositorio.VentasMetUsuario(Usuario);
                Repositorio.VentasMetPeriodoAbierto(Moneda, Fecha);
                decimal Total = 0;
                foreach (DataRow Fila in Filas)
                {
                    DataTable RecibosConsultados = Repositorio.Recibos.VentasFuncPrepararDepositoConsultarPago(Fila["idPago"], Moneda);
                    ClsValidacionesVentas.VentasMetExigir(RecibosConsultados.Rows.Count == 1 && Fecha.Date >= Convert.ToDateTime(RecibosConsultados.Rows[0]["fechaPago"]).Date, "Recibo no confirmado, moneda o fecha incorrecta.");
                    decimal Importe = Convert.ToDecimal(Fila["Importe"]);
                    decimal Usado = ClsValidacionesVentas.VentasFuncDecimal(Repositorio.Recibos.VentasFuncPrepararDepositoConsultarDepositoventasdetalle(Fila["idPago"]));
                    ClsValidacionesVentas.VentasMetExigir(Importe > 0 && Importe == ClsValidacionesVentas.VentasFuncRedondear(Importe) && Importe <= (decimal)RecibosConsultados.Rows[0]["montoPago"] - Usado, "Importe de deposito invalido o superior al disponible.");
                    Total += Importe;
                }

                if (Numero.Length == 0)
                    Numero = Repositorio.VentasFuncNumero("DEP");
                ClsValidacionesVentas.VentasMetExigir(Total <= ClsValidacionesVentas.ImporteMaximo, "El deposito supera la capacidad del documento.");
                ClsValidacionesVentas.VentasMetExigir(Repositorio.Recibos.VentasFuncPrepararDepositoInsertarDepositoventas(
                    Numero, Fecha.Date, Referencia, Moneda, Total, Usuario, Cuenta, Cuenta, Banco, Banco, Banco, Concepto, Concepto, Concepto, Moneda) == 1,
                    "La cuenta bancaria o el tipo de transacción no está configurado para esta moneda.");
                int Id = Repositorio.VentasFuncObtenerId();
                foreach (DataRow Fila in Filas)
                    Repositorio.Recibos.VentasFuncPrepararDepositoInsertarDepositoventasdetalle(Id, Fila["idPago"], Fila["Importe"]);
                Repositorio.VentasMetConfirmar();
                return Numero;
            }
        }

        // CuentaMovimientos.
        public DataTable VentasFuncCatalogo(string Nombre)
        {
            using (var Repositorio = new ClsMovimientosRepositorio())
                return Repositorio.VentasFuncCatalogo(Nombre);
        }

        public DataSet VentasFuncEstadoCuenta(int Cliente, int Moneda, DateTime Desde, DateTime Hasta)
        {
            ClsValidacionesVentas.VentasMetExigir(Cliente > 0 && Moneda > 0, "Seleccione cliente y moneda.");
            ClsValidacionesVentas.VentasMetExigir(Desde.Date <= Hasta.Date, "La fecha inicial supera la fecha final.");
            using (var Repositorio = new ClsMovimientosRepositorio())
            {
                var Datos = new DataSet();
                Datos.Tables.Add(Repositorio.Consultas.VentasFuncEstadoCuentaConsultarMovimientocxc(Desde.Date, Desde.Date, Desde.Date, Cliente, Moneda, Hasta.Date));
                DataTable Libro = Repositorio.Consultas.VentasFuncEstadoCuentaConsultarMovimientocxc2(Cliente, Moneda, Desde.Date, Hasta.Date);
                Libro.Columns.Add("Saldo", typeof(decimal));
                decimal Saldo = ClsValidacionesVentas.VentasFuncDecimal(Datos.Tables[0].Rows[0]["Inicial"]);
                foreach (DataRow Fila in Libro.Rows)
                {
                    Saldo += (decimal)Fila["Cargo"] - (decimal)Fila["Abono"];
                    Fila["Saldo"] = Saldo;
                }

                Datos.Tables.Add(Libro);
                return Datos;
            }
        }

        public DataTable VentasFuncPendientes(int Cliente, int Moneda)
        {
            using (var Repositorio = new ClsMovimientosRepositorio())
                return Repositorio.Consultas.VentasFuncPendientesConsultarFactura(Cliente, Moneda);
        }

        public DataRow VentasFuncLeerMovimiento(int Id)
        {
            using (var Repositorio = new ClsMovimientosRepositorio())
            {
                DataTable Datos = Repositorio.Consultas.VentasFuncLecturaConsultarMovimiento(Id);
                ClsValidacionesVentas.VentasMetExigir(Datos.Rows.Count == 1, "El documento pertenece a Facturas o no admite cambios desde CxC.");
                return Datos.Rows[0];
            }
        }
    }
}
