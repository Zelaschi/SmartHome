import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import environmentLocal from '../../environments/environment.local';
import IntelligentLampCreationModel from '../services/IntelligentLamp/models/IntelligentLampCreationModel';
import IntelligentLampCreatedModel from '../services/IntelligentLamp/models/IntelligentLampCreatedModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class IntelligentLampRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/inteligentLamps', http);
  }

  public registerIntelligentLamp(
    credentials: IntelligentLampCreationModel
  ): Observable<IntelligentLampCreatedModel> {
    return this.post(credentials);
  }
}
