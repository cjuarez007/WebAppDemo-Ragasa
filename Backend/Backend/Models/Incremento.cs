using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Incremento
{
    public int Nn { get; set; }

    public string Nombre { get; set; } = null!;

    public string IdPuesto { get; set; } = null!;

    public string Puesto { get; set; } = null!;

    public string Jefe { get; set; } = null!;

    public DateOnly FechaIngreso { get; set; }

    public decimal? Sem12023 { get; set; }

    public decimal? Sem22023 { get; set; }

    public decimal SueldoActual { get; set; }

    public decimal PorcSugerido { get; set; }

    public decimal SueldoSugerido { get; set; }

    public decimal PorcOtorgado { get; set; }

    public decimal PorcOtorgadoJefe { get; set; }

    public decimal SueldoNuevo { get; set; }

    public int RolId { get; set; }
}
