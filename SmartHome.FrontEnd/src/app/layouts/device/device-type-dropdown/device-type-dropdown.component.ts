import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import DeviceTypeStatus from './models/device-type.status';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';
import { DevicesService } from '../../../../backend/services/Device/devices.service';
import { DropdownComponent } from '../../../components/dropdown/dropdown.component';

@Component({
  selector: 'app-device-type-dropdown',
  standalone: true,
  templateUrl: './device-type-dropdown.component.html',
  styleUrl: './device-type-dropdown.component.css',
  imports: [DropdownComponent, CommonModule]
})
export class DeviceTypeDropdownComponent implements OnInit, OnDestroy{
  @Input() value : string | null = null;

  status: DeviceTypeStatus = {
    loading: true,
    deviceTypes: [],
  }

  private _deviceTypesSubscription: Subscription | null = null;
  
  constructor(private readonly _devicesService: DevicesService) {}

  ngOnDestroy(): void {
      this._deviceTypesSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this._deviceTypesSubscription = this._devicesService.getDeviceTypes().subscribe({
      next: (deviceTypes) => {
        this.status = {
          deviceTypes: deviceTypes.map((deviceType) => ({
            value: deviceType.type,
            label: deviceType.type
          })),
        };
      },
      error: (error) => {
        this.status = { 
          deviceTypes : [],
          error,
         };
      },
    });
  }

}
