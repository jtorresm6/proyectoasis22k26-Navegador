// Autor: Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
// Persiste asignaciones, comisiones y abonos en transacciones del ERP.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using CapaModelo_ComisionVendedores.Contratos;
using CapaModelo_ComisionVendedores.Conexion;
using CapaModelo_ComisionVendedores.Entidades;

namespace CapaModelo_ComisionVendedores.Repositorios
{
    public partial class ClsRepositorioComision : ClsConexion, IRepositorioComision
    {
        public int VentasMetActualizarPorcentaje(string Metodo, int IdClasificacion, decimal Porcentaje)
        {
            if (IdClasificacion <= 0) throw new ArgumentOutOfRangeException("IdClasificacion", "Seleccione una clasificación válida.");
            if (Porcentaje < 0m || Porcentaje > 99m)
                throw new ArgumentOutOfRangeException("Porcentaje", "Ingrese una comisión entre 0.00% y 99.00%.");
            string Tabla, Columna, Id;
            switch (Metodo)
            {
                case "Producto": Tabla = "tblProducto"; Columna = "porcentajeComisionProducto"; Id = "idProducto"; break;
                case "Marca": Tabla = "tblMarca"; Columna = "porcentajeComisionMarca"; Id = "idMarca"; break;
                case "Linea": Tabla = "tblLinea"; Columna = "porcentajeComisionLinea"; Id = "idLinea"; break;
                case "Categoria": Tabla = "tblCategoria"; Columna = "porcentajeComisionCategoria"; Id = "idCategoria"; break;
                default: throw new ArgumentException("Seleccione Marca, Línea, Producto o Categoría.");
            }
            string Comando = "UPDATE " + Tabla + " SET " + Columna + " = ? WHERE " + Id + " = ?";
            using (OdbcConnection Conexion = VentasMetObtenerConexion())
            {
                VentasMetAbrirConexion(Conexion);
                using (OdbcCommand Actualizar = new OdbcCommand(Comando, Conexion))
                {
                    // El esquema almacena la tasa como fracción (1% = 0.01).
                    // Texto numérico invariante evita la conversión incorrecta SQL_C_NUMERIC del ODBC.
                    Actualizar.Parameters.Add(new OdbcParameter("Porcentaje", OdbcType.VarChar, 32)
                    { Value = (Porcentaje / 100m).ToString(CultureInfo.InvariantCulture) });
                    Actualizar.Parameters.Add(new OdbcParameter("IdClasificacion", OdbcType.Int) { Value = IdClasificacion });
                    int FilasAfectadas = Actualizar.ExecuteNonQuery();
                    if (FilasAfectadas == 0)
                    {
                        using (OdbcCommand Verificar = new OdbcCommand("SELECT COUNT(*) FROM " + Tabla + " WHERE " + Id + "=?", Conexion))
                        {
                            Verificar.Parameters.Add(new OdbcParameter("IdClasificacion", OdbcType.Int) { Value = IdClasificacion });
                            int Existe = Convert.ToInt32(Verificar.ExecuteScalar());
                            if (Existe == 0)
                                throw new InvalidOperationException("No se encontró el elemento seleccionado en " + Tabla + " (ID " + IdClasificacion + "). No se actualizó ningún porcentaje.");
                            // Un resultado de cero también puede significar que el valor ya era igual.
                            return Existe;
                        }
                    }
                    return FilasAfectadas;
                }
            }
        }

