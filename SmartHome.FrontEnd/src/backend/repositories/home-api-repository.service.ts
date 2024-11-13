import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import HomeCreationModel from '../services/Home/models/HomeCreationModel';
import HomeCreatedModel from '../services/Home/models/HomeCreatedModel';
import environmentLocal from '../../environments/environment.local';
import { Observable } from 'rxjs';
import HomeMemberResponseModel from '../services/Home/models/HomeMemberResponseModel';

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
}
