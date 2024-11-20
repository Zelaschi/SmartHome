import { Injectable } from '@angular/core';
import { HomeMemberApiRepositoryService } from '../../repositories/home-member-api-repository.service';
import HomePermissionResponseModel from './models/HomePermissionResponseModel';
import { Observable } from 'rxjs';
import HomePermissionsRequest from './models/HomePermissionsRequest';

@Injectable({
  providedIn: 'root'
})

export class HomeMemberService {

  constructor(private readonly _repository: HomeMemberApiRepositoryService) { }

  public listAllHomePermissions(
  ): Observable<Array<HomePermissionResponseModel>> {
    return this._repository.ListAllHomePermissions();
  }

  public addHomePermissions(
    homePermissions: Array<string>
  ): Observable<Array<string>> {
    return this._repository.addHomePermissions(homePermissions);
  }

  public updateHomePermissions(
    homePermissions: HomePermissionsRequest,
    homeMemberId: string
  ): Observable<void> {
    return this._repository.updateHomePermissions(homePermissions, homeMemberId);
  }
}
