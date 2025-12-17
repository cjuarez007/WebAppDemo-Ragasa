export interface IncrementosRes {
    Cia:           number;
    TipoTrab:      string;
    Nomina:        number;
    Nombre:        string;
    Puesto:        string;
    Departamento:  string;
    Segmento:      string;
    FechaIngreso:  Date;
    SueldoDiario:  number;
    SueldoMensual: number;
    SueldoNuevo: number;
    NivelNum:      number;
    Nivel:         string;
    Tipotab:       string;
    Antiguedad:    number;
    Jefe:          number;
    SupJefe:       number | null;
    Sem12023:       number;
    Sem22023:       number;
    Desempenio: number;
    PorcTabulador: number;
    PosicionReal: number;
    PorcIncrementoSugerido: number;
    porcentaje_minimo: number;
    porcentaje_minimo_jefe: number;
}
