using System;
using System.Collections.Generic;
using System.Linq;
using CapaControlador_ListadoPrecios.Contratos.CapaControlador_ListadoPrecios;
using CapaModelo_ListadoPrecios;
using CapaModelo_ListadoPrecios.Entidades;

namespace CapaControlador_ListadoPrecios
{
    // Controlador del punto 2.1: validación, cálculo y coordinación con el modelo.
    public class ClsCtrlListadoPrecios : ICtrlListadoPrecios
    {
        private readonly ClsModeloListadoPrecios _Modelo;
        public string MensajeEstado { get { return _Modelo.MensajeEstado; } }
        public bool UsaBaseDeDatos { get { return _Modelo.UsaBaseDeDatos; } }

        public ClsCtrlListadoPrecios()
        {
            _Modelo = new ClsModeloListadoPrecios();
        }

        public List<ClsPrecioCliente> VentasFuncConsultarListado(string tipoCliente, string marca, string linea, string categoria, string proveedor, string producto)
        {
            return _Modelo.VentasFuncObtenerListado()
                .OrderBy(x => x.TipoCliente)
                .ThenBy(x => x.Producto)
                .ToList();
        }

        public decimal VentasFuncCalcularPrecioFinal(decimal precioBase, decimal valor, string tipoModificacion)
        {
            return ClsModeloListadoPrecios.VentasFuncCalcularPrecioFinal(precioBase, valor, tipoModificacion);
        }

        // Mantiene la firma existente para no romper otras llamadas del proyecto.
        public bool VentasFuncGuardar(string tipoCliente, string producto, decimal porcentaje, decimal precioBase, string tipoModificacion, decimal valor, string alcance, out string mensaje)
        {
            ClsPrecioCliente existente = _Modelo.VentasFuncObtenerListado()
                .FirstOrDefault(x => x.TipoCliente == tipoCliente && x.Producto == producto);
            if (existente == null)
            {
                mensaje = "No se encontró la lista de precios y el producto seleccionados.";
                return false;
            }

            existente.PrecioFinal = VentasFuncCalcularPrecioFinal(precioBase, valor, tipoModificacion);
            existente.PrecioBase = precioBase;
            existente.TipoModificacion = tipoModificacion;
            existente.Valor = valor;
            existente.AlcanceModificacion = alcance;
            existente.PorcentajeDescuento = tipoModificacion == "Descuento" ? valor : 0m;
            return VentasFuncGuardarVarios(new List<ClsPrecioCliente> { existente }, out mensaje);
        }

        // Recibe las filas reales seleccionadas en el formulario y las guarda juntas.
        public bool VentasFuncGuardarVarios(IList<ClsPrecioCliente> registros, out string mensaje)
        {
            if (registros == null || registros.Count == 0)
            {
                mensaje = "No hay productos seleccionados para guardar.";
                return false;
            }

            foreach (ClsPrecioCliente registro in registros)
            {
                if (registro == null || registro.IdProducto <= 0)
                {
                    mensaje = "Una de las filas no contiene un identificador de producto válido.";
                    return false;
                }
                if (registro.PrecioFinal < 0m)
                {
                    mensaje = "El precio final no puede ser negativo.";
                    return false;
                }
            }

            return _Modelo.VentasFuncGuardarVarios(registros, out mensaje);
        }
    }
}
