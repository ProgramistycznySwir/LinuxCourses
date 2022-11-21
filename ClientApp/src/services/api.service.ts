import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuthResponse } from 'src/models/auth-response';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private history: string[] = []

  constructor(
      private _router: Router,
      private _http: HttpClient,
      @Inject('API_URL') private API_URL: string,
    ) {
    }

  public login(username: string, password: string): Observable<AuthResponse> {
    
    const creadentials = {
      'username': username,
      'password': password
    }
    const result =
      this._http
        .post<AuthResponse>(`${this.API_URL}Auth/login`, creadentials)
        .pipe(tap(console.log))
    
    result.subscribe({next: res  => {
        const token = (<any>res).token
        localStorage.setItem("jwt", token)
        this._router.navigate(['/'])
      }, error: err => {
        alert("Nieudana próba logowania")
      }})
    return result;
  }
  public register(username: string, email: string, password: string): Observable<AuthResponse> {
    
    const creadentials = {
      'username': username,
      'email': email,
      'password': password
    }
    const result =
      this._http
        .post<AuthResponse>(`${this.API_URL}Auth/register`, creadentials)
        .pipe(tap(console.log))
    
    result.subscribe({next: res  => {
        const token = (<any>res).token
        localStorage.setItem("jwt", token)
        this._router.navigate(['/'])
      }, error: err => {
        alert("Nieudana próba logowania")
      }})
    return result;
  }

  public get client() {
    
  }
}
