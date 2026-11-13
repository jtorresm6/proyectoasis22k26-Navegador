using CapaModelo_CierreNominas.Contratos;
using CapaModelo_CierreNominas.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ==================================================================
 * Área: Recursos Humanos
 * Autores: Lourdes Isabel Melendez Pineda
 * Fecha o ultima edicion: 31/10/2026
 * ==================================================================
 * Propósito : Repositorio donde están las consultas SQL del cierre de
 * nómina: listar nóminas abiertas, el previo por empleado, las
 * líneas por cuenta para la póliza y el guardado del cierre
 * (encabezado e histórico por empleado) en una sola
 * transacción.
 * ===================================================================
 */

namespace CapaModelo_CierreNominas.Repositorios
{
    public class ClsRepositorioCierreNomina : ClsSentencias, IRepositorioCierreNomina
    {
        private string _SelectAll;
        private string _Insert;
        private string _SelectNominasAbiertas;
        private string _SelectPrevio;
        private string _SelectLineasPoliza;
        private string _SelectEmpleadosPorCerrar;
        private string _InsertEmpleado;
        private string _SelectDetalleCierre;
        private string _SelectNominaCerrada;

        public ClsRepositorioCierreNomina()
        {
            _SelectAll = @"
        SELECT
            idCierreNomina, periodoCierreNomina,
            fechaInicioCierreNomina, fechaFinalCierreNomina,
            cantidadEmpleadoCierreNomina,
            totalPercepcionCierreNomina, totalDeduccionCierreNomina, totalNetoCierreNomina,
            totalPatronalCierreNomina, totalDebeCierreNomina, totalHaberCierreNomina,
            documentoOrigenCierreNomina, codigoPolizaCierreNomina,
            fechaPolizaCierreNomina, estadoPolizaCierreNomina,
            idUsuarioCierra, fechaEjecucionCierreNomina,
            created_at, updated_at
        FROM tblCierreNomina
        ORDER BY fechaFinalCierreNomina DESC, idCierreNomina DESC";

            _Insert = @"
        INSERT INTO tblCierreNomina
            (periodoCierreNomina, fechaInicioCierreNomina, fechaFinalCierreNomina,
             cantidadEmpleadoCierreNomina, totalPercepcionCierreNomina, totalDeduccionCierreNomina,
             totalNetoCierreNomina, totalPatronalCierreNomina, totalDebeCierreNomina, totalHaberCierreNomina,
             documentoOrigenCierreNomina, codigoPolizaCierreNomina, fechaPolizaCierreNomina,
             estadoPolizaCierreNomina, idUsuarioCierra)
        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

            // Una nómina está ABIERTA si está Aprobada o Pagada y todavía no existe en la tabla que se creo tblCierreNominaEmpleado.
            // El cierre es por mes: se agrupa por el mes de la fecha final de la nómina.
            _SelectNominasAbiertas = @"
        SELECT
            DATE_FORMAT(MIN(ne.fechaFinalNominaEncabezado), '%Y-%m') AS periodoNomina,
            DATE_SUB(MIN(ne.fechaFinalNominaEncabezado),
                     INTERVAL DAYOFMONTH(MIN(ne.fechaFinalNominaEncabezado)) - 1 DAY) AS fechaInicioNomina,
            LAST_DAY(MIN(ne.fechaFinalNominaEncabezado)) AS fechaFinalNomina,
            COUNT(*) AS cantidadEmpleadoNomina
        FROM tblNominaEncabezado ne
        WHERE ne.estadoNminaEncabezado IN ('Aprobada', 'Pagada')
          AND NOT EXISTS (SELECT 1 FROM tblCierreNominaEmpleado ce
                          WHERE ce.idNominaEncabezado = ne.idNominaEncabezado)
        GROUP BY YEAR(ne.fechaFinalNominaEncabezado), MONTH(ne.fechaFinalNominaEncabezado)
        ORDER BY MIN(ne.fechaFinalNominaEncabezado) DESC";

            // efectoPercepcionDeduccion: 1 = percepción, 0 = deducción 
            _SelectPrevio = @"
        SELECT
            ne.idNominaEncabezado, ne.idEmpleado, e.codigoEmpleado,
            CONCAT(e.nombresEmpleado, ' ', e.apellidosEmpleado) AS nombreEmpleado,
            ne.salarioBaseNominaEncabezado,
            ne.totalPercepcionNominaEncabezado,
            ne.totalDeduccionNominaEncabezado,
            ne.totalNetoNominaEncabezado,
            COALESCE(SUM(CASE WHEN pd.efectoPercepcionDeduccion = 1 THEN nd.montoNominaDetalle END), 0) AS detallePercepcion,
            COALESCE(SUM(CASE WHEN pd.efectoPercepcionDeduccion = 0 THEN nd.montoNominaDetalle END), 0) AS detalleDeduccion,
            COALESCE(SUM(CASE WHEN nd.idNominaDetalle IS NOT NULL AND pd.idCuenta IS NULL THEN 1 ELSE 0 END), 0) AS conceptosSinCuenta
        FROM tblNominaEncabezado ne
        INNER JOIN tblEmpleado e ON e.idEmpleado = ne.idEmpleado
        LEFT JOIN tblNominaDetalle nd ON nd.idNominaEncabezado = ne.idNominaEncabezado
        LEFT JOIN tblPercepcionDeduccion pd ON pd.idPercepcionDeduccion = nd.idPercepcionDeduccion
        WHERE ne.fechaFinalNominaEncabezado >= ?
          AND ne.fechaFinalNominaEncabezado <= ?
          AND ne.estadoNminaEncabezado IN ('Aprobada', 'Pagada')
          AND NOT EXISTS (SELECT 1 FROM tblCierreNominaEmpleado ce
                          WHERE ce.idNominaEncabezado = ne.idNominaEncabezado)
        GROUP BY ne.idNominaEncabezado, ne.idEmpleado, e.codigoEmpleado, e.nombresEmpleado, e.apellidosEmpleado,
                 ne.salarioBaseNominaEncabezado, ne.totalPercepcionNominaEncabezado,
                 ne.totalDeduccionNominaEncabezado, ne.totalNetoNominaEncabezado
        ORDER BY e.codigoEmpleado";

            _SelectLineasPoliza = @"
        SELECT
            pd.idCuenta, c.codigoCuenta, c.nombreCuenta,
            pd.efectoPercepcionDeduccion AS esPercepcion,
            SUM(nd.montoNominaDetalle) AS montoTotal
        FROM tblNominaDetalle nd
        INNER JOIN tblNominaEncabezado ne ON ne.idNominaEncabezado = nd.idNominaEncabezado
        INNER JOIN tblPercepcionDeduccion pd ON pd.idPercepcionDeduccion = nd.idPercepcionDeduccion
        INNER JOIN tblCuenta c ON c.idCuenta = pd.idCuenta
        WHERE ne.fechaFinalNominaEncabezado >= ?
          AND ne.fechaFinalNominaEncabezado <= ?
          AND ne.estadoNminaEncabezado IN ('Aprobada', 'Pagada')
          AND NOT EXISTS (SELECT 1 FROM tblCierreNominaEmpleado ce
                          WHERE ce.idNominaEncabezado = ne.idNominaEncabezado)
        GROUP BY pd.idCuenta, c.codigoCuenta, c.nombreCuenta, pd.efectoPercepcionDeduccion
        ORDER BY c.codigoCuenta";


            _SelectEmpleadosPorCerrar = @"
        SELECT
            ne.idNominaEncabezado, ne.idEmpleado,
            ne.salarioBaseNominaEncabezado,
            ne.totalPercepcionNominaEncabezado,
            ne.totalDeduccionNominaEncabezado,
            ne.totalNetoNominaEncabezado,
            COALESCE((SELECT SUM(ce.totalPercepcionCierreNominaEmpleado)
                      FROM tblCierreNominaEmpleado ce
                      INNER JOIN tblCierreNomina c ON c.idCierreNomina = ce.idCierreNomina
                      WHERE ce.idEmpleado = ne.idEmpleado
                        AND YEAR(c.fechaFinalCierreNomina) = ?), 0) AS acumuladoPercepcionPrevio,
            COALESCE((SELECT SUM(ce.totalDeduccionCierreNominaEmpleado)
                      FROM tblCierreNominaEmpleado ce
                      INNER JOIN tblCierreNomina c ON c.idCierreNomina = ce.idCierreNomina
                      WHERE ce.idEmpleado = ne.idEmpleado
                        AND YEAR(c.fechaFinalCierreNomina) = ?), 0) AS acumuladoDeduccionPrevio
        FROM tblNominaEncabezado ne
        WHERE ne.fechaFinalNominaEncabezado >= ?
          AND ne.fechaFinalNominaEncabezado <= ?
          AND ne.estadoNminaEncabezado IN ('Aprobada', 'Pagada')
          AND NOT EXISTS (SELECT 1 FROM tblCierreNominaEmpleado x
                          WHERE x.idNominaEncabezado = ne.idNominaEncabezado)";

            _InsertEmpleado = @"
        INSERT INTO tblCierreNominaEmpleado
            (idCierreNomina, idNominaEncabezado, idEmpleado, salarioBaseCierreNominaEmpleado,
             totalPercepcionCierreNominaEmpleado, totalDeduccionCierreNominaEmpleado, totalNetoCierreNominaEmpleado,
             acumuladoPercepcionAnioCierreNominaEmpleado, acumuladoDeduccionAnioCierreNominaEmpleado)
        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

            _SelectDetalleCierre = @"
        SELECT
            ce.idCierreNominaEmpleado, ce.idCierreNomina, ce.idNominaEncabezado, ce.idEmpleado,
            e.codigoEmpleado,
            CONCAT(e.nombresEmpleado, ' ', e.apellidosEmpleado) AS nombreEmpleado,
            ce.salarioBaseCierreNominaEmpleado,
            ce.totalPercepcionCierreNominaEmpleado,
            ce.totalDeduccionCierreNominaEmpleado,
            ce.totalNetoCierreNominaEmpleado,
            ce.acumuladoPercepcionAnioCierreNominaEmpleado,
            ce.acumuladoDeduccionAnioCierreNominaEmpleado,
            ce.created_at, ce.updated_at
        FROM tblCierreNominaEmpleado ce
        INNER JOIN tblEmpleado e ON e.idEmpleado = ce.idEmpleado
        WHERE ce.idCierreNomina = ?
        ORDER BY e.codigoEmpleado";

            _SelectNominaCerrada = "SELECT COUNT(*) FROM tblCierreNominaEmpleado WHERE idNominaEncabezado = ?";
        }

