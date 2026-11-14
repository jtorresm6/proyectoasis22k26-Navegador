using System;
using System.Data.Odbc;

/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 05/11/2026
 * ==================================================================
 * Propósito :
 * Repositorio que registra la salida, llegada o cancelación del
 * viaje de transporte propio asociado a un traslado.
 * ===================================================================
 */

namespace CapaModelo_Traslados_Logistica.Repositorios
{
    public class ClsRepositorioViajeTraslado : ClsSentencias
    {
        public object LogisticaMetObtenerRefrigeracionTransporte(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoTransporte)
        {
            return LogisticaMetEjecutarEscalar(
                Conexion, Transaccion,
                "SELECT esRefrigeradoTransporte FROM tblTransporte WHERE idTransporte = ?",
                LogisticaMetCrearParametro("Transporte", OdbcType.Int, CodigoTransporte));
        }

        public long LogisticaMetInsertarEncabezadoViaje(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            bool RequiereCadenaFrio, double PesoTotal, int CodigoTransporte, int CodigoConductor)
        {
            const string Consulta =
                "INSERT INTO tblEncabezadoViaje " +
                "(fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, " +
                "requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, " +
                "pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo) " +
                "VALUES (CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, ?, 'EN_CURSO', ?, ?, ?)";
            LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion, Consulta,
                LogisticaMetCrearParametro("RequiereCadenaFrio", OdbcType.Bit, RequiereCadenaFrio),
                LogisticaMetCrearParametro("Peso", OdbcType.Double, PesoTotal),
                LogisticaMetCrearParametro("Transporte", OdbcType.Int, CodigoTransporte),
                LogisticaMetCrearParametro("Conductor", OdbcType.Int, CodigoConductor));
            return LogisticaMetObtenerUltimoIdLargo(Conexion, Transaccion);
        }

        public long LogisticaMetInsertarDetalleViaje(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            long CodigoViaje, int Orden, string Descripcion, int Cantidad, double PesoTotal)
        {
            LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion,
                "INSERT INTO tblDetalleViaje (Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, " +
                "descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje) VALUES (?, ?, ?, ?, ?)",
                LogisticaMetCrearParametro("Viaje", OdbcType.BigInt, CodigoViaje),
                LogisticaMetCrearParametro("Orden", OdbcType.Int, Orden),
                LogisticaMetCrearParametro("Descripcion", OdbcType.VarChar, 255, Descripcion),
                LogisticaMetCrearParametro("Cantidad", OdbcType.Decimal, Cantidad),
                LogisticaMetCrearParametro("Peso", OdbcType.Double, PesoTotal));
            return LogisticaMetObtenerUltimoIdLargo(Conexion, Transaccion);
        }

        public void LogisticaMetInsertarDetalleViajeLote(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            long CodigoDetalle, long CodigoLote, int Cantidad, double PesoTotal)
        {
            LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion,
                "INSERT INTO tblDetalleViajeLoteProducto " +
                "(Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, " +
                "cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto) " +
                "VALUES (?, ?, ?, ?)",
                LogisticaMetCrearParametro("Detalle", OdbcType.BigInt, CodigoDetalle),
                LogisticaMetCrearParametro("Lote", OdbcType.BigInt, CodigoLote),
                LogisticaMetCrearParametro("Cajas", OdbcType.Int, Cantidad),
                LogisticaMetCrearParametro("Peso", OdbcType.Double, PesoTotal));
        }

        public void LogisticaMetActualizarViajeActivo(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            int CodigoTransporte, string Estado, bool RegistrarLlegada)
        {
            string Consulta = RegistrarLlegada
                ? "UPDATE tblEncabezadoViaje SET horaLlegadaEncabezadoViaje = CURRENT_TIMESTAMP, " +
                    "estadoViajeEncabezadoViaje = ? WHERE Fk_Transporte_Codigo = ? " +
                    "AND estadoViajeEncabezadoViaje = 'EN_CURSO'"
                : "UPDATE tblEncabezadoViaje SET estadoViajeEncabezadoViaje = ? " +
                    "WHERE Fk_Transporte_Codigo = ? AND estadoViajeEncabezadoViaje = 'EN_CURSO'";
            LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion, Consulta,
                LogisticaMetCrearParametro("Estado", OdbcType.VarChar, 30, Estado),
                LogisticaMetCrearParametro("Transporte", OdbcType.Int, CodigoTransporte));
        }

    }
}
