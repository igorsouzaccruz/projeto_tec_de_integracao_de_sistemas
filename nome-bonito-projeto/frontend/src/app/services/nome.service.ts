import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class NomeService {
  private api1Url = ' http://localhost:8082/api1';

  constructor(private http: HttpClient) {}

  verificarNome(nome: string): Observable<void> {
    return this.http.post<void>(`${this.api1Url}/verificarNome`, { nome });
  }

  obterResultado(nome: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.api1Url}/resultado?nome=${nome}`);
  }
}
