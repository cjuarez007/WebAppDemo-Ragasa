import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PorcentajesEstandar } from '../interfaces/porcentajes-estandar';

@Injectable({
  providedIn: 'root'
})
export class PorcentajesEstandarService {

  constructor(private httpClient: HttpClient) { }

    private headers = new HttpHeaders({
      "key":"Content-Type",
      "value":"application/json",
      "description":"",
      "type":"default",
      "enabled":"true"
    });
  
    public getPorcentajesEstandar():Observable<PorcentajesEstandar[]>{
      return this.httpClient.get<PorcentajesEstandar[]>('http://localhost:5094/api/Porcentajes_Estandar', {headers: this.headers});
    }

}
