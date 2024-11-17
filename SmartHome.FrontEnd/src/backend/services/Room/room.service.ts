import { Injectable } from '@angular/core';
import RoomCreationModel from './models/RoomCreationModel';
import RoomCreatedModel from './models/RoomCreatedModel';
import { RoomApiRepositoryService } from '../../repositories/room-api-repository.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoomService {

  constructor(private readonly _repository: RoomApiRepositoryService) { }

  public registerRoom(
    homeId : string,
    credentials: RoomCreationModel
  ): Observable<RoomCreatedModel> {
    return this._repository.registerRoom(homeId, credentials);
  }

  public getRooms(homeId: string): Observable<Array<RoomCreatedModel>> {
    return this._repository.getRooms(homeId);
  }
}