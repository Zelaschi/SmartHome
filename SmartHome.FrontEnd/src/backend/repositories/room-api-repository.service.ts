import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import RoomCreationModel from '../services/Room/models/RoomCreationModel';
import RoomCreatedModel from '../services/Room/models/RoomCreatedModel';
import environmentLocal from '../../app/environments/environment.local';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoomApiRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/rooms', http);
  }

  public registerRoom(
    credentials: RoomCreationModel
  ): Observable<RoomCreatedModel> {
    return this.post(credentials);
  }
}
