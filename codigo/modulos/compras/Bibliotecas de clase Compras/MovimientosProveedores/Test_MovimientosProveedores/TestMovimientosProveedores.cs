using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using CapaControlador_MovimientosProveedores;
using CapaModelo_MovimientosProveedores.Contratos;
using CapaModelo_MovimientosProveedores.Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PruebaUnitaria_MovimientosProveedores
{
    [TestClass]
    public class TestMovimientosProveedores
    {
        private static ClsModeloNotaProveedor CrearNota(RepositorioPrueba repositorio)
        {
            return new ClsModeloNotaProveedor(repositorio) {
                IdProveedor = 1, IdConceptoProveedor = 2, Serie = " NC ", Numero = " 1001 ",
                FechaEmision = new DateTime(2026, 11, 5), FechaVencimiento = new DateTime(2026, 12, 5),
                IdMoneda = 1, TipoCambio = 7.65m, Monto = 1250.50m, Estado = EstadoEntidad.Added
            };
        }

        // Cada fila altera una sola propiedad de una nota valida.
        [DataTestMethod]
        [DataRow("IdProveedor", "0", "Seleccione un proveedor.")]
        [DataRow("IdProveedor", "-1", "Seleccione un proveedor.")]
        [DataRow("IdConceptoProveedor", "0", "Seleccione un tipo de nota.")]
        [DataRow("IdConceptoProveedor", "-1", "Seleccione un tipo de nota.")]
        [DataRow("Serie", null, "La serie es obligatoria y admite hasta 20 caracteres.")]
        [DataRow("Serie", "", "La serie es obligatoria y admite hasta 20 caracteres.")]
        [DataRow("Serie", "   ", "La serie es obligatoria y admite hasta 20 caracteres.")]
        [DataRow("Serie", "123456789012345678901", "La serie es obligatoria y admite hasta 20 caracteres.")]
        [DataRow("Numero", null, "El número es obligatorio y admite hasta 30 caracteres.")]
        [DataRow("Numero", "", "El número es obligatorio y admite hasta 30 caracteres.")]
        [DataRow("Numero", "   ", "El número es obligatorio y admite hasta 30 caracteres.")]
        [DataRow("Numero", "1234567890123456789012345678901", "El número es obligatorio y admite hasta 30 caracteres.")]
        [DataRow("FechaVencimiento", "2026-11-04", "El vencimiento no puede ser anterior a la emisión.")]
        [DataRow("IdMoneda", "0", "Seleccione una moneda.")]
        [DataRow("IdMoneda", "-1", "Seleccione una moneda.")]
        [DataRow("TipoCambio", "0", "Ingrese un tipo de cambio válido, con hasta seis decimales.")]
        [DataRow("TipoCambio", "-1", "Ingrese un tipo de cambio válido, con hasta seis decimales.")]
        [DataRow("TipoCambio", "1000000", "Ingrese un tipo de cambio válido, con hasta seis decimales.")]
        [DataRow("TipoCambio", "1.1234567", "Ingrese un tipo de cambio válido, con hasta seis decimales.")]
        [DataRow("Monto", "0", "Ingrese un importe mayor que cero, con hasta cuatro decimales.")]
        [DataRow("Monto", "-1", "Ingrese un importe mayor que cero, con hasta cuatro decimales.")]
        [DataRow("Monto", "100000000", "Ingrese un importe mayor que cero, con hasta cuatro decimales.")]
        [DataRow("Monto", "1.12345", "Ingrese un importe mayor que cero, con hasta cuatro decimales.")]
        public void GrabarCambios_DatoInvalido_RechazaSinGuardar(string campo, string valor, string mensaje)
        {
            var repositorio = new RepositorioPrueba();
            var nota = CrearNota(repositorio);
            var propiedad = typeof(ClsModeloNotaProveedor).GetProperty(campo);
            propiedad.SetValue(nota, valor == null ? null : Convert.ChangeType(valor, propiedad.PropertyType, CultureInfo.InvariantCulture));
            var error = Assert.ThrowsException<ArgumentException>(() => nota.ComprasMetValidar());
            Assert.AreEqual(mensaje, error.Message);
            Assert.ThrowsException<ArgumentException>(() => nota.GrabarCambios());
            Assert.AreEqual(0, repositorio.Notas.Count);
        }

        [DataTestMethod]
        [DataRow("0.000001", "0.0001")]
        [DataRow("999999.999999", "99999999.9999")]
        [DataRow("7.65", "1250.50")]
        public void GrabarCambios_DatosValidosYLimites_GuardaNota(string cambio, string monto)
        {
            var repo = new RepositorioPrueba();
            var nota = CrearNota(repo);
            nota.TipoCambio = decimal.Parse(cambio, CultureInfo.InvariantCulture);
            nota.Monto = decimal.Parse(monto, CultureInfo.InvariantCulture);
            nota.Serie = "12345678901234567890";
            nota.Numero = "123456789012345678901234567890";
            nota.FechaVencimiento = nota.FechaEmision;
            nota.ComprasMetValidar();
            Assert.AreEqual("Registro agregado correctamente.", nota.GrabarCambios());
            Assert.AreEqual(1, repo.Notas.Count);
            var guardada = repo.Notas[0];
            Assert.AreEqual(nota.IdProveedor, guardada.IdProveedor);
            Assert.AreEqual(nota.IdConceptoProveedor, guardada.IdConceptoProveedor);
            Assert.AreEqual(nota.Serie, guardada.Serie);
            Assert.AreEqual(nota.Numero, guardada.Numero);
            Assert.AreEqual(nota.FechaEmision, guardada.FechaEmision);
            Assert.AreEqual(nota.FechaVencimiento, guardada.FechaVencimiento);
            Assert.AreEqual(nota.IdMoneda, guardada.IdMoneda);
            Assert.AreEqual(nota.TipoCambio, guardada.TipoCambio);
            Assert.AreEqual(nota.Monto, guardada.Monto);
        }

        [TestMethod]
        public void Agregar_DatosValidos_RecortaEspaciosYGuarda()
        {
            var repo = new RepositorioPrueba();
            var nota = new ClsModeloNotaProveedor(repo);
            nota.ComprasMetAgregar(1, 2, " NC ", " 1001 ", new DateTime(2026, 11, 5), new DateTime(2026, 12, 5), 1, 7.65m, 1250.50m);
            Assert.AreEqual(1, repo.Notas.Count);
            Assert.AreEqual("NC", repo.Notas[0].Serie);
            Assert.AreEqual("1001", repo.Notas[0].Numero);
            Assert.AreEqual(1250.50m, repo.Notas[0].Monto);
        }

        [TestMethod]
        public void Agregar_MontoInvalido_NoGuarda()
        {
            var repo = new RepositorioPrueba();
            Assert.ThrowsException<ArgumentException>(() => new ClsModeloNotaProveedor(repo).ComprasMetAgregar(
                1, 2, "NC", "1001", new DateTime(2026, 11, 5), new DateTime(2026, 12, 5), 1, 1m, 0m));
            Assert.AreEqual(0, repo.Notas.Count);
        }

        [DataTestMethod]
        [DataRow(EstadoEntidad.Modified)]
        [DataRow(EstadoEntidad.Deleted)]
        [DataRow((EstadoEntidad)99)]
        public void GrabarCambios_EstadoNoPermitido_NoGuarda(EstadoEntidad estado)
        {
            var repo = new RepositorioPrueba();
            var nota = CrearNota(repo);
            nota.Estado = estado;
            Assert.ThrowsException<InvalidOperationException>(() => nota.GrabarCambios());
            Assert.AreEqual(0, repo.Notas.Count);
        }

        [TestMethod]
        public void Constructor_RepositorioNulo_Rechaza()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ClsModeloNotaProveedor(null));
        }

        [TestMethod]
        public void Consultas_DevuelvenDatosDelRepositorio()
        {
            var repo = new RepositorioPrueba();
            var nota = CrearNota(repo);
            Assert.AreSame(repo.Proveedores, nota.ComprasMetObtenerProveedores());
            Assert.AreSame(repo.Conceptos, nota.ComprasMetObtenerConceptos());
            Assert.AreSame(repo.Monedas, nota.ComprasMetObtenerMonedas());
            Assert.AreSame(repo.Historial, nota.ComprasMetConsultarNotas(15));
            Assert.AreEqual(15, repo.ProveedorConsultado);
            Assert.AreEqual("Proveedor de prueba", repo.Proveedores.Rows[0]["Nombre"]);
        }
    }

    // Doble de prueba: no conecta a ODBC ni reproduce reglas de negocio.
    internal sealed class RepositorioPrueba : IRepositorioNotasProveedor
    {
        internal readonly List<ClsNotaProveedor> Notas = new List<ClsNotaProveedor>();
        internal readonly DataTable Proveedores = Tabla("Proveedor de prueba");
        internal readonly DataTable Conceptos = Tabla("Nota de credito");
        internal readonly DataTable Monedas = Tabla("GTQ");
        internal readonly DataTable Historial = Tabla("NC-1001");
        internal int ProveedorConsultado;
        private static DataTable Tabla(string nombre)
        {
            var tabla = new DataTable();
            tabla.Columns.Add("Nombre", typeof(string));
            tabla.Rows.Add(nombre);
            return tabla;
        }
        public DataTable ComprasMetObtenerProveedores() { return Proveedores; }
        public DataTable ComprasMetObtenerConceptos() { return Conceptos; }
        public DataTable ComprasMetObtenerMonedas() { return Monedas; }
        public DataTable ComprasMetConsultarNotas(int idProveedor) { ProveedorConsultado = idProveedor; return Historial; }
        public void ComprasMetAgregar(ClsNotaProveedor nota) { Notas.Add(nota); }
    }
}
