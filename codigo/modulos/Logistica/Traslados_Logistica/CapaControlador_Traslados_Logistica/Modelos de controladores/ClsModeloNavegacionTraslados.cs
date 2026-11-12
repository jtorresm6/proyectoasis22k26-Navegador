using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_Traslados_Logistica.Contratos;
using CapaModelo_Traslados_Logistica.Entidades;

/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 * Administra carga, filtros y navegación de traslados.
 * ===================================================================
 */

namespace CapaControlador_Traslados_Logistica
{
    public partial class ClsModeloTraslado
    {
        public DataTable LogisticaMetCargarTraslados()
        {
            _TablaTrasladosCompleta = _RepositorioTraslado.LogisticaMetObtenerTraslados();
            if (_TablaTrasladosCompleta == null)
            {
                throw new DataException("El repositorio devolvió una lista de traslados nula.");
            }

            _TablaTraslados = _TablaTrasladosCompleta.Copy();
            _IndiceActual = _TablaTraslados.Rows.Count == 0 ? -1 : 0;
            return _TablaTraslados;
        }

        public DataTable LogisticaMetObtenerTrasladosParaReporte()
        {
            DataTable Traslados = _RepositorioTraslado.LogisticaMetObtenerTraslados();
            if (Traslados == null)
            {
                throw new DataException("El repositorio devolvió una lista de traslados nula.");
            }

            return Traslados;
        }

        public DataTable LogisticaMetFiltrarTraslados(
            string Codigo, DateTime? FechaSolicitud, string Estado)
        {
            DataTable Resultados = _TablaTrasladosCompleta.Clone();
            string CodigoBuscado = (Codigo ?? string.Empty).Trim();
            string EstadoBuscado = (Estado ?? string.Empty).Trim();

            foreach (DataRow Traslado in _TablaTrasladosCompleta.Rows)
            {
                if (CodigoBuscado.Length > 0 &&
                    Convert.ToString(Traslado["CodigoTraslado"]).IndexOf(
                        CodigoBuscado, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                if (FechaSolicitud.HasValue &&
                    (Traslado.IsNull("FechaSolicitud") ||
                    Convert.ToDateTime(Traslado["FechaSolicitud"]).Date != FechaSolicitud.Value.Date))
                {
                    continue;
                }

                if (EstadoBuscado.Length > 0 &&
                    !string.Equals(EstadoBuscado, "Todos", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(
                        Convert.ToString(Traslado["Estado"]), EstadoBuscado,
                        StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                Resultados.ImportRow(Traslado);
            }

            _TablaTraslados = Resultados;
            _IndiceActual = _TablaTraslados.Rows.Count == 0 ? -1 : 0;
            return _TablaTraslados;
        }

        public DataRow LogisticaMetIrAInicio()
        {
            return LogisticaMetObtenerFila(0);
        }

        public DataRow LogisticaMetIrAlAnterior()
        {
            return LogisticaMetObtenerFila(Math.Max(0, _IndiceActual - 1));
        }

        public DataRow LogisticaMetIrAlSiguiente()
        {
            return LogisticaMetObtenerFila(Math.Min(_TablaTraslados.Rows.Count - 1, _IndiceActual + 1));
        }

        public DataRow LogisticaMetIrAlFin()
        {
            return LogisticaMetObtenerFila(_TablaTraslados.Rows.Count - 1);
        }

        public DataRow LogisticaMetSeleccionarTraslado(DataRow Traslado)
        {
            if (Traslado == null)
            {
                throw new ArgumentNullException("Traslado");
            }

            int Indice = _TablaTraslados.Rows.IndexOf(Traslado);
            if (Indice < 0)
            {
                throw new ArgumentException("El traslado no pertenece a la lista cargada.", "Traslado");
            }

            _IndiceActual = Indice;
            return Traslado;
        }

        private DataRow LogisticaMetObtenerFila(int Indice)
        {
            if (_TablaTraslados.Rows.Count == 0)
            {
                _IndiceActual = -1;
                return null;
            }

            _IndiceActual = Indice;
            return _TablaTraslados.Rows[_IndiceActual];
        }

        private DataTable LogisticaMetCrearTablaVacia(params string[] Columnas)
        {
            var Tabla = new DataTable();
            foreach (string Columna in Columnas)
            {
                Tabla.Columns.Add(Columna);
            }

            return Tabla;
        }
    }
}