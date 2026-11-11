// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using CapaControlador_CierreMovimientos.Validaciones;
using CapaModelo_CierreMovimientos;
using System.Data;
using System.Linq;
using System;
using VentasValidaciones;

namespace CapaControlador_CierreMovimientos.Servicios
{
    internal sealed class ClsCierreServicio
    {
        // TrasladoCierres.
        public DataTable VentasFuncEstadoTraslado(int IdCierre)
        {
            using (var Repositorio = new ClsCierreRepositorio())
                return Repositorio.Contabilidad.VentasFuncEstadoTrasladoConsultarTrasladocxc(IdCierre);
        }

        public DataSet VentasFuncPrepararTraslado(int IdCierre, int Usuario)
        {
            DataTable Partida = VentasFuncPartida(IdCierre);
            ClsValidacionesVentas.VentasMetExigir(Partida.Rows.Count > 0 && !Partida.AsEnumerable().Any(Registro => ((string)Registro["Cuenta"]).StartsWith("PENDIENTE", StringComparison.Ordinal)), "Contabilidad debe configurar las cuentas de cada tipo de movimiento antes del traslado.");
            ClsValidacionesVentas.VentasMetExigir(Partida.AsEnumerable().Sum(Registro => (decimal)Registro["Debe"]) == Partida.AsEnumerable().Sum(Registro => (decimal)Registro["Haber"]), "La partida no esta balanceada.");
            using (var Repositorio = new ClsCierreRepositorio())
            {
                Repositorio.VentasMetIniciar();
                Repositorio.VentasMetUsuario(Usuario);
                DataTable Cierre = Repositorio.Contabilidad.VentasFuncPrepararTrasladoConsultarCierremovimientos(IdCierre);
                ClsValidacionesVentas.VentasMetExigir(Cierre.Rows.Count == 1, "El cierre debe estar confirmado antes del traslado.");
                if (Convert.ToInt32(Repositorio.Contabilidad.VentasFuncPrepararTrasladoConsultarTrasladocxc(IdCierre)) == 0)
                {
                    Repositorio.Contabilidad.VentasFuncPrepararTrasladoInsertarTrasladocxc(IdCierre, "VENTAS-CXC-CIERRE-" + IdCierre, Usuario);
                    int NumeroLinea = 0;
                    foreach (DataRow Fila in Partida.Rows)
                        Repositorio.Contabilidad.VentasFuncPrepararTrasladoInsertarTrasladocxcdetalle(IdCierre, ++NumeroLinea, Fila["Cuenta"], Fila["Descripcion"], Fila["Debe"], Fila["Haber"]);
                }

                var ConjuntoDatos = new DataSet();
                ConjuntoDatos.Tables.Add(Repositorio.Contabilidad.VentasFuncPrepararTrasladoConsultarTrasladocxc2(IdCierre));
                ConjuntoDatos.Tables.Add(Repositorio.Contabilidad.VentasFuncPrepararTrasladoConsultarTrasladocxcdetalle(IdCierre));
                Repositorio.VentasMetConfirmar();
                return ConjuntoDatos;
            }
        }

        public void VentasMetConfirmarTraslado(int IdCierre, int Usuario, string Referencia)
        {
            Referencia = ClsValidacionesVentas.VentasFuncTexto(Referencia, 100, "referencia devuelta por Contabilidad");
            using (var Repositorio = new ClsCierreRepositorio())
            {
                Repositorio.VentasMetIniciar();
                Repositorio.VentasMetUsuario(Usuario);
                DataTable TrasladoConsultado = Repositorio.Contabilidad.VentasFuncConfirmarTrasladoConsultarTrasladocxc(IdCierre);
                ClsValidacionesVentas.VentasMetExigir(TrasladoConsultado.Rows.Count == 1, "La partida no fue preparada.");
                if ((string)TrasladoConsultado.Rows[0]["estado"] == "CONFIRMADO")
                    ClsValidacionesVentas.VentasMetExigir((string)TrasladoConsultado.Rows[0]["referenciaContable"] == Referencia, "Contabilidad devolvio referencias diferentes para la misma clave.");
                else
                    Repositorio.Contabilidad.VentasFuncConfirmarTrasladoActualizarTrasladocxc(Referencia, IdCierre);
                Repositorio.VentasMetConfirmar();
            }
        }

        // RegistroCierres.
        public DataTable VentasFuncListar(int Moneda)
        {
            using (var Repositorio = new ClsCierreRepositorio())
                return Repositorio.Consultas.VentasFuncListarConsultarCierremovimientos(Moneda, Moneda);
        }

