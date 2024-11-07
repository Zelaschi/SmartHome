import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable , tap } from 'rxjs';
import UserCredentialsModel from "./models/UserCredentialsModel";
import SessionCreatedModel from "./models/SessionCreatedModel";
import UserLoggedModel from "./models/UserLoggedModel";
import { SessionApiRepositoryService } from "../../repositories/session-api-repository.service";


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly _userLogged$: BehaviorSubject<UserLoggedModel | null> =
    new BehaviorSubject<UserLoggedModel | null>(null);

    get userLogged(): Observable<UserLoggedModel | null> {
      if (!this._userLogged$.value) {
        const token = localStorage.getItem("token");
  
        if (token) {
          const systemPermissions = JSON.parse(
            localStorage.getItem("systemPermissions") || "[]"
          );
          this._userLogged$.next({ token, systemPermissions });
        }
      }
  
      return this._userLogged$.asObservable();
    }
  
    constructor(private readonly _repository: SessionApiRepositoryService) {}
  
    public login(
      credentials: UserCredentialsModel
    ): Observable<SessionCreatedModel> {
      return this._repository.login(credentials).pipe(
        tap((response : SessionCreatedModel) => {
          localStorage.setItem("token", response.token);
          localStorage.setItem(
            "systemPermissions",
            JSON.stringify(response.systemPermissions)
          );
          this._userLogged$.next(response);
        })
      );
    }
 
}
