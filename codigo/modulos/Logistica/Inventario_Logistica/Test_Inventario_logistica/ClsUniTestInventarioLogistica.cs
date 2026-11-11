/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsUniTestInventarioLogistica.cs
 * DESCRIPCIÓN: Pruebas unitarias de los controladores de Producto y Stock.
 * Utiliza repositorios falsos para no modificar la base de datos.
 * 
 * Responsable: Velveth Saraí Chavez Mejía
 * Carné: 0901 23 6269
 * Fecha de creación: 10/11/2026s
 * ============================================================================
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CapaControlador_Inventario_Logistica.Modelos_de_Controladores;
using CapaModelo_Inventario_Logistica.Contratos;
using CapaModelo_Inventario_Logistica.Entidades;

namespace Test_Inventario_logistica
{
    [TestClass]
    public class ClsUniTestInventarioLogistica
    {
        // =========================================================
        // PRUEBAS DE PRODUCTO
        // =========================================================

        [TestMethod]
        public void AgregarProducto_ConDatosValidos_EnviaProductoAlRepositorio()
        {
            // Arrange
            var Repositorio = new RepositorioProductoFalso();
            var Controlador = new ClsModeloProducto(Repositorio)
            {
                Estado = ClsEstadoEntidad.Added,
                Producto = CrearProductoValido()
            };

            // Act
            string Mensaje = Controlador.LogisticaFuncGrabarCambios();

            // Assert
            Assert.AreEqual("Producto guardado correctamente.", Mensaje);
            Assert.IsNotNull(Repositorio.ProductoAgregado);
            Assert.AreEqual("EMB-001", Repositorio.ProductoAgregado.SkuProducto);
            Assert.AreEqual("Chorizo parrillero", Repositorio.ProductoAgregado.NombreProducto);
            Assert.AreEqual(1, Repositorio.ProductoAgregado.IdProveedorProductos);
            Assert.AreEqual(0.10m, Repositorio.ProductoAgregado.PorcentajeComisionProducto);
        }

        [TestMethod]
        public void AgregarProducto_SinImagen_LanzaValidacion()
        {
            var Repositorio = new RepositorioProductoFalso();
            var Datos = CrearProductoValido();
            Datos.ImagenProducto = new byte[0];
            var Controlador = new ClsModeloProducto(Repositorio)
            {
                Estado = ClsEstadoEntidad.Added,
                Producto = Datos
            };

            Assert.ThrowsException<ValidationException>(
                () => Controlador.LogisticaFuncGrabarCambios());
            Assert.IsNull(Repositorio.ProductoAgregado);
        }

        [TestMethod]
        public void AgregarProducto_ComisionMayorAlLimite_LanzaValidacion()
        {
            var Datos = CrearProductoValido();
            Datos.PorcentajeComisionProducto = 1.00m;
            var Controlador = new ClsModeloProducto(new RepositorioProductoFalso())
            {
                Estado = ClsEstadoEntidad.Added,
                Producto = Datos
            };

            Assert.ThrowsException<ValidationException>(
                () => Controlador.LogisticaFuncGrabarCambios());
        }

        [TestMethod]
        public void AgregarProducto_StockMaximoMenorAlMinimo_LanzaValidacion()
        {
            var Datos = CrearProductoValido();
            Datos.StockMinimoProducto = 20;
            Datos.StockMaximoProducto = 10;
            var Controlador = new ClsModeloProducto(new RepositorioProductoFalso())
            {
                Estado = ClsEstadoEntidad.Added,
                Producto = Datos
            };

            Assert.ThrowsException<ValidationException>(
                () => Controlador.LogisticaFuncGrabarCambios());
        }

        [TestMethod]
        public void AgregarProducto_SkuVacio_LanzaValidacion()
        {
            var Datos = CrearProductoValido();
            Datos.SkuProducto = " ";
            var Controlador = new ClsModeloProducto(new RepositorioProductoFalso())
            {
                Estado = ClsEstadoEntidad.Added,
                Producto = Datos
            };

            Assert.ThrowsException<ValidationException>(
                () => Controlador.LogisticaFuncGrabarCambios());
        }

