using System;
using System.Collections.Generic;

// Modelos de la capa controlador de pólizas. La Vista (FrmPolizas) solo conoce estos tipos;
// no necesita referenciar la DLL de Contabilidad.
// Ajuste el namespace al de su proyecto de controlador de pólizas.
namespace CapaControlador_Polizas
{
    /// <summary>Póliza completa que la Vista entrega al controlador.</summary>
    public class ClsModeloPolizaCompras
    {
        public string DocumentoOrigen { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public List<ClsModeloLineaPolizaCompras> Lineas { get; set; } = new List<ClsModeloLineaPolizaCompras>();
    }

    /// <summary>Movimiento contable de una póliza.</summary>
    public class ClsModeloLineaPolizaCompras
    {
        public string CodigoCuenta { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public string Descripcion { get; set; }
    }

    /// <summary>Cuenta que Contabilidad permite usar en pólizas de compras.</summary>
    public class ClsModeloCuentaPolizaCompras
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Texto { get { return Codigo + " - " + Nombre; } }
    }
}