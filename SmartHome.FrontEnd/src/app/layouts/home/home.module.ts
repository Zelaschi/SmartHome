import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeFormComponent } from './home-form/home-form.component';
import { FormComponent } from "../../components/form/form/form.component";
import { FormInputComponent } from "../../components/form/form-input/form-input.component";
import { FormButtonComponent } from "../../components/form/form-button/form-button.component";
import { HomeRoutingModule } from './home-routing.module';



@NgModule({
  declarations: [
    HomeFormComponent,
  ],
  imports: [
    HomeRoutingModule,
    CommonModule,
    FormComponent,
    FormInputComponent,
    FormButtonComponent
]
})
export class HomeModule { }
