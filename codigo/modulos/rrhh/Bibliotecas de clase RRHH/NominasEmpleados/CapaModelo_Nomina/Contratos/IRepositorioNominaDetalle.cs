using CapaModelo_Nomina.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Contratos
{
    public interface IRepositorioNominaDetalle
    {
        // Inserta todas las lineas en una sola transaccion 
        int RrhhMetAgregarLote(List<ClsNominaDetalle> Lineas);

        IEnumerable<ClsNominaDetalle> RrhhMetObtenerPorNomina(int IdNominaEncabezado);

        int RrhhMetContarPorNomina(int IdNominaEncabezado);

        int? RrhhMetObtenerIdConceptoPorNombre(string NombreConcepto);
    }
}
