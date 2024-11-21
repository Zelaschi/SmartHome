// device-list.component.ts
import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Device } from '../../../backend/services/Device/models/Device';
import { DevicesService } from '../../../backend/services/Device/devices.service';
import { Subscription } from 'rxjs';
import DeviceStatus from './models/device.status';
import deviceFilters from './models/deviceFilters';
import { HomeService } from '../../../backend/services/Home/home.service';
import { PaginationComponent } from '../../components/pagination/pagination.component';
import { DeviceItemComponent } from '../device-item/device-item.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { DeviceTypeDropdownComponent } from '../device-type-dropdown/device-type-dropdown.component';
import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { FilterComponent } from '../../components/filter/filter/filter.component';
import { CommonModule } from '@angular/common';
import { LoadingComponent } from '../../components/loading/loading.component';

@Component({
  standalone: true,
  imports: [LoadingComponent, PaginationComponent, DeviceItemComponent, FormButtonComponent, DeviceTypeDropdownComponent, FormInputComponent, FilterComponent, CommonModule],
  selector: 'app-device-list',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.css']
})
export class DeviceListComponent  {
  @Input() homeId: string | null = null;
  @Input() isAddingDevice: boolean = false;
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
  constructor(private readonly _devicesService: DevicesService
    ,private readonly _homeService: HomeService
  ) {}

  status: DeviceStatus = {
    moreDevices: false,
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
        var moreDevices = response.data.length === this.pageSize;
        if(moreDevices){
          this.status ={
            devices: response.data,
            moreDevices: true,
          }
        }
        else{
          this.status ={
            devices: response.data,
            moreDevices: false,
          }
        }
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
    this.status.loading =  true;
    this.filters = {
      deviceName: values.deviceName,
      modelNumber: values.modelNumber,
      businessName: values.businessName,
      type: this.typeAux
    }
    this.loadProducts();
  }

  onDeviceAdded(device: Device): void {
    if (this.homeId && device.id) {
      this._homeService.addDeviceToHome(this.homeId, device.id).subscribe({
        next: () => {
          console.log(`Device ${device.name} added to home ${this.homeId} successfully.`);
        },
        error: (error: any) => {
          console.error(`Failed to add device ${device.name} to home:`, error);
        }
      });
    } else {
      console.warn('Home ID or Device ID is missing');
    }
  }
}