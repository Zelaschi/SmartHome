import { Injectable } from '@angular/core';
import { HomeMemberApiRepositoryService } from '../../repositories/home-member-api-repository.service';
import HomePermissionResponseModel from './models/HomePermissionResponseModel';
import { Observable } from 'rxjs';

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
    homePermissions: Array<string>,
    homeMemberId: string
  ): Observable<Array<string>> {
    console.log('homePermissions', homePermissions);
    console.log('homeMemberId', homeMemberId);
    return this._repository.updateHomePermissions(homePermissions, homeMemberId);
  }
}
