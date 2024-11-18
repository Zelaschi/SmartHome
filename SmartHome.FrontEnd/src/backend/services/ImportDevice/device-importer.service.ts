import { Injectable } from '@angular/core';
import { DeviceImporterApiRepositoryService } from '../../repositories/device-importer-api-repository.service';
import { Observable } from 'rxjs';
import DeviceImportRequestModel from './models/DeviceImportRequestModel';

@Injectable({
  providedIn: 'root'
})
export class DeviceImporterService {

  constructor(private readonly _repository: DeviceImporterApiRepositoryService) { }

  public importDevices(
    requestModel: DeviceImportRequestModel
  ): Observable<string>{
    return this._repository.importDevices(requestModel);
  }
}
