import { Injectable } from '@angular/core';
import environmentLocal from '../../app/environments/environment.local';
import { HttpClient } from '@angular/common/http';
import ApiRepository from './api-repository';
import { Observable } from 'rxjs';
import DeviceCreationModel from '../services/Device/models/DeviceCreationModel';
import DeviceCreatedModel from '../services/Device/models/DeviceCreatedModel';

@Injectable({
  providedIn: 'root'
})
export class DeviceApiRepositoryService extends ApiRepository {

  constructor(http: HttpClient) { 
    super(environmentLocal.SmartHome, 'api/v2/devices', http);
  }
  
  public registerDevice(
    credentials: DeviceCreationModel
  ): Observable<DeviceCreatedModel>
  {
    return this.post(credentials);
  }
}
