using System;

namespace CapaControlador_Compras
{
    public sealed class ClsResumenIndicadoresCompras
    {
        public decimal AntiguedadPromedio { get; set; }
        public decimal PorcentajeAutorizadas { get; set; }
        public long PendientesRecepcion { get; set; }
        public decimal SaldoPorPagar { get; set; }
        public decimal SaldoVencido { get; set; }
        public decimal ProximosVencimientos { get; set; }
    }

    public sealed class ClsIndicadoresCompras
    {
        public ClsResumenIndicadoresCompras ComprasFuncConsultar()
        {
            using (var Datos = new CapaModelo_Compras.ClsIndicadoresCompras().ComprasFuncConsultar())
            {
                var Fila = Datos.Rows[0];
                return new ClsResumenIndicadoresCompras
                {
                    AntiguedadPromedio = Convert.ToDecimal(Fila["AntiguedadPromedio"]),
                    PorcentajeAutorizadas = Convert.ToDecimal(Fila["PorcentajeAutorizadas"]),
                    PendientesRecepcion = Convert.ToInt64(Fila["PendientesRecepcion"]),
                    SaldoPorPagar = Convert.ToDecimal(Fila["SaldoPorPagar"]),
                    SaldoVencido = Convert.ToDecimal(Fila["SaldoVencido"]),
                    ProximosVencimientos = Convert.ToDecimal(Fila["ProximosVencimientos"])
                };
            }
        }
    }
}
