using System;
using System.Collections.Generic;

namespace Backend.Models
{
    public partial class AdmonSueldo
    {
        public int ID { get; set; } // PK auto-generada

        public int? Cia { get; set; }
        public string? Tipotrab { get; set; }
        public int? Nomina { get; set; }
        public string? Nombre { get; set; }
        public string? Puesto { get; set; }
        public string? Departamento { get; set; }
        public string? Segmento { get; set; }
        public DateTime? Fechaingreso { get; set; }
        public decimal? Sueldodiario { get; set; }
        public decimal? Sueldomensual { get; set; }
        public int? Nivelnum { get; set; }
        public string? Nivel { get; set; }
        public string? Tipotab { get; set; }
        public int? Antiguedad { get; set; }
        public string? Vacio { get; set; }
        public decimal? Mediatab { get; set; }
        public decimal? Pra { get; set; }
        public decimal? Ppa { get; set; }

        // Cambiados a decimal para coincidir con JSON
        public decimal? Porctab { get; set; }
        public string? Porcformula { get; set; }
        public decimal? Sueldonuevo { get; set; }
        public string? Vacio2 { get; set; }
        public decimal? Mediatab2 { get; set; }
        public decimal? Nvopra { get; set; }
        public decimal? Normppa { get; set; }
        public decimal? PpaAster { get; set; }
        public decimal? Tabulador { get; set; }
        public decimal? Posicion { get; set; }
        public decimal? Sintope { get; set; }
        public decimal? Contope { get; set; }
    }
}
