
/*
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsModeloStock.cs
 * REALIZADO POR: VELVETH SARAÍ CHAVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 * FECHA: 07/11/2026
 *
 * DESCRIPCIÓN:
 * Valida los datos del Stock y coordina las operaciones
 * entre la Vista y la Capa Modelo.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using CapaModelo_Inventario_Logistica.Contratos;
using CapaModelo_Inventario_Logistica.Entidades;
using CapaModelo_Inventario_Logistica.Repositorios;

namespace CapaControlador_Inventario_Logistica.Modelos_de_Controladores
{
    public class ClsModeloStock
    {
        private readonly ClsIRepositorioStock _Repositorio;

        public ClsDatosStock Stock { get; set; }

        public ClsEstadoEntidad Estado { get; set; }

        public ClsModeloStock()
        {
            _Repositorio = new ClsRepositorioStock();

            Stock = new ClsDatosStock();
        }

        // ========================================================
        // GUARDAR, MODIFICAR O ELIMINAR
        // ========================================================

        public string LogisticaFuncGrabarCambios()
        {
            if (Stock == null)
            {
                throw new ValidationException(
                    "No hay datos de Stock.");
            }

            int Filas;
            string Mensaje;

            switch (Estado)
            {
                case ClsEstadoEntidad.Added:

                    LogisticaProcValidarStock(Stock);

                    Filas = _Repositorio.LogisticaMetAgregar(
                        LogisticaFuncCrearEntidad(Stock));

                    Mensaje =
                        "El stock se guardó correctamente.";
                    break;

                case ClsEstadoEntidad.Modified:

                    if (Stock.IdStock <= 0)
                    {
                        throw new ValidationException(
                            "Seleccione el stock a modificar.");
                    }

                    LogisticaProcValidarStock(Stock);

                    Filas = _Repositorio.LogisticaMetEditar(
                        LogisticaFuncCrearEntidad(Stock));

                    Mensaje =
                        "El stock se modificó correctamente.";
                    break;

                case ClsEstadoEntidad.Deleted:

                    if (Stock.IdStock <= 0)
                    {
                        throw new ValidationException(
                            "Seleccione el stock a eliminar.");
                    }

                    Filas = _Repositorio.LogisticaMetEliminar(
                        Stock.IdStock);

                    Mensaje =
                        "El stock se eliminó correctamente.";
                    break;

                default:
                    throw new InvalidOperationException(
                        "Operación no válida.");
            }

            if (Filas <= 0)
            {
                throw new InvalidOperationException(
                    "No se modificó ningún registro.");
            }

            return Mensaje;
        }

        // ========================================================
        // VALIDAR STOCK
        // ========================================================

        private void LogisticaProcValidarStock(
            ClsDatosStock Datos)
        {
            var Resultados =
                new List<ValidationResult>();

            bool Valido = Validator.TryValidateObject(
                Datos,
                new ValidationContext(Datos),
                Resultados,
                true);

            if (!Valido)
            {
                throw new ValidationException(
                    string.Join(
                        "\n",
                        Resultados.Select(
                            R => R.ErrorMessage)));
            }

            if (Datos.ApartadosStock >
                Datos.StockActualStock)
            {
                throw new ValidationException(
                    "Los apartados no pueden superar " +
                    "el stock actual.");
            }
        }

        // ========================================================
        // CONVERTIR DATOS EN ENTIDAD
        // ========================================================

        private ClsStock LogisticaFuncCrearEntidad(
            ClsDatosStock D)
        {
            return new ClsStock
            {
                IdStock = D.IdStock,

                FkSucursalCodigo = D.FkSucursalCodigo,
                FkBodegaCodigo = D.FkBodegaCodigo,
                FkProductoCodigo = D.FkProductoCodigo,

                StockFisicoStock = D.StockFisicoStock,
                StockVirtualStock = D.StockVirtualStock,
                StockAnteriorStock = D.StockAnteriorStock,
                StockActualStock = D.StockActualStock,

                CargosMesStock = D.CargosMesStock,
                AbonosMesStock = D.AbonosMesStock,

                CargosAcumuladosStock =
                    D.CargosAcumuladosStock,

                AbonosAcumuladosStock =
                    D.AbonosAcumuladosStock,

                ApartadosStock = D.ApartadosStock,

                EstadoVentaStock = D.EstadoVentaStock,
                EstadoCompraStock = D.EstadoCompraStock
            };
        }

        // ========================================================
        // CONSULTAR STOCK
        // ========================================================

        public List<ClsDatosStock> LogisticaFuncObtenerTodos()
        {
            return _Repositorio.LogisticaMetObtenerTodos()
                .Select(D => new ClsDatosStock
                {
                    IdStock = D.IdStock,

                    FkSucursalCodigo = D.FkSucursalCodigo,
                    FkBodegaCodigo = D.FkBodegaCodigo,
                    FkProductoCodigo = D.FkProductoCodigo,

                    StockFisicoStock = D.StockFisicoStock,
                    StockVirtualStock = D.StockVirtualStock,
                    StockAnteriorStock = D.StockAnteriorStock,
                    StockActualStock = D.StockActualStock,

                    CargosMesStock = D.CargosMesStock,
                    AbonosMesStock = D.AbonosMesStock,

                    CargosAcumuladosStock =
                        D.CargosAcumuladosStock,

                    AbonosAcumuladosStock =
                        D.AbonosAcumuladosStock,

                    ApartadosStock = D.ApartadosStock,

                    EstadoVentaStock = D.EstadoVentaStock,
                    EstadoCompraStock = D.EstadoCompraStock,

                    NombreSucursal = D.NombreSucursal,
                    NombreBodega = D.NombreBodega,
                    NombreProducto = D.NombreProducto
                })
                .ToList();
        }

        // ========================================================
        // CONVERTIR CATÁLOGOS
        // ========================================================

        private List<ClsOpcionCatalogo>
            LogisticaFuncConvertirCatalogo(
                IEnumerable<ClsCatalogo> Datos)
        {
            return Datos.Select(X =>
                new ClsOpcionCatalogo
                {
                    Id = X.Id,
                    Nombre = X.Nombre
                })
                .ToList();
        }

        // ========================================================
        // OBTENER SUCURSALES
        // ========================================================

        public List<ClsOpcionCatalogo>
            LogisticaFuncSucursales()
        {
            return LogisticaFuncConvertirCatalogo(
                _Repositorio.LogisticaMetObtenerSucursales());
        }

        // ========================================================
        // OBTENER BODEGAS DE UNA SUCURSAL
        // ========================================================

        public List<ClsOpcionCatalogo>
            LogisticaFuncBodegas(int IdSucursal)
        {
            return LogisticaFuncConvertirCatalogo(
                _Repositorio.LogisticaMetObtenerBodegas(
                    IdSucursal));
        }

        // ========================================================
        // OBTENER PRODUCTOS
        // ========================================================

        public List<ClsOpcionCatalogo>
            LogisticaFuncProductos()
        {
            return LogisticaFuncConvertirCatalogo(
                _Repositorio.LogisticaMetObtenerProductos());
        }
    }
}
