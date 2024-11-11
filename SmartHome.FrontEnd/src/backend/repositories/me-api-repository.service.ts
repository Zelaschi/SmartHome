import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import environmentLocal from '../../app/environments/environment.local';
import { HttpClient } from '@angular/common/http';
import NotificationResponse from '../services/Me/models/NotificationResponse';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MeApiRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/me', http);
  }

  public getNotifications(
  ): Observable<NotificationResponse> {
    return this.get<NotificationResponse>('notifications');
  }
}
