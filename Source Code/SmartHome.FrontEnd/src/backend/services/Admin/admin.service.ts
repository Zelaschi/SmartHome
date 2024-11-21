import { Injectable } from '@angular/core';
import { AdminApiRepositoryService } from '../../repositories/admin-api-repository.service';
import AdminCreationModel from './models/AdminCreationModel';
import AdminCreatedModel from './models/AdminCreatedModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private readonly _repository: AdminApiRepositoryService) { }

  public registerAdmin(
    credentials: AdminCreationModel
  ): Observable<AdminCreatedModel> {
    return this._repository.registerAdmin(credentials);
  }
  public deleteAdmin(
    id: string
  ): Observable<void> {
    return this._repository.deleteAdmin(id);
  }
}
