using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string? Password { get; set; }

    public string Nombres { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public string? Puesto { get; set; }

    public int NominaId { get; set; }

    public int RolId { get; set; }
}
