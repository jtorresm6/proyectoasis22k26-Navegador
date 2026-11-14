using System;
using CapaModelo_CierreMovimientos.Repositorios;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 04/11/2026
 * ==================================================================
 * Propósito :
 *  Validador de períodos cerrados para los demás submódulos de Compras
 *  (Movimientos, Órdenes de compra, Pagos, Devoluciones). Antes de registrar,
 *  editar o anular un documento, el submódulo llama a
 *  ComprasProcValidarFechaAbierta con la fecha del documento:
 *
 *      new ClsValidadorPeriodoCompras().ComprasProcValidarFechaAbierta(Fecha, "la factura");
 *
 *  Una fecha está cerrada si es igual o anterior al último día del último
 *  período cerrado (los cierres son consecutivos).
 * ===================================================================
*/

namespace CapaModelo_CierreMovimientos.Servicios
{
    public class ClsValidadorPeriodoCompras
    {
        private readonly ClsConsultasCierreMovimientos _Consultas;

        public ClsValidadorPeriodoCompras() : this(new ClsConsultasCierreMovimientos()) { }

        public ClsValidadorPeriodoCompras(ClsConsultasCierreMovimientos Consultas)
        {
            _Consultas = Consultas ?? throw new ArgumentNullException(nameof(Consultas));
        }

        public DateTime? ComprasFuncObtenerUltimaFechaCerrada()
        {
            return _Consultas.ComprasFuncObtenerUltimaFechaCerrada();
        }

        public bool ComprasFuncFechaCerrada(DateTime Fecha)
        {
            return ComprasFuncFechaCerrada(Fecha, ComprasFuncObtenerUltimaFechaCerrada());
        }

        public static bool ComprasFuncFechaCerrada(DateTime Fecha, DateTime? UltimaFechaCerrada)
        {
            return UltimaFechaCerrada.HasValue && Fecha.Date <= UltimaFechaCerrada.Value.Date;
        }

        public void ComprasProcValidarFechaAbierta(DateTime Fecha, string Documento = "el documento")
        {
            DateTime? UltimaFechaCerrada = ComprasFuncObtenerUltimaFechaCerrada();
            if (ComprasFuncFechaCerrada(Fecha, UltimaFechaCerrada))
                throw new InvalidOperationException("No se puede registrar ni modificar " + Documento + " con fecha " +
                    Fecha.ToString("dd/MM/yyyy") + ": Compras está cerrado hasta el " +
                    UltimaFechaCerrada.Value.ToString("dd/MM/yyyy") + ".");
        }
    }
}
