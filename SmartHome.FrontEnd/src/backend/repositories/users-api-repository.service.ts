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
    pageSize: number,
    role: string | null,
    fullName: string | null
  ): Observable<UserPaginatedResponse> {
    const params = new URLSearchParams();

    params.append('pageNumber', pageNumber.toString());
    params.append('pageSize', pageSize.toString());

    if (role) params.append('role', role);
    if (fullName) params.append('fullName', fullName);

    const queryString = params.toString();
    const url = queryString ? `?${queryString}` : '';

    return this.get<UserPaginatedResponse>(url);
  }
}
