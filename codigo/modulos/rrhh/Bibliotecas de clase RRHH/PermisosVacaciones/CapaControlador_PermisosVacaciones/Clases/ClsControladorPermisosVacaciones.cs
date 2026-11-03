/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 12/10/2026
 * ==================================================================
 * Propósito :
 *  Capa Controlador de la ventana 9201 - PermisosVacaciones. Le pasa a
 *  la Vista la tabla, el módulo y la aplicación con que se arma el
 *  Navegador de Permisos (9202) y de Vacaciones (9203).
 * Modificado por : Byron Alexander Chiquito Paredes 0901-23-3747 (23/10/2026)
 * Modificado por : Daniella Jurado 0901-23-3679 (02/11/2026)
 *  También entrega las columnas de fecha inicio y fin que valida la Vista
 *  y aplica las reglas de vacaciones del Código de Trabajo de Guatemala:
 *   - Art. 130: 15 días hábiles por cada año de trabajo continuo.
 *   - Art. 136: no se acumulan; cada año de servicio es un período.
 *   - Art. 137: se pueden dividir como máximo en dos partes.
 *  Los días hábiles se cuentan de lunes a viernes. La vacación se
 *  carga al último año de servicio cumplido a la fecha de inicio.
 * ===================================================================
*/

using System;
using System.Data;
using CapaModelo_PermisosVacaciones.Clases;

namespace CapaControlador_PermisosVacaciones.Clases
{
    public class ClsControladorPermisosVacaciones
    {
        private const int _IdModulo = 9000;
        private const int _IdAplicacionPermisos = 9202;
        private const int _IdAplicacionVacaciones = 9203;
        private const int _DiasVacacionPorAnio = 15;
        private const int _PartesMaximas = 2;

        private readonly ClsSentenciasPermisosVacaciones _Sentencias = new ClsSentenciasPermisosVacaciones();

        public int RrhhFuncIdModulo()
        {
            return _IdModulo;
        }

        public string RrhhFuncTablaPermisos()
        {
            return _Sentencias.RrhhFuncTablaPermisos();
        }

        public int RrhhFuncIdAplicacionPermisos()
        {
            return _IdAplicacionPermisos;
        }

        public string RrhhFuncTablaVacaciones()
        {
            return _Sentencias.RrhhFuncTablaVacaciones();
        }

        public int RrhhFuncIdAplicacionVacaciones()
        {
            return _IdAplicacionVacaciones;
        }

        public string[] RrhhFuncFechasPermisos()
        {
            return _Sentencias.RrhhFuncFechasPermisos();
        }

        public string[] RrhhFuncFechasVacaciones()
        {
            return _Sentencias.RrhhFuncFechasVacaciones();
        }

        // Días de lunes a viernes entre las dos fechas, incluyendo ambas.
        public int RrhhFuncDiasHabiles(DateTime Inicio, DateTime Fin)
        {
            int Dias = 0;
            for (DateTime Dia = Inicio.Date; Dia <= Fin.Date; Dia = Dia.AddDays(1))
            {
                if (Dia.DayOfWeek != DayOfWeek.Saturday && Dia.DayOfWeek != DayOfWeek.Sunday)
                    Dias++;
            }
            return Dias;
        }

        // Años completos de trabajo entre la fecha de contratación y la fecha indicada.
        private static int RrhhFuncAniosServicio(DateTime Contratacion, DateTime Fecha)
        {
            int Anios = Fecha.Year - Contratacion.Year;
            if (Contratacion.AddYears(Anios) > Fecha.Date) Anios--;
            return Anios;
        }

        // Puede optar a vacaciones si a la fecha actual ya cumplió un año de trabajo (Art. 130).
        // Devuelve null si puede optar, o el mensaje con la fecha en que podrá hacerlo.
        public string RrhhFuncPuedeOptar(int IdEmpleado)
        {
            DateTime Contratacion = _Sentencias.RrhhFuncFechaContratacion(IdEmpleado).Date;
            if (RrhhFuncAniosServicio(Contratacion, DateTime.Today) >= 1) return null;

            return "El empleado no puede optar a vacaciones todavía.\n" +
                   "Según el Art. 130 del Código de Trabajo, el derecho se adquiere después de un año de trabajo continuo.\n\n" +
                   "Fecha de contratación: " + Contratacion.ToString("dd/MM/yyyy") + "\n" +
                   "Podrá optar a partir del: " + Contratacion.AddYears(1).ToString("dd/MM/yyyy");
        }

