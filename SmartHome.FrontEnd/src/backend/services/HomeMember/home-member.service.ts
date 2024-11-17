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
}
