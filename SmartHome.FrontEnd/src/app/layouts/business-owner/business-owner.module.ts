import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BusinessOwnerRoutingModule } from './business-owner-routing.module';
import { FormComponent } from '../../components/form/form/form.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { DeviceItemComponent } from '../../business-components/device-item/device-item.component';
import { DeviceTypeDropdownComponent } from '../../business-components/device-type-dropdown/device-type-dropdown.component';
import { DeviceFormComponent } from './device-form/device-form.component';
import { BusinessFormComponent } from './business-form/business-form.component';
import { ValidatorsDropDownComponent } from './validators-drop-down/validators-drop-down.component';
import { DropdownComponent } from '../../components/dropdown/dropdown.component';
import { ValidatorFormComponent } from './validator-form/validator-form.component';
import { ButtonComponent } from '../../components/button/button.component';
import { DeviceImporterFormComponent } from './device-importer-form/device-importer-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MyBusinessItemComponent } from './my-business-item/my-business-item.component';
import { LoadingComponent } from '../../components/loading/loading.component';



@NgModule({
  declarations: [
    DeviceFormComponent,
    BusinessFormComponent,
    ValidatorsDropDownComponent,
    ValidatorFormComponent,
    DeviceImporterFormComponent,
    MyBusinessItemComponent,
  ],
  imports: [
    LoadingComponent,
    ReactiveFormsModule,
    ButtonComponent,
    CommonModule,
    BusinessOwnerRoutingModule,
    FormComponent,
    FormButtonComponent,
    FormInputComponent,
    DeviceItemComponent,
    DeviceTypeDropdownComponent,
    DropdownComponent
  ]
})
export class BusinessOwnerModule { }
