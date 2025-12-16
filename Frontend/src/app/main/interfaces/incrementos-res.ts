export interface IncrementosRes {
    Nn:               number;
    Nombre:           string;
    IdPuesto:         string;
    Puesto:           string;
    Jefe:             Jefe;
    FechaIngreso:     Date;
    Sem12023:         number | null;
    Sem22023:         number | null;
    SueldoActual:     number;
    PorcSugerido:     number;
    SueldoSugerido:   number;
    PorcOtorgado:     number;
    PorcOtorgadoJefe: number;
    SueldoNuevo:      number;
    RolId:            number;
}

export enum Jefe {
    GerenteÁrea1Y2 = "GERENTE ÁREA 1 Y 2",
    JefeÁrea1 = "JEFE ÁREA 1",
    JefeÁrea2 = "JEFE ÁREA 2",
}
