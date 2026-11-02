using System;
using System.Collections.Generic;
using CapaModelo_DisponibilidadDiariaBancos.Entidades;

namespace CapaModelo_DisponibilidadDiariaBancos.Repositorio
{
    public class ClsRepositorioDisponibilidad
    {
        public List<ClsDisponibilidad> BancosMetObtenerDisponibilidad(
            DateTime fechaCorte,
            string banco,
            string numeroCuenta,
            string moneda)
        {
            var datos = new List<ClsDisponibilidad>
            {
                new ClsDisponibilidad
                {
                    Banco          = "Industrial",
                    NumeroCuenta   = "001-123456-7",
                    TipoCuenta     = "Monetaria",
                    Moneda         = "GTQ",
                    FechaCorte     = fechaCorte,
                    SaldoInicial   = 250000.00m,
                    Ingresos       = 85000.00m,
                    Egresos        = 40000.00m,
                    Comprometido   = 30000.00m,
                    Restringido    = 5000.00m
                    // Disponible = 250000 + 85000 - 40000 - 30000 - 5000 = 260000.00
                },
                new ClsDisponibilidad
                {
                    Banco          = "Agromercantil",
                    NumeroCuenta   = "002-654321-8",
                    TipoCuenta     = "Monetaria",
                    Moneda         = "GTQ",
                    FechaCorte     = fechaCorte,
                    SaldoInicial   = 180000.00m,
                    Ingresos       = 45000.00m,
                    Egresos        = 22000.00m,
                    Comprometido   = 18000.00m,
                    Restringido    = 2000.00m
                    // Disponible = 180000 + 45000 - 22000 - 18000 - 2000 = 183000.00
                }
            };

            if (!string.IsNullOrWhiteSpace(banco) && banco != "Todos")
                datos = datos.FindAll(d => d.Banco.Equals(banco, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(numeroCuenta) && numeroCuenta != "Todas")
                datos = datos.FindAll(d => d.NumeroCuenta == numeroCuenta);

            if (!string.IsNullOrWhiteSpace(moneda) && moneda != "Todas")
                datos = datos.FindAll(d => d.Moneda == moneda);

            return datos;
        }

        public List<string> BancosMetObtenerBancos()
        {
            return new List<string>
            {
                "Todos",
                "Industrial",
                "Agromercantil"
            };
        }

        public List<string> BancosMetObtenerCuentas(string banco)
        {
            var cuentas = new List<string> { "Todas" };
            if (banco == "Industrial")
            {
                cuentas.Add("001-123456-7");
            }
            else if (banco == "Agromercantil")
            {
                cuentas.Add("002-654321-8");
            }
            else
            {
                cuentas.Add("001-123456-7");
                cuentas.Add("002-654321-8");
            }
            return cuentas;
        }

        public List<ClsCompromiso> BancosMetObtenerCompromisos(string numeroCuenta)
        {
            return new List<ClsCompromiso>
            {
                new ClsCompromiso
                {
                    Fecha = new DateTime(2026, 10, 15),
                    Concepto = "Pago a proveedor",
                    Beneficiario = "Cárnicos del Norte, S.A.",
                    Monto = 18000.00m
                },
                new ClsCompromiso
                {
                    Fecha = new DateTime(2026, 10, 16),
                    Concepto = "Transporte",
                    Beneficiario = "Transporte Rápido, S.A.",
                    Monto = 12000.00m
                }
            };
        }

        public List<ClsResumenCuenta> BancosMetObtenerResumenCuenta(string numeroCuenta)
        {
            return new List<ClsResumenCuenta>
            {
                new ClsResumenCuenta { Concepto = "Saldo al cierre", Monto = 295000.00m },
                new ClsResumenCuenta { Concepto = "Fondos no disponibles", Monto = 35000.00m },
                new ClsResumenCuenta { Concepto = "Disponible", Monto = 260000.00m }
            };
        }
    }
}
