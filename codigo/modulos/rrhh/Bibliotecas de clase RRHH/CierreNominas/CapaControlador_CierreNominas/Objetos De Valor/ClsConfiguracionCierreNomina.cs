using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ==================================================================
 * Área: Recursos Humanos
 * Autores: Lourdes Isabel Melendez Pineda
 * Fecha o ultima edicion: 31/10/2026
 * ==================================================================
 * Propósito : Valores fijos que usa el cierre de nómina para armar y
 * enviar la póliza a Contabilidad: código del módulo contable,
 * país, prefijo del documento origen y la cuenta donde se
 * acredita el salario líquido.
 * ===================================================================
 */

namespace CapaControlador_CierreNominas.Objetos_de_valor
{
    public static class ClsConfiguracionCierreNomina
    {
        public const string CodigoModuloContable = "MOD-RRHH";
        public const int IdPais = 1;
        public const string PrefijoDocumentoOrigen = "NOM-";

        
        public const string CodigoCuentaSueldosPorPagar = "2.01.02.001";
    }
}