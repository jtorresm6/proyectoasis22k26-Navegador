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
        private readonly ClsModeloListadoPrecios _modelo;
        public string MensajeEstado { get { return _modelo.MensajeEstado; } }
        public bool UsaBaseDeDatos { get { return _modelo.UsaBaseDeDatos; } }

        public ClsCtrlListadoPrecios() { _modelo = new ClsModeloListadoPrecios(); }

        public List<ClsPrecioCliente> ListadoPreciosMetConsultarListado()
        {
            return _modelo.ListadoPreciosMetObtenerListado().OrderBy(PrecioClienteActual => PrecioClienteActual.TipoCliente).ThenBy(PrecioClienteActual => PrecioClienteActual.TipoProducto).Select(PrecioClienteActual => new ClsPrecioCliente { TipoCliente = PrecioClienteActual.TipoCliente, TipoProducto = PrecioClienteActual.TipoProducto, PorcentajeDescuento = PrecioClienteActual.PorcentajeDescuento, PrecioBase = PrecioClienteActual.PrecioBase, PrecioFinal = PrecioClienteActual.PrecioFinal }).ToList();
        }

        public decimal ListadoPreciosMetCalcularPrecioFinal(decimal PrecioBase, decimal PorcentajeDescuento)
        {
            if (PrecioBase < 0) throw new ArgumentOutOfRangeException("PrecioBase", "El precio base no puede ser negativo.");
            if (PorcentajeDescuento < 0 || PorcentajeDescuento > 100) throw new ArgumentOutOfRangeException("PorcentajeDescuento", "El descuento debe estar entre 0 y 100 %.");
            return ClsModeloListadoPrecios.ListadoPreciosMetCalcularPrecioFinal(PrecioBase, PorcentajeDescuento);
        }

        public bool ListadoPreciosMetGuardar(string TipoCliente, string TipoProducto, decimal PorcentajeDescuento, decimal PrecioBase, out string Mensaje)
        {
            if (string.IsNullOrWhiteSpace(TipoCliente)) { Mensaje = "Seleccione un tipo de cliente."; return false; }
            if (string.IsNullOrWhiteSpace(TipoProducto)) { Mensaje = "Seleccione un tipo de producto."; return false; }
            if (PrecioBase < 0) { Mensaje = "El precio base no puede ser negativo."; return false; }
            if (PorcentajeDescuento < 0 || PorcentajeDescuento > 100) { Mensaje = "El descuento debe estar entre 0 y 100 %."; return false; }
            CapaModelo_ListadoPrecios.Entidades.ClsPrecioCliente RegistroPrecio = new CapaModelo_ListadoPrecios.Entidades.ClsPrecioCliente
            {
                TipoCliente = TipoCliente.Trim(), TipoProducto = TipoProducto.Trim(),
                PorcentajeDescuento = PorcentajeDescuento, PrecioBase = PrecioBase,
                PrecioFinal = ListadoPreciosMetCalcularPrecioFinal(PrecioBase, PorcentajeDescuento)
            };
            return _modelo.ListadoPreciosMetGuardar(RegistroPrecio, out Mensaje);
        }
    }
}
