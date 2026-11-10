using System;
using System.Collections.Generic;
using System.Data.Odbc;
using CapaModelo_Traslados_Logistica.Entidades;

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
        private readonly ClsRepositorioInventarioTraslado _Inventario;

        public ClsRepositorioViajeTraslado()
        {
            _Inventario = new ClsRepositorioInventarioTraslado();
        }

        public void LogisticaMetRegistrarSalida(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            ClsTrasladoPersistido Traslado, IList<ClsProductoTrasladoDetalle> Detalles)
        {
            if (Traslado.CodigoTransporte <= 0 ||
                LogisticaMetTieneServicioExterno(Conexion, Transaccion, Traslado.CodigoInterno))
            {
                return;
            }

            if (Traslado.CodigoConductor <= 0)
            {
                throw new InvalidOperationException(
                    "Debe seleccionar un conductor para registrar el viaje del traslado.");
            }

            LogisticaMetValidarConductorDisponible(
                Conexion, Transaccion, Traslado.CodigoConductor);
            decimal PesoTotal = 0;
            foreach (ClsProductoTrasladoDetalle Detalle in Detalles)
            {
                PesoTotal += Detalle.PesoTotal;
            }
            bool RequiereCadenaFrio =
                LogisticaMetRequiereCadenaFrio(Conexion, Transaccion, Traslado, Detalles);

            const string Consulta =
                "INSERT INTO tblEncabezadoViaje " +
                "(fechaDocumentoEncabezadoViaje, horaSalidaEncabezadoViaje, " +
                "requiereCadenaFrioEncabezadoViaje, estadoViajeEncabezadoViaje, " +
                "pesoTotalEncabezadoViaje, Fk_Transporte_Codigo, Fk_Conductor_Codigo) " +
                "VALUES (CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, ?, 'EN_CURSO', ?, ?, ?)";
            LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion, Consulta,
                LogisticaMetCrearParametro(
                    "RequiereCadenaFrio", OdbcType.Bit, RequiereCadenaFrio),
                LogisticaMetCrearParametro("Peso", OdbcType.Decimal, PesoTotal),
                LogisticaMetCrearParametro("Transporte", OdbcType.Int, Traslado.CodigoTransporte),
                LogisticaMetCrearParametro("Conductor", OdbcType.Int, Traslado.CodigoConductor));
            long CodigoViaje = LogisticaMetObtenerUltimoIdLargo(Conexion, Transaccion);

            int Orden = 1;
            foreach (ClsProductoTrasladoDetalle Detalle in Detalles)
            {
                string Descripcion = string.IsNullOrWhiteSpace(Detalle.Nombre)
                    ? "Producto " + Detalle.CodigoProducto
                    : Detalle.Nombre;
                LogisticaMetEjecutarNoConsulta(
                    Conexion, Transaccion,
                    "INSERT INTO tblDetalleViaje (Fk_Encabezado_Viaje_Codigo, ordenDetalleViaje, " +
                    "descripcionDetalleViaje, cantidadDetalleViaje, pesoDetalleViaje) " +
                    "VALUES (?, ?, ?, ?, ?)",
                    LogisticaMetCrearParametro("Viaje", OdbcType.BigInt, CodigoViaje),
                    LogisticaMetCrearParametro("Orden", OdbcType.Int, Orden),
                    LogisticaMetCrearParametro("Descripcion", OdbcType.VarChar, 255, Descripcion),
                    LogisticaMetCrearParametro("Cantidad", OdbcType.Decimal, Detalle.Cantidad),
                    LogisticaMetCrearParametro("Peso", OdbcType.Decimal, Detalle.PesoTotal));
                long CodigoDetalle = LogisticaMetObtenerUltimoIdLargo(Conexion, Transaccion);
                object Lote = _Inventario.LogisticaMetObtenerLoteProducto(
                    Conexion, Transaccion, Detalle.CodigoProducto);
                if (Lote != DBNull.Value && Lote != null)
                {
                    LogisticaMetEjecutarNoConsulta(
                        Conexion, Transaccion,
                        "INSERT INTO tblDetalleViajeLoteProducto " +
                        "(Fk_Detalle_Viaje_Codigo, Fk_Lote_Producto_Codigo, " +
                        "cantidadCajasDetalleViajeLoteProducto, pesoKgDetalleViajeLoteProducto) " +
                        "VALUES (?, ?, ?, ?)",
                        LogisticaMetCrearParametro("Detalle", OdbcType.BigInt, CodigoDetalle),
                        LogisticaMetCrearParametro("Lote", OdbcType.BigInt, Convert.ToInt64(Lote)),
                        LogisticaMetCrearParametro("Cajas", OdbcType.Int, Detalle.Cantidad),
                        LogisticaMetCrearParametro("Peso", OdbcType.Decimal, Detalle.PesoTotal));
                }

                Orden++;
            }
        }

        public void LogisticaMetRegistrarLlegada(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoTransporte)
        {
            LogisticaMetActualizarViajeActivo(
                Conexion, Transaccion, CodigoTransporte, "FINALIZADO", true);
        }

        public void LogisticaMetCancelarViaje(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoTransporte)
        {
            LogisticaMetActualizarViajeActivo(
                Conexion, Transaccion, CodigoTransporte, "CANCELADO", false);
        }

        private void LogisticaMetActualizarViajeActivo(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            int CodigoTransporte, string Estado, bool RegistrarLlegada)
        {
            if (CodigoTransporte <= 0)
            {
                return;
            }

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

        private bool LogisticaMetTieneServicioExterno(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoTraslado)
        {
            object Resultado = LogisticaMetEjecutarEscalar(
                Conexion, Transaccion,
                "SELECT 1 FROM tblServicioTransporteExterno WHERE Fk_Traslado_Codigo = ? LIMIT 1",
                LogisticaMetCrearParametro("Traslado", OdbcType.Int, CodigoTraslado));
            return Resultado != null;
        }

        private bool LogisticaMetRequiereCadenaFrio(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            ClsTrasladoPersistido Traslado, IList<ClsProductoTrasladoDetalle> Detalles)
        {
            object Refrigerado = LogisticaMetEjecutarEscalar(
                Conexion, Transaccion,
                "SELECT esRefrigeradoTransporte FROM tblTransporte WHERE idTransporte = ?",
                LogisticaMetCrearParametro("Transporte", OdbcType.Int, Traslado.CodigoTransporte));
            if (Refrigerado != null && Convert.ToBoolean(Refrigerado))
            {
                return true;
            }

            foreach (ClsProductoTrasladoDetalle Detalle in Detalles)
            {
                if (_Inventario.LogisticaMetProductoRequiereFrio(
                    Conexion, Transaccion, Detalle.CodigoProducto))
                {
                    return true;
                }
            }

            return false;
        }

        private void LogisticaMetValidarConductorDisponible(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoConductor)
        {
            object Resultado = LogisticaMetEjecutarEscalar(
                Conexion, Transaccion,
                "SELECT conductor.idConductor FROM tblConductor conductor " +
                "INNER JOIN tblEstado estado ON estado.idEstado = conductor.Fk_Estado_Codigo " +
                "WHERE conductor.idConductor = ? " +
                "AND UPPER(TRIM(estado.tipoEstado)) = 'DISPONIBLE' " +
                "AND NOT EXISTS (SELECT 1 FROM tblEncabezadoViaje viaje " +
                "WHERE viaje.Fk_Conductor_Codigo = conductor.idConductor " +
                "AND viaje.estadoViajeEncabezadoViaje = 'EN_CURSO') " +
                "FOR UPDATE",
                LogisticaMetCrearParametro("Conductor", OdbcType.Int, CodigoConductor));
            if (Resultado == null)
            {
                throw new InvalidOperationException(
                    "El conductor seleccionado no está disponible para el viaje.");
            }
        }
    }
}
