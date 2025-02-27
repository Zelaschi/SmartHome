import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import environmentLocal from '../../environments/environment.local';
import { HttpClient } from '@angular/common/http';
import AdminCreationModel from '../services/Admin/models/AdminCreationModel';
import AdminCreatedModel from '../services/Admin/models/AdminCreatedModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminApiRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/admins', http);
  }
  public registerAdmin(
    credentials: AdminCreationModel
  ): Observable<AdminCreatedModel> {
    return this.post(credentials);
  }
  public deleteAdmin(
    id: string
  ): Observable<void> {
    return this.delete(id);
  }
  public addHomeOwnerPermissionsToAdmin()
  : Observable<string> {
    console.log('role');
    return this.patch("homeOwnerPermissions");
  }
}
