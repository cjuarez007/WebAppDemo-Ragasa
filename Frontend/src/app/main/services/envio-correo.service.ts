import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EnvioCorreoService {  
  constructor() { }

  private http = inject(HttpClient);

  // La URL de tu Logic App obtenida de Postman
  private readonly logicAppUrl = 'https://prod-11.southcentralus.logic.azure.com:443/workflows/63f27ac94e2f42978b7ecabe01a678b6/triggers/When_an_HTTP_request_is_received/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2FWhen_an_HTTP_request_is_received%2Frun&sv=1.0&sig=ntaxtZGx4rOZvEGT9eTuNnexJPYzyPFZnmT352MpTqU';

  enviarNotificacionPdf(destinatario: string, nombreArchivo: string): Observable<any> {
    const body = {
      destinatario: destinatario,
      nombreArchivo: nombreArchivo
    };

    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.http.post(this.logicAppUrl, body, { headers });
  }
}
