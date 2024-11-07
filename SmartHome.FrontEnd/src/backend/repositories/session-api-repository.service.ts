import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import environmentLocal from '../../app/environments/environment.local';
import SessionCreatedModel from '../services/Session/models/SessionCreatedModel';
import UserCredentialsModel from '../services/Session/models/UserCredentialsModel';
import { Observable } from 'rxjs';



@Injectable({
  providedIn: 'root'
})
export class SessionApiRepositoryService extends ApiRepository{
  constructor(http : HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/authentication', http);
   }

   public login(
    credentials: UserCredentialsModel
  ): Observable<SessionCreatedModel> {
    return this.post(credentials);
  }
}
