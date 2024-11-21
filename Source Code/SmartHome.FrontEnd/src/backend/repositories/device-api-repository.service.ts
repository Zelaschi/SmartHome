import { Injectable } from '@angular/core';
import environmentLocal from '../../environments/environment.local';
import { HttpClient } from '@angular/common/http';
import ApiRepository from './api-repository';
import { Observable } from 'rxjs';
import DeviceCreationModel from '../services/Device/models/DeviceCreationModel';
import DeviceCreatedModel from '../services/Device/models/DeviceCreatedModel';
import { Device } from '../services/Device/models/Device';
import DevicePaginatedResponse from '../services/Device/models/DevicePaginatedResponse';

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

  public getAllDevices(
    pageNumber: number,
    pageSize: number,
    deviceName: string | null,
    modelNumber: string | null,
    businessName: string | null,
    type: string | null,
  ): Observable<DevicePaginatedResponse> {
    const params = new URLSearchParams();
    
    // Add required parameters
    params.append('pageNumber', pageNumber.toString());
    params.append('pageSize', pageSize.toString());
    
    // Add optional parameters only if they are not null
    if (deviceName) params.append('deviceName', deviceName);
    if (modelNumber) params.append('deviceModel', modelNumber);
    if (businessName) params.append('businessName', businessName);
    if (type) params.append('deviceType', type);
  
    // Convert params to string and prepend with '?'
    const queryString = params.toString();
    const url = queryString ? `?${queryString}` : '';
    
    return this.get<DevicePaginatedResponse>(url);
  }

  public getAllDevicesFiltered(
    pageNumber: number,
    pageSize: number,
    deviceName: string | null,
    modelNumber: string | null,
    businessName: string | null,
    type: string | null,
  ):Observable<DevicePaginatedResponse>
  {
    return this.get<DevicePaginatedResponse>(`?pageNumber=${pageNumber}&pageSize=${pageSize}&deviceName=${deviceName}&modelNumber=${modelNumber}&businessName=${businessName}&type=${type}`);
  }
}
