import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import environmentLocal from '../../environments/environment.local';
import ApiRepository from './api-repository';
import HomePermissionResponseModel from '../services/HomeMember/models/HomePermissionResponseModel';
import { Observable } from 'rxjs';
import HomePermissionsRequest from '../services/HomeMember/models/HomePermissionsRequest';

@Injectable({
  providedIn: 'root'
})
export class HomeMemberApiRepositoryService extends ApiRepository {

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/homeMembers', http);
  }

  public ListAllHomePermissions(
  ):
  Observable<Array<HomePermissionResponseModel>> {
    return this.get<Array<HomePermissionResponseModel>>('homePermissions');
  }

  public addHomePermissions(
    homePermissions: Array<string>
  ): Observable<Array<string>> {
    return this.post<Array<string>>('permissions', JSON.stringify(homePermissions));
  }

  public updateHomePermissions(
    homePermissions: HomePermissionsRequest,
    homeMemberId: string
  ): Observable<void> {
    console.log('Request body:', homePermissions);
    
    return this.putById<void>(`${homeMemberId}/permissions`, homePermissions);
  }

}
