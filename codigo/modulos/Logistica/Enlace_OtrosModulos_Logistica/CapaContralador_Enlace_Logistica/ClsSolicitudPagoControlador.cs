using System;
using System.Collections.Generic;
using System.Linq;
using CapaModelo_Enlace_Logistica.Contratos;
using CapaModelo_Enlace_Logistica.Entidades;
using CapaModelo_Enlace_Logistica.Repositorios;
namespace CapaContralador_Enlace_Logistica
{
    public class ClsSolicitudPagoControlador
    {
        private readonly ClsIRepositorioSolicitudPago _repositorio;
        public ClsSolicitudPagoControlador() : this(new ClsRepositorioSolicitudPago()) { }
        public ClsSolicitudPagoControlador(ClsIRepositorioSolicitudPago repositorio)
        {
            if (repositorio == null) throw new ArgumentNullException("repositorio");
            _repositorio = repositorio;
        }
        public List<ClsServicioTransporte> ObtenerServiciosDisponibles()
        {
            return _repositorio.ObtenerServiciosDisponibles().ToList();
        }
        public void ValidarServicio(ClsServicioTransporte servicio)
        {
            if (servicio == null || servicio.IdServicio <= 0)
                throw new ArgumentException("Seleccione un servicio válido.");
            if (servicio.Cantidad <= 0 || servicio.MontoTotal <= 0)
                throw new ArgumentException("El servicio debe tener cantidad y monto mayores que cero.");
            if (servicio.Moneda != "GTQ" && servicio.Moneda != "USD")
                throw new ArgumentException("La moneda del servicio no es válida.");
        }
    }
}
