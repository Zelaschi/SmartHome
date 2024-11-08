import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import environmentLocal from '../../app/environments/environment.local';
import DeviceTypesModel from '../services/Device/models/DeviceTypesModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeviceTypeApiRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/deviceTypes', http);
   }
   public getDeviceTypes(): Observable<DeviceTypesModel>
   {
      return this.get();
   }
}
