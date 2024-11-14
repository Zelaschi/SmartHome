import { Injectable } from '@angular/core';
import { MovementSensorRepositoryService } from '../../repositories/movement-sensor-repository.service';
import MovementSensorCreationModel from './models/MovementSensorCreationModel';
import MovementSensorCreatedModel from './models/MovementSensorCreatedModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MovementSensorService {

  constructor(private readonly _repository: MovementSensorRepositoryService) { }

  public registerMovementSensor(
    credentials: MovementSensorCreationModel
  ): Observable<MovementSensorCreatedModel> {
    return this._repository.registerMovementSensor(credentials);
  }
}
