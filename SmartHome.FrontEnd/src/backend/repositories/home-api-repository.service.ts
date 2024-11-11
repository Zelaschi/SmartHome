import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import HomeCreationModel from '../services/Home/models/HomeCreationModel';
import HomeCreatedModel from '../services/Home/models/HomeCreatedModel';
import environmentLocal from '../../app/environments/environment.local';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HomeApiRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/homes', http);
  }

  public registerHome(
    credentials: HomeCreationModel
  ): Observable<HomeCreatedModel> {
    return this.post(credentials);
  }
}
