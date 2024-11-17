import { Injectable } from '@angular/core';
import { BusinessApiRepositoryService } from '../../repositories/business-api-repository.service';
import BusinessCreationModel from './models/BusinessCreationModel';
import BusinessCreatedModel from './models/BusinessCreatedModel';
import { Observable } from 'rxjs';
import BusinessPaginatedResponse from './models/BusinessPaginatedResponse';
import { ValidatorResponseModel } from './models/ValidatorResponseModel';

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

  public getBusinesses(
    pageNumber: number,
    pageSize: number,
    businessName: string | null,
    fullName: string | null
  ): Observable<BusinessPaginatedResponse> {
    return this._repository.getBusinesses(pageNumber, pageSize, businessName, fullName);
  }

  public getValidators(
  ): Observable<Array<ValidatorResponseModel>> {
    return this._repository.getValidators();
  }

  public addValidatorToBusiness(
    validatorId: string
  ): Observable<void> {
    return this._repository.addValidatorToBusiness(validatorId);
  }
}