        [TestMethod]
        public void ModificarProducto_ConDatosValidos_EnviaCambiosAlRepositorio()
        {
            var Repositorio = new RepositorioProductoFalso();
            var Datos = CrearProductoValido();
            Datos.IdProducto = 101;
            Datos.NombreProducto = "Chorizo especial";
            var Controlador = new ClsModeloProducto(Repositorio)
            {
                Estado = ClsEstadoEntidad.Modified,
                Producto = Datos
            };

            string Mensaje = Controlador.LogisticaFuncGrabarCambios();

            Assert.AreEqual("Producto modificado correctamente.", Mensaje);
            Assert.IsNotNull(Repositorio.ProductoEditado);
            Assert.AreEqual(101, Repositorio.ProductoEditado.IdProducto);
            Assert.AreEqual("Chorizo especial", Repositorio.ProductoEditado.NombreProducto);
        }

        [TestMethod]
        public void ModificarProducto_SinIdentificador_LanzaValidacion()
        {
            var Datos = CrearProductoValido();
            Datos.IdProducto = 0;
            var Controlador = new ClsModeloProducto(new RepositorioProductoFalso())
            {
                Estado = ClsEstadoEntidad.Modified,
                Producto = Datos
            };

            Assert.ThrowsException<ValidationException>(
                () => Controlador.LogisticaFuncGrabarCambios());
        }

        [TestMethod]
        public void EliminarProducto_IdentificadorValido_EnviaIdAlRepositorio()
        {
            var Repositorio = new RepositorioProductoFalso();
            var Datos = CrearProductoValido();
            Datos.IdProducto = 101;
            var Controlador = new ClsModeloProducto(Repositorio)
            {
                Estado = ClsEstadoEntidad.Deleted,
                Producto = Datos
            };

            string Mensaje = Controlador.LogisticaFuncGrabarCambios();

            Assert.AreEqual("Producto eliminado correctamente.", Mensaje);
            Assert.AreEqual(101, Repositorio.IdProductoEliminado);
        }

        [TestMethod]
        public void EliminarProducto_SinIdentificador_LanzaValidacion()
        {
            var Controlador = new ClsModeloProducto(new RepositorioProductoFalso())
            {
                Estado = ClsEstadoEntidad.Deleted,
                Producto = CrearProductoValido()
            };

            Assert.ThrowsException<ValidationException>(
                () => Controlador.LogisticaFuncGrabarCambios());
        }

        [TestMethod]
        public void AgregarProducto_SinFilasAfectadas_LanzaError()
        {
            var Repositorio = new RepositorioProductoFalso { FilasAfectadas = 0 };
            var Controlador = new ClsModeloProducto(Repositorio)
            {
                Estado = ClsEstadoEntidad.Added,
                Producto = CrearProductoValido()
            };

            Assert.ThrowsException<InvalidOperationException>(
                () => Controlador.LogisticaFuncGrabarCambios());
        }

        [TestMethod]
        public void BuscarProducto_PorSkuIgnorandoMayusculas_DevuelveCoincidencia()
        {
            var Repositorio = new RepositorioProductoFalso();
            Repositorio.Productos.Add(new ClsProducto
            {
                IdProducto = 101,
                SkuProducto = "EMB-001",
                NombreProducto = "Chorizo parrillero"
            });
            Repositorio.Productos.Add(new ClsProducto
            {
                IdProducto = 102,
                SkuProducto = "EMB-002",
                NombreProducto = "Salchicha"
            });
            var Controlador = new ClsModeloProducto(Repositorio);

            var Resultado = Controlador.LogisticaFuncBuscar("emb-001");

            Assert.AreEqual(1, Resultado.Count);
            Assert.AreEqual(101, Resultado[0].IdProducto);
        }

        [TestMethod]
        public void ConsultarCategoria_ConNombreValido_DevuelveIdentificador()
        {
            var Repositorio = new RepositorioProductoFalso();
            Repositorio.Categorias.Add(new ClsCatalogo { Id = 4, Nombre = "Carnes" });
            var Controlador = new ClsModeloProducto(Repositorio);

            int IdCategoria = Controlador.LogisticaFuncObtenerIdCategoria(" carnes ");

            Assert.AreEqual(4, IdCategoria);
        }