        public string VentasFuncGuardarBorrador(int Id, int Moneda, int Usuario, string Tipo, DateTime Desde, DateTime Hasta, string Observaciones)
        {
            Tipo = (Tipo ?? "").Trim().ToUpperInvariant();
            ClsReglasCierre.VentasMetValidarPeriodo(Tipo, Desde.Date, Hasta.Date);
            Observaciones = ClsValidacionesVentas.VentasFuncTexto(Observaciones, 500, "observaciones", false);
            using (var Repositorio = new ClsCierreRepositorio())
            {
                Repositorio.VentasMetIniciar();
                Repositorio.VentasMetUsuario(Usuario);
                Repositorio.VentasMetPeriodoAbierto(Moneda, Desde);
                if (Id > 0)
                {
                    DataTable Actual = Repositorio.VentasFuncGuardarBorradorConsultarCierremovimientos(Id);
                    ClsValidacionesVentas.VentasMetExigir(Actual.Rows.Count == 1 && (string)Actual.Rows[0]["estadoCierre"] == "BORRADOR" && Convert.ToInt32(Actual.Rows[0]["idMoneda"]) == Moneda, "Solo se modifica un borrador de la moneda seleccionada. Los cierres confirmados conservan su historico.");
                }

                ClsValidacionesVentas.VentasMetExigir(Convert.ToInt32(Repositorio.VentasFuncGuardarBorradorConsultarCierremovimientos2(Moneda, Tipo, Desde.Date, Hasta.Date, Id)) == 0, "Ya existe un borrador o cierre confirmado para ese periodo y moneda.");
                string Numero;
                if (Id == 0)
                {
                    Numero = Repositorio.VentasFuncNumero("CIE");
                    Repositorio.VentasFuncGuardarBorradorInsertarCierremovimientos(Numero, Desde.Date, Hasta.Date, Hasta.Date, Moneda, Tipo, Observaciones, Usuario);
                    Id = Repositorio.VentasFuncObtenerId();
                }
                else
                {
                    Numero = Convert.ToString(Repositorio.VentasFuncGuardarBorradorConsultarCierremovimientos3(Id));
                    Repositorio.VentasFuncGuardarBorradorActualizarCierremovimientos(Tipo, Desde.Date, Hasta.Date, Hasta.Date, Observaciones, Id);
                }

                Repositorio.VentasMetAuditar("CIERRE", Id, "GUARDAR_BORRADOR", Observaciones, Usuario);
                Repositorio.VentasMetConfirmar();
                return Numero;
            }
        }

        public void VentasMetEliminarBorrador(int Id, int Usuario, string Motivo)
        {
            Motivo = ClsValidacionesVentas.VentasFuncTexto(Motivo, 180, "motivo de eliminacion");
            using (var Repositorio = new ClsCierreRepositorio())
            {
                Repositorio.VentasMetIniciar();
                Repositorio.VentasMetUsuario(Usuario);
                DataTable Origen = Repositorio.VentasFuncGeneralesEliminarBorradorConsultarCierremovimientos(Id);
                ClsValidacionesVentas.VentasMetExigir(Origen.Rows.Count == 1, "No existe el cierre.");
                Repositorio.VentasMetBloquearMoneda(Convert.ToInt32(Origen.Rows[0][0]));
                DataTable Actual = Repositorio.VentasFuncGeneralesEliminarBorradorConsultarCierremovimientos2(Id);
                ClsValidacionesVentas.VentasMetExigir((string)Actual.Rows[0][0] == "BORRADOR", "Solo se elimina un borrador. No se eliminan cierres confirmados.");
                Repositorio.VentasFuncGeneralesEliminarBorradorActualizarCierremovimientos(Motivo, Id);
                Repositorio.VentasMetAuditar("CIERRE", Id, "ELIMINAR_BORRADOR", Motivo, Usuario);
                Repositorio.VentasMetConfirmar();
            }
        }

        // OperacionCierres.
        private static DataTable VentasFuncSaldos(ClsCierreRepositorio Repositorio, int Moneda, DateTime Desde, DateTime Hasta)
        {
            return Repositorio.Consultas.VentasFuncSaldosConsultarMovimientocxc(Desde.Date, Desde.Date, Desde.Date, new DateTime(Desde.Year, 1, 1), new DateTime(Desde.Year, 1, 1), Moneda, Hasta.Date);
        }

        // ConsultaCierres.
        public DataTable VentasFuncCatalogo(string Nombre)
        {
            using (var Repositorio = new ClsCierreRepositorio())
                return Repositorio.VentasFuncCatalogo(Nombre);
        }