        // Devuelve null si la vacación cumple la ley, o el mensaje de lo que no cumple.
        // IdVacacionActual = 0 al ingresar; al modificar es la vacación que se edita (no se cuenta dos veces).
        public string RrhhFuncValidarVacacion(int IdEmpleado, DateTime Inicio, DateTime Fin, int IdVacacionActual)
        {
            if (Fin.Date < Inicio.Date)
                return "La fecha fin no puede ser anterior a la fecha inicio.";

            int Dias = RrhhFuncDiasHabiles(Inicio, Fin);
            if (Dias == 0)
                return "El rango elegido no tiene días hábiles (lunes a viernes).";

            // Años de trabajo continuo cumplidos a la fecha de inicio de la vacación.
            string NoOpta = RrhhFuncPuedeOptar(IdEmpleado);
            if (NoOpta != null) return NoOpta;

            DateTime Contratacion = _Sentencias.RrhhFuncFechaContratacion(IdEmpleado).Date;
            int Anios = RrhhFuncAniosServicio(Contratacion, Inicio);

            if (Anios < 1)
                return "La fecha de inicio de las vacaciones es antes de que el empleado cumpla su primer año de trabajo.\n" +
                       "Las vacaciones pueden iniciar a partir del " + Contratacion.AddYears(1).ToString("dd/MM/yyyy") + ".";

            // Período: el último año de servicio cumplido.
            DateTime InicioPeriodo = Contratacion.AddYears(Anios);
            int Partes;
            decimal DiasTomados = RrhhFuncDiasTomados(IdEmpleado, IdVacacionActual, InicioPeriodo, out Partes);
            string Periodo = RrhhFuncTextoPeriodo(InicioPeriodo);

            if (Partes >= _PartesMaximas)
                return "Según el Art. 137 del Código de Trabajo, las vacaciones se pueden dividir como máximo " +
                       "en dos partes.\nEl empleado ya tomó sus vacaciones en dos partes en el período " + Periodo + ".";

            if (DiasTomados + Dias > _DiasVacacionPorAnio)
                return "Según el Art. 130 del Código de Trabajo, corresponden " + _DiasVacacionPorAnio +
                       " días hábiles por año de trabajo.\nPeríodo " + Periodo + ": ya tomó " + DiasTomados.ToString("0") +
                       " días, le quedan " + (_DiasVacacionPorAnio - DiasTomados).ToString("0") +
                       " y está pidiendo " + Dias + ".";

            return null;
        }

        // Resumen que la Vista muestra al elegir un empleado: contratación, antigüedad a la fecha
        // actual y, si ya puede optar, los días que le quedan en su período vigente.
        public string RrhhFuncResumenEmpleado(int IdEmpleado, int IdVacacionActual)
        {
            DateTime Contratacion = _Sentencias.RrhhFuncFechaContratacion(IdEmpleado).Date;
            int Anios = RrhhFuncAniosServicio(Contratacion, DateTime.Today);
            string Resumen = "Fecha de contratación: " + Contratacion.ToString("dd/MM/yyyy") +
                             "     Antigüedad: " + Anios + (Anios == 1 ? " año" : " años");

            if (Anios < 1)
                return Resumen + "\nNO PUEDE OPTAR a vacaciones: podrá hacerlo a partir del " +
                       Contratacion.AddYears(1).ToString("dd/MM/yyyy") + " (Art. 130, un año de trabajo continuo).";

            DateTime InicioPeriodo = Contratacion.AddYears(Anios);
            int Partes;
            decimal DiasTomados = RrhhFuncDiasTomados(IdEmpleado, IdVacacionActual, InicioPeriodo, out Partes);
            return Resumen + "\nPUEDE OPTAR a vacaciones. Período " + RrhhFuncTextoPeriodo(InicioPeriodo) +
                   ": tomados " + DiasTomados.ToString("0") + " de " + _DiasVacacionPorAnio +
                   " días hábiles, disponibles " + (_DiasVacacionPorAnio - DiasTomados).ToString("0") +
                   " (partes usadas " + Partes + " de " + _PartesMaximas + ").";
        }

        // Días y partes ya tomados en el período (año de servicio) que empieza en InicioPeriodo.
        private decimal RrhhFuncDiasTomados(int IdEmpleado, int IdVacacionActual, DateTime InicioPeriodo, out int Partes)
        {
            DateTime FinPeriodo = InicioPeriodo.AddYears(1);
            decimal DiasTomados = 0;
            Partes = 0;
            foreach (DataRow Fila in _Sentencias.RrhhFuncVacacionesEmpleado(IdEmpleado).Rows)
            {
                DateTime InicioOtra = Convert.ToDateTime(Fila["fechaInicioVacacionEmpleado"]);
                if (Convert.ToInt32(Fila["idVacacionEmpleado"]) == IdVacacionActual ||
                    InicioOtra < InicioPeriodo || InicioOtra >= FinPeriodo) continue;

                DiasTomados += Convert.ToDecimal(Fila["diasTomarVacacionEmpleado"]);
                Partes++;
            }
            return DiasTomados;
        }

        private static string RrhhFuncTextoPeriodo(DateTime InicioPeriodo)
        {
            return InicioPeriodo.ToString("dd/MM/yyyy") + " al " + InicioPeriodo.AddYears(1).AddDays(-1).ToString("dd/MM/yyyy");
        }
    }
}
