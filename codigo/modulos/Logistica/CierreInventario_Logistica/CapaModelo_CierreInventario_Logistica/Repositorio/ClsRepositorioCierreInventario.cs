using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_CierreInventario_Logistica.Contratos;
using CapaModelo_CierreInventario_Logistica.Entidad;
using CapaModelo_CierreInventario_Logistica.Validaciones;

namespace CapaModelo_CierreInventario_Logistica.Repositorio
{
    public class ClsRepositorioCierreInventario : IContratoCierreInventario
    {
        private readonly ClsConexion conexion = new ClsConexion();

        // ODBC utiliza ? y parámetros posicionales, no @nombre.
        private OdbcCommand Comando(OdbcConnection cn, OdbcTransaction tx,
            string sql, params object[] parametros)
        {
            OdbcCommand cmd = new OdbcCommand(sql, cn);
            cmd.Transaction = tx;
            cmd.CommandTimeout = 45;
            foreach (object valor in parametros)
                cmd.Parameters.AddWithValue("p", valor ?? DBNull.Value);
            return cmd;
        }

        private int Contar(OdbcConnection cn, OdbcTransaction tx,
            string sql, params object[] parametros)
        {
            using (OdbcCommand cmd = Comando(cn, tx, sql, parametros))
                return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private DataTable Consultar(OdbcConnection cn, OdbcTransaction tx,
            string sql, params object[] parametros)
        {
            using (OdbcCommand cmd = Comando(cn, tx, sql, parametros))
            using (OdbcDataReader reader = cmd.ExecuteReader())
            {
                DataTable tabla = new DataTable();
                tabla.Load(reader);
                return tabla;
            }
        }

        private void Ejecutar(OdbcConnection cn, OdbcTransaction tx,
            string sql, params object[] parametros)
        {
            using (OdbcCommand cmd = Comando(cn, tx, sql, parametros))
                cmd.ExecuteNonQuery();
        }

        // La base integrada puede tener idSucursal o Pk_Sucursal.
        // Solo se permite usar uno de estos dos nombres verificados.
        private string ColumnaSucursal(OdbcConnection cn, OdbcTransaction tx)
        {
            using (OdbcCommand cmd = Comando(cn, tx,
                @"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS
                  WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='tblSucursal'
                  AND COLUMN_NAME IN ('idSucursal','Pk_Sucursal')
                  ORDER BY CASE WHEN COLUMN_NAME='idSucursal' THEN 0 ELSE 1 END
                  LIMIT 1"))
            {
                object valor = cmd.ExecuteScalar();
                if (valor == null || valor == DBNull.Value)
                    throw new InvalidOperationException(
                        "No se encontró idSucursal ni Pk_Sucursal en tblSucursal. Revise la base configurada en el DSN.");
                return Convert.ToString(valor);
            }
        }

        public DataTable ObtenerSucursales()
        {
            using (OdbcConnection cn = conexion.AbrirConexion())
            {
                string id = ColumnaSucursal(cn, null);
                return Consultar(cn, null,
                    "SELECT " + id + " AS idSucursal, nombreSucursal FROM tblSucursal ORDER BY nombreSucursal");
            }
        }

        private DataTable ObtenerStock(OdbcConnection cn, OdbcTransaction tx,
            int sucursal, bool bloquear)
        {
            string sql = @"SELECT s.idStock, s.Fk_Bodega_Codigo AS IdBodega,
                s.Fk_Producto_Codigo AS IdProducto,
                b.nombreBodega AS Bodega, p.skuProducto AS Codigo,
                p.nombreProducto AS Producto,
                s.stockAnteriorStock AS Anterior, s.cargosMesStock AS Cargos,
                s.abonosMesStock AS Abonos, s.stockActualStock AS Actual,
                s.cargosAcumuladosStock AS CargosAcumulados,
                s.abonosAcumuladosStock AS AbonosAcumulados,
                (s.stockAnteriorStock+s.cargosMesStock-s.abonosMesStock) AS Calculado,
                (s.stockActualStock-(s.stockAnteriorStock+s.cargosMesStock-s.abonosMesStock)) AS Diferencia
                FROM tblStock s
                INNER JOIN tblBodega b ON b.idBodega=s.Fk_Bodega_Codigo
                   AND b.Fk_Sucursal_Codigo=s.Fk_Sucursal_Codigo
                INNER JOIN tblProducto p ON p.idProducto=s.Fk_Producto_Codigo
                WHERE s.Fk_Sucursal_Codigo=? ORDER BY s.idStock";
            if (bloquear) sql += " FOR UPDATE";
            return Consultar(cn, tx, sql, sucursal);
        }

        private int ContarMovimientos(OdbcConnection cn, OdbcTransaction tx,
            int sucursal, DateTime inicio, DateTime finExclusiva)
        {
            return Contar(cn, tx, @"SELECT COUNT(DISTINCT m.idMovimientoInventario)
                FROM tblMovimientoInventario m
                LEFT JOIN tblBodega bo ON bo.idBodega=m.Fk_Bodega_Origen_Codigo
                LEFT JOIN tblBodega bd ON bd.idBodega=m.Fk_Bodega_Destino_Codigo
                WHERE (bo.Fk_Sucursal_Codigo=? OR bd.Fk_Sucursal_Codigo=?)
                  AND m.fechaMovimientoInventario>=? AND m.fechaMovimientoInventario<?
                  AND m.estadoMovimientoInventario<>'ANULADO'",
                sucursal, sucursal, inicio, finExclusiva);
        }

        private string Validar(OdbcConnection cn, OdbcTransaction tx,
            int sucursal, DateTime inicio, DateTime fin, DataTable detalle)
        {
            if (!ClsValidacionCierreInventario.ValidarPeriodoFinalizado(
                inicio.Year, inicio.Month, DateTime.Today))
            {
                return "El mes todavía no ha finalizado.";
            }
            if (detalle.Rows.Count == 0)
                return "La sucursal no tiene existencias registradas.";

            DateTime? ultimo = null;
            using (OdbcCommand cmd = Comando(cn, tx,
                @"SELECT MAX(fechaFinCierreInventario) FROM tblCierreInventario
                  WHERE Fk_Sucursal_Codigo=? AND estadoCierreInventario='CERRADO'
                    AND esCierreAnualCierreInventario=0", sucursal))
            {
                object valor = cmd.ExecuteScalar();
                if (valor != null && valor != DBNull.Value)
                    ultimo = Convert.ToDateTime(valor);
            }
            if (ultimo.HasValue && inicio != ultimo.Value.Date.AddDays(1))
                return "Debe cerrar el mes siguiente al último cierre realizado.";

            if (Contar(cn, tx, @"SELECT COUNT(*) FROM tblCierreInventario
                  WHERE Fk_Sucursal_Codigo=? AND fechaFinCierreInventario=?
                    AND esCierreAnualCierreInventario=0", sucursal, fin) > 0)
                return "El cierre del período ya existe.";


            foreach (DataRow fila in detalle.Rows)
            {
                long anterior = Convert.ToInt64(fila["Anterior"]);
                long cargos = Convert.ToInt64(fila["Cargos"]);
                long abonos = Convert.ToInt64(fila["Abonos"]);
                long actual = Convert.ToInt64(fila["Actual"]);

                if (!ClsValidacionCierreInventario.ValidarCuadratura(
                    anterior, cargos, abonos, actual))
                {
                    return "El inventario contiene cantidades negativas " +
                           "o diferencias en los saldos.";
                }
            }


            // No cerrar meses anteriores cuando el stock ya incorporó movimientos nuevos.
            int posteriores = Contar(cn, tx, @"SELECT COUNT(*) FROM tblMovimientoInventario m
                LEFT JOIN tblBodega bo ON bo.idBodega=m.Fk_Bodega_Origen_Codigo
                LEFT JOIN tblBodega bd ON bd.idBodega=m.Fk_Bodega_Destino_Codigo
                WHERE (bo.Fk_Sucursal_Codigo=? OR bd.Fk_Sucursal_Codigo=?)
                  AND m.fechaMovimientoInventario>=?
                  AND m.estadoMovimientoInventario<>'ANULADO'",
                sucursal, sucursal, fin.AddDays(1));
            if (posteriores > 0)
                return "Existen movimientos posteriores al mes. No puede usarse el saldo actual para este cierre.";

            if (!ultimo.HasValue)
            {
                int previos = Contar(cn, tx, @"SELECT COUNT(*) FROM tblMovimientoInventario m
                    LEFT JOIN tblBodega bo ON bo.idBodega=m.Fk_Bodega_Origen_Codigo
                    LEFT JOIN tblBodega bd ON bd.idBodega=m.Fk_Bodega_Destino_Codigo
                    WHERE (bo.Fk_Sucursal_Codigo=? OR bd.Fk_Sucursal_Codigo=?)
                      AND m.fechaMovimientoInventario<?
                      AND m.estadoMovimientoInventario<>'ANULADO'",
                    sucursal, sucursal, inicio);
                if (previos > 0)
                    return "Se requiere establecer el saldo inicial: existen movimientos anteriores al primer cierre.";
            }

            int pendientes = Contar(cn, tx, @"SELECT COUNT(*) FROM tblMovimientoInventario m
                LEFT JOIN tblBodega bo ON bo.idBodega=m.Fk_Bodega_Origen_Codigo
                LEFT JOIN tblBodega bd ON bd.idBodega=m.Fk_Bodega_Destino_Codigo
                WHERE (bo.Fk_Sucursal_Codigo=? OR bd.Fk_Sucursal_Codigo=?)
                  AND m.fechaMovimientoInventario>=? AND m.fechaMovimientoInventario<?
                  AND m.estadoMovimientoInventario NOT IN ('CUADRADO','ANULADO')",
                sucursal, sucursal, inicio, fin.AddDays(1));
            if (pendientes > 0)
                return "Existen " + pendientes + " movimientos pendientes o ya cerrados en el período.";

            int intersucursal = Contar(cn, tx, @"SELECT COUNT(*) FROM tblMovimientoInventario m
                INNER JOIN tblBodega bo ON bo.idBodega=m.Fk_Bodega_Origen_Codigo
                INNER JOIN tblBodega bd ON bd.idBodega=m.Fk_Bodega_Destino_Codigo
                WHERE (bo.Fk_Sucursal_Codigo=? OR bd.Fk_Sucursal_Codigo=?)
                  AND bo.Fk_Sucursal_Codigo<>bd.Fk_Sucursal_Codigo
                  AND m.fechaMovimientoInventario>=? AND m.fechaMovimientoInventario<?
                  AND m.estadoMovimientoInventario<>'ANULADO'",
                sucursal, sucursal, inicio, fin.AddDays(1));
            if (intersucursal > 0)
                return "Existen movimientos entre sucursales. Se requiere coordinación de cierres.";

            int saldosInconsistentes = Contar(cn, tx, @"SELECT COUNT(*) FROM tblStock s
                LEFT JOIN tblSaldos a ON a.Fk_Bodega_Codigo=s.Fk_Bodega_Codigo
                    AND a.Fk_Producto_Codigo=s.Fk_Producto_Codigo
                WHERE s.Fk_Sucursal_Codigo=?
                  AND (a.idSaldo IS NULL OR a.saldoActualSaldo<>s.stockActualStock)", sucursal);
            if (saldosInconsistentes > 0)
                return "tblSaldos no coincide con tblStock. Revise la sincronización.";
            return null;
        }

        public ResultadoCierre VistaPrevia(int sucursal, int anio, int mes)
        {
            DateTime inicio = new DateTime(anio, mes, 1);
            DateTime fin = inicio.AddMonths(1).AddDays(-1);
            using (OdbcConnection cn = conexion.AbrirConexion())
            {
                DataTable detalle = ObtenerStock(cn, null, sucursal, false);
                string mensaje = Validar(cn, null, sucursal, inicio, fin, detalle);
                HashSet<int> productos = new HashSet<int>();
                HashSet<int> bodegas = new HashSet<int>();
                HashSet<int> diferencias = new HashSet<int>();
                foreach (DataRow fila in detalle.Rows)
                {
                    int producto = Convert.ToInt32(fila["IdProducto"]);
                    productos.Add(producto);
                    bodegas.Add(Convert.ToInt32(fila["IdBodega"]));
                    if (Convert.ToInt64(fila["Diferencia"]) != 0)
                        diferencias.Add(producto);
                }
                return new ResultadoCierre
                {
                    Detalle = detalle,
                    Mensaje = mensaje,
                    Productos = productos.Count,
                    Bodegas = bodegas.Count,
                    Movimientos = ContarMovimientos(cn, null, sucursal, inicio, fin.AddDays(1)),
                    Diferencias = diferencias.Count
                };
            }
        }

        public int EjecutarCierre(int sucursal, int anio, int mes, string nombre)
        {
            DateTime inicio = new DateTime(anio, mes, 1);
            DateTime fin = inicio.AddMonths(1).AddDays(-1);
            using (OdbcConnection cn = conexion.AbrirConexion())
            using (OdbcTransaction tx = cn.BeginTransaction(IsolationLevel.Serializable))
            {
                try
                {
                    // Bloquea la sucursal para coordinar los cierres simultáneos.
                    string idSucursal = ColumnaSucursal(cn, tx);
                    using (OdbcCommand bloqueo = Comando(cn, tx,
                        "SELECT " + idSucursal + " FROM tblSucursal WHERE " + idSucursal + "=? FOR UPDATE", sucursal))
                    {
                        if (bloqueo.ExecuteScalar() == null)
                            throw new InvalidOperationException("La sucursal seleccionada no existe.");
                    }
                    DataTable detalle = ObtenerStock(cn, tx, sucursal, true);
                    string mensaje = Validar(cn, tx, sucursal, inicio, fin, detalle);
                    if (mensaje != null)
                        throw new InvalidOperationException(mensaje);

                    Ejecutar(cn, tx, @"INSERT INTO tblCierreInventario
                        (nombreCierreInventario, Fk_Sucursal_Codigo,
                         fechaInicioCierreInventario, fechaFinCierreInventario,
                         estadoCierreInventario, esCierreAnualCierreInventario,
                         diferenciaUnidadesCierreInventario,
                         diferenciaMontoCierreInventario,
                         observacionesCierreInventario, fechaEjecucionCierreInventario)
                        VALUES (?, ?, ?, ?, 'EN_PROCESO', 0, 0, 0, '', NOW())",
                        nombre, sucursal, inicio, fin);
                    int id = Contar(cn, tx, "SELECT LAST_INSERT_ID()");

                    Ejecutar(cn, tx, @"INSERT INTO tblDetalleCierre
                        (Fk_Cierre_Inventario_Codigo, Fk_Stock_Codigo,
                         stockAnteriorDetalleCierre, stockActualDetalleCierre,
                         cargosMesDetalleCierre, abonosMesDetalleCierre,
                         cargosAcumuladosDetalleCierre, abonosAcumuladosDetalleCierre)
                        SELECT ?, idStock, stockAnteriorStock, stockActualStock,
                               cargosMesStock, abonosMesStock,
                               cargosAcumuladosStock, abonosAcumuladosStock
                        FROM tblStock WHERE Fk_Sucursal_Codigo=?", id, sucursal);

                    // Historial completo antes de reiniciar contadores mensuales.
                    Ejecutar(cn, tx, @"INSERT INTO tblHistoricoInventario
                        (Fk_Cierre_Inventario_Codigo, Fk_Stock_Codigo,
                         Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo,
                         stockFisicoStock, stockVirtualStock, stockAnteriorStock,
                         stockActualStock, cargosMesStock, abonosMesStock,
                         cargosAcumuladosStock, abonosAcumuladosStock,
                         apartadosStock, estadoVentaStock, estadoCompraStock,
                         fechaCorteHistoricoInventario)
                        SELECT ?, idStock, Fk_Sucursal_Codigo, Fk_Bodega_Codigo,
                               Fk_Producto_Codigo, stockFisicoStock, stockVirtualStock,
                               stockAnteriorStock, stockActualStock, cargosMesStock,
                               abonosMesStock, cargosAcumuladosStock,
                               abonosAcumuladosStock, apartadosStock,
                               estadoVentaStock, estadoCompraStock, ?
                        FROM tblStock WHERE Fk_Sucursal_Codigo=?", id, fin, sucursal);

                    // Un documento compartido entre sucursales fue bloqueado en Validar.
                    Ejecutar(cn, tx, @"UPDATE tblMovimientoInventario m
                        LEFT JOIN tblBodega bo ON bo.idBodega=m.Fk_Bodega_Origen_Codigo
                        LEFT JOIN tblBodega bd ON bd.idBodega=m.Fk_Bodega_Destino_Codigo
                        SET m.estadoMovimientoInventario='CERRADO',
                            m.Fk_Cierre_Inventario_Codigo=?
                        WHERE (bo.Fk_Sucursal_Codigo=? OR bd.Fk_Sucursal_Codigo=?)
                          AND m.fechaMovimientoInventario>=? AND m.fechaMovimientoInventario<?
                          AND m.estadoMovimientoInventario='CUADRADO'",
                        id, sucursal, sucursal, inicio, fin.AddDays(1));

                    Ejecutar(cn, tx, @"UPDATE tblStock SET stockAnteriorStock=stockActualStock,
                        cargosMesStock=0, abonosMesStock=0
                        WHERE Fk_Sucursal_Codigo=?", sucursal);

                    Ejecutar(cn, tx, @"UPDATE tblCierreInventario
                        SET estadoCierreInventario='CERRADO',
                            fechaEjecucionCierreInventario=NOW()
                        WHERE idCierreInventario=?", id);
                    tx.Commit();
                    return id;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public DataTable ConsultarCierres(int? sucursal, DateTime inicio,
            DateTime fin, string nombre)
        {
            using (OdbcConnection cn = conexion.AbrirConexion())
            {
                string idSucursal = ColumnaSucursal(cn, null);
                return Consultar(cn, null, @"SELECT c.idCierreInventario AS Codigo,
                    c.nombreCierreInventario AS Nombre, s.nombreSucursal AS Sucursal,
                    c.fechaInicioCierreInventario AS FechaInicio,
                    c.fechaFinCierreInventario AS FechaFin,
                    c.estadoCierreInventario AS Estado,
                    c.fechaEjecucionCierreInventario AS FechaEjecucion
                    FROM tblCierreInventario c
                    INNER JOIN tblSucursal s ON s." + idSucursal + @"=c.Fk_Sucursal_Codigo
                    WHERE (?=0 OR c.Fk_Sucursal_Codigo=?)
                      AND c.fechaFinCierreInventario>=?
                      AND c.fechaFinCierreInventario<?
                      AND c.nombreCierreInventario LIKE ?
                    ORDER BY c.fechaFinCierreInventario DESC",
                    sucursal ?? 0, sucursal ?? 0, inicio.Date, fin.Date.AddDays(1),
                    "%" + (nombre ?? "").Trim() + "%");
            }
        }
    }
}
