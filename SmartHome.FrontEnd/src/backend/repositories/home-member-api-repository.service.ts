import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import environmentLocal from '../../environments/environment.local';
import ApiRepository from './api-repository';
import HomePermissionResponseModel from '../services/HomeMember/models/HomePermissionResponseModel';
import { Observable } from 'rxjs';

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

}
