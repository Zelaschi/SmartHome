import { Injectable } from '@angular/core';
import { SecurityCameraRepositoryService } from '../../repositories/security-camera-repository.service';
import SecurityCameraCreationModel from './models/SecurityCameraCreationModel';
import { Observable } from 'rxjs';
import SecurityCameraCreatedModel from './models/SecurityCameraCreatedModel';

@Injectable({
  providedIn: 'root'
})
export class SecurityCameraService {

  constructor(private readonly _repository: SecurityCameraRepositoryService) { }

  public registerSecurityCamera(
    credentials: SecurityCameraCreationModel
  ): Observable<SecurityCameraCreatedModel> {
    return this._repository.registerSecurityCamera(credentials);
  }
}
