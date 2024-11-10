import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BusinessFormComponent } from './business-form/business-form.component';

import { FormComponent } from '../../components/form/form/form.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { BusinessRoutingModule } from './business-routing.module';
import { BusinessListComponent } from './business-list/business-list.component';
import { BusinessItemComponent } from './business-item/business-item.component';
import { PaginationComponent } from "../../components/pagination/pagination.component";


@NgModule({
  declarations: [
    BusinessFormComponent,
    BusinessListComponent,
    BusinessItemComponent
  ],
  imports: [
    BusinessRoutingModule,
    CommonModule,
    FormComponent,
    FormButtonComponent,
    FormInputComponent,
    PaginationComponent
]
})
export class BusinessModule { }
