import { Injectable } from '@angular/core';
import { WindowSensorRepositoryService } from '../../repositories/window-sensor-repository.service';
import WindowSensorsCreationModel from './models/WindowSensorsCreationModel';
import WindowSensorsCreatedModel from './models/WindowSensorsCreatedModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WindowSensorService {

  constructor(private readonly _repository: WindowSensorRepositoryService) { }

  public registerWindowSensor(
    credentials: WindowSensorsCreationModel
  ): Observable<WindowSensorsCreatedModel> {
    return this._repository.registerWindowSensors(credentials);
  }
}
