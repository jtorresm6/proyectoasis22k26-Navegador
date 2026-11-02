using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using CapaModelo_ComisionVendedores.Contratos;
using CapaModelo_ComisionVendedores.Entidades;

namespace CapaModelo_ComisionVendedores.Repositorios
{
    public class ClsRepositorioComision : ClsSentencias, IRepositorioComision
    {
        public List<ClsFacturaComision> VentasFuncObtenerFacturas()
        {
            string Comando = @"SELECT f.Pk_FacturaEncabezado id,f.fechaEmisionFacturaEncabezado fecha,c.nombreClienteFacturaVersion cliente,
                COALESCE((SELECT SUM(d.totalLineaFacturaDetalle) FROM tblFacturaDetalle d WHERE d.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado),0) total,
                COALESCE((SELECT SUM(a.montoAplicadoAplicacionPago) FROM tblAplicacionPago a WHERE a.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado AND a.estadoAplicacionPago='APLICADA'),0) total_pagado,
                COALESCE((SELECT a.montoAplicadoAplicacionPago FROM tblAplicacionPago a WHERE a.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado AND a.estadoAplicacionPago='APLICADA' ORDER BY a.fechaAplicacionPago DESC,a.Pk_AplicacionPago DESC LIMIT 1),0) ultimo_abono
                FROM tblFacturaEncabezado f JOIN tblClienteFacturaVersion c ON c.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo
                ORDER BY f.fechaEmisionFacturaEncabezado DESC,f.Pk_FacturaEncabezado DESC";
            DataTable Datos = VentasFuncEjecucionConsulta(Comando, new List<OdbcParameter>());
            List<ClsFacturaComision> Resultado = new List<ClsFacturaComision>();
            foreach (DataRow Fila in Datos.Rows)
                Resultado.Add(new ClsFacturaComision { IdFactura = Convert.ToInt32(Fila["id"]), Fecha = Convert.ToDateTime(Fila["fecha"]), Cliente = Convert.ToString(Fila["cliente"]), Total = Convert.ToDecimal(Fila["total"]), TotalPagado = Convert.ToDecimal(Fila["total_pagado"]), UltimoAbono = Convert.ToDecimal(Fila["ultimo_abono"]) });
            return Resultado;
        }

