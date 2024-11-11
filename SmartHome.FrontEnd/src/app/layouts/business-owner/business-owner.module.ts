import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BusinessOwnerFormComponent } from './business-owner-form/business-owner-form.component';
import { BusinessOwnerRoutingModule } from './business-owner-routing.module';
import { FormComponent } from '../../components/form/form/form.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { PaginationComponent } from "../../components/pagination/pagination.component";



@NgModule({
  declarations: [
    BusinessOwnerFormComponent
  ],
  imports: [
    CommonModule,
    BusinessOwnerRoutingModule,
    FormComponent,
    FormButtonComponent,
    FormInputComponent,
    PaginationComponent  
  ]
})
export class BusinessOwnerModule { }
