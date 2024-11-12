import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RoomFormComponent } from './room-form/room-form.component';
import { FormComponent } from "../../components/form/form/form.component";
import { FormInputComponent } from "../../components/form/form-input/form-input.component";
import { FormButtonComponent } from "../../components/form/form-button/form-button.component";
import { RoomRoutingModule } from './room-routing.module';



@NgModule({
  declarations: [
    RoomFormComponent
  ],
  imports: [
    RoomRoutingModule,
    CommonModule,
    FormComponent,
    FormInputComponent,
    FormButtonComponent
]
})
export class RoomModule { }