        // Guarda el cierre completo en UNA transacción: encabezado + histórico de cada empleado.
        // Devuelve el idCierreNomina generado. Si algo falla se revierte todo y se relanza la excepción.
        public int RrhhMetAgregar(ClsCierreNomina Entidad)
        {
            OdbcConnection Conexion = RrhhMetObtenerConexion();
            OdbcTransaction Transaccion = null;
            try
            {
                Conexion.Open();
                Transaccion = Conexion.BeginTransaction();

                // 1. Encabezado del cierre
                var Parametros = new List<OdbcParameter>();
                Parametros.Add(new OdbcParameter("p_periodo", Entidad.PeriodoCierreNomina));
                Parametros.Add(new OdbcParameter("p_fechaInicio", Entidad.FechaInicioCierreNomina.Date));
                Parametros.Add(new OdbcParameter("p_fechaFinal", Entidad.FechaFinalCierreNomina.Date));
                Parametros.Add(new OdbcParameter("p_cantidad", Entidad.CantidadEmpleadoCierreNomina));
                Parametros.Add(RrhhMetCrearParametroDecimal("p_totalPercepcion", Entidad.TotalPercepcionCierreNomina));
                Parametros.Add(RrhhMetCrearParametroDecimal("p_totalDeduccion", Entidad.TotalDeduccionCierreNomina));
                Parametros.Add(RrhhMetCrearParametroDecimal("p_totalNeto", Entidad.TotalNetoCierreNomina));
                Parametros.Add(RrhhMetCrearParametroDecimal("p_totalPatronal", Entidad.TotalPatronalCierreNomina));
                Parametros.Add(RrhhMetCrearParametroDecimal("p_totalDebe", Entidad.TotalDebeCierreNomina));
                Parametros.Add(RrhhMetCrearParametroDecimal("p_totalHaber", Entidad.TotalHaberCierreNomina));
                Parametros.Add(new OdbcParameter("p_documentoOrigen", Entidad.DocumentoOrigenCierreNomina));
                Parametros.Add(new OdbcParameter("p_codigoPoliza", Entidad.CodigoPolizaCierreNomina));
                Parametros.Add(new OdbcParameter("p_fechaPoliza", Entidad.FechaPolizaCierreNomina.Date));
                Parametros.Add(new OdbcParameter("p_estadoPoliza", Entidad.EstadoPolizaCierreNomina));
                Parametros.Add(new OdbcParameter("p_idUsuario", Entidad.IdUsuarioCierra));

                using (var Comando = new OdbcCommand(_Insert, Conexion, Transaccion))
                {
                    Comando.Parameters.AddRange(Parametros.ToArray());
                    Comando.ExecuteNonQuery();
                }

                int IdCierre;
                using (var Comando = new OdbcCommand("SELECT LAST_INSERT_ID()", Conexion, Transaccion))
                {
                    IdCierre = Convert.ToInt32(Comando.ExecuteScalar());
                }

                // 2. Empleados por cerrar, con su acumulado del año ya cerrado
                var TablaEmpleados = new DataTable();
                using (var Comando = new OdbcCommand(_SelectEmpleadosPorCerrar, Conexion, Transaccion))
                {
                    int Anio = Entidad.FechaFinalCierreNomina.Year;
                    Comando.Parameters.Add(new OdbcParameter("p_anioPercepcion", Anio));
                    Comando.Parameters.Add(new OdbcParameter("p_anioDeduccion", Anio));
                    Comando.Parameters.Add(new OdbcParameter("p_fechaInicio", Entidad.FechaInicioCierreNomina.Date));
                    Comando.Parameters.Add(new OdbcParameter("p_fechaFinal", Entidad.FechaFinalCierreNomina.Date));
                    using (var Lector = Comando.ExecuteReader())
                    {
                        TablaEmpleados.Load(Lector);
                    }
                }

                // 3. Histórico por empleado (el acumulado incluye el cierre actual)
                int Insertados = 0;
                foreach (DataRow Fila in TablaEmpleados.Rows)
                {
                    decimal Percepcion = Convert.ToDecimal(Fila["totalPercepcionNominaEncabezado"]);
                    decimal Deduccion = Convert.ToDecimal(Fila["totalDeduccionNominaEncabezado"]);

                    using (var Comando = new OdbcCommand(_InsertEmpleado, Conexion, Transaccion))
                    {
                        Comando.Parameters.Add(new OdbcParameter("p_idCierre", IdCierre));
                        Comando.Parameters.Add(new OdbcParameter("p_idNomina", Convert.ToInt32(Fila["idNominaEncabezado"])));
                        Comando.Parameters.Add(new OdbcParameter("p_idEmpleado", Convert.ToInt32(Fila["idEmpleado"])));
                        Comando.Parameters.Add(RrhhMetCrearParametroDecimal("p_salarioBase", Convert.ToDecimal(Fila["salarioBaseNominaEncabezado"])));
                        Comando.Parameters.Add(RrhhMetCrearParametroDecimal("p_totalPercepcion", Percepcion));
                        Comando.Parameters.Add(RrhhMetCrearParametroDecimal("p_totalDeduccion", Deduccion));
                        Comando.Parameters.Add(RrhhMetCrearParametroDecimal("p_totalNeto", Convert.ToDecimal(Fila["totalNetoNominaEncabezado"])));
                        Comando.Parameters.Add(RrhhMetCrearParametroDecimal("p_acumuladoPercepcion", Convert.ToDecimal(Fila["acumuladoPercepcionPrevio"]) + Percepcion));
                        Comando.Parameters.Add(RrhhMetCrearParametroDecimal("p_acumuladoDeduccion", Convert.ToDecimal(Fila["acumuladoDeduccionPrevio"]) + Deduccion));
                        Comando.ExecuteNonQuery();
                    }
                    Insertados++;
                }

                // Si la nómina cambió entre el previo y el cierre, no se guarda nada
                if (Insertados != Entidad.CantidadEmpleadoCierreNomina)
                    throw new InvalidOperationException("La cantidad de empleados a cerrar (" + Insertados +
                        ") no coincide con la esperada (" + Entidad.CantidadEmpleadoCierreNomina +
                        "). Se revierte el cierre.");

                Transaccion.Commit();
                return IdCierre;
            }
            catch (Exception Ex)
            {
                try
                {
                    var Texto = new System.Text.StringBuilder();
                    Texto.AppendLine(DateTime.Now.ToString());
                    Texto.AppendLine(Ex.ToString());

                    var OdbcEx = Ex as OdbcException;
                    if (OdbcEx != null)
                    {
                        foreach (OdbcError Error in OdbcEx.Errors)
                            Texto.AppendLine("SQLState=" + Error.SQLState + " | Native=" + Error.NativeError +
                                             " | Source=" + Error.Source + " | Msg=" + Error.Message);
                    }

                    Texto.AppendLine("IdUsuario=" + Entidad.IdUsuarioCierra +
                                     " | Estado=[" + Entidad.EstadoPolizaCierreNomina + "]" +
                                     " | Poliza=[" + Entidad.CodigoPolizaCierreNomina + "]" +
                                     " | Doc=[" + Entidad.DocumentoOrigenCierreNomina + "]");

                    System.IO.File.WriteAllText(
                        System.IO.Path.Combine(System.IO.Path.GetTempPath(), "cierre_nomina_error.txt"),
                        Texto.ToString());
                }
                catch (Exception) { }

                if (Transaccion != null)
                {
                    try { Transaccion.Rollback(); }
                    catch (Exception) { }
                }
                throw;
            }
            finally
            {
                RrhhMetDesconexion(Conexion);
            }
        }

