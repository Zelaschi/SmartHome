import { Injectable } from '@angular/core';
import { MeApiRepositoryService } from '../../repositories/me-api-repository.service';
import NotificationResponse from './models/NotificationResponse';
import { Observable, map } from 'rxjs';
import HomeCreatedModel from '../Home/models/HomeCreatedModel';

@Injectable({
  providedIn: 'root'
})
export class MeService {

  constructor(private readonly _repository: MeApiRepositoryService) { }

  public getNotifications(
  ): Observable<NotificationResponse> {
    return this._repository.getNotifications()
      .pipe(
        map(response => {
          console.log('Response from API:', response);
          return response;
        })
      );
  }

  public listAllHomesFromUser(
  ): Observable<Array<HomeCreatedModel>>{
    return this._repository.listAllHomesFromUser();
  }
}
