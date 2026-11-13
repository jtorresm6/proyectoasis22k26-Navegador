/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Construye la tabla de partidas que muestran la ventana y el
 *  reporte de conciliación bancaria.
 * ================================================================
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using CapaModelo_ConciliacionBancaria.Entidades;

namespace CapaControlador_ConciliacionBancaria
{
    public static class ClsTablaConciliacion
    {
        private static readonly CultureInfo CulturaFechas = CultureInfo.InvariantCulture;

        public static DataTable BancosFuncConstruir(IEnumerable<ClsDetalleConciliacion> Detalles, ClsConciliacion Encabezado)
        {
            DataTable Tabla = new DataTable("Conciliacion");
            Tabla.Columns.Add("IdMovimiento", typeof(long));
            Tabla.Columns.Add("IdLinea", typeof(long));
            Tabla.Columns.Add("CodigoResultado", typeof(string));
            Tabla.Columns.Add("CodigoMetodo", typeof(string));
            Tabla.Columns.Add("Estado", typeof(string));
            Tabla.Columns.Add("Origen", typeof(string));
            Tabla.Columns.Add("Fecha", typeof(string));
            Tabla.Columns.Add("ReferenciaLibro", typeof(string));
            Tabla.Columns.Add("ConceptoLibro", typeof(string));
            Tabla.Columns.Add("ImporteLibro", typeof(decimal));
            Tabla.Columns.Add("ReferenciaBanco", typeof(string));
            Tabla.Columns.Add("ConceptoBanco", typeof(string));
            Tabla.Columns.Add("ImporteBanco", typeof(decimal));
            Tabla.Columns.Add("Diferencia", typeof(decimal));
            Tabla.Columns.Add("Metodo", typeof(string));
            Tabla.Columns.Add("Motivo", typeof(string));

            if (Encabezado != null)
            {
                Tabla.Columns.Add("Cuenta", typeof(string));
                Tabla.Columns.Add("Banco", typeof(string));
                Tabla.Columns.Add("Moneda", typeof(string));
                Tabla.Columns.Add("FechaCorte", typeof(string));
                Tabla.Columns.Add("Extracto", typeof(string));
                Tabla.Columns.Add("SaldoLibro", typeof(decimal));
                Tabla.Columns.Add("SaldoBanco", typeof(decimal));
                Tabla.Columns.Add("DiferenciaSaldos", typeof(decimal));
                Tabla.Columns.Add("EstadoConciliacion", typeof(string));
                Tabla.Columns.Add("Responsable", typeof(string));
            }

            foreach (ClsDetalleConciliacion Detalle in Detalles)
            {
                DataRow Fila = Tabla.NewRow();
                Fila["IdMovimiento"] = Detalle.Movimiento != null ? (object)Detalle.Movimiento.IdMovimiento : DBNull.Value;
                Fila["IdLinea"] = Detalle.Linea != null ? (object)Detalle.Linea.IdLinea : DBNull.Value;
                Fila["CodigoResultado"] = Detalle.Resultado;
                Fila["CodigoMetodo"] = Detalle.Metodo;
                Fila["Estado"] = BancosFuncTextoResultado(Detalle.Resultado);
                Fila["Origen"] = BancosFuncTextoOrigen(Detalle);
                Fila["Fecha"] = ClsMotorConciliacion.BancosFuncFecha(Detalle).ToString("dd/MM/yyyy", CulturaFechas);

                if (Detalle.Movimiento != null)
                {
                    Fila["ReferenciaLibro"] = Detalle.Movimiento.Referencia ?? string.Empty;
                    Fila["ConceptoLibro"] = Detalle.Movimiento.Concepto;
                    Fila["ImporteLibro"] = Detalle.Movimiento.Importe * Detalle.Movimiento.Sentido;
                }

                if (Detalle.Linea != null)
                {
                    Fila["ReferenciaBanco"] = Detalle.Linea.Referencia ?? string.Empty;
                    Fila["ConceptoBanco"] = Detalle.Linea.Concepto;
                    Fila["ImporteBanco"] = Detalle.Linea.Importe * Detalle.Linea.Sentido;
                }

                Fila["Diferencia"] = Detalle.Diferencia;
                Fila["Metodo"] = Detalle.Metodo == ClsValoresConciliacion.MetodoManual ? "Manual" : "Automático";
                Fila["Motivo"] = Detalle.Motivo ?? string.Empty;

                if (Encabezado != null)
                {
                    Fila["Cuenta"] = Encabezado.NumeroCuenta + " - " + Encabezado.NombreCuenta;
                    Fila["Banco"] = Encabezado.Banco;
                    Fila["Moneda"] = Encabezado.Moneda;
                    Fila["FechaCorte"] = Encabezado.FechaCorte.ToString("dd/MM/yyyy", CulturaFechas);
                    Fila["Extracto"] = Encabezado.ReferenciaExtracto;
                    Fila["SaldoLibro"] = Encabezado.SaldoLibro;
                    Fila["SaldoBanco"] = Encabezado.SaldoBanco;
                    Fila["DiferenciaSaldos"] = Encabezado.Diferencia;
                    Fila["EstadoConciliacion"] = ClsMotorConciliacion.BancosFuncTextoEstado(Encabezado.Estado);
                    Fila["Responsable"] = Encabezado.Responsable;
                }

                Tabla.Rows.Add(Fila);
            }

            return Tabla;
        }

        public static string BancosFuncTextoResultado(string Resultado)
        {
            switch (Resultado)
            {
                case ClsValoresConciliacion.ResultadoCoincide: return "Conciliado";
                case ClsValoresConciliacion.ResultadoDiferencia: return "Con diferencia";
                default: return "Pendiente";
            }
        }

        private static string BancosFuncTextoOrigen(ClsDetalleConciliacion Detalle)
        {
            if (Detalle.Movimiento != null && Detalle.Linea != null) return "Libro y banco";
            return Detalle.Movimiento != null ? "Solo libro" : "Solo banco";
        }
    }
}
