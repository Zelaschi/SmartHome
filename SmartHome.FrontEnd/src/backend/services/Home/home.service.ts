import { Injectable } from '@angular/core';
import HomeCreationModel from './models/HomeCreationModel';
import HomeCreatedModel from './models/HomeCreatedModel';
import { HomeApiRepositoryService } from '../../repositories/home-api-repository.service';
import { Observable } from 'rxjs';
import HomeMemberResponseModel from './models/HomeMemberResponseModel';
import HomeDeviceResponseModel from './models/HomeDeviceResponseModel';
import { User } from '../User/models/User';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  constructor(private readonly _repository: HomeApiRepositoryService) { }

  public registerHome(
    credentials: HomeCreationModel
  ): Observable<HomeCreatedModel> {
    return this._repository.registerHome(credentials);
  }

  public getHomeMembers(
    homeId : string
  ): Observable<Array<HomeMemberResponseModel>> {
    return this._repository.getHomeMembers(homeId);
  }
  
  public addDeviceToHome(
    homeId : string,
    deviceId : string
  ): Observable<void> {
    return this._repository.addDeviceToHome(homeId, deviceId);
  }

  public getHomeDevices(
    homeId : string,
    room : string | null
  ): Observable<Array<HomeDeviceResponseModel>> {
    return this._repository.getHomeDevices(homeId, room);
  }

  public UpdateHomeName(
    homeId: string,
    newName: string
  ){
    return this._repository.UpdateHomeName(homeId, newName);
  }

  public UpdateHomeDeviceName(
    homeDeviceId: string,
    newName: string
  ){
    return this._repository.UpdateHomeDeviceName(homeDeviceId, newName);
  }

  public UnRelatedHomeOwners(
    homeId: string,
  ): Observable<Array<User>>
  {
    return this._repository.UnRelatedHomeOwners(homeId);
  }

  public AddHomeMemberToHome(
    homeId: string,
    userId: string,
  ): Observable<void>
  {
    return this._repository.AddHomeMemberToHome(homeId, userId);
  }

  public TurnOnOffDevice(
    homeDeviceId: string,
  ): Observable<void>
  {
    return this._repository.TurnOnOffDevice(homeDeviceId);
  }

  public GetHomeByHomeId(
    homeId: string,
  ): Observable<HomeCreatedModel>
  {
    return this._repository.GetHomeByHomeId(homeId);
  }
}