        public List<ClsVendedorComision> VentasFuncObtenerVendedores()
        {
            DataTable Datos = VentasFuncEjecucionConsulta(@"SELECT v.Pk_Vendedor id,CONCAT(e.nombresEmpleado,' ',e.apellidosEmpleado) nombre,
                COALESCE((SELECT r.tasaComisionReglaComision*100 FROM tblReglaComision r WHERE r.baseCalculoReglaComision='VENTA_NETA' AND r.vigenteDesdeReglaComision<=CURRENT_DATE() AND (r.vigenteHastaReglaComision IS NULL OR r.vigenteHastaReglaComision>=CURRENT_DATE()) ORDER BY r.vigenteDesdeReglaComision DESC LIMIT 1),0) porcentaje
                FROM tblVendedor v JOIN tblEmpleado e ON e.idEmpleado=v.Fk_Empleado_Codigo
                WHERE v.activoVendedor=1 ORDER BY e.nombresEmpleado,e.apellidosEmpleado", new List<OdbcParameter>());
            List<ClsVendedorComision> Resultado = new List<ClsVendedorComision>();
            foreach (DataRow Fila in Datos.Rows)
                Resultado.Add(new ClsVendedorComision { IdVendedor = Convert.ToInt32(Fila["id"]), NombreVendedor = Convert.ToString(Fila["nombre"]), Porcentaje = Convert.ToDecimal(Fila["porcentaje"]) });
            return Resultado;
        }

        public ClsFacturaComision VentasFuncObtenerFactura(int IdFactura)
        {
            string Comando = @"SELECT f.Pk_FacturaEncabezado id,f.fechaEmisionFacturaEncabezado fecha,c.nombreClienteFacturaVersion cliente,
                COALESCE((SELECT SUM(d.totalLineaFacturaDetalle) FROM tblFacturaDetalle d WHERE d.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado),0) total,
                COALESCE((SELECT SUM(a.montoAplicadoAplicacionPago) FROM tblAplicacionPago a WHERE a.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado AND a.estadoAplicacionPago='APLICADA'),0) total_pagado,
                COALESCE((SELECT a.montoAplicadoAplicacionPago FROM tblAplicacionPago a WHERE a.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado AND a.estadoAplicacionPago='APLICADA' ORDER BY a.fechaAplicacionPago DESC,a.Pk_AplicacionPago DESC LIMIT 1),0) ultimo_abono
                FROM tblFacturaEncabezado f JOIN tblClienteFacturaVersion c ON c.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo
                WHERE f.Pk_FacturaEncabezado=?";
            DataTable Datos = VentasFuncEjecucionConsulta(Comando,
                new List<OdbcParameter> { new OdbcParameter("IdFactura", OdbcType.Int) { Value = IdFactura } });
            if (Datos.Rows.Count == 0) return null;
            DataRow Fila = Datos.Rows[0];
            return new ClsFacturaComision { IdFactura = Convert.ToInt32(Fila["id"]), Fecha = Convert.ToDateTime(Fila["fecha"]), Cliente = Convert.ToString(Fila["cliente"]), Total = Convert.ToDecimal(Fila["total"]), TotalPagado = Convert.ToDecimal(Fila["total_pagado"]), UltimoAbono = Convert.ToDecimal(Fila["ultimo_abono"]) };
        }

        public ClsVendedorComision VentasFuncObtenerVendedor(int IdVendedor, DateTime Fecha)
        {
            DataTable Datos = VentasFuncEjecucionConsulta(@"SELECT v.Pk_Vendedor id,CONCAT(e.nombresEmpleado,' ',e.apellidosEmpleado) nombre,
                COALESCE((SELECT r.tasaComisionReglaComision*100 FROM tblReglaComision r WHERE r.baseCalculoReglaComision='VENTA_NETA' AND r.vigenteDesdeReglaComision<=? AND (r.vigenteHastaReglaComision IS NULL OR r.vigenteHastaReglaComision>=?) ORDER BY r.vigenteDesdeReglaComision DESC LIMIT 1),0) porcentaje
                FROM tblVendedor v JOIN tblEmpleado e ON e.idEmpleado=v.Fk_Empleado_Codigo WHERE v.Pk_Vendedor=?",
                new List<OdbcParameter> {
                    new OdbcParameter("FechaDesde", OdbcType.Date) { Value = Fecha.Date },
                    new OdbcParameter("FechaHasta", OdbcType.Date) { Value = Fecha.Date },
                    new OdbcParameter("IdVendedor", OdbcType.Int) { Value = IdVendedor }
                });
            if (Datos.Rows.Count == 0) return null;
            DataRow Fila = Datos.Rows[0];
            return new ClsVendedorComision { IdVendedor = Convert.ToInt32(Fila["id"]), NombreVendedor = Convert.ToString(Fila["nombre"]), Porcentaje = Convert.ToDecimal(Fila["porcentaje"]) };
        }

        public List<ClsDetalleProductoComision> VentasFuncObtenerDetallesFactura(int IdFactura, string Metodo)
        {
            string Comando = @"SELECT p.Pk_Producto id_producto,p.nombreProducto nombre,ma.nombreMarca marca,li.nombreLinea linea,ca.nombreCategoria categoria,
                df.cantidadFacturaDetalle cantidad,df.precioUnitarioFacturaDetalle precio_unitario,df.descuentoMontoFacturaDetalle descuento,
                CASE ? WHEN 'Marca' THEN ma.porcentajeComisionMarca WHEN 'Linea' THEN li.porcentajeComisionLinea WHEN 'Categoria' THEN ca.porcentajeComisionCategoria ELSE p.porcentajeComisionProducto END*100 porcentaje
                FROM tblFacturaDetalle df JOIN tblProducto p ON p.Pk_Producto=df.Fk_Producto_Codigo
                JOIN tblMarca ma ON ma.Pk_Marca=p.Fk_Marca_Codigo JOIN tblLinea li ON li.Pk_Linea=p.Fk_Linea_Codigo
                JOIN tblCategoria ca ON ca.Pk_Categoria=p.Fk_Categoria_Codigo
                WHERE df.Fk_FacturaEncabezado_Codigo=? ORDER BY p.nombreProducto";
            DataTable Datos = VentasFuncEjecucionConsulta(Comando,
                new List<OdbcParameter> {
                    new OdbcParameter("Metodo", OdbcType.VarChar, 20) { Value = Metodo },
                    new OdbcParameter("IdFactura", OdbcType.Int) { Value = IdFactura }
                });
            List<ClsDetalleProductoComision> Resultado = new List<ClsDetalleProductoComision>();
            foreach (DataRow Fila in Datos.Rows)
                Resultado.Add(new ClsDetalleProductoComision { IdProducto = Convert.ToInt32(Fila["id_producto"]), NombreProducto = Convert.ToString(Fila["nombre"]), Marca = Convert.ToString(Fila["marca"]), Linea = Convert.ToString(Fila["linea"]), Categoria = Convert.ToString(Fila["categoria"]), Cantidad = Convert.ToDecimal(Fila["cantidad"]), PrecioUnitario = Convert.ToDecimal(Fila["precio_unitario"]), Descuento = Convert.ToDecimal(Fila["descuento"]), Porcentaje = Convert.ToDecimal(Fila["porcentaje"]) });
            return Resultado;
        }

        public List<ClsVentaVendedorBase> VentasFuncObtenerVentasPorMetodo(
            string Metodo, DateTime FechaInicio, DateTime FechaFinExclusiva)
        {
            string Campo = "";
            string Porcentaje = "COALESCE(r.tasaComisionReglaComision,0)*100";
            string MontoBase = "SUM(COALESCE(t.total,0))";
            string EnlacesProducto = "";
            string Agrupacion = "v.Pk_Vendedor,e.nombresEmpleado,e.apellidosEmpleado,r.tasaComisionReglaComision";
            switch (Metodo)
            {
                case "Vendedor": Campo = "'Todos'"; break;
                case "Marca": Campo = "ma.nombreMarca"; Porcentaje = "ma.porcentajeComisionMarca*100"; EnlacesProducto = " INNER JOIN tblFacturaDetalle df ON df.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado INNER JOIN tblProducto p ON p.Pk_Producto=df.Fk_Producto_Codigo INNER JOIN tblMarca ma ON ma.Pk_Marca=p.Fk_Marca_Codigo"; MontoBase = "SUM(df.cantidadFacturaDetalle*df.precioUnitarioFacturaDetalle-df.descuentoMontoFacturaDetalle)"; Agrupacion += ",ma.Pk_Marca,ma.nombreMarca,ma.porcentajeComisionMarca"; break;
                case "Linea": Campo = "li.nombreLinea"; Porcentaje = "li.porcentajeComisionLinea*100"; EnlacesProducto = " INNER JOIN tblFacturaDetalle df ON df.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado INNER JOIN tblProducto p ON p.Pk_Producto=df.Fk_Producto_Codigo INNER JOIN tblLinea li ON li.Pk_Linea=p.Fk_Linea_Codigo"; MontoBase = "SUM(df.cantidadFacturaDetalle*df.precioUnitarioFacturaDetalle-df.descuentoMontoFacturaDetalle)"; Agrupacion += ",li.Pk_Linea,li.nombreLinea,li.porcentajeComisionLinea"; break;
                case "Producto": Campo = "p.nombreProducto"; Porcentaje = "p.porcentajeComisionProducto*100"; EnlacesProducto = " INNER JOIN tblFacturaDetalle df ON df.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado INNER JOIN tblProducto p ON p.Pk_Producto=df.Fk_Producto_Codigo"; MontoBase = "SUM(df.cantidadFacturaDetalle*df.precioUnitarioFacturaDetalle-df.descuentoMontoFacturaDetalle)"; Agrupacion += ",p.Pk_Producto,p.nombreProducto,p.porcentajeComisionProducto"; break;
                case "Categoria": Campo = "ca.nombreCategoria"; Porcentaje = "ca.porcentajeComisionCategoria*100"; EnlacesProducto = " INNER JOIN tblFacturaDetalle df ON df.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado INNER JOIN tblProducto p ON p.Pk_Producto=df.Fk_Producto_Codigo INNER JOIN tblCategoria ca ON ca.Pk_Categoria=p.Fk_Categoria_Codigo"; MontoBase = "SUM(df.cantidadFacturaDetalle*df.precioUnitarioFacturaDetalle-df.descuentoMontoFacturaDetalle)"; Agrupacion += ",ca.Pk_Categoria,ca.nombreCategoria,ca.porcentajeComisionCategoria"; break;
                default: throw new ArgumentException("Método de comisión no válido.");
            }
            string ComandoTexto = @"
                SELECT v.Pk_Vendedor AS id_vendedor,
                       CONCAT(e.nombresEmpleado,' ',e.apellidosEmpleado) AS nombre_vendedor,
                       " + Campo + @" AS elemento_comision,
                       " + Porcentaje + @" AS porcentaje_vendedor,
                       COALESCE(" + MontoBase + @", 0) AS base_ventas
                FROM tblVendedor v
                JOIN tblEmpleado e ON e.idEmpleado=v.Fk_Empleado_Codigo
                JOIN tblFacturaEncabezado f ON f.Fk_Vendedor_Codigo=v.Pk_Vendedor
                LEFT JOIN tblReglaComision r ON r.Pk_ReglaComision=(SELECT r2.Pk_ReglaComision FROM tblReglaComision r2 WHERE r2.baseCalculoReglaComision='VENTA_NETA' AND r2.vigenteDesdeReglaComision<=f.fechaEmisionFacturaEncabezado AND (r2.vigenteHastaReglaComision IS NULL OR r2.vigenteHastaReglaComision>=f.fechaEmisionFacturaEncabezado) ORDER BY r2.vigenteDesdeReglaComision DESC LIMIT 1)
                LEFT JOIN (SELECT Fk_FacturaEncabezado_Codigo,SUM(cantidadFacturaDetalle*precioUnitarioFacturaDetalle-descuentoMontoFacturaDetalle) total FROM tblFacturaDetalle GROUP BY Fk_FacturaEncabezado_Codigo) t ON t.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado" + EnlacesProducto + @"
                WHERE f.fechaEmisionFacturaEncabezado >= ? AND f.fechaEmisionFacturaEncabezado < ? AND f.estadoFacturaEncabezado='EMITIDA'
                GROUP BY " + Agrupacion + @"
                ORDER BY nombre_vendedor";

            List<OdbcParameter> Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("FechaInicio", OdbcType.DateTime) { Value = FechaInicio },
                new OdbcParameter("FechaFin", OdbcType.DateTime) { Value = FechaFinExclusiva }
            };

            DataTable TablaDatos = VentasFuncEjecucionConsulta(ComandoTexto, Parametros);
            List<ClsVentaVendedorBase> Resultados = new List<ClsVentaVendedorBase>();

            foreach (DataRow Fila in TablaDatos.Rows)
            {
                Resultados.Add(new ClsVentaVendedorBase
                {
                    IdVendedor = Convert.ToInt32(Fila["id_vendedor"]),
                    NombreVendedor = Convert.ToString(Fila["nombre_vendedor"]),
                    ElementoComision = Convert.ToString(Fila["elemento_comision"]),
                    PorcentajeVendedor = Convert.ToDecimal(Fila["porcentaje_vendedor"]),
                    BaseVentas = Convert.ToDecimal(Fila["base_ventas"])
                });
            }

            return Resultados;
        }

