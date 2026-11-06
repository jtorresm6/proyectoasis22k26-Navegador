using System.Collections.Generic;
using CapaModelo_Enlace_Logistica.Entidades;
namespace CapaModelo_Enlace_Logistica.Contratos
{
    public interface ClsIRepositorioSolicitudPago
    {
        IEnumerable<ClsServicioTransporte> ObtenerServiciosDisponibles();
    }
}
