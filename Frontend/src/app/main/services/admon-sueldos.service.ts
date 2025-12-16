import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AdmonSueldo } from '../interfaces/admon-sueldos';

@Injectable({
  providedIn: 'root'
})
export class AdmonSueldosService {  

  constructor(private http: HttpClient) {}

  private headers = new HttpHeaders({
    "key":"Content-Type",
    "value":"application/json",
    "description":"",
    "type":"default",
    "enabled":"true"
  });

  public addAdmonSueldos(payload: AdmonSueldo[]): Observable<any> {
    return this.http.post('http://localhost:5094/api/Admon_Sueldos', payload, {headers:this.headers});
  }
}