        public List<ClsProductoComision> VentasFuncConsultarPorcentajes(string Metodo)
        {
            string Comando = @"SELECT p.Pk_Producto id,p.nombreProducto nombre,ma.nombreMarca marca,li.nombreLinea linea,ca.nombreCategoria categoria,
                CASE ? WHEN 'Marca' THEN ma.porcentajeComisionMarca WHEN 'Linea' THEN li.porcentajeComisionLinea WHEN 'Categoria' THEN ca.porcentajeComisionCategoria ELSE p.porcentajeComisionProducto END*100 porcentaje
                FROM tblProducto p JOIN tblMarca ma ON ma.Pk_Marca=p.Fk_Marca_Codigo
                JOIN tblLinea li ON li.Pk_Linea=p.Fk_Linea_Codigo JOIN tblCategoria ca ON ca.Pk_Categoria=p.Fk_Categoria_Codigo
                ORDER BY p.nombreProducto";
            DataTable Datos = VentasFuncEjecucionConsulta(Comando, new List<OdbcParameter>
            {
                new OdbcParameter("Metodo", OdbcType.VarChar, 20) { Value = Metodo }
            });
            List<ClsProductoComision> Lista = new List<ClsProductoComision>();
            foreach (DataRow Fila in Datos.Rows)
                Lista.Add(new ClsProductoComision { IdProducto = Convert.ToInt32(Fila["id"]), NombreProducto = Convert.ToString(Fila["nombre"]), Marca = Convert.ToString(Fila["marca"]), Linea = Convert.ToString(Fila["linea"]), Categoria = Convert.ToString(Fila["categoria"]), Porcentaje = Convert.ToDecimal(Fila["porcentaje"]) });
            return Lista;
        }

