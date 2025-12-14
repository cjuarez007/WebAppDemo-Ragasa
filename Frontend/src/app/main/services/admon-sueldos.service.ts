import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdmonSueldosService {

  constructor(private httpClient : HttpClient ) { }

  public addAdmonSueldos(json : any[]):Observable<any[]>{
    return this.httpClient.post<any[]>(`http://localhost:5094/api/Admon_Sueldos`, json);
  }
  
}
