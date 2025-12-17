using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Empleado
{
    public int Nomina { get; set; }

    public string? Nombre { get; set; }

    public string? Tipotrab { get; set; }

    public int? Cia { get; set; }

    public string? Puesto { get; set; }

    public string? Departamento { get; set; }

    public string? Segmento { get; set; }

    public DateTime? Fechaingreso { get; set; }

    public int? Antiguedad { get; set; }

    public string? Nivel { get; set; }

    public int? Nivelnum { get; set; }

    public int? Jefe { get; set; }

    public int? SupJefe { get; set; }
}
