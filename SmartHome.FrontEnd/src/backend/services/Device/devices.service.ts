import { Injectable } from '@angular/core';
import { DeviceApiRepositoryService } from '../../repositories/device-api-repository.service';
import { DeviceTypeApiRepositoryService } from '../../repositories/device-type-api-repository.service';
import DeviceCreationModel from './models/DeviceCreationModel';
import DeviceTypesModel from './models/DeviceTypesModel';
import { Observable } from 'rxjs';
import DeviceCreatedModel from './models/DeviceCreatedModel';

@Injectable({
  providedIn: 'root'
})
export class DevicesService {

  constructor(
    private readonly _devicerepository: DeviceApiRepositoryService,
    private readonly _devicetypesrepository: DeviceTypeApiRepositoryService
  ) { }

  public registerDevice(
    credentials: DeviceCreationModel
  ): Observable<DeviceCreatedModel> {
    return this._devicerepository.registerDevice(credentials);
  }

  public getDeviceTypes(): Observable<DeviceTypesModel> {
    return this._devicetypesrepository.getDeviceTypes();
  }
}
