import { Injectable } from '@angular/core';
import { IntelligentLampRepositoryService } from '../../repositories/intelligent-lamp-repository.service';
import IntelligentLampCreationModel from './models/IntelligentLampCreationModel';
import IntelligentLampCreatedModel from './models/IntelligentLampCreatedModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class IntelligentLampService {

  constructor(private readonly _repository: IntelligentLampRepositoryService) { }

  public registerIntelligentLamp(
    credentials: IntelligentLampCreationModel
  ): Observable<IntelligentLampCreatedModel> {
    return this._repository.registerIntelligentLamp(credentials);
  }
}