        [TestMethod]
        public void Producto_IndicadorTieneImagen_DependeDelContenido()
        {
            var Datos = CrearProductoValido();
            Assert.IsTrue(Datos.TieneImagen);

            Datos.ImagenProducto = null;
            Assert.IsFalse(Datos.TieneImagen);
        }

        // =========================================================
        // PRUEBAS DE STOCK
        // =========================================================

        [TestMethod]
        public void AgregarStock_ConDatosValidos_EnviaStockAlRepositorio()
        {
            var Repositorio = new RepositorioStockFalso();
            var Controlador = new ClsModeloStock(Repositorio)
            {
                Estado = ClsEstadoEntidad.Added,
                Stock = CrearStockValido()
            };

            string Mensaje = Controlador.LogisticaFuncGrabarCambios();

            Assert.AreEqual("El stock se guardó correctamente.", Mensaje);
            Assert.IsNotNull(Repositorio.StockAgregado);
            Assert.AreEqual(1, Repositorio.StockAgregado.FkSucursalCodigo);
            Assert.AreEqual(2, Repositorio.StockAgregado.FkBodegaCodigo);
            Assert.AreEqual(101, Repositorio.StockAgregado.FkProductoCodigo);
            Assert.AreEqual(20, Repositorio.StockAgregado.StockActualStock);
            Assert.AreEqual(3, Repositorio.StockAgregado.ApartadosStock);
        }

        [TestMethod]
        public void AgregarStock_CantidadNegativa_LanzaValidacion()
        {
            var Datos = CrearStockValido();
            Datos.StockActualStock = -1;
            var Controlador = new ClsModeloStock(new RepositorioStockFalso())
            {
                Estado = ClsEstadoEntidad.Added,
                Stock = Datos
            };

            Assert.ThrowsException<ValidationException>(
                () => Controlador.LogisticaFuncGrabarCambios());
        }

        [TestMethod]
        public void AgregarStock_ApartadosMayoresAlActual_LanzaValidacion()
        {
            var Datos = CrearStockValido();
            Datos.ApartadosStock = 21;
            var Controlador = new ClsModeloStock(new RepositorioStockFalso())
            {
                Estado = ClsEstadoEntidad.Added,
                Stock = Datos
            };

            var Error = Assert.ThrowsException<ValidationException>(
                () => Controlador.LogisticaFuncGrabarCambios());
            StringAssert.Contains(Error.Message, "Los apartados no pueden superar");
        }

        [TestMethod]
        public void AgregarStock_SinSucursal_LanzaValidacion()
        {
            var Datos = CrearStockValido();
            Datos.FkSucursalCodigo = 0;
            var Controlador = new ClsModeloStock(new RepositorioStockFalso())
            {
                Estado = ClsEstadoEntidad.Added,
                Stock = Datos
            };

            Assert.ThrowsException<ValidationException>(
                () => Controlador.LogisticaFuncGrabarCambios());
        }

        [TestMethod]
        public void ModificarStock_ConDatosValidos_EnviaCambiosAlRepositorio()
        {
            var Repositorio = new RepositorioStockFalso();
            var Datos = CrearStockValido();
            Datos.IdStock = 15;
            Datos.StockActualStock = 35;
            var Controlador = new ClsModeloStock(Repositorio)
            {
                Estado = ClsEstadoEntidad.Modified,
                Stock = Datos
            };

            string Mensaje = Controlador.LogisticaFuncGrabarCambios();

            Assert.AreEqual("El stock se modificó correctamente.", Mensaje);
            Assert.IsNotNull(Repositorio.StockEditado);
            Assert.AreEqual(15, Repositorio.StockEditado.IdStock);
            Assert.AreEqual(35, Repositorio.StockEditado.StockActualStock);
        }

