import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Device } from '../services/Device/Device';
import { SecurityCamera } from '../services/Device/SecurityCamera';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  constructor(private http: HttpClient) { }

  createMovementSensor(device : Device): Observable<Device> {
    return this.http.post<Device>('${Envoriment.api}/api/v2/movementSensor', device);
  }
  createWindowSensor(device : Device): Observable<Device> {
    return this.http.post<Device>('${Envoriment.api}/api/v2/windowSensor', device);
  }
  createInteligentLamp(device : Device): Observable<Device> {
    return this.http.post<Device>('${Envoriment.api}/api/v2/inteligentLamp', device);   
  }
  createSecurityCamera(camera : SecurityCamera): Observable<SecurityCamera> {
    return this.http.post<SecurityCamera>('${Envoriment.api}/api/v2/securityCamera', camera);
  }
}
