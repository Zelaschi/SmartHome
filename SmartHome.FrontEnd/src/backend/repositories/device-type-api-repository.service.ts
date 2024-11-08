import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import environmentLocal from '../../app/environments/environment.local';
import DeviceTypeModel from '../services/Device/models/DeviceTypeModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeviceTypeApiRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/deviceTypes', http);
   }
   public getDeviceTypes(): Observable<Array<DeviceTypeModel>>
   {
      return this.get<Array<DeviceTypeModel>>();
   }
}