        [TestMethod]
        public void ModificarStock_SinIdentificador_LanzaValidacion()
        {
            var Controlador = new ClsModeloStock(new RepositorioStockFalso())
            {
                Estado = ClsEstadoEntidad.Modified,
                Stock = CrearStockValido()
            };

            Assert.ThrowsException<ValidationException>(
                () => Controlador.LogisticaFuncGrabarCambios());
        }

        [TestMethod]
        public void EliminarStock_ConIdentificadorValido_EnviaIdAlRepositorio()
        {
            var Repositorio = new RepositorioStockFalso();
            var Datos = CrearStockValido();
            Datos.IdStock = 15;
            var Controlador = new ClsModeloStock(Repositorio)
            {
                Estado = ClsEstadoEntidad.Deleted,
                Stock = Datos
            };

            string Mensaje = Controlador.LogisticaFuncGrabarCambios();

            Assert.AreEqual("El stock se eliminó correctamente.", Mensaje);
            Assert.AreEqual(15, Repositorio.IdStockEliminado);
        }

        [TestMethod]
        public void ConsultarStock_ConRegistros_ConvierteDatosCorrectamente()
        {
            var Repositorio = new RepositorioStockFalso();
            Repositorio.Stocks.Add(new ClsStock
            {
                IdStock = 15,
                FkSucursalCodigo = 1,
                FkBodegaCodigo = 2,
                FkProductoCodigo = 101,
                StockActualStock = 20,
                NombreSucursal = "Central",
                NombreBodega = "Bodega Norte",
                NombreProducto = "Chorizo parrillero"
            });
            var Controlador = new ClsModeloStock(Repositorio);

            var Resultado = Controlador.LogisticaFuncObtenerTodos();

            Assert.AreEqual(1, Resultado.Count);
            Assert.AreEqual(15, Resultado[0].IdStock);
            Assert.AreEqual(20, Resultado[0].StockActualStock);
            Assert.AreEqual("Bodega Norte", Resultado[0].NombreBodega);
        }

        [TestMethod]
        public void ConsultarBodegas_PorSucursal_RespetaElFiltro()
        {
            var Repositorio = new RepositorioStockFalso();
            Repositorio.Bodegas.Add(new ClsCatalogo { Id = 2, Nombre = "Bodega Norte" });
            var Controlador = new ClsModeloStock(Repositorio);

            var Resultado = Controlador.LogisticaFuncBodegas(1);

            Assert.AreEqual(1, Repositorio.SucursalConsultada);
            Assert.AreEqual(1, Resultado.Count);
            Assert.AreEqual(2, Resultado[0].Id);
        }

        [TestMethod]
        public void Stock_TieneExistencias_CambiaSegunStockActual()
        {
            var Datos = CrearStockValido();
            Assert.IsTrue(Datos.TieneExistencias);

            Datos.StockActualStock = 0;
            Assert.IsFalse(Datos.TieneExistencias);
        }

        [TestMethod]
        public void AgregarStock_SinFilasAfectadas_LanzaError()
        {
            var Repositorio = new RepositorioStockFalso { FilasAfectadas = 0 };
            var Controlador = new ClsModeloStock(Repositorio)
            {
                Estado = ClsEstadoEntidad.Added,
                Stock = CrearStockValido()
            };

            Assert.ThrowsException<InvalidOperationException>(
                () => Controlador.LogisticaFuncGrabarCambios());
        }

        // =========================================================
        // CREACIÓN DE DATOS VÁLIDOS
        // =========================================================

        private static ClsDatosProducto CrearProductoValido()
        {
            return new ClsDatosProducto
            {
                SkuProducto = "EMB-001",
                ClasificacionProducto = "REF",
                NombreProducto = "Chorizo parrillero",
                DescripcionProducto = "Paquete refrigerado de 1 kg",
                PaisProducto = "Guatemala",
                IdProveedorProductos = 1,
                FkMarcaCodigo = 1,
                FkCategoriaCodigo = 1,
                FkLineaCodigo = 1,
                PrecioCostoProducto = 15.50m,
                PrecioVentaProducto = 20.00m,
                PorcentajeComisionProducto = 0.10m,
                StockMinimoProducto = 5,
                StockMaximoProducto = 100,
                PesoProducto = 1.00m,
                ImagenProducto = new byte[] { 1, 2, 3 }
            };
        }

