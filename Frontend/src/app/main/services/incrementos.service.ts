import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IncrementosRes } from '../interfaces/incrementos-res';

@Injectable({
  providedIn: 'root'
})
export class IncrementosService {

  constructor(private httpClient : HttpClient) { }

  public getAllIncrementos():Observable<IncrementosRes[]>{
    return this.httpClient.get<IncrementosRes[]>('http://localhost:5094/api/incrementos/all')
  }

  public getJefesIncrementos():Observable<IncrementosRes[]>{
    return this.httpClient.get<IncrementosRes[]>('http://localhost:5094/api/incrementos/jefes')
  }

  public getEmpleadosIncrementos():Observable<IncrementosRes[]>{
    return this.httpClient.get<IncrementosRes[]>('http://localhost:5094/api/incrementos/empleados')
  }

  public actualizarEmpleado(empleado: IncrementosRes) {
    return this.httpClient.put<IncrementosRes>(`http://localhost:5000/api/incrementos/updateIncremento/${empleado.Nn}`, empleado);
  }
  
}
