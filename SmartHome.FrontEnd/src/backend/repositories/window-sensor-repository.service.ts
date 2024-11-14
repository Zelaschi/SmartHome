import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import environmentLocal from '../../environments/environment.local';
import WindowSensorsCreationModel from '../services/WindowSensor/models/WindowSensorsCreationModel';
import WindowSensorsCreatedModel from '../services/WindowSensor/models/WindowSensorsCreatedModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WindowSensorRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/windowSensors', http);
  }

  public registerWindowSensors(
    credentials: WindowSensorsCreationModel
  ): Observable<WindowSensorsCreatedModel> {
    return this.post(credentials);
  }
}
