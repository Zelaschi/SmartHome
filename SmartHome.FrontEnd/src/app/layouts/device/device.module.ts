// src/app/layouts/device/device.module.ts
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeviceFormComponent } from './device-form/device-form.component';
import { DeviceRoutingModule } from './device-routing.module';
import { ReactiveFormsModule } from '@angular/forms';

// Importa los componentes reutilizables
import { FormComponent } from '../../components/form/form/form.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { DeviceListComponent } from './device-list/device-list.component';
import { DropdownComponent } from '../../components/dropdown/dropdown.component';
import { DeviceTypeDropdownComponent } from './device-type-dropdown/device-type-dropdown.component';
import { DeviceItemComponent } from './device-item/device-item.component';
import { PhotosCarouselComponent } from '../../components/photos-carousel/photos-carousel.component';
import { PaginationComponent } from '../../components/pagination/pagination.component';
import { FilterComponent } from '../../components/filter/filter/filter.component';


@NgModule({
  declarations: [
    DeviceFormComponent,
    DeviceListComponent,
    DeviceItemComponent,

  ],
  imports: [
    PhotosCarouselComponent,
    PaginationComponent,
    DeviceTypeDropdownComponent,
    CommonModule,
    ReactiveFormsModule,
    DeviceRoutingModule,
    FormComponent,
    FormButtonComponent,
    FormInputComponent,
    DropdownComponent,
    FilterComponent,
  ]
})
export class DeviceModule { }

