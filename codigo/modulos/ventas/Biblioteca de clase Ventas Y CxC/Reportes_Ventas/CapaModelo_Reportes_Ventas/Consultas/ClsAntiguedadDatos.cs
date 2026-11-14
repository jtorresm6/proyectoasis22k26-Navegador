// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using System;
using System.Data;
using VentasDatos;

namespace CapaModelo_Reportes_Ventas
{
    public sealed class ClsAntiguedadDatos
    {
        public DataTable VentasFuncClientes()
        {
            using (var Datos = new ClsDatosVentas())
                return Datos.VentasFuncTabla("SELECT idCliente idCliente, CONCAT(codigoCliente,' - ',nombreCliente) Cliente FROM tblCliente ORDER BY nombreCliente");
        }

        public DataTable VentasFuncVendedores()
        {
            using (var Datos = new ClsDatosVentas())
                return Datos.VentasFuncTabla("SELECT v.idVendedor Id, CONCAT(e.nombresEmpleado,' ',e.apellidosEmpleado) Nombre FROM tblVendedor v JOIN tblempleado e ON e.idEmpleado=v.idEmpleado WHERE v.activoVendedor=1");
        }
        public DataTable VentasFuncConsultar(DateTime Corte, int Cliente) { return VentasFuncConsultar(Corte, Cliente, 0, new DateTime(2000,1,1)); }
        public DataTable VentasFuncConsultar(DateTime Corte, int Cliente, int Vendedor, DateTime Desde)
        {
            // El libro conserva el saldo historico aunque hoy la factura ya este pagada o anulada.
            const string Consulta = @"SELECT c.nombreCliente Cliente,
                CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado) Documento,
                f.fechaEmisionFacturaEncabezado Emision, f.fechaVencimientoFacturaEncabezado Vencimiento, SUM(m.cargoMovimientoCxC-m.abonoMovimientoCxC) Saldo
                FROM tblMovimientoCxC m
                JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=m.idFacturaEncabezado
                JOIN tblClienteFacturaVersion cv ON cv.idClienteFacturaVersion=f.idClienteFacturaVersion
                JOIN tblCliente c ON c.idCliente=cv.idCliente
                JOIN tblMoneda mo ON mo.idMoneda=f.idMoneda
                WHERE m.fechaMovimientoCxC<=? AND f.fechaEmisionFacturaEncabezado<=?
                  AND (?=0 OR c.idCliente=?) AND UPPER(mo.codigoMoneda)='GTQ' AND (?=0 OR f.idVendedor=?) AND f.fechaEmisionFacturaEncabezado>=?
                GROUP BY f.idFacturaEncabezado,c.nombreCliente,f.serieFacturaEncabezado,f.numeroFacturaEncabezado,f.fechaVencimientoFacturaEncabezado,f.fechaEmisionFacturaEncabezado
                HAVING Saldo>0
                ORDER BY c.nombreCliente,f.fechaVencimientoFacturaEncabezado,f.idFacturaEncabezado";
            using (var Datos = new ClsDatosVentas())
                return Datos.VentasFuncTabla(Consulta, Corte.Date, Corte.Date, Cliente, Cliente, Vendedor, Vendedor, Desde.Date);
        }
    }
}
