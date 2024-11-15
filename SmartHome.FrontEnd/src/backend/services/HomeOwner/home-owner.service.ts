import { Injectable } from '@angular/core';
import { HomeOwnerApiRepositoryService } from '../../repositories/home-owner-api-repository.service';
import HomeOwnerCreationModel from './models/HomeOwnerCreationModel';
import HomeOwnerCreatedModel from './models/HomeOwnerCreatedModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HomeOwnerService {

  constructor(private readonly _repository: HomeOwnerApiRepositoryService) { }

  public registerHomeOwner(
    credentials: HomeOwnerCreationModel
  ): Observable<HomeOwnerCreatedModel> {
    return this._repository.registerHomeOwner(credentials);
  }
}
