import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import environmentLocal from '../../environments/environment.local';
import { Observable } from 'rxjs';
import UserPaginatedResponse from '../services/User/models/UserPaginatedResponse';
import ApiRepository from './api-repository';

@Injectable({
  providedIn: 'root'
})
export class UsersApiRepositoryService extends ApiRepository {

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/users', http);
  }

  public getUsers(
    pageNumber: number,
    pageSize: number
  ): Observable<UserPaginatedResponse> {
    return this.get<UserPaginatedResponse>(`?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }
}
