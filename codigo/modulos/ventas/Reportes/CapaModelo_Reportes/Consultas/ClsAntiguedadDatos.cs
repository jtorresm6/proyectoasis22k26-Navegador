// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using System;
using System.Data;
using VentasDatos;

namespace CapaModelo_Reportes
{
    public sealed class ClsAntiguedadDatos
    {
        public DataTable VentasFuncClientes()
        {
            using (var Datos = new ClsDatosVentas())
                return Datos.VentasFuncTabla("SELECT Pk_Cliente idCliente, CONCAT(codigoCliente,' - ',nombreCliente) Cliente FROM tblCliente ORDER BY nombreCliente");
        }

        public DataTable VentasFuncConsultar(DateTime Corte, int Cliente)
        {
            // El libro conserva el saldo historico aunque hoy la factura ya este pagada o anulada.
            const string Consulta = @"SELECT c.nombreCliente Cliente,
                CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado) Documento,
                f.fechaVencimientoFacturaEncabezado Vencimiento, SUM(m.cargoMovimientoCxC-m.abonoMovimientoCxC) Saldo
                FROM tblMovimientoCxC m
                JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=m.Fk_FacturaEncabezado_Codigo
                JOIN tblClienteFacturaVersion cv ON cv.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo
                JOIN tblCliente c ON c.Pk_Cliente=cv.Fk_Cliente_Codigo
                JOIN tblMoneda mo ON mo.Pk_Moneda=f.Fk_Moneda_Codigo
                WHERE m.fechaMovimientoCxC<=? AND f.fechaEmisionFacturaEncabezado<=?
                  AND (?=0 OR c.Pk_Cliente=?) AND UPPER(mo.codigoMoneda)='GTQ'
                GROUP BY f.Pk_FacturaEncabezado,c.nombreCliente,f.serieFacturaEncabezado,f.numeroFacturaEncabezado,f.fechaVencimientoFacturaEncabezado
                HAVING Saldo>0
                ORDER BY c.nombreCliente,f.fechaVencimientoFacturaEncabezado,f.Pk_FacturaEncabezado";
            using (var Datos = new ClsDatosVentas())
                return Datos.VentasFuncTabla(Consulta, Corte.Date, Corte.Date, Cliente, Cliente);
        }
    }
}