        private static ClsDatosStock CrearStockValido()
        {
            return new ClsDatosStock
            {
                FkSucursalCodigo = 1,
                FkBodegaCodigo = 2,
                FkProductoCodigo = 101,
                StockFisicoStock = 20,
                StockVirtualStock = 20,
                StockAnteriorStock = 15,
                StockActualStock = 20,
                CargosMesStock = 5,
                AbonosMesStock = 0,
                CargosAcumuladosStock = 5,
                AbonosAcumuladosStock = 0,
                ApartadosStock = 3,
                EstadoVentaStock = true,
                EstadoCompraStock = true
            };
        }

        // =========================================================
        // REPOSITORIO FALSO DE PRODUCTO
        // =========================================================

        private sealed class RepositorioProductoFalso : ClsIRepositorioProducto
        {
            public int FilasAfectadas { get; set; } = 1;
            public ClsProducto ProductoAgregado { get; private set; }
            public ClsProducto ProductoEditado { get; private set; }
            public int IdProductoEliminado { get; private set; }

            public List<ClsProducto> Productos { get; } = new List<ClsProducto>();
            public List<ClsCatalogo> Categorias { get; } = new List<ClsCatalogo>();

            public int LogisticaMetAgregar(ClsProducto Entidad)
            {
                ProductoAgregado = Entidad;
                return FilasAfectadas;
            }

            public int LogisticaMetEditar(ClsProducto Entidad)
            {
                ProductoEditado = Entidad;
                return FilasAfectadas;
            }

            public int LogisticaMetEliminar(int IdProducto)
            {
                IdProductoEliminado = IdProducto;
                return FilasAfectadas;
            }

            public IEnumerable<ClsProducto> LogisticaMetObtenerTodos()
            {
                return Productos;
            }

            public IEnumerable<ClsCatalogo> LogisticaMetObtenerCategorias()
            {
                return Categorias;
            }

            public IEnumerable<ClsCatalogo> LogisticaMetObtenerMarcas()
            {
                return new List<ClsCatalogo>();
            }

            public IEnumerable<ClsCatalogo> LogisticaMetObtenerLineas()
            {
                return new List<ClsCatalogo>();
            }

            public IEnumerable<ClsCatalogo> LogisticaMetObtenerProveedores()
            {
                return new List<ClsCatalogo>();
            }
        }

        // =========================================================
        // REPOSITORIO FALSO DE STOCK
        // =========================================================

        private sealed class RepositorioStockFalso : ClsIRepositorioStock
        {
            public int FilasAfectadas { get; set; } = 1;
            public ClsStock StockAgregado { get; private set; }
            public ClsStock StockEditado { get; private set; }
            public int IdStockEliminado { get; private set; }
            public int SucursalConsultada { get; private set; }

            public List<ClsStock> Stocks { get; } = new List<ClsStock>();
            public List<ClsCatalogo> Bodegas { get; } = new List<ClsCatalogo>();

            public int LogisticaMetAgregar(ClsStock Entidad)
            {
                StockAgregado = Entidad;
                return FilasAfectadas;
            }

            public int LogisticaMetEditar(ClsStock Entidad)
            {
                StockEditado = Entidad;
                return FilasAfectadas;
            }

            public int LogisticaMetEliminar(int IdStock)
            {
                IdStockEliminado = IdStock;
                return FilasAfectadas;
            }

            public IEnumerable<ClsStock> LogisticaMetObtenerTodos()
            {
                return Stocks;
            }

            public IEnumerable<ClsCatalogo> LogisticaMetObtenerSucursales()
            {
                return new List<ClsCatalogo>();
            }

            public IEnumerable<ClsCatalogo> LogisticaMetObtenerBodegas(int IdSucursal)
            {
                SucursalConsultada = IdSucursal;
                return Bodegas;
            }

            public IEnumerable<ClsCatalogo> LogisticaMetObtenerProductos()
            {
                return new List<ClsCatalogo>();
            }
        }
    }
}
