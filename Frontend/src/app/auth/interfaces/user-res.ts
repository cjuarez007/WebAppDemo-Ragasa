export interface UserRes {
    success: boolean;
    usuario: Usuario;
}

export interface Usuario {
    UsuarioId:       number;
    Nombres:         string;
    ApellidoPaterno: string;
    ApellidoMaterno: string;
    Puesto:          string;
    NominaId:        number;
    RolId:           number;
}
