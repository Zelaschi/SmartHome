import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BusinessOwnerFormComponent } from '../admin/business-owner-form/business-owner-form.component';
import { BusinessOwnerRoutingModule } from './business-owner-routing.module';
import { FormComponent } from '../../components/form/form/form.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { PaginationComponent } from "../../components/pagination/pagination.component";
import { DeviceItemComponent } from '../../business-components/device-item/device-item.component';
import { DeviceListComponent } from '../../business-components/device-list/device-list.component';
import { DeviceTypeDropdownComponent } from '../../business-components/device-type-dropdown/device-type-dropdown.component';
import { BusinessItemComponent } from '../admin/business-item/business-item.component';
import { DeviceFormComponent } from './device-form/device-form.component';
import { BusinessFormComponent } from './business-form/business-form.component';
import { ValidatorsDropDownComponent } from './validators-drop-down/validators-drop-down.component';
import { DropdownComponent } from '../../components/dropdown/dropdown.component';



@NgModule({
  declarations: [
    DeviceFormComponent,
    BusinessFormComponent,
    ValidatorsDropDownComponent,
  ],
  imports: [
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