        public DataTable VentasFuncRevisar(int Moneda, string Tipo, DateTime Desde, DateTime Hasta)
        {
            ClsReglasCierre.VentasMetValidarPeriodo(Tipo, Desde, Hasta);
            using (var Repositorio = new ClsCierreRepositorio())
                return VentasFuncSaldos(Repositorio, Moneda, Desde, Hasta);
        }

        public DataTable VentasFuncEstadoPeriodo(int Moneda, string Tipo, DateTime Desde, DateTime Hasta)
        {
            using (var Repositorio = new ClsCierreRepositorio())
                return Repositorio.Consultas.VentasFuncEstadoPeriodoConsultarCierremovimientos(Moneda, Tipo, Desde.Date, Hasta.Date);
        }

        public DataTable VentasFuncHistorial(int Moneda, int Cliente, string Periodo)
        {
            Periodo = (Periodo ?? "").Trim();
            ClsValidacionesVentas.VentasMetExigir(Periodo.Length == 0 || System.Text.RegularExpressions.Regex.IsMatch(Periodo, @"^\d{4}(-\d{2})?$"), "Consulte el periodo como AAAA o AAAA-MM.");
            using (var Repositorio = new ClsCierreRepositorio())
                return Repositorio.Consultas.VentasFuncHistorialConsultarCierremovimientos(Moneda, Cliente, Cliente, Periodo);
        }

        public DataTable VentasFuncPartida(int IdCierre)
        {
            var Partida = new DataTable();
            Partida.Columns.Add("Cuenta", typeof(string));
            Partida.Columns.Add("Descripcion", typeof(string));
            Partida.Columns.Add("Debe", typeof(decimal));
            Partida.Columns.Add("Haber", typeof(decimal));
            using (var Repositorio = new ClsCierreRepositorio())
            {
                DataTable Guardada = Repositorio.Contabilidad.VentasFuncPrepararTrasladoConsultarTrasladocxcdetalle(IdCierre);
                if (Guardada.Rows.Count > 0)
                    return Guardada;
                DataTable Movimientos = Repositorio.Contabilidad.VentasFuncPartidaConsultarCierremovimientos(IdCierre);
                foreach (DataRow Movimiento in Movimientos.Rows)
                {
                    string Clientes = Movimiento["cuentaClientes"] == DBNull.Value ? "PENDIENTE: CxC" : (string)Movimiento["cuentaClientes"];
                    string Contra = Movimiento["cuentaContrapartida"] == DBNull.Value ? "PENDIENTE: contrapartida" : (string)Movimiento["cuentaContrapartida"];
                    decimal Cargos = (decimal)Movimiento["Cargos"], Abonos = (decimal)Movimiento["Abonos"];
                    if (Cargos > 0)
                    {
                        Partida.Rows.Add(Clientes, Movimiento["tipoMovimiento"], Cargos, 0m);
                        Partida.Rows.Add(Contra, Movimiento["tipoMovimiento"], 0m, Cargos);
                    }

                    if (Abonos > 0)
                    {
                        Partida.Rows.Add(Contra, Movimiento["tipoMovimiento"], Abonos, 0m);
                        Partida.Rows.Add(Clientes, Movimiento["tipoMovimiento"], 0m, Abonos);
                    }
                }
            }

            return Partida;
        }