        public int VentasFuncActualizarPorcentaje(string Metodo, List<int> IdsProducto, decimal Porcentaje)
        {
            if (IdsProducto == null || IdsProducto.Count == 0)
                return 0;

            string Marcadores = string.Join(", ", IdsProducto.ConvertAll(IdProducto => "?"));
            string ComandoTexto;
            switch (Metodo)
            {
                case "Producto":
                    ComandoTexto = "UPDATE tblProducto SET porcentajeComisionProducto=? WHERE Pk_Producto IN (" + Marcadores + ")";
                    break;
                case "Marca":
                    ComandoTexto = "UPDATE tblMarca SET porcentajeComisionMarca=? WHERE Pk_Marca IN (SELECT Fk_Marca_Codigo FROM tblProducto WHERE Pk_Producto IN (" + Marcadores + "))";
                    break;
                case "Linea":
                    ComandoTexto = "UPDATE tblLinea SET porcentajeComisionLinea=? WHERE Pk_Linea IN (SELECT Fk_Linea_Codigo FROM tblProducto WHERE Pk_Producto IN (" + Marcadores + "))";
                    break;
                case "Categoria":
                    ComandoTexto = "UPDATE tblCategoria SET porcentajeComisionCategoria=? WHERE Pk_Categoria IN (SELECT Fk_Categoria_Codigo FROM tblProducto WHERE Pk_Producto IN (" + Marcadores + "))";
                    break;
                default:
                    throw new ArgumentException("Método de comisión no válido.");
            }
            using (OdbcConnection Conexion = VentasFuncObtenerConexion())
            {
                Conexion.Open();
                using (OdbcCommand Comando = new OdbcCommand(ComandoTexto, Conexion))
                {
                    Comando.Parameters.Add(new OdbcParameter("Porcentaje", OdbcType.VarChar, 20) { Value = (Porcentaje / 100m).ToString(System.Globalization.CultureInfo.InvariantCulture) });
                    foreach (int IdProducto in IdsProducto)
                        Comando.Parameters.Add(new OdbcParameter("IdProducto", OdbcType.Int) { Value = IdProducto });
                    return Comando.ExecuteNonQuery();
                }
            }
        }

