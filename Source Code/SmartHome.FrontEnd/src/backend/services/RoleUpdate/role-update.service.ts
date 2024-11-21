import { Injectable } from '@angular/core';
import { AdminApiRepositoryService } from '../../repositories/admin-api-repository.service';
import { BusinessOwnerApiRepositoryService } from '../../repositories/business-owner-api-repository.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoleUpdateService {

  constructor(
    private readonly _adminRepository: AdminApiRepositoryService,
    private readonly _businessOwnerRepository: BusinessOwnerApiRepositoryService
  ) { }

  public addHomeOwnerPermissionsToAdmin()
  : Observable<string> {
    return this._adminRepository.addHomeOwnerPermissionsToAdmin();
  }

  public addHomeOwnerPermissionsToBusinessOwner()
  : Observable<string> {
    return this._businessOwnerRepository.addHomeOwnerPermissionsToBusinessOwner();
  }
}
