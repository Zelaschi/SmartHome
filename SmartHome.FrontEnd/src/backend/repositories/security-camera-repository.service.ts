import { Injectable } from '@angular/core';
import SecurityCameraCreationModel from '../services/SecurityCamera/models/SecurityCameraCreationModel';
import SecurityCameraCreatedModel from '../services/SecurityCamera/models/SecurityCameraCreatedModel';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import environmentLocal from '../../environments/environment.local';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SecurityCameraRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/securityCameras', http);
  }

  public registerSecurityCamera(
    credentials: SecurityCameraCreationModel
  ): Observable<SecurityCameraCreatedModel> {
    return this.post(credentials);
  }
}
