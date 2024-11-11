// device-list.component.ts
import { Component, Input, OnInit } from '@angular/core';
import DropdownOption from '../../../components/dropdown/models/DropdownOption';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Device } from '../../../../backend/services/Device/models/Device';
import { DevicesService } from '../../../../backend/services/Device/devices.service';
import { Subscription } from 'rxjs';
import DeviceStatus from './models/device.status';
import deviceFilters from './models/deviceFilters';

@Component({
  selector: 'app-device-list',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.css']
})
export class DeviceListComponent  {
  private _devicesSubscription: Subscription | null = null;
  pageNumber: number = 1;
  pageSize: number = 9;
  loading: boolean = false;

  filters: deviceFilters = {
    deviceName : null,
    modelNumber:  null,
    businessName:  null,
    type: null,
  }
  typeAux: string | null = null;
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
    this._devicesService.getAllDevices(this.pageNumber, this.pageSize, this.filters.deviceName, this.filters.modelNumber, this.filters.businessName, this.filters.type).subscribe({
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
  
  readonly formField: any = {
    deviceName: {
      name: 'deviceName'
    },
    modelNumber: {
      name: 'modelNumber'
    },
    businessName: {
      name: 'businessName'
    },
    type: {
      name: 'type'
    }
  };
  

  readonly deviceFilterForm = new FormGroup({
    [this.formField.deviceName.name]: new FormControl('', []),
    [this.formField.modelNumber.name]: new FormControl('', []),
    [this.formField.businessName.name]: new FormControl('', []),
    [this.formField.type.name]: new FormControl('', [])
  });

  onDeviceTypeChange(values: any){
    this.typeAux = values;
  }
  onSubmit(values: any) {
    console.log('Datos del dispositivo:', values);
    this.status.loading =  true;
    this.filters = {
      deviceName: values.deviceName,
      modelNumber: values.modelNumber,
      businessName: values.businessName,
      type: this.typeAux
    }
    this.loadProducts();
  }
}