import { Injectable } from '@angular/core';
import { DeviceApiRepositoryService } from '../../repositories/device-api-repository.service';
import { DeviceTypeApiRepositoryService } from '../../repositories/device-type-api-repository.service';
import DeviceCreationModel from './models/DeviceCreationModel';
import DeviceTypeModel from './models/DeviceTypeModel';
import { Observable } from 'rxjs';
import DevicePaginatedResponse from './models/DevicePaginatedResponse';
import WindowSensorsCreationModel from '../WindowSensor/models/WindowSensorsCreationModel';
import WindowSensorsCreatedModel from '../WindowSensor/models/WindowSensorsCreatedModel';
import { WindowSensorRepositoryService } from '../../repositories/window-sensor-repository.service';
import MovementSensorCreationModel from '../MovementSensor/models/MovementSensorCreationModel';
import MovementSensorCreatedModel from '../MovementSensor/models/MovementSensorCreatedModel';
import { MovementSensorRepositoryService } from '../../repositories/movement-sensor-repository.service';
import IntelligentLampCreationModel from '../IntelligentLamp/models/IntelligentLampCreationModel';
import { IntelligentLampRepositoryService } from '../../repositories/intelligent-lamp-repository.service';
import IntelligentLampCreatedModel from '../IntelligentLamp/models/IntelligentLampCreatedModel';
import SecurityCameraCreationModel from '../SecurityCamera/models/SecurityCameraCreationModel';
import SecurityCameraCreatedModel from '../SecurityCamera/models/SecurityCameraCreatedModel';
import { SecurityCameraRepositoryService } from '../../repositories/security-camera-repository.service';

@Injectable({
  providedIn: 'root'
})
export class DevicesService {

  constructor(
    private readonly _devicerepository: DeviceApiRepositoryService,
    private readonly _devicetypesrepository: DeviceTypeApiRepositoryService,
    private readonly _windowsensorrepository: WindowSensorRepositoryService,
    private readonly _movementsensorrepository: MovementSensorRepositoryService,
    private readonly _intelligentlamprepository: IntelligentLampRepositoryService,
    private readonly _securitycamerarepository: SecurityCameraRepositoryService,
  ) { }

  private mapDeviceToSecurityCamera(credentials: DeviceCreationModel): SecurityCameraCreationModel {
    return {
      name: credentials.name,
      modelNumber: credentials.modelNumber,
      description: credentials.description,
      photos: credentials.photos,
      type: credentials.type,
      inDoor: credentials.indoor,
      outDoor: credentials.outdoor,
      personDetection: credentials.personDetection,
      movementDetection: credentials.movementDetection,
    };
  }

  public registerSecurityCamera(
    credentials : DeviceCreationModel
  ): Observable<SecurityCameraCreatedModel>
  {
    const securityCamera = this.mapDeviceToSecurityCamera(credentials);
    return this._securitycamerarepository.registerSecurityCamera(securityCamera);
  }
  
  private mapDeviceToIntelligentLamp(credentials: DeviceCreationModel): IntelligentLampCreationModel {
    return {
      name: credentials.name,
      modelNumber: credentials.modelNumber,
      description: credentials.description,
      photos: credentials.photos,
      type: credentials.type,
    };
  }

  public registerIntelligentLamp(
    credentials : DeviceCreationModel
  ): Observable<IntelligentLampCreatedModel>
  {
    const intelligentLamp = this.mapDeviceToIntelligentLamp(credentials);
    return this._intelligentlamprepository.registerIntelligentLamp(intelligentLamp);
  }

  private mapDeviceToMovementSensor(credentials: DeviceCreationModel): MovementSensorCreationModel {
    return {
      name: credentials.name,
      modelNumber: credentials.modelNumber,
      description: credentials.description,
      photos: credentials.photos,
      type: credentials.type,
    };
  }

  public registerMovementSensor(
    credentials : DeviceCreationModel
  ): Observable<MovementSensorCreatedModel>
  {
    const movementSensor = this.mapDeviceToMovementSensor(credentials);
    return this._movementsensorrepository.registerMovementSensor(movementSensor);
  }

  private mapDeviceToWindowSensor(credentials: DeviceCreationModel): WindowSensorsCreationModel {
    return {
      name: credentials.name,
      modelNumber: credentials.modelNumber,
      description: credentials.description,
      photos: credentials.photos,
      type: credentials.type,
    };
  }

  public registerWindowSensor(
    credentials : DeviceCreationModel
  ): Observable<WindowSensorsCreatedModel>
  {
    const windowSensor = this.mapDeviceToWindowSensor(credentials);
    return this._windowsensorrepository.registerWindowSensors(windowSensor);
  }

  public getDeviceTypes(): Observable<Array<DeviceTypeModel>> {
    return this._devicetypesrepository.getDeviceTypes();
  }
  
  public getAllDevices(
    pageNumber: number,
    pageSize: number,
    deviceName: string | null,
    modelNumber: string | null,
    businessName: string | null,
    type: string | null,
  ): Observable<DevicePaginatedResponse>
  {
    return this._devicerepository.getAllDevices(pageNumber, pageSize,deviceName, modelNumber, businessName, type);
  }
}
