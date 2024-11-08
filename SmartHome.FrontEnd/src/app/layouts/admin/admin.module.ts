import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminFormComponent } from './admin-form/admin-form.component';
import { AdminRoutingModule } from './admin-routing.module';
import { FormComponent } from '../../components/form/form/form.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormInputComponent } from '../../components/form/form-input/form-input.component';



@NgModule({
  declarations: [
    AdminFormComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    FormComponent,
    FormButtonComponent,
    FormInputComponent,
  ]
})
export class AdminModule { }
