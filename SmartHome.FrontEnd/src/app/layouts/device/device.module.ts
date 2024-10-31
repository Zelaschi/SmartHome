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

@NgModule({
  declarations: [
    DeviceFormComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DeviceRoutingModule,
    FormComponent,
    FormButtonComponent,
    FormInputComponent,
  ]
})
export class DeviceModule { }