        public int VentasMetGuardarComisiones(List<ClsVentaVendedorBase> Comisiones, DateTime FechaInicio, DateTime FechaFin, string Metodo)
        {
            if (Comisiones == null || Comisiones.Count == 0) return 0;
            List<int> Ids = Comisiones.Where(Movimiento => Movimiento.IdComisionAbono > 0)
                .Select(Movimiento => Movimiento.IdComisionAbono).Distinct().ToList();
            if (Ids.Count == 0) return 0;
            int Guardados = 0;
            using (OdbcConnection Conexion = VentasMetObtenerConexion())
            {
                VentasMetAbrirConexion(Conexion);
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        foreach (int IdAbono in Ids)
                        {
                            using (OdbcCommand Actualizar = new OdbcCommand(@"
                                UPDATE tblComisionAbono
                                SET estadoPagoVendedorComisionAbono='PAGADO', fechaPagoVendedorComisionAbono=CURDATE()
                                WHERE idComisionAbono=? AND estadoPagoVendedorComisionAbono='PENDIENTE'", Conexion, Transaccion))
                            {
                                Actualizar.Parameters.Add(new OdbcParameter("IdAbono", OdbcType.Int) { Value = IdAbono });
                                Guardados += Actualizar.ExecuteNonQuery();
                            }
                        }
                        Transaccion.Commit();
                        return Guardados;
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        private string VentasMetObtenerIdClasificacionSql(string Metodo)
        {
            switch (Metodo)
            {
                case "Producto": return "p.idProducto";
                case "Marca": return "m.idMarca";
                case "Linea": return "l.idLinea";
                case "Categoria": return "c.idCategoria";
                default: throw new ArgumentException("Seleccione Línea, Producto, Categoría o Marca.");
            }
        }

        private static string VentasMetNormalizarMetodo(string Metodo)
        {
            if (String.IsNullOrWhiteSpace(Metodo)) return String.Empty;
            string Valor = Metodo.Trim().Replace('í', 'i').Replace('Í', 'I')
                .Replace('á', 'a').Replace('Á', 'A').Replace('é', 'e').Replace('É', 'E');
            switch (Valor.ToUpperInvariant())
            {
                case "PRODUCTO": return "Producto";
                case "MARCA": return "Marca";
                case "LINEA": return "Linea";
                case "CATEGORIA": return "Categoria";
                default: return String.Empty;
            }
        }

        private string VentasMetObtenerPorcentajeSql(string Metodo)
        {
            switch (Metodo)
            {
                case "Producto": return "p.porcentajeComisionProducto";
                case "Marca": return "m.porcentajeComisionMarca";
                case "Linea": return "l.porcentajeComisionLinea";
                case "Categoria": return "c.porcentajeComisionCategoria";
                default: throw new ArgumentException("Seleccione Línea, Producto, Categoría o Marca.");
            }
        }

        private ClsFacturaComision VentasMetMapearFactura(DataRow Fila)
        {
            return new ClsFacturaComision
            {
                IdFactura = Convert.ToInt32(Fila["id"]),
                IdVendedor = Fila.IsNull("id_vendedor") ? 0 : Convert.ToInt32(Fila["id_vendedor"]),
                Fecha = Convert.ToDateTime(Fila["fecha"]), Cliente = Convert.ToString(Fila["cliente"]),
                Total = Convert.ToDecimal(Fila["total"]), TotalPagado = Convert.ToDecimal(Fila["total_pagado"]),
                UltimoAbono = Convert.ToDecimal(Fila["ultimo_abono"])
            };
        }

        private List<ClsFacturaComision> VentasMetMapearFacturas(DataTable Datos)
        {
            List<ClsFacturaComision> Resultado = new List<ClsFacturaComision>();
            foreach (DataRow Fila in Datos.Rows) Resultado.Add(VentasMetMapearFactura(Fila));
            return Resultado;
        }

        public int VentasMetAsignarVendedorFactura(int IdFactura, int IdVendedor)
        {
            using (OdbcConnection Conexion = VentasMetObtenerConexion())
            {
                VentasMetAbrirConexion(Conexion);
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        using (OdbcCommand YaCalculada = new OdbcCommand(
                            "SELECT idVendedor FROM tblFacturaEncabezado WHERE idFacturaEncabezado = ?", Conexion, Transaccion))
                        {
                            YaCalculada.Parameters.Add(new OdbcParameter("IdFactura", OdbcType.Int) { Value = IdFactura });
                            object VendedorGuardado = YaCalculada.ExecuteScalar();
                            if (VendedorGuardado != null && VendedorGuardado != DBNull.Value)
                            {
                                if (Convert.ToInt32(VendedorGuardado) != IdVendedor)
                                    throw new InvalidOperationException("La comisión ya está guardada; no se puede cambiar el vendedor de esa factura.");
                                Transaccion.Commit();
                                return 1;
                            }
                        }
                        using (OdbcCommand Actualizar = new OdbcCommand(@"
                            UPDATE tblFacturaEncabezado SET idVendedor = ?
                            WHERE idFacturaEncabezado = ? AND estadoFacturaEncabezado = 'EMITIDA'", Conexion, Transaccion))
                        {
                            Actualizar.Parameters.Add(new OdbcParameter("IdVendedor", OdbcType.Int) { Value = IdVendedor });
                            Actualizar.Parameters.Add(new OdbcParameter("IdFactura", OdbcType.Int) { Value = IdFactura });
                            int Afectados = Actualizar.ExecuteNonQuery();
                            if (Afectados == 0)
                            {
                                using (OdbcCommand Confirmar = new OdbcCommand(
                                    "SELECT COUNT(*) FROM tblFacturaEncabezado WHERE idFacturaEncabezado=? AND idVendedor=? AND estadoFacturaEncabezado='EMITIDA'", Conexion, Transaccion))
                                {
                                    Confirmar.Parameters.Add(new OdbcParameter("IdFactura", OdbcType.Int) { Value = IdFactura });
                                    Confirmar.Parameters.Add(new OdbcParameter("IdVendedor", OdbcType.Int) { Value = IdVendedor });
                                    Afectados = Convert.ToInt32(Confirmar.ExecuteScalar());
                                }
                            }
                            if (Afectados == 0) throw new InvalidOperationException("No se pudo asignar: la factura no existe o no está emitida.");
                        }
                        Transaccion.Commit();
                        return 1;
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        public int VentasMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, string Metodo)
        {
            ClsFacturaComision FacturaAnterior = VentasMetObtenerFactura(IdFactura);
            if (FacturaAnterior == null) throw new InvalidOperationException("No se encontró la factura seleccionada.");
            List<ClsDetalleProductoComision> Lineas = VentasMetObtenerDetallesFactura(IdFactura, Metodo);
            foreach (ClsDetalleProductoComision Linea in Lineas) Linea.MetodoSeleccionado = Metodo;
            return VentasMetAsignarVendedorYGuardarComision(IdFactura, IdVendedor, Lineas);
        }

        public int VentasMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, List<ClsDetalleProductoComision> Selecciones)
        {
            if (Selecciones == null || Selecciones.Count == 0) throw new ArgumentException("Elija el método para cada producto antes de asignar.");
            List<ClsDetalleProductoComision> Lineas = new List<ClsDetalleProductoComision>();
            foreach (ClsDetalleProductoComision Seleccion in Selecciones)
            {
                string MetodoOriginal = Seleccion.MetodoSeleccionado;
                Seleccion.MetodoSeleccionado = VentasMetNormalizarMetodo(Seleccion.MetodoSeleccionado);
                if (String.IsNullOrWhiteSpace(Seleccion.MetodoSeleccionado))
                    throw new InvalidOperationException("Método no válido en el detalle " + Seleccion.IdFacturaDetalle + ": «" + (MetodoOriginal ?? "(vacío)") + "».");
                ClsDetalleProductoComision Linea = VentasMetObtenerDetallesFactura(IdFactura, Seleccion.MetodoSeleccionado)
                    .FirstOrDefault(Detalle => Detalle.IdFacturaDetalle == Seleccion.IdFacturaDetalle);
                if (Linea == null || Linea.IdClasificacionLogistica <= 0) throw new InvalidOperationException("No se encontró la clasificación/tasa para un producto de la factura.");
                Linea.MetodoSeleccionado = Seleccion.MetodoSeleccionado;
                Lineas.Add(Linea);
            }
            if (Lineas.Select(Linea => Linea.IdFacturaDetalle).Distinct().Count() != Lineas.Count) throw new InvalidOperationException("Hay renglones de factura duplicados.");
            using (OdbcConnection Conexion = VentasMetObtenerConexion())
            {
                VentasMetAbrirConexion(Conexion);
                string PasoActual = "iniciar transacción";
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        int VendedorActual = 0;
                        bool FacturaEncontrada = false;
                        using (OdbcCommand ConsultarAsignacion = new OdbcCommand(@"
                            SELECT COALESCE(f.idVendedor,0) AS vendedor_actual
                            FROM tblFacturaEncabezado f
                            WHERE f.idFacturaEncabezado=? FOR UPDATE", Conexion, Transaccion))
                        {
                            ConsultarAsignacion.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            using (OdbcDataReader Lector = ConsultarAsignacion.ExecuteReader())
                            {
                                if (Lector.Read())
                                {
                                    FacturaEncontrada = true;
                                    VendedorActual = Convert.ToInt32(Lector["vendedor_actual"]);
                                }
                            }
                        }
                        if (!FacturaEncontrada) throw new InvalidOperationException("La factura seleccionada ya no existe.");
                        if (VendedorActual > 0 && VendedorActual != IdVendedor)
                            throw new InvalidOperationException("Esta factura ya tiene asignado otro vendedor. Una factura solo puede pertenecer a un vendedor.");

                        PasoActual = "asignar vendedor a la factura";
                        using (OdbcCommand Asignar = new OdbcCommand("UPDATE tblFacturaEncabezado SET idVendedor=? WHERE idFacturaEncabezado=? AND estadoFacturaEncabezado='EMITIDA'", Conexion, Transaccion))
                        {
                            Asignar.Parameters.Add(new OdbcParameter("Vendedor", OdbcType.Int) { Value = IdVendedor });
                            Asignar.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            Asignar.ExecuteNonQuery();
                        }
                        PasoActual = "guardar encabezado y detalle de comisión por producto";
                        int IdComision = VentasMetGuardarComisionFacturaLineas(Conexion, Transaccion, IdFactura, IdVendedor, Lineas);
                        PasoActual = "sincronizar comisiones con los pagos aplicados";
                        VentasMetSincronizarAbonosInterno(Conexion, Transaccion, IdFactura);
                        Transaccion.Commit();
                        return IdComision;
                    }
                    catch (Exception ErrorOriginal)
                    {
                        // No permitir que un error secundario del rollback oculte el
                        // error SQL que hizo fallar la asignación de la comisión.
                        try { Transaccion.Rollback(); }
                        catch (Exception ErrorRollback)
                        {
                            throw new InvalidOperationException(
                                "Falló la asignación en el paso «" + PasoActual + "». Error original: " +
                                VentasMetDescribirError(ErrorOriginal) +
                                " Error al revertir la transacción: " + VentasMetDescribirError(ErrorRollback),
                                ErrorOriginal);
                        }
                        throw new InvalidOperationException(
                            "Falló la asignación en el paso «" + PasoActual + "». " +
                            VentasMetDescribirError(ErrorOriginal), ErrorOriginal);
                    }
                }
            }
        }

        private static string VentasMetDescribirError(Exception Error)
        {
            List<string> Detalles = new List<string>();
            for (Exception Actual = Error; Actual != null; Actual = Actual.InnerException)
            {
                string Mensaje = Actual.Message;
                Detalles.Add(String.IsNullOrWhiteSpace(Mensaje)
                    ? Actual.GetType().FullName
                    : Actual.GetType().FullName + ": " + Mensaje);
                OdbcException ErrorOdbc = Actual as OdbcException;
                if (ErrorOdbc != null)
                {
                    foreach (OdbcError ErrorDriver in ErrorOdbc.Errors)
                        Detalles.Add("ODBC SQLSTATE " + ErrorDriver.SQLState + " (" + ErrorDriver.NativeError + "): " + ErrorDriver.Message);
                    if (ErrorOdbc.Errors.Count == 0 && !String.IsNullOrWhiteSpace(ErrorOdbc.StackTrace))
                        Detalles.Add("Pila ODBC: " + ErrorOdbc.StackTrace);
                }
            }
            return String.Join(" -> ", Detalles);
        }

        public int VentasMetGuardarComisionFacturaPorProducto(int IdFactura, int IdVendedor, List<ClsDetalleProductoComision> Selecciones)
        {
            return VentasMetAsignarVendedorYGuardarComision(IdFactura, IdVendedor, Selecciones);
        }

        private int VentasMetGuardarComisionFacturaLineas(OdbcConnection Conexion, OdbcTransaction Transaccion, int IdFactura, int IdVendedor, List<ClsDetalleProductoComision> Lineas)
        {
            if (Lineas.Select(Item => Item.IdFacturaDetalle).Distinct().Count() != Lineas.Count)
                throw new InvalidOperationException("Hay productos repetidos en la selección de comisión.");
                        using (OdbcCommand Existente = new OdbcCommand("SELECT idComisionFacturaEncabezado,idFacturaEncabezado FROM tblComisionFacturaEncabezado WHERE idFacturaEncabezado=? AND estadoComisionFacturaEncabezado='CALCULADA' FOR UPDATE", Conexion, Transaccion))
                        {
                            Existente.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            using (OdbcDataReader Reader = Existente.ExecuteReader())
                            {
                                if (Reader.Read())
                                {
                                    int IdComisionExistente = Convert.ToInt32(Reader["idComisionFacturaEncabezado"]);
                                    Reader.Close();
                                    foreach (ClsDetalleProductoComision Linea in Lineas)
                                    {
                        if (!Linea.ComisionGuardada || !String.Equals(Linea.MetodoGuardado, Linea.MetodoSeleccionado, StringComparison.OrdinalIgnoreCase))
                                            throw new InvalidOperationException("La factura ya fue calculada con otros métodos. No se puede alterar su histórico.");
                                    }
                                    return IdComisionExistente;
                                }
                            }
                        }
                        using (OdbcCommand Verificar = new OdbcCommand("SELECT COUNT(*) FROM tblVendedor WHERE idVendedor=? AND activoVendedor=1", Conexion, Transaccion))
                        {
                            Verificar.Parameters.Add(new OdbcParameter("Vendedor", OdbcType.Int) { Value = IdVendedor });
                            if (Convert.ToInt32(Verificar.ExecuteScalar()) == 0) throw new InvalidOperationException("El vendedor no existe o está inactivo.");
                        }
                        int Conteo;
                        using (OdbcCommand Contar = new OdbcCommand(@"SELECT COUNT(*)
                            FROM tblFacturaDetalle fd
                            INNER JOIN tblFacturaEncabezado f ON f.uuidDteFacturaEncabezado=fd.idFacturaEncabezadoUuidDte
                            WHERE f.idFacturaEncabezado=?", Conexion, Transaccion))
                        {
                            Contar.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            Conteo = Convert.ToInt32(Contar.ExecuteScalar());
                        }
                        if (Conteo != Lineas.Count) throw new InvalidOperationException("Debe elegir un método para todos los productos de la factura.");
                        using (OdbcCommand Validar = new OdbcCommand(@"SELECT f.estadoFacturaEncabezado,f.idVendedor,
                            COALESCE((SELECT SUM(fd.totalLineaFacturaDetalle) FROM tblFacturaDetalle fd
                                      WHERE fd.idFacturaEncabezadoUuidDte=f.uuidDteFacturaEncabezado),0) AS total
                            FROM tblFacturaEncabezado f WHERE f.idFacturaEncabezado=? FOR UPDATE", Conexion, Transaccion))
                        {
                            Validar.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            using (OdbcDataReader Reader = Validar.ExecuteReader())
                            {
                                if (!Reader.Read() || !String.Equals(Convert.ToString(Reader["estadoFacturaEncabezado"]), "EMITIDA", StringComparison.OrdinalIgnoreCase)) throw new InvalidOperationException("Solo se puede comisionar una factura emitida.");
                                if (Reader.IsDBNull(Reader.GetOrdinal("idVendedor")) || Convert.ToInt32(Reader["idVendedor"]) != IdVendedor) throw new InvalidOperationException("El vendedor debe estar asignado a esta factura.");
                                if (Convert.ToDecimal(Reader["total"]) <= 0m) throw new InvalidOperationException("La factura debe tener total mayor que cero.");
                            }
                        }
                        decimal BaseTotal = Lineas.Sum(Item => Item.BaseComisionable);
                        decimal ComisionTotal = Lineas.Sum(Item => Math.Round(Item.BaseComisionable * Item.Porcentaje / 100m, 2, MidpointRounding.AwayFromZero));
                    ClsDetalleProductoComision Primera = Lineas[0];
                        string MetodoCabecera = Lineas.Select(Item => Item.MetodoSeleccionado.ToUpperInvariant()).Distinct().Count() == 1 ? Primera.MetodoSeleccionado.ToUpperInvariant() : "MIXTA";
                        using (OdbcCommand Insertar = new OdbcCommand(@"INSERT INTO tblComisionFacturaEncabezado
                            (idFacturaEncabezado,metodoComisionFacturaEncabezado,estadoComisionFacturaEncabezado)
                            VALUES (?,?,'CALCULADA')", Conexion, Transaccion))
                        {
                            Insertar.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            Insertar.Parameters.Add(new OdbcParameter("Metodo", OdbcType.VarChar) { Value = MetodoCabecera });
                            Insertar.ExecuteNonQuery();
                        }
                        int IdComision;
                        using (OdbcCommand ObtenerId = new OdbcCommand("SELECT LAST_INSERT_ID()", Conexion, Transaccion)) IdComision = Convert.ToInt32(ObtenerId.ExecuteScalar());
                        foreach (ClsDetalleProductoComision Linea in Lineas)
                        {
                            string ColumnaClasificacion = Linea.MetodoSeleccionado == "Producto" ? "" : Linea.MetodoSeleccionado == "Marca" ? ",idMarca" : Linea.MetodoSeleccionado == "Linea" ? ",idLinea" : ",idCategoria";
                            using (OdbcCommand InsertarLinea = new OdbcCommand(@"INSERT INTO tblComisionFacturaDetalle
                            (idComisionFacturaEncabezado,idFacturaDetalle,metodoComisionFacturaDetalle,nombreClasificacionHistoricaComisionFacturaDetalle,tasaAplicadaComisionFacturaDetalle,baseComisionFacturaDetalle" + ColumnaClasificacion + @")
                                VALUES (?,?,?,?,?,?" + (ColumnaClasificacion.Length == 0 ? "" : ",?") + ")", Conexion, Transaccion))
                            {
                                InsertarLinea.Parameters.Add(new OdbcParameter("Comision", OdbcType.Int) { Value = IdComision });
                                InsertarLinea.Parameters.Add(new OdbcParameter("Detalle", OdbcType.Int) { Value = Linea.IdFacturaDetalle });
                                InsertarLinea.Parameters.Add(new OdbcParameter("Metodo", OdbcType.VarChar) { Value = Linea.MetodoSeleccionado.ToUpperInvariant() });
                                InsertarLinea.Parameters.Add(new OdbcParameter("Nombre", OdbcType.VarChar, 100) { Value = Linea.ElementoComision });
                                // El controlador ODBC instalado falla al enlazar ciertos DECIMAL
                                // en INSERT. Enviarlos como texto numérico invariante permite que
                                // MySQL los convierta según la escala de las columnas DECIMAL.
                                InsertarLinea.Parameters.Add(new OdbcParameter("Porcentaje", OdbcType.VarChar, 32)
                                { Value = (Linea.Porcentaje / 100m).ToString(CultureInfo.InvariantCulture) });
                                InsertarLinea.Parameters.Add(new OdbcParameter("Base", OdbcType.VarChar, 32)
                                { Value = Linea.BaseComisionable.ToString(CultureInfo.InvariantCulture) });
                                if (ColumnaClasificacion.Length > 0) InsertarLinea.Parameters.Add(new OdbcParameter("Clasificacion", OdbcType.Int) { Value = Linea.IdClasificacionLogistica });
                                InsertarLinea.ExecuteNonQuery();
                            }
                        }
                        return IdComision;
        }

        public int VentasMetGuardarComisionFactura(int IdFactura, int IdVendedor, string Metodo)
        {
            List<ClsDetalleProductoComision> Lineas = VentasMetObtenerDetallesFactura(IdFactura, Metodo);
            foreach (ClsDetalleProductoComision Linea in Lineas) Linea.MetodoSeleccionado = Metodo;
            return VentasMetAsignarVendedorYGuardarComision(IdFactura, IdVendedor, Lineas);
        }

        public decimal VentasMetRegistrarAbonoFactura(int IdFactura, decimal Monto,
            int IdFormaPago, DateTime Fecha, string Referencia, int IdUsuario)
        {
            using (OdbcConnection Conexion = VentasMetObtenerConexion())
            {
                VentasMetAbrirConexion(Conexion);
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        int IdCliente;
                        long IdMoneda;
                        string SimboloMoneda;
                        decimal TotalFactura;
                        decimal SaldoFactura;
                        bool TieneMovimientosCxC;
                        string EstadoFactura;
                        using (OdbcCommand BloquearFactura = new OdbcCommand(@"
                            SELECT f.estadoFacturaEncabezado,f.idMoneda,COALESCE(m.simboloMoneda,''),
                                   cfv.idCliente,
                                   EXISTS(SELECT 1 FROM tblMovimientoCxC mc
                                          WHERE mc.idFacturaEncabezado=f.idFacturaEncabezado) AS tiene_movimientos_cxc,
                                   COALESCE((SELECT SUM(fd.totalLineaFacturaDetalle) FROM tblFacturaDetalle fd
                                        WHERE fd.idFacturaEncabezadoUuidDte=f.uuidDteFacturaEncabezado),0) AS total_factura,
                                   GREATEST(COALESCE(NULLIF(
                                       (SELECT SUM(mc.cargoMovimientoCxC-mc.abonoMovimientoCxC)
                                        FROM tblMovimientoCxC mc WHERE mc.idFacturaEncabezado=f.idFacturaEncabezado),0),
                                       COALESCE((SELECT SUM(fd.totalLineaFacturaDetalle) FROM tblFacturaDetalle fd
                                                  WHERE fd.idFacturaEncabezadoUuidDte=f.uuidDteFacturaEncabezado),0)
                                       - COALESCE((SELECT SUM(ap.montoAplicadoAplicacionPago) FROM tblAplicacionPago ap
                                                   INNER JOIN tblReciboPago rp ON rp.idReciboPago=ap.idReciboPago
                                                   WHERE ap.idFacturaEncabezado=f.idFacturaEncabezado
                                                     AND ap.estadoAplicacionPago='APLICADA' AND rp.estadoReciboPago='CONFIRMADO'),0)
                                   ),0) AS saldo_factura
                            FROM tblFacturaEncabezado f
                            INNER JOIN tblClienteFacturaVersion cfv ON cfv.idClienteFacturaVersion=f.idClienteFacturaVersion
                            INNER JOIN tblMoneda m ON m.idMoneda=f.idMoneda
                            WHERE f.idFacturaEncabezado=? FOR UPDATE", Conexion, Transaccion))
                        {
                            BloquearFactura.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            using (OdbcDataReader Lector = BloquearFactura.ExecuteReader())
                            {
                                if (!Lector.Read()) throw new InvalidOperationException("La factura no existe o no tiene datos de cliente.");
                                EstadoFactura = Convert.ToString(Lector["estadoFacturaEncabezado"]);
                                IdMoneda = Convert.ToInt64(Lector["idMoneda"]);
                                SimboloMoneda = Convert.ToString(Lector[2]);
                                IdCliente = Convert.ToInt32(Lector["idCliente"]);
                                TieneMovimientosCxC = Convert.ToInt32(Lector["tiene_movimientos_cxc"]) != 0;
                                TotalFactura = Convert.ToDecimal(Lector["total_factura"]);
                                SaldoFactura = Convert.ToDecimal(Lector["saldo_factura"]);
                            }
                        }
                        if (!String.Equals(EstadoFactura, "EMITIDA", StringComparison.OrdinalIgnoreCase))
                            throw new InvalidOperationException("Solo se reciben abonos de facturas emitidas.");
                        decimal Saldo = SaldoFactura;
                        if (TotalFactura <= 0m) throw new InvalidOperationException("La factura no tiene un total válido.");
                        if (Monto > Saldo) throw new InvalidOperationException("El abono excede el saldo pendiente de " + (SimboloMoneda ?? String.Empty) + " " + Saldo.ToString("N2", CultureInfo.GetCultureInfo("es-GT")) + ".");

                        bool RequiereReferencia;
                        using (OdbcCommand Forma = new OdbcCommand("SELECT requiereReferenciaFormaPagoVenta FROM tblFormaPagoVenta WHERE idFormaPagoVenta=?", Conexion, Transaccion))
                        {
                            Forma.Parameters.Add(new OdbcParameter("Forma", OdbcType.Int) { Value = IdFormaPago });
                            object Valor = Forma.ExecuteScalar();
                            if (Valor == null || Valor == DBNull.Value) throw new InvalidOperationException("El método de pago seleccionado ya no existe.");
                            RequiereReferencia = Convert.ToBoolean(Valor);
                        }
                        if (RequiereReferencia && String.IsNullOrWhiteSpace(Referencia))
                            throw new InvalidOperationException("El método de pago seleccionado requiere número de referencia.");

                        string NumeroRecibo = "CV" + DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) +
                            Guid.NewGuid().ToString("N").Substring(0, 6).ToUpperInvariant();
                        using (OdbcCommand Recibo = new OdbcCommand(@"
                            INSERT INTO tblReciboPago
                                (numeroReciboPago,idCliente,idMoneda,fechaReciboPago,montoReciboPago,
                                 referenciaReciboPago,estadoReciboPago,idUsuarioRegistro,idFormaPagoVenta)
                            VALUES (?,?,?,?,?,?,'CONFIRMADO',?,?)", Conexion, Transaccion))
                        {
                            Recibo.Parameters.Add(new OdbcParameter("Numero", OdbcType.VarChar, 30) { Value = NumeroRecibo });
                            Recibo.Parameters.Add(new OdbcParameter("Cliente", OdbcType.Int) { Value = IdCliente });
                            Recibo.Parameters.Add(new OdbcParameter("Moneda", OdbcType.BigInt) { Value = IdMoneda });
                            Recibo.Parameters.Add(new OdbcParameter("Fecha", OdbcType.Date) { Value = Fecha.Date });
                            Recibo.Parameters.Add(new OdbcParameter("Monto", OdbcType.VarChar, 32)
                            { Value = Monto.ToString(CultureInfo.InvariantCulture) });
                            Recibo.Parameters.Add(new OdbcParameter("Referencia", OdbcType.VarChar, 255)
                            { Value = String.IsNullOrWhiteSpace(Referencia) ? (object)DBNull.Value : Referencia.Trim() });
                            Recibo.Parameters.Add(new OdbcParameter("Usuario", OdbcType.Int) { Value = IdUsuario });
                            Recibo.Parameters.Add(new OdbcParameter("Forma", OdbcType.Int) { Value = IdFormaPago });
                            Recibo.ExecuteNonQuery();
                        }
                        int IdRecibo;
                        using (OdbcCommand ObtenerRecibo = new OdbcCommand("SELECT LAST_INSERT_ID()", Conexion, Transaccion))
                            IdRecibo = Convert.ToInt32(ObtenerRecibo.ExecuteScalar());

                        using (OdbcCommand Aplicacion = new OdbcCommand(@"
                            INSERT INTO tblAplicacionPago
                                (idReciboPago,idFacturaEncabezado,fechaAplicacionPago,
                                 montoAplicadoAplicacionPago,estadoAplicacionPago,idUsuarioRegistro)
                            VALUES (?,?,?,?,'APLICADA',?)", Conexion, Transaccion))
                        {
                            Aplicacion.Parameters.Add(new OdbcParameter("Recibo", OdbcType.Int) { Value = IdRecibo });
                            Aplicacion.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            Aplicacion.Parameters.Add(new OdbcParameter("Fecha", OdbcType.Date) { Value = Fecha.Date });
                            Aplicacion.Parameters.Add(new OdbcParameter("Monto", OdbcType.VarChar, 32)
                            { Value = Monto.ToString(CultureInfo.InvariantCulture) });
                            Aplicacion.Parameters.Add(new OdbcParameter("Usuario", OdbcType.Int) { Value = IdUsuario });
                            Aplicacion.ExecuteNonQuery();
                        }
                        int IdAplicacion;
                        using (OdbcCommand ObtenerAplicacion = new OdbcCommand("SELECT LAST_INSERT_ID()", Conexion, Transaccion))
                            IdAplicacion = Convert.ToInt32(ObtenerAplicacion.ExecuteScalar());

                        // Algunas facturas anteriores al libro CxC tienen pagos aplicados,
                        // pero aún no tienen movimientos. Abrir el saldo vigente en el libro
                        // para que este y los próximos abonos queden conciliados.
                        if (!TieneMovimientosCxC)
                        {
                            using (OdbcCommand SaldoInicial = new OdbcCommand(@"
                                INSERT INTO tblMovimientoCxC
                                    (idFacturaEncabezado,fechaMovimientoCxC,tipoMovimientoCxC,
                                     descripcionMovimientoCxC,cargoMovimientoCxC,abonoMovimientoCxC,
                                     idUsuarioRegistro,idAplicacionPago,idNotaCxC,
                                     idMovimientoCxCRevertido)
                                VALUES (?,?,'AJUSTE_POSITIVO',?,?,0,?,NULL,NULL,NULL)", Conexion, Transaccion))
                            {
                                SaldoInicial.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                                SaldoInicial.Parameters.Add(new OdbcParameter("Fecha", OdbcType.Date) { Value = Fecha.Date });
                                SaldoInicial.Parameters.Add(new OdbcParameter("Descripcion", OdbcType.VarChar, 255)
                                { Value = "Saldo inicial CxC al registrar " + NumeroRecibo });
                                SaldoInicial.Parameters.Add(new OdbcParameter("Cargo", OdbcType.VarChar, 32)
                                { Value = Saldo.ToString(CultureInfo.InvariantCulture) });
                                SaldoInicial.Parameters.Add(new OdbcParameter("Usuario", OdbcType.Int) { Value = IdUsuario });
                                SaldoInicial.ExecuteNonQuery();
                            }
                        }

                        using (OdbcCommand MovimientoCxC = new OdbcCommand(@"
                            INSERT INTO tblMovimientoCxC
                                (idFacturaEncabezado,fechaMovimientoCxC,tipoMovimientoCxC,
                                 descripcionMovimientoCxC,cargoMovimientoCxC,abonoMovimientoCxC,
                                 idUsuarioRegistro,idAplicacionPago,idNotaCxC,
                                 idMovimientoCxCRevertido)
                            VALUES (?,?,'PAGO',?,0,?,?,?,NULL,NULL)", Conexion, Transaccion))
                        {
                            MovimientoCxC.Parameters.Add(new OdbcParameter("Factura", OdbcType.Int) { Value = IdFactura });
                            MovimientoCxC.Parameters.Add(new OdbcParameter("Fecha", OdbcType.Date) { Value = Fecha.Date });
                            MovimientoCxC.Parameters.Add(new OdbcParameter("Descripcion", OdbcType.VarChar, 255)
                            { Value = "Abono de comisión " + NumeroRecibo });
                            MovimientoCxC.Parameters.Add(new OdbcParameter("Abono", OdbcType.VarChar, 32)
                            { Value = Monto.ToString(CultureInfo.InvariantCulture) });
                            MovimientoCxC.Parameters.Add(new OdbcParameter("Usuario", OdbcType.Int) { Value = IdUsuario });
                            MovimientoCxC.Parameters.Add(new OdbcParameter("Aplicacion", OdbcType.Int) { Value = IdAplicacion });
                            MovimientoCxC.ExecuteNonQuery();
                        }

                        VentasMetSincronizarAbonosInterno(Conexion, Transaccion, IdFactura);
                        decimal ComisionDevengada;
                        using (OdbcCommand ComisionPago = new OdbcCommand(@"
                            SELECT COALESCE(SUM(montoComisionDevengadoComisionAbono),0)
                            FROM tblComisionAbono WHERE idAplicacionPago=?
                              AND estadoPagoVendedorComisionAbono IN ('PENDIENTE','PAGADO')", Conexion, Transaccion))
                        {
                            ComisionPago.Parameters.Add(new OdbcParameter("Aplicacion", OdbcType.Int) { Value = IdAplicacion });
                            ComisionDevengada = Convert.ToDecimal(ComisionPago.ExecuteScalar());
                        }
                        Transaccion.Commit();
                        return ComisionDevengada;
                    }
                    catch (Exception ErrorOriginal)
                    {
                        try { Transaccion.Rollback(); }
                        catch (Exception ErrorRollback)
                        {
                            throw new InvalidOperationException("Falló el registro del abono: " +
                                VentasMetDescribirError(ErrorOriginal) + " Error al revertir: " +
                                VentasMetDescribirError(ErrorRollback), ErrorOriginal);
                        }
                        throw;
                    }
                }
            }
        }

        public void VentasMetSincronizarAbonos(int? IdFactura)
        {
            using (OdbcConnection Conexion = VentasMetObtenerConexion())
            {
                VentasMetAbrirConexion(Conexion);
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        VentasMetSincronizarAbonosInterno(Conexion, Transaccion, IdFactura);
                        Transaccion.Commit();
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        private void VentasMetSincronizarAbonosInterno(OdbcConnection Conexion, OdbcTransaction Transaccion, int? IdFactura)
        {
            // Algunas filas de prueba se guardaron con tasa/base en cero por el
            // enlace numérico ODBC anterior. Reparar únicamente comisiones totalmente
            // en cero y sin abonos; no tocar históricos que ya generaron pagos.
            List<int> ComisionesSinAbonoEnCero = new List<int>();
            using (OdbcCommand BuscarCeros = new OdbcCommand(@"
                SELECT cf.idComisionFacturaEncabezado
                FROM tblComisionFacturaEncabezado cf
                INNER JOIN tblComisionFacturaDetalle cd ON cd.idComisionFacturaEncabezado=cf.idComisionFacturaEncabezado
                WHERE cf.estadoComisionFacturaEncabezado='CALCULADA'
                  AND NOT EXISTS (SELECT 1 FROM tblComisionAbono ca WHERE ca.idComisionFacturaEncabezado=cf.idComisionFacturaEncabezado)
                GROUP BY cf.idComisionFacturaEncabezado
                HAVING COALESCE(SUM(cd.baseComisionFacturaDetalle*cd.tasaAplicadaComisionFacturaDetalle),0)=0", Conexion, Transaccion))
            using (OdbcDataReader LectorCeros = BuscarCeros.ExecuteReader())
            {
                while (LectorCeros.Read()) ComisionesSinAbonoEnCero.Add(Convert.ToInt32(LectorCeros[0]));
            }
            foreach (int IdComisionReparar in ComisionesSinAbonoEnCero)
            {
                using (OdbcCommand Reparar = new OdbcCommand(@"
                    UPDATE tblComisionFacturaDetalle cd
                    INNER JOIN tblComisionFacturaEncabezado cf ON cf.idComisionFacturaEncabezado=cd.idComisionFacturaEncabezado
                    INNER JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=cf.idFacturaEncabezado
                    INNER JOIN tblFacturaDetalle fd ON fd.idFacturaDetalle=cd.idFacturaDetalle
                    INNER JOIN tblProducto p ON p.idProducto=fd.idProducto
                    LEFT JOIN tblMarca m ON m.idMarca=p.Fk_Marca_Codigo
                    LEFT JOIN tblLinea l ON l.idLinea=p.Fk_Linea_Codigo
                    LEFT JOIN tblCategoria c ON c.idCategoria=p.Fk_Categoria_Codigo
                    SET cd.baseComisionFacturaDetalle=GREATEST(fd.totalLineaFacturaDetalle-COALESCE(fd.impuestoMontoFacturaDetalle,0),0),
                        cd.tasaAplicadaComisionFacturaDetalle=CASE UPPER(cd.metodoComisionFacturaDetalle)
                            WHEN 'PRODUCTO' THEN COALESCE(p.porcentajeComisionProducto,0)
                            WHEN 'MARCA' THEN COALESCE(m.porcentajeComisionMarca,0)
                            WHEN 'LINEA' THEN COALESCE(l.porcentajeComisionLinea,0)
                            WHEN 'CATEGORIA' THEN COALESCE(c.porcentajeComisionCategoria,0)
                            ELSE 0 END
                    WHERE cd.idComisionFacturaEncabezado=?
                      AND NOT EXISTS (SELECT 1 FROM tblComisionAbono ca WHERE ca.idComisionFacturaEncabezado=cf.idComisionFacturaEncabezado)", Conexion, Transaccion))
                {
                    Reparar.Parameters.Add(new OdbcParameter("Comision", OdbcType.Int) { Value = IdComisionReparar });
                    Reparar.ExecuteNonQuery();
                }
            }

            string Filtro = IdFactura.HasValue ? " AND f.idFacturaEncabezado = ?" : "";
            string SqlComisiones = @"
                SELECT cf.idComisionFacturaEncabezado, cf.idFacturaEncabezado,
                       COALESCE((SELECT SUM(cd.baseComisionFacturaDetalle * cd.tasaAplicadaComisionFacturaDetalle) FROM tblComisionFacturaDetalle cd
                         WHERE cd.idComisionFacturaEncabezado=cf.idComisionFacturaEncabezado),0) AS montoComisionTotal,
                       COALESCE((SELECT SUM(fd.totalLineaFacturaDetalle) FROM tblFacturaDetalle fd
                         WHERE fd.idFacturaEncabezadoUuidDte=f.uuidDteFacturaEncabezado),0) AS total
                FROM tblComisionFacturaEncabezado cf
                INNER JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=cf.idFacturaEncabezado
                WHERE cf.estadoComisionFacturaEncabezado='CALCULADA' AND f.estadoFacturaEncabezado='EMITIDA'" + Filtro + " ORDER BY cf.idComisionFacturaEncabezado FOR UPDATE";
            using (OdbcCommand BuscarComisiones = new OdbcCommand(SqlComisiones, Conexion, Transaccion))
            {
                if (IdFactura.HasValue) BuscarComisiones.Parameters.Add(new OdbcParameter("IdFactura", OdbcType.Int) { Value = IdFactura.Value });
                using (OdbcDataReader Comisiones = BuscarComisiones.ExecuteReader())
                {
                    List<int> IdComisiones = new List<int>();
                    List<int> IdFacturas = new List<int>();
                    List<decimal> TotalesFactura = new List<decimal>();
                    List<decimal> TotalesComision = new List<decimal>();
                    while (Comisiones.Read())
                    {
                        IdComisiones.Add(Convert.ToInt32(Comisiones["idComisionFacturaEncabezado"]));
                        IdFacturas.Add(Convert.ToInt32(Comisiones["idFacturaEncabezado"]));
                        TotalesComision.Add(Convert.ToDecimal(Comisiones["montoComisionTotal"]));
                        TotalesFactura.Add(Convert.ToDecimal(Comisiones["total"]));
                    }
                    Comisiones.Close();

                    for (int Indice = 0; Indice < IdComisiones.Count; Indice++)
                        VentasMetSincronizarAbonosComision(Conexion, Transaccion,
                            IdComisiones[Indice], IdFacturas[Indice], TotalesComision[Indice], TotalesFactura[Indice]);
                }
            }
        }

        private void VentasMetSincronizarAbonosComision(OdbcConnection Conexion, OdbcTransaction Transaccion,
            int IdComision, int IdFactura, decimal MontoComisionTotal, decimal TotalFactura)
        {
            const string PagosSql = @"
                SELECT ap.idAplicacionPago, ap.montoAplicadoAplicacionPago
                FROM tblAplicacionPago ap INNER JOIN tblReciboPago rp ON rp.idReciboPago=ap.idReciboPago
                WHERE ap.idFacturaEncabezado=? AND ap.estadoAplicacionPago='APLICADA'
                  AND rp.estadoReciboPago='CONFIRMADO' AND ap.montoAplicadoAplicacionPago > 0
                ORDER BY ap.fechaAplicacionPago, ap.idAplicacionPago";
            using (OdbcCommand Pagos = new OdbcCommand(PagosSql, Conexion, Transaccion))
            {
                Pagos.Parameters.Add(new OdbcParameter("IdFactura", OdbcType.Int) { Value = IdFactura });
                using (OdbcDataReader Lector = Pagos.ExecuteReader())
                {
                    List<int> IdAplicaciones = new List<int>();
                    List<decimal> MontosAplicados = new List<decimal>();
                    while (Lector.Read())
                    {
                        IdAplicaciones.Add(Convert.ToInt32(Lector["idAplicacionPago"]));
                        MontosAplicados.Add(Convert.ToDecimal(Lector["montoAplicadoAplicacionPago"]));
                    }
                    Lector.Close();

                    decimal AcumuladoAplicado = 0m;
                    decimal AcumuladoComision = 0m;
                    for (int Indice = 0; Indice < IdAplicaciones.Count; Indice++)
                    {
                        AcumuladoAplicado += MontosAplicados[Indice];
                        decimal Proporcion = TotalFactura <= 0m ? 0m : Math.Min(1m, AcumuladoAplicado / TotalFactura);
                        decimal ComisionAcumuladaObjetivo = Math.Round(MontoComisionTotal * Proporcion, 4, MidpointRounding.AwayFromZero);
                        using (OdbcCommand Existente = new OdbcCommand(@"
                            SELECT montoComisionDevengadoComisionAbono, estadoPagoVendedorComisionAbono
                            FROM tblComisionAbono WHERE idAplicacionPago=?", Conexion, Transaccion))
                        {
                            Existente.Parameters.Add(new OdbcParameter("IdAplicacion", OdbcType.Int) { Value = IdAplicaciones[Indice] });
                            using (OdbcDataReader Fila = Existente.ExecuteReader())
                            {
                                if (Fila.Read())
                                {
                                    if (!string.Equals(Convert.ToString(Fila["estadoPagoVendedorComisionAbono"]), "REVERSADO", StringComparison.OrdinalIgnoreCase))
                                        AcumuladoComision += Convert.ToDecimal(Fila["montoComisionDevengadoComisionAbono"]);
                                    continue;
                                }
                            }
                        }

                        decimal MontoAbonoComision = Math.Max(0m, ComisionAcumuladaObjetivo - AcumuladoComision);
                        if (MontoAbonoComision <= 0m) continue;
                        decimal PorcentajePagado = Math.Round(Proporcion, 4, MidpointRounding.AwayFromZero);
                        using (OdbcCommand Insertar = new OdbcCommand(@"
                            INSERT INTO tblComisionAbono
                                (idComisionFacturaEncabezado,idAplicacionPago,
                                 tasaFacturaPagadaAcumuladaComisionAbono,montoComisionDevengadoComisionAbono,estadoPagoVendedorComisionAbono)
                            VALUES (?,?,?,?,'PENDIENTE')", Conexion, Transaccion))
                        {
                            Insertar.Parameters.Add(new OdbcParameter("IdComision", OdbcType.Int) { Value = IdComision });
                            Insertar.Parameters.Add(new OdbcParameter("IdAplicacion", OdbcType.Int) { Value = IdAplicaciones[Indice] });
                                Insertar.Parameters.Add(new OdbcParameter("PorcentajePagado", OdbcType.Decimal) { Value = PorcentajePagado });
                                Insertar.Parameters.Add(new OdbcParameter("MontoComision", OdbcType.Decimal) { Value = MontoAbonoComision });
                            Insertar.ExecuteNonQuery();
                        }
                        AcumuladoComision += MontoAbonoComision;
                    }
                }
            }
        }
    }
}
