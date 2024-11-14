import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import environmentLocal from '../../environments/environment.local';
import BusinessCreationModel from '../services/Business/models/BusinessCreationModel';
import BusinessCreatedModel from '../services/Business/models/BusinessCreatedModel';
import { Observable } from 'rxjs';
import ApiRepository from './api-repository';
import BusinessPaginatedResponse from '../services/Business/models/BusinessPaginatedResponse';


@Injectable({
  providedIn: 'root'
})
export class BusinessApiRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environmentLocal.SmartHome, 'api/v2/businesses', http);
  }
  public registerBusiness(
    credentials: BusinessCreationModel
  ): Observable<BusinessCreatedModel> {
    return this.post(credentials);
  }

  public getBusinesses(
    pageNumber: number,
    pageSize: number,
    businessName: string | null,
    fullName: string | null,
  ): Observable<BusinessPaginatedResponse> {
    const params = new URLSearchParams();

    params.append('pageNumber', pageNumber.toString());
    params.append('pageSize', pageSize.toString());

    if (businessName) params.append('businessName', businessName);
    if (fullName) params.append('fullName', fullName);

    const queryString = params.toString();
    const url = queryString ? `?${queryString}` : '';

    return this.get<BusinessPaginatedResponse>(url);
  }
}
