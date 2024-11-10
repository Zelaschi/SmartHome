import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminFormComponent } from './admin-form/admin-form.component';
import { AdminRoutingModule } from './admin-routing.module';
import { FormComponent } from '../../components/form/form/form.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { UserListComponent } from './user-list/user-list.component';
import { UserItemComponent } from './user-item/user-item.component';
import { PaginationComponent } from "../../components/pagination/pagination.component";



@NgModule({
  declarations: [
    AdminFormComponent,
    UserListComponent,
    UserItemComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    FormComponent,
    FormButtonComponent,
    FormInputComponent,
    PaginationComponent
]
})
export class AdminModule { }
