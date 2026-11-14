/*
 * Autor: José Xavier Bolaños Tenas
 * Carné: 0901-23-2128
 * Descripción: Representa los datos de un tipo de movimiento de inventario.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Inventario_Logistica.Entidades
{
    public class ClsTipoMovimiento
    {
        public int Pk_TipoMovimiento { get; set; }
        public string NombreTipoMovimiento { get; set; }
        public string DescripcionTipoMovimiento { get; set; }
        public string SignoTipoMovimiento { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
