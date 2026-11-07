using System;
using System.Collections.Generic;
using System.Linq;
using CapaModelo_CierreMovimientos.Entidades;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 04/11/2026
 * ==================================================================
 * Propósito :
 *  Reglas de negocio del cierre de movimientos. No acceden a la base de datos, por lo que se
 *  prueban con pruebas unitarias:
 *   - Validaciones previas y si el período se puede cerrar.
 *   - Clasificación de los documentos pendientes por rango de antigüedad.
 * ===================================================================
*/

namespace CapaModelo_CierreMovimientos.Servicios
{
    public static class ClsReglasCierreMovimientos
    {
        public static List<ClsValidacionCierre> ComprasFuncEvaluarValidaciones(ClsPeriodoCompras Periodo,
            ClsIndicadoresCierre Indicadores, int ProveedoresConMovimiento, bool HaySesion, DateTime Hoy)
        {
            if (Periodo == null) throw new ArgumentNullException(nameof(Periodo));
            if (Indicadores == null) throw new ArgumentNullException(nameof(Indicadores));

            var Validaciones = new List<ClsValidacionCierre>();

            if (Periodo.EstaCerrado)
                ComprasProcAgregar(Validaciones, ComprasEnumEstadoValidacion.Bloqueante, "El período ya está cerrado", "3.8 Cierre");

            ComprasProcAgregar(Validaciones,
                Indicadores.PeriodosAbiertosAnteriores == 0 ? ComprasEnumEstadoValidacion.Correcta : ComprasEnumEstadoValidacion.Bloqueante,
                Indicadores.PeriodosAbiertosAnteriores == 0
                    ? "Períodos anteriores cerrados"
                    : "Hay " + Indicadores.PeriodosAbiertosAnteriores + " período(s) anterior(es) sin cerrar",
                "3.8 Cierre");

            ComprasProcAgregar(Validaciones,
                Periodo.FechaFin < Hoy.Date ? ComprasEnumEstadoValidacion.Correcta : ComprasEnumEstadoValidacion.Bloqueante,
                Periodo.FechaFin < Hoy.Date
                    ? "El mes ya terminó"
                    : "El mes termina el " + Periodo.FechaFin.ToString("dd/MM/yyyy") + "; se puede cerrar a partir del día siguiente",
                "3.8 Cierre");

            ComprasProcAgregar(Validaciones,
                Indicadores.PagosProgramados == 0 ? ComprasEnumEstadoValidacion.Correcta : ComprasEnumEstadoValidacion.Bloqueante,
                Indicadores.PagosProgramados == 0
                    ? "Sin pagos programados pendientes"
                    : Indicadores.PagosProgramados + " pago(s) programado(s) sin resolver (Q " +
                      Indicadores.MontoPagosProgramados.ToString("N2") + ")",
                "3.5 Pagos a proveedores");

            ComprasProcAgregar(Validaciones,
                Indicadores.RangosAntiguedadActivos > 0 ? ComprasEnumEstadoValidacion.Correcta : ComprasEnumEstadoValidacion.Bloqueante,
                Indicadores.RangosAntiguedadActivos > 0
                    ? "Rangos de antigüedad configurados (" + Indicadores.RangosAntiguedadActivos + ")"
                    : "No hay rangos de antigüedad activos",
                "3.4 Reportes");

            ComprasProcAgregar(Validaciones,
                Indicadores.ComprasSinCuentaPorPagar == 0 ? ComprasEnumEstadoValidacion.Correcta : ComprasEnumEstadoValidacion.Advertencia,
                Indicadores.ComprasSinCuentaPorPagar == 0
                    ? "Compras del mes con su cuenta por pagar"
                    : Indicadores.ComprasSinCuentaPorPagar + " compra(s) del mes sin cuenta por pagar",
                "3.1 Movimientos");

            ComprasProcAgregar(Validaciones,
                Indicadores.OrdenesPendientes == 0 ? ComprasEnumEstadoValidacion.Correcta : ComprasEnumEstadoValidacion.Advertencia,
                Indicadores.OrdenesPendientes == 0
                    ? "Sin órdenes de compra pendientes"
                    : Indicadores.OrdenesPendientes + " orden(es) de compra pendiente(s) de autorizar",
                "3.3 Órdenes de compra");

            if (Indicadores.DocumentosMonedaExtranjera > 0)
                ComprasProcAgregar(Validaciones, ComprasEnumEstadoValidacion.Advertencia,
                    Indicadores.DocumentosMonedaExtranjera + " documento(s) en moneda extranjera: se suman en Q con su tipo de cambio de registro",
                    "Bancos");

            ComprasProcAgregar(Validaciones,
                ProveedoresConMovimiento > 0 ? ComprasEnumEstadoValidacion.Correcta : ComprasEnumEstadoValidacion.Advertencia,
                ProveedoresConMovimiento > 0
                    ? ProveedoresConMovimiento + " proveedor(es) con saldo o movimientos"
                    : "El período no tiene saldos ni movimientos",
                "3.1 Movimientos");

            ComprasProcAgregar(Validaciones,
                HaySesion ? ComprasEnumEstadoValidacion.Correcta : ComprasEnumEstadoValidacion.Advertencia,
                HaySesion ? "Usuario identificado en Seguridad" : "Sin sesión de Seguridad: el cierre se registrará sin usuario",
                "Seguridad");

            return Validaciones;
        }

        public static bool ComprasFuncPuedeCerrar(IEnumerable<ClsValidacionCierre> Validaciones)
        {
            return Validaciones != null && Validaciones.All(Validacion => Validacion.Estado != ComprasEnumEstadoValidacion.Bloqueante);
        }

        public static List<ClsAntiguedadCierre> ComprasFuncCalcularAntiguedad(IEnumerable<ClsDocumentoPendiente> Documentos,
            IList<ClsRangoAntiguedad> Rangos, DateTime FechaCorte)
        {
            if (Rangos == null || Rangos.Count == 0)
                throw new InvalidOperationException("No hay rangos de antigüedad activos.");

            var Acumulado = new Dictionary<Tuple<int, int>, ClsAntiguedadCierre>();
            foreach (ClsDocumentoPendiente Documento in Documentos ?? Enumerable.Empty<ClsDocumentoPendiente>())
            {
                if (Documento.SaldoLocal <= 0) continue;
                int DiasVencidos = (FechaCorte.Date - Documento.FechaVencimiento.Date).Days;
                ClsRangoAntiguedad Rango = Rangos.FirstOrDefault(Candidato => Candidato.ComprasFuncIncluye(DiasVencidos));
                if (Rango == null)
                    throw new InvalidOperationException("Ningún rango de antigüedad incluye " + DiasVencidos + " días vencidos.");

                var Llave = Tuple.Create(Documento.IdProveedor, Rango.IdRango);
                if (!Acumulado.TryGetValue(Llave, out ClsAntiguedadCierre Fila))
                {
                    Fila = new ClsAntiguedadCierre { IdProveedor = Documento.IdProveedor, IdRango = Rango.IdRango };
                    Acumulado.Add(Llave, Fila);
                }
                Fila.Monto += Documento.SaldoLocal;
                Fila.CantidadDocumentos++;
            }
            return Acumulado.Values.OrderBy(Fila => Fila.IdProveedor).ThenBy(Fila => Fila.IdRango).ToList();
        }

        private static void ComprasProcAgregar(List<ClsValidacionCierre> Validaciones, ComprasEnumEstadoValidacion Estado,
            string Validacion, string Origen)
        {
            Validaciones.Add(new ClsValidacionCierre { Estado = Estado, Validacion = Validacion, Origen = Origen });
        }
    }
}
