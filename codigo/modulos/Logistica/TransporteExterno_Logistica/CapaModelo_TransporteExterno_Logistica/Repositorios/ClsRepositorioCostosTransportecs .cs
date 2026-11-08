// ------------------------------------------------------
// Módulo: Logística - Transporte Externo
// Clase: Repositorio de Costos de Transporte
// ------------------------------------------------------

using System;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_TransporteExterno_Logistica.Repositorio
{
    public class ClsRepositorioCostosTransporte : ClsRepositorio
    {
        // ------------------------------------------------------
        // OBTENER GUÍAS
        // ------------------------------------------------------
        public DataTable LogisticaMetObtenerGuias()
        {
            DataTable tabla = new DataTable();

            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                string consulta = @"
                    SELECT
                        idServicioTransporteExterno,
                        numeroGuiaServicioTransporteExterno
                    FROM tblServicioTransporteExterno
                    WHERE estadoServicioTransporteExterno <> 'Cancelado'
                    ORDER BY idServicioTransporteExterno DESC";

                using (OdbcDataAdapter adaptador =
                    new OdbcDataAdapter(consulta, conexion))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        // ------------------------------------------------------
        // OBTENER TARIFAS
        // ------------------------------------------------------
        public DataTable LogisticaMetObtenerTarifas()
        {
            DataTable tabla = new DataTable();

            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                string consulta = @"
                    SELECT
                        idTarifaTransporte,
                        nombreTarifaTransporte
                    FROM tblTarifaTransporte
                    ORDER BY idTarifaTransporte ASC";

                using (OdbcDataAdapter adaptador =
                    new OdbcDataAdapter(consulta, conexion))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        // ------------------------------------------------------
        // CONSULTAR COSTO POR GUÍA
        // ------------------------------------------------------
        public DataTable LogisticaMetObtenerCostoPorGuia(string numeroGuia)
        {
            DataTable tabla = new DataTable();

            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                string consulta = @"
                    SELECT
                        C.idCostoTransporte,
                        C.Fk_Servicio_Transporte_Externo_Codigo,
                        C.Fk_Tarifa_Transporte_Codigo,
                        C.cantidadCostoTransporte,
                        C.costoUnitarioCostoTransporte,
                        C.costoTotalCostoTransporte,
                        C.monedaCostoTransporte,
                        C.tipoPagoCostoTransporte,
                        T.nombreTarifaTransporte
                    FROM tblCostoTransporte C
                    INNER JOIN tblServicioTransporteExterno S
                        ON S.idServicioTransporteExterno =
                           C.Fk_Servicio_Transporte_Externo_Codigo
                    INNER JOIN tblTarifaTransporte T
                        ON T.idTarifaTransporte =
                           C.Fk_Tarifa_Transporte_Codigo
                    WHERE S.numeroGuiaServicioTransporteExterno = ?";

                using (OdbcCommand comando =
                    new OdbcCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@guia", numeroGuia);

                    using (OdbcDataAdapter adaptador =
                        new OdbcDataAdapter(comando))
                    {
                        adaptador.Fill(tabla);
                    }
                }
            }

            return tabla;
        }

        // ------------------------------------------------------
        // VERIFICAR SI EXISTE COSTO
        // ------------------------------------------------------
        public bool LogisticaMetExisteCostoPorGuia(string numeroGuia)
        {
            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                string consulta = @"
                    SELECT COUNT(*)
                    FROM tblCostoTransporte C
                    INNER JOIN tblServicioTransporteExterno S
                        ON S.idServicioTransporteExterno =
                           C.Fk_Servicio_Transporte_Externo_Codigo
                    WHERE S.numeroGuiaServicioTransporteExterno = ?";

                using (OdbcCommand comando =
                    new OdbcCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@guia", numeroGuia);

                    return Convert.ToInt32(
                        comando.ExecuteScalar()) > 0;
                }
            }
        }

        // ------------------------------------------------------
        // GUARDAR COSTO
        // ------------------------------------------------------
        public bool LogisticaMetGuardarCosto(
            string numeroGuia,
            int idTarifa,
            decimal cantidad,
            decimal costoUnitario,
            string moneda,
            string tipoPago)
        {
            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                string consulta = @"
                    INSERT INTO tblCostoTransporte
                    (
                        Fk_Servicio_Transporte_Externo_Codigo,
                        Fk_Tarifa_Transporte_Codigo,
                        cantidadCostoTransporte,
                        costoUnitarioCostoTransporte,
                        monedaCostoTransporte,
                        tipoPagoCostoTransporte
                    )
                    SELECT
                        S.idServicioTransporteExterno,
                        ?,
                        ?,
                        ?,
                        ?,
                        ?
                    FROM tblServicioTransporteExterno S
                    WHERE S.numeroGuiaServicioTransporteExterno = ?
                    AND EXISTS
                    (
                        SELECT 1
                        FROM tblTarifaTransporte T
                        WHERE T.idTarifaTransporte = ?
                    )
                    AND NOT EXISTS
                    (
                        SELECT 1
                        FROM tblCostoTransporte C
                        WHERE C.Fk_Servicio_Transporte_Externo_Codigo =
                              S.idServicioTransporteExterno
                    )";

                using (OdbcCommand comando =
                    new OdbcCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@tarifa", idTarifa);
                    comando.Parameters.AddWithValue("@cantidad", cantidad);
                    comando.Parameters.AddWithValue("@unitario", costoUnitario);
                    comando.Parameters.AddWithValue("@moneda", moneda);
                    comando.Parameters.AddWithValue("@tipoPago", tipoPago);
                    comando.Parameters.AddWithValue("@guia", numeroGuia);
                    comando.Parameters.AddWithValue("@tarifaExiste", idTarifa);

                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        // ------------------------------------------------------
        // MODIFICAR COSTO
        // ------------------------------------------------------
        public bool LogisticaMetModificarCosto(
            string numeroGuia,
            int idTarifa,
            decimal cantidad,
            decimal costoUnitario,
            string moneda,
            string tipoPago)
        {
            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                string consulta = @"
                    UPDATE tblCostoTransporte C
                    INNER JOIN tblServicioTransporteExterno S
                        ON S.idServicioTransporteExterno =
                           C.Fk_Servicio_Transporte_Externo_Codigo
                    SET
                        C.Fk_Tarifa_Transporte_Codigo = ?,
                        C.cantidadCostoTransporte = ?,
                        C.costoUnitarioCostoTransporte = ?,
                        C.monedaCostoTransporte = ?,
                        C.tipoPagoCostoTransporte = ?
                    WHERE S.numeroGuiaServicioTransporteExterno = ?
                    AND EXISTS
                    (
                        SELECT 1
                        FROM tblTarifaTransporte T
                        WHERE T.idTarifaTransporte = ?
                    )";

                using (OdbcCommand comando =
                    new OdbcCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@tarifa", idTarifa);
                    comando.Parameters.AddWithValue("@cantidad", cantidad);
                    comando.Parameters.AddWithValue("@unitario", costoUnitario);
                    comando.Parameters.AddWithValue("@moneda", moneda);
                    comando.Parameters.AddWithValue("@tipoPago", tipoPago);
                    comando.Parameters.AddWithValue("@guia", numeroGuia);
                    comando.Parameters.AddWithValue("@tarifaExiste", idTarifa);

                    int filasAfectadas = comando.ExecuteNonQuery();

                    // No se interpreta cero filas afectadas como error:
                    // MySQL puede devolver cero si los valores no cambiaron.
                }
            }

            return LogisticaMetExisteCostoPorGuia(numeroGuia);
        }

        // ------------------------------------------------------
        // ELIMINAR COSTO
        // ------------------------------------------------------
        public bool LogisticaMetEliminarCosto(string numeroGuia)
        {
            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            {
                if (conexion.State != ConnectionState.Open)
                    conexion.Open();

                string consulta = @"
                    DELETE C
                    FROM tblCostoTransporte C
                    INNER JOIN tblServicioTransporteExterno S
                        ON S.idServicioTransporteExterno =
                           C.Fk_Servicio_Transporte_Externo_Codigo
                    WHERE S.numeroGuiaServicioTransporteExterno = ?";

                using (OdbcCommand comando =
                    new OdbcCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@guia", numeroGuia);

                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}