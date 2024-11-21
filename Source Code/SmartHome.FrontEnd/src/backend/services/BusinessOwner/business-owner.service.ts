import { Injectable } from '@angular/core';
import { BusinessOwnerApiRepositoryService } from '../../repositories/business-owner-api-repository.service';
import BusinessOwnerCreationModel from './models/BusinessOwnerCreationModel';
import BusinessOwnerCreatedModel from './models/BusinessOwnerCreatedModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BusinessOwnerService {

  constructor(private readonly _repository: BusinessOwnerApiRepositoryService) { }

  public registerBusinessOwner(
    credentials: BusinessOwnerCreationModel
  ): Observable<BusinessOwnerCreatedModel> {
    return this._repository.registerBusinessOwner(credentials);
  }
}
