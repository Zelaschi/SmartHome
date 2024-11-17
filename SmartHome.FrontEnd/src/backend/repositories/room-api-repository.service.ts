import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import RoomCreationModel from '../services/Room/models/RoomCreationModel';
import RoomCreatedModel from '../services/Room/models/RoomCreatedModel';
import environmentLocal from '../../environments/environment.local';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoomApiRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/rooms', http);
  }

  public registerRoom(
    homeId : string,
    credentials: RoomCreationModel
  ): Observable<RoomCreatedModel> {
    console.log(credentials);
    console.log(`${homeId}`);
    return this.post<RoomCreatedModel>(credentials, `${homeId}`);
  }

  public getRooms(homeId: string): Observable<Array<RoomCreatedModel>> {
    return this.get<Array<RoomCreatedModel>>(`${homeId}`);
  }
}
