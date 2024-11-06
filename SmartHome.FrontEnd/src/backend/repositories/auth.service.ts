import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable , tap } from 'rxjs';
import { Envoriment } from '../services/models/Enviroment';
import { AuthResponse } from '../services/models/AuthResponse';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient, private router: Router) { }

  login(email: string, password: string) : Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${Envoriment.api}/api/v2/authentication`, { email, password })
    .pipe(
      tap((response: AuthResponse) => {
        localStorage.setItem(Envoriment.auth, response.token);
      })
    );
  }

  autoLogin() : void {
    const token = localStorage.getItem(Envoriment.auth);
    if (!token) {
      return;
    }
  }

  logout() : void {
    localStorage.removeItem(Envoriment.auth);
    this.router.navigate(['/']);
  }
}
