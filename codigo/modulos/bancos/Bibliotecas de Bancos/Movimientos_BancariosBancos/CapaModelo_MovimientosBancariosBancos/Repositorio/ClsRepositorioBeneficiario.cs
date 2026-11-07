using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaModelo_MovimientosBancarios.Contratos;
using CapaModelo_MovimientosBancarios.Entidades;

namespace CapaModelo_MovimientosBancarios.Repositorio
{
    public class ClsRepositorioBeneficiario :
        ClsSentencias, IRepositorioBeneficiario
    {
        public IEnumerable<ClsBeneficiario> BancosMetObtenerPorTipo(string TipoBeneficiario)
        {
            string Consulta;
            TipoBeneficiario = (TipoBeneficiario ?? "").Trim().ToUpperInvariant();

            switch (TipoBeneficiario)
            {
                case "CLIENTE":
                    Consulta = @"SELECT idCliente AS ID, codigoCliente AS Codigo, nombreCliente AS Nombre FROM tblCliente WHERE activoCliente = 1 ORDER BY nombreCliente";
                    break;

                case "PROVEEDOR":
                    Consulta = @"SELECT idProveedor AS ID, nitProveedor AS Codigo, nombreProveedor AS Nombre FROM tblProveedor WHERE estadoProveedor = 1 ORDER BY nombreProveedor";
                    break;

                case "EMPLEADO":
                    Consulta = @"SELECT idEmpleado AS ID, codigoEmpleado AS Codigo, CONCAT(nombresEmpleado, ' ', apellidosEmpleado) AS Nombre FROM tblempleado WHERE is_active = 1 ORDER BY nombresEmpleado, apellidosEmpleado";
                    break;

                default:
                    throw new ArgumentException("Tipo de beneficiario no válido.");
            }

            DataTable Tabla = BancosMetEjecucionConsulta(Consulta, CommandType.Text);
            List<ClsBeneficiario> Beneficiarios = new List<ClsBeneficiario>();

            foreach (DataRow Fila in Tabla.Rows)
            {
                ClsBeneficiario Beneficiario = new ClsBeneficiario();
                Beneficiario.IdBeneficiario = Convert.ToInt64(Fila["ID"]);
                Beneficiario.CodigoBeneficiario = Convert.ToString(Fila["Codigo"]);
                Beneficiario.NombreBeneficiario = Convert.ToString(Fila["Nombre"]);
                Beneficiario.TipoBeneficiario = TipoBeneficiario;
                Beneficiarios.Add(Beneficiario);
            }

            return Beneficiarios;
        }

        public IEnumerable<ClsCuentaBancariaTercero> BancosMetObtenerCuentasPorBeneficiario(
            string TipoBeneficiario,
            long IdBeneficiario)
        {
            TipoBeneficiario = (TipoBeneficiario ?? "").Trim().ToUpperInvariant();

            string Columna;

            switch (TipoBeneficiario)
            {
                case "CLIENTE":
                    Columna = "idCliente";
                    break;

                case "PROVEEDOR":
                    Columna = "idProveedor";
                    break;

                case "EMPLEADO":
                    Columna = "idEmpleado";
                    break;

                default:
                    throw new ArgumentException("Tipo de beneficiario no válido.");
            }

            if (IdBeneficiario <= 0)
                throw new ArgumentException("Identificador de beneficiario no válido.");

            string Consulta = $@"
            SELECT
                c.idCuentaBancariaTercero AS ID,
                c.numeroCuentaBancariaTercero AS Numero,
                c.nombreTitularCuentaBancariaTercero AS Titular,
                b.nombreBanco AS Banco,
                m.codigoMoneda AS Moneda
            FROM tblCuentaBancariaTercero c
            INNER JOIN tblBanco b
                ON c.idBanco = b.idBanco
            INNER JOIN tblMoneda m
                ON c.idMoneda = m.idMoneda
            WHERE c.{Columna} = {IdBeneficiario}
              AND c.is_active = 1
              AND b.is_active = 1
              AND m.is_active = 1
            ORDER BY b.nombreBanco, c.numeroCuentaBancariaTercero;";

            DataTable Tabla = BancosMetEjecucionConsulta(
                Consulta,
                CommandType.Text
            );

            List<ClsCuentaBancariaTercero> Cuentas = new List<ClsCuentaBancariaTercero>();
            foreach (DataRow Fila in Tabla.Rows)
            {
                ClsCuentaBancariaTercero Cuenta = new ClsCuentaBancariaTercero();
                Cuenta.IdCuentaBancariaTercero = Convert.ToInt64(Fila["ID"]);
                Cuenta.NumeroCuentaBancariaTercero = Convert.ToString(Fila["Numero"]);
                Cuenta.NombreTitularCuentaBancariaTercero = Convert.ToString(Fila["Titular"]);
                Cuenta.NombreBanco = Convert.ToString(Fila["Banco"]);
                Cuenta.CodigoMoneda = Convert.ToString(Fila["Moneda"]);
                Cuentas.Add(Cuenta);
            }
            return Cuentas;
        }


        public IEnumerable<ClsBeneficiario> BancosMetObtenerTodos()
        {
            throw new NotSupportedException("Debe especificar el tipo de beneficiario.");
        }

        public int BancosMetAgregar(ClsBeneficiario Entidad)
        {
            throw new NotSupportedException("Este módulo no registra beneficiarios.");
        }

        public int BancosMetEditar(ClsBeneficiario Entidad)
        {
            throw new NotSupportedException("Este módulo no modifica beneficiarios.");
        }

        public int BancosMetRemover(ClsBeneficiario Entidad)
        {
            throw new NotSupportedException("Este módulo no elimina beneficiarios.");
        }
    }
}
