import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BusinessFormComponent } from './business-form/business-form.component';

import { FormComponent } from '../../components/form/form/form.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { BusinessRoutingModule } from './business-routing.module';


@NgModule({
  declarations: [
    BusinessFormComponent
  ],
  imports: [
    BusinessRoutingModule,
    CommonModule,
    FormComponent,
    FormButtonComponent,
    FormInputComponent,
  ]
})
export class BusinessModule { }
