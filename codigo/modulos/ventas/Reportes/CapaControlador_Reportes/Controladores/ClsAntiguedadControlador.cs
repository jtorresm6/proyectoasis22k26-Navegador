// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using System;
using System.Data;
using CapaModelo_Reportes;

namespace CapaControlador_Reportes
{
    public sealed class ClsAntiguedadControlador
    {
        public DataTable VentasFuncClientes()
        {
            DataTable Clientes = new ClsAntiguedadDatos().VentasFuncClientes();
            DataRow Todos = Clientes.NewRow();
            Todos["idCliente"] = 0;
            Todos["Cliente"] = "Todos los clientes";
            Clientes.Rows.InsertAt(Todos, 0);
            return Clientes;
        }

        public DataTable VentasFuncConsultar(DateTime Corte, int Cliente, string Estado)
        {
            if (Cliente < 0)
                throw new ArgumentException("Seleccione un cliente valido.");
            if (Estado != "Todos" && Estado != "Vencidos" && Estado != "Vigentes")
                throw new ArgumentException("Seleccione un estado valido.");
            return VentasFuncClasificar(new ClsAntiguedadDatos().VentasFuncConsultar(Corte, Cliente), Corte, Estado);
        }

        public DataTable VentasFuncClasificar(DataTable Saldos, DateTime Corte, string Estado)
        {
            DataTable Resultado = new DataTable("DsAntiguedad");
            Resultado.Columns.Add("Cliente", typeof(string));
            Resultado.Columns.Add("Documento", typeof(string));
            Resultado.Columns.Add("Vencimiento", typeof(DateTime));
            Resultado.Columns.Add("DiasMora", typeof(int));
            foreach (string Campo in new[]
            {
                "Vigente",
                "Mora30",
                "Mora60",
                "Mora90",
                "Mayor90",
                "Total"
            }

            )
                Resultado.Columns.Add(Campo, typeof(decimal));
            foreach (DataRow Saldo in Saldos.Rows)
            {
                int Dias = Math.Max(0, (Corte.Date - Convert.ToDateTime(Saldo["Vencimiento"]).Date).Days);
                decimal Importe = decimal.Round(Convert.ToDecimal(Saldo["Saldo"]), 2);
                if (Importe <= 0 || (Estado == "Vencidos" && Dias == 0) || (Estado == "Vigentes" && Dias > 0))
                    continue;
                DataRow Fila = Resultado.NewRow();
                Fila["Cliente"] = Saldo["Cliente"];
                Fila["Documento"] = Saldo["Documento"];
                Fila["Vencimiento"] = Saldo["Vencimiento"];
                Fila["DiasMora"] = Dias;
                foreach (string Campo in new[]
                {
                    "Vigente",
                    "Mora30",
                    "Mora60",
                    "Mora90",
                    "Mayor90"
                }

                )
                    Fila[Campo] = 0m;
                string Rango = Dias == 0 ? "Vigente" : Dias <= 30 ? "Mora30" : Dias <= 60 ? "Mora60" : Dias <= 90 ? "Mora90" : "Mayor90";
                Fila[Rango] = Importe;
                Fila["Total"] = Importe;
                Resultado.Rows.Add(Fila);
            }

            return Resultado;
        }
    }
}
