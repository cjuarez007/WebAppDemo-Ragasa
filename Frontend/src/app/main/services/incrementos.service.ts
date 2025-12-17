import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IncrementosRes } from '../interfaces/incrementos-res';

@Injectable({
  providedIn: 'root'
})
export class IncrementosService {

  constructor(private httpClient : HttpClient) { }

  private headers = new HttpHeaders({
    "key":"Content-Type",
    "value":"application/json",
    "description":"",
    "type":"default",
    "enabled":"true"
  });


  public getAllIncrementos():Observable<IncrementosRes[]>{
    return this.httpClient.get<IncrementosRes[]>('http://localhost:5094/api/incrementos/all')
  }

  public getJefesIncrementos(nominaID:number):Observable<IncrementosRes[]>{
    return this.httpClient.get<IncrementosRes[]>(`http://localhost:5094/api/incrementos/jefes/${nominaID}`)
  }

  public getEmpleadosIncrementos(nominaID:number):Observable<IncrementosRes[]>{
    return this.httpClient.get<IncrementosRes[]>(`http://localhost:5094/api/incrementos/empleados/${nominaID}`)
  }

  public actualizarEmpleado(empleado: IncrementosRes) {
    return this.httpClient.put<IncrementosRes>(`http://localhost:5094/api/incrementos/updateIncremento/${empleado.Nomina}`, empleado, {headers:this.headers});
  }
  public getUserIncrementos(nominaID:number):Observable<IncrementosRes[]>{
    return this.httpClient.get<IncrementosRes[]>(`http://localhost:5094/api/incrementos/user/${nominaID}`)
  }
  
}
