using System;
using System.Data;
using CapaModelo_TransporteExterno_Logistica.Repositorio;

namespace CapaControlador_TransporteExterno_Logistica
{
    public class ClsControladorRutaSeguimiento
    {
        private readonly ClsRepositorioRutaSeguimiento _repositorio;

        public ClsControladorRutaSeguimiento()
        {
            _repositorio = new ClsRepositorioRutaSeguimiento();
        }

        public DataTable LogisticaFuncObtenerGuias()
        {
            return _repositorio.LogisticaFuncObtenerGuias();
        }

        public DataTable LogisticaFuncObtenerBodegas()
        {
            return _repositorio.LogisticaFuncObtenerBodegas();
        }

        public DataTable LogisticaFuncObtenerRuta(int idServicio)
        {
            return _repositorio.LogisticaFuncObtenerRuta(idServicio);
        }

        public DataTable LogisticaFuncObtenerUbicacion(int idServicio)
        {
            return _repositorio.LogisticaFuncObtenerUbicacion(idServicio);
        }

        public void LogisticaMetAgregarRuta(int idServicio, DataRow origen, DataRow destino)
        {
            if (idServicio <= 0) throw new Exception("Debe seleccionar una guía válida.");
            if (origen == null || destino == null) throw new Exception("Debe seleccionar origen y destino.");

            string nomOrigen = Convert.ToString(origen["Nombre"]);
            string dirOrigen = Convert.ToString(origen["Direccion"]);
            string nomDestino = Convert.ToString(destino["Nombre"]);
            string dirDestino = Convert.ToString(destino["Direccion"]);

            _repositorio.LogisticaMetGuardarRuta(idServicio, nomOrigen, dirOrigen, nomDestino, dirDestino);
        }

        public void LogisticaMetModificarRuta(int idServicio, DataRow origen, DataRow destino)
        {
            LogisticaMetAgregarRuta(idServicio, origen, destino);
        }

        public void LogisticaMetEliminarRuta(int idServicio)
        {
            if (idServicio <= 0) throw new Exception("Debe seleccionar una guía válida.");
            _repositorio.LogisticaMetEliminarRuta(idServicio);
        }
    }
}