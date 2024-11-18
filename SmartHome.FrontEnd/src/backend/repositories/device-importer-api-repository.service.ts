import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import environmentLocal from '../../environments/environment.local';
import { HttpClient } from '@angular/common/http';
import DeviceImportRequestModel from '../services/ImportDevice/models/DeviceImportRequestModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeviceImporterApiRepositoryService extends ApiRepository {

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/deviceImport', http);
  }
  
  public importDevices(
    requestModel: DeviceImportRequestModel
  ): Observable<string>{
    return this.post(requestModel);
  }
}
