import { Injectable } from '@angular/core';
import HomeCreationModel from './models/HomeCreationModel';
import HomeCreatedModel from './models/HomeCreatedModel';
import { HomeApiRepositoryService } from '../../repositories/home-api-repository.service';
import { Observable } from 'rxjs';
import HomeMemberResponseModel from './models/HomeMemberResponseModel';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  constructor(private readonly _repository: HomeApiRepositoryService) { }

  public registerHome(
    credentials: HomeCreationModel
  ): Observable<HomeCreatedModel> {
    return this._repository.registerHome(credentials);
  }

  public getHomeMembers(
    homeId : string
  ): Observable<Array<HomeMemberResponseModel>> {
    return this._repository.getHomeMembers(homeId);
  }
  
}
