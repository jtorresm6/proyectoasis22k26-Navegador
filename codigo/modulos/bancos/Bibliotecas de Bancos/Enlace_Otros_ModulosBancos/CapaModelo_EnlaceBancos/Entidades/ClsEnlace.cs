// =====================================================================
// Autor:       Andre De Jesus Gonzalez Camey
// Carné:       9959-23-3117
// Módulo:      Tesorería y Bancos – Enlace con otros módulos
// Descripción: Entidades del enlace: enumeración de módulos destino, contexto de la
//              partida y datos de la partida contable.
// Fecha:       12/11/2026
// =====================================================================
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace CapaModelo_EnlaceB.Entidades
{
    public enum ModuloEnlace
    {
        Logistica,
        Compras,
        Ventas,
        RecursosHumanos
    }

    public sealed class ClsContextoPartida
    {
        public int Empresa { get; set; }
        public int Pais { get; set; }
        public int ModuloContable { get; set; }
        public string Responsable { get; set; }
        public int UsuarioId { get; set; }
        public string Ip { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
    }

    public sealed class ClsPartidaEnlace
    {
        public long Id { get; set; }
        public string Estado { get; set; }
        public string Referencia { get; set; }
        public DataTable Detalle { get; set; }
    }

    public sealed class ClsEnvioContable
    {
        public string CodigoModulo { get; set; }
        public int Pais { get; set; }
        public string DocumentoOrigen { get; set; }
        public DateTime Fecha { get; set; }
        public string Concepto { get; set; }
        public DataTable Lineas { get; set; }
    }

    public sealed class ClsResultadoEnvioContable
    {
        public string Codigo { get; set; }
        public string Estado { get; set; }
        public string Mensaje { get; set; }
    }

    public static class ClsReglasEnlace
    {
        public static void BancarioMetValidarFechas(DateTime Inicio, DateTime Fin, bool Mensual)
        {
            if (Inicio.Date > Fin.Date)
                throw new ArgumentException("La fecha inicial no puede ser mayor que la final.");
            if (Fin.Year >= 9999)
                throw new ArgumentException("Seleccione un año anterior a 9999.");
            if (Mensual && (Inicio.Day != 1 || Fin.Date != new DateTime(Inicio.Year, Inicio.Month, DateTime.DaysInMonth(Inicio.Year, Inicio.Month))))
                throw new ArgumentException("La partida debe comprender un mes calendario completo.");
        }

        public static void BancarioMetValidarContexto(ClsContextoPartida Contexto)
        {
            if (Contexto == null)
                throw new ArgumentException("Faltan los datos de la partida.");
            BancarioMetValidarFechas(Contexto.Inicio, Contexto.Fin, true);
            if (Contexto.Empresa <= 0 || Contexto.Pais <= 0 || Contexto.ModuloContable <= 0)
                throw new ArgumentException("Seleccione empresa, país y módulo contable.");
            if (string.IsNullOrWhiteSpace(Contexto.Responsable) || Contexto.Responsable.Trim().Length > 100)
                throw new ArgumentException("Indique un responsable válido de hasta 100 caracteres.");
        }

        public static DataTable BancarioMetPrepararLineasContables(DataTable Detalle)
        {
            if (Detalle == null || Detalle.Rows.Count == 0)
                throw new ArgumentException("La partida no tiene detalle.");
            var Resultado = new DataTable();
            Resultado.Columns.Add("Codigo", typeof(string));
            Resultado.Columns.Add("Debe", typeof(decimal));
            Resultado.Columns.Add("Haber", typeof(decimal));
            decimal Debe = 0, Haber = 0;
            var Codigos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow Fila in Detalle.Rows)
            {
                string Codigo = Convert.ToString(Fila["CodigoCuentaDetallePartida"]);
                decimal ImporteDebe = Convert.ToDecimal(Fila["DebeDetallePartida"]);
                decimal ImporteHaber = Convert.ToDecimal(Fila["HaberDetallePartida"]);
                if (!Regex.IsMatch(Codigo, @"^[1-6]\.[0-9]{2}\.[0-9]{2}\.[0-9]{3}\z"))
                    throw new ArgumentException("Use una cuenta con formato C.GG.CC.SSS.");
                if (!Codigos.Add(Codigo))
                    throw new ArgumentException("La partida contiene una cuenta duplicada.");
                if (ImporteDebe < 0 || ImporteHaber < 0 || (ImporteDebe == 0 && ImporteHaber == 0))
                    throw new ArgumentException("La cuenta debe tener importes válidos.");
                if (ImporteDebe > 99999999.9999m || ImporteHaber > 99999999.9999m)
                    throw new ArgumentException("El importe supera la capacidad de la base de datos.");
                if (ImporteDebe > 0)
                    Resultado.Rows.Add(Codigo, ImporteDebe, 0m);
                if (ImporteHaber > 0)
                    Resultado.Rows.Add(Codigo, 0m, ImporteHaber);
                Debe += ImporteDebe;
                Haber += ImporteHaber;
            }

            if (Debe <= 0 || Debe != Haber || Debe > 99999999.9999m)
                throw new ArgumentException("La partida debe cuadrar, tener importes positivos y respetar la capacidad de la base de datos.");
            return Resultado;
        }
    }
}
