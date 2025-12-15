import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AdmonSueldo {
  cia: number | null;
  tipotrab: string | null;
  nomina: number | null;
  nombre: string | null;
  puesto: string | null;
  departamento: string | null;
  segmento: string | null;
  fechaingreso: string | null;
  sueldodiario: number | null;
  sueldomensual: number | null;
  nivelnum: number | null;
  nivel: string | null;
  tipotab: string | null;
  antiguedad: number | null;
  vacio: string | null;
  mediatab: number | null;
  pra: number | null;
  ppa: number | null;
  porctab: string | null;
  porcformula: string | null;
  sueldonuevo: number | null;
  vacio2: string | null;
  mediatab2: number | null;
  nvopra: number | null;
  normppa: number | null;
  ppa_aster: number | null;
  tabulador: string | null;
  posicion: string | null;
  sintope: string | null;
  contope: string | null;
}

@Injectable({
  providedIn: 'root'
})
export class AdmonSueldosService {

  private apiUrl = 'http://localhost:5094/api/Admon_Sueldos';

  constructor(private http: HttpClient) {}

  addAdmonSueldos(payload: { items: AdmonSueldo[] }): Observable<any> {
    return this.http.post(this.apiUrl, payload);
  }
}
