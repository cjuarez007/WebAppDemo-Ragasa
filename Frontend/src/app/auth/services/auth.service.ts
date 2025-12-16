import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../interfaces/user';
import { Observable } from 'rxjs';
import { UserRes } from '../interfaces/user-res';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private httpClient : HttpClient) { }

  private headers = new HttpHeaders({
    "key":"Content-Type",
    "value":"application/json",
    "description":"",
    "type":"default",
    "enabled":"true"
  });

  public login(user : User):Observable<UserRes>{
    console.log(user)
    return this.httpClient.post<UserRes>('http://localhost:5094/api/UsuariosConreoller/login', user, {headers: this.headers})
  }
}