        // El cierre es irreversible: las correcciones se harán con la póliza de ajuste (pendiente de definir).
        public int RrhhMetEditar(ClsCierreNomina Entidad)
        {
            throw new NotSupportedException("El cierre de nómina es irreversible y no se puede editar.");
        }

        public int RrhhMetRemover(ClsCierreNomina Entidad)
        {
            throw new NotSupportedException("El cierre de nómina es irreversible y no se puede eliminar.");
        }

        public IEnumerable<ClsCierreNomina> RrhhMetObtenerTodos()
        {
            var ListaCierres = new List<ClsCierreNomina>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Cierre = new ClsCierreNomina();
                Cierre.IdCierreNomina = Convert.ToInt32(Fila["idCierreNomina"]);
                Cierre.PeriodoCierreNomina = Fila["periodoCierreNomina"].ToString();
                Cierre.FechaInicioCierreNomina = Convert.ToDateTime(Fila["fechaInicioCierreNomina"]);
                Cierre.FechaFinalCierreNomina = Convert.ToDateTime(Fila["fechaFinalCierreNomina"]);
                Cierre.CantidadEmpleadoCierreNomina = Convert.ToInt32(Fila["cantidadEmpleadoCierreNomina"]);
                Cierre.TotalPercepcionCierreNomina = Convert.ToDecimal(Fila["totalPercepcionCierreNomina"]);
                Cierre.TotalDeduccionCierreNomina = Convert.ToDecimal(Fila["totalDeduccionCierreNomina"]);
                Cierre.TotalNetoCierreNomina = Convert.ToDecimal(Fila["totalNetoCierreNomina"]);
                Cierre.TotalPatronalCierreNomina = Convert.ToDecimal(Fila["totalPatronalCierreNomina"]);
                Cierre.TotalDebeCierreNomina = Convert.ToDecimal(Fila["totalDebeCierreNomina"]);
                Cierre.TotalHaberCierreNomina = Convert.ToDecimal(Fila["totalHaberCierreNomina"]);
                Cierre.DocumentoOrigenCierreNomina = Fila["documentoOrigenCierreNomina"].ToString();
                Cierre.CodigoPolizaCierreNomina = Fila["codigoPolizaCierreNomina"].ToString();
                Cierre.FechaPolizaCierreNomina = Convert.ToDateTime(Fila["fechaPolizaCierreNomina"]);
                Cierre.EstadoPolizaCierreNomina = Fila["estadoPolizaCierreNomina"].ToString();
                Cierre.IdUsuarioCierra = Convert.ToInt32(Fila["idUsuarioCierra"]);
                Cierre.FechaEjecucionCierreNomina = Convert.ToDateTime(Fila["fechaEjecucionCierreNomina"]);
                Cierre.CreatedAt = Convert.ToDateTime(Fila["created_at"]);
                Cierre.UpdatedAt = Convert.ToDateTime(Fila["updated_at"]);
                ListaCierres.Add(Cierre);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaCierres;
        }

