import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import HomeCreationModel from '../services/Home/models/HomeCreationModel';
import HomeCreatedModel from '../services/Home/models/HomeCreatedModel';
import environmentLocal from '../../environments/environment.local';
import { Observable } from 'rxjs';
import HomeMemberResponseModel from '../services/Home/models/HomeMemberResponseModel';
import HomeDeviceResponseModel from '../services/Home/models/HomeDeviceResponseModel';
import { User } from '../services/User/models/User';

@Injectable({
  providedIn: 'root'
})
export class HomeApiRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/homes', http);
  }

  public registerHome(
    credentials: HomeCreationModel
  ): Observable<HomeCreatedModel> {
    return this.post(credentials);
  }

  public getHomeMembers(
    homeId : string
  ): Observable<Array<HomeMemberResponseModel>> {
    return this.get<Array<HomeMemberResponseModel>>(`${homeId}/members`);
  }

  public addDeviceToHome(
    homeId : string,
    deviceId : string
  ): Observable<void> {
    console.log('deviceId', deviceId);
    return this.post<void>({ deviceId }, `${homeId}/homeDevices`,);
  }

  public getHomeDevices(
    homeId : string,
    room : string | null
  ): Observable<Array<HomeDeviceResponseModel>> {
    const query = room ? `room=${room}` : '';
    return this.get<Array<HomeDeviceResponseModel>>(`${homeId}/homeDevices`, query);
  }

  public UpdateHomeName(
    homeId: string,
    newName: string
  ) {
    const body = { newName }
    console.log('newName', newName);
    console.log('homeId', homeId);
    return this.patchById(`${homeId}/homeName`, body);
  }

  public UpdateHomeDeviceName(
    homeDeviceId: string,
    newName: string
  ) {
    const body = { newName }
    return this.patchById(`${homeDeviceId}/homeDeviceName`, body);
  }

  public UnRelatedHomeOwners(
    homeId: string,
  ): Observable<Array<User>>
  {
    return this.get<Array<User>>(`${homeId}/unRelatedHomeOwners`);
  }

  public AddHomeMemberToHome(
    homeId: string,
    userId: string,
  ): Observable<void>
  {
    return this.post<void>({ userId }, `${homeId}/members`);
  }
}