        // ConfirmacionCierres.
        public string VentasFuncConfirmar(int Moneda, int Usuario, string Tipo, DateTime Desde, DateTime Hasta, bool Revisado)
        {
            ClsReglasCierre.VentasMetValidarPeriodo(Tipo, Desde, Hasta);
            ClsValidacionesVentas.VentasMetExigir(Revisado, "Confirme que reviso los documentos y los recibos del periodo.");
            ClsValidacionesVentas.VentasMetExigir(Hasta.Date < DateTime.Today, "El periodo debe haber finalizado antes de confirmarlo.");
            using (var Repositorio = new ClsCierreRepositorio())
            {
                Repositorio.VentasMetIniciar();
                Repositorio.VentasMetUsuario(Usuario);
                Repositorio.VentasMetBloquearMoneda(Moneda);
                ClsValidacionesVentas.VentasMetExigir(Convert.ToInt32(Repositorio.VentasFuncConfirmarConsultarCierremovimientos(Moneda, Tipo, Desde.Date, Hasta.Date)) == 0, "El periodo ya fue cerrado.");
                if (Tipo == "ANUAL")
                {
                    object Primera = Repositorio.VentasFuncConfirmarConsultarCierremovimientos2(Moneda);
                    int Inicio = Primera != null && Primera != DBNull.Value && Convert.ToDateTime(Primera).Year == Desde.Year ? Convert.ToDateTime(Primera).Month : 1;
                    ClsValidacionesVentas.VentasMetExigir(Convert.ToInt32(Repositorio.VentasFuncConfirmarConsultarCierremovimientos3(Moneda, Desde.Year, Inicio)) == 13 - Inicio, "Cierre primero todos los meses operados del ano, hasta diciembre, en esta moneda.");
                }
                else
                {
                    object Ultimo = Repositorio.VentasFuncConfirmarConsultarControlperiodocxc(Moneda);
                    if (Ultimo != null && Ultimo != DBNull.Value)
                        ClsValidacionesVentas.VentasMetExigir(Desde.Date == Convert.ToDateTime(Ultimo).Date.AddDays(1), "Los cierres mensuales deben ser consecutivos y no pueden superponerse.");
                    else
                    {
                        object Primera = Repositorio.VentasFuncConfirmarConsultarMovimientocxc(Moneda);
                        if (Primera != null && Primera != DBNull.Value)
                        {
                            DateTime Fecha = Convert.ToDateTime(Primera);
                            ClsValidacionesVentas.VentasMetExigir(Desde.Date <= new DateTime(Fecha.Year, Fecha.Month, 1), "Hay movimientos de meses anteriores sin cerrar.");
                        }
                    }
                }

                ClsValidacionesVentas.VentasMetExigir(Convert.ToInt32(Repositorio.VentasFuncConfirmarConsultarFactura(Moneda, Desde.Date, Hasta.Date)) == 0, "Hay facturas en borrador en el periodo. Emitirlas o anularlas corresponde a Facturas.");
                ClsValidacionesVentas.VentasMetExigir(Convert.ToInt32(Repositorio.VentasFuncConfirmarConsultarPago(Moneda, Desde.Date, Hasta.Date)) == 0, "Hay recibos en borrador en el periodo.");
                DataTable Clientes = VentasFuncSaldos(Repositorio, Moneda, Desde, Hasta);
                foreach (DataRow ClienteSeleccionado in Clientes.Rows)
                    foreach (string Campo in new[]
                    {
                        "Anterior",
                        "Cargos",
                        "Abonos",
                        "CargosAcumulados",
                        "AbonosAcumulados",
                        "Actual"
                    }

                    )
                        ClsValidacionesVentas.VentasMetExigir(Math.Abs(Convert.ToDecimal(ClienteSeleccionado[Campo])) <= ClsValidacionesVentas.ImporteMaximo, "Un saldo supera la capacidad del historico de cierre.");
                DataTable Borrador = Repositorio.VentasFuncConfirmarConsultarCierremovimientos4(Moneda, Tipo, Desde.Date, Hasta.Date);
                ClsValidacionesVentas.VentasMetExigir(Borrador.Rows.Count <= 1, "Hay varios borradores del mismo periodo; revise los registros antes de confirmar.");
                string Numero;
                int Id;
                if (Borrador.Rows.Count == 1)
                {
                    Id = Convert.ToInt32(Borrador.Rows[0]["idCierre"]);
                    Numero = Convert.ToString(Borrador.Rows[0]["numeroCierre"]);
                    Repositorio.VentasFuncConfirmarActualizarCierremovimientos(Usuario, Id);
                }
                else
                {
                    Numero = Repositorio.VentasFuncNumero("CIE");
                    Repositorio.VentasFuncConfirmarInsertarCierremovimientos(Numero, Desde.Date, Hasta.Date, Hasta.Date, Moneda, Tipo, Usuario);
                    Id = Repositorio.VentasFuncObtenerId();
                }

                foreach (DataRow ClienteSeleccionado2 in Clientes.Rows)
                    Repositorio.VentasFuncConfirmarInsertarCierresaldocliente(Id, ClienteSeleccionado2["idCliente"], ClienteSeleccionado2["Codigo"], ClienteSeleccionado2["Nombre"], ClienteSeleccionado2["Anterior"], ClienteSeleccionado2["Cargos"], ClienteSeleccionado2["Abonos"], ClienteSeleccionado2["CargosAcumulados"], ClienteSeleccionado2["AbonosAcumulados"], ClienteSeleccionado2["Actual"]);
                Repositorio.VentasFuncConfirmarInsertarCierremovimientosdetalle(Id, Desde.Date, Desde.Date, Desde.Date, Moneda, Hasta.Date);
                Repositorio.VentasFuncConfirmarActualizarCierremovimientos2(Id);
                if (Tipo == "MENSUAL")
                    Repositorio.VentasFuncConfirmarActualizarControlperiodocxc(Hasta.Date, Moneda);
                Repositorio.VentasMetAuditar("CIERRE", Id, "CONFIRMAR", Numero, Usuario);
                Repositorio.VentasMetConfirmar();
                return Numero;
            }
        }
    }
}
