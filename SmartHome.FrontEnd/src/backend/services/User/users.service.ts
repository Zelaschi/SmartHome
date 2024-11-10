import { Injectable } from '@angular/core';
import UserPaginatedResponse from './models/UserPaginatedResponse';
import { Observable } from 'rxjs';
import { UsersApiRepositoryService } from '../../repositories/users-api-repository.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(
    private readonly _userRepository: UsersApiRepositoryService
  ) { }

  public getAllUsers(
    pageNumber: number,
    pageSize: number
  ): Observable<UserPaginatedResponse>
  {
    return this._userRepository.getUsers(pageNumber, pageSize);
  }
}
