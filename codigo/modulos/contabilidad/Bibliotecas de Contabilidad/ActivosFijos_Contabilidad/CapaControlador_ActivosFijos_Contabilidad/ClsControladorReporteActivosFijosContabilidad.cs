/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 *  Reglas del reporte de activos fijos y depreciación a una fecha de
 *  corte: valida los filtros, calcula la depreciación acumulada y el
 *  valor en libros de cada activo y arma las filas del reporte.
 * Reglas especificas:
 *  La depreciación es de línea recta por meses completos y usa la
 *  misma cuota de ClsControladorActivosFijosContabilidad que la póliza
 *  mensual: empieza en el mes de inicio de uso e incluye el mes de la
 *  fecha de corte. Salen los activos vigentes adquiridos hasta el
 *  corte. La fecha de corte no puede ser futura.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CapaModelo_Contabilidad.Contratos;
using CapaModelo_Contabilidad.Entidades;
using CapaModelo_Contabilidad.Repositorios;

namespace CapaControlador_Contabilidad
{
    // Controlador del reporte de activos fijos
    public class ClsControladorReporteActivosFijosContabilidad
    {
        // Opción sin filtro
        public const string Todos = "Todos";

        // Agrupar por módulo
        public const string AgruparModulo = "Módulo";

        // Agrupar por vida útil
        public const string AgruparVidaUtil = "Vida útil";

        // Fecha mínima de corte
        private static readonly DateTime _CorteMinimo = new DateTime(2000, 1, 1);

        // Consulta del reporte
        private readonly IRepositorioReporteActivosFijosContabilidad _Repositorio;

        // Catálogos del módulo
        private readonly ClsControladorActivosFijosContabilidad _Activos = new ClsControladorActivosFijosContabilidad();

        // Usa la base de datos
        public ClsControladorReporteActivosFijosContabilidad() : this(new ClsRepositorioReporteActivosFijosContabilidad())
        {
        }

        // Usa el repositorio dado
        public ClsControladorReporteActivosFijosContabilidad(IRepositorioReporteActivosFijosContabilidad Repositorio)
        {
            if (Repositorio == null) throw new ArgumentNullException("Repositorio");
            _Repositorio = Repositorio;
        }

        // Opciones de agrupación
        public string[] ContabilidadFuncAgrupaciones()
        {
            return new[] { AgruparModulo, AgruparVidaUtil };
        }

        // Módulos para el filtro
        public string[] ContabilidadFuncModulos()
        {
            List<string> Modulos = new List<string> { Todos };
            Modulos.AddRange(_Activos.ContabilidadFuncModulos());
            return Modulos.ToArray();
        }

        // Tipos para el filtro
        public DataTable ContabilidadFuncTipos()
        {
            DataTable Tabla = _Activos.ContabilidadFuncVidaUtil();
            DataRow Fila = Tabla.NewRow();
            Fila["Tipo"] = string.Empty;
            Fila["Nombre"] = Todos;
            Fila["Meses"] = 0;
            Tabla.Rows.InsertAt(Fila, 0);
            return Tabla;
        }

        // Revisa los filtros
        public static string ContabilidadFuncValidarFiltros(DateTime Corte, string AgruparPor)
        {
            if (Corte.Date > DateTime.Today) return "La fecha de corte no puede ser posterior a hoy.";
            if (Corte.Date < _CorteMinimo) return "La fecha de corte debe ser del año 2000 en adelante.";
            if (AgruparPor != AgruparModulo && AgruparPor != AgruparVidaUtil) return "Elija si el reporte se agrupa por módulo o por vida útil.";
            return string.Empty;
        }

        // Depreciación acumulada al corte
        public static decimal ContabilidadFuncDepreciacionAlCorte(decimal Compra, decimal Residual, int Meses, DateTime Inicio,
            DateTime Corte, out int MesesDepreciados)
        {
            MesesDepreciados = 0;
            if (Inicio.Date > Corte.Date) return 0m;
            decimal Libros = Compra;
            DateTime Ultimo = new DateTime(Corte.Year, Corte.Month, 1);
            for (DateTime Mes = new DateTime(Inicio.Year, Inicio.Month, 1); Mes <= Ultimo; Mes = Mes.AddMonths(1))
            {
                decimal Cuota = ClsControladorActivosFijosContabilidad.ContabilidadFuncCuotaMensual(Compra, Residual, Meses, Libros, Inicio, Mes);
                if (Cuota <= 0m) break;
                Libros -= Cuota;
                MesesDepreciados++;
            }
            return Compra - Libros;
        }

