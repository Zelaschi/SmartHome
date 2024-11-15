import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import environmentLocal from '../../environments/environment.local';
import { HttpClient } from '@angular/common/http';
import BusinessOwnerCreationModel from '../services/BusinessOwner/models/BusinessOwnerCreationModel';
import BusinessOwnerCreatedModel from '../services/BusinessOwner/models/BusinessOwnerCreatedModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BusinessOwnerApiRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/businessOwners', http);
  }
  public registerBusinessOwner(
    credentials: BusinessOwnerCreationModel
  ): Observable<BusinessOwnerCreatedModel> {
    return this.post(credentials);
  }
  
  public addHomeOwnerPermissionsToBusinessOwner()
  : Observable<string> {
    console.log('role');
    return this.patch("homeOwnerPermissions");
  }
}
