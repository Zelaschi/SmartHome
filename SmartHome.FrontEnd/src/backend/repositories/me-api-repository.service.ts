import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import environmentLocal from '../../environments/environment.local';
import { HttpClient } from '@angular/common/http';
import NotificationResponse from '../services/Me/models/NotificationResponse';
import { Observable, map } from 'rxjs';
import HomeCreatedModel from '../services/Home/models/HomeCreatedModel';
import { DeviceNotification } from '../services/Me/models/DeviceNotification';

@Injectable({
  providedIn: 'root'
})
export class MeApiRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/me', http);
  }

  public getNotifications(
  ): Observable<NotificationResponse> {
    return this.get<DeviceNotification[]>('notifications')
      .pipe(
        map(notifications => ({
          data: notifications
        }))
      );
  }

  public listAllHomesFromUser(
  ): Observable<Array<HomeCreatedModel>>{
    return this.get<Array<HomeCreatedModel>>('homes');
  }
}
