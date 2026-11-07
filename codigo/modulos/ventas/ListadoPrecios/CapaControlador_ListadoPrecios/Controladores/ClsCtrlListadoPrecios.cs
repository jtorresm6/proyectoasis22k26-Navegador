using System;
using System.Collections.Generic;
using System.Linq;
using CapaModelo_ListadoPrecios;
using CapaControlador_ListadoPrecios.Contratos;

namespace CapaControlador_ListadoPrecios
{
    // Controlador del punto 2.1: validación, cálculo y coordinación con el modelo.
    public class ClsCtrlListadoPrecios : ICtrlListadoPrecios
    {
        private readonly ClsModeloListadoPrecios _Modelo;
        public string MensajeEstado { get { return _Modelo.MensajeEstado; } }
        public bool UsaBaseDeDatos { get { return _Modelo.UsaBaseDeDatos; } }

        public ClsCtrlListadoPrecios() { _Modelo = new ClsModeloListadoPrecios(); }

        public List<ClsPrecioCliente> VentasFuncConsultarListado()
        {
            return _Modelo.VentasFuncObtenerListado().OrderBy(Elemento => Elemento.TipoCliente).ThenBy(Elemento => Elemento.TipoProducto).Select(Elemento => new ClsPrecioCliente { TipoCliente = Elemento.TipoCliente, TipoProducto = Elemento.TipoProducto, PorcentajeDescuento = Elemento.PorcentajeDescuento, PrecioBase = Elemento.PrecioBase, PrecioFinal = Elemento.PrecioFinal }).ToList();
        }

        public decimal VentasFuncCalcularPrecioFinal(decimal PrecioBase, decimal Descuento)
        {
            if (PrecioBase < 0) throw new ArgumentOutOfRangeException("precioBase", "El precio base no puede ser negativo.");
            if (Descuento < 0 || Descuento > 100) throw new ArgumentOutOfRangeException("descuento", "El descuento debe estar entre 0 y 100 %.");
            return ClsModeloListadoPrecios.VentasFuncCalcularPrecioFinal(PrecioBase, Descuento);
        }

        public bool VentasFuncGuardar(string TipoCliente, string TipoProducto, decimal Porcentaje, decimal PrecioBase, out string Mensaje)
        {
            if (string.IsNullOrWhiteSpace(TipoCliente)) { Mensaje = "Seleccione un tipo de cliente."; return false; }
            if (string.IsNullOrWhiteSpace(TipoProducto)) { Mensaje = "Seleccione un tipo de producto."; return false; }
            if (PrecioBase < 0) { Mensaje = "El precio base no puede ser negativo."; return false; }
            if (Porcentaje < 0 || Porcentaje > 100) { Mensaje = "El descuento debe estar entre 0 y 100 %."; return false; }
            CapaModelo_ListadoPrecios.Entidades.ClsPrecioCliente Registro = new CapaModelo_ListadoPrecios.Entidades.ClsPrecioCliente
            {
                TipoCliente = TipoCliente.Trim(), TipoProducto = TipoProducto.Trim(),
                PorcentajeDescuento = Porcentaje, PrecioBase = PrecioBase,
                PrecioFinal = VentasFuncCalcularPrecioFinal(PrecioBase, Porcentaje)
            };
            return _Modelo.VentasFuncGuardar(Registro, out Mensaje);
        }
    }
}