        public int VentasFuncGuardarComisiones(List<ClsComisionCalculada> Comisiones)
        {
            if (Comisiones == null || Comisiones.Count == 0)
                return 0;
            const string SqlLineas = @"SELECT f.Pk_FacturaEncabezado Factura,f.Fk_Moneda_Codigo Moneda,d.Pk_FacturaDetalle Detalle,
                d.cantidadFacturaDetalle*d.precioUnitarioFacturaDetalle-d.descuentoMontoFacturaDetalle Base,
                (SELECT r.tasaComisionReglaComision FROM tblReglaComision r WHERE r.baseCalculoReglaComision='VENTA_NETA' AND r.vigenteDesdeReglaComision<=f.fechaEmisionFacturaEncabezado AND (r.vigenteHastaReglaComision IS NULL OR r.vigenteHastaReglaComision>=f.fechaEmisionFacturaEncabezado) ORDER BY r.vigenteDesdeReglaComision DESC LIMIT 1) TasaVendedor,
                p.nombreProducto Producto,p.porcentajeComisionProducto TasaProducto,
                ma.Pk_Marca MarcaId,ma.nombreMarca Marca,ma.porcentajeComisionMarca TasaMarca,
                li.Pk_Linea LineaId,li.nombreLinea Linea,li.porcentajeComisionLinea TasaLinea,
                ca.Pk_Categoria CategoriaId,ca.nombreCategoria Categoria,ca.porcentajeComisionCategoria TasaCategoria
                FROM tblFacturaEncabezado f JOIN tblFacturaDetalle d ON d.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado
                JOIN tblProducto p ON p.Pk_Producto=d.Fk_Producto_Codigo
                JOIN tblMarca ma ON ma.Pk_Marca=p.Fk_Marca_Codigo JOIN tblLinea li ON li.Pk_Linea=p.Fk_Linea_Codigo
                JOIN tblCategoria ca ON ca.Pk_Categoria=p.Fk_Categoria_Codigo
                WHERE f.Fk_Vendedor_Codigo=? AND f.fechaEmisionFacturaEncabezado>=? AND f.fechaEmisionFacturaEncabezado<=?
                  AND f.estadoFacturaEncabezado='EMITIDA' ORDER BY f.Pk_FacturaEncabezado,d.Pk_FacturaDetalle";
            using (OdbcConnection ConexionActiva = VentasFuncObtenerConexion())
            {
                ConexionActiva.Open();
                using (OdbcTransaction Transaccion = ConexionActiva.BeginTransaction())
                {
                    try
                    {
                        foreach (ClsComisionCalculada Comision in Comisiones)
                        {
                            var Lineas = new DataTable();
                            using (var Comando = VentasFuncComando(ConexionActiva, Transaccion, SqlLineas,
                                Comision.IdVendedor, Comision.FechaInicio.Date, Comision.FechaFin.Date))
                            using (var Lector = Comando.ExecuteReader())
                                Lineas.Load(Lector);
                            if (Lineas.Rows.Count == 0)
                                throw new InvalidOperationException("No hay facturas emitidas para la comisión seleccionada.");
                            if (Lineas.AsEnumerable().Select(Fila => Convert.ToInt32(Fila["Moneda"])).Distinct().Count() != 1)
                                throw new InvalidOperationException("Calcule las comisiones por moneda; el período contiene facturas de varias monedas.");
                            if (Comision.Metodo == "Vendedor")
                                VentasMetGuardarComisionVendedor(ConexionActiva, Transaccion, Comision, Lineas);
                            else
                            {
                                if (Comision.Metodo != "Marca" && Comision.Metodo != "Linea" &&
                                    Comision.Metodo != "Producto" && Comision.Metodo != "Categoria")
                                    throw new ArgumentException("Método de comisión no válido.");
                                VentasMetGuardarComisionFactura(ConexionActiva, Transaccion, Comision.Metodo, Lineas);
                            }
                        }

                        Transaccion.Commit();
                        return Comisiones.Count;
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        private static OdbcCommand VentasFuncComando(OdbcConnection Conexion, OdbcTransaction Transaccion,
            string Sql, params object[] Valores)
        {
            var Comando = new OdbcCommand(Sql, Conexion, Transaccion);
            foreach (object Valor in Valores)
            {
                var Parametro = new OdbcParameter();
                if (Valor is decimal)
                {
                    Parametro.OdbcType = OdbcType.VarChar;
                    Parametro.Size = 64;
                    Parametro.Value = ((decimal)Valor).ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    if (Valor is DateTime) Parametro.OdbcType = OdbcType.Date;
                    Parametro.Value = Valor ?? DBNull.Value;
                }
                Comando.Parameters.Add(Parametro);
            }
            return Comando;
        }

        private static object VentasFuncEscalar(OdbcConnection Conexion, OdbcTransaction Transaccion,
            string Sql, params object[] Valores)
        {
            using (var Comando = VentasFuncComando(Conexion, Transaccion, Sql, Valores))
                return Comando.ExecuteScalar();
        }

        private static int VentasFuncEjecutar(OdbcConnection Conexion, OdbcTransaction Transaccion,
            string Sql, params object[] Valores)
        {
            using (var Comando = VentasFuncComando(Conexion, Transaccion, Sql, Valores))
                return Comando.ExecuteNonQuery();
        }

        private static void VentasMetGuardarComisionVendedor(OdbcConnection Conexion, OdbcTransaction Transaccion,
            ClsComisionCalculada Comision, DataTable Lineas)
        {
            const string BuscarRegla = @"SELECT Pk_ReglaComision FROM tblReglaComision
                WHERE baseCalculoReglaComision='VENTA_NETA' AND vigenteDesdeReglaComision<=?
                  AND (vigenteHastaReglaComision IS NULL OR vigenteHastaReglaComision>=?)
                ORDER BY vigenteDesdeReglaComision DESC LIMIT 1";
            int Regla;
            using (var Comando = VentasFuncComando(Conexion, Transaccion, BuscarRegla,
                Comision.FechaInicio.Date, Comision.FechaInicio.Date))
            using (var Lector = Comando.ExecuteReader())
            {
                if (!Lector.Read())
                    throw new InvalidOperationException("Configure una regla de comisión VENTA_NETA vigente al inicio del período.");
                Regla = Convert.ToInt32(Lector[0]);
            }
            int Moneda = Convert.ToInt32(Lineas.Rows[0]["Moneda"]);
            object Existente = VentasFuncEscalar(Conexion, Transaccion, @"SELECT Pk_ComisionEncabezado FROM tblComisionEncabezado
                WHERE Fk_Vendedor_Codigo=? AND Fk_ReglaComision_Codigo=? AND Fk_Moneda_Codigo=?
                  AND fechaDesdeComisionEncabezado=? AND fechaHastaComisionEncabezado=? AND estadoComisionEncabezado='BORRADOR'
                FOR UPDATE", Comision.IdVendedor, Regla, Moneda, Comision.FechaInicio.Date, Comision.FechaFin.Date);
            int Encabezado;
            if (Existente != null && Existente != DBNull.Value)
            {
                Encabezado = Convert.ToInt32(Existente);
                VentasFuncEjecutar(Conexion, Transaccion,
                    "DELETE FROM tblComisionDetalle WHERE Fk_ComisionEncabezado_Codigo=?", Encabezado);
            }
            else
            {
                string Numero = "COM-" + Guid.NewGuid().ToString("N").Substring(0, 20);
                VentasFuncEjecutar(Conexion, Transaccion, @"INSERT INTO tblComisionEncabezado
                    (numeroComisionEncabezado,Fk_Vendedor_Codigo,Fk_ReglaComision_Codigo,Fk_Moneda_Codigo,fechaDesdeComisionEncabezado,fechaHastaComisionEncabezado,estadoComisionEncabezado)
                    VALUES (?,?,?,?,?,?,'BORRADOR')", Numero, Comision.IdVendedor, Regla, Moneda,
                    Comision.FechaInicio.Date, Comision.FechaFin.Date);
                Encabezado = Convert.ToInt32(VentasFuncEscalar(Conexion, Transaccion, "SELECT LAST_INSERT_ID()"));
            }
            foreach (var Factura in Lineas.AsEnumerable().GroupBy(Fila => Convert.ToInt32(Fila["Factura"])))
            {
                decimal Base = Factura.Sum(Fila => Convert.ToDecimal(Fila["Base"], CultureInfo.InvariantCulture));
                if (Factura.First()["TasaVendedor"] == DBNull.Value)
                    throw new InvalidOperationException("Configure una regla VENTA_NETA vigente para cada factura del período.");
                decimal Tasa = Convert.ToDecimal(Factura.First()["TasaVendedor"], CultureInfo.InvariantCulture);
                VentasFuncEjecutar(Conexion, Transaccion, @"INSERT INTO tblComisionDetalle
                    (Fk_ComisionEncabezado_Codigo,Fk_FacturaEncabezado_Codigo,baseComisionDetalle,tasaAplicadaComisionDetalle)
                    VALUES (?,?,?,?)", Encabezado, Factura.Key, Base, Tasa);
            }
        }

        private static void VentasMetGuardarComisionFactura(OdbcConnection Conexion, OdbcTransaction Transaccion,
            string Metodo, DataTable Lineas)
        {
            string MetodoSql = Metodo.ToUpperInvariant();
            foreach (var Factura in Lineas.AsEnumerable().GroupBy(Fila => Convert.ToInt32(Fila["Factura"])))
            {
                object Existente = VentasFuncEscalar(Conexion, Transaccion,
                    "SELECT Pk_ComisionFacturaEncabezado FROM tblComisionFacturaEncabezado WHERE Fk_FacturaEncabezado_Codigo=? FOR UPDATE",
                    Factura.Key);
                if (Existente != null && Existente != DBNull.Value)
                    throw new InvalidOperationException("La factura " + Factura.Key + " ya tiene una comisión calculada.");
                VentasFuncEjecutar(Conexion, Transaccion, @"INSERT INTO tblComisionFacturaEncabezado
                    (Fk_FacturaEncabezado_Codigo,metodoComisionFacturaEncabezado) VALUES (?,?)", Factura.Key, MetodoSql);
                int Encabezado = Convert.ToInt32(VentasFuncEscalar(Conexion, Transaccion, "SELECT LAST_INSERT_ID()"));
                foreach (DataRow Linea in Factura)
                {
                    string Nombre = Convert.ToString(Linea[Metodo]);
                    decimal Tasa = Convert.ToDecimal(Linea["Tasa" + Metodo], CultureInfo.InvariantCulture);
                    object IdLinea = Metodo == "Linea" ? Linea["LineaId"] : DBNull.Value;
                    object IdCategoria = Metodo == "Categoria" ? Linea["CategoriaId"] : DBNull.Value;
                    object IdMarca = Metodo == "Marca" ? Linea["MarcaId"] : DBNull.Value;
                    VentasFuncEjecutar(Conexion, Transaccion, @"INSERT INTO tblComisionFacturaDetalle
                        (Fk_ComisionFacturaEncabezado_Codigo,Fk_FacturaDetalle_Codigo,metodoComisionFacturaDetalle,nombreClasificacionHistoricaComisionFacturaDetalle,tasaAplicadaComisionFacturaDetalle,baseComisionFacturaDetalle,Fk_Linea_Codigo,Fk_Categoria_Codigo,Fk_Marca_Codigo)
                        VALUES (?,?,?,?,?,?,?,?,?)", Encabezado, Linea["Detalle"], MetodoSql, Nombre, Tasa,
                        Convert.ToDecimal(Linea["Base"], CultureInfo.InvariantCulture), IdLinea, IdCategoria, IdMarca);
                }
            }
        }
    }
}
