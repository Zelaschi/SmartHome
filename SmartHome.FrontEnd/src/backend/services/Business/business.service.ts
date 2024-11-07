import { Injectable } from '@angular/core';
import { BusinessApiRepositoryService } from '../../repositories/business-api-repository.service';
import BusinessCreationModel from './models/BusinessCreationModel';
import BusinessCreatedModel from './models/BusinessCreatedModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BusinessService {

  constructor(private readonly _repository: BusinessApiRepositoryService) { }

  public registerBusiness(
    credentials: BusinessCreationModel
  ): Observable<BusinessCreatedModel> {
    return this._repository.registerBusiness(credentials);
  }
}