        // Datos del reporte o null
        public DataTable ContabilidadFuncDatosReporte(DateTime Corte, string Modulo, string Tipo, string AgruparPor, out string Mensaje)
        {
            Mensaje = ContabilidadFuncValidarFiltros(Corte, AgruparPor);
            if (Mensaje != string.Empty) return null;
            try
            {
                List<ClsActivoReporteContabilidad> Activos = _Repositorio.ContabilidadFuncObtenerActivosAlCorte(Corte.Date,
                    ContabilidadFuncFiltro(Modulo), ContabilidadFuncFiltro(Tipo));
                return ContabilidadFuncArmarTabla(Activos, Corte, AgruparPor);
            }
            catch (Exception Excepcion)
            {
                Mensaje = "No se pudieron consultar los activos: " + Excepcion.Message;
                return null;
            }
        }

        // Filas calculadas del reporte
        public static DataTable ContabilidadFuncArmarTabla(IEnumerable<ClsActivoReporteContabilidad> Activos, DateTime Corte, string AgruparPor)
        {
            DataTable Tabla = ContabilidadFuncCrearTabla();
            foreach (ClsActivoReporteContabilidad Activo in Activos.Where(Activo => Activo.Adquisicion.Date <= Corte.Date)
                .OrderBy(Activo => ContabilidadFuncGrupo(Activo, AgruparPor)).ThenBy(Activo => Activo.Codigo))
            {
                int MesesDepreciados;
                decimal Acumulada = ContabilidadFuncDepreciacionAlCorte(Activo.Compra, Activo.Residual, Activo.Meses, Activo.Inicio,
                    Corte, out MesesDepreciados);
                bool SinIniciar = Activo.Inicio.Date > Corte.Date;
                bool Depreciado = !SinIniciar && Acumulada >= Activo.Compra - Activo.Residual;
                decimal Cuota = Depreciado || Activo.Meses <= 0 ? 0m
                    : Math.Round((Activo.Compra - Activo.Residual) / Activo.Meses, 2, MidpointRounding.AwayFromZero);
                string Estado = SinIniciar ? "Sin iniciar" : Depreciado ? "Depreciado" : "En uso (" + MesesDepreciados + "/" + Activo.Meses + ")";
                Tabla.Rows.Add(ContabilidadFuncGrupo(Activo, AgruparPor), Activo.Codigo, Activo.Descripcion, Activo.Modulo,
                    string.IsNullOrWhiteSpace(Activo.NombreTipo) ? Activo.Tipo : Activo.NombreTipo, Activo.Meses, Activo.Adquisicion.Date,
                    Activo.Inicio.Date, Activo.Compra, Activo.Residual, Cuota, Acumulada, Activo.Compra - Acumulada, MesesDepreciados,
                    Estado, Depreciado);
            }
            return Tabla;
        }

        // Columnas del reporte
        public static DataTable ContabilidadFuncCrearTabla()
        {
            DataTable Tabla = new DataTable("ActivosFijos");
            Tabla.Columns.Add("Grupo", typeof(string));
            Tabla.Columns.Add("Codigo", typeof(string));
            Tabla.Columns.Add("Descripcion", typeof(string));
            Tabla.Columns.Add("Modulo", typeof(string));
            Tabla.Columns.Add("VidaUtil", typeof(string));
            Tabla.Columns.Add("Meses", typeof(int));
            Tabla.Columns.Add("Adquisicion", typeof(DateTime));
            Tabla.Columns.Add("Inicio", typeof(DateTime));
            Tabla.Columns.Add("Compra", typeof(decimal));
            Tabla.Columns.Add("Residual", typeof(decimal));
            Tabla.Columns.Add("Cuota", typeof(decimal));
            Tabla.Columns.Add("Acumulada", typeof(decimal));
            Tabla.Columns.Add("Libros", typeof(decimal));
            Tabla.Columns.Add("MesesDepreciados", typeof(int));
            Tabla.Columns.Add("Estado", typeof(string));
            Tabla.Columns.Add("Depreciado", typeof(bool));
            return Tabla;
        }

        // Nombre del grupo
        private static string ContabilidadFuncGrupo(ClsActivoReporteContabilidad Activo, string AgruparPor)
        {
            if (AgruparPor == AgruparVidaUtil) return string.IsNullOrWhiteSpace(Activo.NombreTipo) ? Activo.Tipo : Activo.NombreTipo;
            return Activo.Modulo;
        }

        // Valor del filtro
        private static string ContabilidadFuncFiltro(string Valor)
        {
            return string.IsNullOrWhiteSpace(Valor) || Valor.Trim() == Todos ? string.Empty : Valor.Trim();
        }
    }
}
