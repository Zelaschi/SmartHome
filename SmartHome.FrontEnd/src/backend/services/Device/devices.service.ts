import { Injectable } from '@angular/core';
import { DeviceApiRepositoryService } from '../../repositories/device-api-repository.service';
import { DeviceTypeApiRepositoryService } from '../../repositories/device-type-api-repository.service';
import DeviceCreationModel from './models/DeviceCreationModel';
import DeviceTypeModel from './models/DeviceTypeModel';
import { Observable } from 'rxjs';
import DeviceCreatedModel from './models/DeviceCreatedModel';
import { Device } from './models/Device';
import DevicePaginatedResponse from './models/DevicePaginatedResponse';

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
