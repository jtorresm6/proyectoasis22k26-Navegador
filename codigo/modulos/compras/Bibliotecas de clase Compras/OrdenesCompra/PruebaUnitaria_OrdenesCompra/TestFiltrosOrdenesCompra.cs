using System;
using System.Data;
using System.Linq;
using CapaControlador_OrdenesCompra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PruebaUnitaria_OrdenesCompra.Apoyo;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Verifica los filtros de búsqueda, estado y fechas de las órdenes de compra.
 * ===================================================================
*/

namespace PruebaUnitaria_OrdenesCompra
{
    [TestClass]
    public class TestFiltrosOrdenesCompra
    {
        private ClsRepositorioOrdenesCompraPrueba _Repositorio;
        private ClsOrdenesCompraControlador _Controlador;

        [TestInitialize]
        public void Preparar()
        {
            _Repositorio = new ClsRepositorioOrdenesCompraPrueba();
            _Controlador = new ClsOrdenesCompraControlador(_Repositorio);
        }

        [TestCleanup]
        public void Liberar() => _Repositorio.Dispose();

        [DataTestMethod]
        [DataRowAttribute(" oc-003 ", "103")]
        [DataRowAttribute("jose carnicos", "101,103")]
        [DataRowAttribute("NIT-4", "102,105")]
        [DataRowAttribute("NO-EXISTE", "")]
        [DataRowAttribute("' OR 1=1 --", "")]
        [DataRowAttribute("[", "")]
        [DataRowAttribute(null, "101,102,103,104,105")]
        [DataRowAttribute("   ", "101,102,103,104,105")]
        public void BuscarPorCodigoProveedorONit(string texto, string ids)
        {
            using (var resultado = Filtrar(texto))
                Assert.AreEqual(ids, Ids(resultado));
        }

        [DataTestMethod]
        [DataRowAttribute("Pendiente", 101)]
        [DataRowAttribute("Autorizada", 102)]
        [DataRowAttribute("Rechazada", 103)]
        [DataRowAttribute("Anulada", 104)]
        [DataRowAttribute("Recibida", 105)]
        public void FiltrarPorEstado(string estado, int id)
        {
            using (var resultado = Filtrar(estado: estado))
                Assert.AreEqual(id.ToString(), Ids(resultado));
        }

        [TestMethod]
        public void FechasIncluyenAmbosLimitesSinImportarHora()
        {
            using (var resultado = Filtrar(inicio: new DateTime(2026, 10, 5, 18, 0, 0), fin: new DateTime(2026, 10, 15, 1, 0, 0)))
                Assert.AreEqual("102,103,104", Ids(resultado));
        }

        [TestMethod]
        public void CombinarTextoEstadoYFecha()
        {
            var fecha = new DateTime(2026, 10, 10);
            using (var resultado = Filtrar("jose", "Rechazada", fecha, fecha))
                Assert.AreEqual("103", Ids(resultado));
        }

        [TestMethod]
        public void FiltrarNoModificaDatosOriginales()
        {
            var original = _Repositorio.Datos.Rows.Find(101)["codigoOrdenCompra"];
            using (var resultado = Filtrar("OC-001"))
                resultado.Rows[0]["codigoOrdenCompra"] = "OTRO";
            Assert.AreEqual(original, _Repositorio.Datos.Rows.Find(101)["codigoOrdenCompra"]);
        }

        [TestMethod]
        public void FiltrarOmiteFilasEliminadas()
        {
            _Repositorio.Datos.Rows.Find(101).Delete();
            using (var resultado = Filtrar())
                Assert.AreEqual("102,103,104,105", Ids(resultado));
        }

        [TestMethod]
        public void FiltrarTablaVaciaConservaColumnas()
        {
            _Repositorio.Datos.Clear();
            using (var resultado = Filtrar())
            {
                Assert.AreEqual(0, resultado.Rows.Count);
                Assert.AreEqual(_Repositorio.Datos.Columns.Count, resultado.Columns.Count);
            }
        }

        [DataTestMethod]
        [DataRowAttribute(true, false)]
        [DataRowAttribute(false, true)]
        public void RechazarRangoIncompleto(bool conInicio, bool conFin)
        {
            DateTime? inicio = conInicio ? new DateTime(2026, 10, 1) : (DateTime?)null;
            DateTime? fin = conFin ? new DateTime(2026, 10, 31) : (DateTime?)null;
            Assert.ThrowsException<ArgumentException>(() => Filtrar(inicio: inicio, fin: fin));
        }

        [TestMethod]
        public void RechazarRangoInvertido() => Assert.ThrowsException<ArgumentException>(() => Filtrar(inicio: new DateTime(2026, 10, 31), fin: new DateTime(2026, 10, 1)));

        [TestMethod]
        public void RechazarEstadoDesconocido() => Assert.ThrowsException<ArgumentException>(() => Filtrar(estado: "Aprobada"));

        [TestMethod]
        public void RechazarTablaNula() => Assert.ThrowsException<ArgumentNullException>(() => _Controlador.ComprasFuncFiltrar(null, "", "Todos", null, null));

        private DataTable Filtrar(string texto = "", string estado = "Todos", DateTime? inicio = null, DateTime? fin = null)
            => _Controlador.ComprasFuncFiltrar(_Repositorio.Datos, texto, estado, inicio, fin);

        private static string Ids(DataTable tabla)
            => string.Join(",", tabla.Rows.Cast<DataRow>().Select(fila => Convert.ToInt32(fila["idOrdenCompra"])));
    }
}