        public IEnumerable<ClsNominaAbierta> RrhhMetObtenerNominasAbiertas()
        {
            var ListaNominas = new List<ClsNominaAbierta>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectNominasAbiertas, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Nomina = new ClsNominaAbierta();
                Nomina.PeriodoNomina = Fila["periodoNomina"].ToString();
                Nomina.FechaInicioNomina = Convert.ToDateTime(Fila["fechaInicioNomina"]);
                Nomina.FechaFinalNomina = Convert.ToDateTime(Fila["fechaFinalNomina"]);
                Nomina.CantidadEmpleadoNomina = Convert.ToInt32(Fila["cantidadEmpleadoNomina"]);
                ListaNominas.Add(Nomina);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaNominas;
        }

        public IEnumerable<ClsPrevioCierreNomina> RrhhMetObtenerPrevio(DateTime FechaInicio, DateTime FechaFinal)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_fechaInicio", FechaInicio.Date));
            Parametros.Add(new OdbcParameter("p_fechaFinal", FechaFinal.Date));

            var ListaPrevio = new List<ClsPrevioCierreNomina>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectPrevio, CommandType.Text, Parametros);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Previo = new ClsPrevioCierreNomina();
                Previo.IdNominaEncabezado = Convert.ToInt32(Fila["idNominaEncabezado"]);
                Previo.IdEmpleado = Convert.ToInt32(Fila["idEmpleado"]);
                Previo.CodigoEmpleado = Fila["codigoEmpleado"].ToString();
                Previo.NombreEmpleado = Fila["nombreEmpleado"].ToString();
                Previo.SalarioBaseNominaEncabezado = Convert.ToDecimal(Fila["salarioBaseNominaEncabezado"]);
                Previo.TotalPercepcionNominaEncabezado = Convert.ToDecimal(Fila["totalPercepcionNominaEncabezado"]);
                Previo.TotalDeduccionNominaEncabezado = Convert.ToDecimal(Fila["totalDeduccionNominaEncabezado"]);
                Previo.TotalNetoNominaEncabezado = Convert.ToDecimal(Fila["totalNetoNominaEncabezado"]);
                Previo.DetallePercepcion = Convert.ToDecimal(Fila["detallePercepcion"]);
                Previo.DetalleDeduccion = Convert.ToDecimal(Fila["detalleDeduccion"]);
                Previo.ConceptosSinCuenta = Convert.ToInt32(Fila["conceptosSinCuenta"]);
                ListaPrevio.Add(Previo);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaPrevio;
        }

        public IEnumerable<ClsLineaPolizaNomina> RrhhMetObtenerLineasPoliza(DateTime FechaInicio, DateTime FechaFinal)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_fechaInicio", FechaInicio.Date));
            Parametros.Add(new OdbcParameter("p_fechaFinal", FechaFinal.Date));

            var ListaLineas = new List<ClsLineaPolizaNomina>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectLineasPoliza, CommandType.Text, Parametros);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Linea = new ClsLineaPolizaNomina();
                Linea.IdCuenta = Convert.ToInt32(Fila["idCuenta"]);
                Linea.CodigoCuenta = Fila["codigoCuenta"].ToString();
                Linea.NombreCuenta = Fila["nombreCuenta"].ToString();
                Linea.EsPercepcion = Convert.ToBoolean(Fila["esPercepcion"]);
                Linea.MontoTotal = Convert.ToDecimal(Fila["montoTotal"]);
                ListaLineas.Add(Linea);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaLineas;
        }

        public IEnumerable<ClsCierreNominaEmpleado> RrhhMetObtenerDetalleCierre(int IdCierreNomina)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_idCierre", IdCierreNomina));

            var ListaDetalle = new List<ClsCierreNominaEmpleado>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectDetalleCierre, CommandType.Text, Parametros);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Detalle = new ClsCierreNominaEmpleado();
                Detalle.IdCierreNominaEmpleado = Convert.ToInt32(Fila["idCierreNominaEmpleado"]);
                Detalle.IdCierreNomina = Convert.ToInt32(Fila["idCierreNomina"]);
                Detalle.IdNominaEncabezado = Convert.ToInt32(Fila["idNominaEncabezado"]);
                Detalle.IdEmpleado = Convert.ToInt32(Fila["idEmpleado"]);
                Detalle.CodigoEmpleado = Fila["codigoEmpleado"].ToString();
                Detalle.NombreEmpleado = Fila["nombreEmpleado"].ToString();
                Detalle.SalarioBaseCierreNominaEmpleado = Convert.ToDecimal(Fila["salarioBaseCierreNominaEmpleado"]);
                Detalle.TotalPercepcionCierreNominaEmpleado = Convert.ToDecimal(Fila["totalPercepcionCierreNominaEmpleado"]);
                Detalle.TotalDeduccionCierreNominaEmpleado = Convert.ToDecimal(Fila["totalDeduccionCierreNominaEmpleado"]);
                Detalle.TotalNetoCierreNominaEmpleado = Convert.ToDecimal(Fila["totalNetoCierreNominaEmpleado"]);
                Detalle.AcumuladoPercepcionAnioCierreNominaEmpleado = Convert.ToDecimal(Fila["acumuladoPercepcionAnioCierreNominaEmpleado"]);
                Detalle.AcumuladoDeduccionAnioCierreNominaEmpleado = Convert.ToDecimal(Fila["acumuladoDeduccionAnioCierreNominaEmpleado"]);
                Detalle.CreatedAt = Convert.ToDateTime(Fila["created_at"]);
                Detalle.UpdatedAt = Convert.ToDateTime(Fila["updated_at"]);
                ListaDetalle.Add(Detalle);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaDetalle;
        }

        public bool RrhhMetNominaCerrada(int IdNominaEncabezado)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_idNomina", IdNominaEncabezado));

            var TablaDatos = RrhhMetEjecucionConsulta(_SelectNominaCerrada, CommandType.Text, Parametros);
            return Convert.ToInt32(TablaDatos.Rows[0][0]) > 0;
        }

        private static OdbcParameter RrhhMetCrearParametroDecimal(string Nombre, decimal Valor)
        {
            var Parametro = new OdbcParameter(Nombre, OdbcType.VarChar, 30);
            Parametro.Value = Valor.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture);
            return Parametro;
        }
    }
}