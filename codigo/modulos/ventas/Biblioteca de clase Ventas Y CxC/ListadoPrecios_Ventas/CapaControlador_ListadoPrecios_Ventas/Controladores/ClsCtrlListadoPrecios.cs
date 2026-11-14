using System;
using System.Collections.Generic;
using System.Linq;
using CapaControlador_ListadoPrecios_Ventas.Contratos;
using CapaModelo_ListadoPrecios_Ventas;
using CapaModelo_ListadoPrecios_Ventas.Entidades;

namespace CapaControlador_ListadoPrecios_Ventas
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

        public List<ClsPrecioCliente> VentasFuncConsultarListado(
            string tipoCliente, string marca, string linea,
            string categoria, string proveedor, string producto)
        {
            IEnumerable<ClsPrecioCliente> consulta = _Modelo.VentasFuncObtenerListado();

            if (!string.IsNullOrWhiteSpace(tipoCliente) && tipoCliente != "Todos")
                consulta = consulta.Where(x => string.Equals(x.TipoCliente, tipoCliente, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(marca) && marca != "Todos")
                consulta = consulta.Where(x => string.Equals(x.Marca, marca, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(linea) && linea != "Todos")
                consulta = consulta.Where(x => string.Equals(x.Linea, linea, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(categoria) && categoria != "Todos")
                consulta = consulta.Where(x => string.Equals(x.Categoria, categoria, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(proveedor) && proveedor != "Todos")
                consulta = consulta.Where(x => string.Equals(x.Proveedor, proveedor, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(producto) && producto != "Todos")
                consulta = consulta.Where(x => string.Equals(x.Producto, producto, StringComparison.OrdinalIgnoreCase));

            return consulta.OrderBy(x => x.TipoCliente).ThenBy(x => x.Producto).ToList();
        }

        public List<ClsPrecioClienteDTO> VentasFuncConsultarListadoVista(
            string tipoCliente, string marca, string linea,
            string categoria, string proveedor, string producto)
        {
            return VentasFuncConsultarListado(tipoCliente, marca, linea, categoria, proveedor, producto)
                .Select(registro => new ClsPrecioClienteDTO
                {
                    IdListaPrecio = registro.IdListaPrecio,
                    IdTipoCliente = registro.IdTipoCliente,
                    IdProducto = registro.IdProducto,
                    IdProveedor = registro.IdProveedor,
                    TipoCliente = registro.TipoCliente,
                    Marca = registro.Marca,
                    Linea = registro.Linea,
                    Categoria = registro.Categoria,
                    Proveedor = registro.Proveedor,
                    Producto = registro.Producto,
                    TipoModificacion = registro.TipoModificacion,
                    Valor = registro.Valor,
                    AlcanceModificacion = registro.AlcanceModificacion,
                    PorcentajeDescuento = registro.PorcentajeDescuento,
                    PrecioBase = registro.PrecioBase,
                    PrecioFinal = registro.PrecioFinal,
                }).ToList();
        }

        public bool VentasFuncGuardarVariosVista(IList<ClsPrecioClienteDTO> registros, out string mensaje)
        {
            var entidades = registros?.Select(registro => registro == null ? null : new ClsPrecioCliente
            {
                IdListaPrecio = registro.IdListaPrecio,
                IdTipoCliente = registro.IdTipoCliente,
                IdProducto = registro.IdProducto,
                IdProveedor = registro.IdProveedor,
                TipoCliente = registro.TipoCliente,
                Marca = registro.Marca,
                Linea = registro.Linea,
                Categoria = registro.Categoria,
                Proveedor = registro.Proveedor,
                Producto = registro.Producto,
                TipoModificacion = registro.TipoModificacion,
                Valor = registro.Valor,
                AlcanceModificacion = registro.AlcanceModificacion,
                PorcentajeDescuento = registro.PorcentajeDescuento,
                PrecioBase = registro.PrecioBase,
                PrecioFinal = registro.PrecioFinal,
            }).ToList();

            return VentasFuncGuardarVarios(entidades, out mensaje);
        }

        public List<string> VentasFuncObtenerProveedores()
        {
            return _Modelo.VentasFuncObtenerProveedores();
        }

        public decimal VentasFuncCalcularPrecioFinal(
            decimal precioBase, decimal valor, string tipoModificacion)
        {
            return ClsModeloListadoPrecios.VentasFuncCalcularPrecioFinal(
                precioBase, valor, tipoModificacion);
        }

        // Mantiene la firma anterior para no romper llamadas existentes del proyecto.
        public bool VentasFuncGuardar(
            string tipoCliente, string producto, decimal porcentaje,
            decimal precioBase, string tipoModificacion, decimal valor,
            string alcance, out string mensaje)
        {
            ClsPrecioCliente existente = _Modelo.VentasFuncObtenerListado()
                .FirstOrDefault(x => x.TipoCliente == tipoCliente && x.Producto == producto);

            if (existente == null)
            {
                mensaje = "No se encontró el tipo de cliente y producto seleccionados.";
                return false;
            }

            decimal nuevoPrecioBase = precioBase;
            decimal nuevoPorcentaje = existente.PorcentajeDescuento;

            if (tipoModificacion == "AumentoBase")
                nuevoPrecioBase = precioBase + valor;
            else if (tipoModificacion == "AumentoPorcentaje")
                nuevoPrecioBase = precioBase + (precioBase * valor / 100m);
            else if (tipoModificacion == "Descuento")
                nuevoPorcentaje = porcentaje;

            existente.PrecioBase = nuevoPrecioBase;
            existente.PorcentajeDescuento = nuevoPorcentaje;
            existente.PrecioFinal = nuevoPrecioBase -
                (nuevoPrecioBase * nuevoPorcentaje / 100m);
            existente.TipoModificacion = tipoModificacion;
            existente.Valor = valor;
            existente.AlcanceModificacion = alcance;

            return _Modelo.VentasFuncGuardar(existente, out mensaje);
        }

        // Recibe las filas reales seleccionadas en el formulario y las guarda juntas.
        public bool VentasFuncGuardarVarios(
            IList<ClsPrecioCliente> registros, out string mensaje)
        {
            if (registros == null || registros.Count == 0)
            {
                mensaje = "No hay productos seleccionados para guardar.";
                return false;
            }

            foreach (ClsPrecioCliente registro in registros)
            {
                if (registro == null || registro.IdProducto <= 0 || registro.IdTipoCliente <= 0)
                {
                    mensaje = "Una de las filas no contiene un identificador válido de producto o tipo de cliente.";
                    return false;
                }
                if (registro.PrecioBase < 0m || registro.PrecioFinal < 0m)
                {
                    mensaje = "El precio base y el precio final no pueden ser negativos.";
                    return false;
                }
            }

            return _Modelo.VentasFuncGuardarVarios(registros, out mensaje);
        }
    }
}
