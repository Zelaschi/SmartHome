// device-list.component.ts
import { Component, Input, OnInit } from '@angular/core';
import DropdownOption from '../../../components/dropdown/models/DropdownOption';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Device } from '../../../../backend/services/Device/models/Device';
import { DevicesService } from '../../../../backend/services/Device/devices.service';
import { Subscription } from 'rxjs';
import DeviceStatus from './models/device.status';

@Component({
  selector: 'app-device-list',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.css']
})
export class DeviceListComponent  {
  private _devicesSubscription: Subscription | null = null;
  pageNumber: number = 1;
  pageSize: number = 10;
  loading: boolean = false;

  constructor(private readonly _devicesService: DevicesService) {}

  status: DeviceStatus = {
    loading: true,
    devices: [],
  }

  ngOnDestroy(): void {
    this._devicesSubscription?.unsubscribe();
  }
  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading = true;
    this._devicesService.getAllDevices(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.status ={
          devices: response.data,
        }
        console.log(this.status.devices);
        this.loading = false;
      },
      error: (error) => {
        console.error(error);
        this.loading = false;
      }
    });
  }

  onPageChange(page: number): void {
    this.pageNumber = page;
    this.loadProducts();
  }
  

}