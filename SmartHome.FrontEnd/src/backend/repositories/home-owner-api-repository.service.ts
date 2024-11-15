import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import environmentLocal from '../../environments/environment.local';
import HomeOwnerCreationModel from '../services/HomeOwner/models/HomeOwnerCreationModel';
import HomeOwnerCreatedModel from '../services/HomeOwner/models/HomeOwnerCreatedModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HomeOwnerApiRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/homeOwners', http);
  }

  public registerHomeOwner(
    credentials: HomeOwnerCreationModel
  ): Observable<HomeOwnerCreatedModel> {
    return this.post(credentials);
  }
}
