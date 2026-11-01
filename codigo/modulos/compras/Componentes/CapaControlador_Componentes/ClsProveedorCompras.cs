using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Miguel David Contreras Jacinto
 * Carné : 0901-21-3878
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Representa los datos del proveedor seleccionado para presentarlos en los formularios de Compras.
 * ===================================================================
*/

namespace CapaControlador_Componentes
{
    public sealed class ClsProveedorCompras
    {
        public int IdProveedor { get; }
        public string NombreProveedor { get; }
        public string NitProveedor { get; }
        public string TelefonoProveedor { get; }
        public string CorreoProveedor { get; }
        public string DireccionProveedor { get; }
        public bool EstadoProveedor { get; }
        public DateTime? FechaCreacion { get; }
        public DateTime? FechaActualizacion { get; }
        public IReadOnlyDictionary<string, object> Campos { get; }

        public ClsProveedorCompras(DataRow Fila)
        {
            if (Fila == null) throw new ArgumentNullException(nameof(Fila));
            var Valores = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            foreach (DataColumn Columna in Fila.Table.Columns)
                Valores.Add(Columna.ColumnName, Fila.IsNull(Columna) ? null : Fila[Columna]);
            Campos = new ReadOnlyDictionary<string, object>(Valores);
            IdProveedor = Convert.ToInt32(Fila["idProveedor"]);
            NombreProveedor = Convert.ToString(Fila["nombreProveedor"]);
            NitProveedor = Convert.ToString(Fila["nitProveedor"]);
            TelefonoProveedor = Fila.IsNull("telefonoProveedor") ? null : Convert.ToString(Fila["telefonoProveedor"]);
            CorreoProveedor = Fila.IsNull("correoProveedor") ? null : Convert.ToString(Fila["correoProveedor"]);
            DireccionProveedor = Fila.IsNull("direccionProveedor") ? null : Convert.ToString(Fila["direccionProveedor"]);
            EstadoProveedor = Convert.ToBoolean(Fila["estadoProveedor"]);
            FechaCreacion = Fila.IsNull("created_at") ? (DateTime?)null : Convert.ToDateTime(Fila["created_at"]);
            FechaActualizacion = Fila.IsNull("updated_at") ? (DateTime?)null : Convert.ToDateTime(Fila["updated_at"]);
        }
    }
}
