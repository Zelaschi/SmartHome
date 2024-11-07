import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import environmentLocal from '../../app/environments/environment.local';
import BusinessCreationModel from '../services/Business/models/BusinessCreationModel';
import BusinessCreatedModel from '../services/Business/models/BusinessCreatedModel';
import { Observable } from 'rxjs';
import ApiRepository from './api-repository';


@Injectable({
  providedIn: 'root'
})
export class BusinessApiRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/businesses', http);
  }
  public registerBusiness(
    credentials: BusinessCreationModel
  ): Observable<BusinessCreatedModel> {
    return this.post(credentials);
  }
}
