namespace CapaControlador_ListadoPrecios_Ventas.Enumeraciones
{
    /// <summary>Estados informativos del módulo de Listado de Precios.</summary>
    namespace CapaModelo_ListadoPrecios_Ventas
    {
        public enum EnumEstadoListadoPrecios
        {
            Demostracion,
            BaseDeDatos
        }

        public enum EnumTipoModificacion
        {
            DescuentoPorcentaje,
            AumentoPorcentaje,
            AumentoPorBase
        }

        public enum EnumAlcanceModificacion
        {
            ProductoIndividual,
            ProductosSeleccionados,
            TodosResultados
        }
    }
}
