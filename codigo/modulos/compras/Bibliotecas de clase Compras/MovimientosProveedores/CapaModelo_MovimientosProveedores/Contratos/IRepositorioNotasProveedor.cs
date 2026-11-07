using System;
using System.Data;
using CapaModelo_MovimientosProveedores.Entidades;

namespace CapaModelo_MovimientosProveedores.Contratos
{
    /// <summary>Operaciones necesarias para registrar y consultar documentos de proveedor.</summary>
    public interface IRepositorioNotasProveedor
    {
        DataTable ComprasMetObtenerProveedores();
        DataTable ComprasMetObtenerConceptos();
        DataTable ComprasMetObtenerMonedas();
        decimal? ComprasMetObtenerTipoCambio(long IdMoneda, DateTime FechaEmision);
        DataTable ComprasMetConsultarNotas(int IdProveedor);
        void ComprasMetAgregar(ClsNotaProveedor Nota);
    }
}
