using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public partial class Resultado
{
    public int? Cia { get; set; }

    public string? Tipotrab { get; set; }

    public int Nomina { get; set; }

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

    public string? Porctab { get; set; }

    public string? Porcformula { get; set; }

    public decimal? Sueldonuevo { get; set; }

    public string? Vacio2 { get; set; }

    public decimal? Mediatab2 { get; set; }

    public decimal? Nvopra { get; set; }

    public decimal? Normppa { get; set; }

    public decimal? PpaAster { get; set; }

    public string? Tabulador { get; set; }

    public string? Posicion { get; set; }

    public string? Sintope { get; set; }

    public string? Contope { get; set; }

    public int Id { get; set; }

    public int? Jefe { get; set; }

    public int? SupJefe { get; set; }

    public decimal? Sem12023 { get; set; }

    public decimal? Sem22023 { get; set; }

    public decimal? Desempenio { get; set; }

    public decimal? PorcTabulador { get; set; }

    public int PosicionReal { get; set; }

    public decimal? PorcIncrementoSugerido { get; set; }

    public decimal? PorcentajeMinimo { get; set; }

    public decimal? PorcentajeMinimoJefe { get; set; }

    public string? JustificacionJefe { get; set; }

    public string? JustificacionSuperJefe { get; set; }
}
