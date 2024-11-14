import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import environmentLocal from '../../environments/environment.local';
import MovementSensorCreationModel from '../services/MovementSensor/models/MovementSensorCreationModel';
import MovementSensorCreatedModel from '../services/MovementSensor/models/MovementSensorCreatedModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MovementSensorRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/movementSensors', http);
  }

  public registerMovementSensor(
    credentials: MovementSensorCreationModel
  ): Observable<MovementSensorCreatedModel> {
    return this.post(credentials);
  }
}
